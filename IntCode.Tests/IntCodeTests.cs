using AOC2019.Day2;
using AOC2019.Day5;
using AOC2019.Day7;
using AOC2019.Day9;
using Xunit;

namespace IntCode.Tests
{
    public class IntCodeTests
    {
        [Fact]
        public async void Day2_Part1_ShouldBeCorrect()
        {
            var day2PuzzleManager = new Day2PuzzleManager();

            Assert.Equal(7210630, await day2PuzzleManager.SolvePartOnePrivateAsync(12, 2));
        }

        [Fact]
        public async void Day2_Part2_ShouldBeCorrect()
        {
            var day2PuzzleManager = new Day2PuzzleManager();

            Assert.Equal(3892, await day2PuzzleManager.SolvePartTwoPrivateAsync());
        }

        [Fact]
        public async void Day5_Part1_ShouldBeCorrect()
        {
            var day5PuzzleManager = new Day5PuzzleManager();

            Assert.Equal(7265618, await day5PuzzleManager.SolvePrivateAsync(1));
        }

        [Fact]
        public async void Day5_Part2_ShouldBeCorrect()
        {
            var day5PuzzleManager = new Day5PuzzleManager();

            Assert.Equal(7731427, await day5PuzzleManager.SolvePrivateAsync(5));
        }

        [Fact]
        public async void Day7_Part1_ShouldBeCorrect()
        {
            var day7PuzzleManager = new Day7PuzzleManager();

            Assert.Equal(34686, await day7PuzzleManager.SolvePartOnePrivate());
        }

        [Fact]
        public async void Day7_Part2_ShouldBeCorrect()
        {
            var day7PuzzleManager = new Day7PuzzleManager();

            var phaseArray = new int[] { 7, 6, 5, 8, 9 };

            Assert.Equal(36384144, await day7PuzzleManager.RunAmplifierConfigFeedbackAsync(phaseArray));
        }
    }
}