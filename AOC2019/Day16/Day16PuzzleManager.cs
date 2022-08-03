namespace AOC2019.Day16
{
    internal class Day16PuzzleManager : PuzzleManager
    {
        public List<int> PartOneInput { get; set; }
        public int[] FftPattern { get; set; } = new int[] { 0, 1, 0, -1 };

        private const int NUMBER_OF_FFT_PHASES = 100;

        public Day16PuzzleManager()
        {
            var inputHelper = new Day16InputHelper(INPUT_FILE_NAME);
            PartOneInput = inputHelper.Parse();
        }

        public override Task SolveBothParts()
        {
            SolvePartOne();
            SolvePartTwo();
            return Task.CompletedTask;
        }

        public override Task SolvePartOne()
        {
            var output = RunFftAlgorithmPartOne(PartOneInput);
            var solution = string.Empty;
            for (var i = 0; i < 8; i++)
            {
                solution += output[i];
            }
            Console.WriteLine($"The solution to part one is '{solution}'.");
            return Task.CompletedTask;
        }

        private List<int> RunFftAlgorithmPartOne(List<int> input)
        {
            var output = input;
            for (var i = 0; i < NUMBER_OF_FFT_PHASES; i++)
            {
                output = RunPhasePartOne(output);
            }
            return output;
        }

        private List<int> RunPhasePartOne(List<int> input)
        {
            var output = new List<int>();

            for (var outputPosition = 0; outputPosition < input.Count; outputPosition++)
            {
                var sum = 0;
                for (var inputPosition = 0; inputPosition < input.Count; inputPosition++)
                {
                    sum += input[inputPosition] * GetFftPatternMultiplier(outputPosition, inputPosition);
                }
                output.Add(Math.Abs(sum) % 10);
            }

            return output;
        }

        private int GetFftPatternMultiplier(int outputPosition, int inputPosition)
        {
            var test = FftPattern[((inputPosition + 1) / (outputPosition + 1)) % FftPattern.Length];
            return test;
        }

        public override Task SolvePartTwo()
        {
            var offsetString = string.Empty;
            for (var i = 0; i < 7; i++)
            {
                offsetString += PartOneInput[i];
            }
            var offset = int.Parse(offsetString);

            var partTwoInput = new List<int>();
            for (var i = 0; i < 10_000; i++)
            {
                partTwoInput.AddRange(PartOneInput);
            }

            partTwoInput = partTwoInput.Skip(offset).ToList();

            var output = RunFftAlgorithmPartTwo(partTwoInput);

            var solution = string.Empty;
            for (var i = 0; i < 8; i++)
            {
                solution += output[i];
            }
            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
        }

        private List<int> RunFftAlgorithmPartTwo(List<int> input)
        {
            var output = input;
            for (var i = 0; i < NUMBER_OF_FFT_PHASES; i++)
            {
                output = RunPhasePartTwo(output);
            }
            return output;
        }

        private List<int> RunPhasePartTwo(List<int> input)
        {
            var output = new int[input.Count];
            var backwardsSum = 0;
            for (var outputPosition = input.Count - 1; outputPosition >= 0; outputPosition--)
            {
                backwardsSum += input[outputPosition];
                backwardsSum = backwardsSum % 10;
                output[outputPosition] = backwardsSum;
            }
            return output.ToList();
        }
    }
}
