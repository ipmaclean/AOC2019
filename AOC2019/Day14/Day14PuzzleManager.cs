namespace AOC2019.Day14
{
    internal class Day14PuzzleManager : PuzzleManager
    {
        public List<Chemical> Chemicals { get; private set; }

        public Day14PuzzleManager()
        {
            var inputHelper = new Day14InputHelper(INPUT_FILE_NAME);
            Chemicals = inputHelper.Parse();
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
            var chemicalStockpile = new Dictionary<Chemical, long>();
            var fuel = Chemicals.First(x => x.Name == "FUEL");
            var oreForOneFuel = fuel.CountParents(1, chemicalStockpile);

            Console.WriteLine($"The solution to part one is '{oreForOneFuel}'.");
            return Task.CompletedTask;
        }

        public override Task SolvePartTwo()
        {
            var chemicalStockpile = new Dictionary<Chemical, long>();
            var fuel = Chemicals.First(x => x.Name == "FUEL");
            var oreForOneFuel = fuel.CountParents(1, chemicalStockpile);

            var oneTrillion = 1_000_000_000_000;
            var maxFuelPossible = oneTrillion / oreForOneFuel;

            long iterator = 100_000;

            while (iterator > 0)
            {
                while (fuel.CountParents(maxFuelPossible + iterator, chemicalStockpile: new Dictionary<Chemical, long>()) < oneTrillion)
                {
                    maxFuelPossible += iterator;
                }
                iterator /= 10;
            }
            Console.WriteLine($"The solution to part two is '{maxFuelPossible}'.");
            return Task.CompletedTask;
        }
    }
}
