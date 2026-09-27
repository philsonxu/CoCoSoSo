using System;
using System.Collections.Generic;

namespace CSharpDataStructures.String
{
    /// <summary>
    /// 字符串算法集合
    /// 包含KMP模式匹配算法
    /// </summary>
    public static class StringAlgorithms
    {
        /// <summary>
        /// 朴素模式匹配（暴力匹配）
        /// 时间复杂度O(n*m)
        /// </summary>
        /// <returns>匹配成功返回起始索引，否则返回-1</returns>
        public static int BrutalMatch(string text, string pattern)
        {
            int n = text.Length;
            int m = pattern.Length;

            for (int i = 0; i <= n - m; i++)
            {
                int j;
                for (j = 0; j < m; j++)
                {
                    if (text[i + j] != pattern[j])
                        break;
                }
                if (j == m)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// KMP模式匹配算法
        /// 利用部分匹配表避免回溯
        /// 时间复杂度O(n+m)
        /// </summary>
        public static int KMPMatch(string text, string pattern)
        {
            int n = text.Length;
            int m = pattern.Length;
            if (m == 0) return 0;

            int[] next = BuildNextArray(pattern);
            int j = 0; // pattern指针

            for (int i = 0; i < n; i++)
            {
                while (j > 0 && text[i] != pattern[j])
                    j = next[j - 1];

                if (text[i] == pattern[j])
                    j++;

                if (j == m)
                    return i - m + 1; // 匹配成功
            }
            return -1;
        }

        /// <summary>
        /// 构建next数组（部分匹配表）
        /// next[i]表示pattern[0..i]的最长相等前后缀长度
        /// </summary>
        private static int[] BuildNextArray(string pattern)
        {
            int m = pattern.Length;
            int[] next = new int[m];
            next[0] = 0;
            int j = 0;

            for (int i = 1; i < m; i++)
            {
                while (j > 0 && pattern[i] != pattern[j])
                    j = next[j - 1];

                if (pattern[i] == pattern[j])
                    j++;

                next[i] = j;
            }
            return next;
        }

        /// <summary>
        /// 查找所有匹配位置
        /// </summary>
        public static List<int> KMPMatchAll(string text, string pattern)
        {
            List<int> result = new List<int>();
            int n = text.Length;
            int m = pattern.Length;
            if (m == 0) return result;

            int[] next = BuildNextArray(pattern);
            int j = 0;

            for (int i = 0; i < n; i++)
            {
                while (j > 0 && text[i] != pattern[j])
                    j = next[j - 1];

                if (text[i] == pattern[j])
                    j++;

                if (j == m)
                {
                    result.Add(i - m + 1);
                    j = next[j - 1]; // 继续匹配下一个
                }
            }
            return result;
        }
    }
}