using System;
using System.Text;

namespace CSharp20Tutorial.Chapters
{
    /// <summary>
    /// 第8章：可空类型与异常处理
    /// </summary>
    public class Chapter08
    {
        public static void Run(HtmlConsole console)
        {
            console.WriteTitle("第8章 可空类型与异常处理");

            console.WriteSection("8.1 可空类型（Nullable）");
            console.WriteParagraph("C# 2.0引入了可空类型，允许值类型（int、bool、DateTime等）赋值为null，表示\"没有值\"，常用于数据库字段映射、表单未填写等场景。语法是T? 或者 Nullable<T>。");
            
            console.WriteCode(@"// 声明可空int，可以赋值为null
int? age = null;
age = 25;
// 判断是否有值
if (age.HasValue)
{
    int realAge = age.Value; // 取出实际值
}
// 空合并运算符 ??：如果左边是null就用右边的值
int safeAge = age ?? 0; // age为null时safeAge=0，否则取age的值");

            StringBuilder sb = new StringBuilder();
            int? score = null;
            sb.AppendLine(string.Format("初始score=null，HasValue={0}", score.HasValue));
            
            score = 88;
            sb.AppendLine(string.Format("赋值88后，HasValue={0}，Value={1}", score.HasValue, score.Value));
            
            score = null;
            int displayScore = score ?? -1;
            sb.AppendLine(string.Format("score为null时，用??运算符默认值：{0}", displayScore));
            
            bool? isPass = null;
            sb.AppendLine(string.Format("\nbool?三态：可以是true/false/null（表示未选择）"));
            sb.AppendLine(string.Format("isPass=null → {0}", isPass.HasValue ? isPass.Value.ToString() : "未填写"));
            
            console.WriteOutput(sb.ToString());
            console.WriteTip("可空类型非常适合表示数据库中允许为NULL的字段，比如用户没有填写年龄，年龄字段就是null而不是0。");
            console.WriteDivider();

            console.WriteSection("8.2 异常处理try-catch-finally");
            console.WriteParagraph("程序运行时难免出现错误（除零、空引用、文件不存在、格式错误等），如果不处理程序会直接崩溃。用try-catch可以捕获异常，让程序继续运行；finally块不管是否出错都会执行，常用于释放资源。");
            
            console.WriteCode(@"try
{
    // 可能出错的代码放在try里
    int a = 10;
    int b = 0;
    int c = a / b; // 除以零会抛出异常
}
catch (DivideByZeroException ex)
{
    // 捕获特定类型的异常
    Console.WriteLine(""出错了："" + ex.Message);
}
catch (Exception ex)
{
    // 捕获其他所有异常
    Console.WriteLine(""其他错误："" + ex.Message);
}
finally
{
    // 不管有没有异常都会执行，比如关闭文件、释放资源
    Console.WriteLine(""finally块总是执行"");
}");

            StringBuilder sb2 = new StringBuilder();
            // 测试1：除以零
            sb2.AppendLine("测试1：除以零异常");
            try
            {
                int a = 10;
                int b = 0;
                int c = a / b;
            }
            catch (DivideByZeroException ex)
            {
                sb2.AppendLine(string.Format("  捕获到异常：{0}", ex.Message));
            }
            finally
            {
                sb2.AppendLine("  finally块执行完毕");
            }
            
            // 测试2：格式转换错误
            sb2.AppendLine("\n测试2：字符串转数字格式错误");
            try
            {
                string s = "abc";
                int num = int.Parse(s);
            }
            catch (FormatException ex)
            {
                sb2.AppendLine(string.Format("  捕获到格式异常：{0}", ex.Message));
            }
            
            // 测试3：空引用
            sb2.AppendLine("\n测试3：空引用异常");
            try
            {
                string s = null;
                int len = s.Length; // 调用null对象的方法
            }
            catch (NullReferenceException ex)
            {
                sb2.AppendLine(string.Format("  捕获到空引用异常：{0}", ex.Message));
            }
            
            console.WriteOutput(sb2.ToString());
            console.WriteDivider();

            console.WriteSection("8.3 安全转换与抛出异常");
            console.WriteParagraph("用TryParse方法替代Parse可以避免异常，转换失败直接返回false而不是抛出异常；如果遇到业务逻辑错误，也可以自己用throw关键字抛出异常。");
            
            console.WriteCode(@"// TryParse：安全转换，不抛异常
string input = ""123"";
int result;
bool ok = int.TryParse(input, out result);
if (ok) { /* 转换成功，result是结果 */ }
else { /* 转换失败，不会抛异常 */ }

// 主动抛出异常
if (age < 0)
{
    throw new ArgumentException(""年龄不能为负数"");
}");

            StringBuilder sb3 = new StringBuilder();
            string[] testInputs = { "123", "456abc", "789", "" };
            sb3.AppendLine("TryParse安全转换测试：");
            foreach (string input in testInputs)
            {
                int res;
                bool ok = int.TryParse(input, out res);
                if (ok)
                    sb3.AppendLine(string.Format("  输入\"{0}\" → 转换成功，结果{1}", input, res));
                else
                    sb3.AppendLine(string.Format("  输入\"{0}\" → 转换失败，不抛出异常", input));
            }
            
            sb3.AppendLine("\n主动抛出异常演示：");
            try
            {
                CheckAge(-5);
            }
            catch (ArgumentException ex)
            {
                sb3.AppendLine(string.Format("  捕获到手动抛出的异常：{0}", ex.Message));
            }
            
            console.WriteOutput(sb3.ToString());
            console.WriteNote("异常处理的原则：不要滥用try-catch吞掉所有异常，能提前判断的错误就提前判断，只在真正不可预知的地方用异常处理。");
            console.WriteSuccess("第8章完成！你已经学会了处理程序运行错误，写出更健壮的代码。");
        }

        private static void CheckAge(int age)
        {
            if (age < 0)
            {
                throw new ArgumentException("年龄不能为负数");
            }
        }
    }
}
