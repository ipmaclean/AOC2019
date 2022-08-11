using AOC2019.IntCode;

namespace AOC2019.Day21
{
    internal class Day21PuzzleManager : PuzzleManager
    {
        private bool _intCodeComputerAwaitingInput = false;
        private bool _intCodeComputerProgramHalted = false;

        public Dictionary<long, long> IntCodeProgram { get; private set; }

        public Day21PuzzleManager()
        {
            var inputHelper = new Day21InputHelper(INPUT_FILE_NAME);
            IntCodeProgram = inputHelper.Parse();
        }

        public override async Task SolveBothParts()
        {
            await SolvePartOne();
            Console.ReadKey();
            await SolvePartTwo();
        }

        public async override Task SolvePartOne()
        {
            await Solve(isPartOne: true);
        }

        public async override Task SolvePartTwo()
        {
            await Solve(isPartOne: false);
        }

        private async Task Solve(bool isPartOne)
        {
            var codeInput = new Dictionary<long, long>(IntCodeProgram);
            var intCodeComputer = new IntCodeComputer(codeInput);
            intCodeComputer.AwaitingInput += AwaitingInputHandler;
            intCodeComputer.ProgramHalted += ProgramHaltedHandler;
            intCodeComputer.ExternalInputs = new Queue<long>();
            var tasks = new Task[2];

            Console.Clear();
            var puzzlePart = isPartOne ? "One" : "Two";
            Console.WriteLine($"Part {puzzlePart}.");
            Console.WriteLine("Would you like to input your own instructions? (y/n)");
            Console.WriteLine("For Part One end commands with 'WALK', for Part Two end commands with 'RUN'.");
            var manualInput = Console.ReadKey();
            Console.WriteLine("");

            if (manualInput.KeyChar == 'y')
            {
                tasks[0] = Run(intCodeComputer, manualInputMode: true, isPartOne);
            }
            else
            {
                tasks[0] = Run(intCodeComputer, manualInputMode: false, isPartOne);
            }
            tasks[1] = intCodeComputer.ProcessAsync();

            await Task.WhenAll(tasks);
        }

        private async Task Run(IntCodeComputer intCodeComputer, bool manualInputMode, bool isPartOne)
        {
            if (manualInputMode)
            {
                Console.Clear();
            }
            var inputCounter = 0;
            string[] automaticInputs;
            if (isPartOne)
            {
                automaticInputs = new string[]
                {
                "NOT C J",
                "NOT A T",
                "OR T J",
                "AND D J",
                "WALK"
                };
            }
            else
            {
                automaticInputs = new string[]
                {
                "OR A T",
                "AND B T",
                "AND C T",
                "NOT T T",
                "AND D T",
                "AND H T",
                "OR A J",
                "AND B J",
                "AND C J",
                "NOT J J",
                "AND D J",
                "AND E J",
                "OR T J",
                "RUN"
                };
            }
            while (!_intCodeComputerProgramHalted)
            {
                while (intCodeComputer.Outputs.Count > 1 && !_intCodeComputerProgramHalted)
                {
                    await Task.Delay(50);
                    AsciiHelper.PrintAscii(intCodeComputer.Outputs);
                }
                if (_intCodeComputerAwaitingInput)
                {
                    if (manualInputMode)
                    {
                        AsciiHelper.ConvertUserInputAsciiToIntCodeInputAndProvideToIntCodeComputer(intCodeComputer.ExternalInputs!);
                    }
                    else
                    {
                        AsciiHelper.ConvertAsciiToIntCodeInputAndProvideToIntCodeComputer(intCodeComputer.ExternalInputs!, automaticInputs[inputCounter++]);
                    }
                    _intCodeComputerAwaitingInput = false;
                }
                await Task.Delay(5);
            }
            _intCodeComputerProgramHalted = false;
            Console.Clear();
            while (intCodeComputer.Outputs.Count > 1)
            {
                await Task.Delay(350);
                AsciiHelper.PrintAscii(intCodeComputer.Outputs);
            }
            if (intCodeComputer.Outputs.Count > 0)
            {
                var puzzlePart = isPartOne ? "one" : "two";
                Console.WriteLine($"The solution to part {puzzlePart} is '{intCodeComputer.Outputs.Dequeue()}'.");
            }
        }

        private void AwaitingInputHandler(object? sender, int e)
        {
            _intCodeComputerAwaitingInput = true;
        }

        private void ProgramHaltedHandler(object? sender, EventArgs e)
        {
            _intCodeComputerProgramHalted = true;
        }
    }
}
