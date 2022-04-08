namespace AOC2019.Day10
{
    internal class Day10InputHelper : InputHelper<((int, int) dimensions, List<(int, int)> asteroids)>
    {
        public Day10InputHelper(string fileName) : base(fileName)
        {
        }

        public override ((int, int) dimensions, List<(int, int)> asteroids) Parse()
        {
            var dimensions = (0, 0);
            var asteroids = new List<(int, int)>();
            using (var sr = new StreamReader(InputPath))
            {
                string ln;
                var row = 0;
                while ((ln = sr.ReadLine()!) != null)
                {
                    dimensions.Item1 = ln.Length;
                    for (var column = 0; column < ln.Length; column++)
                    {
                        if (ln[column] == '#')
                        {
                            asteroids.Add((column, row));
                        }
                    }
                    row++;
                }
                dimensions.Item2 = --row;
                dimensions.Item1--;
            }
            return (dimensions, asteroids);
        }
    }
}
