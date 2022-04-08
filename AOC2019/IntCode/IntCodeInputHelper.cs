namespace AOC2019.IntCode
{
    internal class IntCodeInputHelper : InputHelper<Dictionary<long, long>>
    {
        public IntCodeInputHelper(string fileName) : base(fileName)
        {
        }

        public override Dictionary<long, long> Parse()
        {
            var output = new Dictionary<long, long>();
            using (var sr = new StreamReader(InputPath))
            {
                var ints = sr.ReadLine()!.Split(',');
                for (long i = 0; i < ints.LongLength; i++)
                {
                    output.Add(i, long.Parse(ints[i]));
                }
            }
            return output;
        }
    }
}
