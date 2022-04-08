namespace AOC2019.Day3
{
    internal class Day3InputHelper : InputHelper<List<Instruction>[]>
    {
        public Day3InputHelper(string fileName) : base(fileName)
        {
        }

        public override List<Instruction>[] Parse()
        {
            var wireInstructions = new List<Instruction>[2];
            using (var sr = new StreamReader(InputPath))
            {
                string ln;
                var counter = 0;
                while ((ln = sr.ReadLine()!) != null)
                {
                    wireInstructions[counter] = new List<Instruction>();
                    var instructions = ln.Split(',');
                    foreach (var instruction in instructions)
                    {
                        wireInstructions[counter].Add(new Instruction(instruction[0], int.Parse(instruction.Substring(1))));
                    }
                    counter++;
                }
            }
            return wireInstructions;
        }
    }
}
