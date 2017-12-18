# AssemblySharp

- [English](Readme.md)
- [한국어](Readme.ko.md)

C/C++ 의 `__asm` 혹은 `__asm__` 키워드를 C#에서 최대한 비슷하게 구현해보려는 프로젝트입니다.
신택스는 Microsoft Macro Assembler (MASM)의 standard intel syntax 를 사용합니다.
.Net Framework 4.7 버전에서 지원됩니다.

## Usage

```csharp
int a = 200;
int result = (int)X86Assembly.ExecuteScript(
    ASM.MOV, REG.EAX, 100,
    ASM.ADD, REG.EAX, a,
    ASM.RET);
Console.WriteLine(result); // 300

int i = 100;
result = X86Assembly.ExecuteScript(
    ASM.mov, REG.EAX, 0,
    ASM.mov, REG.ECX, i,
    new Label("myloop"),
    ASM.add, REG.EAX, REG.ECX,
    ASM.loop, "myloop",
    ASM.ret));
Console.WriteLine(result); // 5050
```

## Requirements

gcc, objdump 가 설치되어 있고 환경변수에 추가되있어야 합니다.

## [X86Sharp](https://github.com/phillyai/X86Sharp)

X86 VM 입니다. 이 프로젝트에서 개발되다가 .Net Core 2.0 으로 옮겼습니다.

## [LICENSE](/LICENSE)

The MIT License (MIT) Copyright (c) 2017 phillyai
