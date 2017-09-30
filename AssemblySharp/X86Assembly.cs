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
        public static object ExecuteScript(params object[] code)
        {
            // 델리게이트를 리턴해야 할텐데 그 리턴 타입을 지정해서 줘야 겠지
            // 아마 제너릭을 사용해서 어떻게든 잘 해봐야 하지 않을까
            // 일단은 ObjectDelegate로 object로 모든걸 하는 걸루
            throw new NotImplementedException();
        }

        public static byte[] CompileToMachineCode(string asmcode)
        {
            // objdump를 사용하는 게 괜찮을 거 같은데...
            throw new NotImplementedException();
        }

        public static object RunMachineCode(byte[] bytecode)
        {
            return CompileMachineCode(bytecode)();
        }

        public static ObjectDelegate CompileMachineCode(byte[] bytecode)
        {
            var buffer = WinAPI.VirtualAlloc(IntPtr.Zero, (uint)bytecode.Length, WinAPI.AllocationType.Commit, WinAPI.MemoryProtection.ExecuteReadWrite);
            Marshal.Copy(bytecode, 0, buffer, bytecode.Length);
            return Marshal.GetDelegateForFunctionPointer<ObjectDelegate>(buffer);
        }
    }
}
