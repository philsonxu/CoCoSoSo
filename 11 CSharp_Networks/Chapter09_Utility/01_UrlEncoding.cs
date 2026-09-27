using System;
using System.Text;
using System.Web;

namespace CSharpNetworkProgramming.Chapter09_Utility
{
    /// <summary>
    /// 9.1 URL 编码/解码 与 HTML 实体编码
    /// 知识点：System.Web.HttpUtility.UrlEncode/UrlDecode、UrlPathEncode、
    ///         HtmlEncode/HtmlDecode、ParseQueryString、编码对比(UTF8/GB2312)
    /// </summary>
    public class _01_UrlEncoding : ISample
    {
        public string Id { get { return "9.1"; } }
        public string Title { get { return "URL/HTML 编解码工具"; } }
        public string Description { get { return "演示HttpUtility的常用编解码函数：URL编码/解码、HTML编码/解码、QueryString解析、不同字符集编码差异。"; } }

        public void Run()
        {
            // 1) URL 编码/解码
            Console.WriteLine("[1] URL 编解码 (UTF-8)");
            string[] raw = new string[] {
                "hello world",
                "中文测试",
                "key=value&foo=bar",
                "a+b=c&d=e f",
                "http://example.com/path?q=你好 世界",
                "!@#$%^&*()_+-=[]{}|;':\",./<>?"
            };
            foreach (string s in raw)
            {
                string encoded = HttpUtility.UrlEncode(s, Encoding.UTF8);
                string decoded = HttpUtility.UrlDecode(encoded, Encoding.UTF8);
                Console.WriteLine("    原始: {0}", s);
                Console.WriteLine("    编码: {0}", encoded);
                Console.WriteLine("    解码: {0}  {1}", decoded, decoded == s ? "✓一致" : "✗不一致");
                Console.WriteLine();
            }

            // 2) URL 路径编码
            Console.WriteLine("[2] UrlPathEncode vs UrlEncode 对比");
            string path = "/目录/文件 名.html?a=1&b=2";
            Console.WriteLine("    原始:        {0}", path);
            Console.WriteLine("    UrlEncode:   {0}", HttpUtility.UrlEncode(path));
            Console.WriteLine("    UrlPathEncode:{0}", HttpUtility.UrlPathEncode(path));
            Console.WriteLine("    说明：UrlPathEncode只对path部分编码，保留?&=分隔符用于URL构造");
            Console.WriteLine();

            // 3) HTML 编码/解码
            Console.WriteLine("[3] HTML 编解码");
            string html = "<h1>标题</h1><p>a & b <script>alert('xss')</script></p>\"'";
            string htmlEnc = HttpUtility.HtmlEncode(html);
            string htmlDec = HttpUtility.HtmlDecode(htmlEnc);
            Console.WriteLine("    原始: {0}", html);
            Console.WriteLine("    编码: {0}", htmlEnc);
            Console.WriteLine("    解码: {0}", htmlDec);
            Console.WriteLine();

            // 4) ParseQueryString 解析查询字符串
            Console.WriteLine("[4] ParseQueryString");
            string qs = "name=%E5%BC%A0%E4%B8%89&age=25&city=Beijing&hobby=reading&hobby=music";
            Console.WriteLine("    QueryString: {0}", qs);
            System.Collections.Specialized.NameValueCollection nvc = HttpUtility.ParseQueryString(qs, Encoding.UTF8);
            foreach (string key in nvc.AllKeys)
            {
                Console.WriteLine("    {0} = {1}", key, nvc[key]);
            }
            Console.WriteLine();

            // 5) 不同编码对比
            Console.WriteLine("[5] GB2312 vs UTF-8 编码差异");
            string chinese = "中文";
            Console.WriteLine("    字符串: {0}", chinese);
            Console.WriteLine("    UTF-8:   {0}", HttpUtility.UrlEncode(chinese, Encoding.UTF8));
            Console.WriteLine("    GB2312:  {0}", HttpUtility.UrlEncode(chinese, Encoding.GetEncoding("GB2312")));
            Console.WriteLine("    (一个中文字符在UTF-8占3字节%XX%XX%XX，GB2312占2字节%XX%XX)");
        }
    }
}
