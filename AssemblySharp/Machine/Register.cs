using System.Runtime.InteropServices;

namespace AssemblySharp.Machine
{
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct Register
    {
        [FieldOffset(0)]
        public uint DWord;
        [FieldOffset(0)]
        public ushort HighWord;
        [FieldOffset(2)]
        public ushort LowWord;
        [FieldOffset(2)]
        public byte HighByte;
        [FieldOffset(3)]
        public byte LowByte;

        public static implicit operator uint(Register register)
            => register.DWord;

        public static implicit operator Register(uint value)
            => new Register() { DWord = value };
    }
}
