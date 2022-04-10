using System.Text;

namespace AOC2019.Day13
{
    internal class Screen
    {
        public Dictionary<(long, long), long> Tiles { get; set; } = new Dictionary<(long, long), long>();
        public Queue<long> ExternalInputs { get; set; } = new Queue<long>();
        public Queue<long> Outputs { get; set; } = new Queue<long>();

        public Screen(Queue<long> externalInputs)
        {
            ExternalInputs = externalInputs;
        }

        private void PrintResult()
        {
            var highestX = Tiles.Max(x => x.Key.Item1);
            var highestY = Tiles.Max(x => x.Key.Item2);

            var canvas = new char[highestX + 1, highestY + 1];

            long score = 0;
            foreach (var tile in Tiles)
            {
                if (tile.Key.Item1 == -1)
                {
                    score = tile.Value;
                    continue;
                }
                canvas[tile.Key.Item1, tile.Key.Item2] = GetTileRepresentation(tile.Value);
            }

            var sb = new StringBuilder();
            sb.Append($"The current score is: {score}");
            sb.Append(Environment.NewLine);
            for (var j = 0; j <= highestY; j++)
            {
                for (var i = 0; i <= highestX; i++)
                {
                    sb.Append(canvas[i, j]);
                }
                sb.Append(Environment.NewLine);
            }
            Console.Clear();
            Console.Write(sb.ToString());
        }

        private char GetTileRepresentation(long value)
        {
            switch (value)
            {
                case 0:
                    return ' ';
                case 1:
                    return '#';
                case 2:
                    return 'x';
                case 3:
                    return '-';
                case 4:
                    return 'o';
                default:
                    throw new ArgumentException("Did not recognise tile id.");

            }
        }

        public int RunPartOne()
        {
            while (ExternalInputs.Count > 1)
            {
                DrawTile();
            }
            return Tiles.Count(x => x.Value == 2);
        }

        public async Task RunPartTwo(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested || ExternalInputs.Any())
            {
                while (!ExternalInputs!.Any())
                {
                    await Task.Delay(20);
                    if (cancellationToken.IsCancellationRequested && !ExternalInputs.Any())
                    {
                        return;
                    }
                }
                DrawTile();
                if (ExternalInputs.Count == 0)
                {
                    PrintResult();
                    var ballXCoord = Tiles.FirstOrDefault(x => x.Value == 4).Key.Item1;
                    var paddleXCoord = Tiles.FirstOrDefault(x => x.Value == 3).Key.Item1;
                    if (ballXCoord < paddleXCoord)
                    {
                        Outputs.Enqueue(-1);
                    }
                    else if (ballXCoord > paddleXCoord)
                    {
                        Outputs.Enqueue(1);
                    }
                    else
                    {
                        Outputs.Enqueue(0);
                    }
                }
            }
        }

        private void DrawTile()
        {
            var xCoord = ExternalInputs.Dequeue();
            var yCoord = ExternalInputs.Dequeue();
            var tileId = ExternalInputs.Dequeue();

            if (Tiles.ContainsKey((xCoord, yCoord)))
            {
                Tiles[(xCoord, yCoord)] = tileId;
            }
            else
            {
                Tiles.Add((xCoord, yCoord), tileId);
            }
        }
    }
}
