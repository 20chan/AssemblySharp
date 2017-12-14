using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblySharp.Machine
{
    [Flags]
    public enum Flags : uint
    {
        ZF = 1 << 0,
        SF = 1 << 1,
    }
}
