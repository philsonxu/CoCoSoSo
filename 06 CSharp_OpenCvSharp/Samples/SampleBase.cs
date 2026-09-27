using System;
using System.IO;
using OpenCvSharp;

namespace OpenCvSharpExamples.Samples
{
    /// <summary>
    /// 所有示例的抽象基类，提供公共的路径解析与运行入口
    /// C# 2.0 语法：abstract class、虚方法、显式类型声明
    /// </summary>
    public abstract class SampleBase
    {
        /// <summary>
        /// 数据根目录（相对于可执行文件，指向 Data 文件夹）
        /// </summary>
        protected string DataDir
        {
            get
            {
                string exeDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                return Path.Combine(exeDir, "..", "..", "Data");
            }
        }

        /// <summary>
        /// 输出目录（保存处理结果）
        /// </summary>
        protected string OutputDir
        {
            get
            {
                string dir = Path.Combine(DataDir, "..", "Output");
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                return dir;
            }
        }

        /// <summary>
        /// 获取测试图片路径
        /// </summary>
        protected string GetImagePath(string fileName)
        {
            return Path.Combine(DataDir, "Images", fileName);
        }

        /// <summary>
        /// 获取级联分类器路径
        /// </summary>
        protected string GetCascadePath(string fileName)
        {
            return Path.Combine(DataDir, "Cascade", fileName);
        }

        /// <summary>
        /// 获取数据文件路径
        /// </summary>
        protected string GetDataPath(string fileName)
        {
            return Path.Combine(DataDir, "Data", fileName);
        }

        /// <summary>
        /// 获取视频文件路径
        /// </summary>
        protected string GetVideoPath(string fileName)
        {
            return Path.Combine(DataDir, "Video", fileName);
        }

        /// <summary>
        /// 示例名称，用于显示菜单
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// 示例编号，用于分类排序
        /// </summary>
        public abstract string Index { get; }

        /// <summary>
        /// 示例功能描述
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// 执行示例，子类必须实现
        /// </summary>
        public abstract void Run();

        /// <summary>
        /// 等待用户按键的便捷方法
        /// </summary>
        protected void WaitForKey(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("按任意键继续...");
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

        /// <summary>
        /// 显示一幅图像并等待按键
        /// </summary>
        protected void ShowAndWait(string windowName, Mat image, string message)
        {
            Cv2.ImShow(windowName, image);
            Console.WriteLine(message);
            Console.WriteLine("按任意键继续...");
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

        /// <summary>
        /// 在控制台打印分隔线
        /// </summary>
        protected void PrintSeparator()
        {
            Console.WriteLine("--------------------------------------------------");
        }
    }
}
