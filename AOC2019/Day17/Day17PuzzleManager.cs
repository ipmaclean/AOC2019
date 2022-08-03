using AOC2019.IntCode;

namespace AOC2019.Day17
{
    internal class Day17PuzzleManager : PuzzleManager
    {
        private const int CHARACTERS_IN_VIDEO_FEED = 2347;

        public Dictionary<long, long> IntCodeProgram { get; private set; }

        public Day17PuzzleManager()
        {
            var inputHelper = new Day17InputHelper(INPUT_FILE_NAME);
            IntCodeProgram = inputHelper.Parse();
        }

        public async override Task SolveBothParts()
        {
            await SolvePartOne();
            Console.WriteLine("Press the Enter key to continue.");
            Console.ReadKey();
            await SolvePartTwo();
            await Task.CompletedTask;
        }

        public async override Task SolvePartOne()
        {
            Console.WriteLine("Would you like to see the video feed? (y/n)");
            var videoFeedInput = Console.ReadKey();
            Console.WriteLine("");

            var codeInput = new Dictionary<long, long>(IntCodeProgram);
            var intCodeComputer = new IntCodeComputer(codeInput);
            await intCodeComputer.ProcessAsync();
            var asciiHelper = new AsciiHelper();
            var mapAsCharacters = asciiHelper.CharListAscii(intCodeComputer.Outputs);
            var solution = GetIntersectionValues(mapAsCharacters);

            if (videoFeedInput.KeyChar == 'y')
            {
                codeInput = new Dictionary<long, long>(IntCodeProgram);
                intCodeComputer = new IntCodeComputer(codeInput);
                await intCodeComputer.ProcessAsync();
                Console.Clear();
                asciiHelper.PrintAscii(intCodeComputer.Outputs);
            }

            Console.WriteLine($"The solution to part one is '{solution}'.");
        }

        private int GetIntersectionValues(List<List<char>> mapAsCharacters)
        {
            var intersectionSum = 0;
            for (var yCoord = 0; yCoord < mapAsCharacters.Count; yCoord++)
            {
                var line = mapAsCharacters[yCoord];
                for (var xCoord = 0; xCoord < line.Count; xCoord++)
                {
                    intersectionSum += GetIntersectionValue(xCoord, yCoord, mapAsCharacters);
                }
            }
            return intersectionSum;
        }

        private int GetIntersectionValue(int xCoord, int yCoord, List<List<char>> mapAsCharacters)
        {
            if (xCoord <= 0 || yCoord <= 0 || xCoord >= mapAsCharacters[0].Count - 1 || yCoord >= mapAsCharacters.Count - 2)
            {
                return 0;
            }
            if (mapAsCharacters[yCoord][xCoord] == '.')
            {
                return 0;
            }
            if (mapAsCharacters[yCoord][xCoord] == '#' &&
                mapAsCharacters[yCoord + 1][xCoord] == '#' &&
                mapAsCharacters[yCoord - 1][xCoord] == '#' &&
                mapAsCharacters[yCoord][xCoord + 1] == '#' &&
                mapAsCharacters[yCoord][xCoord - 1] == '#')
            {
                return xCoord * yCoord;
            }
            return 0;
        }

        // I did a lot of part two by hand. First, from the printout of the map, I worked out the commands that the robot would need to reach the end.
        // Next, I worked out the common groups of commands that appeared and labeled them as A, B and C.
        // You can see these commands below on lines 117-120.
        // If you want to try your own commands, you can.
        public async override Task SolvePartTwo()
        {
            var codeInput = new Dictionary<long, long>(IntCodeProgram);
            var intCodeComputer = new IntCodeComputer(codeInput);
            await intCodeComputer.ProcessAsync();
            var asciiHelper = new AsciiHelper();
            Console.Clear();
            asciiHelper.PrintAscii(intCodeComputer.Outputs);

            var intCodeComputerInputs = new Queue<long>();

            Console.WriteLine("Would you like to input your own instructions? (y/n)");
            var manualInput = Console.ReadKey();
            Console.WriteLine("");

            if (manualInput.KeyChar == 'y')
            {
                Console.WriteLine("Input main movement routine.");
                ConvertUserInputAsciiToIntCodeInputAndProvideToIntCodeComputer(intCodeComputerInputs);
                Console.WriteLine("Input movement function A.");
                ConvertUserInputAsciiToIntCodeInputAndProvideToIntCodeComputer(intCodeComputerInputs);
                Console.WriteLine("Input movement function B.");
                ConvertUserInputAsciiToIntCodeInputAndProvideToIntCodeComputer(intCodeComputerInputs);
                Console.WriteLine("Input movement function C.");
                ConvertUserInputAsciiToIntCodeInputAndProvideToIntCodeComputer(intCodeComputerInputs);
            }
            else
            {
                ConvertAsciiToIntCodeInputAndProvideToIntCodeComputer(intCodeComputerInputs, "A,C,A,C,B,B,C,B,C,A");
                ConvertAsciiToIntCodeInputAndProvideToIntCodeComputer(intCodeComputerInputs, "R,12,L,8,R,12");
                ConvertAsciiToIntCodeInputAndProvideToIntCodeComputer(intCodeComputerInputs, "R,8,L,8,R,8,R,4,R,4");
                ConvertAsciiToIntCodeInputAndProvideToIntCodeComputer(intCodeComputerInputs, "R,8,R,6,R,6,R,8");
            }

            Console.WriteLine("Would you like to see the continuous video feed? (y/n)");
            var continuousVideoFeedInput = Console.ReadKey();
            Console.WriteLine("");
            intCodeComputerInputs.Enqueue(continuousVideoFeedInput.KeyChar);
            intCodeComputerInputs.Enqueue(10);

            codeInput = new Dictionary<long, long>(IntCodeProgram);
            codeInput[0] = 2;
            intCodeComputer = new IntCodeComputer(codeInput);
            intCodeComputer.ExternalInputs = intCodeComputerInputs;

            Console.Clear();
            await intCodeComputer.ProcessAsync();
            while (intCodeComputer.Outputs.Count > 1)
            {
                await Task.Delay(50);
                asciiHelper.PrintAscii(intCodeComputer.Outputs);
            }
            var solution = intCodeComputer.Outputs.Dequeue();
            Console.WriteLine($"The solution to part two is '{solution}'.");
        }

        private void ConvertAsciiToIntCodeInputAndProvideToIntCodeComputer(Queue<long> intCodeComputerInputs, string asciiInput)
        {
            foreach (var character in asciiInput)
            {
                intCodeComputerInputs.Enqueue(character);
            }
            intCodeComputerInputs.Enqueue(10);
        }

        private void ConvertUserInputAsciiToIntCodeInputAndProvideToIntCodeComputer(Queue<long> intCodeComputerInputs)
        {
            var input = Console.ReadLine() ?? string.Empty;
            var asciiCharString = string.Empty;
            foreach (var character in input)
            {
                intCodeComputerInputs.Enqueue(character);
            }
            intCodeComputerInputs.Enqueue(10);
        }
    }
}
