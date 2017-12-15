using System.Runtime.InteropServices;

namespace AssemblySharp.Machine
{
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct EFlags
    {
        [FieldOffset(0)]
        private uint Value;
        #region Status Flags
        /// <summary>
        /// Carry flag
        /// </summary>
        [FieldOffset(0)]
        public bool CF;
        /// <summary>
        /// Parity flag
        /// </summary>
        [FieldOffset(2)]
        public bool PF;
        /// <summary>
        /// Auxiliary Carry flag
        /// </summary>
        [FieldOffset(4)]
        public bool AF;
        /// <summary>
        /// Zero flag
        /// </summary>
        [FieldOffset(6)]
        public bool ZF;
        /// <summary>
        /// Sign flag
        /// </summary>
        [FieldOffset(7)]
        public bool SF;
        /// <summary>
        /// Overflow flag
        /// </summary>
        [FieldOffset(11)]
        public bool OF;
        #endregion

        /// <summary>
        /// Direction flag
        /// </summary>
        [FieldOffset(10)]
        public bool DF;

        #region System Flags and IOPL
        /// <summary>
        /// Trap flag
        /// </summary>
        [FieldOffset(8)]
        public bool TF;
        /// <summary>
        /// Interrupt enable flag
        /// </summary>
        [FieldOffset(9)]
        public bool IF;
        /// <summary>
        /// I/O previlege level field
        /// </summary>
        [FieldOffset(12)]
        public bool IOPL;
        [FieldOffset(13)]
        public bool IOPL2;
        /// <summary>
        /// Nested task flag
        /// </summary>
        [FieldOffset(14)]
        public bool NT;
        /// <summary>
        /// Resume flag
        /// </summary>
        [FieldOffset(16)]
        public bool RF;
        /// <summary>
        /// Virtual-8086 mode flag
        /// </summary>
        [FieldOffset(17)]
        public bool VM;
        /// <summary>
        /// Alignment check (or access check control) flag
        /// </summary>
        [FieldOffset(18)]
        public bool AC;
        /// <summary>
        /// Virtual interrupt flag
        /// </summary>
        [FieldOffset(19)]
        public bool VIF;
        /// <summary>
        /// Virtual interrupt pending flag
        /// </summary>
        [FieldOffset(20)]
        public bool VIP;
        /// <summary>
        /// Identification flag
        /// </summary>
        [FieldOffset(21)]
        public bool ID;
        #endregion

        public static implicit operator uint(EFlags register)
            => register.Value;

        public static implicit operator EFlags(uint value)
            => new EFlags() { Value = value };
    }
}
