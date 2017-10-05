using Microsoft.VisualStudio.TestTools.UnitTesting;
using AssemblySharp;
using System;
using System.Runtime.InteropServices;

namespace AssemblySharp.Tests
{
    [TestClass()]
    public class X86AssemblyTests
    {
        [TestMethod()]
        public void ExecuteScriptTest()
        {
            Assert.AreEqual(100,
                X86Assembly.ExecuteScript(
                    ASM.mov, REG.EAX, 100,
                    ASM.ret));

            Assert.AreEqual(200,
                X86Assembly.ExecuteScript(
                    ASM.mov, REG.EAX, 100,
                    ASM.add, REG.EAX, 200,
                    ASM.ret));

            Assert.AreEqual(42,
                X86Assembly.ExecuteScript(
                    ASM.push, 42,
                    ASM.pop, REG.EAX,
                    ASM.ret));

            unsafe
            {
                int a = 84;
                Assert.AreEqual(84,
                    X86Assembly.ExecuteScript(
                        ASM.mov, REG.EAX, (int)&a,
                        ASM.sub, REG.EAX, 4,
                        ASM.mov, REG.EAX, (REG.EAX + 4).Ptr,
                        ASM.ret));
            }
        }

        [TestMethod()]
        public void ConvertInlineAssemblyTest()
        {
            Assert.AreEqual("ret",
                X86Assembly.FromInline(ASM.ret, new object[] { }));
            Assert.AreEqual("mov EAX, 10",
                X86Assembly.FromInline(ASM.mov, new object[] { REG.EAX, 10 }));
            Assert.AreEqual("mov EAX, [EBX+4]",
                X86Assembly.FromInline(ASM.mov, new object[] { REG.EAX, (REG.EBX + 4).Ptr }));
            Assert.AreEqual("mov WORD PTR [EBX], 2",
                X86Assembly.FromInline(ASM.mov, new object[] { REG.EBX.Ptr.Word, 2 }));
        }

        [TestMethod()]
        public void ConvertInlineAssemblyErrorTest()
        {
            Assert.ThrowsException<FormatException>(() =>
                X86Assembly.FromInline(ASM.mov, new object[] { ASM.mov }));
            Assert.ThrowsException<FormatException>(() =>
                X86Assembly.FromInline(ASM.mov, new object[] { ASM.mov }));
        }

        [TestMethod()]
        public void CompileToMachineCodeTest()
        {
            Assert.Fail();
        }


        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void CPUID0Delegate(byte* buffer);

        [TestMethod()]
        public unsafe void RunMachineCodeTest()
        {
            Assert.AreEqual(1,
                X86Assembly.RunMachineCode(new byte[] { 0xb8, 0x01, 0x00, 0x00, 0x00, 0xc3 }));
            Assert.AreEqual(145764,
                X86Assembly.RunMachineCode(new byte[] { 0xB8, 0x40, 0x01, 0x00, 0x00, 0x05, 0x24, 0x38, 0x02, 0x00, 0xc3 }));

            byte[] buffer = new byte[12];
            fixed (byte* newBuffer = &buffer[0])
            {
                try
                {
                    X86Assembly.RunMachineCode(new byte[]
                    {
                        0x53,                      // push   %ebx
                        0x31, 0xc0,                // xor    %eax,%eax
                        0x0f, 0xa2,                // cpuid
                        0x8b, 0x44, 0x24, 0x08,    // mov    0x8(%esp),%eax
                        0x89, 0x18,                // mov    %ebx,0x0(%eax)
                        0x89, 0x50, 0x04,          // mov    %edx,0x4(%eax)
                        0x89, 0x48, 0x08,          // mov    %ecx,0x8(%eax)
                        0x5b,                      // pop    %ebx
                        0xc3                       // ret
                    }, typeof(CPUID0Delegate), new IntPtr(newBuffer));

                    CollectionAssert.AreNotEqual(buffer, new byte[12]);
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.Message);
                }
            }
        }
    }
}
