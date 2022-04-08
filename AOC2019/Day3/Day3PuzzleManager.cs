namespace AOC2019.Day3
{
    internal class Day3PuzzleManager : PuzzleManager
    {
        private List<Instruction>[] WireInstructions { get; set; }
        private Dictionary<(int, int), PositionStatus> PositionsDictionary { get; set; } = new Dictionary<(int, int), PositionStatus>();
        public Day3PuzzleManager()
        {
            var inputHelper = new Day3InputHelper(INPUT_FILE_NAME);
            WireInstructions = inputHelper.Parse();
        }
        public override Task SolveBothParts()
        {
            SolvePrivate();
            var solution1 = PositionsDictionary.Where(x => x.Value.WireVisited[0] == true && x.Value.WireVisited[1] == true)
                .Select(x => Math.Abs(x.Key.Item1) + Math.Abs(x.Key.Item2))
                .Min();

            Console.WriteLine($"The solution to part one is '{solution1}'.");
            Console.WriteLine();
            var solution2 = PositionsDictionary.Where(x => x.Value.WireVisited[0] == true && x.Value.WireVisited[1] == true)
                .Select(x => Math.Abs(x.Value.WireSteps[0]) + Math.Abs(x.Value.WireSteps[1]))
                .Min();

            Console.WriteLine($"The solution to part two is '{solution2}'.");
            return Task.CompletedTask;
        }

        public override Task SolvePartOne()
        {
            SolvePrivate();

            var solution = PositionsDictionary.Where(x => x.Value.WireVisited[0] == true && x.Value.WireVisited[1] == true)
                .Select(x => Math.Abs(x.Key.Item1) + Math.Abs(x.Key.Item2))
                .Min();

            Console.WriteLine($"The solution to part one is '{solution}'.");
            return Task.CompletedTask;
        }

        private void SolvePrivate()
        {
            var currentWire = 0;
            foreach (var wireInstruction in WireInstructions)
            {
                var currentPosition = (0, 0);
                var currentSteps = 0;
                foreach (var instruction in wireInstruction)
                {
                    (currentPosition, currentSteps) = CompleteInstruction(currentPosition, currentSteps, instruction, currentWire);
                }
                currentWire++;
            }
            
        }

        public override Task SolvePartTwo()
        {
            SolvePrivate();

            var solution = PositionsDictionary.Where(x => x.Value.WireVisited[0] == true && x.Value.WireVisited[1] == true)
                .Select(x => x.Value.WireSteps[0] + x.Value.WireSteps[1])
                .Min();

            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
        }

        public override Task Reset()
        {
            var inputHelper = new Day3InputHelper(INPUT_FILE_NAME);
            WireInstructions = inputHelper.Parse();
            PositionsDictionary = new Dictionary<(int, int), PositionStatus>();
            return Task.CompletedTask;
        }

        private ((int, int), int) CompleteInstruction((int, int) currentPosition, int currentSteps, Instruction instruction, int currentWire)
        {
            for (var i = 1; i <= instruction.Distance; i++)
            {
                currentSteps++;
                switch (instruction.Direction)
                {
                    case 'U':
                        currentPosition.Item2++;
                        break;
                    case 'D':
                        currentPosition.Item2--;
                        break;
                    case 'R':
                        currentPosition.Item1++;
                        break;
                    case 'L':
                        currentPosition.Item1--;
                        break;
                    default:
                        throw new ArgumentException("Malformed instruction.");
                }
                if (PositionsDictionary.ContainsKey(currentPosition))
                {
                    PositionsDictionary[currentPosition].WireVisited[currentWire] = true;
                    PositionsDictionary[currentPosition].WireSteps[currentWire] = Math.Min(currentSteps, PositionsDictionary[currentPosition].WireSteps[currentWire]);
                }
                else
                {
                    var positionStatus = new PositionStatus();
                    positionStatus.WireVisited[currentWire] = true;
                    positionStatus.WireSteps[currentWire] = currentSteps;
                    PositionsDictionary.Add(currentPosition, positionStatus);
                }
            }
            return (currentPosition, currentSteps);
        }
    }
}
