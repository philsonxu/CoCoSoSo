using System;

namespace CSharp20Tutorial.OOP
{
    /// <summary>
    /// 继承示例
    /// 演示C#中的类继承、base关键字调用父类构造、方法重写
    /// </summary>
    
    // 父类（基类）：动物
    class Animal
    {
        protected string _name;
        protected int _age;
        
        public Animal(string name, int age)
        {
            _name = name;
            _age = age;
            Console.WriteLine("Animal构造函数被调用");
        }
        
        // 虚方法：可以被子类重写
        public virtual void Speak()
        {
            Console.WriteLine("{0}发出声音...", _name);
        }
        
        public void Eat()
        {
            Console.WriteLine("{0}在吃东西", _name);
        }
        
        public void Sleep()
        {
            Console.WriteLine("{0}在睡觉", _name);
        }
        
        public virtual void ShowInfo()
        {
            Console.WriteLine("动物：{0}，年龄：{1}岁", _name, _age);
        }
    }
    
    // 子类（派生类）：狗，继承自Animal
    class Dog : Animal
    {
        private string _breed;  // 品种
        
        // 子类构造函数，使用base调用父类构造函数
        public Dog(string name, int age, string breed) : base(name, age)
        {
            _breed = breed;
            Console.WriteLine("Dog构造函数被调用");
        }
        
        // 重写父类虚方法
        public override void Speak()
        {
            Console.WriteLine("{0}汪汪汪！", _name);
        }
        
        // 子类特有方法
        public void Fetch()
        {
            Console.WriteLine("{0}在接飞盘！", _name);
        }
        
        public override void ShowInfo()
        {
            base.ShowInfo();  // 调用父类方法
            Console.WriteLine("品种：{0}", _breed);
        }
    }
    
    // 子类：猫
    class Cat : Animal
    {
        public Cat(string name, int age) : base(name, age)
        {
            Console.WriteLine("Cat构造函数被调用");
        }
        
        public override void Speak()
        {
            Console.WriteLine("{0}喵喵喵~", _name);
        }
        
        public void Climb()
        {
            Console.WriteLine("{0}在爬树！", _name);
        }
    }
    
    //  sealed关键字：密封类，不能被继承
    sealed class ChineseGardenDog : Dog
    {
        public ChineseGardenDog(string name, int age) : base(name, age, "中华田园犬")
        {
        }
        
        public override void Speak()
        {
            Console.WriteLine("{0}：汪汪汪！我是中华田园犬！", _name);
        }
    }
    
    // 错误！密封类不能被继承
    // class SuperDog : ChineseGardenDog { }
    
    class InheritanceDemo
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===== 继承示例 =====");
            Console.WriteLine("--- 创建Dog对象 ---");
            Dog dog = new Dog("旺财", 3, "金毛");
            dog.ShowInfo();
            dog.Speak();
            dog.Eat();
            dog.Sleep();
            dog.Fetch();
            
            Console.WriteLine("\n--- 创建Cat对象 ---");
            Cat cat = new Cat("咪咪", 2);
            cat.ShowInfo();
            cat.Speak();
            cat.Eat();
            cat.Climb();
            
            Console.WriteLine("\n--- 创建中华田园犬对象 ---");
            ChineseGardenDog tuDog = new ChineseGardenDog("阿黄", 1);
            tuDog.ShowInfo();
            tuDog.Speak();
            
            Console.WriteLine("\n===== 里氏替换原则 =====");
            // 父类引用指向子类对象
            Animal myPet = new Dog("小黑", 2, "拉布拉多");
            myPet.Speak();  // 调用子类重写的方法
            myPet.Eat();
            // myPet.Fetch();  // 错误！父类引用不能调用子类特有方法
            
            if (myPet is Dog)
            {
                Dog dogRef = (Dog)myPet;  // 强制类型转换
                dogRef.Fetch();
            }
            
            // as运算符安全转换
            Animal animal2 = new Cat("小花", 1);
            Cat catRef = animal2 as Cat;
            if (catRef != null)
            {
                catRef.Climb();
            }
            
            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }
    }
}
