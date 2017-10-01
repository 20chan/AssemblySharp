using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblySharp
{
    public class InstructionPattern
    {
        private static Type R = typeof(REG);
        private static Type M = typeof(MEM);
        private static Type I = typeof(int);

        /// <summary>
        /// mov <reg>,<reg>
        /// mov<reg>,<mem>
        /// mov<mem>,<reg>
        /// mov<reg>,<const>
        /// mov<mem>,<const>
        /// </summary>
        private static readonly object[][] PATTERNS = new object[][]
        {
            new object[] { ASM.mov, R, I },
        };

        /// <summary>
        /// Check pattern right and return count of parameter.
        /// </summary>
        /// <returns>Returns -1 if pattern wrong pattern. Else it return how many parameter it use.</returns>
        public static int CheckPattern(object[] code, int current)
        {
            if (!(code[0] is ASM)) throw new FormatException("Should be ASM");

            var asm = (ASM)code[0];

            foreach (var pattern in PATTERNS)
            {
                if ((ASM)pattern[0] != asm) continue;
                if (pattern.Length > code.Length - current) continue;

                bool match = true;
                for (int i = 1; i < pattern.Length; i++)
                    if (pattern[i] as Type != code[current + i].GetType())
                    {
                        // TODO: 패턴이 매칭되는게 여러개 있는 경우에 대한 해결법?
                        match = false; break;
                    }
                if (match) return pattern.Length;
            }

            return -1;
        }
    }
}
