using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblySharp
{
    public class MEM
    {
        public MEM Byte => new MEM(_register, MemoryStructureType.Byte);
        public MEM Word => new MEM(_register, MemoryStructureType.Word);
        public MEM DWord => new MEM(_register, MemoryStructureType.DWord);
        public MEM QWord => new MEM(_register, MemoryStructureType.QWord);
        public MEM TByte => new MEM(_register, MemoryStructureType.TByte);

        protected REG _register;
        protected MemoryStructureType _type;

        public MEM(REG reg, MemoryStructureType type = MemoryStructureType.DWord)
        {
            if (!IsValidRegistry(reg)) throw new ArgumentException();
            _register = reg;
            _type = type;
        }

        private static bool IsValidRegistry(REG reg) => reg.IsValidExpressionForMemory();

        public override string ToString()
        {
            return $"{_type.ToString().ToLower()} ptr [{_register.ExpressionToString()}]";
        }
    }
}
