# C# 程序设计入门源程序库

这是一套完整的 C# 2.0 入门级示例代码库，覆盖 C# 程序设计入门阶段的绝大部分核心知识点，适合初学者系统学习 C# 语法和面向对象编程基础。

## 📚 内容概览

本代码库按知识点分为 10 个章节，共 20+ 个可运行示例，每个示例都包含详细注释和运行演示。

### 目录结构

```
CSharp20Tutorial/
├── 01_BasicSyntax/              # 第1章：基础语法
│   ├── 01_HelloWorld.cs         # 第一个C#程序，Hello World
│   ├── 02_VariablesAndConstants.cs  # 变量与常量
│   ├── 03_Operators.cs          # 各类运算符
│   └── 04_ConsoleIO.cs          # 控制台输入输出与格式化
├── 02_DataTypes/                # 第2章：数据类型
│   ├── 01_ValueTypes.cs         # 值类型（整型、浮点、布尔、字符、结构体、可空类型）
│   ├── 02_ReferenceTypes.cs     # 引用类型（字符串、object、类型转换）
│   └── 03_EnumAndArray.cs       # 枚举、一维/二维/交错数组
├── 03_ControlFlow/              # 第3章：流程控制
│   ├── 01_ConditionalStatements.cs  # if-else、switch-case条件语句
│   ├── 02_LoopStatements.cs     # for、while、do-while、foreach循环，break/continue
│   └── 03_Methods.cs            # 方法定义、参数传递（值/ref/out/params）、重载、递归
├── 04_ClassesAndObjects/        # 第4章：类与对象
│   ├── 01_ClassAndObjectBasics.cs   # 类的定义、字段、属性、构造函数、实例化
│   └── 02_StaticAndAccessModifiers.cs  # 静态成员、访问修饰符
├── 05_OOP/                      # 第5章：面向对象编程
│   ├── 01_Encapsulation.cs      # 封装（银行账户案例）
│   ├── 02_Inheritance.cs        # 继承、base关键字、方法重写、密封类
│   └── 03_PolymorphismAndInterface.cs  # 多态、抽象类、接口
├── 06_Collections/              # 第6章：集合
│   └── 01_NonGenericCollections.cs  # ArrayList、Hashtable、Queue、Stack、SortedList
├── 07_DelegatesEvents/          # 第7章：委托与事件
│   └── 01_DelegatesAndEvents.cs # 委托、多播委托、匿名方法(C#2.0)、事件
├── 08_Generics/                 # 第8章：泛型（C#2.0核心特性）
│   └── 01_GenericsBasics.cs     # 泛型类、泛型方法、List<T>、Dictionary<K,V>
├── 09_ExceptionHandling/        # 第9章：异常处理
│   └── 01_ExceptionHandling.cs  # try-catch-finally、自定义异常、using语句
└── 10_FileIO/                   # 第10章：文件与IO
    └── 01_FileIO.cs             # 文本/二进制文件读写、目录操作、文件监控
```

## ✨ 核心知识点覆盖

### C# 语法基础
- ✅ .NET Framework 与 C# 基本概念
- ✅ 变量与常量声明、数据类型系统
- ✅ 算术、赋值、比较、逻辑、三元运算符
- ✅ 控制台输入输出与字符串格式化
- ✅ 值类型与引用类型、装箱拆箱
- ✅ 隐式/显式类型转换、Parse/Convert/TryParse
- ✅ 枚举类型、一维数组、多维数组、交错数组

### 流程控制
- ✅ if-else 分支判断
- ✅ switch-case 多分支
- ✅ for、while、do-while 循环
- ✅ foreach 遍历
- ✅ break、continue 跳转语句
- ✅ 方法定义与调用、return 返回值
- ✅ 值参数、ref 引用参数、out 输出参数、params 可变参数
- ✅ 方法重载
- ✅ 递归算法（阶乘、斐波那契）

### 类与面向对象
- ✅ 类与对象的概念、new 实例化
- ✅ 字段、属性、自动属性
- ✅ 构造函数、构造函数重载、构造函数链
- ✅ 静态字段、静态方法、静态构造函数、静态类
- ✅ public/private/protected/internal 访问修饰符
- ✅ 封装：隐藏实现细节、提供公共接口
- ✅ 继承：base 关键字、虚方法重写、密封类
- ✅ 多态：里氏替换原则、虚方法调用
- ✅ 抽象类、抽象方法
- ✅ 接口定义与多接口实现

### C# 2.0 新特性
- ✅ 泛型（Generic）：类型安全的代码复用
  - 泛型类、泛型方法
  - 泛型集合 List<T>、Dictionary<K,V>
  - 泛型约束
- ✅ 可空类型（Nullable）：值类型可赋 null
- ✅ 匿名方法（Anonymous Method）：简化委托编写
- ✅ 迭代器（基础概念）
- ✅ 分部类型（partial）

### 高级主题
- ✅ 委托（Delegate）：类型安全的函数指针
- ✅ 多播委托
- ✅ 事件（Event）：发布-订阅模式
- ✅ 非泛型集合类
- ✅ 异常处理机制：try-catch-finally
- ✅ 自定义异常
- ✅ using 语句资源管理
- ✅ 文件与目录操作
- ✅ Stream 流读写
- ✅ 文本文件与二进制文件
- ✅ FileSystemWatcher 文件监控

## 🚀 如何使用

### 环境要求
- .NET Framework 2.0 或更高版本
- Visual Studio 2005 及以上版本，或 VS Code + .NET SDK
- Windows 操作系统（.NET Framework 2.0 为 Windows 原生支持）

### 使用 Visual Studio 运行
1. 打开 Visual Studio
2. 选择 `文件 -> 打开 -> 项目/解决方案`
3. 选择 `CSharp20Tutorial.csproj` 文件打开
4. 在解决方案资源管理器中，右键点击想要运行的 cs 文件
5. 选择 `设为启动项`
6. 按 F5 或 Ctrl+F5 编译运行

### 命令行编译（csc编译器）
每个示例都可以独立编译运行，使用 .NET Framework 自带的 csc 编译器：

```cmd
# 编译单个文件
csc /out:HelloWorld.exe 01_BasicSyntax\01_HelloWorld.cs

# 运行
HelloWorld.exe
```

### 注意事项
- 每个示例的 `Main` 函数都是独立的入口点，一次只能运行一个示例
- 修改启动项即可切换运行不同的示例
- 所有代码均严格遵循 C# 2.0 语法规范，不使用 C# 3.0 及以上版本特性（如 LINQ、lambda、var 等）

## 📝 学习建议

1. **按顺序学习**：建议从第1章基础语法开始，按章节顺序学习，循序渐进
2. **动手编写**：不要只看代码，亲手输入每一个示例，运行查看结果
3. **修改调试**：尝试修改代码中的参数和逻辑，观察运行结果变化，加深理解
4. **完成习题**：每个知识点掌握后，尝试自己编写小练习巩固
5. **阅读注释**：每个示例代码中都有详细注释，解释语法要点和注意事项

## 📖 适用人群
- C# 零基础初学者
- 从其他语言转学 C# 的开发者
- 准备学习 .NET 开发的入门者
- 复习 C# 基础知识的开发者
- 高校程序设计课程的学生

## 🔧 技术规范
- 目标框架：.NET Framework 2.0
- 语言版本：C# 2.0
- 编码标准：遵循微软 C# 编码规范
- 注释语言：中文
- 所有示例均为控制台应用程序，无需额外依赖

---

祝你学习愉快！
