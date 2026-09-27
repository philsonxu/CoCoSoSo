using System;
using OpenCvSharp;

namespace OpenCvSharpExamples.Samples._04_Advanced
{
    /// <summary>
    /// 04-04 图像拼接（Stitcher）：将多张有重叠区域的图像拼接成一幅全景图
    /// 知识点：Stitcher.Stitch、Stitcher.Status、多图拼接思路；附带图像加法/金字塔融合演示
    /// </summary>
    public class ImageStitching : SampleBase
    {
        public override string Name { get { return "图像拼接（全景图）+ 图像金字塔"; } }
        public override string Index { get { return "4.4"; } }
        public override string Description { get { return "演示图像拼接 Stitcher（合成全景）以及高斯/拉普拉斯金字塔"; } }

        public override void Run()
        {
            Console.WriteLine("=== A. 演示图像金字塔（高斯/拉普拉斯）===");
            string imgPath = GetImagePath("lena.png");
            Mat src = Cv2.ImRead(imgPath, ImreadModes.Color);
            if (src.Empty())
            {
                src = new Mat(400, 400, MatType.CV_8UC3, new Scalar(200, 180, 160));
                Cv2.Circle(src, new Point(200, 200), 100, new Scalar(0, 0, 255), -1);
                Cv2.Rectangle(src, new Point(50, 50), new Point(200, 200), new Scalar(0, 255, 0), -1);
            }

            // 高斯金字塔：反复下采样
            Mat g1 = new Mat();
            Mat g2 = new Mat();
            Mat g3 = new Mat();
            Cv2.PyrDown(src, g1);
            Cv2.PyrDown(g1, g2);
            Cv2.PyrDown(g2, g3);
            Console.WriteLine("高斯金字塔尺寸: G0={0}x{1}, G1={2}x{3}, G2={4}x{5}, G3={6}x{7}",
                src.Cols, src.Rows, g1.Cols, g1.Rows, g2.Cols, g2.Rows, g3.Cols, g3.Rows);

            // 拉普拉斯金字塔：L = G - PyrUp(G_{i+1})
            Mat g2Up = new Mat();
            Cv2.PyrUp(g1, g2Up, new Size(src.Cols, src.Rows));
            // 尺寸可能差1像素，裁剪到一致
            Mat laplacian0 = new Mat();
            Cv2.Resize(g2Up, g2Up, src.Size());
            Cv2.Subtract(src, g2Up, laplacian0);
            // 拉普拉斯结果像素值偏小，归一化显示
            Mat laplacianShow = new Mat();
            Cv2.Normalize(laplacian0, laplacianShow, 0, 255, NormTypes.MinMax);
            laplacianShow.ConvertTo(laplacianShow, MatType.CV_8UC3);

            Cv2.ImShow("原图 G0", src);
            Cv2.ImShow("G1 PyrDown", g1);
            Cv2.ImShow("G2 PyrDown", g2);
            Cv2.ImShow("G3 PyrDown", g3);
            Cv2.ImShow("L0 拉普拉斯金字塔", laplacianShow);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();

            g1.Dispose(); g2.Dispose(); g3.Dispose();
            g2Up.Dispose(); laplacian0.Dispose(); laplacianShow.Dispose();

            // === B. 图像拼接 Stitcher ===
            Console.WriteLine();
            Console.WriteLine("=== B. 图像拼接（Stitcher）===");
            Console.WriteLine("从原图左、中、右裁切三幅有重叠的子图模拟三视角，然后拼接。");

            int h = src.Rows;
            int w = src.Cols;
            int subW = (int)(w * 0.5); // 每幅子图宽度为原图50%，保证重叠
            int overlap = (int)(subW * 0.3); // 30% 重叠

            Mat leftImg = new Mat(src, new Rect(0, 0, subW, h)).Clone();
            Mat midImg = new Mat(src, new Rect(subW - overlap, 0, subW, h)).Clone();
            int rightStart = w - subW;
            Mat rightImg = new Mat(src, new Rect(rightStart, 0, subW, h)).Clone();

            // 如果子图超出（右边界），进行裁切
            Cv2.PutText(leftImg, "L", new Point(10, 30),
                HersheyFonts.HersheySimplex, 1, new Scalar(0, 0, 255), 2);
            Cv2.PutText(midImg, "M", new Point(10, 30),
                HersheyFonts.HersheySimplex, 1, new Scalar(0, 255, 0), 2);
            Cv2.PutText(rightImg, "R", new Point(10, 30),
                HersheyFonts.HersheySimplex, 1, new Scalar(255, 0, 0), 2);

            Mat[] imgs = new Mat[] { leftImg, midImg, rightImg };
            Mat pano = new Mat();
            try
            {
                using (Stitcher stitcher = Stitcher.Create(StitcherMode.Panorama))
                {
                    Stitcher.Status status = stitcher.Stitch(imgs, pano);
                    if (status == Stitcher.Status.OK)
                    {
                        Console.WriteLine("拼接成功！全景图尺寸: {0}x{1}", pano.Cols, pano.Rows);
                        Cv2.ImShow("左图", leftImg);
                        Cv2.ImShow("中图", midImg);
                        Cv2.ImShow("右图", rightImg);
                        Cv2.ImShow("拼接全景图", pano);
                    }
                    else
                    {
                        Console.WriteLine("Stitcher 拼接返回状态: " + status + "（可能因重叠不足，下方改用简单的横向拼接演示）");
                        ShowSimpleConcat(leftImg, midImg, rightImg);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Stitcher 调用异常: " + ex.Message);
                Console.WriteLine("改用简单横向拼接展示。");
                ShowSimpleConcat(leftImg, midImg, rightImg);
            }

            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();

            leftImg.Dispose(); midImg.Dispose(); rightImg.Dispose();
            pano.Dispose(); src.Dispose();
        }

        private void ShowSimpleConcat(Mat a, Mat b, Mat c)
        {
            // 简单 hconcat 拼接（无融合）
            Mat concat = new Mat();
            Cv2.HConcat(new Mat[] { a, b, c }, concat);
            Cv2.PutText(concat, "Simple HConcat (no blend)", new Point(10, a.Rows - 10),
                HersheyFonts.HersheySimplex, 0.6, new Scalar(0, 0, 255), 1);
            Cv2.ImShow("左", a);
            Cv2.ImShow("中", b);
            Cv2.ImShow("右", c);
            Cv2.ImShow("简单横向拼接", concat);
            concat.Dispose();
        }
    }
}
