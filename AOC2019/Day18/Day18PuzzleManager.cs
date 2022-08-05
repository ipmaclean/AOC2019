namespace AOC2019.Day18
{
    internal class Day18PuzzleManager : PuzzleManager
    {

        public List<Tile> TilesPartOne { get; set; }
        public List<Tile> TilesPartTwo { get; set; }

        private const string INPUT_FILE_NAME_PART_TWO = "inputPart2.txt";
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
        private HashSet<ShortestPath> _shortestPathsPartOne = new HashSet<ShortestPath>();
        private HashSet<ShortestPath> _shortestPathsPartTwo = new HashSet<ShortestPath>();

        public Day18PuzzleManager()
        {
            var inputHelper = new Day18InputHelper(INPUT_FILE_NAME);
            TilesPartOne = inputHelper.Parse();

            inputHelper = new Day18InputHelper(INPUT_FILE_NAME_PART_TWO);
            TilesPartTwo = inputHelper.Parse();
        }

        public override Task SolveBothParts()
        {
            SolvePartOne();
            SolvePartTwo();
            return Task.CompletedTask;
        }

        public override Task SolvePartOne()
        {
            if (_shortestPathsPartOne.Count == 0)
            {
                _shortestPathsPartOne = FindShortestPaths(TilesPartOne);
            }
            var solution = FindShortestNumberOfStepsToFindAllKeys(TilesPartOne, _shortestPathsPartOne);
            Console.WriteLine($"The solution to part one is '{solution}'.");
            return Task.CompletedTask;
        }

        public override Task SolvePartTwo()
        {
            if (_shortestPathsPartTwo.Count == 0)
            {
                _shortestPathsPartTwo = FindShortestPaths(TilesPartTwo, isPartTwo: true);
            }
            var solution = FindShortestNumberOfStepsToFindAllKeys(TilesPartTwo, _shortestPathsPartTwo, isPartTwo: true);
            Console.WriteLine($"The solution to part one is '{solution}'.");
            return Task.CompletedTask;
        }

        private HashSet<ShortestPath> FindShortestPaths(List<Tile> allTiles, bool isPartTwo = false)
        {
            var shortestPaths = new HashSet<ShortestPath>();

            var keysAndStartingPositions = new List<char>() { '@' };
            if (isPartTwo)
            {
                keysAndStartingPositions.AddRange(new List<char>() { '<', '>', '^' });
            }
            keysAndStartingPositions.AddRange(allTiles.Where(x => x.IsKey).Select(x => x.Value));

            foreach (var tile in keysAndStartingPositions)
            {
                GetAllShortestPathsFromStartingTile(tile, shortestPaths, keysAndStartingPositions.Count, allTiles);
            }
            return shortestPaths;
        }

        private void GetAllShortestPathsFromStartingTile(char tile, HashSet<ShortestPath> shortestPaths, int totalNumberOfStartingPositions, List<Tile> allTiles)
        {
            var startTiles = allTiles.Where(x => x.Value == tile);
            foreach (var startTile in startTiles)
            {
                var allStates = new Queue<Day18BreadthFirstSearchState>();
                allStates.Enqueue(new Day18BreadthFirstSearchState(0, startTile, new HashSet<Tile>() { startTile }));
                while (allStates.Count > 0)
                {
                    if (shortestPaths.Where(x => x.TilesBetween.Contains(tile)).Count() >= totalNumberOfStartingPositions)
                    {
                        return;
                    }
                    var currentState = allStates.Dequeue();

                    foreach (var unexploredNeighbour in FindUnexploredNeighbours(currentState, allTiles))
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
        }

        private IEnumerable<Tile> FindUnexploredNeighbours(Day18BreadthFirstSearchState step, List<Tile> allTiles)
        {
            return allTiles.Where(x => (
            x.Coordinates == (step.CurrentTile.Coordinates.Item1, step.CurrentTile.Coordinates.Item2 + 1) ||
            x.Coordinates == (step.CurrentTile.Coordinates.Item1, step.CurrentTile.Coordinates.Item2 - 1) ||
            x.Coordinates == (step.CurrentTile.Coordinates.Item1 + 1, step.CurrentTile.Coordinates.Item2) ||
            x.Coordinates == (step.CurrentTile.Coordinates.Item1 - 1, step.CurrentTile.Coordinates.Item2)
            ) && !step.TilesVisited.Contains(x));
        }

        private int FindShortestNumberOfStepsToFindAllKeys(List<Tile> allTiles, HashSet<ShortestPath> shortestPaths, bool isPartTwo = false)
        {
            var totalNumberOfKeys = allTiles.Count(x => x.IsKey);
            var startTile = allTiles.First(x => x.IsStartingPosition);

            var allStates = new Queue<Day18SearchState>();

            var startingPositions = new HashSet<char>() { '@' };
            if (isPartTwo)
            {
                startingPositions = new HashSet<char>() { '@', '<', '>', '^' };
            }
            allStates.Enqueue(new Day18SearchState(startingPositions, new HashSet<char>(), 0));
            var minStepsForKeysFoundAtFinalPosition = new Dictionary<(string, string), int>();
            var minSteps = int.MaxValue;
            while (allStates.Count > 0)
            {
                var currentState = allStates.Dequeue();
                if (currentState.KeysCollected.Count > 1)
                {
                    var keysCollectedString = ConvertHashSetToOrderedString(currentState.KeysCollected);
                    var currentLocationsString = ConvertHashSetToOrderedString(currentState.CurrentLocations);
                    if (minStepsForKeysFoundAtFinalPosition.ContainsKey((keysCollectedString, currentLocationsString)))
                    {
                        if (minStepsForKeysFoundAtFinalPosition[(keysCollectedString, currentLocationsString)] <= currentState.Distance)
                        {
                            // If this exact state has already been reached more efficiently then discard the current state
                            continue;
                        }
                        minStepsForKeysFoundAtFinalPosition[(keysCollectedString, currentLocationsString)] = currentState.Distance;
                    }
                    else
                    {
                        minStepsForKeysFoundAtFinalPosition.Add((keysCollectedString, currentLocationsString), currentState.Distance);
                    }
                }
                if (currentState.KeysCollected.Count == totalNumberOfKeys)
                {
                    minSteps = Math.Min(minSteps, currentState.Distance);
                    continue;
                }
                foreach (var currentLocation in currentState.CurrentLocations)
                {
                    foreach (var shortestPath in FindReachableUnownedKeys(currentLocation, currentState.KeysCollected, shortestPaths))
                    {
                        var destination = shortestPath.TilesBetween.First(x => x != currentLocation);
                        var stateToQueueCurrentLocations = new HashSet<char>(currentState.CurrentLocations);
                        stateToQueueCurrentLocations.Remove(currentLocation);
                        stateToQueueCurrentLocations.Add(destination);
                        var stateToQueue = new Day18SearchState(stateToQueueCurrentLocations, currentState.KeysCollected, currentState.Distance + shortestPath.Distance);
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
            }
            return minSteps;
        }

        private HashSet<ShortestPath> FindReachableUnownedKeys(char currentPosition, HashSet<char> keysHeld, HashSet<ShortestPath> shortestPaths)
        {
            var keysHeldExceptForCurrentPosition = new HashSet<char>(keysHeld);
            keysHeldExceptForCurrentPosition.Remove(currentPosition);
            if (currentPosition != '@')
            {
                keysHeldExceptForCurrentPosition.Add('@');
            }
            if (currentPosition != '<')
            {
                keysHeldExceptForCurrentPosition.Add('<');
            }
            if (currentPosition != '>')
            {
                keysHeldExceptForCurrentPosition.Add('>');
            }
            if (currentPosition != '^')
            {
                keysHeldExceptForCurrentPosition.Add('^');
            }
            // Contains the startingPosition
            // Does not contain any member of keysHeld except for currentPosition and startingPosition
            // doorsBetween must be a subset of keysHeld
            return shortestPaths.Where(x =>
                x.TilesBetween.Contains(currentPosition) &&
                !x.TilesBetween.Any(y => keysHeldExceptForCurrentPosition.Contains(y)) &&
                x.DoorsBetween.Select(x => _doorToKeyMap[x]).ToHashSet().IsSubsetOf(keysHeld))
                .ToHashSet();
        }

        private string ConvertHashSetToOrderedString(HashSet<char> hashSet)
        {
            return new string(hashSet.OrderBy(x => x).ToArray());
        }
    }
}
