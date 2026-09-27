using System;
using OpenCvSharp;

namespace OpenCvSharpExamples.Samples._02_ImageProcessing
{
    /// <summary>
    /// 02-04 阈值分割：固定阈值、自适应阈值、Otsu 大津法
    /// 知识点：Threshold、AdaptiveThreshold、ThresholdTypes、AdaptiveThresholdTypes
    /// </summary>
    public class Threshold : SampleBase
    {
        public override string Name { get { return "阈值分割（二值化）"; } }
        public override string Index { get { return "2.4"; } }
        public override string Description { get { return "演示固定阈值、自适应阈值和 Otsu 自动阈值三种图像二值化方法"; } }

        public override void Run()
        {
            string imgPath = GetImagePath("text.png");
            Mat src = Cv2.ImRead(imgPath, ImreadModes.Grayscale);
            if (src.Empty())
            {
                src = new Mat(300, 500, MatType.CV_8UC1);
                // 生成渐变 + 文字
                for (int y = 0; y < src.Rows; y++)
                {
                    for (int x = 0; x < src.Cols; x++)
                    {
                        src.Set<byte>(y, x, (byte)(x * 255 / src.Cols));
                    }
                }
                Cv2.PutText(src, "OpenCvSharp 2024", new Point(60, 170),
                    HersheyFonts.HersheySimplex, 1.5, new Scalar(50), 3);
            }

            // 1. 固定阈值 Binary（黑白翻转）
            Mat threshBinary = new Mat();
            Cv2.Threshold(src, threshBinary, 127, 255, ThresholdTypes.Binary);

            // 2. 固定阈值 BinaryInv（反向）
            Mat threshBinaryInv = new Mat();
            Cv2.Threshold(src, threshBinaryInv, 127, 255, ThresholdTypes.BinaryInv);

            // 3. Trunc 截断阈值（高于阈值的截断）
            Mat threshTrunc = new Mat();
            Cv2.Threshold(src, threshTrunc, 127, 255, ThresholdTypes.Trunc);

            // 4. ToZero 阈值（低于阈值的置零）
            Mat threshToZero = new Mat();
            Cv2.Threshold(src, threshToZero, 127, 255, ThresholdTypes.Tozero);

            // 5. Otsu 大津法自动阈值
            Mat threshOtsu = new Mat();
            double otsuThresh = Cv2.Threshold(src, threshOtsu, 0, 255,
                ThresholdTypes.Binary | ThresholdTypes.Otsu);
            Console.WriteLine("Otsu 自动计算的阈值: " + otsuThresh);

            // 6. 自适应阈值（光照不均匀时效果好）
            Mat threshAdaptiveMean = new Mat();
            Mat threshAdaptiveGauss = new Mat();
            Cv2.AdaptiveThreshold(src, threshAdaptiveMean, 255,
                AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, 11, 2);
            Cv2.AdaptiveThreshold(src, threshAdaptiveGauss, 255,
                AdaptiveThresholdTypes.GaussianC, ThresholdTypes.Binary, 11, 2);

            Cv2.ImShow("原灰度图", src);
            Cv2.ImShow("固定阈值 Binary(127)", threshBinary);
            Cv2.ImShow("固定阈值 BinaryInv(127)", threshBinaryInv);
            Cv2.ImShow("Trunc 截断(127)", threshTrunc);
            Cv2.ImShow("ToZero(127)", threshToZero);
            Cv2.ImShow("Otsu 自动阈值", threshOtsu);
            Cv2.ImShow("自适应阈值 MeanC", threshAdaptiveMean);
            Cv2.ImShow("自适应阈值 GaussianC", threshAdaptiveGauss);

            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();

            src.Dispose();
            threshBinary.Dispose(); threshBinaryInv.Dispose();
            threshTrunc.Dispose(); threshToZero.Dispose();
            threshOtsu.Dispose();
            threshAdaptiveMean.Dispose(); threshAdaptiveGauss.Dispose();
        }
    }
}
