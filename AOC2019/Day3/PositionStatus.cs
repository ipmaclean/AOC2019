namespace AOC2019.Day3
{
    internal class PositionStatus
    {
        internal bool[] WireVisited { get; set; } = new bool[] {false, false};
        internal int[] WireSteps { get; set; } = new int[] { int.MaxValue, int.MaxValue };
    }
}
