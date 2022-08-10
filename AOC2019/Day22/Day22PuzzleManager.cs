namespace AOC2019.Day22
{
    internal class Day22PuzzleManager : PuzzleManager
    {
        //protected override string INPUT_FILE_NAME { get; set; } = "test4.txt";

        public List<Command> Commands { get; set; }
        public List<int> Deck { get; set; } = new List<int>();

        private const int _deckSize = 10007;
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
            Deck = Enumerable.Range(0, _deckSize).ToList();
            foreach (var command in Commands)
            {
                ExecuteCommand(command);
            }
            var solution = Deck.IndexOf(2019);
            Console.WriteLine($"The solution to part one is '{solution}'.");
            return Task.CompletedTask;
        }

        private void ExecuteCommand(Command command)
        {
            switch (command.Description)
            {
                case CommandDescription.DealIntoNewStack:
                    Deck.Reverse();
                    break;
                case CommandDescription.DealWithIncrement:
                    DealWithIncrement(command.Value);
                    break;
                case CommandDescription.Cut:
                    Cut(command.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("The space deck shuffle command was not valid.");
            }

        }

        private void DealWithIncrement(int commandValue)
        {
            var newDeck = new int[_deckSize];
            var dealIndex = 0;

            foreach (var card in Deck)
            {
                newDeck[dealIndex] = card;
                dealIndex = (dealIndex + commandValue) % _deckSize;
            }
            Deck = newDeck.ToList();
        }

        private void Cut(int commandValue)
        {
            if (commandValue < 0)
            {
                commandValue = _deckSize + commandValue;
            }
            Deck.AddRange(Deck.Take(commandValue));
            Deck.RemoveRange(0, commandValue);
        }

        public override Task SolvePartTwo()
        {
            throw new NotImplementedException();
        }
    }
}
