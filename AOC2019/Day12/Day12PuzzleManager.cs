using System.Text;

namespace AOC2019.Day12
{
    internal class Day12PuzzleManager : PuzzleManager
    {
        public int[][] MoonPositions { get; private set; }
        public int[][] MoonVelocities { get; private set; }


#pragma warning disable CS8618
        public Day12PuzzleManager()
#pragma warning restore CS8618
        {
            Reset();
        }

        public override void Reset()
        {
            var inputHelper = new Day12InputHelper(INPUT_FILE_NAME);
            MoonPositions = inputHelper.Parse();
            MoonVelocities = GetStartingMoonVelocities();
        }

        private int[][] GetStartingMoonVelocities()
        {
            var moonVelocities = new int[4][];
            for (int i = 0; i < moonVelocities.Length; i++)
            {
                moonVelocities[i] = new int[3];
            }
            return moonVelocities;
        }

        private void ApplyGravity(int moonOne, int moonTwo)
        {
            var moonOnePosition = MoonPositions[moonOne];
            var moonOneVelocity = MoonVelocities[moonOne];
            var moonTwoPosition = MoonPositions[moonTwo];
            var moonTwoVelocity = MoonVelocities[moonTwo];

            for (int i = 0; i < moonOnePosition.Length; i++)
            {
                if (moonOnePosition[i] < moonTwoPosition[i])
                {
                    moonOneVelocity[i]++;
                    moonTwoVelocity[i]--;
                }
                else if (moonOnePosition[i] > moonTwoPosition[i])
                {
                    moonOneVelocity[i]--;
                    moonTwoVelocity[i]++;
                }
            }
        }

        private void UpdatePositions()
        {
            for (int i = 0; i < MoonPositions.Length; i++)
            {
                for (var j = 0; j < MoonPositions[i].Length; j++)
                {
                    MoonPositions[i][j] += MoonVelocities[i][j];
                }
            }
        }

        private int CalculateSystemEnergy()
        {
            var totalEnergy = 0;
            for (var i = 0; i < MoonPositions.Length; i++)
            {
                totalEnergy += CalculateMoonEnergy(i);
            }
            return totalEnergy;
        }

        private int CalculateMoonEnergy(int i)
        {
            var potentialEnergy = 0;
            foreach (var coordinate in MoonPositions[i])
            {
                potentialEnergy += Math.Abs(coordinate);
            }

            var kineticEnergy = 0;
            foreach (var coordinate in MoonVelocities[i])
            {
                kineticEnergy += Math.Abs(coordinate);
            }

            return potentialEnergy * kineticEnergy;
        }

        public override Task SolveBothParts()
        {
            SolvePartOne();
            Console.WriteLine();
            SolvePartTwo();
            return Task.CompletedTask;
        }

        public override Task SolvePartOne()
        {
            var counter = 0;
            while (counter++ < 1000)
            {
                SimulateStep();
            }
            Console.WriteLine($"The solution to part one is '{CalculateSystemEnergy()}'.");
            return Task.CompletedTask;
        }

        private void SimulateStep()
        {
            for (var i = 0; i < MoonPositions.Length - 1; i++)
            {
                for (var j = i + 1; j < MoonPositions.Length; j++)
                {
                    ApplyGravity(i, j);
                }
            }
            UpdatePositions();
        }

        public override Task SolvePartTwo()
        {
            // 1) The axes (x,y,z) are totally independent. So it suffices to find the period for each axis separately. Then the answer is the lcm of these.
            // 2) Since each state has a unique parent, the first repeat must be a repeat of state 0.
            var stepsToReset = new long[3];
            for (var i = 0; i < 3; i++)
            {
                var firstState = CreateUniverseHashSingleAxis(i);
                long counter = 0;
                var foundPreviousState = false;
                while (!foundPreviousState)
                {
                    counter++;
                    SimulateStepSingleAxis(i);
                    var universeHash = CreateUniverseHashSingleAxis(i);
                    if (universeHash == firstState)
                    {
                        foundPreviousState = true;
                        stepsToReset[i] = counter;
                    }
                }
            }
            var solution = GetLcmOfSteps(stepsToReset);
            
            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
        }

        private void SimulateStepSingleAxis(int axis)
        {
            for (var i = 0; i < MoonPositions.Length - 1; i++)
            {
                for (var j = i + 1; j < MoonPositions.Length; j++)
                {
                    ApplyGravitySingleAxis(i, j, axis);
                }
            }
            UpdatePositionsSingleAxis(axis);
        }

        private void ApplyGravitySingleAxis(int moonOne, int moonTwo, int axis)
        {
            var moonOnePosition = MoonPositions[moonOne][axis];
            var moonTwoPosition = MoonPositions[moonTwo][axis];

            if (moonOnePosition < moonTwoPosition)
            {
                MoonVelocities[moonOne][axis]++;
                MoonVelocities[moonTwo][axis]--;
            }
            else if (moonOnePosition > moonTwoPosition)
            {
                MoonVelocities[moonOne][axis]--;
                MoonVelocities[moonTwo][axis]++;
            }
        }

        private void UpdatePositionsSingleAxis(int axis)
        {
            for (int i = 0; i < MoonPositions.Length; i++)
            {
                MoonPositions[i][axis] += MoonVelocities[i][axis];
            }
        }

        public string CreateUniverseHashSingleAxis(int axis)
        {
            var sb = new StringBuilder();
            var prefix = "";
            foreach (var position in MoonPositions)
            {

                sb.Append(prefix + position[axis].ToString());
                prefix = ",";

            }
            foreach (var velocity in MoonVelocities)
            {
                sb.Append(prefix + velocity[axis].ToString());
            }
            return sb.ToString();
        }

        private long GetLcmOfSteps(long[] steps)
        {
            if (steps.Length == 0)
            {
                return 0;
            }
            if (steps.Length == 1)
            {
                return steps[0];
            }

            var currentLcm = steps[0];
            for (var i = 1; i < steps.Length; i++)
            {
                currentLcm = LCM(currentLcm, steps[i]);
            }
            return currentLcm;
        }

        private long GCF(long a, long b)
        {
            while (b != 0)
            {
                var temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        private long LCM(long a, long b)
        {
            return (a / GCF(a, b)) * b;
        }
    }
}
