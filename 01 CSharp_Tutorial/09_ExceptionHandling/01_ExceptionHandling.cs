using System;
using System.IO;

namespace CSharp20Tutorial.ExceptionHandling
{
    /// <summary>
    /// 异常处理示例
    /// 演示try-catch-finally、异常类型、自定义异常、异常抛出
    /// </summary>
    
    // 自定义异常类
    class AgeException : ApplicationException
    {
        public AgeException(string message) : base(message)
        {
        }
        
        public AgeException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
    
    // 用户信息类，用于验证年龄
    class UserValidator
    {
        public static void ValidateAge(int age)
        {
            if (age < 0)
            {
                throw new AgeException("年龄不能为负数！");
            }
            if (age > 150)
            {
                throw new AgeException("年龄不能超过150岁，输入不合法！");
            }
        }
        
        public static int Divide(int a, int b)
        {
            return a / b;  // 可能抛出除零异常
        }
        
        public static void CheckArrayIndex(int[] array, int index)
        {
            // 直接访问可能抛出索引越界异常
            Console.WriteLine("数组索引{0}的值：{1}", index, array[index]);
        }
    }
    
    class ExceptionHandlingDemo
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===== 基本异常处理：try-catch =====");
            // 除零异常
            try
            {
                Console.WriteLine("尝试执行除法...");
                int result = UserValidator.Divide(10, 0);
                Console.WriteLine("结果：{0}", result);  // 不会执行到这里
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine("捕获到除零异常：{0}", ex.Message);
            }
            
            Console.WriteLine("\n===== 多个catch块 =====");
            // 捕获不同类型的异常
            try
            {
                Console.Write("请输入一个整数：");
                string input = Console.ReadLine();
                int num = int.Parse(input);
                
                int[] arr = { 1, 2, 3 };
                Console.WriteLine("尝试访问索引{0}...", num);
                UserValidator.CheckArrayIndex(arr, num);
                
                int result = 100 / num;
                Console.WriteLine("100 / {0} = {1}", num, result);
            }
            catch (FormatException ex)
            {
                Console.WriteLine("格式异常：输入不是有效的整数 - {0}", ex.Message);
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine("索引越界异常：索引超出数组范围 - {0}", ex.Message);
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine("除零异常：不能除以零 - {0}", ex.Message);
            }
            catch (Exception ex)  // 捕获其他所有异常（父类异常放在最后）
            {
                Console.WriteLine("捕获到其他异常：{0} - {1}", ex.GetType().Name, ex.Message);
            }
            
            Console.WriteLine("\n===== finally块 =====");
            // finally块无论是否发生异常都会执行，通常用于释放资源
            StreamReader reader = null;
            try
            {
                Console.WriteLine("尝试读取文件...");
                // 故意读取一个可能不存在的文件
                reader = new StreamReader("test.txt");
                string content = reader.ReadToEnd();
                Console.WriteLine("文件内容：{0}", content);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("文件未找到异常：{0}", ex.Message);
            }
            catch (IOException ex)
            {
                Console.WriteLine("IO异常：{0}", ex.Message);
            }
            finally
            {
                Console.WriteLine("执行finally块：释放资源");
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    Console.WriteLine("文件已关闭");
                }
            }
            
            Console.WriteLine("\n===== 抛出自定义异常 =====");
            try
            {
                Console.Write("请输入您的年龄：");
                int age = int.Parse(Console.ReadLine());
                UserValidator.ValidateAge(age);
                Console.WriteLine("年龄验证通过，您{0}岁", age);
            }
            catch (AgeException ex)
            {
                Console.WriteLine("年龄验证失败：{0}", ex.Message);
            }
            catch (FormatException)
            {
                Console.WriteLine("输入格式错误，请输入数字！");
            }
            
            Console.WriteLine("\n===== 异常嵌套与内部异常 =====");
            try
            {
                try
                {
                    // 内层发生异常
                    int.Parse("abc");
                }
                catch (FormatException ex)
                {
                    // 包装为新的异常抛出，保留内部异常信息
                    throw new InvalidOperationException("处理输入时发生错误", ex);
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("外层捕获异常：{0}", ex.Message);
                Console.WriteLine("内部异常：{0}", ex.InnerException.Message);
            }
            
            Console.WriteLine("\n===== using语句：自动释放资源 =====");
            // using语句会在代码块结束后自动调用Dispose方法释放资源
            // 相当于try-finally的简化写法
            try
            {
                using (StreamWriter writer = new StreamWriter("output.txt"))
                {
                    writer.WriteLine("这是使用using语句写入的内容");
                    writer.WriteLine("using语句会自动释放文件资源");
                    Console.WriteLine("文件写入成功");
                    // 离开using块时，writer会自动被Dispose
                }
                
                // 验证文件已正确关闭，可以重新打开
                if (File.Exists("output.txt"))
                {
                    string content = File.ReadAllText("output.txt");
                    Console.WriteLine("写入内容：\n{0}", content);
                    File.Delete("output.txt");  // 删除测试文件
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("文件操作异常：{0}", ex.Message);
            }
            
            Console.WriteLine("\n===== 异常处理最佳实践 =====");
            Console.WriteLine("1. 只捕获能处理的异常，不要捕获所有异常");
            Console.WriteLine("2. catch块从具体异常到通用异常排列");
            Console.WriteLine("3. 使用finally块确保资源释放");
            Console.WriteLine("4. 优先使用using语句管理IDisposable资源");
            Console.WriteLine("5. 抛出有意义的异常信息，包含必要上下文");
            Console.WriteLine("6. 自定义异常应继承自ApplicationException");
            
            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }
    }
}
