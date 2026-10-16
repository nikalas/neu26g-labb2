using System;
using System.Collections.Generic;
using System.Text;

namespace neu26g_labb2.Models
{
    internal class Snake : Enemy
    {
        public Snake(int x, int y) : base(
            x,
            y,
            's',
            ConsoleColor.Green,
            "Snake",
            25,
            new Dice(1, 6),
            new Dice(1, 4))
        {
        }

        public override void Update()
        {
            // TODO: Implement snake update logic
        }
    }
}
