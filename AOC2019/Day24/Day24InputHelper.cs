namespace AOC2019.Day24
{
    internal class Day24InputHelper : InputHelper<List<Tile>>
    {
        public Day24InputHelper(string fileName) : base(fileName)
        {
        }

        public override List<Tile> Parse()
        {
            var output = new List<Tile>();
            using (var sr = new StreamReader(InputPath))
            {
                var yCoord = 0;
                string ln;
                while ((ln = sr.ReadLine()!) != null)
                {
                    var xCoord = 0;
                    foreach (var character in ln)
                    {
                        output.Add(new Tile((xCoord++, yCoord), character == '#'));
                    }
                    yCoord++;
                }
            }
            return output;
        }
    }
}
