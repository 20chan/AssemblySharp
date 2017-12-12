using System;
using System.Runtime.InteropServices;

namespace AssemblySharp.Machine
{
    /// <summary>
    /// Execute machine code by allocating memory.
    /// </summary>
    public class DynamicInvoke
    {
        public static object ExecuteMethod(byte[] codes, Type delegateType, dynamic[] parameters)
        {
            var func = CompileMachineCode(codes, out var buf, delegateType);
            var res = func.DynamicInvoke(parameters);
            WinAPI.VirtualFree(buf, codes.Length, WinAPI.FreeType.Release);
            return res;
        }

        public static Delegate CompileMachineCode(byte[] codes, out IntPtr buffer, Type delegateType = null)
        {
            buffer = WinAPI.VirtualAlloc(IntPtr.Zero, (uint)codes.Length, WinAPI.AllocationType.Commit | WinAPI.AllocationType.Reserve, WinAPI.MemoryProtection.ExecuteReadWrite);
            Marshal.Copy(codes, 0, buffer, codes.Length);
            return Marshal.GetDelegateForFunctionPointer(buffer, delegateType ?? typeof(IntDelegate));
        }
    }
}
