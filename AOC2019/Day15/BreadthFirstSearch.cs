namespace AOC2019.Day15
{
    internal class BreadthFirstSearch
    {
        public List<Tile> Tiles { get; private set; }
        public Queue<(int xCoord, int yCoord, int steps)> PositionsWithSteps { get; set; } = new Queue<(int, int, int)>();

        public BreadthFirstSearch(List<Tile> tiles)
        {
            Tiles = tiles;
        }

        internal int FindShortestNumberOfStepsFromStartToOxygen()
        {
            foreach(var tile in Tiles)
            {
                tile.Explored = false;
            }
            PositionsWithSteps = new Queue<(int, int, int)>();
            var startTile = Tiles.First(x => x.Position == (0, 0));
            PositionsWithSteps.Enqueue((startTile.Position.Item1, startTile.Position.Item2, 0));

            while (PositionsWithSteps.Count > 0)
            {
                var nextPositionWithStep = PositionsWithSteps.Dequeue();
                var currentTile = Tiles.First(x => x.Position == (nextPositionWithStep.xCoord, nextPositionWithStep.yCoord));
                if (currentTile.Type == TileType.Oxygen)
                {
                    return nextPositionWithStep.steps;
                }
                currentTile.Explored = true;

                foreach (var unexploredNeighbour in FindUnexploredNeighbours(currentTile))
                {
                    PositionsWithSteps.Enqueue((unexploredNeighbour.Position.Item1, unexploredNeighbour.Position.Item2, nextPositionWithStep.steps + 1));
                }
                
            }
            throw new Exception("Shortest path could not be found.");
        }

        private List<Tile> FindUnexploredNeighbours(Tile currentTile)
        {
            return Tiles.Where(x => (
            x.Position == (currentTile.Position.Item1, currentTile.Position.Item2 + 1) ||
            x.Position == (currentTile.Position.Item1, currentTile.Position.Item2 - 1) ||
            x.Position == (currentTile.Position.Item1 + 1, currentTile.Position.Item2) ||
            x.Position == (currentTile.Position.Item1 - 1, currentTile.Position.Item2)
            ) && x.Type != TileType.Wall 
            && !x.Explored).ToList();
        }

        internal object FindTimeToFillWithOxygen()
        {
            foreach (var tile in Tiles)
            {
                tile.Explored = false;
            }
            PositionsWithSteps = new Queue<(int, int, int)>();
            var startTile = Tiles.First(x => x.Type == TileType.Oxygen);
            PositionsWithSteps.Enqueue((startTile.Position.Item1, startTile.Position.Item2, 0));
            var latestStepCount = 0;

            while (PositionsWithSteps.Count > 0)
            {
                var nextPositionWithStep = PositionsWithSteps.Dequeue();
                var currentTile = Tiles.First(x => x.Position == (nextPositionWithStep.xCoord, nextPositionWithStep.yCoord));
                currentTile.Explored = true;

                foreach (var unexploredNeighbour in FindUnexploredNeighbours(currentTile))
                {
                    PositionsWithSteps.Enqueue((unexploredNeighbour.Position.Item1, unexploredNeighbour.Position.Item2, nextPositionWithStep.steps + 1));
                }
                latestStepCount = nextPositionWithStep.steps;
            }
            return latestStepCount;
        }
    }
}
