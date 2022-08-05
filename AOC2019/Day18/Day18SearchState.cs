namespace AOC2019.Day18
{
    internal class Day18SearchState
    {
        public char CurrentLocation { get; set; }
        public HashSet<char> KeysCollected { get; set; }
        public int Distance { get; set; }

        public Day18SearchState(char currentLocation, HashSet<char> keysCollected, int distance)
        {
            CurrentLocation = currentLocation;
            KeysCollected = new HashSet<char>(keysCollected);
            Distance = distance;
        }
    }
}
