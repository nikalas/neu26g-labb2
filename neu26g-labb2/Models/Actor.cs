using System;

namespace neu26g_labb2.Models
{
    public abstract class Actor : LevelElement
    {
        public string Name { get; }
        public int HP { get; private set; }
        public int MaxHP { get; }

        public Dice AttackDice { get; }
        public Dice DefenceDice { get; }

        public bool IsDead => HP <= 0;

        protected Actor(
            int x,
            int y,
            char symbol,
            ConsoleColor color,
            string name,
            int maxHp,
            Dice attackDice,
            Dice defenceDice
        ) : base(x, y, symbol, color)
        {
            Name = name;
            MaxHP = maxHp;
            HP = maxHp;
            AttackDice = attackDice;
            DefenceDice = defenceDice;
        }

        public int TakeDamage(int amount)
        {
            if (amount <= 0)
            {
                return 0;
            }

            int applied = Math.Min(amount, HP);
            HP -= applied;
            return applied;
        }

        public abstract void OnTick(IGameContext context);
    }
}
