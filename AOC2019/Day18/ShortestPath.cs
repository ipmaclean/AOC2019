namespace AOC2019.Day18
{
    internal class ShortestPath
    {
        public HashSet<char> TilesBetween { get; set; }
        public HashSet<char> DoorsBetween { get; set; }
        public HashSet<char> KeysBetween { get; set; }
        public int Distance { get; set; }

        public ShortestPath(HashSet<char> tilesBetween, HashSet<char> doorsBetween, HashSet<char> keysBetween, int Distance)
        {
            TilesBetween = new HashSet<char>(tilesBetween);
            DoorsBetween = new HashSet<char>(doorsBetween);
            KeysBetween = new HashSet<char>(keysBetween);
            this.Distance = Distance;
        }
    }
}
