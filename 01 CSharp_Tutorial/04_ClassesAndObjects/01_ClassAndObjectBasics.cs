using System;

namespace CSharp20Tutorial.ClassesAndObjects
{
    /// <summary>
    /// 类与对象基础示例
    /// 演示类的定义、字段、属性、构造函数、方法、实例化对象
    /// </summary>
    
    // 定义一个Person类
    class Person
    {
        // 字段（成员变量）
        private string _name;
        private int _age;
        private string _idCard;
        
        // 属性：提供对字段的安全访问
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
                // 可以在属性中加入验证逻辑
                if (value >= 0 && value <= 150)
                {
                    _age = value;
                }
                else
                {
                    Console.WriteLine("年龄设置不合法，使用默认值0");
                    _age = 0;
                }
            }
        }
        
        // 自动实现属性（C# 2.0支持），编译器自动生成私有字段
        public string Gender { get; set; }
        public string Address { get; set; }
        
        // 构造函数：创建对象时调用
        // 默认构造函数（无参数）
        public Person()
        {
            Console.WriteLine("调用了Person的默认构造函数");
            _name = "未知";
            _age = 0;
        }
        
        // 带参数的构造函数（构造函数重载）
        public Person(string name, int age)
        {
            Console.WriteLine("调用了Person的带参构造函数");
            _name = name;
            _age = age;
        }
        
        // 构造函数链
        public Person(string name, int age, string gender) : this(name, age)
        {
            Gender = gender;
        }
        
        // 成员方法
        public void Introduce()
        {
            Console.WriteLine("大家好，我叫{0}，今年{1}岁，性别{2}。", _name, _age, Gender);
        }
        
        public void CelebrateBirthday()
        {
            _age++;
            Console.WriteLine("{0}过生日啦！现在{1}岁了！", _name, _age);
        }
        
        // 析构函数（C#中很少显式定义，由GC自动调用）
        ~Person()
        {
            // Console.WriteLine("Person对象{0}被回收", _name);
        }
    }
    
    class ClassAndObjectBasics
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===== 创建对象 =====");
            // 使用new关键字实例化对象，调用默认构造函数
            Person person1 = new Person();
            person1.Name = "张三";
            person1.Age = 25;
            person1.Gender = "男";
            person1.Introduce();
            
            Console.WriteLine();
            
            // 调用带参构造函数
            Person person2 = new Person("李四", 22);
            person2.Gender = "女";
            person2.Address = "北京市海淀区";
            person2.Introduce();
            Console.WriteLine("住址：{0}", person2.Address);
            
            Console.WriteLine();
            
            // 调用三个参数的构造函数
            Person person3 = new Person("王五", 30, "男");
            person3.Introduce();
            person3.CelebrateBirthday();
            person3.Introduce();
            
            Console.WriteLine("\n===== 属性验证 =====");
            Person person4 = new Person();
            person4.Name = "赵六";
            person4.Age = 200;  // 不合法的年龄，触发验证逻辑
            person4.Introduce();
            
            Console.WriteLine("\n===== 引用类型特性 =====");
            // 类是引用类型，赋值是引用传递
            Person p1 = new Person("小明", 18);
            Person p2 = p1;  // p2和p1指向同一个对象
            p2.Name = "小红";  // 通过p2修改对象
            Console.WriteLine("p1的名字：{0}", p1.Name);  // p1的名字也变了
            Console.WriteLine("p2的名字：{0}", p2.Name);
            
            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }
    }
}
