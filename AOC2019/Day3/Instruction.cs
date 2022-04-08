namespace AOC2019.Day3
{
    internal class Instruction
    {
        internal char Direction { get; private set; }
        internal int Distance { get; private set; }

        internal Instruction(char direction, int distance)
        {
            Direction = direction;
            Distance = distance;
        }
    }
}
