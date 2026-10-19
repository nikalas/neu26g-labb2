using System;
using GameApp = neu26g_labb2.Game.Game;

namespace neu26g_labb2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GameApp game = args.Length > 0 ? new GameApp(args[0]) : new GameApp();
            game.Run();
        }
    }
}
