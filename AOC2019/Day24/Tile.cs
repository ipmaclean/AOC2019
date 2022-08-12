namespace AOC2019.Day24
{
    internal class Tile
    {
        public (int X, int Y) Coordinates { get; set; }
        public bool IsBug { get; set; }
        public int Level { get; set; }

        public Tile((int X, int Y) coordinates, bool isBug, int level = 0)
        {
            Coordinates = coordinates;
            IsBug = isBug;
            Level = level;
        }
    }
}
