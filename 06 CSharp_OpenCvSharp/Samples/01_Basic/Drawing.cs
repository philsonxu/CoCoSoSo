using System;
using OpenCvSharp;

namespace OpenCvSharpExamples.Samples._01_Basic
{
    /// <summary>
    /// 01-04 基本绘图：Line、Rectangle、Circle、Ellipse、Polygon、PutText
    /// 知识点：Scalar颜色、LineTypes、厚度/填充、 HersheyFonts
    /// </summary>
    public class Drawing : SampleBase
    {
        public override string Name { get { return "基本绘图"; } }
        public override string Index { get { return "1.4"; } }
        public override string Description { get { return "演示画线、矩形、圆、椭圆、多边形、文字等基本绘图函数"; } }

        public override void Run()
        {
            Mat canvas = new Mat(500, 700, MatType.CV_8UC3, new Scalar(255, 255, 255));

            // 1. 直线
            Cv2.Line(canvas, new Point(20, 30), new Point(200, 30), new Scalar(255, 0, 0), 2, LineTypes.Link8);
            Cv2.Line(canvas, new Point(20, 60), new Point(200, 60), new Scalar(0, 255, 0), 4, LineTypes.AntiAlias);

            // 2. 矩形
            Cv2.Rectangle(canvas, new Point(20, 80), new Point(180, 180), new Scalar(0, 0, 255), 2);
            Cv2.Rectangle(canvas, new Rect(200, 80, 160, 100), new Scalar(255, 0, 255), -1); // -1 表示填充

            // 3. 圆
            Cv2.Circle(canvas, new Point(460, 130), 50, new Scalar(0, 255, 255), 3);
            Cv2.Circle(canvas, new Point(580, 130), 40, new Scalar(128, 128, 0), -1);

            // 4. 椭圆
            Cv2.Ellipse(canvas, new Point(100, 280), new Size(80, 40),
                30, 0, 360, new Scalar(255, 128, 0), 2);
            Cv2.Ellipse(canvas, new RotatedRect(new Point2f(250, 280), new Size2f(120, 60), 0),
                new Scalar(0, 128, 255), 2);

            // 5. 多边形（填充三角形和五角星示意）
            Point[] triangle = new Point[]
            {
                new Point(420, 220), new Point(380, 340), new Point(480, 340)
            };
            Cv2.FillConvexPoly(canvas, triangle, new Scalar(0, 200, 0));

            Point[] poly = new Point[]
            {
                new Point(580, 220), new Point(600, 280), new Point(660, 280),
                new Point(610, 320), new Point(630, 380), new Point(580, 340),
                new Point(530, 380), new Point(550, 320), new Point(500, 280),
                new Point(560, 280)
            };
            Cv2.Polylines(canvas, new Point[][] { poly }, true, new Scalar(200, 0, 200), 2);

            // 6. 箭头线
            Cv2.ArrowedLine(canvas, new Point(20, 400), new Point(200, 400),
                new Scalar(0, 0, 0), 2, LineTypes.Link8, 0, 0.2);

            // 7. 文字
            Cv2.PutText(canvas, "OpenCvSharp Drawing", new Point(20, 470),
                HersheyFonts.HersheyTriplex, 1.0, new Scalar(0, 0, 0), 2, LineTypes.AntiAlias);
            Cv2.PutText(canvas, "C# 2.0", new Point(450, 470),
                HersheyFonts.HersheyScriptSimplex, 1.2, new Scalar(100, 100, 100), 2);

            // 8. 在图上标注各种形状名称
            Cv2.PutText(canvas, "Line", new Point(20, 22), HersheyFonts.HersheyPlain, 0.8, new Scalar(0, 0, 0));
            Cv2.PutText(canvas, "Rect", new Point(20, 205), HersheyFonts.HersheyPlain, 0.8, new Scalar(0, 0, 0));
            Cv2.PutText(canvas, "Circle", new Point(430, 205), HersheyFonts.HersheyPlain, 0.8, new Scalar(0, 0, 0));
            Cv2.PutText(canvas, "Ellipse", new Point(60, 360), HersheyFonts.HersheyPlain, 0.8, new Scalar(0, 0, 0));

            Cv2.ImShow("Drawing 基本绘图", canvas);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
            canvas.Dispose();
        }
    }
}
