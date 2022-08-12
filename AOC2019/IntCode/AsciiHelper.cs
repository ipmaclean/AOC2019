using System.Text;

namespace AOC2019.IntCode
{
    public static class AsciiHelper
    {
        public static void PrintAscii(Queue<long> input, bool overwrite = true)
        {
            if (overwrite)
            {
                Console.SetCursorPosition(0, 0);
            }
            var sb = new StringBuilder();
            var shouldKeepPrinting = true;
            char oldChar = '0';
            while (shouldKeepPrinting && input.Count > 0)
            {
                var nextChar = (char)input.Dequeue();
                sb.Append(nextChar);
                if (nextChar == 10 && oldChar == 10)
                {
                    shouldKeepPrinting = false;
                }
                oldChar = nextChar;
            }
            Console.Write(sb.ToString());
        }

        public static List<List<char>> CharListAscii(Queue<long> input)
        {
            var output = new List<List<char>>();
            var line = new List<char>();
            while (input.Count > 0)
            {
                var character = (char)input.Dequeue();
                if (character == 10)
                {
                    output.Add(line);
                    line = new List<char>();
                }
                else
                {
                    line.Add(character);
                }
            }
            return output;
        }

        public static void ConvertAsciiToIntCodeInputAndProvideToIntCodeComputer(Queue<long> intCodeComputerInputs, string asciiInput)
        {
            foreach (var character in asciiInput)
            {
                intCodeComputerInputs.Enqueue(character);
            }
            intCodeComputerInputs.Enqueue(10);
        }

        public static void ConvertUserInputAsciiToIntCodeInputAndProvideToIntCodeComputer(Queue<long> intCodeComputerInputs)
        {
            var input = Console.ReadLine() ?? string.Empty;
            var asciiCharString = string.Empty;
            foreach (var character in input)
            {
                intCodeComputerInputs.Enqueue(character);
            }
            intCodeComputerInputs.Enqueue(10);
        }
    }
}
