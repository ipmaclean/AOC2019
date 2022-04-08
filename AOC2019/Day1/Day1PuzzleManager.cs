namespace AOC2019.Day1
{
    public class Day1PuzzleManager : PuzzleManager
    {
        public List<int> Masses { get; private set; }
        public Day1PuzzleManager()
        {
            var inputHelper = new Day1InputHelper(INPUT_FILE_NAME);
            Masses = inputHelper.Parse();
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
            var solution = 0;
            foreach (var mass in Masses)
            {
                solution += CalculateFuel(mass);
            }
            Console.WriteLine($"The solution to part one is '{solution}'.");
            return Task.CompletedTask;
        }

        public override Task SolvePartTwo()
        {
            var solution = 0;
            foreach (var mass in Masses)
            {
                var currentMass = mass;
                while (currentMass > 8)
                {
                    var fuelForThisMass = CalculateFuel(currentMass);
                    solution += fuelForThisMass;
                    currentMass = fuelForThisMass;
                }
            }
            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
        }

        private int CalculateFuel(int mass)
        {
            return (mass / 3) - 2;
        }
    }
}
