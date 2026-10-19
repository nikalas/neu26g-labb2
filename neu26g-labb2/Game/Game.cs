using neu26g_labb2.Models;

namespace neu26g_labb2.Game
{
    public class Game : IGameContext
    {
        private readonly LevelData levelData = new();
        private readonly Player player;
        private readonly HashSet<(int X, int Y)> discovered = [];

        private const int ViewDistance = 5;
        private const int StatusOffset = 2;

        private bool gameOver;
        private string statusMessage = string.Empty;

        public event Action<IGameContext>? Tick;

        public Player Player => player;
        public IReadOnlyList<LevelElement> Elements => levelData.Elements;

        public Game() : this(ResolveDefaultLevelPath()) { }

        public Game(string mapFilePath)
        {
            levelData.Load(mapFilePath);
            player = levelData.Elements.OfType<Player>().FirstOrDefault()
                ?? throw new InvalidOperationException("Player not found in level data.");

            Tick += player.OnTick;

            foreach (Enemy enemy in levelData.Elements.OfType<Enemy>())
            {
                Tick += enemy.OnTick;
            }
        }

        public void Run()
        {
            Console.CursorVisible = false;

            try
            {
                UpdateExploration();
                DrawScreen();

                while (!gameOver)
                {
                    if (!TryReadPlayerAction())
                    {
                        break;
                    }

                    TriggerTick();
                    UpdateExploration();
                    DrawScreen();

                    if (player.IsDead)
                    {
                        gameOver = true;
                        ShowEndScreen("You died. Press any key to exit.");
                    }
                    else if (!EnemiesRemaining())
                    {
                        gameOver = true;
                        ShowEndScreen("You cleared the dungeon! Press any key to exit.");
                    }
                }
            }
            finally
            {
                Console.CursorVisible = true;
                Console.ResetColor();
            }
        }

        public LevelElement? ElementAt(int x, int y)
        {
            return levelData.ElementAt(x, y);
        }

        public bool TryMove(Actor actor, int x, int y)
        {
            if (actor.IsDead)
            {
                return false;
            }

            if (x < 0 || y < 0 || x >= levelData.Width || y >= levelData.Height)
            {
                return false;
            }

            LevelElement? target = ElementAt(x, y);

            if (target is Wall)
            {
                return false;
            }

            if (target is Actor other)
            {
                if (other == actor)
                {
                    return false;
                }

                bool isPlayerVersusEnemy =
                    (actor is Player && other is Enemy) ||
                    (actor is Enemy && other is Player);

                if (isPlayerVersusEnemy)
                {
                    ResolveAttack(actor, other);
                }

                return false;
            }

            if (target is not null)
            {
                return false;
            }

            actor.X = x;
            actor.Y = y;
            return true;
        }

        public void ResolveAttack(Actor attacker, Actor defender)
        {
            if (attacker.IsDead || defender.IsDead)
            {
                return;
            }

            int applied = defender.TakeDamage(RollDamage(attacker, defender));
            statusMessage = $"{attacker.Name} hits {defender.Name} for {applied} damage.";

            if (defender.IsDead)
            {
                statusMessage += $" {defender.Name} dies.";
                return;
            }

            applied = attacker.TakeDamage(RollDamage(defender, attacker));
            statusMessage += $" {defender.Name} counters for {applied} damage.";

            if (attacker.IsDead)
            {
                statusMessage += $" {attacker.Name} dies.";
            }
        }

        public void Remove(LevelElement element)
        {
            levelData.Remove(element);
        }

        private static int RollDamage(Actor attacker, Actor defender)
        {
            return Math.Max(0, attacker.AttackDice.Throw() - defender.DefenceDice.Throw());
        }

        private static string ResolveDefaultLevelPath()
        {
            string[] candidates =
            [
                Path.Combine(AppContext.BaseDirectory, "level1.txt"),
                Path.Combine(Directory.GetCurrentDirectory(), "level1.txt"),
                Path.Combine(Directory.GetCurrentDirectory(), "neu26g-labb2", "level1.txt")
            ];

            return candidates.FirstOrDefault(File.Exists) ?? candidates[0];
        }

        private void TriggerTick()
        {
            Tick?.Invoke(this);
            RemoveDeadEnemies();
        }

        private bool TryReadPlayerAction()
        {
            while (true)
            {
                ConsoleKey key = Console.ReadKey(intercept: true).Key;

                if (key is ConsoleKey.Escape or ConsoleKey.Q)
                {
                    return false;
                }

                Direction? direction = key switch
                {
                    ConsoleKey.UpArrow or ConsoleKey.W => Direction.Up,
                    ConsoleKey.DownArrow or ConsoleKey.S => Direction.Down,
                    ConsoleKey.LeftArrow or ConsoleKey.A => Direction.Left,
                    ConsoleKey.RightArrow or ConsoleKey.D => Direction.Right,
                    _ => null
                };

                if (direction is null)
                {
                    continue;
                }

                player.PendingDirection = direction;
                return true;
            }
        }

        private bool EnemiesRemaining()
        {
            return levelData.Elements.OfType<Enemy>().Any(enemy => !enemy.IsDead);
        }

        private void RemoveDeadEnemies()
        {
            foreach (Enemy enemy in levelData.Elements.OfType<Enemy>().Where(enemy => enemy.IsDead).ToList())
            {
                levelData.Remove(enemy);
            }
        }

        private void UpdateExploration()
        {
            for (int x = player.X - ViewDistance; x <= player.X + ViewDistance; x++)
            {
                for (int y = player.Y - ViewDistance; y <= player.Y + ViewDistance; y++)
                {
                    int dx = x - player.X;
                    int dy = y - player.Y;

                    if (dx * dx + dy * dy <= ViewDistance * ViewDistance)
                    {
                        discovered.Add((x, y));
                    }
                }
            }
        }

        private void DrawScreen()
        {
            Console.Clear();

            foreach (LevelElement element in levelData.Elements)
            {
                switch (element)
                {
                    case neu26g_labb2.Models.Player:
                        element.Draw();
                        break;
                    case Wall when discovered.Contains((element.X, element.Y)):
                        element.Draw();
                        break;
                    case Enemy enemy when !enemy.IsDead && IsWithinViewDistance(enemy):
                        element.Draw();
                        break;
                }
            }

            DrawHud();
        }

        private void DrawHud()
        {
            int row = HudRow;

            if (row >= ConsoleHeight)
            {
                return;
            }

            Console.SetCursorPosition(0, row);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"HP: {player.HP}/{player.MaxHP}  Enemies left: {levelData.Elements.OfType<Enemy>().Count(enemy => !enemy.IsDead)}");

            if (statusMessage.Length > 0 && row + 1 < ConsoleHeight)
            {
                Console.SetCursorPosition(0, row + 1);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write(statusMessage);
            }

            Console.ResetColor();
        }

        private void ShowEndScreen(string message)
        {
            int row = HudRow + 1;

            if (row < ConsoleHeight)
            {
                Console.SetCursorPosition(0, row);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(message);
            }

            Console.ResetColor();
            Console.ReadKey(intercept: true);
        }

        private static int ConsoleHeight => Console.WindowHeight > 0 ? Console.WindowHeight : 25;

        private int HudRow => levelData.Elements.Count == 0
            ? 0
            : levelData.Elements.Max(element => element.Y) + StatusOffset;

        private bool IsWithinViewDistance(LevelElement element)
        {
            int dx = element.X - player.X;
            int dy = element.Y - player.Y;
            return dx * dx + dy * dy <= ViewDistance * ViewDistance;
        }
    }
}
