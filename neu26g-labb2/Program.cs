using System;
using neu26g_labb2.Models;
namespace neu26g_labb2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            Console.ReadKey();

            // load level data from file
            LevelData levelData = new();
            levelData.Load("../../../level1.txt");

            // simulate simple game loop
            var player = levelData.Elements.OfType<Player>().FirstOrDefault();
            while (true)
            {
                Console.Clear(); // refresh the console for each frame

                // draw the level data
                Console.SetCursorPosition(0, 0);
                foreach(var element in levelData.Elements)
                {
                    element.Draw();
                }

                // wait for user input to update
                var key = Console.ReadKey();

                if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }

                player.X++; // move the player one step right
            }

        }
    }
}
