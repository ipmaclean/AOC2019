using AOC2019.IntCode;

namespace AOC2019.Day23
{
    internal class Day23PuzzleManager : PuzzleManager
    {
        private long _xPacketNat = 0L;
        private long _yPacketNat = 0L;
        private bool[] _intCodeComputersAwaitingInput = new bool[0];

        public Dictionary<long, long> IntCodeProgram { get; private set; }
        public Queue<long>[] Inputs { get; set; } = new Queue<long>[0];

        public Day23PuzzleManager()
        {
            var inputHelper = new Day23InputHelper(INPUT_FILE_NAME);
            IntCodeProgram = inputHelper.Parse();
        }

        public override async Task SolveBothParts()
        {
            await SolvePartOne();
            await Task.Delay(10);
            await SolvePartTwo();
        }

        public override async Task SolvePartOne()
        {
            await Solve(true);
        }

        public override async Task SolvePartTwo()
        {
            await Solve(false);
        }

        private async Task Solve(bool isPartOne)
        {
            var tasks = new List<Task>();
            Inputs = new Queue<long>[50];
            var cancellationTokenSource = new CancellationTokenSource();
            _intCodeComputersAwaitingInput = new bool[50];

            for (var i = 0; i < 50; i++)
            {
                var codeInput = new Dictionary<long, long>(IntCodeProgram);
                var intCodeComputer = new IntCodeComputer(codeInput, cancellationToken: cancellationTokenSource.Token, name: i);
                Inputs[i] = new Queue<long>();
                Inputs[i].Enqueue(i);
                intCodeComputer.ExternalInputs = Inputs[i];
                intCodeComputer.AwaitingInput += AwaitingInputHandler;
                tasks.Add(intCodeComputer.ProcessAsync(nonBlockingNetworkMode: true));
                tasks.Add(CoordinateNetwork(intCodeComputer, isPartOne, cancellationTokenSource));
            }
            if (!isPartOne)
            {
                tasks.Add(RunNat(cancellationTokenSource));
            }
            await Task.WhenAll(tasks);
        }

        private async Task CoordinateNetwork(IntCodeComputer intCodeComputer, bool isPartOne, CancellationTokenSource cancellationTokenSource)
        {
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                while (intCodeComputer.Outputs.Count >= 3 && (intCodeComputer.Outputs.Peek() == 255 || Inputs[(int)intCodeComputer.Outputs.Peek()] != null))
                {
                    var networkLocation = (int)intCodeComputer.Outputs.Dequeue();
                    var xPacket = intCodeComputer.Outputs.Dequeue();
                    var yPacket = intCodeComputer.Outputs.Dequeue();

                    if (networkLocation == 255 && isPartOne)
                    {
                        cancellationTokenSource.Cancel();
                        Console.WriteLine($"The solution to part one is '{yPacket}'.");
                        return;
                    }
                    else if (networkLocation == 255)
                    {
                        _xPacketNat = xPacket;
                        _yPacketNat = yPacket;
                        continue;
                    }
                    _intCodeComputersAwaitingInput[intCodeComputer.Name] = false;
                    Inputs[networkLocation].Enqueue(xPacket);
                    Inputs[networkLocation].Enqueue(yPacket);
                }
                await Task.Delay(1);
            }
        }

        private async Task RunNat(CancellationTokenSource cancellationTokenSource)
        {
            var previousYPacketNatDelivered = -1L;
            var idleCounter = 0;
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                await Task.Delay(1);
                // Ensure that the network really is idling and not just waiting for a computer to compute an output.
                if (Inputs.All(x => x.Count == 0) && _intCodeComputersAwaitingInput.All(x => x == true))
                {
                    idleCounter++;
                }
                else
                {
                    idleCounter = 0;
                }
                if (Inputs.All(x => x.Count == 0) && _intCodeComputersAwaitingInput.All(x => x == true) && idleCounter > 10)
                {
                    if (previousYPacketNatDelivered == _yPacketNat)
                    {
                        cancellationTokenSource.Cancel();
                        Console.WriteLine($"The solution to part two is '{previousYPacketNatDelivered}'.");
                        return;
                    }
                    previousYPacketNatDelivered = _yPacketNat;
                    _intCodeComputersAwaitingInput[0] = false;
                    Inputs[0].Enqueue(_xPacketNat);
                    Inputs[0].Enqueue(_yPacketNat);
                    idleCounter = 0;
                }
            }
        }

        private void AwaitingInputHandler(object? sender, int name)
        {
            _intCodeComputersAwaitingInput[name] = true;
        }
    }
}