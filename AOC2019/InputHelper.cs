using System.Text.RegularExpressions;

namespace AOC2019
{
    public abstract class InputHelper<T> : IInputHelper<T>
    {
        protected string FileName { get; set; }
        protected string InputPath { get; set; } = string.Empty;

        public InputHelper(string fileName)
        {
            FileName = fileName;

            var inputHelperClassName = this.GetType().Name;
            var dayRegex = new Regex(@"^Day(\d+)");
            var dayString = dayRegex.Match(inputHelperClassName).Value;
            InputPath = Path.Combine("..", "..", "..", dayString, FileName);
        }

        public abstract T Parse();
    }
}
