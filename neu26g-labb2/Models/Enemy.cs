using System;

namespace neu26g_labb2.Models
{
    public abstract class Enemy : Actor
    {
        protected Enemy(
            int x,
            int y,
            char symbol,
            ConsoleColor color,
            string name,
            int hp,
            Dice attackDice,
            Dice defenceDice
        ) : base(x, y, symbol, color, name, hp, attackDice, defenceDice)
        {
        }

        public sealed override void OnTick(IGameContext context)
        {
            if (IsDead || context.Player.IsDead)
            {
                return;
            }

            Update(context);
        }

        public abstract void Update(IGameContext context);
    }
}
