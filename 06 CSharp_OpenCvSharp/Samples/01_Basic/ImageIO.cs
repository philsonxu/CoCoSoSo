using System;
using System.IO;
using OpenCvSharp;

namespace OpenCvSharpExamples.Samples._01_Basic
{
    /// <summary>
    /// 01-02 图像读写：ImRead、ImWrite、ImShow 基本用法
    /// 知识点：ImreadModes、图像信息（Rows/Cols/Channels/Type）、图像克隆、保存
    /// </summary>
    public class ImageIO : SampleBase
    {
        public override string Name { get { return "图像读取/显示/保存"; } }
        public override string Index { get { return "1.2"; } }
        public override string Description { get { return "演示图像文件的读取、属性查询、显示和保存（ImRead/ImWrite/ImShow）"; } }

        public override void Run()
        {
            string imgPath = GetImagePath("lena.png");
            if (!File.Exists(imgPath))
            {
                Console.WriteLine("测试图片不存在，将使用程序生成的测试图像。");
                imgPath = Path.Combine(OutputDir, "generated.png");
                Mat gen = new Mat(300, 400, MatType.CV_8UC3, new Scalar(200, 150, 100));
                Cv2.Circle(gen, new Point(200, 150), 80, new Scalar(0, 0, 255), -1);
                Cv2.ImWrite(imgPath, gen);
                gen.Dispose();
            }

            // 1. 彩色模式读取
            Mat colorImg = Cv2.ImRead(imgPath, ImreadModes.Color);
            if (colorImg.Empty())
            {
                Console.WriteLine("读取图像失败！");
                return;
            }
            Console.WriteLine("彩色图像信息：");
            Console.WriteLine("  尺寸: {0} x {1}", colorImg.Cols, colorImg.Rows);
            Console.WriteLine("  通道数: {0}", colorImg.Channels());
            Console.WriteLine("  深度: {0}", colorImg.Depth());
            Console.WriteLine("  类型: {0}", colorImg.Type());
            Console.WriteLine("  总像素数: {0}", colorImg.Total());
            Console.WriteLine("  每像素字节数: {0}", colorImg.ElemSize());

            // 2. 灰度模式读取
            Mat grayImg = Cv2.ImRead(imgPath, ImreadModes.Grayscale);
            Console.WriteLine("灰度图像通道数: {0}", grayImg.Channels());

            // 3. 显示
            Cv2.ImShow("彩色图", colorImg);
            Cv2.ImShow("灰度图", grayImg);

            // 4. 保存
            string savePath1 = Path.Combine(OutputDir, "io_color_copy.jpg");
            string savePath2 = Path.Combine(OutputDir, "io_gray.png");
            Cv2.ImWrite(savePath1, colorImg);
            Cv2.ImWrite(savePath2, grayImg);
            Console.WriteLine("已保存：" + savePath1);
            Console.WriteLine("已保存：" + savePath2);

            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
            colorImg.Dispose();
            grayImg.Dispose();
        }
    }
}
