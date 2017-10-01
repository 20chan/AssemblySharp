using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblySharp
{
    public abstract class MEM
    {
        public MEM Byte => throw new NotImplementedException();
        public MEM Word => throw new NotImplementedException();
        public MEM DWord => throw new NotImplementedException();
        public MEM QWord => throw new NotImplementedException();
        public MEM TByte => throw new NotImplementedException();
    }
}
