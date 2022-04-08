using AOC2019.IntCode;

namespace AOC2019.Day9
{
    internal class Day9PuzzleManager : PuzzleManager
    {
        public long[] IntCodeProgram { get; private set; }

        public Day9PuzzleManager()
        {
            var inputHelper = new Day9InputHelper(INPUT_FILE_NAME);
            IntCodeProgram = inputHelper.Parse();
        }

        public async override Task SolveBothParts()
        {
            await SolvePartOne();
            Console.WriteLine();
            await SolvePartTwo();
        }

        public async override Task SolvePartOne()
        {

        }


        public async override Task SolvePartTwo()
        {

        }


        private async Task<Queue<long>> SolvePrivateAsync(Queue<long> inputs)
        {
            var codeInput = (long[])IntCodeProgram.Clone();
            var intCodeComputer = new IntCodeComputer(codeInput, inputs);
            await intCodeComputer.ProcessAsync();
            return intCodeComputer.Outputs;
        }
    }
}
