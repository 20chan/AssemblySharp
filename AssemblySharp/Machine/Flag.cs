using System;

namespace AssemblySharp.Machine
{
    [Flags]
    public enum Flags : uint
    {
        ZF = 1 << 0,
        SF = 1 << 1,
    }
}
