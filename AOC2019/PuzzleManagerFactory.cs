using System.Reflection;

namespace AOC2019
{
    internal class PuzzleManagerFactory
    {
        private IEnumerable<Type> _puzzleManagers;
        public PuzzleManagerFactory()
        {
            _puzzleManagers = Assembly.GetAssembly(typeof(PuzzleManager))!
                .GetTypes()
                .Where(t => typeof(PuzzleManager).IsAssignableFrom(t));
        }
        internal PuzzleManager CreatePuzzleManager(string day)
        {
            var puzzleManager = _puzzleManagers.Single(x =>
                x.Name.ToLowerInvariant().Contains("day" + day.ToLowerInvariant() + "puzzle"));

            return (PuzzleManager)Activator.CreateInstance(puzzleManager)!;
        }
    }
}
