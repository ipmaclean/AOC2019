using AOC2019.IntCode;

namespace AOC2019.Day9
{
    public class Day9PuzzleManager : PuzzleManager
    {
        public Dictionary<long, long> IntCodeProgram { get; private set; }
        //protected override string INPUT_FILE_NAME { get; set; } = "test1.txt";

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
            var solution = await SolvePartOnePrivateAsync();
            Console.WriteLine($"The solution to part one is '{solution}'.");
        }

        public async Task<long> SolvePartOnePrivateAsync()
        {
            var inputs = new Queue<long>();
            inputs.Enqueue(1);
            var outputs = await ProcessAndReturnOutputs(inputs);
            return outputs.Last();
        }

        public async override Task SolvePartTwo()
        {
            var solution = await SolvePartTwoPrivateAsync();
            Console.WriteLine($"The solution to part two is '{solution}'.");
        }

        public async Task<long> SolvePartTwoPrivateAsync()
        {
            var inputs = new Queue<long>();
            inputs.Enqueue(2);
            var outputs = await ProcessAndReturnOutputs(inputs);
            return outputs.Last();
        }

        private async Task<Queue<long>> ProcessAndReturnOutputs(Queue<long>? inputs)
        {
            var codeInput = new Dictionary<long, long>(IntCodeProgram);
            var intCodeComputer = new IntCodeComputer(codeInput, inputs);
            await intCodeComputer.ProcessAsync();
            return intCodeComputer.Outputs;
        }
    }
}
