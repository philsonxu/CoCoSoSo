using System;
using System.Collections.Generic;
using System.Text;
using CSharp20DataStructures.DataStructures;

namespace CSharp20DataStructures.Demos
{
    /// <summary>
    /// 字符串匹配与常用算法演示
    /// </summary>
    public class StringMatchDemo
    {
        public static void Run(HtmlConsole console)
        {
            console.WriteTitle("第9章：字符串匹配（KMP算法）");
            
            console.WriteSection("9.1 字符串匹配问题");
            console.WriteLine("字符串匹配是在主串S中查找模式串P首次出现位置的算法，是文本处理、搜索引擎、编译器的基础。");
            console.WriteLine("朴素匹配：逐个位置比较，最坏时间复杂度O(n*m)；");
            console.WriteLine("KMP算法：利用已匹配信息避免回溯，时间复杂度O(n+m)。");

            string mainStr = "ABABDABACDABABCABAB";
            string pattern = "ABABCABAB";
            console.WriteResult("主串S：" + mainStr + "\n模式串P：" + pattern);

            console.WriteSection("9.2 朴素匹配算法");
            console.WriteCode(
@"public static int NaiveMatch(string s, string p)
{
    int n = s.Length, m = p.Length;
    for (int i = 0; i <= n - m; i++)
    {
        int j;
        for (j = 0; j < m; j++)
        {
            if (s[i + j] != p[j]) break;
        }
        if (j == m) return i; // 匹配成功
    }
    return -1;
}");
            int naiveResult = NaiveMatch(mainStr, pattern);
            console.WriteResult("朴素匹配结果：首次出现在索引 " + naiveResult);

            console.WriteSection("9.3 KMP算法核心思想");
            console.WriteLine("KMP算法的核心是利用部分匹配表（next数组），当匹配失败时，模式串不需要回溯到开头，而是根据next数组移动到合适位置，避免重复比较。");
            console.WriteTip("next数组含义：对于模式串前j个字符，最长相等前缀后缀的长度。");

            console.WriteCode(
@"// 计算next数组
private static int[] BuildNext(string p)
{
    int[] next = new int[p.Length];
    next[0] = 0;
    int len = 0; // 最长前缀后缀长度
    int i = 1;
    while (i < p.Length)
    {
        if (p[i] == p[len])
        {
            len++;
            next[i] = len;
            i++;
        }
        else
        {
            if (len != 0) len = next[len - 1];
            else { next[i] = 0; i++; }
        }
    }
    return next;
}");
            int[] nextArr = BuildNext(pattern);
            StringBuilder nextStr = new StringBuilder();
            nextStr.Append("模式串 " + pattern + " 的next数组：\n[");
            for (int i = 0; i < nextArr.Length; i++)
            {
                nextStr.Append(nextArr[i]);
                if (i < nextArr.Length - 1) nextStr.Append(", ");
            }
            nextStr.Append("]");
            console.WriteResult(nextStr.ToString());

            console.WriteLine("KMP匹配结果：");
            int kmpResult = KMPMatch(mainStr, pattern);
            console.WriteResult("KMP算法匹配位置：索引 " + kmpResult);

            console.WriteSection("9.4 匹配结果验证");
            console.WriteResult("主串从索引" + kmpResult + "开始的子串：" + mainStr.Substring(kmpResult, pattern.Length) + "\n与模式串" + pattern + "完全一致：" + (mainStr.Substring(kmpResult, pattern.Length) == pattern));

            console.WriteSection("9.5 常用字符串算法总结");
            console.WriteLine("✅ KMP算法：单模式串匹配，O(n+m)");
            console.WriteLine("✅ BM算法：实际工程中常用，从后往前比较，效率更高");
            console.WriteLine("✅ Trie字典树：多模式串匹配，前缀搜索");
            console.WriteLine("✅ 正则表达式：复杂模式匹配");

            console.WriteHr();
            console.Render();
        }

        public static int NaiveMatch(string s, string p)
        {
            int n = s.Length, m = p.Length;
            for (int i = 0; i <= n - m; i++)
            {
                int j;
                for (j = 0; j < m; j++)
                {
                    if (s[i + j] != p[j]) break;
                }
                if (j == m) return i;
            }
            return -1;
        }

        private static int[] BuildNext(string p)
        {
            int[] next = new int[p.Length];
            next[0] = 0;
            int len = 0;
            int i = 1;
            while (i < p.Length)
            {
                if (p[i] == p[len])
                {
                    len++;
                    next[i] = len;
                    i++;
                }
                else
                {
                    if (len != 0)
                        len = next[len - 1];
                    else
                    {
                        next[i] = 0;
                        i++;
                    }
                }
            }
            return next;
        }

        public static int KMPMatch(string s, string p)
        {
            int[] next = BuildNext(p);
            int i = 0, j = 0;
            while (i < s.Length)
            {
                if (s[i] == p[j])
                {
                    i++; j++;
                }
                if (j == p.Length)
                    return i - j;
                else if (i < s.Length && s[i] != p[j])
                {
                    if (j != 0) j = next[j - 1];
                    else i++;
                }
            }
            return -1;
        }
    }
}
