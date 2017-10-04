using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace AssemblySharp
{
    public delegate object ObjectDelegate();
    public static class X86Assembly
    {
        public static T ExecuteScript<T>(params object[] code)
        {
            // 델리게이트를 리턴해야 할텐데 그 리턴 타입을 지정해서 줘야 겠지
            // 아마 제너릭을 사용해서 어떻게든 잘 해봐야 하지 않을까
            // 일단은 ObjectDelegate로 object로 모든걸 하는 걸루

            string asmcode = "";

            for (int i = 0; i < code.Length; i++)
            {
                if (!(code[i] is ASM))
                    if (!(code[i] is int))
                        if (!(code[i] is REG))
                            if (!(code[i] is MEM))
                                throw new ArrayTypeMismatchException("Not supported type");
                var cnt = InstructionPattern.CheckPattern(code, i);
                if (cnt < 0)
                    throw new FormatException("Format error");

                asmcode += FromInline((ASM)code[i], code.Skip(i + 1).Take(cnt));
            }

            return RunMachineCode<T>(CompileToMachineCode(asmcode));
        }

        public static string FromInline(ASM inst, IEnumerable<object> parameters)
        {
            return $"{inst} {string.Join(", ", parameters)}";
        }

        public static byte[] CompileToMachineCode(string asmcode)
        {
            // objdump를 사용하는 게 괜찮을 거 같은데...
            throw new NotImplementedException();
        }

        public static T RunMachineCode<T>(byte[] bytecode)
        {
            return ((dynamic)CompileMachineCode<T>(bytecode))();
        }

        public static Delegate CompileMachineCode<T>(byte[] bytecode)
        {
            return CompileMachineCode(bytecode, DelegateCreator.NewDelegateType(typeof(T)));
        }

        public static Delegate CompileMachineCode(byte[] bytecode, Type type)
        {
            var buffer = WinAPI.VirtualAlloc(IntPtr.Zero, (uint)bytecode.Length, WinAPI.AllocationType.Commit, WinAPI.MemoryProtection.ExecuteReadWrite);
            Marshal.Copy(bytecode, 0, buffer, bytecode.Length);
            return Marshal.GetDelegateForFunctionPointer(buffer, type);
        }
    }
}
