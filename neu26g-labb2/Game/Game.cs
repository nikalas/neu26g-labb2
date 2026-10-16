using neu26g_labb2.Models;

namespace neu26g_labb2.Game
{
    internal class Game
    {

        private readonly LevelData levelData = new();

        public Game()
        {
            // load the map data
            levelData.Load("../../../level1.txt");

        }

        public void Run()
        {
            // simulate simple game loop
            var player = levelData.Elements.OfType<Player>().FirstOrDefault();
            while (true)
            {
                Console.Clear(); // refresh the console for each frame

                // draw the level data
                foreach (var element in levelData.Elements)
                {
                    element.Draw();
                }

                // wait for user input to update
                var key = Console.ReadKey();

                if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }
                player.X++;
            }
        }
    }
}
