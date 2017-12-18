namespace AssemblySharp.Machine
{
    public partial class VM
    {
        [Instruction(InstructionType.Mov, 0x88)]
        public void Mov(ref IOperand dest, ref IOperand source)
        {
            dest.Value = source.Value;
        }
    }
}
