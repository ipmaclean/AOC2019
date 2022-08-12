using AOC2019.IntCode;

namespace AOC2019.Day25
{
    internal class Day25PuzzleManager : PuzzleManager
    {
        private bool _intCodeComputerAwaitingInput = false;
        private bool _intCodeComputerProgramHalted = false;

        public Dictionary<long, long> IntCodeProgram { get; private set; }

        public Day25PuzzleManager()
        {
            var inputHelper = new Day25InputHelper(INPUT_FILE_NAME);
            IntCodeProgram = inputHelper.Parse();
        }

        public override async Task SolveBothParts()
        {
            await SolvePartOne();
            Console.ReadKey();
            await SolvePartTwo();
        }

        // For this one, make sure you collect the:
        //  astronaut ice cream
        //  dark matter
        //  weather machine
        //  easter egg
        public async override Task SolvePartOne()
        {
            var codeInput = new Dictionary<long, long>(IntCodeProgram);
            var intCodeComputer = new IntCodeComputer(codeInput);
            intCodeComputer.AwaitingInput += AwaitingInputHandler;
            intCodeComputer.ProgramHalted += ProgramHaltedHandler;
            intCodeComputer.ExternalInputs = new Queue<long>();
            var tasks = new Task[2];


            tasks[0] = Run(intCodeComputer, manualInputMode: true);
            tasks[1] = intCodeComputer.ProcessAsync();

            await Task.WhenAll(tasks);
        }

        public override Task SolvePartTwo()
        {
            Console.WriteLine("There is no part two! Congratulations on saving Santa.");
            return Task.CompletedTask;
        }

        private async Task Run(IntCodeComputer intCodeComputer, bool manualInputMode)
        {
            if (manualInputMode)
            {
                Console.Clear();
            }
            while (!_intCodeComputerProgramHalted)
            {
                while (intCodeComputer.Outputs.Count > 0 && !_intCodeComputerProgramHalted)
                {
                    AsciiHelper.PrintAscii(intCodeComputer.Outputs, false);
                }
                if (_intCodeComputerAwaitingInput)
                {
                    if (manualInputMode)
                    {
                        AsciiHelper.ConvertUserInputAsciiToIntCodeInputAndProvideToIntCodeComputer(intCodeComputer.ExternalInputs!);
                    }
                    else
                    {
                        //AsciiHelper.ConvertAsciiToIntCodeInputAndProvideToIntCodeComputer(intCodeComputer.ExternalInputs!, automaticInputs[inputCounter++]);
                    }
                    _intCodeComputerAwaitingInput = false;
                }
                await Task.Delay(5);
            }
            _intCodeComputerProgramHalted = false;
            while (intCodeComputer.Outputs.Count > 0)
            {
                AsciiHelper.PrintAscii(intCodeComputer.Outputs, false);
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
