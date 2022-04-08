namespace AOC2019.IntCode
{
    internal class IntCodeInputHelper : InputHelper<long[]>
    {
        public IntCodeInputHelper(string fileName) : base(fileName)
        {
        }

        public override long[] Parse()
        {
            var output = new long[0];
            using (var sr = new StreamReader(InputPath))
            {
                var ints = sr.ReadLine()!.Split(',');
                output = Array.ConvertAll(ints, long.Parse);
            }
            return output;
        }
    }
}
