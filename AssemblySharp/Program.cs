using System;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace AssemblySharp
{
    public static class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr VirtualAlloc(IntPtr lpAddress, uint dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool VirtualFree(IntPtr lpAddress, int dwSize, FreeType dwFreeType);

        [Flags]
        public enum FreeType
        {
            Decommit = 0x4000,
            Release = 0x8000,
        }

        [Flags]
        public enum AllocationType
        {
            Commit = 0x1000,
            Reserve = 0x2000,
            Decommit = 0x4000,
            Release = 0x8000,
            Reset = 0x80000,
            Physical = 0x400000,
            TopDown = 0x100000,
            WriteWatch = 0x200000,
            LargePages = 0x20000000
        }

        [Flags]
        public enum MemoryProtection
        {
            Execute = 0x10,
            ExecuteRead = 0x20,
            ExecuteReadWrite = 0x40,
            ExecuteWriteCopy = 0x80,
            NoAccess = 0x01,
            ReadOnly = 0x02,
            ReadWrite = 0x04,
            WriteCopy = 0x08,
            GuardModifierflag = 0x100,
            NoCacheModifierflag = 0x200,
            WriteCombineModifierflag = 0x400
        }
        const int 
            PAGE_READWRITE = 0x40,
            PROCESS_VM_OPERATION = 0x0008,
            PROCESS_VM_READ = 0x0010,
            PROCESS_VM_WRITE = 0x0020;

        const uint PAGE_EXECUTE_READWRITE = 0x40;

        delegate int IntDelegate();
        static byte[] code = new byte[] {
            // mov eax, 100
            0xb8,
            0x64, 0x00, 0x00, 0x00,
            // add eax, 200
            0x05,
            0xc8, 0x00, 0x00, 0x00,
            // ret
            0xc3
        };

        static void PrintElapsedTime(string name, Func<int> func, int count = 1000000)
        {
            var now = DateTime.Now;
            int total = 100000;
            for (int i = 0; i < total; i++)
                func();
            var ellapsed = DateTime.Now - now;
            Console.WriteLine($"{$"{total} times {name}".PadRight(40)} runned: {ellapsed.TotalMilliseconds}ms elapsed");
        }

        static void Main(string[] args)
        {
            var buffer = VirtualAlloc(IntPtr.Zero, (uint)code.Length, AllocationType.Commit, MemoryProtection.ExecuteReadWrite);
            Marshal.Copy(code, 0, buffer, code.Length);

            ptr = Marshal.GetDelegateForFunctionPointer<IntDelegate>(buffer);

            Expression<Func<int, int>> exp = (i) => 200 + i;
            compiled = exp.Compile();

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Phase {i}...");

                PrintElapsedTime(nameof(AllocAndExec), AllocAndExec);
                PrintElapsedTime(nameof(RunCompiledExpression), AllocAndExec);
                PrintElapsedTime(nameof(RunLambda), RunLambda);
            }

            VirtualFree(buffer, 0, FreeType.Release);

            Console.Read();
        }

        static IntDelegate ptr;
        static int AllocAndExec()
        {
            return ptr();
        }

        static Func<int, int> compiled;
        static int RunCompiledExpression()
        {
            return compiled(100);
        }

        static Func<int, int> lambd = (i) => i + 200;
        static int RunLambda()
        {
            return lambd(100);
        }
    }
}
