using System.Diagnostics;

namespace AOC2019.Day18
{
    internal class Day18PuzzleManager : PuzzleManager
    {

        public List<Tile> Tiles { get; set; }
        //protected override string INPUT_FILE_NAME { get; set; } = "test5.txt";

        private readonly Dictionary<char, char> _doorToKeyMap = new Dictionary<char, char>
        {
            { 'A', 'a' },
            { 'B', 'b' },
            { 'C', 'c' },
            { 'D', 'd' },
            { 'E', 'e' },
            { 'F', 'f' },
            { 'G', 'g' },
            { 'H', 'h' },
            { 'I', 'i' },
            { 'J', 'j' },
            { 'K', 'k' },
            { 'L', 'l' },
            { 'M', 'm' },
            { 'N', 'n' },
            { 'O', 'o' },
            { 'P', 'p' },
            { 'Q', 'q' },
            { 'R', 'r' },
            { 'S', 's' },
            { 'T', 't' },
            { 'U', 'u' },
            { 'V', 'v' },
            { 'W', 'w' },
            { 'X', 'x' },
            { 'Y', 'y' },
            { 'Z', 'z' }
        };

        private HashSet<ShortestPath> _shortestPaths;

        public Day18PuzzleManager()
        {
            var inputHelper = new Day18InputHelper(INPUT_FILE_NAME);
            Tiles = inputHelper.Parse();
            _shortestPaths = FindShortestPaths();
        }

        //private HashSet<ShortestPath> FindShortestPaths()
        //{
        //    var shortestPaths = new HashSet<ShortestPath>();

        //    var keysAndStartingPositions = new List<char>() { '@' };
        //    keysAndStartingPositions.AddRange(Tiles.Where(x => x.IsKey).Select(x => x.Value));

        //    for (var i = 0; i < keysAndStartingPositions.Count - 1; i++)
        //    {
        //        for (var j = i + 1; j < keysAndStartingPositions.Count; j++)
        //        {
        //            (var distance, var doorsBetween, var keysBetween) = GetShortestDistanceDoorsBetweenAndKeysBetween(keysAndStartingPositions[i], keysAndStartingPositions[j]);
        //            var shortestPath = new ShortestPath(new HashSet<char>() { keysAndStartingPositions[i], keysAndStartingPositions[j] }, doorsBetween, keysBetween, distance);
        //            shortestPaths.Add(shortestPath);
        //        }
        //    }
        //    return shortestPaths;
        //}

        // This is taking ages! I could do a BFS for each key/start and then fill in the shortest paths as they're found?
        private (int distance, HashSet<char> doorsBetween, HashSet<char> keysBetween) GetShortestDistanceDoorsBetweenAndKeysBetween(char tile1, char tile2)
        {
            var startTile = Tiles.First(x => x.Value == tile1);
            var allStates = new Queue<Day18BreadthFirstSearchState>();
            allStates.Enqueue(new Day18BreadthFirstSearchState(0, startTile, new HashSet<Tile>() { startTile }));
            while (allStates.Count > 0)
            {
                var currentState = allStates.Dequeue();

                foreach (var unexploredNeighbour in FindUnexploredNeighbours(currentState))
                {
                    if (unexploredNeighbour.Value == tile2)
                    {
                        return (currentState.Steps + 1, currentState.DoorsBetween, currentState.KeysBetween);
                    }
                    var stateToQueue = new Day18BreadthFirstSearchState(currentState.Steps + 1, unexploredNeighbour, currentState.TilesVisited, currentState.DoorsBetween, currentState.KeysBetween);

                    if (unexploredNeighbour.IsDoor && !stateToQueue.DoorsBetween.Contains(unexploredNeighbour.Value))
                    {
                        stateToQueue.DoorsBetween.Add(unexploredNeighbour.Value);
                    }
                    if (unexploredNeighbour.IsKey && !stateToQueue.KeysBetween.Contains(unexploredNeighbour.Value))
                    {
                        stateToQueue.KeysBetween.Add(unexploredNeighbour.Value);
                    }
                    stateToQueue.TilesVisited.Add(unexploredNeighbour);
                    allStates.Enqueue(stateToQueue);
                }
            }
            throw new Exception($"Shortest path between '{tile1}' and '{tile2}' could not be found.");
        }

        private HashSet<ShortestPath> FindShortestPaths()
        {
            var shortestPaths = new HashSet<ShortestPath>();

            var keysAndStartingPositions = new List<char>() { '@' };
            keysAndStartingPositions.AddRange(Tiles.Where(x => x.IsKey).Select(x => x.Value));

            foreach (var tile in keysAndStartingPositions)
            {
                GetAllShortestPathsFromStartingTile(tile, shortestPaths, keysAndStartingPositions.Count);
            }
            return shortestPaths;
        }

