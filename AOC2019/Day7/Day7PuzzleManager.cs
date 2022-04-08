using AOC2019.IntCode;

namespace AOC2019.Day7
{
    public class Day7PuzzleManager : PuzzleManager
    {
        public long[] IntCodeProgram { get; private set; }

        public Day7PuzzleManager()
        {
            var inputHelper = new Day7InputHelper(INPUT_FILE_NAME);
            IntCodeProgram = inputHelper.Parse();
        }

        public async override Task SolveBothParts()
        {
            await SolvePartOne();
            Console.WriteLine();
            await SolvePartTwo();
        }

        public async override Task SolvePartOne()
        {
            var solution = await SolvePartOnePrivate();
            Console.WriteLine($"The solution to part one is '{solution}'.");
        }

        public async Task<long> SolvePartOnePrivate()
        {
            var phaseSettings = Permute(new long[] { 0, 1, 2, 3, 4 });
            long highestOutputSetting = 0;
            foreach (var phaseSetting in phaseSettings)
            {
                var phaseSettingArray = phaseSetting.ToArray();
                highestOutputSetting = Math.Max(highestOutputSetting, await RunAmplifierConfigAsync(phaseSettingArray));
            }
            return highestOutputSetting;
        }

        private async Task<long> RunAmplifierConfigAsync(long[] phaseSettings)
        {
            long outputSetting = 0;
            for (var i = 0; i < 5; i++)
            {
                var inputs = new Queue<long>();
                inputs.Enqueue(phaseSettings[i]);
                inputs.Enqueue(outputSetting);
                outputSetting = (await ProcessAndReturnOutputs(inputs)).Last();
            }
            return outputSetting;
        }

        public async override Task SolvePartTwo()
        {
            var phaseSettings = Permute(new long[] { 5, 6, 7, 8, 9 });
            long highestOutputSetting = 0;
            foreach (var phaseSetting in phaseSettings)
            {
                var phaseSettingArray = phaseSetting.ToArray();
                highestOutputSetting = Math.Max(highestOutputSetting, await RunAmplifierConfigFeedbackAsync(phaseSettingArray));
            }
            Console.WriteLine($"The solution to part two is '{highestOutputSetting}'.");
        }

        public async Task<long> RunAmplifierConfigFeedbackAsync(long[] phaseSettings)
        {
            var amplifiers = new IntCodeComputer[5];
            var previousAmplifierOutputs = new Queue<long>();
            var previousAmplifierCancellationToken = new CancellationTokenSource();
            for (var i = 0; i < 5; i++)
            {
                var amplifierCodeInput = (long[])IntCodeProgram.Clone();
                var amplifierInputs = new Queue<long>();
                amplifierInputs.Enqueue(phaseSettings[i]);
                if (i == 0)
                {
                    amplifierInputs.Enqueue(0);
                }
                var amplifier = new IntCodeComputer(amplifierCodeInput, amplifierInputs, previousAmplifierOutputs);
                previousAmplifierOutputs = amplifier.Outputs;
                amplifiers[i] = amplifier;
            }
            amplifiers[0].ExternalInputs = previousAmplifierOutputs;

            var processes = new Task[5];

            for (var i = 0; i < 5; i++)
            {
                processes[i] = amplifiers[i].ProcessAsync();
            }
            await Task.WhenAll(processes);

            return amplifiers[4].Outputs.Last();
        }

        private async Task<Queue<long>> ProcessAndReturnOutputs(Queue<long> inputs)
        {
            var codeInput = (long[])IntCodeProgram.Clone();
            var intCodeComputer = new IntCodeComputer(codeInput, inputs);
            await intCodeComputer.ProcessAsync();
            return intCodeComputer.Outputs;
        }

        private IList<IList<long>> Permute(long[] nums)
        {
            var list = new List<IList<long>>();
            return DoPermute(nums, 0, nums.Length - 1, list);
        }

        private IList<IList<long>> DoPermute(long[] nums, long start, long end, IList<IList<long>> list)
        {
            if (start == end)
            {
                // We have one of our possible n! solutions,
                // add it to the list.
                list.Add(new List<long>(nums));
            }
            else
            {
                for (var i = start; i <= end; i++)
                {
                    Swap(ref nums[start], ref nums[i]);
                    DoPermute(nums, start + 1, end, list);
                    Swap(ref nums[start], ref nums[i]);
                }
            }

            return list;
        }

        private void Swap(ref long a, ref long b)
        {
            var temp = a;
            a = b;
            b = temp;
        }
    }
}
