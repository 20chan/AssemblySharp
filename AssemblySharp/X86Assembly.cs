using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading.Tasks;
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
            if (code[0] is ASM) return FromInline((ASM)code[0], code.Skip(1));
            if (code[0] is Label) return $"{(code[0] as Label).Name}:";
            if (code[0] is RawAssemblyCode) return (code[0] as RawAssemblyCode).Code;

            throw new Exception();
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

        public static object RunMachineCode(byte[] bytecode, Type delegateType, params dynamic[] parameters)
        {
            var func = CompileMachineCode(bytecode, out var buf, delegateType);
            var res = func.DynamicInvoke(parameters);
            WinAPI.VirtualFree(buf, bytecode.Length, WinAPI.FreeType.Release);
            return res;
        }

        public static Delegate CompileMachineCode(byte[] bytecode, out IntPtr buffer, Type delegateType = null)
        {
            buffer = WinAPI.VirtualAlloc(IntPtr.Zero, (uint)bytecode.Length, WinAPI.AllocationType.Commit | WinAPI.AllocationType.Reserve, WinAPI.MemoryProtection.ExecuteReadWrite);
            Marshal.Copy(bytecode, 0, buffer, bytecode.Length);
            return Marshal.GetDelegateForFunctionPointer(buffer, delegateType ?? typeof(IntDelegate));
        }
    }
}
