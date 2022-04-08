using AOC2019.IntCode;

namespace AOC2019.Day2
{
    public class Day2PuzzleManager : PuzzleManager
    {
        public int[] IntCodeProgram { get; private set; }
        public Day2PuzzleManager()
        {
            var inputHelper = new Day2InputHelper(INPUT_FILE_NAME);
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
            var codeOutput = await SolvePartOnePrivateAsync(12, 2);
            Console.WriteLine($"The solution to part one is '{codeOutput}'.");
        }

        public async Task<int> SolvePartOnePrivateAsync(int noun, int verb)
        {
            var codeInput = (int[])IntCodeProgram.Clone();

            codeInput[1] = noun;
            codeInput[2] = verb;

            var intCodeComputer = new IntCodeComputer(codeInput);
            return (await intCodeComputer.ProcessAsync())[0];
        }

        public async override Task SolvePartTwo()
        {
            var solution = await SolvePartTwoPrivateAsync();
            Console.WriteLine($"The solution to part two is '{solution}'.");
        }

        public async Task<int> SolvePartTwoPrivateAsync()
        {
            for (var noun = 0; noun < 100; noun++)
            {
                for (var verb = 0; verb < 100; verb++)
                {
                    var codeOutput = await SolvePartOnePrivateAsync(noun, verb);
                    if (codeOutput == 19690720)
                    {
                        return noun * 100 + verb;
                    }
                }
            }
            return 0;
        }
    }
}
