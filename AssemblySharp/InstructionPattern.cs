using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblySharp
{
    public class InstructionPattern
    {
        private static PatternType LS = PatternType.label_string;
        private static PatternType R = PatternType.reg;
        private static PatternType R32 = PatternType.reg32;
        private static PatternType M = PatternType.mem;
        private static PatternType C = PatternType.con;
        private static PatternType C8 = PatternType.con8;
        private static PatternType CL = PatternType.cl;
        private static PatternType GetPatternType(object obj)
        {
            // TODO: REG를 사이즈에 따라 분리
            if (obj is REG) return PatternType.reg;
            if (obj is MEM) return PatternType.mem;
            if (obj is int) return PatternType.con;
            if (obj is Label) return PatternType.label;
            if (obj is string) return PatternType.label_string;

            return PatternType.NONE;
        }

        private static Dictionary<ASM, PatternType[][]> _patterns;

        private static Dictionary<ASM, PatternType[][]> PATTERNS
        {
            get
            {
                if (_patterns == null)
                {
                    var defaultType = new PatternType[][]
                    {
                        new [] { R, R },
                        new [] { R, M },
                        new [] { M, R },
                        new [] { R, C },
                        new [] { M, C },
                    };
                    var RM = new PatternType[][]
                    {
                        new [] { R }, new [] { M }
                    };
                    var shift = new PatternType[][]
                    {
                        new [] { R, C8 },
                        new [] { M, C8 },
                        new [] { R, CL },
                        new [] { M, CL },
                    };
                    var label = new PatternType[][]
                    {
                        new [] { LS },
                    };

                    _patterns = new Dictionary<ASM, PatternType[][]>()
                    {
                        {
                            ASM.mov, defaultType
                        },
                        {
                            ASM.push, new PatternType[][]
                            {
                                new [] { R },
                                new [] { M },
                                new [] { C },
                            }
                        },
                        {
                            ASM.pop, RM
                        },
                        {
                            ASM.lea, new PatternType[][]
                            {
                                new [] { R, M }
                            }
                        },
                        {
                            ASM.add, defaultType
                        },
                        {
                            ASM.sub, defaultType
                        },
                        {
                            ASM.inc, RM
                        },
                        {
                            ASM.dec, RM
                        },
                        {
                            ASM.imul, new PatternType[][]
                            {
                                new [] { R32, R32 },
                                new [] { R32, M },
                                new [] { R32, R32, C },
                                new [] { R32, M, C },
                            }
                        },
                        {
                            ASM.idiv, new PatternType[][]
                            {
                                new [] { R32 },
                                new [] { M },
                            }
                        },
                        {
                            ASM.and, defaultType
                        },
                        {
                            ASM.or, defaultType
                        },
                        {
                            ASM.xor, defaultType
                        },
                        {
                            ASM.not, RM
                        },
                        {
                            ASM.neg, RM
                        },
                        {
                            ASM.shl, shift
                        },
                        {
                            ASM.shr, shift
                        },
                        {
                            ASM.jmp, label
                        },
                        {  ASM.je, label },
                        {  ASM.jne, label },
                        {  ASM.jz, label },
                        {  ASM.jg, label },
                        {  ASM.jge, label },
                        {  ASM.jl, label },
                        {  ASM.jle, label },
                        {
                            ASM.cmp, new PatternType[][]
                            {
                                new [] { R, R },
                                new [] { R, M },
                                new [] { M, R },
                                new [] { R, C },
                            }
                        },
                        {
                            ASM.call, label
                        },
                        {
                            ASM.loop, label
                        },
                        {
                            ASM.ret, new PatternType[][]
                            {
                                new PatternType[] { }
                            }
                        },
                    };
                }
                return _patterns;
            }
        }

        /// <summary>
        /// Check pattern right and return count of parameter.
        /// </summary>
        /// <returns>Returns -1 if pattern wrong pattern. Else it return how many parameter it use.</returns>
        public static int CheckPattern(object[] code, int current)
        {
            if (code[current] is Label || code[current] is RawAssemblyCode) return 0;
            if (!(code[current] is ASM)) throw new FormatException("Should be ASM");

            var asm = (ASM)code[current];

            foreach (var pattern in PATTERNS[asm])
            {
                if (pattern.Length >= code.Length - current) continue;
                bool match = true;
                for (int i = 0; i < pattern.Length; i++)
                {
                    if (!pattern[i].HasFlag(GetPatternType(code[current + i + 1])))
                    {
                        match = false; break;
                    }
                }
                if (match) return pattern.Length;
            }

            return -1;
        }
    }
}
