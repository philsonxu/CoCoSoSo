using System;
using OpenCvSharp;

namespace OpenCvSharpExamples.Samples._02_ImageProcessing
{
    /// <summary>
    /// 02-06 直方图：CalcHist、归一化、均衡化、直方图比较
    /// 知识点：CalcHist、Normalize、EqualizeHist、CompareHist、直方图绘制
    /// </summary>
    public class Histogram : SampleBase
    {
        public override string Name { get { return "图像直方图计算与均衡化"; } }
        public override string Index { get { return "2.6"; } }
        public override string Description { get { return "演示直方图计算、绘制、直方图均衡化，以及直方图比较（相关性/巴氏距离）"; } }

        public override void Run()
        {
            string imgPath = GetImagePath("lena.png");
            Mat src = Cv2.ImRead(imgPath, ImreadModes.Color);
            if (src.Empty())
            {
                src = new Mat(300, 400, MatType.CV_8UC3);
                for (int y = 0; y < src.Rows; y++)
                {
                    for (int x = 0; x < src.Cols; x++)
                    {
                        byte v = (byte)(((x + y) * 128) % 256);
                        src.Set<Vec3b>(y, x, new Vec3b(v, v, v));
                    }
                }
            }

            Mat gray = new Mat();
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

            // 1. 灰度直方图计算
            Mat hist = new Mat();
            int[] channels = new int[] { 0 };
            int[] histSize = new int[] { 256 };
            Rangef[] ranges = new Rangef[] { new Rangef(0, 256) };
            Cv2.CalcHist(new Mat[] { gray }, channels, null, hist, 1, histSize, ranges);

            // 归一化直方图到 [0, 400]（用于显示）
            Cv2.Normalize(hist, hist, 0, 400, NormTypes.MinMax);

            // 绘制灰度直方图
            Mat histImage = new Mat(450, 512, MatType.CV_8UC3, new Scalar(255, 255, 255));
            int binW = 2;
            for (int i = 0; i < 256; i++)
            {
                float binVal = hist.Get<float>(i);
                Cv2.Rectangle(histImage,
                    new Point(i * binW, histImage.Rows),
                    new Point((i + 1) * binW - 1, histImage.Rows - (int)binVal),
                    new Scalar(100, 100, 100), -1);
            }

            // 2. BGR 三通道彩色直方图
            Mat[] bgrPlanes = Cv2.Split(src);
            Mat bHist = new Mat(), gHist = new Mat(), rHist = new Mat();
            Cv2.CalcHist(new Mat[] { bgrPlanes[0] }, new int[] { 0 }, null, bHist, 1, histSize, ranges);
            Cv2.CalcHist(new Mat[] { bgrPlanes[1] }, new int[] { 0 }, null, gHist, 1, histSize, ranges);
            Cv2.CalcHist(new Mat[] { bgrPlanes[2] }, new int[] { 0 }, null, rHist, 1, histSize, ranges);
            Cv2.Normalize(bHist, bHist, 0, 400, NormTypes.MinMax);
            Cv2.Normalize(gHist, gHist, 0, 400, NormTypes.MinMax);
            Cv2.Normalize(rHist, rHist, 0, 400, NormTypes.MinMax);

            Mat colorHist = new Mat(450, 512, MatType.CV_8UC3, new Scalar(255, 255, 255));
            for (int i = 0; i < 256; i++)
            {
                float bv = bHist.Get<float>(i);
                float gv = gHist.Get<float>(i);
                float rv = rHist.Get<float>(i);
                Cv2.Rectangle(colorHist,
                    new Point(i * binW, colorHist.Rows),
                    new Point((i + 1) * binW - 1, colorHist.Rows - (int)bv),
                    new Scalar(255, 0, 0), 1);
                Cv2.Rectangle(colorHist,
                    new Point(i * binW, colorHist.Rows),
                    new Point((i + 1) * binW - 1, colorHist.Rows - (int)gv),
                    new Scalar(0, 255, 0), 1);
                Cv2.Rectangle(colorHist,
                    new Point(i * binW, colorHist.Rows),
                    new Point((i + 1) * binW - 1, colorHist.Rows - (int)rv),
                    new Scalar(0, 0, 255), 1);
            }

            // 3. 直方图均衡化（提升对比度）
            Mat equalized = new Mat();
            Cv2.EqualizeHist(gray, equalized);

            // 均衡化后的直方图
            Mat eqHist = new Mat();
            Cv2.CalcHist(new Mat[] { equalized }, channels, null, eqHist, 1, histSize, ranges);
            Cv2.Normalize(eqHist, eqHist, 0, 400, NormTypes.MinMax);
            Mat eqHistImg = new Mat(450, 512, MatType.CV_8UC3, new Scalar(255, 255, 255));
            for (int i = 0; i < 256; i++)
            {
                float v = eqHist.Get<float>(i);
                Cv2.Rectangle(eqHistImg,
                    new Point(i * binW, eqHistImg.Rows),
                    new Point((i + 1) * binW - 1, eqHistImg.Rows - (int)v),
                    new Scalar(0, 0, 200), -1);
            }

            // 4. 直方图比较
            double corr = Cv2.CompareHist(hist, eqHist, HistCompMethods.Correl);
            Console.WriteLine("原直方图与均衡化直方图相关性: {0:F4}", corr);

            Console.WriteLine("直方图计算完成。");

            Cv2.ImShow("原灰度图", gray);
            Cv2.ImShow("灰度直方图", histImage);
            Cv2.ImShow("彩色直方图（BGR）", colorHist);
            Cv2.ImShow("均衡化后", equalized);
            Cv2.ImShow("均衡化后直方图", eqHistImg);

            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();

            src.Dispose(); gray.Dispose(); hist.Dispose(); histImage.Dispose();
            for (int i = 0; i < bgrPlanes.Length; i++) { bgrPlanes[i].Dispose(); }
            bHist.Dispose(); gHist.Dispose(); rHist.Dispose(); colorHist.Dispose();
            equalized.Dispose(); eqHist.Dispose(); eqHistImg.Dispose();
        }
    }
}
