using System;

namespace AssemblySharp.Machine
{
    [AttributeUsage(AttributeTargets.Method)]
    public class Instruction : Attribute
    {
        public InstructionType InstructionType;
        public uint OpCode;
        public Instruction(InstructionType type, uint opcode)
        {
            OpCode = opcode;
            InstructionType = type;
        }
    }
}
