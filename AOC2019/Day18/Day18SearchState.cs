namespace AOC2019.Day18
{
    internal class Day18SearchState
    {
        public HashSet<char> CurrentLocations { get; set; }
        public HashSet<char> KeysCollected { get; set; }
        public int Distance { get; set; }

        public Day18SearchState(HashSet<char> currentLocations, HashSet<char> keysCollected, int distance)
        {
            CurrentLocations = new HashSet<char>(currentLocations);
            KeysCollected = new HashSet<char>(keysCollected);
            Distance = distance;
        }
    }
}
