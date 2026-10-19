using System;

namespace neu26g_labb2.Models
{
    public class Snake : Enemy
    {
        private const int AggroRange = 2;

        public Snake(int x, int y) : base(
            x,
            y,
            's',
            ConsoleColor.Green,
            "Snake",
            25,
            new Dice(3, 4, 2),
            new Dice(1, 8, 5))
        {
        }

        public override void Update(IGameContext context)
        {
            int dx = X - context.Player.X;
            int dy = Y - context.Player.Y;

            if (dx * dx + dy * dy > AggroRange * AggroRange)
            {
                return;
            }

            int moveX = Math.Abs(dx) >= Math.Abs(dy) ? Math.Sign(dx) : 0;
            int moveY = Math.Abs(dy) > Math.Abs(dx) ? Math.Sign(dy) : 0;

            if (moveX == 0 && moveY == 0)
            {
                return;
            }

            context.TryMove(this, X + moveX, Y + moveY);
        }
    }
}
