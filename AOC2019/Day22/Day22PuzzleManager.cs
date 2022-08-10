namespace AOC2019.Day22
{
    internal class Day22PuzzleManager : PuzzleManager
    {
        public List<Command> Commands { get; set; }
        public (long Offset, long Increment) DeckMakeup { get; set; } = (0, 0);

        private const long _deckSizePartOne = 10_007;
        private const long _deckSizePartTwo = 119_315_717_514_047;
        public Day22PuzzleManager()
        {
            var inputHelper = new Day22InputHelper(INPUT_FILE_NAME);
            Commands = inputHelper.Parse();
        }

        public override Task SolveBothParts()
        {
            throw new NotImplementedException();
        }

        public override Task SolvePartOne()
        {
            DeckMakeup = (0, 1);
            foreach (var command in Commands)
            {
                ExecuteCommand(command, _deckSizePartOne);
            }
            var index = 0;
            var offset = DeckMakeup.Offset;
            while (offset != 2019)
            {
                offset = Mod(offset + DeckMakeup.Increment, _deckSizePartOne);
                index++;
            }
            var solution = index;
            Console.WriteLine($"The solution to part one is '{solution}'.");
            return Task.CompletedTask;
        }

        private void ExecuteCommand(Command command, long deckSize)
        {
            switch (command.Description)
            {
                case CommandDescription.DealIntoNewStack:
                    DealIntoNewStack(deckSize);
                    break;
                case CommandDescription.DealWithIncrement:
                    DealWithIncrement(command.Value, deckSize);
                    break;
                case CommandDescription.Cut:
                    Cut(command.Value, deckSize);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("The space deck shuffle command was not valid.");
            }

        }

        private void DealIntoNewStack(long deckSize)
        {
            var newIncrement = DeckMakeup.Increment * -1;
            var newOffset = Mod(DeckMakeup.Offset + newIncrement, deckSize);
            DeckMakeup = (newOffset, newIncrement);
        }

        private void DealWithIncrement(long commandValue, long deckSize)
        {
            var commandValueInverse = ModInverse(commandValue, deckSize);
            var newIncrement = Mod(DeckMakeup.Increment * commandValueInverse, deckSize);
            DeckMakeup = (DeckMakeup.Offset, newIncrement);
        }

        private void Cut(long commandValue, long deckSize)
        {
            var newOffset = Mod(DeckMakeup.Offset + DeckMakeup.Increment * commandValue, deckSize);
            DeckMakeup = (newOffset, DeckMakeup.Increment);
        }

        public override Task SolvePartTwo()
        {
            throw new NotImplementedException();
        }

        private long ModInverse(long a, long m)
        {

            for (long x = 1; x < m; x++)
                if (((a % m) * (x % m)) % m == 1)
                    return x;
            return 1;
        }

        private long Mod(long x, long m)
        {
            return (x % m + m) % m;
        }
    }
}
