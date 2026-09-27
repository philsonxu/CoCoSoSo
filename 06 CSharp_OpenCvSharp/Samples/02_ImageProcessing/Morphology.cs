using System;
using OpenCvSharp;

namespace OpenCvSharpExamples.Samples._02_ImageProcessing
{
    /// <summary>
    /// 02-03 形态学操作：腐蚀、膨胀、开/闭运算、形态学梯度、顶帽/黑帽
    /// 知识点：Erode、Dilate、MorphologyEx、GetStructuringElement、MorphShapes
    /// </summary>
    public class Morphology : SampleBase
    {
        public override string Name { get { return "形态学操作"; } }
        public override string Index { get { return "2.3"; } }
        public override string Description { get { return "演示腐蚀、膨胀、开/闭运算、形态学梯度、顶帽、黑帽等形态学操作"; } }

        public override void Run()
        {
            string imgPath = GetImagePath("shapes.png");
            Mat src = Cv2.ImRead(imgPath, ImreadModes.Grayscale);
            if (src.Empty())
            {
                src = Mat.Zeros(300, 450, MatType.CV_8UC1);
                // 画几个形状（白底黑图 or 黑底白图）
                Cv2.Rectangle(src, new Point(30, 30), new Point(150, 150), new Scalar(255), -1);
                Cv2.Circle(src, new Point(280, 100), 60, new Scalar(255), -1);
                Cv2.FillConvexPoly(src, new Point[] {
                    new Point(380, 40), new Point(430, 160), new Point(330, 160)
                }, new Scalar(255));
                // 加一些噪点（小白点和小黑点）
                Random rand = new Random(1);
                for (int i = 0; i < 200; i++)
                {
                    int x = rand.Next(src.Cols);
                    int y = rand.Next(src.Rows);
                    src.Set<byte>(y, x, (byte)255);
                }
            }

            // 二值化
            Mat binary = new Mat();
            Cv2.Threshold(src, binary, 127, 255, ThresholdTypes.Binary);

            // 定义结构元素
            Mat kernel3 = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
            Mat kernel5 = Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(5, 5));

            // 1. 腐蚀（消除小亮点，物体缩小）
            Mat eroded = new Mat();
            Cv2.Erode(binary, eroded, kernel3);

            // 2. 膨胀（填补小洞，物体扩大）
            Mat dilated = new Mat();
            Cv2.Dilate(binary, dilated, kernel3);

            // 3. 开运算（先腐蚀后膨胀，去小亮点噪声）
            Mat opened = new Mat();
            Cv2.MorphologyEx(binary, opened, MorphTypes.Open, kernel3);

            // 4. 闭运算（先膨胀后腐蚀，填小黑洞）
            Mat closed = new Mat();
            Cv2.MorphologyEx(binary, closed, MorphTypes.Close, kernel3);

            // 5. 形态学梯度（膨胀-腐蚀，提取边缘）
            Mat gradient = new Mat();
            Cv2.MorphologyEx(binary, gradient, MorphTypes.Gradient, kernel3);

            // 6. 顶帽（原图-开运算，提取亮细节）
            Mat tophat = new Mat();
            Cv2.MorphologyEx(binary, tophat, MorphTypes.TopHat, kernel5);

            // 7. 黑帽（闭运算-原图，提取暗细节）
            Mat blackhat = new Mat();
            Cv2.MorphologyEx(binary, blackhat, MorphTypes.BlackHat, kernel5);

            Console.WriteLine("形态学操作完成。");

            Cv2.ImShow("原二值图", binary);
            Cv2.ImShow("腐蚀 Erode", eroded);
            Cv2.ImShow("膨胀 Dilate", dilated);
            Cv2.ImShow("开运算 Open（去亮噪）", opened);
            Cv2.ImShow("闭运算 Close（填黑洞）", closed);
            Cv2.ImShow("形态学梯度 Gradient", gradient);
            Cv2.ImShow("顶帽 TopHat", tophat);
            Cv2.ImShow("黑帽 BlackHat", blackhat);

            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();

            src.Dispose(); binary.Dispose(); kernel3.Dispose(); kernel5.Dispose();
            eroded.Dispose(); dilated.Dispose(); opened.Dispose(); closed.Dispose();
            gradient.Dispose(); tophat.Dispose(); blackhat.Dispose();
        }
    }
}
