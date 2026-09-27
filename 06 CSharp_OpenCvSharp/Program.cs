using System;
using System.Collections.Generic;
using OpenCvSharpExamples.Samples;
using OpenCvSharpExamples.Samples._01_Basic;
using OpenCvSharpExamples.Samples._02_ImageProcessing;
using OpenCvSharpExamples.Samples._03_FeatureAnalysis;
using OpenCvSharpExamples.Samples._04_Advanced;

namespace OpenCvSharpExamples
{
    /// <summary>
    /// 程序主入口，采用控制台菜单方式运行所有示例
    /// 严格 C# 2.0 语法：显式类型、无 var、无 LINQ、无 lambda
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // 注册所有示例
            List<SampleBase> samples = new List<SampleBase>();

            // 01 基础
            samples.Add(new HelloOpenCV());
            samples.Add(new ImageIO());
            samples.Add(new PixelAccess());
            samples.Add(new Drawing());
            samples.Add(new ROIAndMask());
            samples.Add(new ColorSpace());

            // 02 图像处理
            samples.Add(new Blurring());
            samples.Add(new EdgeDetection());
            samples.Add(new Morphology());
            samples.Add(new Threshold());
            samples.Add(new GeometricTransform());
            samples.Add(new Histogram());

            // 03 特征分析
            samples.Add(new Contours());
            samples.Add(new TemplateMatching());
            samples.Add(new HoughTransform());
            samples.Add(new CornerDetection());
            samples.Add(new ConnectedComponents());

            // 04 高级应用
            samples.Add(new FaceDetection());
            samples.Add(new VideoCaptureSample());
            samples.Add(new BackgroundSubtraction());
            samples.Add(new ImageStitching());

            Console.WriteLine("==============================================");
            Console.WriteLine("  OpenCvSharp C# 2.0 图像处理示例库");
            Console.WriteLine("==============================================");
            Console.WriteLine("OpenCV 版本: " + Cv2.GetVersionString());
            Console.WriteLine();

            if (args != null && args.Length > 0)
            {
                // 命令行传入编号直接运行
                RunByIndex(samples, args[0]);
                return;
            }

            // 显示菜单
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("请选择要运行的示例：");
                Console.WriteLine("  0  - 运行全部示例");
                for (int i = 0; i < samples.Count; i++)
                {
                    SampleBase s = samples[i];
                    Console.WriteLine("  {0,2} - [{1}] {2}", (i + 1), s.Index, s.Name);
                }
                Console.WriteLine("  q  - 退出");
                Console.Write("请输入编号 > ");

                string input = Console.ReadLine();
                if (input == null)
                {
                    continue;
                }
                input = input.Trim().ToLower();

                if (input == "q" || input == "quit" || input == "exit")
                {
                    break;
                }
                if (input == "0")
                {
                    RunAll(samples);
                    continue;
                }

                int choice;
                if (int.TryParse(input, out choice))
                {
                    if (choice >= 1 && choice <= samples.Count)
                    {
                        RunSample(samples[choice - 1]);
                    }
                    else
                    {
                        Console.WriteLine("编号超出范围！");
                    }
                }
                else
                {
                    Console.WriteLine("无效输入，请输入数字或 q 退出。");
                }
            }

            Cv2.DestroyAllWindows();
            Console.WriteLine("程序结束。");
        }

        static void RunAll(List<SampleBase> samples)
        {
            for (int i = 0; i < samples.Count; i++)
            {
                RunSample(samples[i]);
            }
        }

        static void RunByIndex(List<SampleBase> samples, string index)
        {
            for (int i = 0; i < samples.Count; i++)
            {
                if (samples[i].Index == index)
                {
                    RunSample(samples[i]);
                    return;
                }
            }
            Console.WriteLine("未找到编号为 " + index + " 的示例。");
        }

        static void RunSample(SampleBase sample)
        {
            Console.WriteLine();
            Console.WriteLine("==============================================");
            Console.WriteLine("运行示例: [{0}] {1}", sample.Index, sample.Name);
            Console.WriteLine("说明: {0}", sample.Description);
            Console.WriteLine("==============================================");
            try
            {
                sample.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("示例运行异常: " + ex.Message);
                Console.WriteLine(ex.StackTrace);
                Cv2.DestroyAllWindows();
            }
            Console.WriteLine();
            Console.WriteLine("示例 [" + sample.Index + "] 结束。");
        }
    }
}
