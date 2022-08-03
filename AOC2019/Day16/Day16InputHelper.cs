namespace AOC2019.Day16
{
    internal class Day16InputHelper : InputHelper<List<int>>
    {
        public Day16InputHelper(string fileName) : base(fileName)
        {
        }

        public override List<int> Parse()
        {
            var output = new List<int>();
            using (var sr = new StreamReader(InputPath))
            {
                var ln = sr.ReadLine();
                foreach (var digit in ln!)
                {
                    output.Add(digit - '0');
                }
            }
            return output;
        }
    }
}