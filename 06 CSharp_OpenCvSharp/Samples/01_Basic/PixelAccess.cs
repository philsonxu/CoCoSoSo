using System;
using OpenCvSharp;

namespace OpenCvSharpExamples.Samples._01_Basic
{
    /// <summary>
    /// 01-03 像素访问：Get/Set、At<>、指针式访问、MatExpr 运算
    /// 知识点：Vec3b、GetGeneric/SetGeneric、索引器、像素遍历、反相、亮度/对比度
    /// </summary>
    public class PixelAccess : SampleBase
    {
        public override string Name { get { return "像素级访问与遍历"; } }
        public override string Index { get { return "1.3"; } }
        public override string Description { get { return "演示多种像素访问方式（At、Get/Set、指针遍历）以及反相/亮度/对比度调整"; } }

        public override void Run()
        {
            string imgPath = GetImagePath("lena.png");
            Mat src = Cv2.ImRead(imgPath, ImreadModes.Color);
            if (src.Empty())
            {
                src = new Mat(300, 400, MatType.CV_8UC3, new Scalar(100, 150, 200));
                Cv2.Rectangle(src, new Point(50, 50), new Point(350, 250), new Scalar(50, 200, 50), -1);
            }

            // === 1. 反相（逐像素 Set 访问）===
            Mat inverted = new Mat(src.Size(), src.Type());
            for (int y = 0; y < src.Rows; y++)
            {
                for (int x = 0; x < src.Cols; x++)
                {
                    Vec3b p = src.Get<Vec3b>(y, x);
                    Vec3b np = new Vec3b(
                        (byte)(255 - p.Item0),
                        (byte)(255 - p.Item1),
                        (byte)(255 - p.Item2));
                    inverted.Set<Vec3b>(y, x, np);
                }
            }

            // === 2. 亮度/对比度调整 ===
            // 公式: g(x) = contrast * f(x) + brightness
            double alpha = 1.5;  // 对比度
            int beta = 30;        // 亮度
            Mat adjusted = new Mat();
            src.ConvertTo(adjusted, MatType.CV_8UC3, alpha, beta);

            // === 3. 通道拆分与合并 ===
            Mat[] bgr = Cv2.Split(src);
            Mat onlyBlue = new Mat(src.Size(), MatType.CV_8UC3, new Scalar(0, 0, 0));
            Mat[] mergeChannels = new Mat[3];
            mergeChannels[0] = bgr[0];
            mergeChannels[1] = Mat.Zeros(src.Size(), MatType.CV_8UC1);
            mergeChannels[2] = Mat.Zeros(src.Size(), MatType.CV_8UC1);
            Cv2.Merge(mergeChannels, onlyBlue);

            // 打印某个像素的值进行验证
            Vec3b center = src.Get<Vec3b>(src.Rows / 2, src.Cols / 2);
            Console.WriteLine("图像中心点像素 (B,G,R) = ({0},{1},{2})",
                center.Item0, center.Item1, center.Item2);

            Cv2.ImShow("原图", src);
            Cv2.ImShow("反相", inverted);
            Cv2.ImShow("亮度+对比度", adjusted);
            Cv2.ImShow("仅B通道", onlyBlue);

            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();

            src.Dispose();
            inverted.Dispose();
            adjusted.Dispose();
            onlyBlue.Dispose();
            for (int i = 0; i < bgr.Length; i++) { bgr[i].Dispose(); }
            for (int i = 0; i < mergeChannels.Length; i++) { mergeChannels[i].Dispose(); }
        }
    }
}
