using AOC2019.IntCode;

namespace AOC2019.Day7
{
    internal class Day7PuzzleManager : PuzzleManager
    {
        public int[] IntCodeProgram { get; private set; }

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
            var phaseSettings = Permute(new int[] { 0, 1, 2, 3, 4 });
            var highestOutputSetting = 0;
            foreach (var phaseSetting in phaseSettings)
            {
                var phaseSettingArray = phaseSetting.ToArray();
                highestOutputSetting = Math.Max(highestOutputSetting, await RunAmplifierConfigAsync(phaseSettingArray));
            }
            // need to grab the highest output
            Console.WriteLine($"The solution to part one is '{highestOutputSetting}'.");
        }

        private async Task<int> RunAmplifierConfigAsync(int[] phaseSettings)
        {
            var outputSetting = 0;
            for (var i = 0; i < 5; i++)
            {
                var inputs = new Queue<int>();
                inputs.Enqueue(phaseSettings[i]);
                inputs.Enqueue(outputSetting);
                outputSetting = (await SolvePrivateAsync(inputs)).Last();
            }
            return outputSetting;
        }

        public async override Task SolvePartTwo()
        {
            var phaseSettings = Permute(new int[] { 5, 6, 7, 8, 9 });
            var highestOutputSetting = 0;
            var counter = 0;
            foreach (var phaseSetting in phaseSettings)
            {
                var phaseSettingArray = phaseSetting.ToArray();
                highestOutputSetting = Math.Max(highestOutputSetting, await RunAmplifierConfigFeedbackAsync(phaseSettingArray));
                counter++;
            }
            Console.WriteLine($"The solution to part two is '{highestOutputSetting}'.");
        }

        private async Task<int> RunAmplifierConfigFeedbackAsync(int[] phaseSettings)
        {
            var amplifiers = new IntCodeComputer[5];
            var previousAmplifierOutputs = new Queue<int>();
            var previousAmplifierCancellationToken = new CancellationTokenSource();
            for (var i = 0; i < 5; i++)
            {
                var amplifierCodeInput = (int[])IntCodeProgram.Clone();
                var amplifierInputs = new Queue<int>();
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

        private async Task<Queue<int>> SolvePrivateAsync(Queue<int> inputs)
        {
            var codeInput = (int[])IntCodeProgram.Clone();
            var intCodeComputer = new IntCodeComputer(codeInput, inputs);
            await intCodeComputer.ProcessAsync();
            return intCodeComputer.Outputs;
        }

        private Queue<int> CombineQueues(Queue<int> leadingQueue, Queue<int> trailingQueue)
        {
            while (trailingQueue.Count > 0)
            {
                leadingQueue.Enqueue(trailingQueue.Dequeue());
            }
            return leadingQueue;
        }

        private IList<IList<int>> Permute(int[] nums)
        {
            var list = new List<IList<int>>();
            return DoPermute(nums, 0, nums.Length - 1, list);
        }

        private IList<IList<int>> DoPermute(int[] nums, int start, int end, IList<IList<int>> list)
        {
            if (start == end)
            {
                // We have one of our possible n! solutions,
                // add it to the list.
                list.Add(new List<int>(nums));
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

        private void Swap(ref int a, ref int b)
        {
            var temp = a;
            a = b;
            b = temp;
        }
    }
}
