using System.Runtime.InteropServices;

namespace AssemblySharp.Machine
{
    public interface IOperand
    {
        uint Value { get; set; }
    }

    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct dwordop : IOperand
    {
        [FieldOffset(0)]
        private uint _value;

        public uint Value
        {
            get => _value;
            set => _value = value;
        }

        public static implicit operator uint(dwordop op)
            => op.Value;

        public static implicit operator dwordop(uint value)
            => new dwordop() { Value = value };
    }

    [StructLayout(LayoutKind.Explicit, Size = 2)]
    public struct wordop : IOperand
    {
        [FieldOffset(0)]
        private ushort _value;

        public uint Value
        {
            get => _value;
            set => _value = (ushort)value;
        }

        public static implicit operator uint(wordop op)
            => op.Value;

        public static implicit operator wordop(uint value)
            => new wordop() { Value = value };
    }

    [StructLayout(LayoutKind.Explicit, Size = 1)]
    public struct byteop : IOperand
    {
        [FieldOffset(0)]
        private ushort _value;

        public uint Value
        {
            get => _value;
            set => _value = (ushort)value;
        }

        public static implicit operator uint(byteop op)
            => op.Value;

        public static implicit operator byteop(uint value)
            => new byteop() { Value = value };
    }
}
