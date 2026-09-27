using System;
using OpenCvSharp;

namespace OpenCvSharpExamples.Samples._02_ImageProcessing
{
    /// <summary>
    /// 02-01 图像滤波/模糊：均值、高斯、中值、双边滤波
    /// 知识点：Blur、GaussianBlur、MedianBlur、BilateralFilter、BoxFilter、核大小与 sigma
    /// </summary>
    public class Blurring : SampleBase
    {
        public override string Name { get { return "图像滤波（模糊/去噪）"; } }
        public override string Index { get { return "2.1"; } }
        public override string Description { get { return "演示均值模糊、高斯模糊、中值模糊、双边滤波等常见去噪滤波算法"; } }

        public override void Run()
        {
            string imgPath = GetImagePath("lena.png");
            Mat src = Cv2.ImRead(imgPath, ImreadModes.Color);
            if (src.Empty())
            {
                src = new Mat(300, 400, MatType.CV_8UC3, new Scalar(200, 180, 160));
                Cv2.Randn(src, new Scalar(128, 128, 128), new Scalar(30, 30, 30));
            }

            // 给原图加椒盐噪声以展示中值滤波效果
            Mat noisy = src.Clone();
            Random rand = new Random(42);
            int noiseCount = (int)(src.Rows * src.Cols * 0.02);
            for (int i = 0; i < noiseCount; i++)
            {
                int x = rand.Next(src.Cols);
                int y = rand.Next(src.Rows);
                byte v = (byte)(rand.Next(2) * 255);
                noisy.Set<Vec3b>(y, x, new Vec3b(v, v, v));
            }

            // 1. 均值模糊 (归一化框滤波)
            Mat blur = new Mat();
            Cv2.Blur(noisy, blur, new Size(5, 5));

            // 2. 高斯模糊
            Mat gauss = new Mat();
            Cv2.GaussianBlur(noisy, gauss, new Size(5, 5), 1.5);

            // 3. 中值滤波（对椒盐噪声非常有效）
            Mat median = new Mat();
            Cv2.MedianBlur(noisy, median, 5);

            // 4. 双边滤波（保边去噪）
            Mat bilateral = new Mat();
            Cv2.BilateralFilter(noisy, bilateral, 9, 75, 75);

            // 5. 方框滤波（非归一化）
            Mat box = new Mat();
            Cv2.BoxFilter(noisy, box, MatType.CV_8UC3, new Size(5, 5), new Point(-1, -1), false);

            // 6. Sobel/Scharr 核做锐化（自定义卷积核）
            Mat sharpKernel = new Mat(3, 3, MatType.CV_32FC1, new Scalar(0));
            sharpKernel.Set<float>(0, 1, -1);
            sharpKernel.Set<float>(1, 0, -1);
            sharpKernel.Set<float>(1, 1, 5);
            sharpKernel.Set<float>(1, 2, -1);
            sharpKernel.Set<float>(2, 1, -1);
            Mat sharp = new Mat();
            Cv2.Filter2D(src, sharp, MatType.CV_8UC3, sharpKernel);

            Console.WriteLine("各种滤波处理完成。");

            Cv2.ImShow("原图", src);
            Cv2.ImShow("加椒盐噪声", noisy);
            Cv2.ImShow("均值模糊 Blur", blur);
            Cv2.ImShow("高斯模糊 GaussianBlur", gauss);
            Cv2.ImShow("中值滤波 MedianBlur（去椒盐）", median);
            Cv2.ImShow("双边滤波 BilateralFilter（保边）", bilateral);
            Cv2.ImShow("锐化 Filter2D", sharp);

            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();

            src.Dispose(); noisy.Dispose(); blur.Dispose(); gauss.Dispose();
            median.Dispose(); bilateral.Dispose(); box.Dispose();
            sharpKernel.Dispose(); sharp.Dispose();
        }
    }
}
