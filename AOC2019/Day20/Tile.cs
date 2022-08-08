namespace AOC2019.Day20
{
    internal class Tile
    {
        public (int, int) Coordinates { get; set; }
        public List<Tile> Neighbours { get; set; } = new List<Tile>();
        public string? TeleportValue { get; set; } = null;
        public bool IsTeleportTile { get; set; } = false;
        public bool IsOuterTeleportTile { get; set; } = false;
        public bool IsTeleportIdentifier { get; set; } = false;
        public bool IsStartingPosition { get; set; } = false;
        public bool IsEndingPosition { get; set; } = false;
        public HashSet<int> VisitedOnLevel { get; set; } = new HashSet<int>();

        public Tile((int, int) coordinates,
            string? teleportValue = null,
            bool isTeleportTile = false,
            bool isTeleportIdentifier = false,
            bool isStartingPosition = false,
            bool isEndingPosition = false,
            bool isOuterTeleportTile = false)
        {
            Coordinates = coordinates;
            TeleportValue = teleportValue;
            IsTeleportTile = isTeleportTile;
            IsTeleportIdentifier = isTeleportIdentifier;
            IsStartingPosition = isStartingPosition;
            IsEndingPosition = isEndingPosition;
            IsOuterTeleportTile = isOuterTeleportTile;
        }
    }
}
