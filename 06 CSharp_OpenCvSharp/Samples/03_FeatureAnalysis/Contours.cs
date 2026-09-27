using System;
using OpenCvSharp;

namespace OpenCvSharpExamples.Samples._03_FeatureAnalysis
{
    /// <summary>
    /// 03-01 轮廓检测与分析：FindContours、DrawContours、轮廓特征（面积/周长/外接矩形/最小外接圆/凸包/近似多边形）
    /// 知识点：FindContours（检索模式+近似方法）、ContourArea、ArcLength、BoundingRect、MinEnclosingCircle、ConvexHull、ApproxPolyDP
    /// </summary>
    public class Contours : SampleBase
    {
        public override string Name { get { return "轮廓检测与形状分析"; } }
        public override string Index { get { return "3.1"; } }
        public override string Description { get { return "演示轮廓查找、绘制，以及轮廓的面积、周长、外接矩形、最小外接圆、凸包、多边形近似等几何特征"; } }

        public override void Run()
        {
            string imgPath = GetImagePath("shapes.png");
            Mat src = Cv2.ImRead(imgPath, ImreadModes.Color);
            if (src.Empty())
            {
                src = Mat.Zeros(400, 600, MatType.CV_8UC3);
                // 生成若干几何形状
                Cv2.Rectangle(src, new Point(50, 50), new Point(180, 180), new Scalar(255, 255, 255), -1);
                Cv2.Circle(src, new Point(350, 120), 60, new Scalar(255, 255, 255), -1);
                Point[] tri = new Point[] {
                    new Point(500, 50), new Point(460, 190), new Point(550, 190)
                };
                Cv2.FillConvexPoly(src, tri, new Scalar(255, 255, 255));
                // 不规则图形
                Point[] irreg = new Point[] {
                    new Point(80, 250), new Point(200, 230), new Point(250, 350),
                    new Point(140, 380), new Point(60, 340)
                };
                Cv2.FillPoly(src, new Point[][] { irreg }, new Scalar(255, 255, 255));
                // 旋转矩形（近似椭圆）
                Cv2.Ellipse(src, new Point(420, 320), new Size(100, 50), 20, 0, 360,
                    new Scalar(255, 255, 255), -1);
            }

            Mat gray = new Mat();
            Mat binary = new Mat();
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
            Cv2.Threshold(gray, binary, 127, 255, ThresholdTypes.Binary);

            // 查找轮廓
            Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(binary, out contours, out hierarchy,
                RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            Console.WriteLine("检测到轮廓数量: " + contours.Length);

            // 在彩色图上绘制轮廓，并标注每个轮廓的属性
            Mat result = src.Clone();
            Random rnd = new Random(0);

            for (int i = 0; i < contours.Length; i++)
            {
                Point[] contour = contours[i];
                double area = Cv2.ContourArea(contour);
                double perimeter = Cv2.ArcLength(contour, true);
                if (area < 20) continue; // 跳过噪点

                // 随机颜色绘制轮廓
                Scalar color = new Scalar(rnd.Next(100, 255), rnd.Next(100, 255), rnd.Next(100, 255));
                Cv2.DrawContours(result, contours, i, color, 2);

                // 外接矩形
                Rect bRect = Cv2.BoundingRect(contour);
                Cv2.Rectangle(result, bRect, new Scalar(0, 255, 0), 1);

                // 最小外接圆
                Point2f center;
                float radius;
                Cv2.MinEnclosingCircle(contour, out center, out radius);
                Cv2.Circle(result, (Point)center, (int)radius, new Scalar(0, 0, 255), 1);

                // 凸包
                Point[] hull = Cv2.ConvexHull(contour);
                Cv2.Polylines(result, new Point[][] { hull }, true, new Scalar(255, 0, 255), 1);

                // 多边形近似
                Point[] approx = Cv2.ApproxPolyDP(contour, 0.02 * perimeter, true);
                Cv2.Polylines(result, new Point[][] { approx }, true, new Scalar(255, 255, 0), 2);

                // 判断形状：根据顶点数
                string shapeName;
                if (approx.Length == 3) shapeName = "Triangle";
                else if (approx.Length == 4) shapeName = "Rectangle";
                else if (approx.Length >= 8) shapeName = "Circle/Ellipse";
                else shapeName = approx.Length + "-gon";

                // 标注编号
                Cv2.PutText(result, "#" + i + " " + shapeName,
                    new Point(bRect.X, bRect.Y - 5),
                    HersheyFonts.HersheyPlain, 0.9, new Scalar(0, 255, 255), 1);

                Console.WriteLine("  轮廓#{0}: 面积={1:F1}, 周长={2:F1}, 近似顶点数={3}, 形状={4}",
                    i, area, perimeter, approx.Length, shapeName);
            }

            Cv2.ImShow("原图", src);
            Cv2.ImShow("轮廓与几何特征（绿=外接矩形, 红=最小圆, 紫=凸包, 黄=近似多边形）", result);

            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();

            src.Dispose(); gray.Dispose(); binary.Dispose(); result.Dispose();
        }
    }
}
