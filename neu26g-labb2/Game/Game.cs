using neu26g_labb2.Models;
using System.Data;

namespace neu26g_labb2.Game
{
    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    internal class Game
    {

        private readonly LevelData levelData = new();
        private readonly Player? player;

        public Game()
        {
            levelData.Load("../../../level1.txt");
            player = levelData.Elements.OfType<Player>().FirstOrDefault() ?? throw new Exception("Player not found in level data.");
        }

        public void Run()
        {
            while (true)
            {
                Update();

                DrawScreen();

                ReadInput();
            }
        }

         private void Update()
        {
            var enemies = levelData.Elements.OfType<Enemy>();
            foreach (var enemy in enemies)
            {
                enemy.Update();
            }
            // player.Update();
        }

        static private void ExitGame()
        {
            Console.WriteLine();
            Console.WriteLine("Exiting the game...");
            Environment.Exit(0);
        }

        private void ReadInput()
        {

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.Escape:
                case ConsoleKey.Q:
                    ExitGame();
                    break;
                case ConsoleKey.UpArrow:
                    PlayerAction(Direction.Up);
                    break;
                case ConsoleKey.DownArrow:
                    PlayerAction(Direction.Down);
                    break;
                case ConsoleKey.LeftArrow:
                    PlayerAction(Direction.Left);
                    break;
                case ConsoleKey.RightArrow:
                    PlayerAction(Direction.Right);
                    break;
            }
        }

        private void PlayerAction(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    MovePlayer(Direction.Up);
                    break;
                case Direction.Down:
                    MovePlayer(Direction.Down);
                    break;
                case Direction.Left:
                    MovePlayer(Direction.Left);
                    break;
                case Direction.Right:
                    MovePlayer(Direction.Right);
                    break;
            }
        }

        private void MovePlayer(Direction direction)
        {
            int newX = player.X;
            int newY = player.Y;
            switch (direction)
            {
                case Direction.Up:
                    newY--;
                    break;
                case Direction.Down:
                    newY++;
                    break;
                case Direction.Left:
                    newX--;
                    break;
                case Direction.Right:
                    newX++;
                    break;
            }

            var collidingElement = CollidesWithElement(newX, newY);
            if (collidingElement is Wall)
            {
                // Player cannot move into a wall
                return;
            }   
            if (collidingElement is Enemy)
            {
                // Player collides with an enemy
                Console.WriteLine("You have been caught by an enemy!");
                Console.ReadKey();
            }
            if (collidingElement is null)
            {
                // Player moves to an empty space
                player.X = newX;
                player.Y = newY;
            }
        }

        private LevelElement? CollidesWithElement(int x, int y)
        {
            return levelData.Elements.FirstOrDefault(e => e.X == x && e.Y == y);
        }

        private void DrawScreen()
        {
            Console.Clear();
            foreach (var element in levelData.Elements)
            {
                element.Draw();
            }
        }

}

}