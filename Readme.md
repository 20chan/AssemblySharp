# AssemblySharp

C/C++ �� `__asm` Ȥ�� `__asm__` Ű���带 C#���� �ִ��� ����ϰ� �����غ����� ������Ʈ�Դϴ�.

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
