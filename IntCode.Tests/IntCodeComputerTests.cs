using AOC2019.Day1;
using Xunit;

namespace IntCode.Tests
{
    public class IntCodeComputerTests
    {
        [Fact]
        public async void Day2Part1_ShouldBeCorrect()
        {
            var day2PuzzleManger = new Day2PuzzleManager();
            await day2PuzzleManger.SolvePartOne();
        }

        [Fact]
        public void Day2Part2_ShouldBeCorrect()
        {

        }
    }
}