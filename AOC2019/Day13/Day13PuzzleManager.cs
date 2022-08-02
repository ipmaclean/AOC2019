using AOC2019.IntCode;

namespace AOC2019.Day13
{
    public class Day13PuzzleManager : PuzzleManager
    {
        public Dictionary<long, long> IntCodeProgram { get; private set; }

        public Day13PuzzleManager()
        {
            var inputHelper = new Day13InputHelper(INPUT_FILE_NAME);
            IntCodeProgram = inputHelper.Parse();
        }

        public override void Reset()
        {
            var inputHelper = new Day13InputHelper(INPUT_FILE_NAME);
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
            var intCodeComputer = new IntCodeComputer(IntCodeProgram);
            var screen = new Screen(intCodeComputer);

            await intCodeComputer.ProcessAsync();
            return screen.RunPartOne();
        }

        public async override Task SolvePartTwo()
        {
            Console.WriteLine("");
            Console.WriteLine("Do you want to play the game? y/n");
            var playGameInput = Console.ReadKey();

            var codeInput = new Dictionary<long, long>(IntCodeProgram);
            codeInput[0] = 2;
            var intCodeComputer = new IntCodeComputer(codeInput);
            var screen = new Screen(intCodeComputer);
            intCodeComputer.ExternalInputs = screen.Outputs;

            var screenCancellationTokenSource = new CancellationTokenSource();
            var tasks = new Task[2];
            tasks[0] = screen.RunPartTwo(screenCancellationTokenSource.Token);
            if (playGameInput.KeyChar == 'y')
            {
                tasks[1] = intCodeComputer.ProcessAsync(manualInputMode: true);
            }
            else
            {
                tasks[1] = intCodeComputer.ProcessAsync();
            }
            

            await Task.WhenAny(tasks);
            screenCancellationTokenSource.Cancel();
            await Task.WhenAll(tasks);
        }
    }
}
