using AOC2019.IntCode;

namespace AOC2019.Day15
{
    internal class Day15PuzzleManager : PuzzleManager
    {
        public Dictionary<long, long> IntCodeProgram { get; private set; }

        public Day15PuzzleManager()
        {
            var inputHelper = new Day15InputHelper(INPUT_FILE_NAME);
            IntCodeProgram = inputHelper.Parse();
        }

        public async override Task SolveBothParts()
        {
            Console.WriteLine("");
            Console.WriteLine("Do you want to visualise the damaged section of the ship? y/n");
            var visualiseShipInput = Console.ReadKey();

            var codeInput = new Dictionary<long, long>(IntCodeProgram);
            var intCodeComputer = new IntCodeComputer(codeInput);
            var mapBuilder = new MapBuilder(intCodeComputer);
            intCodeComputer.ExternalInputs = mapBuilder.Outputs;

            var tasks = new Task[2];
            if (visualiseShipInput.KeyChar == 'y')
            {
                tasks[0] = mapBuilder.RunBothParts(printResult: true);
            }
            else
            {
                tasks[0] = mapBuilder.RunBothParts();
            }
            tasks[1] = intCodeComputer.ProcessAsync();

            await Task.WhenAll(tasks);
        }

        public async override Task SolvePartOne()
        {
            Console.WriteLine("");
            Console.WriteLine("Do you want to visualise the damaged section of the ship? y/n");
            var visualiseShipInput = Console.ReadKey();

            var codeInput = new Dictionary<long, long>(IntCodeProgram);
            var intCodeComputer = new IntCodeComputer(codeInput);
            var mapBuilder = new MapBuilder(intCodeComputer);
            intCodeComputer.ExternalInputs = mapBuilder.Outputs;

            var tasks = new Task[2];
            if (visualiseShipInput.KeyChar == 'y')
            {
                tasks[0] = mapBuilder.RunPartOne(printResult: true);
            }
            else
            {
                tasks[0] = mapBuilder.RunPartOne();
            }
            tasks[1] = intCodeComputer.ProcessAsync();

            await Task.WhenAll(tasks);
        }

        public async override Task SolvePartTwo()
        {
            Console.WriteLine("");
            Console.WriteLine("Do you want to visualise the damaged section of the ship? y/n");
            var visualiseShipInput = Console.ReadKey();

            var codeInput = new Dictionary<long, long>(IntCodeProgram);
            var intCodeComputer = new IntCodeComputer(codeInput);
            var mapBuilder = new MapBuilder(intCodeComputer);
            intCodeComputer.ExternalInputs = mapBuilder.Outputs;

            var tasks = new Task[2];
            if (visualiseShipInput.KeyChar == 'y')
            {
                tasks[0] = mapBuilder.RunPartTwo(printResult: true);
            }
            else
            {
                tasks[0] = mapBuilder.RunPartTwo();
            }
            tasks[1] = intCodeComputer.ProcessAsync();

            await Task.WhenAll(tasks);
        }
    }
}
