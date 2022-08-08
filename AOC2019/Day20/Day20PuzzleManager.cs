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
            var tilesToSearchAndStepsTaken = new Queue<(Tile, int)>();
            tilesToSearchAndStepsTaken.Enqueue((Tiles.First(x => x.IsStartingPosition), 0));

            while (true)
            {
                (var currentTile, var steps) = tilesToSearchAndStepsTaken.Dequeue();
                currentTile.IsVisited = true;

                foreach (var neighbour in currentTile.Neighbours.Where(x => !x.IsVisited))
                {
                    if (neighbour.IsEndingPosition)
                    {
                        Console.WriteLine($"The solution to part one is '{steps + 1}'.");
                        foreach (var tile in Tiles)
                        {
                            tile.IsVisited = false;
                        }
                        return Task.CompletedTask;
                    }
                    tilesToSearchAndStepsTaken.Enqueue((neighbour, steps + 1));
                }
            }
        }

        public override Task SolvePartTwo()
        {
            throw new NotImplementedException();
        }
    }
}
