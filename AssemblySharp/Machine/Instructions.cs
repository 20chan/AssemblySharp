namespace AssemblySharp.Machine
{
    public enum InstructionType
    {
        Nop = 0,
    }
    public partial class VM
    {
        [Instruction(InstructionType.Nop ,0x90)]
        public void Nop()
        {

        }
    }
}
