namespace AOC2019.IntCode
{
    internal class IntCodeComputer
    {
        private int _currentPosition = 0;
        private int[] _intCodeProgram;
        private Queue<int> _inputs;
        public Queue<int> Outputs { get; private set; } = new Queue<int>();
        public Queue<int>? ExternalInputs { get; set; }

        public IntCodeComputer(int[] intCodeProgram, Queue<int>? inputs = null, Queue<int>? externalInputs = null)
        {
            _intCodeProgram = intCodeProgram;
            _inputs = inputs ?? new Queue<int>();
            ExternalInputs = externalInputs;
        }

        public async Task<int[]> ProcessAsync()
        {
            while (_intCodeProgram[_currentPosition] % 100 != (int)Opcode.HALT)
            {
                await ProcessInstructionAsync();
            }
            return _intCodeProgram;
        }

        private async Task ProcessInstructionAsync()
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
                    await ProcessInputInstructionAsync();
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
                default:
                    throw new InvalidOperationException();
            }
        }

        private ParameterMode GetParameterMode(int parameter)
        {
            var divisor = (int)Math.Pow(10, parameter + 1);
            return (ParameterMode)(_intCodeProgram[_currentPosition] / divisor % 10);
        }

        private void ProcessAddInstruction()
        {
            var sum = 0;
            for (var i = 1; i <= 2; i++)
            {
                var parameterMode = GetParameterMode(i);
                if (parameterMode == ParameterMode.POSITION)
                {
                    sum += _intCodeProgram[_intCodeProgram[_currentPosition + i]];
                }
                else if (parameterMode == ParameterMode.IMMEDIATE)
                {
                    sum += _intCodeProgram[_currentPosition + i];
                }
            }

            _intCodeProgram[_intCodeProgram[_currentPosition + 3]] = sum;
            _currentPosition += 4;
        }

        private void ProcessProductInstruction()
        {
            var product = 1;
            for (var i = 1; i <= 2; i++)
            {
                var parameterMode = GetParameterMode(i);
                if (parameterMode == ParameterMode.POSITION)
                {
                    product *= _intCodeProgram[_intCodeProgram[_currentPosition + i]];
                }
                else if (parameterMode == ParameterMode.IMMEDIATE)
                {
                    product *= _intCodeProgram[_currentPosition + i];
                }
            }

            _intCodeProgram[_intCodeProgram[_currentPosition + 3]] = product;
            _currentPosition += 4;
        }

        private async Task ProcessInputInstructionAsync()
        {
            if (_inputs.Any())
            {
                _intCodeProgram[_intCodeProgram[_currentPosition + 1]] = _inputs.Dequeue();
            }
            else
            {
                if (ExternalInputs == null)
                {
                    throw new InvalidOperationException("No OtherIntCodeComputerOutputs defined.");
                }
                while (!ExternalInputs!.Any())
                {
                    await Task.Delay(5);
                }
                _intCodeProgram[_intCodeProgram[_currentPosition + 1]] = ExternalInputs!.Dequeue();
            }
            
            _currentPosition += 2;
        }

        private void ProcessOutputInstruction()
        {
            var output = 0;

            var parameterMode = GetParameterMode(1);
            if (parameterMode == ParameterMode.POSITION)
            {
                output = _intCodeProgram[_intCodeProgram[_currentPosition + 1]];
            }
            else if (parameterMode == ParameterMode.IMMEDIATE)
            {
                output = _intCodeProgram[_currentPosition + 1];
            }
            Outputs.Enqueue(output);
            _currentPosition += 2;
        }

        private void ProcessJumpIfTrueInstruction()
        {
            var shouldJump = false;
            var parameterMode = GetParameterMode(1);
            if (parameterMode == ParameterMode.POSITION)
            {
                shouldJump = _intCodeProgram[_intCodeProgram[_currentPosition + 1]] != 0;
            }
            else if (parameterMode == ParameterMode.IMMEDIATE)
            {
                shouldJump = _intCodeProgram[_currentPosition + 1] != 0;
            }
            
            if (shouldJump)
            {
                parameterMode = GetParameterMode(2);
                if (parameterMode == ParameterMode.POSITION)
                {
                    _currentPosition = _intCodeProgram[_intCodeProgram[_currentPosition + 2]];
                }
                else if (parameterMode == ParameterMode.IMMEDIATE)
                {
                    _currentPosition = _intCodeProgram[_currentPosition + 2];
                }
            }
            else
            {
                _currentPosition += 3;
            }
        }

        private void ProcessJumpIfFalseInstruction()
        {
            var shouldJump = false;
            var parameterMode = GetParameterMode(1);
            if (parameterMode == ParameterMode.POSITION)
            {
                shouldJump = _intCodeProgram[_intCodeProgram[_currentPosition + 1]] == 0;
            }
            else if (parameterMode == ParameterMode.IMMEDIATE)
            {
                shouldJump = _intCodeProgram[_currentPosition + 1] == 0;
            }

            if (shouldJump)
            {
                parameterMode = GetParameterMode(2);
                if (parameterMode == ParameterMode.POSITION)
                {
                    _currentPosition = _intCodeProgram[_intCodeProgram[_currentPosition + 2]];
                }
                else if (parameterMode == ParameterMode.IMMEDIATE)
                {
                    _currentPosition = _intCodeProgram[_currentPosition + 2];
                }
            }
            else
            {
                _currentPosition += 3;
            }
        }

        private void ProcessLessThanInstruction()
        {
            var isLessThan = false;
            var leftValue = 0;
            var rightValue = 0;
            for (var i = 1; i <= 2; i++)
            {
                var parameterMode = GetParameterMode(i);
                if (parameterMode == ParameterMode.POSITION)
                {
                    rightValue = _intCodeProgram[_intCodeProgram[_currentPosition + i]];
                }
                else if (parameterMode == ParameterMode.IMMEDIATE)
                {
                    rightValue = _intCodeProgram[_currentPosition + i];
                }

                isLessThan = leftValue < rightValue;
                leftValue = rightValue;
            }

            if (isLessThan)
            {
                _intCodeProgram[_intCodeProgram[_currentPosition + 3]] = 1;
            }
            else
            {
                _intCodeProgram[_intCodeProgram[_currentPosition + 3]] = 0;
            }
            _currentPosition += 4;
        }

        private void ProcessEqualsInstruction()
        {
            var isEquals = false;
            var leftValue = 0;
            var rightValue = 0;
            for (var i = 1; i <= 2; i++)
            {
                var parameterMode = GetParameterMode(i);
                if (parameterMode == ParameterMode.POSITION)
                {
                    rightValue = _intCodeProgram[_intCodeProgram[_currentPosition + i]];
                }
                else if (parameterMode == ParameterMode.IMMEDIATE)
                {
                    rightValue = _intCodeProgram[_currentPosition + i];
                }

                isEquals = leftValue == rightValue;
                leftValue = rightValue;
            }

            if (isEquals)
            {
                _intCodeProgram[_intCodeProgram[_currentPosition + 3]] = 1;
            }
            else
            {
                _intCodeProgram[_intCodeProgram[_currentPosition + 3]] = 0;
            }
            _currentPosition += 4;
        }
    }
}
