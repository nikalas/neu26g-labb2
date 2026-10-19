using System;

namespace neu26g_labb2.Models
{
    public class Rat : Enemy
    {
        private static readonly Direction[] Directions = Enum.GetValues<Direction>();

        public Rat(int x, int y) : base(
            x,
            y,
            'r',
            ConsoleColor.Red,
            "Rat",
            10,
            new Dice(1, 6, 3),
            new Dice(1, 6, 1))
        {
        }

        public override void Update(IGameContext context)
        {
            Direction direction = Directions[Random.Shared.Next(Directions.Length)];
            (int dx, int dy) = direction.Offset();

            context.TryMove(this, X + dx, Y + dy);
        }
    }
}
