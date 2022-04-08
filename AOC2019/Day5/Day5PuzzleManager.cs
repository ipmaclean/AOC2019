using AOC2019.IntCode;

namespace AOC2019.Day5
{
    internal class Day5PuzzleManager : PuzzleManager
    {
        public int[] IntCodeProgram { get; private set; }

        public Day5PuzzleManager()
        {
            var inputHelper = new Day5InputHelper(INPUT_FILE_NAME);
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
            var codeOutput = await SolvePrivateAsync(1);
            Console.WriteLine($"The solution to part one is '{codeOutput}'.");
        }

        public async override Task SolvePartTwo()
        {
            var codeOutput = await SolvePrivateAsync(5);
            Console.WriteLine($"The solution to part two is '{codeOutput}'.");
        }

        private async Task<int> SolvePrivateAsync(int input)
        {
            var codeInput = (int[])IntCodeProgram.Clone();
            var inputs = new Queue<int>();
            inputs.Enqueue(input);
            var intCodeComputer = new IntCodeComputer(codeInput, inputs);
            await intCodeComputer.ProcessAsync();
            return intCodeComputer.Outputs.Last();
        }
    }
}
