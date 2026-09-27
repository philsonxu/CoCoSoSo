using System;
using System.Collections.Generic;
using System.Drawing;

namespace CSharp20AI
{
    class NaiveBayesDemo : DemoBase
    {
        public override void Run(HtmlConsole c)
        {
            c.H1(_title);
            c.H2("算法原理");
            c.P("朴素贝叶斯是基于贝叶斯定理和特征条件独立假设的分类算法，是经典的概率生成模型。所谓「朴素」就是假设所有特征之间相互独立，这个假设大大简化了计算，虽然在现实中往往不成立，但实际效果非常好。");
            c.Code("贝叶斯定理：\nP(类别|特征) = P(特征|类别) * P(类别) / P(特征)\n预测时计算样本属于每个类别的概率，取概率最大的类别作为结果。");
            c.P("拉普拉斯平滑：为了避免某个特征组合在训练集中没出现过导致概率为0，分子加1、分母加特征取值数，做平滑处理。");
            c.Tip("朴素贝叶斯是垃圾邮件过滤的经典算法，至今仍然广泛使用。");

            c.H2("运行演示：垃圾邮件过滤");
            c.P("我们用词袋模型+朴素贝叶斯实现简单的垃圾邮件分类器：统计垃圾邮件和正常邮件中各单词出现的概率，对于新邮件计算是垃圾邮件的概率。");

            // 训练数据：0=正常邮件，1=垃圾邮件
            string[][] emails = new string[][]
            {
                new string[]{"您好", "优惠", "特价", "中奖", "点击", "链接"}, // 垃圾
                new string[]{"免费", "中奖", "优惠", "点击", "领取", "奖金"}, // 垃圾
                new string[]{"特价", "优惠", "折扣", "点击", "购买", "促销"}, // 垃圾
                new string[]{"中奖", "恭喜", "点击", "链接", "领取", "奖金"}, // 垃圾
                new string[]{"会议", "明天", "项目", "讨论", "进度", "报告"}, // 正常
                new string[]{"您好", "明天", "开会", "项目", "资料", "发给"}, // 正常
                new string[]{"报告", "已经", "完成", "请查收", "项目", "进度"}, // 正常
                new string[]{"您好", "明天", "下午", "会议", "请", "参加"} // 正常
            };
            int[] labels = new int[]{1,1,1,1,0,0,0,0};

            int n = emails.Length;
            // 统计
            Dictionary<string, int>[] wordCount = new Dictionary<string,int>[2];
            wordCount[0] = new Dictionary<string, int>();
            wordCount[1] = new Dictionary<string, int>();
            int[] totalWords = new int[2];
            int[] classCount = new int[2];
            // 统计所有去重词数，用于拉普拉斯平滑
            HashSet<string> allWords = new HashSet<string>();
            for (int i = 0; i < n; i++)
            {
                int cls = labels[i];
                classCount[cls]++;
                foreach (string w in emails[i])
                {
                    allWords.Add(w);
                    totalWords[cls]++;
                    if (!wordCount[cls].ContainsKey(w)) wordCount[cls][w] = 0;
                    wordCount[cls][w]++;
                }
            }
            int vocabSize = allWords.Count;
            double pSpam = (double)classCount[1] / n;
            double pHam = (double)classCount[0] / n;
            c.Result(string.Format("先验概率：垃圾邮件P(垃圾)={0:P0}, 正常邮件P(正常)={1:P0}", pSpam, pHam));
            c.Result(string.Format("词表大小：{0}个词，使用拉普拉斯平滑避免0概率", vocabSize));

            c.H3("单词条件概率（部分）");
            string[] headers = new string[]{"单词", "P(词|垃圾)", "P(词|正常)"};
            string[] showWords = new string[]{"中奖", "优惠", "点击", "项目", "会议", "明天"};
            string[,] rows = new string[showWords.Length, 3];
            for (int i = 0; i < showWords.Length; i++)
            {
                string w = showWords[i];
                double pWS = (wordCount[1].ContainsKey(w) ? wordCount[1][w] : 0) + 1;
                pWS = pWS / (totalWords[1] + vocabSize);
                double pWH = (wordCount[0].ContainsKey(w) ? wordCount[0][w] : 0) + 1;
                pWH = pWH / (totalWords[0] + vocabSize);
                rows[i,0] = w;
                rows[i,1] = pWS.ToString("P2");
                rows[i,2] = pWH.ToString("P2");
            }
            c.Table(headers, rows);

            c.H3("新邮件预测测试");
            string[][] testMails = new string[][]
            {
                new string[]{"恭喜", "中奖", "点击", "链接", "领取", "奖金"},
                new string[]{"明天", "项目", "会议", "讨论", "进度"},
                new string[]{"免费", "优惠", "特价", "点击", "购买"}
            };
            foreach (string[] mail in testMails)
            {
                double logPS = Math.Log(pSpam);
                double logPH = Math.Log(pHam);
                foreach (string w in mail)
                {
                    double pWS = (wordCount[1].ContainsKey(w) ? wordCount[1][w] : 0) + 1;
                    pWS = pWS / (totalWords[1] + vocabSize);
                    logPS += Math.Log(pWS);
                    double pWH = (wordCount[0].ContainsKey(w) ? wordCount[0][w] : 0) + 1;
                    pWH = pWH / (totalWords[0] + vocabSize);
                    logPH += Math.Log(pWH);
                }
                // 用对数概率避免下溢，比较大小即可
                int pred = logPS > logPH ? 1 : 0;
                double prob = 1/(1+Math.Exp(logPH - logPS)); // sigmoid转换为近似概率
                c.Result(string.Format("邮件内容【{0}】→ 预测为{1}，垃圾邮件概率约{2:P0}",
                    string.Join(" ", mail), pred == 1 ? "垃圾邮件 ❌" : "正常邮件 ✅", prob));
            }
            c.Success("可以看到：包含「中奖、点击、领取」关键词的邮件被正确判断为垃圾邮件，包含「项目、会议、进度」的邮件被正确判断为正常邮件。");

            c.H2("总结");
            c.P("朴素贝叶斯优点：原理简单、训练预测速度极快、对小样本友好、高维数据表现好；缺点是特征独立假设在实际中往往不成立，但即使不成立很多时候效果仍然很好。");
            c.P("常见变体：多项式朴素贝叶斯（文本分类）、伯努利朴素贝叶斯（布尔特征）、高斯朴素贝叶斯（连续特征）。");
        }
    }
}
