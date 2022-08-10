using System.Text.RegularExpressions;

namespace AOC2019.Day22
{
    internal class Day22InputHelper : InputHelper<List<Command>>
    {
        public Day22InputHelper(string fileName) : base(fileName)
        {
        }

        public override List<Command> Parse()
        {
            var output = new List<Command>();
            using (var sr = new StreamReader(InputPath))
            {
                string ln;
                var numberRegex = new Regex(@"-*\d+");
                while ((ln = sr.ReadLine()!) != null)
                {
                    if (ln.Substring(0, 3) == "cut")
                    {
                        var numberMatch = numberRegex.Match(ln);
                        var number = numberMatch?.Value ?? "0";
                        output.Add(new Command(CommandDescription.Cut, int.Parse(number)));
                    }
                    else if (ln.Substring(0,19) == "deal into new stack")
                    {
                        output.Add(new Command(CommandDescription.DealIntoNewStack, 0));
                    }
                    else if (ln.Substring(0, 19) == "deal with increment")
                    {
                        var numberMatch = numberRegex.Match(ln);
                        var number = numberMatch?.Value ?? "0";
                        output.Add(new Command(CommandDescription.DealWithIncrement, int.Parse(number)));
                    }
                }
            }
            return output;
        }
    }
}
