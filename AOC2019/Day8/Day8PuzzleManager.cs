namespace AOC2019.Day8
{
    internal class Day8PuzzleManager : PuzzleManager
    {
        public List<List<List<int>>> Input { get; private set; }

        public Day8PuzzleManager()
        {
            var inputHelper = new Day8InputHelper(INPUT_FILE_NAME);
            Input = inputHelper.Parse();
        }

        public override Task SolveBothParts()
        {
            SolvePartOne();
            Console.WriteLine();
            SolvePartTwo();
            return Task.CompletedTask;
        }

        public override Task SolvePartOne()
        {
            int layerWithFewestZeroes = FindLayerWithFewestZeroes(Input);
            int solution = SolutionToPart1(Input, layerWithFewestZeroes);
            Console.WriteLine($"The solution to part one is '{solution}'.");
            return Task.CompletedTask;
        }

        private static int SolutionToPart1(List<List<List<int>>> input, int layerWithFewestZeroes)
        {
            int countOfOnes = 0;
            int countOfTwos = 0;
            List<List<int>> layer = input[layerWithFewestZeroes];

            foreach (List<int> row in layer)
            {
                countOfOnes += row.Count(x => x == 1);
                countOfTwos += row.Count(x => x == 2);
            }
            return countOfOnes * countOfTwos;
        }

        private static int FindLayerWithFewestZeroes(List<List<List<int>>> input)
        {
            int layerCounter = 0;
            int minZeroCount = int.MaxValue;
            int minZeroLayer = 0;
            foreach (List<List<int>> layer in input)
            {
                int tempZeroCount = 0;
                foreach (List<int> row in layer)
                {
                    tempZeroCount += row.Where(x => x == 0).Count();
                }

                if (minZeroCount > tempZeroCount)
                {
                    minZeroCount = tempZeroCount;
                    minZeroLayer = layerCounter;
                }

                layerCounter++;
            }
            return minZeroLayer;
        }

        public override Task SolvePartTwo()
        {
            var pixelLayer = GeneratePixelLayer(Input);
            Console.WriteLine("The solution to part two is below:");
            Console.WriteLine();
            foreach (List<string> row in pixelLayer)
            {
                Console.WriteLine(string.Join("", row));
            }
            return Task.CompletedTask;
        }

        private static List<List<string>> GeneratePixelLayer(List<List<List<int>>> input)
        {
            // x and y are coordinates of a pixel on a layer
            List<List<string>> pixelLayer = new List<List<string>>();
            List<string> pixelRow = new List<string>();

            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 25; x++)
                {
                    string colour = GetColour(input, x, y);
                    pixelRow.Add(colour);
                }
                pixelLayer.Add(pixelRow);
                pixelRow = new List<string>();
            }

            return pixelLayer;
        }

        private static string GetColour(List<List<List<int>>> input, int x, int y)
        {
            int layerCount = 0;

            while (layerCount < input.Count)
            {
                if (input[layerCount][y][x] == 0)
                {
                    return " ";
                }
                if (input[layerCount][y][x] == 1)
                {
                    return "#";
                }
                else
                {
                    layerCount++;
                }
            }
            return "x";
        }
    }
}
