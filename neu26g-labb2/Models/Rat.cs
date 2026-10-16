using System;
using System.Collections.Generic;
using System.Text;

namespace neu26g_labb2.Models
{
    internal class Rat : Enemy
    {
        public Rat(int x, int y) : base(
            x,
            y,
            'r',
            ConsoleColor.Red,
            "Rat",
            5,
            new Dice(1, 4),
            new Dice(1, 2))
        {
        }
        public override void Update()
        {
            // TODO: Implement rat update logic
        }
    }
}
