using System;
using System.Collections.Generic;

namespace CSharp20AI
{
    class DecisionTreeDemo : DemoBase
    {
        // 简化版决策树节点，二分类，离散特征
        class TreeNode
        {
            public bool isLeaf;
            public int label; // 叶子节点类别
            public int featureIdx; // 划分特征索引
            public Dictionary<string, TreeNode> children; // 特征值 -> 子节点
            public TreeNode()
            {
                children = new Dictionary<string, TreeNode>();
                isLeaf = false;
            }
        }

        public override void Run(HtmlConsole c)
        {
            c.H1(_title);
            c.H2("算法原理");
            c.P("决策树是基于树结构的分类模型，模拟人类做决策的过程：从根节点开始，根据特征取值一步步判断，最终走到叶子节点得到分类结果。");
            c.P("ID3算法是最经典的决策树算法，核心是用信息增益选择最优划分特征：每次选择信息增益最大的特征划分，使得划分后数据的不确定性（熵）下降最多。");
            c.Code("信息熵：Ent(D) = -Σp_k * log2(p_k)，熵越大表示数据越混乱\n信息增益：Gain(D, a) = Ent(D) - Σ(|Dv|/|D|)*Ent(Dv)，增益越大表示用特征a划分后纯度提升越多");

            c.H2("经典案例：打网球决策");
            c.P("我们使用经典的天气数据集，根据天气、温度、湿度、是否有风4个特征，判断是否适合打网球。");

            // 数据集：每个样本 [天气, 温度, 湿度, 有风, 是否打球]
            // 天气：晴/阴/雨；温度：热/温/凉；湿度：高/正常；有风：是/否；结果：是/否
            string[][] data = new string[][]
            {
                new string[]{"晴", "热", "高", "否", "否"},
                new string[]{"晴", "热", "高", "是", "否"},
                new string[]{"阴", "热", "高", "否", "是"},
                new string[]{"雨", "温", "高", "否", "是"},
                new string[]{"雨", "凉", "正常", "否", "是"},
                new string[]{"雨", "凉", "正常", "是", "否"},
                new string[]{"阴", "凉", "正常", "是", "是"},
                new string[]{"晴", "温", "高", "否", "否"},
                new string[]{"晴", "凉", "正常", "否", "是"},
                new string[]{"雨", "温", "正常", "否", "是"},
                new string[]{"晴", "温", "正常", "是", "是"},
                new string[]{"阴", "温", "高", "是", "是"},
                new string[]{"阴", "热", "正常", "否", "是"},
                new string[]{"雨", "温", "高", "是", "否"}
            };
            string[] featureNames = new string[] { "天气", "温度", "湿度", "有风" };

            c.H3("训练数据集（共14条）");
            string[] headers = new string[] { "天气", "温度", "湿度", "有风", "是否打球" };
            string[,] rows = new string[data.Length, 5];
            for (int i = 0; i < data.Length; i++)
                for (int j = 0; j < 5; j++)
                    rows[i,j] = data[i][j];
            c.Table(headers, rows);

            // 递归构建ID3决策树
            List<int> remainFeatures = new List<int>(new int[]{0,1,2,3});
            TreeNode root = BuildTree(data, remainFeatures);

            c.Result("决策树构建完成！");
            PrintTree(c, root, featureNames, 0);

            c.H3("决策树预测测试");
            string[][] testSamples = new string[][]
            {
                new string[]{"晴", "温", "正常", "否"},
                new string[]{"晴", "热", "高", "是"},
                new string[]{"雨", "温", "高", "是"},
                new string[]{"阴", "凉", "正常", "否"}
            };
            foreach (string[] sample in testSamples)
            {
                int pred = Predict(root, sample);
                c.Result(string.Format("天气={0}, 温度={1}, 湿度={2}, 有风={3} → 预测：{4}",
                    sample[0], sample[1], sample[2], sample[3], pred == 1 ? "适合打球 ✅" : "不适合打球 ❌"));
            }
            c.Success("预测结果和我们的直觉完全一致：阴天肯定适合打球，晴天高湿度有风不适合。");
            c.H2("总结");
            c.P("决策树优点：模型可解释性极强，生成的规则人类可以直接理解；不需要特征归一化；可以处理离散和连续特征；训练速度快。");
            c.P("ID3算法缺点：倾向于选择取值多的特征；容易过拟合；只能处理离散特征；C4.5/CART是ID3的改进版本，解决了这些问题。");
        }

