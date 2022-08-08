namespace AOC2019.Day20
{
    internal class Day20PuzzleManager : PuzzleManager
    {
        public List<Tile> Tiles { get; set; }

        public Day20PuzzleManager()
        {
            var inputHelper = new Day20InputHelper(INPUT_FILE_NAME);
            Tiles = inputHelper.Parse();
        }

        public override Task SolveBothParts()
        {
            SolvePartOne();
            SolvePartTwo();
            return Task.CompletedTask;
        }

        public override Task SolvePartOne()
        {
            var solution = Solve();
            Console.WriteLine($"The solution to part one is '{solution}'.");
            return Task.CompletedTask;
        }

        public override Task SolvePartTwo()
        {
            var solution = Solve(isPartTwo: true);
            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
        }

        private int Solve(bool isPartTwo = false)
        {
            var allSearchStates = new Queue<(Tile currentTile, int steps, int level)>();
            allSearchStates.Enqueue((Tiles.First(x => x.IsStartingPosition), 0, 0));

            while (allSearchStates.Count > 0)
            {
                (var currentTile, var steps, var level) = allSearchStates.Dequeue();
                if (currentTile.VisitedOnLevel.Contains(level))
                {
                    continue;
                }
                currentTile.VisitedOnLevel.Add(level);

                List<Tile> unvisitedNeighbours;
                if (isPartTwo)
                {
                    unvisitedNeighbours = new List<Tile>();
                    foreach (var neighbour in currentTile.Neighbours)
                    {
                        if ((!neighbour.IsTeleportTile || neighbour.IsTeleportTile && !currentTile.IsTeleportTile) && !neighbour.VisitedOnLevel.Contains(level))
                        {
                            unvisitedNeighbours.Add(neighbour);
                        }
                        else if (currentTile.IsTeleportTile && neighbour.IsTeleportTile && currentTile.IsOuterTeleportTile && !neighbour.VisitedOnLevel.Contains(level - 1))
                        {
                            if (level <= 0)
                            {
                                continue;
                            }
                            unvisitedNeighbours.Add(neighbour);
                        }
                        else if (currentTile.IsTeleportTile && neighbour.IsTeleportTile && !currentTile.IsOuterTeleportTile && !neighbour.VisitedOnLevel.Contains(level + 1))
                        {
                            unvisitedNeighbours.Add(neighbour);
                        }
                    }
                }
                else
                {
                    unvisitedNeighbours = currentTile.Neighbours.Where(x => !x.VisitedOnLevel.Contains(level)).ToList();
                }

                foreach (var neighbour in unvisitedNeighbours)
                {
                    var levelToQueue = level;
                    if (neighbour.IsEndingPosition && level == 0)
                    {
                        foreach (var tile in Tiles)
                        {
                            tile.VisitedOnLevel = new HashSet<int>();
                        }
                        return steps + 1;
                    }
                    if (isPartTwo)
                    {
                        if (currentTile.IsTeleportTile && neighbour.IsTeleportTile && currentTile.IsOuterTeleportTile)
                        {
                            if (level == 0)
                            {
                                continue;
                            }
                            levelToQueue--;
                        }
                        else if (currentTile.IsTeleportTile && neighbour.IsTeleportTile && !currentTile.IsOuterTeleportTile)
                        {
                            levelToQueue++;
                        }
                    }

                    allSearchStates.Enqueue((neighbour, steps + 1, levelToQueue));
                }
            }
            return -1;
        }
    }
}
