namespace neu26g_labb2.Models
{
    internal class Dice
    {
        public readonly int numberOfDice;
        public readonly int sidesPerDice;
        public readonly int modifier;

        public Dice(int numberOfDice, int sidesPerDice, int modifier)
        {
            this.numberOfDice = numberOfDice;
            this.sidesPerDice = sidesPerDice;
            this.modifier = modifier;
        }

        public Dice(int numberOfDice, int sidesPerDice) : this(numberOfDice, sidesPerDice, 0) {}

        public int Throw()
        {
            int total = 0;
            for (int i = 0; i < numberOfDice; i++)
            {
                total += new Random().Next(1, sidesPerDice + 1);
            }
            return total + modifier;
        }

        public override string ToString()
        {
            return $"{numberOfDice}d{sidesPerDice}+{modifier}";
        }

    }
}