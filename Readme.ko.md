# AssemblySharp

- [English](Readme.md)
- [한국어](Readme.ko.md)

C/C++ 의 `__asm` 혹은 `__asm__` 키워드를 C#에서 최대한 비슷하게 구현해보려는 프로젝트입니다.
신택스는 Microsoft Macro Assembler (MASM)의 standard intel syntax 를 사용합니다.

## Usage

```csharp
int a = 200;
int result = (int)X86Assembly.ExecuteScript(
    ASM.MOV, REG.EAX, 100,
    ASM.ADD, REG.EAX, a,
    ASM.RET);
Console.WriteLine(result); // 300
```

## [LICENSE](/LICENSE)

The MIT License (MIT) Copyright (c) 2017 phillyai
