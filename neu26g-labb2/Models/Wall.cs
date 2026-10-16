using System;
using System.Collections.Generic;
using System.Text;

namespace neu26g_labb2.Models
{
    internal class Wall : LevelElement
    {
        public Wall(int x, int y) : base(x, y, '#', ConsoleColor.Gray)
        {
        }
    }
}
