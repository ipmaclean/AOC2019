namespace AOC2019
{
    public abstract class PuzzleManager : IPuzzleManager
    {
        protected virtual string INPUT_FILE_NAME { get; set; } = "input.txt";

        public virtual void Reset()
        {
        }
        public abstract Task SolveBothParts();
        public abstract Task SolvePartOne();
        public abstract Task SolvePartTwo();
    }
}
