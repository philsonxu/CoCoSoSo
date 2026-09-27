using System;
using OpenCvSharp;

namespace OpenCvSharpExamples.Samples._03_FeatureAnalysis
{
    /// <summary>
    /// 03-05 连通域分析：ConnectedComponents、ConnectedComponentsWithStats
    /// 知识点：ConnectedComponentsWithStats（标记图+统计信息+质心），用于斑点计数/颗粒分析
    /// </summary>
    public class ConnectedComponents : SampleBase
    {
        public override string Name { get { return "连通域分析（颗粒计数）"; } }
        public override string Index { get { return "3.5"; } }
        public override string Description { get { return "使用 ConnectedComponentsWithStats 检测并统计二值图像中的连通域，实现颗粒计数与属性分析"; } }

        public override void Run()
        {
            string imgPath = GetImagePath("blobs.png");
            Mat src = Cv2.ImRead(imgPath, ImreadModes.Grayscale);
            if (src.Empty())
            {
                // 生成若干大小不同的圆形颗粒
                src = Mat.Zeros(400, 600, MatType.CV_8UC1);
                Random rand = new Random(42);
                for (int i = 0; i < 25; i++)
                {
                    int x = rand.Next(30, src.Cols - 30);
                    int y = rand.Next(30, src.Rows - 30);
                    int r = rand.Next(10, 35);
                    Cv2.Circle(src, new Point(x, y), r, new Scalar(255), -1);
                }
            }

            // 二值化（OTSU）
            Mat binary = new Mat();
            Cv2.Threshold(src, binary, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);

            // 可选：形态学开运算去噪
            Mat kernel = Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(3, 3));
            Mat cleaned = new Mat();
            Cv2.MorphologyEx(binary, cleaned, MorphTypes.Open, kernel);

            // 连通域分析（带统计信息）
            Mat labels = new Mat();
            Mat stats = new Mat();
            Mat centroids = new Mat();
            int nLabels = Cv2.ConnectedComponentsWithStats(cleaned, labels, stats, centroids,
                PixelConnectivity.Connectivity8);

            Console.WriteLine("连通域总数（含背景）: " + nLabels);
            Console.WriteLine("背景区域: 0, 前景颗粒数: {0}", nLabels - 1);

            // 生成彩色结果图（每个连通域用不同颜色）
            Mat result = new Mat(src.Size(), MatType.CV_8UC3, new Scalar(0, 0, 0));
            Random rnd = new Random(0);
            Vec3b[] colors = new Vec3b[nLabels];
            colors[0] = new Vec3b(0, 0, 0); // 背景黑色
            for (int i = 1; i < nLabels; i++)
            {
                colors[i] = new Vec3b(
                    (byte)rnd.Next(80, 256),
                    (byte)rnd.Next(80, 256),
                    (byte)rnd.Next(80, 256));
            }

            for (int y = 0; y < result.Rows; y++)
            {
                for (int x = 0; x < result.Cols; x++)
                {
                    int lbl = labels.Get<int>(y, x);
                    result.Set<Vec3b>(y, x, colors[lbl]);
                }
            }

            // 绘制每个连通域的外接矩形和属性
            int validCount = 0;
            for (int i = 1; i < nLabels; i++)
            {
                int area = stats.Get<int>(i, (int)ConnectedComponentsTypes.Area);
                if (area < 20) continue; // 过滤太小的噪点
                validCount++;

                int x = stats.Get<int>(i, (int)ConnectedComponentsTypes.Left);
                int y = stats.Get<int>(i, (int)ConnectedComponentsTypes.Top);
                int w = stats.Get<int>(i, (int)ConnectedComponentsTypes.Width);
                int h = stats.Get<int>(i, (int)ConnectedComponentsTypes.Height);
                double cx = centroids.Get<double>(i, 0);
                double cy = centroids.Get<double>(i, 1);

                Cv2.Rectangle(result, new Rect(x, y, w, h), new Scalar(255, 255, 255), 1);
                Cv2.Circle(result, new Point((int)Math.Round(cx), (int)Math.Round(cy)),
                    2, new Scalar(255, 255, 255), -1);
                Cv2.PutText(result, "#" + i + "(" + area + ")",
                    new Point(x, y - 2), HersheyFonts.HersheyPlain, 0.5,
                    new Scalar(255, 255, 255));
            }
            Console.WriteLine("有效颗粒数量（面积>=20）: " + validCount);

            Cv2.ImShow("原图", src);
            Cv2.ImShow("二值化", binary);
            Cv2.ImShow("连通域着色+统计（共 " + validCount + " 个颗粒）", result);

            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();

            src.Dispose(); binary.Dispose(); kernel.Dispose(); cleaned.Dispose();
            labels.Dispose(); stats.Dispose(); centroids.Dispose(); result.Dispose();
        }
    }
}
