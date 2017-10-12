using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblySharp
{
    [Flags]
    public enum PatternType
    {
        NONE = -1,
        reg32 = 1 << 0,
        reg16 = 1 << 1,
        reg8 = 1 << 2,
        reg = reg32 | reg16 | reg8,

        mem = 1 << 3,

        con32 = 1 << 4,
        con16 = 1 << 5,
        con8 = 1 << 6,
        con = con32 | con16 | con8,

        cl,

        label = 1 << 7,
        label_string = 1 << 8,
    }
}
