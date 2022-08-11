namespace AOC2019.Day22
{
    internal class Command
    {
        public CommandDescription Description { get; set; }
        public long Value { get; set; }

        public Command(CommandDescription description, long value)
        {
            Description = description;
            Value = value;
        }
    }
}