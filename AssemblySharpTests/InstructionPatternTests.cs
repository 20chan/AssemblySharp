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
    public class InstructionPatternTests
    {
        [TestCategory("Pattern")]
        [TestMethod()]
        public void CheckPatternTest()
        {
            Assert.AreEqual(2,
                InstructionPattern.CheckPattern(new object[] { ASM.mov, REG.EAX, 10 }, 0));
            Assert.AreEqual(2,
                InstructionPattern.CheckPattern(new object[] { ASM.ret, ASM.ret, ASM.ret, ASM.mov, REG.EAX, 10 }, 3));

            Assert.AreEqual(0,
                InstructionPattern.CheckPattern(new object[] { ASM.ret }, 0));
            Assert.AreEqual(1,
                InstructionPattern.CheckPattern(new object[] { ASM.push, 1 }, 0));
            Assert.AreEqual(1,
                InstructionPattern.CheckPattern(new object[] { ASM.push, REG.EAX }, 0));
            Assert.AreEqual(1,
                InstructionPattern.CheckPattern(new object[] { ASM.push, REG.EAX.Ptr }, 0));
        }

        [TestCategory("Pattern")]
        [TestMethod()]
        public void CheckInvalidPatternTest()
        {
            Assert.AreEqual(-1,
                InstructionPattern.CheckPattern(new object[] { ASM.push }, 0));
            Assert.AreEqual(-1,
                InstructionPattern.CheckPattern(new object[] { ASM.mov, ASM.mov }, 0));
            Assert.AreEqual(-1,
                InstructionPattern.CheckPattern(new object[] { ASM.mov, 42, REG.EAX, }, 0));

            Assert.AreEqual(-1,
                InstructionPattern.CheckPattern(new object[] { ASM.push, 1, ASM.push, 1 }, 0));
        }
    }
}