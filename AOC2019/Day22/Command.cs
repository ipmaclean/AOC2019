namespace AOC2019.Day22
{
    internal class Command
    {
        public CommandDescription Description { get; set; }
        public int Value { get; set; }

        public Command(CommandDescription description, int value)
        {
            Description = description;
            Value = value;
        }
    }
}