using System.Text;

namespace AOC2019.Day17
{
    public class AsciiHelper
    {
        public void PrintAscii(Queue<long> input)
        {
            Console.SetCursorPosition(0, 0);
            var sb = new StringBuilder();
            var shouldKeepPrinting = true;
            char oldChar = '0';
            while (shouldKeepPrinting)
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

        public List<List<char>> CharListAscii(Queue<long> input)
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
    }
}