        private void GetAllShortestPathsFromStartingTile(char tile, HashSet<ShortestPath> shortestPaths, int totalNumberOfStartingPositions)
        {
            var startTile = Tiles.First(x => x.Value == tile);
            var allStates = new Queue<Day18BreadthFirstSearchState>();
            allStates.Enqueue(new Day18BreadthFirstSearchState(0, startTile, new HashSet<Tile>() { startTile }));
            while (allStates.Count > 0)
            {
                if (shortestPaths.Where(x => x.TilesBetween.Contains(tile)).Count() >= totalNumberOfStartingPositions)
                {
                    return;
                }
                var currentState = allStates.Dequeue();

                foreach (var unexploredNeighbour in FindUnexploredNeighbours(currentState))
                {
                    if (unexploredNeighbour.IsKey && unexploredNeighbour.Value != tile && !shortestPaths.Any(x => x.TilesBetween.Contains(tile) && x.TilesBetween.Contains(unexploredNeighbour.Value)))
                    {
                        var shortestPath = new ShortestPath(new HashSet<char>() { tile, unexploredNeighbour.Value }, currentState.DoorsBetween, currentState.KeysBetween, currentState.Steps + 1);
                        shortestPaths.Add(shortestPath);
                    }
                    var stateToQueue = new Day18BreadthFirstSearchState(currentState.Steps + 1, unexploredNeighbour, currentState.TilesVisited, currentState.DoorsBetween, currentState.KeysBetween);

                    if (unexploredNeighbour.IsDoor && !stateToQueue.DoorsBetween.Contains(unexploredNeighbour.Value))
                    {
                        stateToQueue.DoorsBetween.Add(unexploredNeighbour.Value);
                    }
                    if (unexploredNeighbour.IsKey && !stateToQueue.KeysBetween.Contains(unexploredNeighbour.Value))
                    {
                        stateToQueue.KeysBetween.Add(unexploredNeighbour.Value);
                    }
                    stateToQueue.TilesVisited.Add(unexploredNeighbour);
                    allStates.Enqueue(stateToQueue);
                }
            }
        }

        private IEnumerable<Tile> FindUnexploredNeighbours(Day18BreadthFirstSearchState step)
        {
            return Tiles.Where(x => (
            x.Coordinates == (step.CurrentTile.Coordinates.Item1, step.CurrentTile.Coordinates.Item2 + 1) ||
            x.Coordinates == (step.CurrentTile.Coordinates.Item1, step.CurrentTile.Coordinates.Item2 - 1) ||
            x.Coordinates == (step.CurrentTile.Coordinates.Item1 + 1, step.CurrentTile.Coordinates.Item2) ||
            x.Coordinates == (step.CurrentTile.Coordinates.Item1 - 1, step.CurrentTile.Coordinates.Item2)
            ) && !step.TilesVisited.Contains(x));
        }

        public override Task SolveBothParts()
        {
            SolvePartOne();
            SolvePartTwo();
            return Task.CompletedTask;
        }

        public override Task SolvePartOne()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var solution = FindShortestNumberOfStepsToFindAllKeys();
            Console.WriteLine($"The solution to part one is '{solution}'.");
            stopwatch.Stop();
            return Task.CompletedTask;
        }

        private int FindShortestNumberOfStepsToFindAllKeys()
        {
            var totalNumberOfKeys = Tiles.Count(x => x.IsKey);
            var startTile = Tiles.First(x => x.IsStartingPosition);

            var allStates = new Queue<Day18SearchState>();
            allStates.Enqueue(new Day18SearchState('@', new HashSet<char>(), 0));
            var minStepsForKeysFoundAtFinalPosition = new Dictionary<(string, char), int>();
            var minSteps = int.MaxValue;
            while (allStates.Count > 0)
            {
                var currentState = allStates.Dequeue();
                if (currentState.KeysCollected.Count > 1)
                {
                    var keysCollectedString = ConvertKeysHeldToString(currentState.KeysCollected);
                    if (minStepsForKeysFoundAtFinalPosition.ContainsKey((keysCollectedString, currentState.CurrentLocation)))
                    {
                        if (minStepsForKeysFoundAtFinalPosition[(keysCollectedString, currentState.CurrentLocation)] <= currentState.Distance)
                        {
                            continue;
                        }
                        minStepsForKeysFoundAtFinalPosition[(keysCollectedString, currentState.CurrentLocation)] = currentState.Distance;
                    }
                    else
                    {
                        minStepsForKeysFoundAtFinalPosition.Add((keysCollectedString, currentState.CurrentLocation), currentState.Distance);
                    }
                }
                if (currentState.KeysCollected.Count == totalNumberOfKeys)
                {
                    minSteps = Math.Min(minSteps, currentState.Distance);
                    continue;
                }

                foreach (var shortestPath in FindReachableUnownedKeys(currentState.CurrentLocation, currentState.KeysCollected))
                {
                    var destination = shortestPath.TilesBetween.First(x => x != currentState.CurrentLocation);
                    var stateToQueue = new Day18SearchState(destination, currentState.KeysCollected, currentState.Distance + shortestPath.Distance);
                    foreach (var key in shortestPath.KeysBetween)
                    {
                        if (!stateToQueue.KeysCollected.Contains(key))
                        {
                            stateToQueue.KeysCollected.Add(key);
                        }
                    }
                    stateToQueue.KeysCollected.Add(destination);
                    allStates.Enqueue(stateToQueue);
                }
            }
            return minSteps;
        }

