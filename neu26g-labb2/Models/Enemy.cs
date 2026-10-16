using System;

namespace neu26g_labb2.Models
{

    internal abstract class Enemy : LevelElement
    {
        public string Name { get; protected set; }
        public int HP { get; set; }

        internal Dice AttackDice { get; }
        internal Dice DefenceDice { get; }

        internal Enemy(
            int x,
            int y,
            char symbol,
            System.ConsoleColor color,
            string name,
            int hp,
            Dice attackDice,
            Dice defenceDice
        ) : base(x, y, symbol, color)
        {
            Name = name;
            HP = hp;
            AttackDice = attackDice;
            DefenceDice = defenceDice;
        }

        public abstract void Update();
    }
}
