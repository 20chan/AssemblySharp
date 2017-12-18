using Microsoft.VisualStudio.TestTools.UnitTesting;
using AssemblySharp.Machine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblySharp.Machine.Tests
{
    [TestClass()]
    public class VMTests
    {
        [TestMethod()]
        public void InstructionsLoadTest()
        {
            try
            {
                var vm = new VM();
                vm.Instructions.GetInstructionFromType<InstructionCallback0args>(InstructionType.Nop)();
            }
            catch(KeyNotFoundException)
            {
                Assert.Fail("NOP 명령어가 로드되지 않음");
            }
        }
    }
}