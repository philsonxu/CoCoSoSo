using System;
using OpenCvSharp;

namespace OpenCvSharpExamples.Samples._01_Basic
{
    /// <summary>
    /// 01-06 颜色空间转换：BGR-GRAY、BGR-HSV、颜色范围提取（InRange）
    /// 知识点：CvtColor、ColorConversionCodes、InRange 颜色分割、肤色/颜色物体提取
    /// </summary>
    public class ColorSpace : SampleBase
    {
        public override string Name { get { return "颜色空间转换与颜色提取"; } }
        public override string Index { get { return "1.6"; } }
        public override string Description { get { return "演示 BGR-GRAY/HSV 颜色空间转换，以及用 InRange 提取特定颜色区域"; } }

        public override void Run()
        {
            string imgPath = GetImagePath("colors.png");
            Mat src = Cv2.ImRead(imgPath, ImreadModes.Color);
            if (src.Empty())
            {
                // 生成彩色测试图：包含红/绿/蓝区域
                src = new Mat(300, 500, MatType.CV_8UC3, new Scalar(255, 255, 255));
                Cv2.Rectangle(src, new Point(20, 20), new Point(160, 280), new Scalar(0, 0, 255), -1);
                Cv2.Rectangle(src, new Point(180, 20), new Point(320, 280), new Scalar(0, 255, 0), -1);
                Cv2.Rectangle(src, new Point(340, 20), new Point(480, 280), new Scalar(255, 0, 0), -1);
                Cv2.Circle(src, new Point(100, 150), 60, new Scalar(0, 165, 255), -1); // 橙色
            }

            // 1. BGR -> GRAY
            Mat gray = new Mat();
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

            // 2. BGR -> HSV
            Mat hsv = new Mat();
            Cv2.CvtColor(src, hsv, ColorConversionCodes.BGR2HSV);

            // 3. 提取红色（HSV 中红色 H 在 0~10 和 160~180 两段，需合并）
            Mat lowerRed = new Mat();
            Mat upperRed = new Mat();
            Cv2.InRange(hsv, new Scalar(0, 100, 100), new Scalar(10, 255, 255), lowerRed);
            Cv2.InRange(hsv, new Scalar(160, 100, 100), new Scalar(180, 255, 255), upperRed);
            Mat redMask = new Mat();
            Cv2.Add(lowerRed, upperRed, redMask);

            // 4. 提取蓝色（H:100-130）
            Mat blueMask = new Mat();
            Cv2.InRange(hsv, new Scalar(100, 100, 100), new Scalar(130, 255, 255), blueMask);

            // 5. 提取绿色（H:40-80）
            Mat greenMask = new Mat();
            Cv2.InRange(hsv, new Scalar(40, 100, 100), new Scalar(80, 255, 255), greenMask);

            // 6. 用掩码提取彩色区域
            Mat redRegion = new Mat();
            src.CopyTo(redRegion, redMask);
            Mat blueRegion = new Mat();
            src.CopyTo(blueRegion, blueMask);
            Mat greenRegion = new Mat();
            src.CopyTo(greenRegion, greenMask);

            Console.WriteLine("颜色空间转换完成。");
            Console.WriteLine("红色像素数: {0}", Cv2.CountNonZero(redMask));
            Console.WriteLine("绿色像素数: {0}", Cv2.CountNonZero(greenMask));
            Console.WriteLine("蓝色像素数: {0}", Cv2.CountNonZero(blueMask));

            Cv2.ImShow("原图", src);
            Cv2.ImShow("灰度图", gray);
            Cv2.ImShow("HSV 空间", hsv);
            Cv2.ImShow("红色提取", redRegion);
            Cv2.ImShow("绿色提取", greenRegion);
            Cv2.ImShow("蓝色提取", blueRegion);

            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();

            src.Dispose(); gray.Dispose(); hsv.Dispose();
            lowerRed.Dispose(); upperRed.Dispose(); redMask.Dispose();
            blueMask.Dispose(); greenMask.Dispose();
            redRegion.Dispose(); blueRegion.Dispose(); greenRegion.Dispose();
        }
    }
}
