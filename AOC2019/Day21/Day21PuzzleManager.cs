using AOC2019.IntCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public override Task SolveBothParts()
        {
            throw new NotImplementedException();
        }

        public async override Task SolvePartOne()
        {
            var codeInput = new Dictionary<long, long>(IntCodeProgram);
            var intCodeComputer = new IntCodeComputer(codeInput);
            intCodeComputer.AwaitingInput += AwaitingInputHandler;
            intCodeComputer.ProgramHalted += ProgramHaltedHandler;
            intCodeComputer.ExternalInputs = new Queue<long>();
            var tasks = new Task[2];

            Console.Clear();
            Console.WriteLine("Would you like to input your own instructions? (y/n)");
            var manualInput = Console.ReadKey();
            Console.WriteLine("");

            if (manualInput.KeyChar == 'y')
            {
                tasks[0] = RunPartOne(intCodeComputer, true);
            }
            else
            {
                tasks[0] = RunPartOne(intCodeComputer, false);
            }
            tasks[1] = intCodeComputer.ProcessAsync();

            await Task.WhenAll(tasks);
        }

        private async Task RunPartOne(IntCodeComputer intCodeComputer, bool manualInputMode)
        {
            if (manualInputMode)
            {
                Console.Clear();
            }
            var inputCounter = 0;
            var automaticInputs = new string[]
            {
                "NOT C J",
                "NOT A T",
                "OR T J",
                "AND D J",
                "WALK"
            };
            while (!_intCodeComputerProgramHalted)
            {
                while (intCodeComputer.Outputs.Count > 1)
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
                Console.WriteLine($"The solution to part one is '{intCodeComputer.Outputs.Dequeue()}'.");
            }
        }

        public override Task SolvePartTwo()
        {
            throw new NotImplementedException();
        }

        private void AwaitingInputHandler(object? sender, EventArgs e)
        {
            _intCodeComputerAwaitingInput = true;
        }

        private void ProgramHaltedHandler(object? sender, EventArgs e)
        {
            _intCodeComputerProgramHalted = true;
        }
    }
}
