using System;
using System.IO;
using OpenCvSharp;

namespace OpenCvSharpExamples.Samples._01_Basic
{
    /// <summary>
    /// 01-05 ROI 与 Mask：感兴趣区域、掩码操作、CopyTo 带掩码、图像叠加
    /// 知识点：Mat 子区域（Mat[Rect]）、Bitwise 运算、掩码复制、AddWeighted
    /// </summary>
    public class ROIAndMask : SampleBase
    {
        public override string Name { get { return "ROI 感兴趣区域与 Mask 掩码"; } }
        public override string Index { get { return "1.5"; } }
        public override string Description { get { return "演示 ROI 子区域提取/修改、Mask 掩码操作、图像混合叠加（AddWeighted）"; } }

        public override void Run()
        {
            string imgPath = GetImagePath("lena.png");
            Mat src = Cv2.ImRead(imgPath, ImreadModes.Color);
            if (src.Empty())
            {
                src = new Mat(400, 400, MatType.CV_8UC3);
                // 生成渐变测试图
                for (int y = 0; y < src.Rows; y++)
                {
                    for (int x = 0; x < src.Cols; x++)
                    {
                        src.Set<Vec3b>(y, x, new Vec3b((byte)(y * 255 / src.Rows),
                            (byte)(x * 255 / src.Cols), 128));
                    }
                }
            }

            // 1. ROI：提取人脸区域示意（左上角 1/4 区域）
            Rect roi = new Rect(src.Cols / 4, src.Rows / 4, src.Cols / 2, src.Rows / 2);
            Mat roiMat = new Mat(src, roi);
            // 在 ROI 上画框（会反映到原图）
            Cv2.Rectangle(roiMat, new Point(0, 0), new Point(roiMat.Cols - 1, roiMat.Rows - 1),
                new Scalar(0, 0, 255), 3);

            // 2. 创建 Mask（圆形掩码）
            Mat mask = Mat.Zeros(src.Size(), MatType.CV_8UC1);
            Cv2.Circle(mask, new Point(src.Cols / 2, src.Rows / 2),
                Math.Min(src.Rows, src.Cols) / 3, new Scalar(255), -1);

            // 3. 使用掩码进行图像复制（仅复制掩码非零区域）
            Mat maskedImg = Mat.Zeros(src.Size(), src.Type());
            src.CopyTo(maskedImg, mask);

            // 4. 图像叠加：在右上角添加一个半透明红色色块
            Mat overlay = new Mat(src.Size(), src.Type(), new Scalar(0, 0, 0));
            Cv2.Rectangle(overlay, new Rect(src.Cols - 150, 20, 130, 80),
                new Scalar(0, 0, 255), -1);
            Mat blended = new Mat();
            Cv2.AddWeighted(src, 0.7, overlay, 0.3, 0, blended);
            Cv2.PutText(blended, "ROI Demo", new Point(src.Cols - 140, 70),
                HersheyFonts.HersheySimplex, 0.7, new Scalar(255, 255, 255), 2);

            // 5. 位运算：与/或/异或/反相
            Mat bitwiseAnd = new Mat();
            Cv2.BitwiseAnd(src, src, bitwiseAnd, mask);
            Mat bitwiseNot = new Mat();
            Cv2.BitwiseNot(src, bitwiseNot);

            Cv2.ImShow("原图+ROI框", blended);
            Cv2.ImShow("Mask", mask);
            Cv2.ImShow("按掩码CopyTo", maskedImg);
            Cv2.ImShow("BitwiseNot 反相", bitwiseNot);

            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();

            src.Dispose(); roiMat.Dispose(); mask.Dispose();
            maskedImg.Dispose(); overlay.Dispose(); blended.Dispose();
            bitwiseAnd.Dispose(); bitwiseNot.Dispose();
        }
    }
}
