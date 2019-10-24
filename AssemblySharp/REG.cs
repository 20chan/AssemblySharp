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
            _exp = Expression.Parameter(GetRegisterType(type), type.ToString().ToLower());
        }

        private REG(Expression exp)
        {
            _exp = exp;
        }

        public bool IsValidExpressionForMemory()
            => IsValidExpressionForMemory(_exp);

        public string ExpressionToString()
            => ExpressionToString(_exp);

        #region Expression

        public static bool IsValidExpressionForMemory(Expression exp)
        {
            if (exp.NodeType == ExpressionType.Parameter)
                return true;
            else if (exp.NodeType == ExpressionType.Add)
            {
                var bi = exp as BinaryExpression;
                return CheckRootAddExpression(bi.Left, bi.Right);
            }
            else if (exp.NodeType == ExpressionType.Multiply)
            {
                // index (*) scale
                return CheckIndexScaleExpression(exp as BinaryExpression);
            }
            else return false;
        }

        private static bool CheckRootAddExpression(Expression left, Expression right)
        {
            /*
             * One of next five states.
             * base (+) index
             * base (+) displacement
             * base (+) index * scale
             * base + index (+) displacement
             * base + index * scale (+) displacement
             * index * scale (+) displacement
             */
            if (left.NodeType == ExpressionType.Parameter)
            {
                if (right.NodeType == ExpressionType.Parameter)
                    return true;
                else if (right.NodeType == ExpressionType.Constant)
                    return true;
                else if (right.NodeType == ExpressionType.Multiply)
                    return CheckIndexScaleExpression(right as BinaryExpression);
                else return false;
            }
            else if (left.NodeType == ExpressionType.Add)
            {
                var inner = left as BinaryExpression;
                /*
                 * (base + index) + displacement
                 * (base + index * scale) + displacement
                 */
                if (right.NodeType != ExpressionType.Constant) return false;
                else if (inner.Left.NodeType != ExpressionType.Parameter) return false;
                else if (inner.Right.NodeType == ExpressionType.Parameter) return true;
                else if (inner.Right.NodeType == ExpressionType.Multiply)
                    return CheckIndexScaleExpression(inner.Right as BinaryExpression);
                else return false;
            }
            else if (left.NodeType == ExpressionType.Multiply)
            {
                if (right.NodeType == ExpressionType.Constant)
                    return CheckIndexScaleExpression(left as BinaryExpression);
                else return false;
            }
            else return false;
        }

        private static bool CheckIndexScaleExpression(BinaryExpression exp)
        {
            if (exp.Left.NodeType != ExpressionType.Parameter)
                return false;
            if (exp.Right.NodeType != ExpressionType.Constant)
                return false;
            var scale = (int)(exp.Right as ConstantExpression).Value;
            return new[] { 1, 2, 4, 8 }.Contains(scale);
        }

        public static string ExpressionToString(Expression exp)
        {
            if (exp.NodeType == ExpressionType.Parameter)
                return (exp as ParameterExpression).Name;
            else if (exp.NodeType == ExpressionType.Add)
            {
                var bi = exp as BinaryExpression;
                return RootAddExpressionToString(bi.Left, bi.Right);
            }
            else if (exp.NodeType == ExpressionType.Multiply)
            {
                return IndexScaleExpressionToString(exp as BinaryExpression);
            }
            else throw new ArgumentException();
        }

        private static string RootAddExpressionToString(Expression left, Expression right)
        {
            /* base (+) index
             * base (+) displacement
             * base (+) index * scale
             * base + index (+) displacement
             * base + index * scale (+) displacement
             * index * scale (+) displacement
             */
            if (left.NodeType == ExpressionType.Parameter)
            {
                var lparam = left as ParameterExpression;
                if (right.NodeType == ExpressionType.Parameter)
                    // base (+) index
                    return $"{lparam.Name}+{(right as ParameterExpression).Name}";
                else if (right.NodeType == ExpressionType.Constant)
                    // base (+) displacement
                    return $"{lparam.Name}+{(right as ConstantExpression).Value}";
                else if (right.NodeType == ExpressionType.Multiply)
                    // base (+) index * scale
                    return IndexScaleExpressionToString(right as BinaryExpression);
                else throw new ArgumentException();
            }
            else if (left.NodeType == ExpressionType.Add)
            {
                var inner = left as BinaryExpression;
                /*
                 * (base + index) + displacement
                 * (base + index * scale) + displacement
                 */
                var cons = right as ConstantExpression;
                if (inner.Right.NodeType == ExpressionType.Add)
                    return $"{(inner.Left as ParameterExpression).Name}+{(inner.Right as ParameterExpression).Name}+{cons.Value}";
                else if (inner.Right.NodeType == ExpressionType.Multiply)
                    return $"{(inner.Left as ParameterExpression).Name}+{IndexScaleExpressionToString(inner.Right as BinaryExpression)}+{cons.Value}";
                else throw new ArgumentException();
            }
            else if (left.NodeType == ExpressionType.Multiply)
            {
                // index * scale (+) displacement
                return $"{IndexScaleExpressionToString(left as BinaryExpression)}+{(right as ConstantExpression).Value}";
            }
            else throw new ArgumentException();
        }

        private static string IndexScaleExpressionToString(BinaryExpression exp)
        {
            var name = (exp.Left as ParameterExpression).Name;
            var scale = (int)(exp.Right as ConstantExpression).Value;
            return $"{name}*{scale}";
        }

        #endregion

        /// <summary>
        /// 포인터 안에 들어가는 연산의 큰 틀은 [base + index * scale + displacement]
        /// base, index는 레지스터, scale은 1, 2, 4, 8중 하나
        /// </summary>
        public MEM Ptr => new MEM(this);

        public static REG operator +(REG left, REG right)
        {
            return new REG(Expression.Add(left._exp, right._exp));
        }

        public static REG operator +(REG left, int right)
        {
            var rExp = Expression.Constant(right, typeof(int));
            return new REG(Expression.Add(left._exp, rExp));
        }

        public static REG operator *(REG left, int right)
        {
            var rExp = Expression.Constant(right, typeof(int));
            return new REG(Expression.Multiply(left._exp, rExp));
        }

        public override string ToString()
        {
            return _exp.ToString().ToLower();
        }
    }
}
