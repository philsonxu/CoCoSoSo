using System;
using System.Text;

namespace CSharp6LLM
{
    /// <summary>
    /// 字符级分词器：每个字符对应一个token，最简单的分词方式，适合教学
    /// </summary>
    public class CharTokenizer
    {
        public string Chars;
        public int[] Stoi; // char to index
        public char[] Itos; // index to char
        public int VocabSize;

        public CharTokenizer(string text)
        {
            bool[] exists = new bool[256];
            var sb = new StringBuilder();
            foreach (char c in text)
                if (!exists[c]) { exists[c] = true; sb.Append(c); }
            Chars = sb.ToString();
            VocabSize = Chars.Length;
            Stoi = new int[256];
            Itos = new char[VocabSize];
            for (int i = 0; i < Chars.Length; i++)
            {
                Stoi[Chars[i]] = i;
                Itos[i] = Chars[i];
            }
        }

        public int[] Encode(string s)
        {
            int[] res = new int[s.Length];
            for (int i = 0; i < s.Length; i++) res[i] = Stoi[s[i]];
            return res;
        }

        public string Decode(int[] ids)
        {
            var sb = new StringBuilder();
            foreach (int id in ids) sb.Append(Itos[id]);
            return sb.ToString();
        }
    }

    /// <summary>
    /// 莎士比亚数据集（内置，无需外部文件），用于训练小模型
    /// 数据量约4KB，CPU上几分钟即可训练出可读文本
    /// </summary>
    public class ShakespeareDataset
    {
        public const string Text = @"
To be, or not to be: that is the question:
Whether 'tis nobler in the mind to suffer
The slings and arrows of outrageous fortune,
Or to take arms against a sea of troubles,
And by opposing end them? To die: to sleep;
No more; and by a sleep to say we end
The heart-ache and the thousand natural shocks
That flesh is heir to, 'tis a consummation
Devoutly to be wish'd. To die, to sleep;
To sleep: perchance to dream: ay, there's the rub;
For in that sleep of death what dreams may come
When we have shuffled off this mortal coil,
Must give us pause: there's the respect
That makes calamity of so long life;
For who would bear the whips and scorns of time,
The oppressor's wrong, the proud man's contumely,
The pangs of despised love, the law's delay,
The insolence of office and the spurns
That patient merit of the unworthy takes,
When he himself might his quietus make
With a bare bodkin? who would fardels bear,
To grunt and sweat under a weary life,
But that the dread of something after death,
The undiscover'd country from whose bourn
No traveller returns, puzzles the will
And makes us rather bear those ills we have
Than fly to others that we know not of?
Thus conscience does make cowards of us all;
And thus the native hue of resolution
Is sicklied o'er with the pale cast of thought,
And enterprises of great pith and moment
With this regard their currents turn awry,
And lose the name of action. Soft you now!
The fair Ophelia! Nymph, in thy orisons
Be all my sins remember'd.

Friends, Romans, countrymen, lend me your ears;
I come to bury Caesar, not to praise him.
The evil that men do lives after them;
The good is oft interred with their bones;
So let it be with Caesar. The noble Brutus
Hath told you Caesar was ambitious:
If it were so, it was a grievous fault,
And grievously hath Caesar answer'd it.
Here, under leave of Brutus and the rest--
For Brutus is an honourable man;
So are they all, all honourable men--
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
";
    }
}
