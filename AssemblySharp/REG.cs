using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace AssemblySharp
{
    /// <summary>
    /// Registers
    /// </summary>
    public class REG
    {
        #region General Registers
        public static REG
            // reg8
            AH = new REG(_reg.AH),
            BH = new REG(_reg.BH),
            CH = new REG(_reg.CH),
            DH = new REG(_reg.DH),
            AL = new REG(_reg.AL),
            BL = new REG(_reg.BL),
            CL = new REG(_reg.CL),
            DL = new REG(_reg.DL),
            // reg16
            AX = new REG(_reg.AX),
            BX = new REG(_reg.BX),
            CX = new REG(_reg.CX),
            DX = new REG(_reg.DX),
            // reg32
            EAX = new REG(_reg.EAX),
            EBX = new REG(_reg.EBX),
            ECX = new REG(_reg.ECX),
            EDX = new REG(_reg.EDX),
            ESI = new REG(_reg.ESI),
            EDI = new REG(_reg.EDI),
            ESP = new REG(_reg.ESP),
            EBP = new REG(_reg.EBP),
            // reg64
            RAX = new REG(_reg.RAX),
            RBX = new REG(_reg.RBX),
            RCX = new REG(_reg.RCX),
            RDX = new REG(_reg.RDX),
            RBP = new REG(_reg.RBP),
            RSI = new REG(_reg.RSI),
            RDI = new REG(_reg.RDI),
            RSP = new REG(_reg.RSP);
        #endregion
        private enum _reg
        {
            NONE,
            // reg8
            AH,
            BH,
            CH,
            DH,
            AL,
            BL,
            CL,
            DL,
            // reg16
            AX,
            BX,
            CX,
            DX,
            // reg32
            EAX,
            EBX,
            ECX,
            EDX,
            ESI,
            EDI,
            ESP,
            EBP,
            // reg64
            RAX,
            RBX,
            RCX,
            RDX,
            RBP,
            RSI,
            RDI,
            RSP
        }
        private Expression _exp;

        private static Type GetRegisterType(_reg reg)
        {
            if (reg == _reg.NONE) return null;
            if (reg <= _reg.DL) return typeof(byte);
            if (reg <= _reg.DX) return typeof(short);
            if (reg <= _reg.EBP) return typeof(int);
            if (reg <= _reg.RSP) return typeof(long);

            throw new KeyNotFoundException();
        }

        private _reg _type;
        private REG(_reg type)
        {
            _type = type;
            _exp = Expression.Parameter(GetRegisterType(type), type.ToString());
        }

        private REG(Expression exp)
        {
            _exp = exp;
        }

        public object Ptr => throw new NotImplementedException();

        public static REG operator +(REG left, REG right)
        {
            return new REG(Expression.Add(left._exp, right._exp));
        }

        public static REG operator +(REG left, int right)
        {
            var rExp = Expression.Constant(right, typeof(int));
            return new REG(Expression.Add(left._exp, rExp));
        }

        public static REG operator -(REG left, REG right)
        {
            return new REG(Expression.Subtract(left._exp, right._exp));
        }

        public static REG operator -(REG left, int right)
        {
            var rExp = Expression.Constant(right, typeof(int));
            return new REG(Expression.Subtract(left._exp, rExp));
        }
    }
}
