using System.Text.RegularExpressions;

namespace AOC2019.Day4
{
    internal class Day4PuzzleManager : PuzzleManager
    {
        private int FirstNumber { get; set; }
        private int LastNumber { get; set; }
        public Day4PuzzleManager()
        {
            var inputHelper = new Day4InputHelper(INPUT_FILE_NAME);
            (FirstNumber, LastNumber) = inputHelper.Parse();
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
            var doubleRegex = new Regex(@"(.)\1");

            Console.WriteLine($"The solution to part one is '{Solve(doubleRegex)}'.");
            return Task.CompletedTask;
        }

        public override Task SolvePartTwo()
        {
            var exactlyDoubleRegex = new Regex(@"(.)(?<!\1\1)\1(?!\1)");

            Console.WriteLine($"The solution to part two is '{Solve(exactlyDoubleRegex)}'.");
            return Task.CompletedTask;
        }

        private int Solve(Regex regex)
        {
            var solution = 0;
            for (var i = FirstNumber; i <= LastNumber; i++)
            {
                var numberAsString = i.ToString();
                if (regex.IsMatch(numberAsString) && DigitsAreIncreasing(numberAsString))
                {
                    solution++;
                }
            }
            return solution;
        }

        // Using this for part two (and something similar for part one) is actually much quicker than regex but I liked the reusability of the code.
        private bool ExactlyDoubleExists(string numberAsString)
        {
            var count = 1;
            var lastDigit = numberAsString[0];
            for (var i = 1; i < numberAsString.Length; i++)
            {
                var currentDigit = numberAsString[i];
                if (currentDigit == lastDigit)
                {
                    count++;
                }
                else
                {
                    if (count == 2)
                    {
                        return true;
                    }
                    count = 1;
                }
                lastDigit = currentDigit;
            }
            if (count == 2)
            {
                return true;
            }
            return false;
        }

        private bool DigitsAreIncreasing(string numberAsString)
        {
            var leftDigit = 0;
            foreach (var digit in numberAsString)
            {
                var rightDigit = digit - '0';
                if (rightDigit < leftDigit)
                {
                    return false;
                }
                leftDigit = rightDigit;
            }
            return true;
        }
    }
}
