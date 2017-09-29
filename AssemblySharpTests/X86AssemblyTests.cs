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
        }

        [TestMethod()]
        public void CompileToMachineCodeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RunMachineCodeTest()
        {
            Assert.Fail();
        }
    }
}