using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace AssemblySharp
{
    public delegate int IntDelegate();
    public static class X86Assembly
    {
        public static int ExecuteScript(params object[] code)
        {
            return (int)ExecuteScript(code, typeof(IntDelegate));
        }

        public static object ExecuteScript(object[] code, Type delegateType, params dynamic[] parameters)
        {
            string asmcode = "";

            for (int i = 0; i < code.Length; i++)
            {
                if (!(code[i] is ASM))
                    if (!(code[i] is int))
                        if (!(code[i] is REG))
                            if (!(code[i] is MEM))
                                if (!(code[i] is string))
                                    if (!(code[i] is Label))
                                        if (!(code[i] is RawAssemblyCode))
                                            throw new ArrayTypeMismatchException("Not supported type");

                var cnt = InstructionPattern.CheckPattern(code, i);
                if (cnt < 0)
                    throw new FormatException("Format error");
                asmcode += FromInline((ASM)code[i], code.Skip(i + 1).Take(cnt));
            }

            return RunMachineCode(CompileToMachineCode(asmcode), delegateType, parameters);
        }

        public static string FromInline(object[] code)
        {
            if (code[0] is ASM) return FromInline((ASM)code[0], code.Skip(1));
            if (code[0] is Label) return $"{(code[0] as Label).Name}:";
            if (code[0] is RawAssemblyCode) return (code[0] as RawAssemblyCode).Code;

            throw new Exception();
        }

        public static string FromInline(ASM inst, IEnumerable<object> parameters)
        {
            return parameters.Count() == 0 ? inst.ToString() : $"{inst} {string.Join(", ", parameters)}";
        }

        public static byte[] CompileToMachineCode(string asmcode)
        {
            // objdump를 사용하는 게 괜찮을 거 같은데...
            throw new NotImplementedException();
        }

        public static int RunMachineCode(byte[] bytecode)
            => (int)RunMachineCode(bytecode, typeof(IntDelegate));

        public static object RunMachineCode(byte[] bytecode, Type delegateType, params dynamic[] parameters)
        {
            var func = CompileMachineCode(bytecode, out var buf, delegateType);
            var res = func.DynamicInvoke(parameters);
            WinAPI.VirtualFree(buf, bytecode.Length, WinAPI.FreeType.Release);
            return res;
        }

        public static Delegate CompileMachineCode(byte[] bytecode, out IntPtr buffer, Type delegateType = null)
        {
            buffer = WinAPI.VirtualAlloc(IntPtr.Zero, (uint)bytecode.Length, WinAPI.AllocationType.Commit | WinAPI.AllocationType.Reserve, WinAPI.MemoryProtection.ExecuteReadWrite);
            Marshal.Copy(bytecode, 0, buffer, bytecode.Length);
            return Marshal.GetDelegateForFunctionPointer(buffer, delegateType ?? typeof(IntDelegate));
        }
    }
}
