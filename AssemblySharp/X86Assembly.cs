using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AssemblySharp
{
    public delegate int IntDelegate();
    public static class X86Assembly
    {
        public static int ExecuteScript(params object[] code)
        {
            return (int)ExecuteScript(code, typeof(IntDelegate));
        }

        public static object ExecuteScript(object[] code, Type delegateType, params dynamic[] parameters)
        {
            return RunMachineCode(CompileToMachineCode(code), delegateType, parameters);
        }

        public static string GetCode(params object[] code)
        {
            string asmcode = "";

            for (int i = 0; i < code.Length; i++)
            {
                if (!(code[i] is ASM))
                    if (!(code[i] is int))
                        if (!(code[i] is REG))
                            if (!(code[i] is MEM))
                                if (!(code[i] is string))
                                    if (!(code[i] is Label))
                                        if (!(code[i] is RawAssemblyCode))
                                            throw new ArrayTypeMismatchException("Not supported type");

                var cnt = InstructionPattern.CheckPattern(code, i);
                if (cnt < 0)
                    throw new FormatException("Format error");
                asmcode += $"{FromInline(code.Skip(i).Take(cnt + 1).ToArray())}\n";
                i += cnt;
            }

            return asmcode;
        }

        public static string FromInline(params object[] code)
        {
            switch(code[0])
            {
                case ASM asm:
                    return FromInline(asm, code.Skip(1));
                case Label label:
                    return $"{label.Name}:";
                case RawAssemblyCode raw:
                    return raw.Code;
                default:
                    throw new Exception("Unexpected Type");
            }
        }

        public static string FromInline(ASM inst, IEnumerable<object> parameters)
        {
            return parameters.Count() == 0 ? inst.ToString() : $"{inst} {string.Join(", ", parameters)}";
        }

        public static byte[] CompileToMachineCode(object[] code)
            => CompileToMachineCode(GetCode(code));

        public static byte[] CompileToMachineCode(string asmcode)
        {
            var fullcode = $".intel_syntax noprefix\n_main:\n{asmcode}";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "temp");
            var asmfile = $"{path}.s";
            var objfile = $"{path}.o";
            File.WriteAllText(asmfile, fullcode, new UTF8Encoding(false));
            var psi = new ProcessStartInfo("gcc", $"-m32 -c {asmfile} -o {objfile}")
            {
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            var gcc = Process.Start(psi);
            gcc.WaitForExit();
            if (gcc.ExitCode == 0)
            {
                psi.FileName = "objdump";
                psi.Arguments = $"-z -M intel -d {objfile}";
                var objdump = Process.Start(psi);
                objdump.WaitForExit();
                if (objdump.ExitCode == 0)
                {
                    var output = objdump.StandardOutput.ReadToEnd();
                    var matches = Regex.Matches(output, @"\b[a-fA-F0-9]{2}(?!.*:)\b");
                    var result = new List<byte>();
                    foreach (Match match in matches)
                    {
                        result.Add((byte)Convert.ToInt32(match.Value, 16));
                    }

                    return result.TakeWhile(b => b != 0x90).ToArray();
                }
            }
            else
            {
                var err = gcc.StandardError.ReadToEnd();
            }

            throw new ArgumentException();
        }

        public static int RunMachineCode(byte[] bytecode)
            => (int)RunMachineCode(bytecode, typeof(IntDelegate));
        
        public static object RunMachineCode(byte[] codes, Type delegateType, params dynamic[] parameters)
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
