namespace AOC2019.Day6
{
    internal class Day6PuzzleManager : PuzzleManager
    {
        public List<Planet> Planets { get; private set; }

        public Day6PuzzleManager()
        {
            var inputHelper = new Day6InputHelper(INPUT_FILE_NAME);
            Planets = inputHelper.Parse();
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
            foreach (var planet in Planets)
            {
                solution += planet.CountParents();
            }
            Console.WriteLine($"The solution to part one is '{solution}'.");
            return Task.CompletedTask;
        }

        public override Task SolvePartTwo()
        {
            var myPlanet = Planets.First(x => x.Name == "YOU");
            var santaPlanet = Planets.First(x => x.Name == "SAN");

            var solution = myPlanet.FindOrbitalTransfersBetween(santaPlanet);
            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
        }
    }
}
