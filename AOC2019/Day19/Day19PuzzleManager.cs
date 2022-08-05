using AOC2019.IntCode;
using System.Text;

namespace AOC2019.Day19
{
    public class Day19PuzzleManager : PuzzleManager
    {
        public Dictionary<long, long> IntCodeProgram { get; private set; }

        public Day19PuzzleManager()
        {
            var inputHelper = new Day19InputHelper(INPUT_FILE_NAME);
            IntCodeProgram = inputHelper.Parse();
        }

        public override async Task SolveBothParts()
        {
            await SolvePartOne();
            await SolvePartTwo();
        }

        public async override Task SolvePartOne()
        {
            Console.WriteLine("");
            Console.WriteLine("Do you want to print the tractor beam? y/n");
            var printTractorBeamInput = Console.ReadKey();
            Console.WriteLine("");
            var printTractorBeam = false;
            if (printTractorBeamInput.KeyChar == 'y')
            {
                printTractorBeam = true;
            }
            var solution = await SolvePartOnePrivateAsync(printTractorBeam);
            Console.WriteLine($"The solution to part one is '{solution}'.");
        }

        public async Task<int> SolvePartOnePrivateAsync(bool printTractorBeam)
        {

            var solution = 0;
            var tiles = new Dictionary<(long, long), long>();
            for (var xCoord = 0L; xCoord < 50; xCoord++)
            {
                for (var yCoord = 0L; yCoord < 50; yCoord++)
                {
                    var codeInput = new Dictionary<long, long>(IntCodeProgram);
                    var inputs = new Queue<long>();
                    inputs.Enqueue(xCoord);
                    inputs.Enqueue(yCoord);
                    var intCodeComputer = new IntCodeComputer(codeInput, inputs);
                    await intCodeComputer.ProcessAsync();
                    var output = intCodeComputer.Outputs.Dequeue();
                    if (output == 1)
                    {
                        solution++;
                    }
                    tiles.Add((xCoord, yCoord), output);
                }
            }
            if (printTractorBeam)
            {
                PrintResult(tiles);
            }
            return solution;
        }

        private void PrintResult(Dictionary<(long, long), long> tiles)
        {
            var highestX = tiles.Max(x => x.Key.Item1);
            var highestY = tiles.Max(x => x.Key.Item2);

            var canvas = new char[highestX + 1, highestY + 1];

            foreach (var tile in tiles)
            {
                canvas[tile.Key.Item1, tile.Key.Item2] = tile.Value == 1 ? '#' : '.';
            }

            var sb = new StringBuilder();
            sb.Append(Environment.NewLine);
            for (var j = 0; j <= highestY; j++)
            {
                for (var i = 0; i <= highestX; i++)
                {
                    sb.Append(canvas[i, j]);
                    sb.Append(' ');
                }
                sb.Append(Environment.NewLine);
            }

            Console.Clear();
            Console.Write(sb.ToString());
        }

        public override Task SolvePartTwo()
        {
            // For part two, I didn't solve programmatically. I printed out a large square (I chose 1250x1250 but it doesn't need to be quite that big - 918x1122 would have been just enough for me) and then examined it in Excel.
            // I noticed that the x-coordinate of the top left of the first nxn square that fits in the beam is the first column that has a height of 2n-1.
            // The correct y-coordinate is then n-1 tiles up from the lowermost active beam tile with the correct x-coordinate.

            // I may write a program at some point using these insights but it didn't seem worth doing for the sake of this puzzle, especially with part one being so easy.

            Console.WriteLine($"The solution to part one is '9181022'.");
            return Task.CompletedTask;
        }
    }
}
