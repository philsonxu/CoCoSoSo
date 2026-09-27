using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CSharp6LLM
{
    /// <summary>
    /// 莎士比亚文本数据集类
    /// 内置莎士比亚戏剧文本，不需要外部数据文件即可运行
    /// 负责数据加载、分词、编码、batch生成
    /// </summary>
    public class ShakespeareDataset
    {
        /// <summary>
        /// 字符表：所有出现过的字符
        /// </summary>
        public List<char> Chars { get; private set; }
        /// <summary>
        /// 字符到索引的映射字典
        /// </summary>
        public Dictionary<char, int> CharToIdx { get; private set; }
        /// <summary>
        /// 词汇表大小
        /// </summary>
        public int VocabSize { get { return Chars.Count; } }
        /// <summary>
        /// 编码后的完整数据序列
        /// </summary>
        public int[] Data { get; private set; }
        /// <summary>
        /// 数据集总长度
        /// </summary>
        public int Length { get { return Data.Length; } }

        private Random random = new Random(123);

        /// <summary>
        /// 加载数据集，初始化词表
        /// </summary>
        public void Load()
        {
            string text = GetShakespeareText();

            // 收集所有唯一字符
            HashSet<char> charSet = new HashSet<char>();
            foreach (char c in text)
                charSet.Add(c);
            Chars = charSet.OrderBy(c => c).ToList();

            // 构建字符到索引的映射
            CharToIdx = new Dictionary<char, int>();
            for (int i = 0; i < Chars.Count; i++)
                CharToIdx[Chars[i]] = i;

            // 编码文本为整数序列
            Data = new int[text.Length];
            for (int i = 0; i < text.Length; i++)
                Data[i] = CharToIdx[text[i]];
        }

        /// <summary>
        /// 编码：字符串 → 整数索引数组
        /// </summary>
        public int[] Encode(string s)
        {
            int[] result = new int[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                if (CharToIdx.ContainsKey(s[i]))
                    result[i] = CharToIdx[s[i]];
                else
                    result[i] = CharToIdx[' ']; // 未知字符替换为空格
            }
            return result;
        }

        /// <summary>
        /// 解码：整数索引数组 → 字符串
        /// </summary>
        public string Decode(int[] idx)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < idx.Length; i++)
            {
                if (idx[i] >= 0 && idx[i] < Chars.Count)
                    sb.Append(Chars[idx[i]]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 随机获取一个训练batch
        /// </summary>
        /// <param name="batchSize">batch大小</param>
        /// <param name="seqLen">序列长度</param>
        /// <returns>x: 输入token [batchSize * seqLen]；y: 目标token，x右移一位</returns>
        public void GetBatch(int batchSize, int seqLen, out int[] x, out int[] y)
        {
            x = new int[batchSize * seqLen];
            y = new int[batchSize * seqLen];
            int maxStart = Data.Length - seqLen - 1;

            for (int b = 0; b < batchSize; b++)
            {
                int start = random.Next(maxStart);
                for (int t = 0; t < seqLen; t++)
                {
                    x[b * seqLen + t] = Data[start + t];
                    y[b * seqLen + t] = Data[start + t + 1]; // 预测下一个token
                }
            }
        }

        /// <summary>
        /// 获取数据预览文本
        /// </summary>
        public string GetPreview(int length = 500)
        {
            string text = GetShakespeareText();
            if (text.Length <= length) return text;
            return text.Substring(0, length) + "...";
        }

        /// <summary>
        /// 内置莎士比亚戏剧文本（哈姆雷特独白+凯撒大帝演讲）
        /// 无需外部文件即可运行
        /// </summary>
        private string GetShakespeareText()
        {
            return @"
To be, or not to be, that is the question:
Whether 'tis nobler in the mind to suffer
The slings and arrows of outrageous fortune,
Or to take arms against a sea of troubles
And by opposing end them. To die—to sleep,
No more; and by a sleep to say we end
The heart-ache and the thousand natural shocks
That flesh is heir to: 'tis a consummation
Devoutly to be wish'd. To die, to sleep;
To sleep, perchance to dream—ay, there's the rub:
For in that sleep of death what dreams may come,
When we have shuffled off this mortal coil,
Must give us pause—there's the respect
That makes calamity of so long life.
For who would bear the whips and scorns of time,
Th'oppressor's wrong, the proud man's contumely,
The pangs of despis'd love, the law's delay,
The insolence of office, and the spurns
That patient merit of th'unworthy takes,
When he himself might his quietus make
With a bare bodkin? Who would these fardels bear,
To grunt and sweat under a weary life,
But that the dread of something after death,
The undiscover'd country, from whose bourn
No traveller returns, puzzles the will,
And makes us rather bear those ills we have
Than fly to others that we know not of?
Thus conscience does make cowards of us all,
And thus the native hue of resolution
Is sicklied o'er with the pale cast of thought,
And enterprises of great pith and moment
With this regard their currents turn awry
And lose the name of action.

Friends, Romans, countrymen, lend me your ears;
I come to bury Caesar, not to praise him.
The evil that men do lives after them;
The good is oft interred with their bones;
So let it be with Caesar. The noble Brutus
Hath told you Caesar was ambitious:
If it were so, it was a grievous fault,
And grievously hath Caesar answer'd it.
Here, under leave of Brutus and the rest—
For Brutus is an honourable man;
So are they all, all honourable men—
Come I to speak in Caesar's funeral.
He was my friend, faithful and just to me:
But Brutus says he was ambitious;
And Brutus is an honourable man.
He hath brought many captives home to Rome
Whose ransoms did the general coffers fill:
Did this in Caesar seem ambitious?
When that the poor have cried, Caesar hath wept:
Ambition should be made of sterner stuff:
Yet Brutus says he was ambitious;
And Brutus is an honourable man.
You all did see that on the Lupercal
I thrice presented him a kingly crown,
Which he did thrice refuse: was this ambition?
Yet Brutus says he was ambitious;
And, sure, he is an honourable man.
I speak not to disprove what Brutus spoke,
But here I am to speak what I do know.
You all did love him once, not without cause:
What cause withholds you then, to mourn for him?
O judgment! thou art fled to brutish beasts,
And men have lost their reason. Bear with me;
My heart is in the coffin there with Caesar,
And I must pause till it come back to me.

Now is the winter of our discontent
Made glorious summer by this sun of York;
And all the clouds that lour'd upon our house
In the deep bosom of the ocean buried.
Now are our brows bound with victorious wreaths;
Our bruised arms hung up for monuments;
Our stern alarums changed to merry meetings,
Our dreadful marches to delightful measures.
Grim-visaged war hath smooth'd his wrinkled front;
And now, instead of mounting barbed steeds
To fright the souls of fearful adversaries,
He capers nimbly in a lady's chamber
To the lascivious pleasing of a lute.
But I, that am not shaped for sportive tricks,
Nor made to court an amorous looking-glass;
I, that am rudely stamp'd, and want love's majesty
To strut before a wanton ambling nymph;
I, that am curtail'd of this fair proportion,
Cheated of feature by dissembling nature,
Deformed, unfinish'd, sent before my time
Into this breathing world, scarce half made up,
And that so lamely and unfashionable
That dogs bark at me as I halt by them;
Why, I, in this weak piping time of peace,
Have no delight to pass away the time,
Unless to spy my shadow in the sun
And descant on mine own deformity:
And therefore, since I cannot prove a lover,
To entertain these fair well-spoken days,
I am determined to prove a villain
And hate the idle pleasures of these days.
";
        }
    }
}
