﻿namespace AOC2019.IntCode
{
    internal enum Opcode
    {
        ADD = 1,
        PRODUCT = 2,
        INPUT = 3,
        OUTPUT = 4,
        JUMP_IF_TRUE = 5,
        JUMP_IF_FALSE = 6,
        LESS_THAN = 7,
        EQUALS = 8,
        ADJUST_RELATIVE_BASE = 9,
        HALT = 99
    }
}
