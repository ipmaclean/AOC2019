namespace AOC2019.Day18
{
    internal class Day18InputHelper : InputHelper<List<Tile>>
    {
        public Day18InputHelper(string fileName) : base(fileName)
        {
        }

        public override List<Tile> Parse()
        {
            var output = new List<Tile>();
            var sr = new StreamReader(InputPath);
            string ln;
            var keys = "abcdefghijklmnopqrstuvwxyz";
            var doors = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var xCoord = 0;
            var startTileCounter = 0;
            var startTiles = new char[] { '@', '<', '>', '^' };
            while ((ln = sr.ReadLine()!) != null)
            {
                var yCoord = 0;
                foreach (var character in ln)
                {
                    if (character == '.')
                    {
                        output.Add(new Tile((xCoord, yCoord), character, isKey: false, isDoor: false, isStartingPosition: false));
                    }
                    else if (character == '@')
                    {
                        output.Add(new Tile((xCoord, yCoord), startTiles[startTileCounter++], isKey: false, isDoor: false, isStartingPosition: true));
                    }
                    else if (keys.Contains(character))
                    {
                        output.Add(new Tile((xCoord, yCoord), character, isKey: true, isDoor: false, isStartingPosition: false));
                    }
                    else if (doors.Contains(character))
                    {
                        output.Add(new Tile((xCoord, yCoord), character, isKey: false, isDoor: true, isStartingPosition: false));
                    }
                    yCoord++;
                }
                xCoord++;
            }
            return output;
        }
    }
}