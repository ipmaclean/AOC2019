namespace AOC2019.Day22
{
    internal class Day22PuzzleManager : PuzzleManager
    {

        public List<Command> Commands { get; set; }
        public (long Offset, long Increment) DeckMakeup { get; set; } = (0, 0);

        private const long _deckSizePartOne = 10_007;

        private const long _deckSizePartTwo = 119_315_717_514_047;
        private const long _iterationsPartTwo = 101_741_582_076_661;
        public Day22PuzzleManager()
        {
            var inputHelper = new Day22InputHelper(INPUT_FILE_NAME);
            Commands = inputHelper.Parse();
        }

        public override Task SolveBothParts()
        {
            SolvePartOne();
            SolvePartTwo();
            return Task.CompletedTask;
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


        public override Task SolvePartTwo()
        {
            DeckMakeup = (0, 1);
            foreach (var command in Commands)
            {
                ExecuteCommand(command, _deckSizePartTwo);
            }
            var incrementBase = DeckMakeup.Increment;
            var offsetMultiplier = DeckMakeup.Offset;
            var oneMinusIncrementBaseInverse = ModInverse(1 - incrementBase, _deckSizePartTwo);
            var multiplier = ModuloMultiplication(oneMinusIncrementBaseInverse, offsetMultiplier, _deckSizePartTwo);

            var offset = _iterationsPartTwo == 0 ? 0 : ModuloMultiplication(1 - ModuloPower(incrementBase, _iterationsPartTwo, _deckSizePartTwo), multiplier, _deckSizePartTwo);
            var increment = ModuloPower(incrementBase, _iterationsPartTwo, _deckSizePartTwo);

            var solution = Mod(offset + ModuloMultiplication(2020, increment, _deckSizePartTwo), _deckSizePartTwo);
            Console.WriteLine($"The solution to part two is '{solution}'.");
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
            var newIncrement = ModuloMultiplication(DeckMakeup.Increment, commandValueInverse, deckSize);
            DeckMakeup = (DeckMakeup.Offset, newIncrement);
        }

        private void Cut(long commandValue, long deckSize)
        {
            var newOffset = Mod(DeckMakeup.Offset + DeckMakeup.Increment * commandValue, deckSize);
            DeckMakeup = (newOffset, DeckMakeup.Increment);
        }

        private long Mod(long x, long m)
        {
            return (x % m + m) % m;
        }

        private long ModInverse(long a, long m)
        {
            // As we know that the deck sizes (m) are primes, we can use Fermat's Little Theorem
            var g = Gcd(a, m);
            if (g != 1)
                throw new ArgumentException($"The inverse of '{a}' modulo '{m}' does not exist.");
            else
            {
                // If a and m are relatively
                // prime, then modulo inverse
                // is a^(m-2) mode m
                return ModuloPower(a, m - 2, m);
            }
        }

        // To compute x^y under
        // modulo m
        private long ModuloPower(long x, long y, long m)
        {
            if (y == 0)
                return 1;

            var p = Mod(ModuloPower(x, y / 2, m), m);
            p = ModuloMultiplication(p, p, m);

            if (y % 2 == 0)
                return p;
            else
                return ModuloMultiplication(x, p, m);
        }

        private long Gcd(long a, long b)
        {
            if (a == 0)
                return b;
            return Gcd(b % a, a);
        }

        // Returns (a * b) % mod
        private long ModuloMultiplication(long a, long b, long mod)
        {
            a = a < 0 ? Mod(a, mod) : a;
            b = b < 0 ? Mod(b, mod) : b;
            long res = 0; // Initialize result

            // Update a if it is more than
            // or equal to mod
            a %= mod;

            while (b > 0)
            {
                // If b is odd, add a with result
                if ((b & 1) > 0)
                    res = (res + a) % mod;

                // Here we assume that doing 2*a
                // doesn't cause overflow
                a = (2 * a) % mod;

                b >>= 1; // b = b / 2
            }

            return res;
        }
    }
}
