namespace AOC2019.Day20
{
    internal class Day20PuzzleManager : PuzzleManager
    {
        //protected override string INPUT_FILE_NAME { get; set; } = "test2.txt";

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
            throw new NotImplementedException();
        }

        private int Solve(bool isPartTwo = false)
        {
            var tilesToSearchAndStepsTaken = new Queue<(Tile currentTile, int steps, int level)>();
            tilesToSearchAndStepsTaken.Enqueue((Tiles.First(x => x.IsStartingPosition), 0, 0));

            while (true)
            {
                (var currentTile, var steps, var level) = tilesToSearchAndStepsTaken.Dequeue();
                currentTile.VisitedOnLevel.Add(level);

                foreach (var neighbour in currentTile.Neighbours.Where(x => !x.VisitedOnLevel.Contains(level)))
                {
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
                        // add code for checking if gone up or down a level here
                    }

                    tilesToSearchAndStepsTaken.Enqueue((neighbour, steps + 1, level));
                }
            }
        }
    }
}
