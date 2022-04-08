namespace AOC2019.IntCode
{
    internal class IntCodeInputHelper : InputHelper<int[]>
    {
        public IntCodeInputHelper(string fileName) : base(fileName)
        {
        }

        public override int[] Parse()
        {
            var output = new int[0];
            using (var sr = new StreamReader(InputPath))
            {
                var ints = sr.ReadLine()!.Split(',');
                output = Array.ConvertAll(ints, int.Parse);
            }
            return output;
        }
    }
}
