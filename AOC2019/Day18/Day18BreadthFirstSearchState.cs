namespace AOC2019.Day18
{
    internal class Day18BreadthFirstSearchState
    {
        public int Steps { get; set; }
        public Tile CurrentTile { get; set; }
        public HashSet<char> DoorsBetween { get; set; }
        public HashSet<char> KeysBetween { get; set; }

        public Day18BreadthFirstSearchState(int steps, Tile currentTile, HashSet<char>? doorsBetween = null, HashSet<char>? keysBetween = null)
        {
            Steps = steps;
            CurrentTile = currentTile;
            DoorsBetween = doorsBetween != null ? new HashSet<char>(doorsBetween) : new HashSet<char>();
            KeysBetween = keysBetween != null ? new HashSet<char>(keysBetween) : new HashSet<char>();
        }
    }
}
