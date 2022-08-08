namespace AOC2019.Day20
{
    internal class Day20InputHelper : InputHelper<List<Tile>>
    {
        public Day20InputHelper(string fileName) : base(fileName)
        {
        }

        public override List<Tile> Parse()
        {
            var output = new List<Tile>();
            var maxXCoord = 0;
            var maxYCoord = -1;
            using (var sr = new StreamReader(InputPath))
            {
                string ln;
                while ((ln = sr.ReadLine()!) != null)
                {
                    maxXCoord = Math.Max(maxXCoord, ln.Length - 1);
                    maxYCoord++;
                }
            }

            using (var sr = new StreamReader(InputPath))
            {
                string ln;
                var yCoord = 0;
                while ((ln = sr.ReadLine()!) != null)
                {
                    var xCoord = 0;
                    foreach (var character in ln)
                    {
                        if (character == ' ' || character == '#')
                        {
                            xCoord++;
                            continue;
                        }
                        if (character == '.')
                        {
                            output.Add(new Tile((xCoord++, yCoord)));
                        }
                        else
                        {
                            var isOuterTeleportTile = false;

                            if (xCoord == 0 || xCoord == maxXCoord || yCoord == 0 || yCoord == maxYCoord)
                            {
                                isOuterTeleportTile = true;
                            }

                            output.Add(
                                new Tile((xCoord++, yCoord),
                                isTeleportIdentifier: true, 
                                teleportValue: character.ToString(),
                                isOuterTeleportTile: isOuterTeleportTile));
                        }
                    }
                    yCoord++;
                }
            }

            CalculateNeighbours(output);

            return output;
        }

        private void CalculateNeighbours(List<Tile> tiles)
        {
            AddTeleportNeighboursStartingAndFinishingTilesThenPruneTiles(tiles);
            AddNormalNeighbours(tiles);
        }

        private void AddTeleportNeighboursStartingAndFinishingTilesThenPruneTiles(List<Tile> tiles)
        {
            var teleportIdentifierTiles = tiles.Where(x => x.IsTeleportIdentifier);
            foreach (var identifierTile in teleportIdentifierTiles)
            {
                var teleportIdentifierList = new List<string>();
                var neighbours = tiles.Where(
                    x => x.Coordinates == (identifierTile.Coordinates.Item1 + 1, identifierTile.Coordinates.Item2) ||
                         x.Coordinates == (identifierTile.Coordinates.Item1 - 1, identifierTile.Coordinates.Item2) ||
                         x.Coordinates == (identifierTile.Coordinates.Item1, identifierTile.Coordinates.Item2 + 1) ||
                         x.Coordinates == (identifierTile.Coordinates.Item1, identifierTile.Coordinates.Item2 - 1));
                if (neighbours.Count() < 2)
                {
                    continue;
                }
                else
                {
                    teleportIdentifierList.Add(identifierTile.TeleportValue!);
                    teleportIdentifierList.Add(neighbours.First(x => x.IsTeleportIdentifier).TeleportValue!);
                }
                var teleporterTile = neighbours.First(x => !x.IsTeleportIdentifier);
                var otherTeleporterIdentifier = neighbours.First(x => x.IsTeleportIdentifier);

                string teleportIdentifierString;
                if (teleporterTile.Coordinates.Item1 == identifierTile.Coordinates.Item1 + 1 || teleporterTile.Coordinates.Item2 == identifierTile.Coordinates.Item2 + 1)
                {
                    teleportIdentifierString = otherTeleporterIdentifier.TeleportValue + identifierTile.TeleportValue;
                }
                else
                {
                    teleportIdentifierString = identifierTile.TeleportValue + otherTeleporterIdentifier.TeleportValue;
                }

                if (teleportIdentifierString == "AA")
                {
                    teleporterTile.IsStartingPosition = true;
                }
                else if (teleportIdentifierString == "ZZ")
                {
                    teleporterTile.IsEndingPosition = true;
                }
                else
                {
                    teleporterTile.IsTeleportTile = true;
                    teleporterTile.TeleportValue = teleportIdentifierString;
                    if (identifierTile.IsOuterTeleportTile || otherTeleporterIdentifier.IsOuterTeleportTile)
                    {
                        teleporterTile.IsOuterTeleportTile = true;
                    }
                }
            }

            var teleportTiles = tiles.Where(x => x.IsTeleportTile);
            foreach (var teleportTile in teleportTiles)
            {
                // Should probably optimise here by adding both neighbours to each other but I don't want to mutate the list I'm iterating through.
                var matchingTeleportTile = teleportTiles.First(x => x.TeleportValue == teleportTile.TeleportValue && x.Coordinates != teleportTile.Coordinates);
                teleportTile.Neighbours.Add(matchingTeleportTile);
            }
            tiles.RemoveAll(x => x.IsTeleportIdentifier);
        }

        private void AddNormalNeighbours(List<Tile> tiles)
        {
            foreach (var tile in tiles)
            {
                var neighbours = tiles.Where(
                    x => x.Coordinates == (tile.Coordinates.Item1 + 1, tile.Coordinates.Item2) ||
                         x.Coordinates == (tile.Coordinates.Item1 - 1, tile.Coordinates.Item2) ||
                         x.Coordinates == (tile.Coordinates.Item1, tile.Coordinates.Item2 + 1) ||
                         x.Coordinates == (tile.Coordinates.Item1, tile.Coordinates.Item2 - 1));

                tile.Neighbours.AddRange(neighbours);
            }
        }
    }
}
