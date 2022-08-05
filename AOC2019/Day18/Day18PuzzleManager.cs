using System.Diagnostics;

namespace AOC2019.Day18
{
    internal class Day18PuzzleManager : PuzzleManager
    {

        public List<Tile> TilesPartOne { get; set; }
        public List<Tile> TilesPartTwo { get; set; }
        protected override string INPUT_FILE_NAME { get; set; } = "test4Part1.txt";
        private const string INPUT_FILE_NAME_PART_TWO = "test1Part2.txt";

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
            _shortestPathsPartOne = FindShortestPaths(TilesPartOne);

            inputHelper = new Day18InputHelper(INPUT_FILE_NAME_PART_TWO);
            TilesPartTwo = inputHelper.Parse();
            _shortestPathsPartTwo = FindShortestPaths(TilesPartTwo, isPartTwo: true);
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

        public override Task SolveBothParts()
        {
            SolvePartOne();
            SolvePartTwo();
            return Task.CompletedTask;
        }

        public override Task SolvePartOne()
        {
            var solution = FindShortestNumberOfStepsToFindAllKeysPart1();
            Console.WriteLine($"The solution to part one is '{solution}'.");
            return Task.CompletedTask;
        }

        private int FindShortestNumberOfStepsToFindAllKeysPart1()
        {
            var totalNumberOfKeys = TilesPartOne.Count(x => x.IsKey);
            var startTile = TilesPartOne.First(x => x.IsStartingPosition);

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

                foreach (var shortestPath in FindReachableUnownedKeys(currentState.CurrentLocation, currentState.KeysCollected, _shortestPathsPartOne))
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
            // contains start
            // does not contain any member of keysHeld except for currentPosition and start
            // doorsBetween must be a subset of keysHeld
            return shortestPaths.Where(x =>
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
