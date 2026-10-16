using System;

namespace neu26g_labb2.Models
{
    public abstract class LevelElement
    {
        public int X { get; set; }
        public int Y { get; set; }

        public char Symbol { get; protected set; }
        public ConsoleColor Color { get; protected set; }

        protected LevelElement(int x, int y, char symbol, ConsoleColor color)
        {
            X = x;
            Y = y;
            Symbol = symbol;
            Color = color;
        }

        public void Draw()
        {
            // TODO: Draw Symbol at X/Y using Color.
        }
    }
}
