namespace AOC2019.Day4
{
    internal class Day4InputHelper : InputHelper<(int, int)>
    {
        public Day4InputHelper(string fileName) : base(fileName)
        {
        }

        public override (int, int) Parse()
        {
            var firstNumber = 0;
            var lastNumber = 0;
            using (var sr = new StreamReader(InputPath))
            {
                var ln = sr.ReadLine();
                firstNumber = int.Parse(ln!.Substring(0, 6));
                lastNumber = int.Parse(ln!.Substring(7));
            }
            return (firstNumber, lastNumber);
        }
    }
}