        private double CalcEntropy(string[][] data)
        {
            int n = data.Length;
            Dictionary<string, int> cnt = new Dictionary<string, int>();
            for (int i = 0; i < n; i++)
            {
                string label = data[i][4];
                if (!cnt.ContainsKey(label)) cnt[label] = 0;
                cnt[label]++;
            }
            double ent = 0;
            foreach (KeyValuePair<string,int> kv in cnt)
            {
                double p = (double)kv.Value / n;
                ent -= p * Math.Log(p, 2);
            }
            return ent;
        }

        private TreeNode BuildTree(string[][] data, List<int> features)
        {
            TreeNode node = new TreeNode();
            // 情况1：所有样本标签相同，直接返回叶子
            bool allSame = true;
            string firstLabel = data[0][4];
            for (int i = 1; i < data.Length; i++)
                if (data[i][4] != firstLabel) { allSame = false; break; }
            if (allSame)
            {
                node.isLeaf = true;
                node.label = firstLabel == "是" ? 1 : 0;
                return node;
            }
            // 情况2：没有特征可分，返回多数类
            if (features.Count == 0)
            {
                node.isLeaf = true;
                int pos = 0;
                for (int i = 0; i < data.Length; i++) if (data[i][4] == "是") pos++;
                node.label = pos >= data.Length - pos ? 1 : 0;
                return node;
            }
            // 选择信息增益最大的特征
            double baseEnt = CalcEntropy(data);
            double bestGain = -1;
            int bestFeat = 0;
            foreach (int f in features)
            {
                // 按特征值分组
                Dictionary<string, List<string[]>> groups = new Dictionary<string, List<string[]>>();
                for (int i = 0; i < data.Length; i++)
                {
                    string v = data[i][f];
                    if (!groups.ContainsKey(v)) groups[v] = new List<string[]>();
                    groups[v].Add(data[i]);
                }
                double newEnt = 0;
                foreach (KeyValuePair<string, List<string[]>> kv in groups)
                {
                    string[][] subData = kv.Value.ToArray();
                    newEnt += (double)subData.Length / data.Length * CalcEntropy(subData);
                }
                double gain = baseEnt - newEnt;
                if (gain > bestGain) { bestGain = gain; bestFeat = f; }
            }
            node.featureIdx = bestFeat;
            // 递归构建子节点
            Dictionary<string, List<string[]>> bestGroups = new Dictionary<string, List<string[]>>();
            for (int i = 0; i < data.Length; i++)
            {
                string v = data[i][bestFeat];
                if (!bestGroups.ContainsKey(v)) bestGroups[v] = new List<string[]>();
                bestGroups[v].Add(data[i]);
            }
            List<int> newFeatures = new List<int>();
            foreach (int f in features) if (f != bestFeat) newFeatures.Add(f);
            foreach (KeyValuePair<string, List<string[]>> kv in bestGroups)
            {
                node.children[kv.Key] = BuildTree(kv.Value.ToArray(), newFeatures);
            }
            return node;
        }

        private int Predict(TreeNode node, string[] sample)
        {
            if (node.isLeaf) return node.label;
            string v = sample[node.featureIdx];
            if (node.children.ContainsKey(v))
                return Predict(node.children[v], sample);
            return 0;
        }

        private void PrintTree(HtmlConsole c, TreeNode node, string[] names, int depth)
        {
            string indent = "";
            for (int i = 0; i < depth; i++) indent += "　　";
            if (node.isLeaf)
            {
                c.Result(indent + "└─ 结果：" + (node.label == 1 ? "打球 ✅" : "不打球 ❌"));
                return;
            }
            c.Result(indent + "├─ 判断特征：" + names[node.featureIdx]);
            foreach (KeyValuePair<string, TreeNode> kv in node.children)
            {
                c.Result(indent + "　├─ 如果" + names[node.featureIdx] + " = " + kv.Key + "：");
                PrintTree(c, kv.Value, names, depth + 2);
            }
        }
    }
}
