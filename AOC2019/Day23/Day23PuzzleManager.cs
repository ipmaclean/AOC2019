using AOC2019.IntCode;

namespace AOC2019.Day23
{
    internal class Day23PuzzleManager : PuzzleManager
    {
        public Dictionary<long, long> IntCodeProgram { get; private set; }
        public Queue<long>[] Inputs { get; set; } = new Queue<long>[0];

        public Day23PuzzleManager()
        {
            var inputHelper = new Day23InputHelper(INPUT_FILE_NAME);
            IntCodeProgram = inputHelper.Parse();
        }

        public override Task SolveBothParts()
        {
            throw new NotImplementedException();
        }

        public override async Task SolvePartOne()
        {
            var tasks = new List<Task>();
            Inputs = new Queue<long>[50];
            var cancellationTokenSource = new CancellationTokenSource();

            for (var i = 0; i < 50; i++)
            {
                var codeInput = new Dictionary<long, long>(IntCodeProgram);
                var intCodeComputer = new IntCodeComputer(codeInput, cancellationToken: cancellationTokenSource.Token);
                Inputs[i] = new Queue<long>();
                Inputs[i].Enqueue(i);
                intCodeComputer.ExternalInputs = Inputs[i];
                tasks.Add(intCodeComputer.ProcessAsync(nonBlockingNetworkMode: true));
                tasks.Add(CoordinateNetwork(intCodeComputer.Outputs, cancellationTokenSource));
            }
            await Task.WhenAll(tasks);
        }

        private async Task CoordinateNetwork(Queue<long> outputs, CancellationTokenSource cancellationTokenSource)
        {
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                while (outputs.Count >= 3 && (outputs.Peek() == 255 || Inputs[(int)outputs.Peek()] != null))
                {
                    var networkLocation = (int)outputs.Dequeue();
                    var xPacket = outputs.Dequeue();
                    var yPacket = outputs.Dequeue();

                    if (networkLocation == 255)
                    {
                        cancellationTokenSource.Cancel();
                        Console.WriteLine($"The solution to part one is '{yPacket}'.");
                        return;
                    }

                    Inputs[networkLocation].Enqueue(xPacket);
                    Inputs[networkLocation].Enqueue(yPacket);
                }
                await Task.Delay(1);
            }
        }

        public override Task SolvePartTwo()
        {
            throw new NotImplementedException();
        }
    }
}
