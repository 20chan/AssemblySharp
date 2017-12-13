using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblySharp.Machine
{
    /// <summary>
    /// X86 Virtual Machine
    /// </summary>
    public class VM
    {
        #region Registers
        public class RegisterManager
        {
            private VM vm;
            public int EAX => vm.EAX;

            public RegisterManager(VM vm)
            {
                this.vm = vm;
            }
        }
        public RegisterManager Registers { get; private set; }
        private int EAX;
        #endregion
        public VM()
        {
            Registers = new RegisterManager(this);
            
        }

        public object ExecuteFunction(byte[] codes, Type funcType, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public void ExecuteCode(params byte[] codes)
        {
            throw new NotImplementedException();
        }
    }
}
