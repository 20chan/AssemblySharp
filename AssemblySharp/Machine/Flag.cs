using System;

namespace AssemblySharp.Machine
{
    [Flags]
    public enum Flags : uint
    {
        #region Status Flags
        /// <summary>
        /// Carry flag
        /// </summary>
        CF = 1 << 0,
        /// <summary>
        /// Parity flag
        /// </summary>
        PF = 1 << 2,
        /// <summary>
        /// Auxiliary Carry flag
        /// </summary>
        AF = 1 << 4,
        /// <summary>
        /// Zero flag
        /// </summary>
        ZF = 1 << 6,
        /// <summary>
        /// Sign flag
        /// </summary>
        SF = 1 << 7,
        /// <summary>
        /// Overflow flag
        /// </summary>
        OF = 1 << 11,
        #endregion

        /// <summary>
        /// Direction flag
        /// </summary>
        DF = 1 << 10,

        #region System Flags and IOPL
        /// <summary>
        /// Trap flag
        /// </summary>
        TF = 1 << 8,
        /// <summary>
        /// Interrupt enable flag
        /// </summary>
        IF = 1 << 9,
        /// <summary>
        /// I/O previlege level field
        /// </summary>
        IOPL = 1 << 12,
        IOPL2 = 1 << 13,
        /// <summary>
        /// Nested task flag
        /// </summary>
        NT = 1 << 14,
        /// <summary>
        /// Resume flag
        /// </summary>
        RF = 1 << 16,
        /// <summary>
        /// Virtual-8086 mode flag
        /// </summary>
        VM = 1 << 17,
        /// <summary>
        /// Alignment check (or access check control) flag
        /// </summary>
        AC = 1 << 18,
        /// <summary>
        /// Virtual interrupt flag
        /// </summary>
        VIF = 1 << 19,
        /// <summary>
        /// Virtual interrupt pending flag
        /// </summary>
        VIP = 1 << 20,
        /// <summary>
        /// Identification flag
        /// </summary>
        ID = 1 << 21,
        #endregion
    }
}
