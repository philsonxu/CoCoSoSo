using System;
using OpenCvSharp;

namespace OpenCvSharpExamples.Samples._01_Basic
{
    /// <summary>
    /// 01-01 Hello OpenCV：验证环境、创建空白图像、显示文字
    /// 知识点：Cv2.GetVersionString、Mat 构造、SetTo、PutText、ImShow
    /// </summary>
    public class HelloOpenCV : SampleBase
    {
        public override string Name { get { return "Hello OpenCV（环境验证）"; } }
        public override string Index { get { return "1.1"; } }
        public override string Description { get { return "验证 OpenCvSharp 环境，创建空白图像并显示 Hello 文字"; } }

        public override void Run()
        {
            // 获取 OpenCV 版本信息
            string version = Cv2.GetVersionString();
            int major = Cv2.GetVersionMajor();
            int minor = Cv2.GetVersionMinor();
            int revision = Cv2.GetVersionRevision();
            Console.WriteLine("OpenCV 版本: " + version);
            Console.WriteLine("主版本: {0}, 次版本: {1}, 修订: {2}", major, minor, revision);

            // 创建一个 400x600 的黑色空白 BGR 图像
            Mat image = new Mat(400, 600, MatType.CV_8UC3, new Scalar(30, 30, 30));

            // 在图像中央绘制文字
            Point org = new Point(50, 210);
            Cv2.PutText(image, "Hello OpenCvSharp!", org, HersheyFonts.HersheySimplex,
                1.5, new Scalar(0, 255, 0), 2, LineTypes.AntiAlias);

            Point org2 = new Point(100, 270);
            Cv2.PutText(image, "C# 2.0 Image Processing", org2, HersheyFonts.HersheySimplex,
                0.8, new Scalar(200, 200, 200), 1, LineTypes.AntiAlias);

            // 画一条装饰线
            Cv2.Line(image, new Point(50, 230), new Point(550, 230), new Scalar(0, 255, 255), 2);

            // 显示
            using (new Window("Hello OpenCV", WindowMode.AutoSize, image))
            {
                Console.WriteLine("已弹出「Hello OpenCV」窗口。");
                Cv2.WaitKey(0);
            }
            image.Dispose();
        }
    }
}
