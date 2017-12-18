using System.Runtime.InteropServices;

namespace AssemblySharp.Machine
{
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct Register
    {
        [FieldOffset(0)]
        public dwordop DWord;
        [FieldOffset(0)]
        public wordop HighWord;
        [FieldOffset(2)]
        public wordop LowWord;
        [FieldOffset(2)]
        public byteop HighByte;
        [FieldOffset(3)]
        public byteop LowByte;

        public static implicit operator dwordop(Register register)
            => register.DWord;

        public static implicit operator Register(dwordop value)
            => new Register() { DWord = value };
    }
}
