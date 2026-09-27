using System;
using System.Text;

namespace CSharp20Tutorial.Chapters
{
    /// <summary>
    /// 第6章：面向对象基础
    /// </summary>
    public class Chapter06
    {
        public static void Run(HtmlConsole console)
        {
            console.WriteTitle("第6章 面向对象编程基础");

            console.WriteSection("6.1 类与对象");
            console.WriteParagraph("类是对象的模板，定义了对象的属性（数据）和方法（行为）；对象是类的实例。C#是纯面向对象语言，所有代码都写在类中。");
            console.WriteNote("注意：C# 2.0不支持自动属性（自动属性是C#3.0引入的），必须手动写字段和属性访问器。");
            
            console.WriteCode(@"// 定义一个Person类
public class Person
{
    // 字段：存储数据（通常private，不对外暴露）
    private string _name;
    private int _age;

    // 属性：提供对外访问接口，可控制读写
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public int Age
    {
        get { return _age; }
        set 
        { 
            // 属性中可以加验证逻辑
            if (value >= 0 && value <= 120)
                _age = value; 
        }
    }

    // 方法：对象的行为
    public void SayHello()
    {
        Console.WriteLine(string.Format(""大家好，我叫{0}，今年{1}岁。"", _name, _age));
    }
}

// 创建对象（实例化类）
Person p = new Person();
p.Name = ""张三"";
p.Age = 25;
p.SayHello();");

            StringBuilder sb = new StringBuilder();
            Person p1 = new Person();
            p1.Name = "张三";
            p1.Age = 25;
            sb.AppendLine("创建Person对象，设置Name=张三，Age=25，调用SayHello()：");
            p1.SayHello(sb);
            
            Person p2 = new Person();
            p2.Name = "李四";
            p2.Age = 30;
            sb.AppendLine("\n创建第二个Person对象：");
            p2.SayHello(sb);
            
            sb.AppendLine("\n尝试设置Age为-5（非法值，验证不通过，保持默认0）：");
            p2.Age = -5;
            p2.SayHello(sb);
            
            console.WriteOutput(sb.ToString());
            console.WriteDivider();

            console.WriteSection("6.2 构造函数");
            console.WriteParagraph("构造函数是创建对象时自动调用的方法，方法名和类名相同，无返回值，用于初始化对象。可以重载多个构造函数。");
            
            console.WriteCode(@"public class Student
{
    private string _name;
    private int _id;

    // 默认构造函数（无参数）
    public Student()
    {
        _name = ""未命名"";
        _id = 0;
    }

    // 带参数的构造函数（重载）
    public Student(string name, int id)
    {
        _name = name;
        _id = id;
    }
}");

            StringBuilder sb2 = new StringBuilder();
            Student s1 = new Student();
            sb2.AppendLine("用无参构造函数创建Student：");
            s1.ShowInfo(sb2);
            
            Student s2 = new Student("王五", 2023001);
            sb2.AppendLine("\n用带参构造函数创建Student：");
            s2.ShowInfo(sb2);
            
            console.WriteOutput(sb2.ToString());
            console.WriteTip("如果自己写了带参数的构造函数，编译器就不会再自动生成默认无参构造函数了。如果还需要无参构造，必须自己写。");
            console.WriteDivider();

            console.WriteSection("6.3 静态成员");
            console.WriteParagraph("用static修饰的成员是静态成员，属于类本身，不属于某个具体对象，直接用类名访问，不需要创建对象。静态成员在整个程序中只有一份。");
            
            console.WriteCode(@"public class Counter
{
    // 静态字段：记录总共创建了多少个对象
    private static int _count = 0;

    public Counter()
    {
        _count++; // 每创建一个对象，计数加1
    }

    // 静态方法
    public static int GetTotalCount()
    {
        return _count;
    }
}");

            StringBuilder sb3 = new StringBuilder();
            sb3.AppendLine(string.Format("初始创建对象数量：{0}", Counter.GetTotalCount()));
            Counter c1 = new Counter();
            sb3.AppendLine(string.Format("创建1个对象后：{0}", Counter.GetTotalCount()));
            Counter c2 = new Counter();
            Counter c3 = new Counter();
            sb3.AppendLine(string.Format("创建3个对象后：{0}", Counter.GetTotalCount()));
            sb3.AppendLine("注意：静态方法只能访问静态成员，不能访问实例成员。");
            
            console.WriteOutput(sb3.ToString());
            console.WriteDivider();

            console.WriteSuccess("第6章完成！你已经入门了C#面向对象编程，接下来会学习C#2.0最重要的新特性——泛型。");
        }
    }

    // --- 教学用辅助类，严格C#2.0语法，无自动属性 ---
    public class Person
    {
        private string _name;
        private int _age;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int Age
        {
            get { return _age; }
            set
            {
                if (value >= 0 && value <= 120)
                    _age = value;
            }
        }

        public void SayHello(StringBuilder sb)
        {
            sb.AppendLine(string.Format("  大家好，我叫{0}，今年{1}岁。", _name, _age));
        }
    }

    public class Student
    {
        private string _name;
        private int _id;

        public Student()
        {
            _name = "未命名";
            _id = 0;
        }

        public Student(string name, int id)
        {
            _name = name;
            _id = id;
        }

        public void ShowInfo(StringBuilder sb)
        {
            sb.AppendLine(string.Format("  学号：{0}，姓名：{1}", _id, _name));
        }
    }

    public class Counter
    {
        private static int _count = 0;

        public Counter()
        {
            _count++;
        }

        public static int GetTotalCount()
        {
            return _count;
        }
    }
}
