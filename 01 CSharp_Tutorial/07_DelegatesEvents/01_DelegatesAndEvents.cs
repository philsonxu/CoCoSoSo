using System;

namespace CSharp20Tutorial.DelegatesEvents
{
    /// <summary>
    /// 委托与事件示例
    /// 演示委托定义、匿名方法（C# 2.0）、多播委托、事件的定义与使用
    /// </summary>
    
    // 1. 定义委托：可以指向签名匹配的方法
    // 委托定义：返回值为void，参数为string
    public delegate void GreetingDelegate(string name);
    
    // 定义计算委托
    public delegate int MathOperation(int a, int b);
    
    // 事件发布者类
    class Button
    {
        // 定义事件：基于EventHandler委托
        public event EventHandler Click;
        
        private string _text;
        
        public Button(string text)
        {
            _text = text;
        }
        
        // 触发Click事件的方法
        public void OnClick()
        {
            Console.WriteLine("按钮[{0}]被点击了！", _text);
            // 如果有订阅者，触发事件
            if (Click != null)
            {
                Click(this, EventArgs.Empty);
            }
        }
    }
    
    // 温度监控类：演示自定义事件参数
    class TemperatureMonitor
    {
        // 自定义事件参数类
        public class TemperatureEventArgs : EventArgs
        {
            public double CurrentTemperature { get; set; }
            public DateTime AlertTime { get; set; }
        }
        
        // 自定义委托
        public delegate void TemperatureAlertHandler(object sender, TemperatureEventArgs e);
        
        // 定义温度过高事件
        public event TemperatureAlertHandler TemperatureTooHigh;
        
        private double _threshold;
        
        public TemperatureMonitor(double threshold)
        {
            _threshold = threshold;
        }
        
        // 检查温度
        public void CheckTemperature(double temperature)
        {
            Console.WriteLine("当前温度：{0}°C", temperature);
            
            if (temperature > _threshold && TemperatureTooHigh != null)
            {
                // 触发报警事件
                TemperatureTooHigh(this, new TemperatureEventArgs
                {
                    CurrentTemperature = temperature,
                    AlertTime = DateTime.Now
                });
            }
        }
    }
    
    class DelegatesAndEvents
    {
        // 普通方法，用于绑定到委托
        static void SayHello(string name)
        {
            Console.WriteLine("Hello, {0}!", name);
        }
        
        static void SayGoodbye(string name)
        {
            Console.WriteLine("Goodbye, {0}!", name);
        }
        
        static void SayChineseGreeting(string name)
        {
            Console.WriteLine("你好，{0}！", name);
        }
        
        // 计算方法
        static int Add(int a, int b) { return a + b; }
        static int Subtract(int a, int b) { return a - b; }
        static int Multiply(int a, int b) { return a * b; }
        
        // 按钮点击事件处理方法
        static void Button_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Console.WriteLine("收到按钮点击事件：用户点击了按钮");
        }
        
        // 温度报警事件处理方法
        static void Monitor_TemperatureTooHigh(object sender, TemperatureMonitor.TemperatureEventArgs e)
        {
            Console.WriteLine("⚠️ 高温警报！当前温度{0}°C，时间：{1}", 
                e.CurrentTemperature, e.AlertTime.ToLongTimeString());
        }
        
        static void Main(string[] args)
        {
            Console.WriteLine("===== 委托基本使用 =====");
            // 实例化委托，绑定方法
            GreetingDelegate greeting = new GreetingDelegate(SayHello);
            // 调用委托（相当于调用绑定的方法）
            greeting("张三");
            
            // 委托可以绑定多个方法（多播委托）
            greeting += SayChineseGreeting;
            greeting += SayGoodbye;
            Console.WriteLine("\n多播委托调用（依次执行所有绑定的方法）：");
            greeting("李四");
            
            // 移除方法绑定
            greeting -= SayGoodbye;
            Console.WriteLine("\n移除SayGoodbye后：");
            greeting("王五");
            
            Console.WriteLine("\n===== C# 2.0 匿名方法 =====");
            // 匿名方法：不需要单独定义方法，直接写委托实现
            GreetingDelegate anonymousGreeting = delegate(string name)
            {
                Console.WriteLine("匿名方法问候：{0}，欢迎学习C#！", name);
            };
            anonymousGreeting("赵六");
            
            // 匿名方法用于计算委托
            MathOperation calc = delegate(int a, int b)
            {
                return a * a + b * b;
            };
            Console.WriteLine("匿名方法计算 3² + 4² = {0}", calc(3, 4));
            
            Console.WriteLine("\n委托动态绑定不同方法：");
            MathOperation mathOp = new MathOperation(Add);
            Console.WriteLine("10 + 5 = {0}", mathOp(10, 5));
            
            mathOp = new MathOperation(Subtract);
            Console.WriteLine("10 - 5 = {0}", mathOp(10, 5));
            
            mathOp = new MathOperation(Multiply);
            Console.WriteLine("10 * 5 = {0}", mathOp(10, 5));
            
            Console.WriteLine("\n===== 事件示例：按钮点击 =====");
            Button okButton = new Button("确定");
            // 订阅事件（将事件处理方法绑定到事件）
            okButton.Click += new EventHandler(Button_Click);
            
            // 模拟按钮被点击
            okButton.OnClick();
            
            Console.WriteLine("\n===== 自定义事件：温度监控 =====");
            TemperatureMonitor monitor = new TemperatureMonitor(40);  // 阈值40度
            // 订阅高温报警事件
            monitor.TemperatureTooHigh += new TemperatureMonitor.TemperatureAlertHandler(Monitor_TemperatureTooHigh);
            
            // 模拟温度检测
            Random rand = new Random();
            for (int i = 0; i < 5; i++)
            {
                double temp = rand.Next(35, 45);
                monitor.CheckTemperature(temp);
                System.Threading.Thread.Sleep(500);
            }
            
            Console.WriteLine("\n===== 委托与事件总结 =====");
            Console.WriteLine("1. 委托是类型安全的函数指针，可以指向一个或多个方法");
            Console.WriteLine("2. 多播委托可以绑定多个方法，调用时按顺序执行");
            Console.WriteLine("3. C# 2.0引入匿名方法，简化委托使用");
            Console.WriteLine("4. 事件基于委托实现，是发布-订阅模式的体现");
            Console.WriteLine("5. 事件用于对象间通信，当状态变化时通知其他对象");
            
            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }
    }
}
