using Microsoft.VisualStudio.TestTools.UnitTesting;
using AssemblySharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        [TestMethod()]
        public void RunMachineCodeTest()
        {
            Assert.AreEqual(1,
                X86Assembly.RunMachineCode(new byte[] { 0xb8, 0x01, 0x00, 0x00, 0x00, 0xc3 }));
            Assert.AreEqual(145764,
                X86Assembly.RunMachineCode(new byte[] { 0xB8, 0x40, 0x01, 0x00, 0x00, 0x05, 0x24, 0x38, 0x02, 0x00, 0xc3 }));
        }
    }
}