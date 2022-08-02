namespace AOC2019.Day15
{
    internal class Tile
    {
        public (int, int) Position { get; private set; }
        public TileType Type { get; private set; }
        public List<Direction> DirectionsSearched { get; private set; } = new List<Direction>();
        public bool Explored { get; set; } = false;

        public Tile((int, int) position, TileType type, Direction? directionSearched = null)
        {
            Position = position;
            Type = type;
            if (directionSearched != null)
            {
                DirectionsSearched.Add(directionSearched.Value);
            }
        }
    }
}
