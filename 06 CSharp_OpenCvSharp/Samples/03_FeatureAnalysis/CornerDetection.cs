using System;
using OpenCvSharp;

namespace OpenCvSharpExamples.Samples._03_FeatureAnalysis
{
    /// <summary>
    /// 03-04 角点检测：Harris 角点、Shi-Tomasi（GoodFeaturesToTrack）角点、FAST 关键点
    /// 知识点：CornerHarris、GoodFeaturesToTrack、FAST（SimpleBlobDetector 可扩展）、CornerSubPix
    /// </summary>
    public class CornerDetection : SampleBase
    {
        public override string Name { get { return "角点检测（Harris/Shi-Tomasi）"; } }
        public override string Index { get { return "3.4"; } }
        public override string Description { get { return "演示 Harris 角点、Shi-Tomasi（GoodFeaturesToTrack）角点检测算法"; } }

        public override void Run()
        {
            string imgPath = GetImagePath("chessboard.png");
            Mat src = Cv2.ImRead(imgPath, ImreadModes.Color);
            if (src.Empty())
            {
                src = new Mat(400, 500, MatType.CV_8UC3, new Scalar(240, 240, 240));
                // 画棋盘格方便展示角点
                int cell = 50;
                for (int r = 0; r < 7; r++)
                {
                    for (int c = 0; c < 9; c++)
                    {
                        if ((r + c) % 2 == 0)
                        {
                            Cv2.Rectangle(src, new Point(c * cell + 20, r * cell + 20),
                                new Point((c + 1) * cell + 20, (r + 1) * cell + 20),
                                new Scalar(30, 30, 30), -1);
                        }
                    }
                }
                // 加一个三角形
                Cv2.FillConvexPoly(src, new Point[] {
                    new Point(300, 320), new Point(250, 390), new Point(380, 390)
                }, new Scalar(100, 50, 150));
            }

            Mat gray = new Mat();
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
            Cv2.GaussianBlur(gray, gray, new Size(3, 3), 1);

            // === 1. Harris 角点检测 ===
            Mat harrisDst = new Mat();
            Mat harrisNorm = new Mat();
            Mat harrisScaled = new Mat();
            Cv2.CornerHarris(gray, harrisDst, 2, 3, 0.04);
            Cv2.Normalize(harrisDst, harrisNorm, 0, 255, NormTypes.MinMax);
            Cv2.ConvertScaleAbs(harrisNorm, harrisScaled);

            Mat harrisResult = src.Clone();
            int harrisCount = 0;
            for (int y = 0; y < harrisNorm.Rows; y++)
            {
                for (int x = 0; x < harrisNorm.Cols; x++)
                {
                    float v = harrisNorm.Get<float>(y, x);
                    if (v > 180) // 阈值
                    {
                        Cv2.Circle(harrisResult, new Point(x, y), 4, new Scalar(0, 0, 255), 2);
                        harrisCount++;
                    }
                }
            }
            Console.WriteLine("Harris 角点数量（阈值180）: " + harrisCount);

            // === 2. Shi-Tomasi GoodFeaturesToTrack ===
            Mat shitomasiResult = src.Clone();
            Point2f[] corners = Cv2.GoodFeaturesToTrack(gray, 100, 0.01, 10, null, 3, false, 0.04);
            Console.WriteLine("Shi-Tomasi 检测到角点: " + corners.Length);

            // 亚像素精度优化
            if (corners.Length > 0)
            {
                TermCriteria criteria = new TermCriteria(CriteriaTypes.Eps | CriteriaTypes.MaxIter, 100, 0.001);
                Cv2.CornerSubPix(gray, corners, new Size(5, 5), new Size(-1, -1), criteria);

                for (int i = 0; i < corners.Length; i++)
                {
                    Point2f pt = corners[i];
                    Cv2.Circle(shitomasiResult, new Point((int)Math.Round(pt.X), (int)Math.Round(pt.Y)),
                        4, new Scalar(255, 0, 0), -1);
                    // 标注序号
                    if (i < 20) // 只标注前20个
                    {
                        Cv2.PutText(shitomasiResult, i.ToString(),
                            new Point((int)pt.X + 5, (int)pt.Y - 5),
                            HersheyFonts.HersheyPlain, 0.5, new Scalar(0, 255, 0));
                    }
                }
            }

            // === 3. FAST 关键点检测 ===
            Mat fastResult = src.Clone();
            using (OpenCvSharp.FastFeatureDetector fast = OpenCvSharp.FastFeatureDetector.Create(30, true))
            {
                KeyPoint[] kps = fast.Detect(gray);
                Console.WriteLine("FAST 关键点数量: " + kps.Length);
                // 手动绘制关键点
                for (int i = 0; i < kps.Length; i++)
                {
                    Point pt = new Point((int)Math.Round(kps[i].Pt.X), (int)Math.Round(kps[i].Pt.Y));
                    Cv2.Circle(fastResult, pt, 3, new Scalar(0, 255, 0), 1);
                }
            }

            Cv2.ImShow("原图", src);
            Cv2.ImShow("Harris 角点 (红)", harrisResult);
            Cv2.ImShow("Shi-Tomasi 角点 (蓝) +亚像素", shitomasiResult);
            Cv2.ImShow("FAST 关键点 (绿)", fastResult);

            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();

            src.Dispose(); gray.Dispose();
            harrisDst.Dispose(); harrisNorm.Dispose(); harrisScaled.Dispose();
            harrisResult.Dispose(); shitomasiResult.Dispose(); fastResult.Dispose();
        }
    }
}
