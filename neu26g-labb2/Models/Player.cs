using System;

namespace neu26g_labb2.Models
{
    public class Player : Actor
    {
        public Direction? PendingDirection { get; set; }

        public Player(int x, int y) : base(
            x,
            y,
            '@',
            ConsoleColor.White,
            "Player",
            100,
            new Dice(2, 6, 2),
            new Dice(2, 6))
        {
        }

        public override void OnTick(IGameContext context)
        {
            if (IsDead || PendingDirection is not Direction direction)
            {
                return;
            }

            PendingDirection = null;

            (int dx, int dy) = direction.Offset();
            context.TryMove(this, X + dx, Y + dy);
        }
    }
}
