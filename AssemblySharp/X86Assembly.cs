using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblySharp
{
    public static class X86Assembly
    {
        public static void ExecuteScript()
        {
            // 델리게이트를 리턴해야 할텐데 그 리턴 타입을 지정해서 줘야 겠지
            // 아마 제너릭을 사용해서 어떻게든 잘 해봐야 하지 않을까
            throw new NotImplementedException();
        }

        public static byte[] CompileToMachineCode(string asmcode)
        {
            throw new NotImplementedException();
        }

        public static void RunMachineCode(byte[] bytecode)
        {
            throw new NotImplementedException();
        }
    }
}
