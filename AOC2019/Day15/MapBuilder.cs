using AOC2019.IntCode;
using System.Text;

namespace AOC2019.Day15
{
    internal class MapBuilder
    {
        private List<Tile> _tiles = new List<Tile>() { new Tile((0, 0), TileType.Open) };
        private Tile _currentTile;

        private Stack<Direction> _commands = new Stack<Direction>();

        private bool _intCodeComputerAwaitingInput = false;
        private bool _intCodeComputerProgramHalted = false;
        private bool _ignoreNextIntCodeOutput = false;

        public Queue<long> ExternalInputs { get; set; } = new Queue<long>();
        public Queue<long> Outputs { get; set; } = new Queue<long>();
        public CancellationTokenSource IntCodeCancellationTokenSource { get; private set; } = new CancellationTokenSource();


        public MapBuilder(IntCodeComputer intCodeComputer)
        {
            ExternalInputs = intCodeComputer.Outputs;
            intCodeComputer.AwaitingInput += AwaitingInputHandler;
            intCodeComputer.ProgramHalted += ProgramHaltedHandler;
            intCodeComputer.CancellationToken = IntCodeCancellationTokenSource.Token;
            _currentTile = _tiles.First();
        }

        private void AwaitingInputHandler(object? sender, int e)
        {
            _intCodeComputerAwaitingInput = true;
        }

        private void ProgramHaltedHandler(object? sender, EventArgs e)
        {
            _intCodeComputerProgramHalted = true;
        }

        public async Task RunPartOne(bool printResult = false)
        {
            await MapShipSection();

            if (printResult)
            {
                PrintResult();
            }

            var bfs = new Day15BreadthFirstSearch(_tiles);
            var solution = bfs.FindShortestNumberOfStepsFromStartToOxygen();
            Console.WriteLine($"");
            Console.WriteLine($"The solution to part one is '{solution}'.");
        }

        public async Task RunPartTwo(bool printResult = false)
        {
            await MapShipSection();

            if (printResult)
            {
                PrintResult();
            }

            var bfs = new Day15BreadthFirstSearch(_tiles);
            var solution = bfs.FindTimeToFillWithOxygen();
            Console.WriteLine($"");
            Console.WriteLine($"The solution to part two is '{solution}'.");
        }

        public async Task RunBothParts(bool printResult = false)
        {
            await MapShipSection();

            if (printResult)
            {
                PrintResult();
            }

            var bfs = new Day15BreadthFirstSearch(_tiles);
            var solution1 = bfs.FindShortestNumberOfStepsFromStartToOxygen();
            var solution2 = bfs.FindTimeToFillWithOxygen();
            Console.WriteLine($"");
            Console.WriteLine($"The solution to part one is '{solution1}'.");
            Console.WriteLine($"The solution to part two is '{solution2}'.");
        }

        private async Task MapShipSection()
        {
            while (!_intCodeComputerProgramHalted || ExternalInputs.Any())
            {
                if (ProgramComplete())
                {
                    IntCodeCancellationTokenSource.Cancel();
                    break;
                }

                GiveDirectionToIntCodeComputer();

                while (!_intCodeComputerAwaitingInput ^ _intCodeComputerProgramHalted)
                {
                    await Task.Delay(5);
                }

                InterpretExternalInput();
            }
        }

        private bool ProgramComplete()
        {
            if (_currentTile.Position == (0, 0) && _currentTile.DirectionsSearched.Count == 4)
            {
                return true;
            }
            return false;
        }

        private void GiveDirectionToIntCodeComputer()
        {
            if (_currentTile.DirectionsSearched.Count == 4)
            {
                UndoLastCommand();
                return;
            }

            Direction direction;

            if (!_currentTile.DirectionsSearched.Contains(Direction.North))
            {
                direction = Direction.North;
            }
            else if (!_currentTile.DirectionsSearched.Contains(Direction.East))
            {
                direction = Direction.East;
            }
            else if (!_currentTile.DirectionsSearched.Contains(Direction.South))
            {
                direction = Direction.South;
            }
            else
            {
                direction = Direction.West;
            }
            _currentTile.DirectionsSearched.Add(direction);
            Outputs.Enqueue((long)direction);
            _commands.Push(direction);

        }

