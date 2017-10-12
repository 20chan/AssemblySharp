using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblySharp
{
    public enum ASM
    {
        #region Data Movement
        mov,
        push,
        pop,
        lea,
        #endregion
        #region Arithmetic and Logic
        add,
        sub,
        inc,
        dec,
        imul,
        idiv,
        and,
        or,
        xor,
        not,
        neg,
        shl,
        shr,
        #endregion
        #region Control Flow
        jmp,
        je, jne, jz, jg, jge, jl, jle,
        cmp,
        call,
        loop,
        ret,
        #endregion
    }
}
