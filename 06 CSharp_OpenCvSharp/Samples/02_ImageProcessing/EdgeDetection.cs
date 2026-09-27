using System;
using OpenCvSharp;

namespace OpenCvSharpExamples.Samples._02_ImageProcessing
{
    /// <summary>
    /// 02-02 边缘检测：Sobel、Laplacian、Canny、Scharr
    /// 知识点：Sobel/Laplacian/Canny/Scharr 算子，Absdiff，ConvertScaleAbs
    /// </summary>
    public class EdgeDetection : SampleBase
    {
        public override string Name { get { return "边缘检测算法"; } }
        public override string Index { get { return "2.2"; } }
        public override string Description { get { return "演示 Sobel、Laplacian、Canny、Scharr 四种经典边缘检测算子"; } }

        public override void Run()
        {
            string imgPath = GetImagePath("lena.png");
            Mat src = Cv2.ImRead(imgPath, ImreadModes.Grayscale);
            if (src.Empty())
            {
                src = new Mat(300, 400, MatType.CV_8UC1, new Scalar(200));
                Cv2.Rectangle(src, new Point(50, 50), new Point(250, 200), new Scalar(50), -1);
                Cv2.Circle(src, new Point(320, 150), 70, new Scalar(100), 3);
            }

            // 先进行高斯模糊以减少噪声对边缘检测的影响
            Mat blurred = new Mat();
            Cv2.GaussianBlur(src, blurred, new Size(3, 3), 0);

            // 1. Sobel 算子（分别求 x 和 y 方向梯度，再合并）
            Mat sobelX = new Mat();
            Mat sobelY = new Mat();
            Mat sobelXAbs = new Mat();
            Mat sobelYAbs = new Mat();
            Mat sobel = new Mat();
            Cv2.Sobel(blurred, sobelX, MatType.CV_16S, 1, 0, 3);
            Cv2.Sobel(blurred, sobelY, MatType.CV_16S, 0, 1, 3);
            Cv2.ConvertScaleAbs(sobelX, sobelXAbs);
            Cv2.ConvertScaleAbs(sobelY, sobelYAbs);
            Cv2.AddWeighted(sobelXAbs, 0.5, sobelYAbs, 0.5, 0, sobel);

            // 2. Scharr 算子（对 Sobel 的改进，精度更高）
            Mat scharrX = new Mat();
            Mat scharrY = new Mat();
            Mat scharrXAbs = new Mat();
            Mat scharrYAbs = new Mat();
            Mat scharr = new Mat();
            Cv2.Scharr(blurred, scharrX, MatType.CV_16S, 1, 0);
            Cv2.Scharr(blurred, scharrY, MatType.CV_16S, 0, 1);
            Cv2.ConvertScaleAbs(scharrX, scharrXAbs);
            Cv2.ConvertScaleAbs(scharrY, scharrYAbs);
            Cv2.AddWeighted(scharrXAbs, 0.5, scharrYAbs, 0.5, 0, scharr);

            // 3. Laplacian 算子
            Mat laplace = new Mat();
            Mat laplaceAbs = new Mat();
            Cv2.Laplacian(blurred, laplace, MatType.CV_16S, 3);
            Cv2.ConvertScaleAbs(laplace, laplaceAbs);

            // 4. Canny 边缘检测
            Mat canny = new Mat();
            double lowThresh = 50;
            double highThresh = 150;
            Cv2.Canny(blurred, canny, lowThresh, highThresh, 3, false);

            Console.WriteLine("Sobel 非零像素: {0}", Cv2.CountNonZero(sobel));
            Console.WriteLine("Scharr 非零像素: {0}", Cv2.CountNonZero(scharr));
            Console.WriteLine("Canny 非零像素: {0}", Cv2.CountNonZero(canny));

            Cv2.ImShow("原灰度图", src);
            Cv2.ImShow("Sobel 边缘", sobel);
            Cv2.ImShow("Scharr 边缘", scharr);
            Cv2.ImShow("Laplacian 边缘", laplaceAbs);
            Cv2.ImShow("Canny 边缘 (50,150)", canny);

            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();

            src.Dispose(); blurred.Dispose();
            sobelX.Dispose(); sobelY.Dispose(); sobelXAbs.Dispose(); sobelYAbs.Dispose(); sobel.Dispose();
            scharrX.Dispose(); scharrY.Dispose(); scharrXAbs.Dispose(); scharrYAbs.Dispose(); scharr.Dispose();
            laplace.Dispose(); laplaceAbs.Dispose(); canny.Dispose();
        }
    }
}
