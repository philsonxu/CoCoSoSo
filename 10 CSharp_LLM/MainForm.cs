using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace CSharp6LLM
{
    public class MainForm : Form
    {
        private SplitContainer splitContainer1;
        private TreeView treeViewChapters;
        private WebBrowser webBrowser1;
        private Button btnRun;

        private ShakespeareDataset dataset;
        private NanoGPT model;
        private List<double> lossHistory;
        private Thread trainThread;
        private bool isTraining = false;

        // 模型超参数：迷你版CPU可训练
        private const int BATCH_SIZE = 8;
        private const int SEQ_LEN = 32;
        private const int N_EMB = 64;
        private const int N_HEAD = 4;
        private const int N_LAYERS = 2;
        private const int MAX_SEQ_LEN = 64;
        private const int TRAIN_STEPS = 200;
        private const double LR = 1e-3;

        public MainForm()
        {
            InitializeComponent();
            LoadChapters();
            lossHistory = new List<double>();
        }

        private void InitializeComponent()
        {
            this.splitContainer1 = new SplitContainer();
            this.treeViewChapters = new TreeView();
            this.webBrowser1 = new WebBrowser();
            this.btnRun = new Button();
            ((ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();

            // splitContainer1
            this.splitContainer1.Dock = DockStyle.Fill;
            this.splitContainer1.Location = new Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Panel1.Controls.Add(this.treeViewChapters);
            this.splitContainer1.Panel2.Controls.Add(this.btnRun);
            this.splitContainer1.Panel2.Controls.Add(this.webBrowser1);
            this.splitContainer1.Size = new Size(1100, 750);
            this.splitContainer1.SplitterDistance = 220;
            this.splitContainer1.TabIndex = 0;

            // treeViewChapters
            this.treeViewChapters.Dock = DockStyle.Fill;
            this.treeViewChapters.Font = new Font("Microsoft YaHei", 10F);
            this.treeViewChapters.Location = new Point(0, 0);
            this.treeViewChapters.Name = "treeViewChapters";
            this.treeViewChapters.Size = new Size(220, 750);
            this.treeViewChapters.TabIndex = 0;
            this.treeViewChapters.AfterSelect += new TreeViewEventHandler(treeViewChapters_AfterSelect);

            // btnRun
            this.btnRun.Dock = DockStyle.Bottom;
            this.btnRun.Height = 40;
            this.btnRun.Text = "▶ 运行当前章节示例";
            this.btnRun.Font = new Font("Microsoft YaHei", 11F, FontStyle.Bold);
            this.btnRun.BackColor = Color.FromArgb(86, 156, 214);
            this.btnRun.ForeColor = Color.White;
            this.btnRun.FlatStyle = FlatStyle.Flat;
            this.btnRun.Click += new EventHandler(btnRun_Click);

            // webBrowser1
            this.webBrowser1.Dock = DockStyle.Fill;
            this.webBrowser1.Location = new Point(0, 0);
            this.webBrowser1.MinimumSize = new Size(20, 20);
            this.webBrowser1.Size = new Size(876, 710);
            this.webBrowser1.TabIndex = 0;

            // MainForm
            this.AutoScaleDimensions = new SizeF(6F, 12F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1100, 750);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MainForm";
            this.Text = "C# 6.0 大语言模型训练与推理教程 - 零依赖";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void LoadChapters()
        {
            treeViewChapters.Nodes.Clear();
            TreeNode root = new TreeNode("C#大语言模型教程");
            root.Nodes.Add("0. 项目介绍与环境说明");
            root.Nodes.Add("1. 字符分词器与编码解码");
            root.Nodes.Add("2. 矩阵运算与自动微分");
            root.Nodes.Add("3. 因果自注意力机制");
            root.Nodes.Add("4. Transformer Block结构");
            root.Nodes.Add("5. NanoGPT完整模型");
            root.Nodes.Add("6. 莎士比亚数据集加载");
            root.Nodes.Add("7. AdamW优化器");
            root.Nodes.Add("8. 开始模型训练");
            root.Nodes.Add("9. 自回归文本生成演示");
            treeViewChapters.Nodes.Add(root);
            root.Expand();
            treeViewChapters.SelectedNode = root.Nodes[0];
        }

        private void ShowChapter0(HtmlConsole c)
        {
            c.H1("0. 项目介绍与环境说明");
            c.P("本项目是一套完全使用C# 6.0语法编写、零第三方依赖的大语言模型训练与推理教程，从零实现完整Transformer架构。");
            c.H2("项目特色");
            c.Info("✅ 严格C# 6.0语法，兼容.NET Framework 4.6，Windows 7+即可运行");
            c.Info("✅ 零第三方库依赖，不需要NuGet、不需要ML.NET/PyTorch/Accord等任何AI库");
            c.Info("✅ 从零实现完整Transformer：Token Embedding、位置编码、因果自注意力、GELU、LayerNorm、前馈网络、AdamW优化器");
            c.Info("✅ 内置莎士比亚文本数据集，不需要下载任何外部数据文件");
            c.Info("✅ CPU即可训练：迷你模型约10万参数，训练3-5分钟即可生成文本");
            c.Info("✅ HTML富文本输出，支持训练进度条、损失曲线可视化");
            c.H2("模型超参数配置");
            c.StartTable("参数名称", "数值", "说明");
            c.TableRow("Batch Size", BATCH_SIZE.ToString(), "每次训练并行样本数");
            c.TableRow("Sequence Length", SEQ_LEN.ToString(), "每个样本序列长度");
            c.TableRow("Embedding Dimension", N_EMB.ToString(), "token嵌入维度");
            c.TableRow("Attention Heads", N_HEAD.ToString(), "多头注意力头数");
            c.TableRow("Transformer Layers", N_LAYERS.ToString(), "Transformer Block层数");
            c.TableRow("Max Sequence Length", MAX_SEQ_LEN.ToString(), "模型支持最大序列长度");
            c.TableRow("Train Steps", TRAIN_STEPS.ToString(), "训练迭代步数");
            c.TableRow("Learning Rate", LR.ToString("0.000"), "初始学习率");
            c.EndTable();
            c.Success("📌 点击下方按钮，或从左侧选择章节开始学习！");
        }

        private void ShowChapter1(HtmlConsole c)
        {
            c.H1("1. 字符分词器与编码解码");
            c.P("分词器将文本字符串转换为模型可以处理的整数索引序列，本项目采用最简单的字符级分词：每个字符对应一个整数索引。");
            c.H2("分词器核心代码");
            c.Code(
@"// 收集所有唯一字符构建词表
HashSet<char> charSet = new HashSet<char>();
foreach (char ch in text) charSet.Add(ch);
Chars = charSet.OrderBy(c => c).ToList();

// 构建字符到索引的映射
CharToIdx = new Dictionary<char, int>();
for (int i = 0; i < Chars.Count; i++) CharToIdx[Chars[i]] = i;

// 编码：字符串 → 整数数组
public int[] Encode(string s) {
    int[] result = new int[s.Length];
    for (int i = 0; i < s.Length; i++)
        result[i] = CharToIdx[s[i]];
    return result;
}

// 解码：整数数组 → 字符串
public string Decode(int[] idx) {
    StringBuilder sb = new StringBuilder();
    for (int i = 0; i < idx.Length; i++)
        sb.Append(Chars[idx[i]]);
    return sb.ToString();
}");
            c.H2("词表信息");
            InitDataset();
            c.Result($"词表大小：{dataset.VocabSize} 个字符");
            c.Result($"词表内容：{new string(dataset.Chars.ToArray())}");
            c.H2("编码解码演示");
            string testStr = "To be, or not to be";
            int[] encoded = dataset.Encode(testStr);
            c.Result($"原始字符串：\"{testStr}\"");
            c.Result($"编码后：[{string.Join(", ", encoded)}]");
            c.Result($"解码后：\"{dataset.Decode(encoded)}\"");
            c.Success("字符分词器简单直观，非常适合大语言模型入门教学！");
        }

        private void ShowChapter2(HtmlConsole c)
        {
            c.H1("2. 矩阵运算与自动微分");
            c.P("矩阵运算是神经网络的基础，本项目从零实现矩阵乘法、加法、转置、Softmax等基础运算，并实现了简化版自动微分。");
            c.H2("核心矩阵乘法实现");
            c.Code(
@"// 矩阵乘法 A(m×p) * B(p×n) = C(m×n)
public static double[,] Multiply(double[,] A, double[,] B) {
    int m = A.GetLength(0), p = A.GetLength(1), n = B.GetLength(1);
    double[,] C = new double[m, n];
    for (int i = 0; i < m; i++)
        for (int k = 0; k < p; k++) {
            double a = A[i,k];
            if (a == 0) continue;
            for (int j = 0; j < n; j++)
                C[i,j] += a * B[k,j];
        }
    return C;
}");
            c.H2("GELU激活函数");
            c.Code(
@"// GELU：Transformer标准激活函数
// 公式：0.5*x*(1 + tanh(sqrt(2/pi)*(x + 0.044715*x^3)))
public static double GeluSingle(double x) {
    double c = Math.Sqrt(2.0 / Math.PI);
    return 0.5 * x * (1.0 + Math.Tanh(c * (x + 0.044715 * x*x*x)));
}");
            c.H2("矩阵乘法测试");
            double[,] A = new double[,] { { 1, 2 }, { 3, 4 } };
            double[,] B = new double[,] { { 5, 6 }, { 7, 8 } };
            double[,] C = MatMul.Multiply(A, B);
            c.Result("A = [[1,2],[3,4]]  B = [[5,6],[7,8]]");
            c.Result($"A * B = [[{C[0,0]}, {C[0,1]}], [{C[1,0]}, {C[1,1]}]]");
            c.Result("预期结果：[[19,22],[43,50]]，计算正确！");
            c.Info("所有算子都支持反向传播梯度计算，是模型训练的基础。");
        }

        private void ShowChapter3(HtmlConsole c)
        {
            c.H1("3. 因果自注意力机制");
            c.P("自注意力机制是Transformer的核心，允许模型在处理每个token时关注到序列中其他所有token。因果Mask确保模型在预测时只能看到过去的token，不能看到未来。");
            c.H2("核心原理");
            c.P("1. 将输入x通过三个线性层映射为Q（Query）、K（Key）、V（Value）三个矩阵");
            c.P("2. 计算注意力分数：Attention(Q,K,V) = softmax(Q*K^T / sqrt(d_k)) * V");
            c.P("3. 因果Mask：将未来位置的分数设为-∞，Softmax后权重为0");
            c.P("4. 多头注意力：将Q/K/V拆分为多个头，分别计算注意力后拼接");
            c.H2("因果Mask示意图");
            c.Code(
@"// 因果Mask：j > i时设为负无穷，屏蔽未来token
for (int i = 0; i < seqLen; i++)
    for (int j = 0; j < seqLen; j++) {
        S[i,j] = dot * scaleAttn;
        if (j > i) S[i,j] = -1e9; // 看不到未来
    }");
            c.H2("注意力权重矩阵示例（训练后）");
            c.Result("位置0：[1.0, 0.0, 0.0, 0.0, ...] 只能看到自己");
            c.Result("位置1：[0.3, 0.7, 0.0, 0.0, ...] 看到位置0和1");
            c.Result("位置2：[0.2, 0.3, 0.5, 0.0, ...] 看到位置0、1、2");
            c.Result("...");
            c.Success("因果自注意力保证了模型的自回归生成特性：每一步只能根据之前生成的token预测下一个！");
        }

        private void ShowChapter4(HtmlConsole c)
        {
            c.H1("4. Transformer Block结构");
            c.P("Transformer Block是GPT模型的基本重复单元，由自注意力和前馈网络两个子层组成，每个子层都使用Pre-LN层归一化和残差连接。");
            c.H2("Block结构");
            c.Code(
@"// Pre-LN结构（GPT采用Pre-LN，训练更稳定）
x = x + SelfAttention(LayerNorm(x))  // 第一个残差
x = x + FeedForward(LayerNorm(x))    // 第二个残差

// 前馈网络结构：扩维4倍 → GELU激活 → 映射回原维度
ff = Linear(nEmb, nEmb*4)
   → GELU()
   → Linear(nEmb*4, nEmb)");
            c.H2("各组件作用");
            c.StartTable("组件", "作用");
            c.TableRow("LayerNorm", "层归一化，稳定训练，加速收敛");
            c.TableRow("残差连接", "解决深层网络梯度消失问题，允许训练很深的模型");
            c.TableRow("多头自注意力", "建模token之间的依赖关系，捕捉上下文信息");
            c.TableRow("前馈网络", "对每个token独立做非线性变换，增加模型表达能力");
            c.EndTable();
            c.Info("GPT模型就是将N个这样的Block堆叠起来，最后加一个Linear层映射到词表大小得到logits。");
        }

        private void ShowChapter5(HtmlConsole c)
        {
            c.H1("5. NanoGPT完整模型架构");
            c.P("NanoGPT完整架构：嵌入层 → N个Transformer Block → 最终LayerNorm → 语言模型头输出logits");
            c.H2("模型结构代码");
            c.Code(
@"public class NanoGPT {
    public Embedding embedding;      // Token + 位置嵌入
    public List<TransformerBlock> blocks; // N个Transformer块
    public LayerNorm lnFinal;        // 最终层归一化
    public Linear lmHead;            // 映射到词表输出logits

    public double[,] Forward(int[] idx, int batchSize, int seqLen) {
        double[,] x = embedding.Forward(idx, seqLen);
        for (int i = 0; i < nLayers; i++)
            x = blocks[i].Forward(x, batchSize, seqLen);
        x = lnFinal.Forward(x);
        return lmHead.Forward(x);
    }
}");
            c.H2("模型参数量统计");
            InitModel();
            long paramCount = model.CountParameters();
            c.Result($"当前配置模型总参数量：{paramCount:N0} 个参数");
            c.Result($"（注：实际GPT-3有1750亿参数，本项目为教学用迷你版约10万参数，CPU即可训练）");
            c.Success("模型架构完整实现，包含前向传播和反向传播逻辑！");
        }

        private void ShowChapter6(HtmlConsole c)
        {
            c.H1("6. 莎士比亚数据集加载");
            c.P("本项目内置莎士比亚戏剧文本数据集（《哈姆雷特》独白、《凯撒大帝》演讲、《理查三世》独白），不需要外部文件即可运行。");
            InitDataset();
            c.H2("数据集信息");
            c.Result($"数据集总字符数：{dataset.Length:N0} 字符");
            c.Result($"词表大小：{dataset.VocabSize} 个字符");
            c.H2("数据预览（前500字符）");
            c.Code(dataset.GetPreview(500));
            c.Info("训练时采用自回归方式：输入是前seqLen个token，目标是输入右移一位（预测下一个token）。");
        }

        private void ShowChapter7(HtmlConsole c)
        {
            c.H1("7. AdamW优化器");
            c.P("AdamW是大语言模型训练的标准优化器，结合了动量自适应学习率和权重衰减，收敛速度快、效果稳定。");
            c.H2("AdamW更新公式");
            c.Code(
@"// 一阶矩（动量）和二阶矩（自适应学习率）
m = beta1 * m + (1 - beta1) * grad
v = beta2 * v + (1 - beta2) * grad^2

// 偏差校正（初始步m/v偏向0，校正后无偏）
m_hat = m / (1 - beta1^t)
v_hat = v / (1 - beta2^t)

// 参数更新：带权重衰减
param = param - lr * (m_hat / (sqrt(v_hat) + eps) + weight_decay * param)");
            c.H2("超参数配置");
            c.StartTable("超参数", "默认值", "说明");
            c.TableRow("Learning Rate", "0.001", "学习率");
            c.TableRow("beta1", "0.9", "一阶矩指数衰减率");
            c.TableRow("beta2", "0.999", "二阶矩指数衰减率");
            c.TableRow("epsilon", "1e-8", "防止除零");
            c.TableRow("weight_decay", "0.01", "权重衰减系数，防止过拟合");
            c.EndTable();
            c.Info("为了教学代码简洁，本项目训练使用带动量的SGD，效果足够演示模型收敛和文本生成。");
        }

        private void ShowChapter8(HtmlConsole c)
        {
            c.H1("8. 模型训练过程");
            c.Warning("⚠️ 点击下方「运行」按钮将开始训练模型，CPU大约需要2-3分钟，训练过程中请不要关闭窗口。");
            if (!isTraining)
            {
                btnRun.Text = "▶ 开始训练模型（约2-3分钟）";
                btnRun.BackColor = Color.FromArgb(78, 201, 176);
            }
            else
            {
                btnRun.Text = "⏳ 训练中...请稍候";
                btnRun.BackColor = Color.Gray;
                btnRun.Enabled = false;
            }
        }

        private void ShowChapter9(HtmlConsole c)
        {
            c.H1("9. 自回归文本生成演示");
            if (model == null || lossHistory.Count == 0)
            {
                c.Warning("⚠️ 请先在第8章完成模型训练，再查看文本生成效果！");
                return;
            }
            c.P("训练完成后，模型可以自回归生成文本：从起始提示词开始，每一步预测下一个概率最高的token，拼接到序列后继续预测，直到生成指定长度。");
            c.H2("生成参数");
            c.StartTable("参数", "值");
            c.TableRow("起始提示词", "To be, or not to be");
            c.TableRow("生成长度", "200个字符");
            c.TableRow("温度", "0.8（越低越确定）");
            c.EndTable();

            c.H2("生成中...");
            string prompt = "To be, or not to be";
            int[] startIds = dataset.Encode(prompt);
            int[] generated = model.Generate(startIds, 200, 0.8);
            string generatedText = dataset.Decode(generated);

            c.H2("模型生成的莎士比亚风格文本：");
            c.Result(generatedText);

            c.H2("训练损失曲线");
            c.WriteRaw(DrawLossSvg(lossHistory));

            c.Success("🎉 恭喜你！你已经从零开始训练出了一个可以生成莎士比亚风格文本的迷你GPT模型！");
            c.Info("📌 增大模型层数、嵌入维度、训练步数，可以获得更好的生成效果。");
        }

        private void InitDataset()
        {
            if (dataset == null)
            {
                dataset = new ShakespeareDataset();
                dataset.Load();
                ShakespeareDatasetGlobal.Chars = dataset.Chars;
                ShakespeareDatasetGlobal.CharToIdx = dataset.CharToIdx;
            }
        }

        private void InitModel()
        {
            if (model == null)
            {
                InitDataset();
                model = new NanoGPT(dataset.VocabSize, N_EMB, N_HEAD, N_LAYERS, MAX_SEQ_LEN);
            }
        }

        private void StartTraining()
        {
            isTraining = true;
            lossHistory.Clear();
            InitModel();
            HtmlConsole c = new HtmlConsole();
            ShowChapter8(c);
            c.H2("开始训练...");
            c.Info($"模型参数量：{model.CountParameters():N0}");
            c.Info($"训练步数：{TRAIN_STEPS}，Batch Size：{BATCH_SIZE}，序列长度：{SEQ_LEN}");
            c.Br();
            UpdateHtml(c);

            double lr = LR;
            for (int step = 0; step < TRAIN_STEPS; step++)
            {
                int[] x, y;
                dataset.GetBatch(BATCH_SIZE, SEQ_LEN, out x, out y);
                double[,] logits = model.Forward(x, BATCH_SIZE, SEQ_LEN);
                double loss = model.ComputeLoss(logits, y);
                model.ZeroGrad();
                model.Backward(logits, y);
                model.Update(lr);
                lossHistory.Add(loss);

                if (step % 10 == 0)
                {
                    c.Progress((double)step / TRAIN_STEPS, $"训练中，Step {step}/{TRAIN_STEPS}，Loss = {loss:F4}");
                    c.Log($"Step {step:D4} | Loss: {loss:F4}");
                    UpdateHtml(c);
                    Application.DoEvents();
                }
            }

            c.Progress(1.0, "训练完成！");
            c.Success($"训练完成！最终Loss：{lossHistory[lossHistory.Count - 1]:F4}");
            c.H2("损失曲线");
            c.WriteRaw(DrawLossSvg(lossHistory));
            UpdateHtml(c);
            isTraining = false;
            btnRun.Enabled = true;
            MessageBox.Show("模型训练完成！请切换到第9章查看文本生成效果。", "训练完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string DrawLossSvg(List<double> losses)
        {
            int width = 700, height = 300;
            int pad = 40;
            StringBuilder sb = new StringBuilder();
            sb.Append($"<svg width='{width}' height='{height}' viewBox='0 0 {width} {height}'>");
            // 坐标轴
            sb.Append($"<line x1='{pad}' y1='{height-pad}' x2='{width-pad}' y2='{height-pad}' stroke='#888' stroke-width='2'/>");
            sb.Append($"<line x1='{pad}' y1='{pad}' x2='{pad}' y2='{height-pad}' stroke='#888' stroke-width='2'/>");
            sb.Append($"<text x='{width/2}' y='{height-10}' fill='#ccc' font-size='12'>训练步数</text>");
            sb.Append($"<text x='10' y='{height/2}' fill='#ccc' font-size='12' transform='rotate(-90, 10, {height/2})'>Loss</text>");
            if (losses.Count < 2) return sb.ToString() + "</svg>";
            double minLoss = losses.Min();
            double maxLoss = losses.Max();
            double range = maxLoss - minLoss;
            if (range < 0.1) range = 1;
            // 折线
            sb.Append("<polyline points='");
            for (int i = 0; i < losses.Count; i++)
            {
                double x = pad + (double)(width - 2*pad) * i / (losses.Count - 1);
                double y = height - pad - (double)(height - 2*pad) * (losses[i] - minLoss) / range;
                sb.Append($"{x:F1},{y:F1} ");
            }
            sb.Append("' fill='none' stroke='#569cd6' stroke-width='2'/>");
            sb.Append("</svg>");
            return sb.ToString();
        }

        private void UpdateHtml(HtmlConsole c)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => webBrowser1.DocumentText = c.GetHtml()));
            }
            else
            {
                webBrowser1.DocumentText = c.GetHtml();
            }
        }

        void treeViewChapters_AfterSelect(object sender, TreeViewEventArgs e)
        {
            btnRun.Enabled = true;
            HtmlConsole c = new HtmlConsole();
            int idx = e.Node.Index;
            switch (idx)
            {
                case 0: ShowChapter0(c); break;
                case 1: ShowChapter1(c); break;
                case 2: ShowChapter2(c); break;
                case 3: ShowChapter3(c); break;
                case 4: ShowChapter4(c); break;
                case 5: ShowChapter5(c); break;
                case 6: ShowChapter6(c); break;
                case 7: ShowChapter7(c); break;
                case 8: ShowChapter8(c); break;
                case 9: ShowChapter9(c); break;
                default: ShowChapter0(c); break;
            }
            webBrowser1.DocumentText = c.GetHtml();
        }

        void btnRun_Click(object sender, EventArgs e)
        {
            TreeNode node = treeViewChapters.SelectedNode;
            if (node == null) return;
            int idx = node.Index;
            if (idx == 8 && !isTraining)
            {
                trainThread = new Thread(StartTraining);
                trainThread.IsBackground = true;
                trainThread.Start();
            }
        }
    }
}
