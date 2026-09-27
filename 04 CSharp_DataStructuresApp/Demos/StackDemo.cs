using System;
using System.Collections.Generic;
using System.Text;
using CSharp20DataStructures.DataStructures;

namespace CSharp20DataStructures.Demos
{
    /// <summary>
    /// 栈演示
    /// </summary>
    public class StackDemo
    {
        public static void Run(HtmlConsole console)
        {
            console.WriteTitle("第3章：栈（Stack）");
            
            console.WriteSection("3.1 基本概念");
            console.WriteLine("栈是一种后进先出（LIFO, Last In First Out）的线性表，只允许在一端（栈顶）进行插入和删除操作。");
            console.WriteLine("栈是一种特殊的线性表，常用于表达式求值、括号匹配、函数调用栈、回溯算法等场景。");
            console.WriteTip("核心操作：Push（入栈）、Pop（出栈）、Peek（查看栈顶），时间复杂度均为O(1)");

            console.WriteSection("3.2 核心操作演示");
            MyStack<int> stack = new MyStack<int>();

            // 入栈
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("依次将 1,2,3,4,5 入栈：");
            stack.Push(1); stack.Push(2); stack.Push(3); stack.Push(4); stack.Push(5);
            sb.AppendLine("栈内元素（栈底→栈顶）：" + ArrayToString(stack.ToArray()));
            sb.AppendLine("栈顶元素：" + stack.Peek() + "，元素个数：" + stack.Count);
            console.WriteResult(sb.ToString());

            // 出栈
            console.WriteLine("执行两次出栈操作：");
            int p1 = stack.Pop();
            int p2 = stack.Pop();
            console.WriteResult("第一次出栈：" + p1 + "，第二次出栈：" + p2 + "\n剩余元素：" + ArrayToString(stack.ToArray()));

            console.WriteSection("3.3 经典应用：括号匹配");
            console.WriteCode(
@"public static bool IsValidParentheses(string s)
{
    MyStack<char> stack = new MyStack<char>();
    foreach (char c in s)
    {
        if (c == '(' || c == '[' || c == '{')
            stack.Push(c);
        else
        {
            if (stack.IsEmpty) return false;
            char top = stack.Pop();
            if (c == ')' && top != '(') return false;
            if (c == ']' && top != '[') return false;
            if (c == '}' && top != '{') return false;
        }
    }
    return stack.IsEmpty;
}");
            console.WriteLine("括号匹配测试：");
            string test1 = "()[]{}";
            string test2 = "([)]";
            string test3 = "{[()]}";
            console.WriteResult(
                test1 + " : " + IsValidParentheses(test1) + "\n" +
                test2 + " : " + IsValidParentheses(test2) + "\n" +
                test3 + " : " + IsValidParentheses(test3)
            );

            console.WriteSection("3.4 其他常见应用场景");
            console.WriteLine("✅ 浏览器前进后退功能");
            console.WriteLine("✅ 编辑器撤销（Undo）操作");
            console.WriteLine("✅ 递归调用的函数栈");
            console.WriteLine("✅ 表达式求值与后缀表达式转换");
            console.WriteLine("✅ 深度优先搜索（DFS）");

            console.WriteHr();
            console.Render();
        }

        /// <summary>
        /// 括号匹配算法
        /// </summary>
        public static bool IsValidParentheses(string s)
        {
            MyStack<char> stack = new MyStack<char>();
            foreach (char c in s)
            {
                if (c == '(' || c == '[' || c == '{')
                {
                    stack.Push(c);
                }
                else
                {
                    if (stack.IsEmpty) return false;
                    char top = stack.Pop();
                    if (c == ')' && top != '(') return false;
                    if (c == ']' && top != '[') return false;
                    if (c == '}' && top != '{') return false;
                }
            }
            return stack.IsEmpty;
        }

        private static string ArrayToString<T>(T[] arr)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[底] ");
            for (int i = 0; i < arr.Length; i++)
            {
                sb.Append(arr[i]);
                if (i < arr.Length - 1) sb.Append(", ");
            }
            sb.Append(" [顶]");
            return sb.ToString();
        }
    }
}
