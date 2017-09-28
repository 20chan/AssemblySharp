using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblySharp
{
    /// <summary>
    /// Registers
    /// </summary>
    public enum REG
    {
        // reg8
        AH,
        BH,
        CH,
        DH,
        AL,
        BL,
        CL,
        DL,
        // reg16
        AX,
        BX,
        CX,
        DX,
        // reg32
        EAX,
        EBX,
        ECX,
        EDX,
        ESI,
        EDI,
        ESP,
        EBP,
        // reg64
        RAX,
        RBX,
        RCX,
        RDX,
        RBP,
        RSI,
        RDI,
        RSP,
    }
}
