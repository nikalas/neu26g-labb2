namespace neu26g_labb2.Models
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public static class DirectionExtensions
    {
        public static (int X, int Y) Offset(this Direction direction)
        {
            return direction switch
            {
                Direction.Up => (0, -1),
                Direction.Down => (0, 1),
                Direction.Left => (-1, 0),
                Direction.Right => (1, 0),
                _ => (0, 0)
            };
        }
    }
}
