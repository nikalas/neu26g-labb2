using System;

namespace neu26g_labb2.Models
{
    public class Dice
    {
        public int NumberOfDice { get; }
        public int SidesPerDice { get; }
        public int Modifier { get; }

        public Dice(int numberOfDice, int sidesPerDice, int modifier)
        {
            if (numberOfDice <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(numberOfDice));
            }

            if (sidesPerDice <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sidesPerDice));
            }

            NumberOfDice = numberOfDice;
            SidesPerDice = sidesPerDice;
            Modifier = modifier;
        }

        public Dice(int numberOfDice, int sidesPerDice) : this(numberOfDice, sidesPerDice, 0) { }

        public int Throw()
        {
            int total = Modifier;

            for (int i = 0; i < NumberOfDice; i++)
            {
                total += Random.Shared.Next(1, SidesPerDice + 1);
            }

            return total;
        }

        public override string ToString()
        {
            string modifier = Modifier switch
            {
                > 0 => $"+{Modifier}",
                < 0 => Modifier.ToString(),
                _ => string.Empty
            };

            return $"{NumberOfDice}d{SidesPerDice}{modifier}";
        }
    }
}
