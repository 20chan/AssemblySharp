using System;

namespace AssemblySharp.Machine
{
    public delegate void InstructionCallback0args();
    public delegate void InstructionCallback1arg(ref IOperand op1);
    public delegate void InstructionCallback2args(ref IOperand op1, ref IOperand op2);
    public delegate void InstructionCallback3args(ref IOperand op1, ref IOperand op2, ref IOperand op3);

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
