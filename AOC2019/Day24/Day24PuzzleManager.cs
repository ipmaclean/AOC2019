using System.Text;

namespace AOC2019.Day24
{
    internal class Day24PuzzleManager : PuzzleManager
    {

        public List<Tile> Tiles { get; set; }

        protected override string INPUT_FILE_NAME { get; set; } = "test1.txt";

        public Day24PuzzleManager()
        {
            var inputHelper = new Day24InputHelper(INPUT_FILE_NAME);
            Tiles = inputHelper.Parse();
        }

        public override Task SolveBothParts()
        {
            throw new NotImplementedException();
        }

        public override Task SolvePartOne()
        {
            var biodiversityRatings = new HashSet<int>();
            var tiles = Tiles;

            while (true)
            {
                var biodiversityRating = CalculateBiodiversityRating(tiles);
                if (biodiversityRatings.Contains(biodiversityRating))
                {
                    Console.WriteLine($"The solution to part one is '{biodiversityRating}'.");
                    return Task.CompletedTask;
                }
                biodiversityRatings.Add(biodiversityRating);
                tiles = RunGameOfLifePartOne(tiles);
            }
        }

        private void Print(List<Tile> tiles)
        {
            Console.Clear();
            var xCoordCounter = 0;
            var sb = new StringBuilder();
            foreach (var tile in tiles.OrderBy(x => x.Coordinates.Y).ThenBy(x => x.Coordinates.X))
            {
                if (xCoordCounter++ % 5 == 0)
                {
                    sb.Append(Environment.NewLine);
                }
                if (tile.IsBug)
                {
                    sb.Append("#");
                }
                else
                {
                    sb.Append(".");
                }
            }
            Console.Write(sb.ToString());
        }

        public override Task SolvePartTwo()
        {
            throw new NotImplementedException();
        }

        private List<Tile> RunGameOfLifePartOne(List<Tile> oldTiles)
        {
            var newTiles = new List<Tile>();

            foreach (var tile in oldTiles)
            {
                var bugCount = CountAdjacentBugsPartOne(tile, oldTiles);
                if (tile.IsBug && bugCount == 1)
                {
                    newTiles.Add(new Tile(tile.Coordinates, true));
                }
                else if (!tile.IsBug && (bugCount == 1 || bugCount == 2))
                {
                    newTiles.Add(new Tile(tile.Coordinates, true));
                }
                else
                {
                    newTiles.Add(new Tile(tile.Coordinates, false));
                }
            }

            return newTiles;
        }

        private int CalculateBiodiversityRating(List<Tile> tiles)
        {
            var rating = 0;
            var binaryCounter = 1;
            foreach (var tile in tiles.OrderBy(x => x.Coordinates.Y).ThenBy(x => x.Coordinates.X))
            {
                if (tile.IsBug)
                {
                    rating += binaryCounter;
                }
                binaryCounter *= 2;
            }
            return rating;
        }

        private int CountAdjacentBugsPartOne(Tile tile, List<Tile> tiles)
        {
            var maxX = tiles.Max(x => x.Coordinates.X);
            var maxY = tiles.Max(x => x.Coordinates.Y);

            var adjacentCoords = new (int X, int Y)[]
            {
                (0, 1),
                (0, -1),
                (1, 0),
                (-1, 0)
            };

            var bugCount = 0;
            foreach (var adjacentCoord in adjacentCoords)
            {
                (int X, int Y) coordToCheck = (tile.Coordinates.X + adjacentCoord.X, tile.Coordinates.Y + adjacentCoord.Y);

                if (coordToCheck.X < 0 || coordToCheck.X > maxX || coordToCheck.Y < 0 || coordToCheck.Y > maxY)
                {
                    continue;
                }
                if (tiles.First(x => x.Coordinates == coordToCheck).IsBug)
                {
                    bugCount++;
                }
            }
            return bugCount;
        }

        private List<Tile> DeepCopyTiles(List<Tile> tiles)
        {
            var copy = new List<Tile>();
            foreach (var tile in tiles)
            {
                copy.Add(new Tile(tile.Coordinates, tile.IsBug));
            }
            return copy;
        }
    }
}
