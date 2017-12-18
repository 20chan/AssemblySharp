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
            private VM _vm;
            public ref dwordop EAX => ref _vm.EAX.DWord;
            public ref dwordop EBX => ref _vm.EBX.DWord;
            public ref dwordop ECX => ref _vm.ECX.DWord;
            public ref dwordop EDX => ref _vm.EDX.DWord;
            public ref dwordop ESP => ref _vm.ESP.DWord;
            public ref dwordop EBP => ref _vm.EBP.DWord;
            public ref dwordop ESI => ref _vm.ESI.DWord;
            public ref dwordop EDI => ref _vm.EDI.DWord;
            public ref dwordop EIP => ref _vm.EIP.DWord;

            public ref wordop AX => ref _vm.EAX.LowWord;
            public ref wordop BX => ref _vm.EBX.LowWord;
            public ref wordop CX => ref _vm.ECX.LowWord;
            public ref wordop DX => ref _vm.EDX.LowWord;
            public ref wordop SP => ref _vm.ESP.LowWord;
            public ref wordop BP => ref _vm.EBP.LowWord;
            public ref wordop SI => ref _vm.ESI.LowWord;
            public ref wordop DI => ref _vm.EDI.LowWord;
            public ref wordop IP => ref _vm.EIP.LowWord;

            public ref byteop AH => ref _vm.EAX.HighByte;
            public ref byteop AL => ref _vm.EAX.LowByte;
            public ref byteop BH => ref _vm.EBX.HighByte;
            public ref byteop BL => ref _vm.EBX.LowByte;
            public ref byteop CH => ref _vm.ECX.HighByte;
            public ref byteop CL => ref _vm.ECX.LowByte;
            public ref byteop DH => ref _vm.EDX.HighByte;
            public ref byteop DL => ref _vm.EDX.LowByte;

            public RegisterManager(VM vm)
            {
                _vm = vm;
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

            public bool CF => _vm.eflag.CF;
            public bool PF => _vm.eflag.PF;
            public bool AF => _vm.eflag.AF;
            public bool ZF => _vm.eflag.ZF;
            public bool SF => _vm.eflag.SF;
            public bool TF => _vm.eflag.TF;
            public bool IF => _vm.eflag.IF;
            public bool DF => _vm.eflag.DF;
            public bool OF => _vm.eflag.OF;
            public bool IOPL => _vm.eflag.IOPL;
            public bool IOPL2 => _vm.eflag.IOPL2;
            public bool NT => _vm.eflag.NT;
            public bool RF => _vm.eflag.RF;
            public bool VM => _vm.eflag.VM;
            public bool AC => _vm.eflag.AC;
            public bool VIF => _vm.eflag.VIF;
            public bool VIP => _vm.eflag.VIP;
            public bool ID => _vm.eflag.ID;

            public SegmentManager(VM vm)
            {
                _vm = vm;
            }
        }
        public SegmentManager Segments { get; private set; }
        private EFlags eflag;
        #endregion

        #region Instructions
        public class InstructionManager
        {
            private VM _vm;

            public InstructionManager(VM vm)
            {
                _vm = vm;
            }

            public T GetInstruction<T>(uint opcode) where T : class
                => GetInstruction(opcode) as T;

            public Delegate GetInstruction(uint opcode)
            {
                if (_vm._instructions0args.ContainsKey(opcode))
                    return _vm._instructions0args[opcode];
                else if (_vm._instructions1arg.ContainsKey(opcode))
                    return _vm._instructions1arg[opcode];
                else if (_vm._instructions2args.ContainsKey(opcode))
                    return _vm._instructions2args[opcode];
                else if (_vm._instructions3args.ContainsKey(opcode))
                    return _vm._instructions3args[opcode];
                else
                    throw new KeyNotFoundException("No instruction that matches opcode");
            }

            public T GetInstructionFromType<T>(InstructionType type) where T : class
                => GetInstructionFromType(type) as T;

            public Delegate GetInstructionFromType(InstructionType type)
                => _vm._instructionsFromType[type];
        }

        public InstructionManager Instructions { get; private set; }
        private Dictionary<InstructionType, Delegate> _instructionsFromType;
        private Dictionary<uint, InstructionCallback0args> _instructions0args;
        private Dictionary<uint, InstructionCallback1arg> _instructions1arg;
        private Dictionary<uint, InstructionCallback2args> _instructions2args;
        private Dictionary<uint, InstructionCallback3args> _instructions3args;
        #endregion

        #region Memory
        readonly int STACK_SIZE = 1024;
        Span<byte> _memory;
        #endregion

        public VM()
        {
            Registers = new RegisterManager(this);
            Segments = new SegmentManager(this);
            Instructions = new InstructionManager(this);
            _instructionsFromType = new Dictionary<InstructionType, Delegate>();
            _instructions0args = new Dictionary<uint, InstructionCallback0args>();
            _instructions1arg = new Dictionary<uint, InstructionCallback1arg>();
            _instructions2args = new Dictionary<uint, InstructionCallback2args>();
            _instructions3args = new Dictionary<uint, InstructionCallback3args>();
            Reset();
            LoadInstructions();
        }

        public void Reset()
        {
            EAX = (dwordop)0;
            EBX = (dwordop)0;
            ECX = (dwordop)0;
            EDX = (dwordop)0;
            ESI = (dwordop)0;
            EDI = (dwordop)0;
            ESP = (dwordop)0;
            EBP = (dwordop)0;
            EIP = (dwordop)0;

            eflag = 0;
            _memory = new byte[STACK_SIZE * 1024].AsSpan();
        }

        private void LoadInstructions()
        {
            foreach (MethodInfo method in (typeof(VM)).GetMethods())
            {
                foreach(var inst in method.GetCustomAttributes<Instruction>(true))
                {
                    Delegate del;
                    switch (method.GetParameters().Length)
                    {
                        case 0:
                            del = Delegate.CreateDelegate(typeof(InstructionCallback0args), this, method);
                            _instructions0args.Add(inst.OpCode, (InstructionCallback0args)del);
                            break;
                        case 1:
                            del = Delegate.CreateDelegate(typeof(InstructionCallback1arg), this, method);
                            _instructions1arg.Add(inst.OpCode, (InstructionCallback1arg)del);
                            break;
                        case 2:
                            del = Delegate.CreateDelegate(typeof(InstructionCallback2args), this, method);
                            _instructions2args.Add(inst.OpCode, (InstructionCallback2args)del);
                            break;
                        case 3:
                            del = Delegate.CreateDelegate(typeof(InstructionCallback3args), this, method);
                            _instructions3args.Add(inst.OpCode, (InstructionCallback3args)del);
                            break;
                        default:
                            throw new Exception("Too many instrucment parameters");
                    }
                    if (!_instructionsFromType.ContainsKey(inst.InstructionType))
                        _instructionsFromType.Add(inst.InstructionType, del);
                }
            }
        }

        private void WriteOperand(ref IOperand operand)
        {

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
