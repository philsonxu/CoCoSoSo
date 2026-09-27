# Lib 目录

本目录用于放置 OpenCvSharp 程序集引用文件。

## 推荐方式（NuGet）

推荐通过 NuGet 包管理器安装 OpenCvSharp4，无需手动拷贝 DLL：

```
Install-Package OpenCvSharp4
Install-Package OpenCvSharp4.runtime.win
```

## 手动引用方式

若使用旧版 OpenCvSharp（2.4.10 等），请将下列 DLL 文件放入本目录：

- `OpenCvSharp.dll` — 托管核心库
- `OpenCvSharpExtern.dll`（或对应版本的原生 DLL）— 原生 OpenCV 绑定

具体版本请根据目标 .NET Framework 版本选择：

| .NET 版本 | 推荐 OpenCvSharp 版本 |
|-----------|----------------------|
| .NET 2.0  | OpenCvSharp 2.4.10   |
| .NET 4.0+ | OpenCvSharp 4.x（OpenCvSharp4）|

> 注意：本示例库的代码使用 **C# 2.0 语法**（显式类型、无 var/lambda/LINQ/自动属性），但可以在更高版本的 .NET Framework（如 4.x）上编译运行。若在 VS2010+/VS2019+ 上编译，需要在项目属性中将「语言版本」设置为 C# 2.0/ISO-2，或将项目升级到更高 .NET Framework 版本后保持源码 C# 2.0 风格即可。
