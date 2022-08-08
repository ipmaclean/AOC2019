using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2019.Day21
{
    internal class Day21PuzzleManager : PuzzleManager
    {
        public Dictionary<long, long> IntCodeProgram { get; private set; }

        public Day21PuzzleManager()
        {
            var inputHelper = new Day21InputHelper(INPUT_FILE_NAME);
            IntCodeProgram = inputHelper.Parse();
        }

        public override Task SolveBothParts()
        {
            throw new NotImplementedException();
        }

        public override Task SolvePartOne()
        {
            throw new NotImplementedException();
        }

        public override Task SolvePartTwo()
        {
            throw new NotImplementedException();
        }
    }
}
