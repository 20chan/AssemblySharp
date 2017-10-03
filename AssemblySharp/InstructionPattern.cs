using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblySharp
{
    public class InstructionPattern
    {
        private static PatternType R = PatternType.reg;
        private static PatternType M = PatternType.mem;
        private static PatternType C = PatternType.con;
        private static PatternType GetPatternType(object obj)
        {
            // TODO: REG를 사이즈에 따라 분리
            if (obj is REG) return PatternType.reg;
            if (obj is MEM) return PatternType.mem;
            if (obj is int) return PatternType.con;

            return PatternType.NONE;
        }
        /// <summary>
        /// mov <reg>,<reg>
        /// mov<reg>,<mem>
        /// mov<mem>,<reg>
        /// mov<reg>,<const>
        /// mov<mem>,<const>
        /// </summary>

        private static readonly Dictionary<ASM, PatternType[][]> PATTERNS = new Dictionary<ASM, PatternType[][]>()
        {
            { ASM.mov, new PatternType[][]
                {
                    new [] { R, R },
                    new [] { R, M },
                    new [] { M, R },
                    new [] { R, C },
                    new [] { M, C },
                }
            },
            { ASM.push, new PatternType[][]
                {
                    new [] { R },
                    new [] { M },
                    new [] { C },
                }
            },
        };
        /// <summary>
        /// Check pattern right and return count of parameter.
        /// </summary>
        /// <returns>Returns -1 if pattern wrong pattern. Else it return how many parameter it use.</returns>
        public static int CheckPattern(object[] code, int current)
        {
            if (!(code[current] is ASM)) throw new FormatException("Should be ASM");

            var asm = (ASM)code[current];

            foreach (var pattern in PATTERNS[asm])
            {
                if (pattern.Length > code.Length - current - 1) continue;
                bool match = true;
                for (int i = 0; i < pattern.Length; i++)
                    if (!pattern[i].HasFlag(GetPatternType(code[current + i + 1])))
                    {
                        match = false; break;
                    }
                if (match) return pattern.Length;
            }

            return -1;
        }
    }
}