        private void UndoLastCommand()
        {
            _ignoreNextIntCodeOutput = true;

            // Change to previous tile
            var lastCommand = _commands.Pop();
            var newPosition = _currentTile.Position;
            Direction oppositeOfLastComment;

            switch (lastCommand)
            {
                case Direction.North:
                    newPosition.Item2--;
                    oppositeOfLastComment = Direction.South;
                    break;
                case Direction.East:
                    newPosition.Item1--;
                    oppositeOfLastComment = Direction.West;
                    break;
                case Direction.South:
                    newPosition.Item2++;
                    oppositeOfLastComment = Direction.North;
                    break;
                case Direction.West:
                    newPosition.Item1++;
                    oppositeOfLastComment = Direction.East;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Command stack output not recognised.");
            }
            _currentTile = _tiles.First(x => x.Position == newPosition);

            Outputs.Enqueue((long)oppositeOfLastComment);
        }

        private void InterpretExternalInput()
        {
            var externalInput = ExternalInputs.Dequeue();
            _intCodeComputerAwaitingInput = false;
            if (_ignoreNextIntCodeOutput)
            {
                _ignoreNextIntCodeOutput = false;
                return;
            }
            AddNewTileAndMove((TileType)externalInput);
        }

        private void AddNewTileAndMove(TileType externalInput)
        {
            var newPosition = _currentTile.Position;
            Direction originDirection;
            switch (_commands.Peek())
            {
                case Direction.North:
                    newPosition.Item2++;
                    originDirection = Direction.South;
                    break;
                case Direction.East:
                    newPosition.Item1++;
                    originDirection = Direction.West;
                    break;
                case Direction.South:
                    newPosition.Item2--;
                    originDirection = Direction.North;
                    break;
                case Direction.West:
                    newPosition.Item1--;
                    originDirection = Direction.East;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Command stack output not recognised.");
            }
            var newTile = new Tile(newPosition, externalInput, originDirection);
            _tiles.Add(newTile);
            if (externalInput == TileType.Open || externalInput == TileType.Oxygen)
            {
                _currentTile = newTile;
            }
            else
            {
                _commands.Pop();
            }
        }

        private void PrintResult()
        {
            Console.Clear();

            var highestX = _tiles.Max(x => x.Position.Item1);
            var highestY = _tiles.Max(x => x.Position.Item2);
            var lowestX = _tiles.Min(x => x.Position.Item1);
            var lowestY = _tiles.Min(x => x.Position.Item2);

            var canvas = new char[highestX - lowestX + 1, highestY - lowestY + 1];

            for (int i = 0; i < (highestX - lowestX + 1) * (highestY - lowestY + 1); i++)
            {
                canvas[i % (highestX - lowestX + 1), i / (highestX - lowestX + 1)] = '#';
            }

            foreach (var tile in _tiles)
            {
                if (tile.Position == (0, 0))
                {
                    canvas[tile.Position.Item1 - lowestX, tile.Position.Item2 - lowestY] = 'x';
                }
                else
                {
                    canvas[tile.Position.Item1 - lowestX, tile.Position.Item2 - lowestY] = GetTileRepresentation(tile.Type);
                }
            }

            var sb = new StringBuilder();
            for (var j = highestY - lowestY; j >= 0; j--)
            {
                for (var i = 0; i <= highestX - lowestX; i++)
                {
                    sb.Append(canvas[i, j]);
                }
                sb.Append(Environment.NewLine);
            }

            Console.Write(sb.ToString());
        }

        private char GetTileRepresentation(TileType value)
        {
            switch (value)
            {
                case TileType.Wall:
                    return '#';
                case TileType.Open:
                    return ' ';
                case TileType.Oxygen:
                    return 'o';
                default:
                    throw new ArgumentException("Did not recognise tile id.");
            }
        }
    }
}
