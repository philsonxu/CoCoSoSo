using System;
using OpenCvSharp;

namespace OpenCvSharpExamples.Samples._03_FeatureAnalysis
{
    /// <summary>
    /// 03-03 霍夫变换：HoughLines 直线、HoughLinesP 概率直线、HoughCircles 圆检测
    /// 知识点：HoughLines、HoughLinesP、HoughCircles、累加器阈值
    /// </summary>
    public class HoughTransform : SampleBase
    {
        public override string Name { get { return "霍夫变换（直线/圆检测）"; } }
        public override string Index { get { return "3.3"; } }
        public override string Description { get { return "演示霍夫直线变换（标准/概率）与霍夫圆变换，用于检测图像中的直线和圆形"; } }

        public override void Run()
        {
            string imgPath = GetImagePath("lines.png");
            Mat src = Cv2.ImRead(imgPath, ImreadModes.Color);
            if (src.Empty())
            {
                src = new Mat(400, 500, MatType.CV_8UC3, new Scalar(240, 240, 240));
                // 画一些直线和圆
                Cv2.Line(src, new Point(50, 50), new Point(450, 100), new Scalar(0, 0, 0), 3);
                Cv2.Line(src, new Point(100, 350), new Point(480, 50), new Scalar(0, 0, 0), 2);
                Cv2.Line(src, new Point(200, 20), new Point(200, 380), new Scalar(0, 0, 0), 2);
                Cv2.Line(src, new Point(20, 200), new Point(480, 200), new Scalar(0, 0, 0), 2);
                Cv2.Circle(src, new Point(350, 300), 50, new Scalar(0, 0, 255), 3);
                Cv2.Circle(src, new Point(100, 300), 40, new Scalar(0, 255, 0), 3);
            }

            Mat gray = new Mat();
            Mat edges = new Mat();
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
            Cv2.Canny(gray, edges, 50, 150, 3);

            // === 1. 标准霍夫直线变换 HoughLines ===
            Mat linesImg = src.Clone();
            LineSegmentPolar[] lines = Cv2.HoughLines(edges, 1, Math.PI / 180, 100);
            Console.WriteLine("HoughLines 检测到直线数: " + (lines == null ? 0 : lines.Length));
            if (lines != null)
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    float rho = lines[i].Rho;
                    float theta = lines[i].Theta;
                    double a = Math.Cos(theta);
                    double b = Math.Sin(theta);
                    double x0 = a * rho;
                    double y0 = b * rho;
                    Point pt1 = new Point(
                        (int)Math.Round(x0 + 1000 * (-b)),
                        (int)Math.Round(y0 + 1000 * (a)));
                    Point pt2 = new Point(
                        (int)Math.Round(x0 - 1000 * (-b)),
                        (int)Math.Round(y0 - 1000 * (a)));
                    Cv2.Line(linesImg, pt1, pt2, new Scalar(0, 0, 255), 1, LineTypes.AntiAlias);
                }
            }

            // === 2. 概率霍夫直线 HoughLinesP（线段端点）===
            Mat linesPImg = src.Clone();
            LineSegmentPoint[] lineSegs = Cv2.HoughLinesP(edges, 1, Math.PI / 180, 80, 30, 10);
            Console.WriteLine("HoughLinesP 检测到线段数: " + (lineSegs == null ? 0 : lineSegs.Length));
            if (lineSegs != null)
            {
                Random rnd = new Random(0);
                for (int i = 0; i < lineSegs.Length; i++)
                {
                    Scalar color = new Scalar(rnd.Next(100, 255), rnd.Next(100, 255), rnd.Next(50, 200));
                    Cv2.Line(linesPImg, lineSegs[i].P1, lineSegs[i].P2, color, 2, LineTypes.AntiAlias);
                }
            }

            // === 3. 霍夫圆变换 HoughCircles ===
            Mat circlesImg = src.Clone();
            // 先做高斯模糊以减少误检
            Mat blurred = new Mat();
            Cv2.GaussianBlur(gray, blurred, new Size(5, 5), 2);
            CircleSegment[] circles = Cv2.HoughCircles(blurred, HoughMethods.Gradient,
                1.5, 50, 100, 60, 10, 200);
            Console.WriteLine("HoughCircles 检测到圆数: " + (circles == null ? 0 : circles.Length));
            if (circles != null)
            {
                for (int i = 0; i < circles.Length; i++)
                {
                    Point center = new Point((int)Math.Round(circles[i].Center.X),
                                             (int)Math.Round(circles[i].Center.Y));
                    int radius = (int)Math.Round(circles[i].Radius);
                    Cv2.Circle(circlesImg, center, 3, new Scalar(0, 255, 0), -1);
                    Cv2.Circle(circlesImg, center, radius, new Scalar(0, 0, 255), 2);
                    Console.WriteLine("  圆#{0}: 圆心=({1},{2}), 半径={3}",
                        i, center.X, center.Y, radius);
                }
            }

            Cv2.ImShow("原图", src);
            Cv2.ImShow("Canny 边缘", edges);
            Cv2.ImShow("HoughLines 标准直线", linesImg);
            Cv2.ImShow("HoughLinesP 概率线段", linesPImg);
            Cv2.ImShow("HoughCircles 圆检测", circlesImg);

            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();

            src.Dispose(); gray.Dispose(); edges.Dispose();
            linesImg.Dispose(); linesPImg.Dispose(); circlesImg.Dispose(); blurred.Dispose();
        }
    }
}
