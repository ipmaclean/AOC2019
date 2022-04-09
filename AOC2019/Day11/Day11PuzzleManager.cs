using AOC2019.IntCode;

namespace AOC2019.Day11
{
    public class Day11PuzzleManager : PuzzleManager
    {
        public Dictionary<long, long> IntCodeProgram { get; private set; }

        public Day11PuzzleManager()
        {
            var inputHelper = new Day11InputHelper(INPUT_FILE_NAME);
            IntCodeProgram = inputHelper.Parse();
        }

        public override void Reset()
        {
            var inputHelper = new Day11InputHelper(INPUT_FILE_NAME);
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
            var paintingRobot = new PaintingRobot();
            var intCodeComputer = new IntCodeComputer(IntCodeProgram, externalInputs: paintingRobot.CurrentColourOutput);
            paintingRobot.ExternalInputs = intCodeComputer.Outputs;

            var robotCancellationTokenSource = new CancellationTokenSource();
            var tasks = new Task<int>[2];
            tasks[0] = paintingRobot.RunRobot(robotCancellationTokenSource.Token);
            tasks[1] = IntCodeComputerProcessAsyncWrapper(intCodeComputer);

            await Task.WhenAny(tasks);
            robotCancellationTokenSource.Cancel();
            await Task.WhenAll(tasks);

            return tasks[0].Result;
        }

        private async Task<int> IntCodeComputerProcessAsyncWrapper(IntCodeComputer intCodeComputer)
        {
            await intCodeComputer.ProcessAsync();
            return 0;
        }

        public async override Task SolvePartTwo()
        {
            var paintingRobot = new PaintingRobot();
            var intCodeComputer = new IntCodeComputer(IntCodeProgram, externalInputs: paintingRobot.CurrentColourOutput);
            paintingRobot.ExternalInputs = intCodeComputer.Outputs;

            var robotCancellationTokenSource = new CancellationTokenSource();
            var tasks = new Task[2];
            tasks[0] = paintingRobot.RunRobot(robotCancellationTokenSource.Token, isPartTwo: true);
            tasks[1] = intCodeComputer.ProcessAsync();

            await Task.WhenAny(tasks);
            robotCancellationTokenSource.Cancel();
            await Task.WhenAll(tasks);

            Console.WriteLine();
            paintingRobot.PrintResult();
            Console.WriteLine();
            Console.WriteLine($"The solution to part two is above.");
        }
    }
}
