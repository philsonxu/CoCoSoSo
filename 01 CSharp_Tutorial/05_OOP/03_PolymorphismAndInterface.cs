using System;

namespace CSharp20Tutorial.OOP
{
    /// <summary>
    /// 多态与接口示例
    /// 演示运行时多态（虚方法、重写）、接口定义与实现
    /// </summary>
    
    // 定义接口：可绘制的
    interface IDrawable
    {
        void Draw();  // 接口方法，默认public
        string GetShapeName();
    }
    
    // 定义接口：可计算面积
    interface IShape
    {
        double CalculateArea();
        double CalculatePerimeter();
    }
    
    // 形状基类
    abstract class Shape
    {
        public abstract string Name { get; }
        
        // 抽象方法，必须由子类实现
        public abstract void Display();
    }
    
    // 圆形类，继承Shape，实现两个接口
    class Circle : Shape, IDrawable, IShape
    {
        private double _radius;
        
        public Circle(double radius)
        {
            _radius = radius;
        }
        
        public override string Name
        {
            get { return "圆形"; }
        }
        
        public double Radius
        {
            get { return _radius; }
        }
        
        // 实现抽象方法
        public override void Display()
        {
            Console.WriteLine("这是一个{0}，半径为{1}", Name, _radius);
        }
        
        // 实现IDrawable接口方法
        public void Draw()
        {
            Console.WriteLine("正在绘制圆形 ○");
        }
        
        public string GetShapeName()
        {
            return Name;
        }
        
        // 实现IShape接口方法
        public double CalculateArea()
        {
            return Math.PI * _radius * _radius;
        }
        
        public double CalculatePerimeter()
        {
            return 2 * Math.PI * _radius;
        }
    }
    
    // 矩形类
    class Rectangle : Shape, IDrawable, IShape
    {
        private double _width;
        private double _height;
        
        public Rectangle(double width, double height)
        {
            _width = width;
            _height = height;
        }
        
        public override string Name
        {
            get { return "矩形"; }
        }
        
        public override void Display()
        {
            Console.WriteLine("这是一个{0}，宽{1}，高{2}", Name, _width, _height);
        }
        
        public void Draw()
        {
            Console.WriteLine("正在绘制矩形 □");
        }
        
        public string GetShapeName()
        {
            return Name;
        }
        
        public double CalculateArea()
        {
            return _width * _height;
        }
        
        public double CalculatePerimeter()
        {
            return 2 * (_width + _height);
        }
    }
    
    // 三角形类
    class Triangle : Shape, IDrawable, IShape
    {
        private double _a, _b, _c;  // 三边长度
        
        public Triangle(double a, double b, double c)
        {
            _a = a;
            _b = b;
            _c = c;
        }
        
        public override string Name
        {
            get { return "三角形"; }
        }
        
        public override void Display()
        {
            Console.WriteLine("这是一个{0}，三边长{1}, {2}, {3}", Name, _a, _b, _c);
        }
        
        public void Draw()
        {
            Console.WriteLine("正在绘制三角形 △");
        }
        
        public string GetShapeName()
        {
            return Name;
        }
        
        public double CalculateArea()
        {
            // 海伦公式
            double p = (_a + _b + _c) / 2;
            return Math.Sqrt(p * (p - _a) * (p - _b) * (p - _c));
        }
        
        public double CalculatePerimeter()
        {
            return _a + _b + _c;
        }
    }
    
    class PolymorphismAndInterface
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===== 多态示例 =====");
            // 使用父类数组存储不同子类对象
            Shape[] shapes = new Shape[3];
            shapes[0] = new Circle(5);
            shapes[1] = new Rectangle(4, 6);
            shapes[2] = new Triangle(3, 4, 5);
            
            Console.WriteLine("--- 多态调用Display ---");
            foreach (Shape shape in shapes)
            {
                shape.Display();  // 根据实际对象类型调用对应方法
            }
            
            Console.WriteLine("\n===== 接口示例 =====");
            // 使用接口数组存储实现了IDrawable的对象
            IDrawable[] drawables = new IDrawable[3];
            drawables[0] = new Circle(3);
            drawables[1] = new Rectangle(2, 5);
            drawables[2] = new Triangle(6, 8, 10);
            
            Console.WriteLine("--- 绘制所有形状 ---");
            foreach (IDrawable drawable in drawables)
            {
                Console.WriteLine("形状：{0}", drawable.GetShapeName());
                drawable.Draw();
            }
            
            Console.WriteLine("\n--- 计算所有形状面积和周长 ---");
            IShape[] shapeCalculators = new IShape[3];
            shapeCalculators[0] = new Circle(5);
            shapeCalculators[1] = new Rectangle(4, 6);
            shapeCalculators[2] = new Triangle(3, 4, 5);
            
            for (int i = 0; i < shapeCalculators.Length; i++)
            {
                Console.WriteLine("形状{0}：", i + 1);
                Console.WriteLine("  面积：{0:F2}", shapeCalculators[i].CalculateArea());
                Console.WriteLine("  周长：{0:F2}", shapeCalculators[i].CalculatePerimeter());
            }
            
            Console.WriteLine("\n===== 多态的好处 =====");
            Console.WriteLine("1. 代码复用：子类复用父类的成员");
            Console.WriteLine("2. 灵活性：可以统一处理不同子类对象");
            Console.WriteLine("3. 扩展性：新增子类不影响原有代码");
            Console.WriteLine("4. 解耦：接口隔离实现与调用");
            
            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }
    }
}
