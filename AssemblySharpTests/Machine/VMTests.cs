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
            var vm = new VM();
            try
            {
                vm.Instructions.GetInstructionFromType<InstructionCallback0args>(InstructionType.Nop)();
            }
            catch (KeyNotFoundException)
            {
                Assert.Fail("NOP 명령어가 로드되지 않음");
            }
        }

        [TestMethod()]
        public void RegisterTest()
        {
            var vm = new VM();
            vm.Registers.EAX = 100;

            Assert.AreEqual((uint)100, (uint)vm.Registers.EAX);

            ref var eax = ref vm.Registers.EAX;
            eax = 50;
            Assert.AreEqual((uint)50, (uint)vm.Registers.EAX);

            var mov = vm.Instructions.GetInstructionFromType<InstructionCallback2args>(InstructionType.Mov);
            // mov(ref (IOperand)eax, 30);
            // ref 변수는 캐스팅이 안된다 쮸밤..
            // 하지만 나는 방법을 찾을 것이다 늘 그랬듯이
        }
    }
}