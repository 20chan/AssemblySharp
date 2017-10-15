using Microsoft.VisualStudio.TestTools.UnitTesting;
using AssemblySharp;
using System;
using System.Runtime.InteropServices;

namespace AssemblySharp.Tests
{
    [TestClass()]
    public class X86AssemblyTests
    {
        [TestCategory("Execute")]
        [TestMethod()]
        public void ExecuteScriptTest()
        {
            Assert.AreEqual(100,
                X86Assembly.ExecuteScript(
                    ASM.mov, REG.EAX, 100,
                    ASM.ret));

            Assert.AreEqual(300,
                X86Assembly.ExecuteScript(
                    ASM.mov, REG.EAX, 100,
                    ASM.add, REG.EAX, 200,
                    ASM.ret));

            Assert.AreEqual(42,
                X86Assembly.ExecuteScript(
                    ASM.push, 42,
                    ASM.pop, REG.EAX,
                    ASM.ret));

            int i = 100;
            Assert.AreEqual(5050,
                X86Assembly.ExecuteScript(
                    ASM.mov, REG.EAX, 0,
                    ASM.mov, REG.ECX, i,
                    new Label("myloop"),
                        ASM.add, REG.EAX, REG.ECX,
                    ASM.loop, "myloop",
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

                byte[] buffer = new byte[12];
                fixed (byte* newBuffer = &buffer[0])
                {
                    try
                    {
                        X86Assembly.ExecuteScript(new object[]
                        {
                            ASM.push, REG.EBX,
                            ASM.xor, REG.EAX, REG.EAX,
                            new RawAssemblyCode("cpuid"),
                            ASM.mov, REG.EAX, (REG.ESP + 8).Ptr,
                            ASM.mov, (REG.EAX + 0).Ptr, REG.EBX,
                            ASM.mov, (REG.EAX + 4).Ptr, REG.EDX,
                            ASM.mov, (REG.EAX + 8).Ptr, REG.ECX,
                            ASM.pop, REG.EBX,
                            ASM.ret,
                        }, typeof(CPUID0Delegate), new IntPtr(newBuffer));

                        CollectionAssert.AreNotEqual(buffer, new byte[12]);
                    }
                    catch
                    {
                        Assert.Fail();
                    }
                }
            }
        }

        [TestCategory("Expression")]
        [TestMethod()]
        public void IsValidExpressionForMemoryTest()
        {
            Assert.IsTrue(REG.EAX.IsValidExpressionForMemory());
            Assert.IsTrue((REG.EAX + 4).IsValidExpressionForMemory());
            Assert.IsTrue((REG.EAX + REG.EBX).IsValidExpressionForMemory());
            Assert.IsTrue((REG.EAX + REG.EBX + 4).IsValidExpressionForMemory());
            Assert.IsTrue((REG.EAX + REG.EBX * 2).IsValidExpressionForMemory());
            Assert.IsTrue((REG.EAX + REG.EBX * 2 + 100).IsValidExpressionForMemory());
            Assert.IsTrue((REG.EAX + REG.EBX * 1 + 1).IsValidExpressionForMemory());
            Assert.IsTrue((REG.EAX * 8).IsValidExpressionForMemory());
            Assert.IsTrue((REG.EAX * 8 + 100).IsValidExpressionForMemory());

            Assert.IsFalse((REG.EAX + REG.EBX + REG.ECX).IsValidExpressionForMemory());
            Assert.IsFalse((REG.EAX * 7).IsValidExpressionForMemory());
            Assert.IsFalse((REG.EAX * 2 + REG.EBX * 2).IsValidExpressionForMemory());
            Assert.IsFalse((REG.EAX + 4 + REG.EBX + 3).IsValidExpressionForMemory());
        }

        [TestCategory("Expression")]
        [TestMethod()]
        public void ConvertInlineAssemblyTest()
        {
            Assert.AreEqual("ret",
                X86Assembly.FromInline(ASM.ret, new object[] { }));
            Assert.AreEqual("mov eax, 10",
                X86Assembly.FromInline(ASM.mov, new object[] { REG.EAX, 10 }));
            Assert.AreEqual("mov byte ptr [eax], 10",
                X86Assembly.FromInline(ASM.mov, new object[] { REG.EAX.Ptr.Byte, 10 }));
            Assert.AreEqual("mov word ptr [eax], 10",
                X86Assembly.FromInline(ASM.mov, new object[] { REG.EAX.Ptr.Word, 10 }));
            Assert.AreEqual("mov dword ptr [eax], 10",
                X86Assembly.FromInline(ASM.mov, new object[] { REG.EAX.Ptr, 10 }));
            Assert.AreEqual("mov dword ptr [eax], 10",
                X86Assembly.FromInline(ASM.mov, new object[] { REG.EAX.Ptr.DWord, 10 }));
            Assert.AreEqual("mov qword ptr [eax], 10",
                X86Assembly.FromInline(ASM.mov, new object[] { REG.EAX.Ptr.QWord, 10 }));
            Assert.AreEqual("mov dword ptr [eax+4], 10",
                X86Assembly.FromInline(ASM.mov, new object[] { (REG.EAX + 4).Ptr, 10 }));
            Assert.AreEqual("mov dword ptr [eax+ebx], 10",
                X86Assembly.FromInline(ASM.mov, new object[] { (REG.EAX + REG.EBX).Ptr, 10 }));
            Assert.AreEqual("mov dword ptr [eax+ebx*8+128], eax",
                X86Assembly.FromInline(ASM.mov, new object[] { (REG.EAX + REG.EBX * 8 + 128).Ptr.DWord, REG.EAX }));
            Assert.AreEqual("mov dword ptr [ebx*8+128], eax",
                X86Assembly.FromInline(ASM.mov, new object[] { (REG.EBX * 8 + 128).Ptr.DWord, REG.EAX }));
            Assert.AreEqual("mov dword ptr [ebx*2], eax",
                X86Assembly.FromInline(ASM.mov, new object[] { (REG.EBX * 2).Ptr.DWord, REG.EAX }));
            Assert.AreEqual("mov eax, dword ptr [ebx+4]",
                X86Assembly.FromInline(ASM.mov, new object[] { REG.EAX, (REG.EBX + 4).Ptr }));
        }

        [TestCategory("Expression")]
        [TestMethod()]
        public void ConvertInlineAssemblyRawCodeTest()
        {
            string code = "Nobody care this project";
            Assert.AreEqual(code, X86Assembly.FromInline(new object[]
            {
                new RawAssemblyCode(code)
            }));
        }

        [TestCategory("Expression")]
        [TestMethod()]
        public void GetCodeTest()
        {
            Assert.AreEqual(
                $"mov eax, 100\nret\n",
                  X86Assembly.GetCode(
                      ASM.mov, REG.EAX, 100,
                      ASM.ret));

            Assert.AreEqual(
                "mov eax, 100\nadd eax, 200\nret\n",
                X86Assembly.GetCode(
                    ASM.mov, REG.EAX, 100,
                    ASM.add, REG.EAX, 200,
                    ASM.ret));

            Assert.AreEqual(
                "push 42\npop eax\nret\n",
                X86Assembly.GetCode(
                    ASM.push, 42,
                    ASM.pop, REG.EAX,
                    ASM.ret));

            Assert.AreEqual(
                "mov eax, 0\nmov ecx, 100\nmyloop:\nadd eax, ecx\nloop myloop\nret\n",
                X86Assembly.GetCode(
                    ASM.mov, REG.EAX, 0,
                    ASM.mov, REG.ECX, 100,
                    new Label("myloop"),
                        ASM.add, REG.EAX, REG.ECX,
                    ASM.loop, "myloop",
                    ASM.ret));


            Assert.AreEqual(
                @"push ebx
mov eax, 0
cpuid
mov eax, dword ptr [esp+8]
mov dword ptr [eax+0], ebx
mov dword ptr [eax+4], edx
mov dword ptr [eax+8], ecx
pop ebx
ret
".Replace("\r\n", "\n"),
                X86Assembly.GetCode(new object[]
                {
                    ASM.push, REG.EBX,
                    ASM.mov, REG.EAX, 0,
                    new RawAssemblyCode("cpuid"),
                    ASM.mov, REG.EAX, (REG.ESP + 8).Ptr,
                    ASM.mov, (REG.EAX + 0).Ptr, REG.EBX,
                    ASM.mov, (REG.EAX + 4).Ptr, REG.EDX,
                    ASM.mov, (REG.EAX + 8).Ptr, REG.ECX,
                    ASM.pop, REG.EBX,
                    ASM.ret,
                }));
        }

        [TestCategory("Compile")]
        [TestMethod()]
        public void CompileToMachineCodeTest()
        {
            CollectionAssert.AreEqual(new byte[]
            {
                0x53,
                0x31, 0xc0,
                0x0f, 0xa2,
                0x8b, 0x44, 0x24, 0x08,
                0x89, 0x18,
                0x89, 0x50, 0x04,
                0x89, 0x48, 0x08,
                0x5b,
                0xc3
            },
            X86Assembly.CompileToMachineCode(new object[]{
                ASM.push, REG.EBX,
                ASM.xor, REG.EAX, REG.EAX,
                new RawAssemblyCode("cpuid"),
                ASM.mov, REG.EAX, (REG.ESP + 8).Ptr,
                ASM.mov, (REG.EAX + 0).Ptr, REG.EBX,
                ASM.mov, (REG.EAX + 4).Ptr, REG.EDX,
                ASM.mov, (REG.EAX + 8).Ptr, REG.ECX,
                ASM.pop, REG.EBX,
                ASM.ret,
            }));
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void CPUID0Delegate(byte* buffer);

        [TestCategory("Execute")]
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
