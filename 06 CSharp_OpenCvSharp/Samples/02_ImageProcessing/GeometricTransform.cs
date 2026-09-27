using System;
using OpenCvSharp;

namespace OpenCvSharpExamples.Samples._02_ImageProcessing
{
    /// <summary>
    /// 02-05 几何变换：缩放、平移、旋转、翻转、仿射变换、透视变换
    /// 知识点：Resize、WarpAffine、GetRotationMatrix2D、Flip、WarpPerspective、GetPerspectiveTransform
    /// </summary>
    public class GeometricTransform : SampleBase
    {
        public override string Name { get { return "几何变换（缩放/旋转/仿射/透视）"; } }
        public override string Index { get { return "2.5"; } }
        public override string Description { get { return "演示缩放、平移、旋转、翻转、仿射变换、透视变换等几何变换"; } }

        public override void Run()
        {
            string imgPath = GetImagePath("lena.png");
            Mat src = Cv2.ImRead(imgPath, ImreadModes.Color);
            if (src.Empty())
            {
                src = new Mat(300, 300, MatType.CV_8UC3, new Scalar(220, 220, 220));
                Cv2.Rectangle(src, new Point(50, 50), new Point(250, 200), new Scalar(0, 255, 0), -1);
                Cv2.Circle(src, new Point(150, 150), 60, new Scalar(255, 0, 0), -1);
                Cv2.PutText(src, "Transform", new Point(60, 280),
                    HersheyFonts.HersheySimplex, 0.8, new Scalar(0, 0, 0), 2);
            }

            // 1. 缩放 Resize（放大2倍、缩小一半）
            Mat scaledUp = new Mat();
            Mat scaledDown = new Mat();
            Cv2.Resize(src, scaledUp, new Size(src.Cols * 2, src.Rows * 2), 0, 0, InterpolationFlags.Linear);
            Cv2.Resize(src, scaledDown, new Size(src.Cols / 2, src.Rows / 2), 0, 0, InterpolationFlags.Area);

            // 2. 平移（仿射变换矩阵 [[1,0,tx],[0,1,ty]]）
            Mat transMat = new Mat(2, 3, MatType.CV_64FC1, new Scalar(0));
            transMat.Set<double>(0, 0, 1); transMat.Set<double>(0, 2, 50);
            transMat.Set<double>(1, 1, 1); transMat.Set<double>(1, 2, 30);
            Mat translated = new Mat();
            Cv2.WarpAffine(src, translated, transMat, new Size(src.Cols + 50, src.Rows + 50));

            // 3. 旋转（绕中心 45 度，缩放 0.8）
            Point2f center = new Point2f(src.Cols / 2f, src.Rows / 2f);
            Mat rotMat = Cv2.GetRotationMatrix2D(center, 45, 0.8);
            Mat rotated = new Mat();
            Cv2.WarpAffine(src, rotated, rotMat, src.Size());

            // 4. 翻转 Flip：0=上下翻转, 1=左右翻转, -1=中心翻转
            Mat flipH = new Mat();
            Mat flipV = new Mat();
            Mat flipHV = new Mat();
            Cv2.Flip(src, flipH, 1);
            Cv2.Flip(src, flipV, 0);
            Cv2.Flip(src, flipHV, -1);

            // 5. 仿射变换（三点映射：倾斜）
            Point2f[] srcTri = new Point2f[] {
                new Point2f(0, 0),
                new Point2f(src.Cols - 1, 0),
                new Point2f(0, src.Rows - 1)
            };
            Point2f[] dstTri = new Point2f[] {
                new Point2f(src.Cols * 0.1f, src.Rows * 0.1f),
                new Point2f(src.Cols * 0.9f, src.Rows * 0.0f),
                new Point2f(src.Cols * 0.2f, src.Rows * 0.9f)
            };
            Mat affineMat = Cv2.GetAffineTransform(srcTri, dstTri);
            Mat affine = new Mat();
            Cv2.WarpAffine(src, affine, affineMat, src.Size());

            // 6. 透视变换（四点映射）
            Point2f[] srcQuad = new Point2f[] {
                new Point2f(0, 0),
                new Point2f(src.Cols - 1, 0),
                new Point2f(src.Cols - 1, src.Rows - 1),
                new Point2f(0, src.Rows - 1)
            };
            Point2f[] dstQuad = new Point2f[] {
                new Point2f(src.Cols * 0.1f, src.Rows * 0.2f),
                new Point2f(src.Cols * 0.9f, src.Rows * 0.0f),
                new Point2f(src.Cols * 0.8f, src.Rows * 0.8f),
                new Point2f(src.Cols * 0.2f, src.Rows * 0.9f)
            };
            Mat perspMat = Cv2.GetPerspectiveTransform(srcQuad, dstQuad);
            Mat perspective = new Mat();
            Cv2.WarpPerspective(src, perspective, perspMat, src.Size());

            Console.WriteLine("几何变换完成。");
            Console.WriteLine("原图尺寸: {0}x{1}", src.Cols, src.Rows);
            Console.WriteLine("放大后尺寸: {0}x{1}", scaledUp.Cols, scaledUp.Rows);
            Console.WriteLine("缩小后尺寸: {0}x{1}", scaledDown.Cols, scaledDown.Rows);

            Cv2.ImShow("原图", src);
            Cv2.ImShow("放大 2x", scaledUp);
            Cv2.ImShow("缩小 1/2", scaledDown);
            Cv2.ImShow("平移 (50,30)", translated);
            Cv2.ImShow("旋转 45° 缩放0.8", rotated);
            Cv2.ImShow("左右翻转", flipH);
            Cv2.ImShow("上下翻转", flipV);
            Cv2.ImShow("仿射倾斜", affine);
            Cv2.ImShow("透视变换", perspective);

            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();

            src.Dispose(); scaledUp.Dispose(); scaledDown.Dispose();
            transMat.Dispose(); translated.Dispose();
            rotMat.Dispose(); rotated.Dispose();
            flipH.Dispose(); flipV.Dispose(); flipHV.Dispose();
            affineMat.Dispose(); affine.Dispose();
            perspMat.Dispose(); perspective.Dispose();
        }
    }
}