        //private int FindShortestNumberOfStepsToFindAllKeys()
        //{
        //    var totalNumberOfKeys = Tiles.Count(x => x.IsKey);
        //    var startTile = Tiles.First(x => x.IsStartingPosition);

        //    var allStates = new HashSet<Day18SearchState>();
        //    allStates.Add(new Day18SearchState('@', new HashSet<char>(), 0));
        //    while (allStates.Count > 0)
        //    {
        //        var currentState = allStates.MinBy(x => x.Distance);
        //        if (currentState!.KeysCollected.Count == totalNumberOfKeys)
        //        {
        //            return currentState.Distance;
        //        }
        //        allStates.Remove(currentState!);

        //        foreach (var shortestPath in FindReachableUnownedKeys(currentState!.CurrentLocation, currentState.KeysCollected))
        //        {
        //            var destination = shortestPath.TilesBetween.First(x => x != currentState.CurrentLocation);
        //            var stateToQueue = new Day18SearchState(destination, currentState.KeysCollected, currentState.Distance + shortestPath.Distance);
        //            stateToQueue.KeysCollected.Add(destination);
        //            allStates.Add(stateToQueue);
        //        }
        //    }
        //    throw new Exception($"Shortest path could not be found.");
        //}

        //private int FindShortestNumberOfStepsToFindAllKeys()
        //{
        //    var startAndKeys = Tiles.Where(x => x.IsStartingPosition || x.IsKey).ToHashSet();
        //    var currentTile = Tiles.First(x => x.IsStartingPosition);
        //    currentTile.TentativeDistance = 0;
        //    var keysHeld = new HashSet<char>();

        //    while (startAndKeys.Any(x => !x.IsVisited))
        //    {
        //        currentTile = startAndKeys.Where(x => !x.IsVisited).MinBy(x => x.TentativeDistance);
        //        if (currentTile!.IsKey)
        //        {
        //            keysHeld.Add(currentTile.Value);
        //        }

        //        foreach (var shortestPath in FindReachableUnownedKeys(currentTile!.Value, keysHeld))
        //        {
        //            var destination = shortestPath.TilesBetween.First(x => x != currentTile.Value);
        //            var destinationTile = startAndKeys.First(x => x.Value == destination);
        //            destinationTile.TentativeDistance = Math.Min(destinationTile.TentativeDistance, currentTile.TentativeDistance + shortestPath.Distance);
        //        }
        //        currentTile.IsVisited = true;

        //    }
        //    return startAndKeys.Max(x => x.TentativeDistance);
        //}

        private HashSet<ShortestPath> FindReachableUnownedKeys(char currentPosition, HashSet<char> keysHeld)
        {
            var keysHeldExceptForCurrentPosition = new HashSet<char>(keysHeld);
            keysHeldExceptForCurrentPosition.Remove(currentPosition);
            if (currentPosition != '@')
            {
                keysHeldExceptForCurrentPosition.Add('@');
            }
            // contains start
            // does not contain any member of keysHeld except for currentPosition and start
            // doorsBetween must be a subset of keysHeld
            return _shortestPaths.Where(x =>
                x.TilesBetween.Contains(currentPosition) &&
                !x.TilesBetween.Any(y => keysHeldExceptForCurrentPosition.Contains(y)) &&
                x.DoorsBetween.Select(x => _doorToKeyMap[x]).ToHashSet().IsSubsetOf(keysHeld))
                .ToHashSet();
        }


        private string ConvertKeysHeldToString(HashSet<char> keysHeld)
        {
            return new string(keysHeld.OrderBy(x => x).ToArray());
        }

        public override Task SolvePartTwo()
        {
            return Task.CompletedTask;
        }
    }
}
