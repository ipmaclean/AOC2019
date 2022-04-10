using System.Text.RegularExpressions;

namespace AOC2019.Day12
{
    internal class Day12InputHelper : InputHelper<int[][]>
    {
        public Day12InputHelper(string fileName) : base(fileName)
        {
        }

        public override int[][] Parse()
        {
            var input = new int[4][];
            using (var sr = new StreamReader(InputPath))
            {
                string ln;
                var moonCounter = 0;
                var numberRegex = new Regex(@"-*\d+");
                while ((ln = sr.ReadLine()!) != null)
                {
                    var position = new int[3];
                    var numberMatches = numberRegex.Matches(ln);
                    var regexCounter = 0;
                    foreach (Match match in numberMatches)
                    {
                        position[regexCounter++] = int.Parse(match.Value);
                    }
                    input[moonCounter++] = position;
                }
            }
            return input;
        }
    }
}
