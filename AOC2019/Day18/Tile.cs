namespace AOC2019.Day18
{
    internal class Tile
    {
        public (int, int) Coordinates { get; set; }
        public char Value { get; set; }
        public bool IsKey { get; set; }
        public bool IsDoor { get; set; }
        public bool IsStartingPosition { get; set; }
        public bool IsVisited { get; set; } = false;

        public Tile(
            (int, int) coordinates,
            char value, bool isKey,
            bool isDoor,
            bool isStartingPosition)
        {
            Coordinates = coordinates;
            Value = value;
            IsKey = isKey;
            IsDoor = isDoor;
            IsStartingPosition = isStartingPosition;
        }
    }
}
