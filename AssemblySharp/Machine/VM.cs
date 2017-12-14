using System;
using System.Collections.Generic;
using System.Reflection;

namespace AssemblySharp.Machine
{
    /// <summary>
    /// X86 Virtual Machine
    /// </summary>
    public partial class VM
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

        #region Instructions
        // 일단은 임시방편으로 인자를 받지 않는 콜백만 만들었다..
        public delegate void InstructionCallback();
        public class InstructionManager
        {
            private VM _vm;

            public readonly InstructionCallback NOP;

            public InstructionManager(VM vm)
            {
                _vm = vm;

                NOP = GetCallback(InstructionType.Nop);
            }

            private InstructionCallback GetCallback(InstructionType type)
                => throw new NotImplementedException();
        }
        public InstructionManager Instructions { get; private set; }
        private Dictionary<uint, InstructionCallback> _instructions;
        #endregion

        public VM()
        {
            Registers = new RegisterManager(this);
            Segments = new SegmentManager(this);
            Instructions = new InstructionManager(this);
            _instructions = new Dictionary<uint, InstructionCallback>();
            Reset();
            LoadInstructions();
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

        private void LoadInstructions()
        {
            foreach (MethodInfo method in (typeof(VM)).GetMethods())
            {
                foreach(var inst in method.GetCustomAttributes<Instruction>())
                {
                    var del = (InstructionCallback)Delegate.CreateDelegate(typeof(InstructionCallback), this, method);
                    _instructions.Add(inst.OpCode, del);
                }
            }
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
