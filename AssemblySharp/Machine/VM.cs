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
            public uint EAX => vm.EAX;
            public uint EBX => vm.EBX;
            public uint ECX => vm.ECX;
            public uint EDX => vm.EDX;
            public uint ESP => vm.ESP;
            public uint EBP => vm.EBP;
            public uint ESI => vm.ESI;
            public uint EDI => vm.EDI;
            public uint EIP => vm.EIP;

            public ushort AX => vm.EAX.LowWord;
            public ushort BX => vm.EBX.LowWord;
            public ushort CX => vm.ECX.LowWord;
            public ushort DX => vm.EDX.LowWord;
            public ushort SP => vm.ESP.LowWord;
            public ushort BP => vm.EBP.LowWord;
            public ushort SI => vm.ESI.LowWord;
            public ushort DI => vm.EDI.LowWord;
            public ushort IP => vm.EIP.LowWord;

            public byte AH => vm.EAX.HighByte;
            public byte AL => vm.EAX.LowByte;
            public byte BH => vm.EBX.HighByte;
            public byte BL => vm.EBX.LowByte;
            public byte CH => vm.ECX.HighByte;
            public byte CL => vm.ECX.LowByte;
            public byte DH => vm.EDX.HighByte;
            public byte DL => vm.EDX.LowByte;

            public RegisterManager(VM vm)
            {
                this.vm = vm;
            }
        }
        public RegisterManager Registers { get; private set; }
        private Register EAX, EBX, ECX, EDX;
        private Register ESI, EDI, ESP, EBP, EIP;
        #endregion


        #region Segments
        public class SegmentManager
        {
            private VM _vm;

            public bool ZF => HasFlag(Flags.ZF);
            public bool SF => HasFlag(Flags.SF);

            public SegmentManager(VM vm)
            {
                _vm = vm;
            }

            private bool HasFlag(Flags flag) => (_vm.flag & flag) == flag;
        }
        public SegmentManager Segments { get; private set; }
        private Flags flag;
        #endregion
        public VM()
        {
            Registers = new RegisterManager(this);
            Segments = new SegmentManager(this);
            Reset();
        }

        public void Reset()
        {
            EAX = 0;
            EBX = 0;
            ECX = 0;
            EDX = 0;
            ESI = 0;
            EDI = 0;
            ESP = 0;
            EBP = 0;
            EIP = 0;

            flag = 0;
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
