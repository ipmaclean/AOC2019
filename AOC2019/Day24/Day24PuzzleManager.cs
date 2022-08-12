using System.Text;

namespace AOC2019.Day24
{
    internal class Day24PuzzleManager : PuzzleManager
    {

        public List<Tile> Tiles { get; set; }

        public Day24PuzzleManager()
        {
            var inputHelper = new Day24InputHelper(INPUT_FILE_NAME);
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

        public override Task SolvePartTwo()
        {
            var tiles = DeepCopyTiles(Tiles);

            for (var i = 0; i < 200; i++)
            {
                tiles = RunGameOfLifePartTwo(tiles);
            }
            Console.WriteLine($"The solution to part two is '{tiles.Count(x => x.IsBug)}'.");
            return Task.CompletedTask;
        }

        private List<Tile> RunGameOfLifePartTwo(List<Tile> oldTiles)
        {
            var newTiles = new List<Tile>();
            var maxLevel = oldTiles.Max(x => x.Level);
            var minLevel = oldTiles.Min(x => x.Level);
            var outsideTiles = new (int, int)[]
            {
                (0, 0),
                (1, 0),
                (2, 0),
                (3, 0),
                (4, 0),
                (4, 1),
                (4, 2),
                (4, 3),
                (4, 4),
                (3, 4),
                (2, 4),
                (1, 4),
                (0, 4),
                (0, 3),
                (0, 2),
                (0, 1)
            };
            if (oldTiles.Any(x => x.Level == minLevel && x.IsBug && outsideTiles.Contains(x.Coordinates)))
            {
                AddEmptyLevel(oldTiles, minLevel - 1);
            }

            var insideTiles = new (int, int)[]
            {
                (1, 1),
                (2, 1),
                (3, 1),
                (3, 2),
                (3, 3),
                (2, 3),
                (1, 3),
                (1, 2)
            };
            if (oldTiles.Any(x => x.Level == maxLevel && x.IsBug && insideTiles.Contains(x.Coordinates)))
            {
                AddEmptyLevel(oldTiles, maxLevel + 1);
            }

            foreach (var tile in oldTiles)
            {
                if (tile.Coordinates == (2, 2))
                {
                    continue;
                }
                var bugCount = CountAdjacentBugsPartTwo(tile, oldTiles);
                if (tile.IsBug && bugCount == 1)
                {
                    newTiles.Add(new Tile(tile.Coordinates, true, tile.Level));
                }
                else if (!tile.IsBug && (bugCount == 1 || bugCount == 2))
                {
                    newTiles.Add(new Tile(tile.Coordinates, true, tile.Level));
                }
                else
                {
                    newTiles.Add(new Tile(tile.Coordinates, false, tile.Level));
                }
            }

            return newTiles;
        }

        private void AddEmptyLevel(List<Tile> tiles, int level)
        {
            for (var y = 0; y < 5; y++)
            {
                for (var x = 0; x < 5; x++)
                {
                    tiles.Add(new Tile((x, y), false, level));
                }
            }
        }

        private int CountAdjacentBugsPartTwo(Tile tile, List<Tile> tiles)
        {
            var maxX = 4;
            var maxY = 4;

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

                if (coordToCheck.X < 0)
                {
                    if (tiles.Any(x => x.Level == tile.Level - 1))
                    {
                        bugCount = tiles.First(x => x.Coordinates == (1, 2) && x.Level == tile.Level - 1).IsBug ? bugCount + 1 : bugCount;
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (coordToCheck.X > maxX)
                {
                    if (tiles.Any(x => x.Level == tile.Level - 1))
                    {
                        bugCount = tiles.First(x => x.Coordinates == (3, 2) && x.Level == tile.Level - 1).IsBug ? bugCount + 1 : bugCount;
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (coordToCheck.Y < 0)
                {
                    if (tiles.Any(x => x.Level == tile.Level - 1))
                    {
                        bugCount = tiles.First(x => x.Coordinates == (2, 1) && x.Level == tile.Level - 1).IsBug ? bugCount + 1 : bugCount;
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (coordToCheck.Y > maxY)
                {
                    if (tiles.Any(x => x.Level == tile.Level - 1))
                    {
                        bugCount = tiles.First(x => x.Coordinates == (2, 3) && x.Level == tile.Level - 1).IsBug ? bugCount + 1 : bugCount;
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (coordToCheck == (2, 2))
                {
                    if (tiles.Any(x => x.Level == tile.Level + 1))
                    {
                        (int, int)[] tilesToCheck;
                        if (tile.Coordinates == (1, 2))
                        {
                            tilesToCheck = new (int, int)[]
                            {
                                (0, 0),
                                (0, 1),
                                (0, 2),
                                (0, 3),
                                (0, 4),
                            };
                        }
                        else if (tile.Coordinates == (3, 2))
                        {
                            tilesToCheck = new (int, int)[]
                            {
                                (4, 0),
                                (4, 1),
                                (4, 2),
                                (4, 3),
                                (4, 4),
                            };
                        }
                        else if (tile.Coordinates == (2, 1))
                        {
                            tilesToCheck = new (int, int)[]
                            {
                                (0, 0),
                                (1, 0),
                                (2, 0),
                                (3, 0),
                                (4, 0),
                            };
                        }
                        else if (tile.Coordinates == (2, 3))
                        {
                            tilesToCheck = new (int, int)[]
                            {
                                (0, 4),
                                (1, 4),
                                (2, 4),
                                (3, 4),
                                (4, 4),
                            };
                        }
                        else
                        {
                            throw new ArgumentException("Impossible tile.");
                        }
                        bugCount += tiles.Count(x => x.Level == tile.Level + 1 && tilesToCheck.Contains(x.Coordinates) && x.IsBug);
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (tiles.First(x => x.Coordinates == coordToCheck && x.Level == tile.Level).IsBug)
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
