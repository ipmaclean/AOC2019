using System.Text;

namespace AOC2019.Day11
{
    internal class PaintingRobot
    {
        public Dictionary<(int, int), long> Tiles { get; set; } = new Dictionary<(int, int), long>();
        public (int, int) Position { get; set; } = (0, 0);
        public char Facing { get; set; } = '^';
        public int TilesPainted { get; set; } = 0;
        public Queue<long> CurrentColourOutput { get; set; } = new Queue<long>();
        public Queue<long> ExternalInputs { get; set; } = new Queue<long>();

        public PaintingRobot()
        {
        }

        public void UpdateFacing(long command)
        {
            switch (Facing)
            {
                case '^':
                    if (command == 0)
                    {
                        Facing = '<';
                    }
                    else if (command == 1)
                    {
                        Facing = '>';
                    }
                    else
                    {
                        throw new ArgumentException("Robot facing command not recognised.");
                    }
                    break;
                case '>':
                    if (command == 0)
                    {
                        Facing = '^';
                    }
                    else if (command == 1)
                    {
                        Facing = 'v';
                    }
                    else
                    {
                        throw new ArgumentException("Robot facing command not recognised.");
                    }
                    break;
                case 'v':
                    if (command == 0)
                    {
                        Facing = '>';
                    }
                    else if (command == 1)
                    {
                        Facing = '<';
                    }
                    else
                    {
                        throw new ArgumentException("Robot facing command not recognised.");
                    }
                    break;
                case '<':
                    if (command == 0)
                    {
                        Facing = 'v';
                    }
                    else if (command == 1)
                    {
                        Facing = '^';
                    }
                    else
                    {
                        throw new ArgumentException("Robot facing command not recognised.");
                    }
                    break;
                default:
                    throw new InvalidOperationException("Robot facing not recognised.");
            }
        }

        public void MoveOneStep()
        {
            switch (Facing)
            {
                case '^':
                    Position = (Position.Item1, Position.Item2 + 1);
                    break;
                case '>':
                    Position = (Position.Item1 + 1, Position.Item2);
                    break;
                case 'v':
                    Position = (Position.Item1, Position.Item2 - 1);
                    break;
                case '<':
                    Position = (Position.Item1 - 1, Position.Item2);
                    break;
                default:
                    throw new InvalidOperationException("Robot facing not recognised.");
            }
        }

        private void UpdatePosition(long command)
        {
            UpdateFacing(command);
            MoveOneStep();
        }

        public void PaintTile(long command)
        {
            if (command == 0 || command == 1)
            {
                if (Tiles.ContainsKey(Position))
                {
                    Tiles[Position] = command;
                }
                else
                {
                    Tiles.Add(Position, command);
                    TilesPainted++;
                }
            }
            else
            {
                throw new ArgumentException("Paint command not recognised.");
            }
        }

        internal void PrintResult()
        {
            var lowestX = Tiles.Min(x => x.Key.Item1);
            var highestX = Tiles.Max(x => x.Key.Item1);
            var lowestY = Tiles.Min(x => x.Key.Item2);
            var highestY = Tiles.Max(x => x.Key.Item2);

            var canvas = new char[highestX - lowestX + 1, highestY - lowestY + 1];

            foreach (var tile in Tiles)
            {
                canvas[tile.Key.Item1 - lowestX, tile.Key.Item2 - lowestY] = tile.Value == 1 ? '#' : ' ';
            }

            var sb = new StringBuilder();
            for (var j = highestY - lowestY; j >= 0; j--)
            {
                for (var i = 0; i < highestX - lowestX + 1; i++)
                {
                    sb.Append(canvas[i, j] == '\x0000' ? ' ' : canvas[i, j]);
                }
                sb.Append(Environment.NewLine);
            }
            Console.Write(sb.ToString());
        }

        public async Task<int> RunRobot(CancellationToken cancellationToken, bool isPartTwo = false)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                long currentColour = 0;
                if (Tiles.ContainsKey(Position))
                {
                    currentColour = Tiles[Position];
                }
                else if (Position == (0, 0) && isPartTwo)
                {
                    currentColour = 1;
                }

                CurrentColourOutput.Enqueue(currentColour);
                await ProcessInputInstructionAsync(cancellationToken);
            }
            return TilesPainted;
        }

        private async Task ProcessInputInstructionAsync(CancellationToken cancellationToken)
        {
            while (ExternalInputs.Count < 2)
            {
                await Task.Delay(5);
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
            }
            PaintTile(ExternalInputs.Dequeue());
            UpdatePosition(ExternalInputs.Dequeue());
        }
    }
}
