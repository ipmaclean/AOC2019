namespace AOC2019.IntCode
{
    internal class IntCodeComputer
    {
        private long _currentPosition = 0;
        private long _relativeBase = 0;
        private Dictionary<long, long> _intCodeProgram;
        private Queue<long> _inputs;

        public Queue<long> Outputs { get; private set; } = new Queue<long>();
        public Queue<long>? ExternalInputs { get; set; }
        public CancellationToken CancellationToken { get; set; }

        public event EventHandler? AwaitingInput;
        public event EventHandler? ProgramHalted;

        public IntCodeComputer(
            Dictionary<long, long> intCodeProgram,
            Queue<long>? inputs = null,
            Queue<long>? externalInputs = null,
            CancellationToken? cancellationToken = null)
        {
            _intCodeProgram = intCodeProgram;
            _inputs = inputs ?? new Queue<long>();
            ExternalInputs = externalInputs;
            CancellationToken = cancellationToken ?? new CancellationTokenSource().Token;
        }

        public async Task<Dictionary<long, long>> ProcessAsync(bool manualInputMode = false)
        {
            while ((Opcode)(_intCodeProgram[_currentPosition] % 100) != Opcode.HALT)
            {
                await ProcessInstructionAsync(manualInputMode);
            }
            ProgramHalted?.Invoke(this, EventArgs.Empty);
            return _intCodeProgram;
        }

        private async Task ProcessInstructionAsync(bool manualInputMode = false)
        {
            switch ((Opcode)(_intCodeProgram[_currentPosition] % 100))
            {
                case Opcode.ADD:
                    ProcessAddInstruction();
                    break;
                case Opcode.PRODUCT:
                    ProcessProductInstruction();
                    break;
                case Opcode.INPUT:
                    await ProcessInputInstructionAsync(manualInputMode);
                    break;
                case Opcode.OUTPUT:
                    ProcessOutputInstruction();
                    break;
                case Opcode.JUMP_IF_TRUE:
                    ProcessJumpIfTrueInstruction();
                    break;
                case Opcode.JUMP_IF_FALSE:
                    ProcessJumpIfFalseInstruction();
                    break;
                case Opcode.LESS_THAN:
                    ProcessLessThanInstruction();
                    break;
                case Opcode.EQUALS:
                    ProcessEqualsInstruction();
                    break;
                case Opcode.ADJUST_RELATIVE_BASE:
                    ProcessAdjustRelativeBaseInstruction();
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        private ParameterMode GetParameterMode(long parameter)
        {
            var divisor = (long)Math.Pow(10, parameter + 1);
            return (ParameterMode)(_intCodeProgram[_currentPosition] / divisor % 10);
        }

        private long GetParameterValue(long parameterPosition, ParameterMode? parameterMode = null)
        {
            parameterMode = parameterMode ?? GetParameterMode(parameterPosition);
            long positionToGet = -1;
            if (parameterMode == ParameterMode.POSITION)
            {
                positionToGet = GetParameterValue(parameterPosition, ParameterMode.IMMEDIATE);
            }
            else if (parameterMode == ParameterMode.IMMEDIATE)
            {
                positionToGet = _currentPosition + parameterPosition;
            }
            else if (parameterMode == ParameterMode.RELATIVE)
            {
                positionToGet = _relativeBase + GetParameterValue(parameterPosition, ParameterMode.IMMEDIATE);
            }

            if (positionToGet >= 0 && _intCodeProgram.ContainsKey(positionToGet))
            {
                return _intCodeProgram[positionToGet];
            }
            else
            {
                return 0;
            }
            throw new ArgumentException("Could not find the matching parameter mode.");
        }

        private void WriteParameterValue(long parameterPosition, long valueToWrite)
        {
            long positionToWrite = -1;
            var parameterMode = GetParameterMode(parameterPosition);
            if (parameterMode == ParameterMode.POSITION)
            {
                positionToWrite = GetParameterValue(parameterPosition, ParameterMode.IMMEDIATE);
                
            }
            else if (parameterMode == ParameterMode.RELATIVE)
            {
                positionToWrite = _relativeBase + GetParameterValue(parameterPosition, ParameterMode.IMMEDIATE);
            }
            if (_intCodeProgram.ContainsKey(positionToWrite))
            {
                _intCodeProgram[positionToWrite] = valueToWrite;
            }
            else
            {
                _intCodeProgram.Add(positionToWrite, valueToWrite);
            }
            
        }

        private void ProcessAddInstruction()
        {
            long sum = 0;
            for (var i = 1; i <= 2; i++)
            {
                var parameterValue = GetParameterValue(i);
                sum += parameterValue;
            }
            WriteParameterValue(3, sum);
            _currentPosition += 4;
        }

        private void ProcessProductInstruction()
        {
            long product = 1;
            for (var i = 1; i <= 2; i++)
            {
                var parameterValue = GetParameterValue(i);
                product *= parameterValue;
            }
            WriteParameterValue(3, product);
            _currentPosition += 4;
        }

        private async Task ProcessInputInstructionAsync(bool manualInputMode = false)
        {
            if (manualInputMode)
            {
                AwaitingInput?.Invoke(this, EventArgs.Empty);
                var input = Console.ReadKey();
                switch(input.Key)
                {
                    case ConsoleKey.LeftArrow:
                        WriteParameterValue(1, -1);
                        break;
                    case ConsoleKey.RightArrow:
                        WriteParameterValue(1, 1);
                        break;
                    case ConsoleKey.DownArrow:
                        WriteParameterValue(1, 0);
                        break;
                    default:
                        throw new ArgumentException("Input not recognised.");
                }
            }
            else
            {
                if (_inputs.Any())
                {
                    WriteParameterValue(1, _inputs.Dequeue());
                }
                else
                {
                    if (ExternalInputs == null)
                    {
                        throw new InvalidOperationException("No OtherIntCodeComputerOutputs defined.");
                    }
                    if (!ExternalInputs!.Any())
                    {
                        AwaitingInput?.Invoke(this, EventArgs.Empty);
                    }
                    while (!ExternalInputs!.Any())
                    {
                        await Task.Delay(5);
                        if (CancellationToken.IsCancellationRequested)
                        {
                            _intCodeProgram[_currentPosition] = (long)Opcode.HALT;
                            return;
                        }
                    }
                    WriteParameterValue(1, ExternalInputs!.Dequeue());
                }
            }
            _currentPosition += 2;
        }

        private void ProcessOutputInstruction()
        {
            var parameterValue = GetParameterValue(1);
            Outputs.Enqueue(parameterValue);
            _currentPosition += 2;
        }

        private void ProcessJumpIfTrueInstruction()
        {
            var parameterValue = GetParameterValue(1);
            var shouldJump = parameterValue != 0;

            if (shouldJump)
            {
                parameterValue = GetParameterValue(2);
                _currentPosition = parameterValue;
            }
            else
            {
                _currentPosition += 3;
            }
        }

        private void ProcessJumpIfFalseInstruction()
        {
            var parameterValue = GetParameterValue(1);
            var shouldJump = parameterValue == 0;

            if (shouldJump)
            {
                parameterValue = GetParameterValue(2);
                _currentPosition = parameterValue;
            }
            else
            {
                _currentPosition += 3;
            }
        }

        private void ProcessLessThanInstruction()
        {
            var isLessThan = false;
            long leftValue = 0;
            for (var i = 1; i <= 2; i++)
            {
                var parameterValue = GetParameterValue(i);
                var rightValue = parameterValue;
                isLessThan = leftValue < rightValue;
                leftValue = rightValue;
            }
            WriteParameterValue(3, isLessThan ? 1 : 0);
            _currentPosition += 4;
        }

        private void ProcessEqualsInstruction()
        {
            var isEqual = false;
            long leftValue = 0;
            for (var i = 1; i <= 2; i++)
            {
                var parameterValue = GetParameterValue(i);
                var rightValue = parameterValue;
                isEqual = leftValue == rightValue;
                leftValue = rightValue;
            }
            WriteParameterValue(3, isEqual ? 1 : 0);
            _currentPosition += 4;
        }
        
        private void ProcessAdjustRelativeBaseInstruction()
        {
            _relativeBase += GetParameterValue(1);
            _currentPosition += 2;
        }
    }
}
