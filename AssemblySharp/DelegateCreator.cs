using System;
using System.Linq.Expressions;
using System.Reflection;

namespace AssemblySharp
{
    public static class DelegateCreator
    {
        private static readonly Func<Type[], Type> MakeNewCustomDelegate = (Func<Type[], Type>)Delegate.CreateDelegate(typeof(Func<Type[], Type>), typeof(Expression).Assembly.GetType("System.Linq.Expressions.Compiler.DelegateHelpers").GetMethod("MakeNewCustomDelegate", BindingFlags.NonPublic | BindingFlags.Static));

        public static Type NewDelegateType(Type ret, params Type[] parameters)
        {
            Type[] args = new Type[parameters.Length];
            parameters.CopyTo(args, 0);
            args[args.Length - 1] = ret;
            var type = typeof(Expression).Assembly.GetType("System.Linq.Expressions.Compiler.DelegateHelpers");
            var makeNewCustomDelegate = type.GetMethod("MakeNewCustomDelegate", BindingFlags.NonPublic | BindingFlags.Static);

            var del = (Func<Type[], Type>)Delegate.CreateDelegate(
                typeof(Func<Type[], Type>),
                makeNewCustomDelegate);
             return del(args);
        }
    }
}
