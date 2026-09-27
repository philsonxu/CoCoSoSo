using System;
using OpenCvSharp;

namespace OpenCvSharpExamples.Samples._04_Advanced
{
    /// <summary>
    /// 04-03 背景减除（运动检测）：BackgroundSubtractorMOG2 / KNN
    /// 知识点：BackgroundSubtractorMOG2、Apply、前景掩膜、轮廓提取运动目标
    /// </summary>
    public class BackgroundSubtraction : SampleBase
    {
        public override string Name { get { return "背景减除（运动目标检测）"; } }
        public override string Index { get { return "4.3"; } }
        public override string Description { get { return "使用 MOG2/KNN 背景减除法检测视频中的前景运动物体，并框出轮廓"; } }

        public override void Run()
        {
            Console.WriteLine("背景减除演示：将使用合成的运动场景。");
            int w = 500, h = 400;

            // 创建背景减除器
            using (BackgroundSubtractorMOG2 bgSubtractor =
                    BackgroundSubtractorMOG2.Create(500, 30, true))
            {
                bgSubtractor.DetectShadows = true;
                Mat frame = new Mat(h, w, MatType.CV_8UC3);
                Mat fgMask = new Mat();
                Mat bg = new Mat();
                Mat kernel = Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(3, 3));
                Random rand = new Random(1);

                Console.WriteLine("生成 60 帧运动序列...");
                for (int f = 0; f < 60; f++)
                {
                    // 模拟静态背景（简单棋盘纹理）
                    frame.SetTo(new Scalar(180, 180, 180));
                    for (int i = 0; i < w; i += 50)
                    {
                        Cv2.Line(frame, new Point(i, 0), new Point(i, h), new Scalar(170, 170, 170), 1);
                    }
                    for (int i = 0; i < h; i += 50)
                    {
                        Cv2.Line(frame, new Point(0, i), new Point(w, i), new Scalar(170, 170, 170), 1);
                    }

                    // 模拟运动物体
                    int x = 30 + f * 7;
                    int y = 200 + (int)(Math.Sin(f * 0.2) * 80);
                    Cv2.Circle(frame, new Point(x, y), 40, new Scalar(50, 50, 200), -1);
                    Cv2.Rectangle(frame, new Point(w - 200 + (f / 3) % 50, 50),
                        new Point(w - 100 + (f / 3) % 50, 150), new Scalar(50, 200, 50), -1);

                    // 加少量噪声
                    for (int k = 0; k < 50; k++)
                    {
                        int nx = rand.Next(w);
                        int ny = rand.Next(h);
                        frame.Set<Vec3b>(ny, nx, new Vec3b(0, 0, 0));
                    }

                    // 应用背景减除
                    bgSubtractor.Apply(frame, fgMask, 0.01);

                    // 形态学去噪
                    Cv2.MorphologyEx(fgMask, fgMask, MorphTypes.Open, kernel);
                    Cv2.MorphologyEx(fgMask, fgMask, MorphTypes.Close, kernel);

                    // 查找前景轮廓，绘制包围框
                    Point[][] contours;
                    HierarchyIndex[] hierarchy;
                    Cv2.FindContours(fgMask.Clone(), out contours, out hierarchy,
                        RetrievalModes.External, ContourApproximationModes.ApproxSimple);

                    Mat result = frame.Clone();
                    for (int i = 0; i < contours.Length; i++)
                    {
                        double area = Cv2.ContourArea(contours[i]);
                        if (area < 200) continue; // 忽略太小的区域
                        Rect box = Cv2.BoundingRect(contours[i]);
                        Cv2.Rectangle(result, box, new Scalar(0, 0, 255), 2);
                        Cv2.PutText(result, "Moving", new Point(box.X, box.Y - 5),
                            HersheyFonts.HersheyPlain, 1, new Scalar(0, 0, 255));
                    }

                    // 取背景图像（仅部分帧有效）
                    bgSubtractor.GetBackgroundImage(bg);

                    Cv2.ImShow("原帧", frame);
                    Cv2.ImShow("前景掩码 MOG2", fgMask);
                    Cv2.ImShow("背景模型+运动框", result);
                    if (!bg.Empty())
                    {
                        Cv2.ImShow("背景图像", bg);
                    }

                    int key = Cv2.WaitKey(50);
                    if (key == 27 || key == 'q') break;

                    result.Dispose();
                }

                Cv2.DestroyAllWindows();
                frame.Dispose(); fgMask.Dispose(); bg.Dispose(); kernel.Dispose();
            }

            // KNN 演示（快速）
            Console.WriteLine("同样帧数据用 KNN 背景减除法快速演示（前 30 帧）...");
            using (BackgroundSubtractorKNN knn = BackgroundSubtractorKNN.Create())
            {
                Mat frame = new Mat(h, w, MatType.CV_8UC3);
                Mat fgMask = new Mat();
                for (int f = 0; f < 30; f++)
                {
                    frame.SetTo(new Scalar(180, 180, 180));
                    for (int i = 0; i < w; i += 50)
                        Cv2.Line(frame, new Point(i, 0), new Point(i, h), new Scalar(170, 170, 170), 1);
                    for (int i = 0; i < h; i += 50)
                        Cv2.Line(frame, new Point(0, i), new Point(w, i), new Scalar(170, 170, 170), 1);
                    int x = 30 + f * 7;
                    int y = 200 + (int)(Math.Sin(f * 0.2) * 80);
                    Cv2.Circle(frame, new Point(x, y), 40, new Scalar(50, 50, 200), -1);
                    knn.Apply(frame, fgMask);
                    Cv2.ImShow("KNN 前景掩码", fgMask);
                    Cv2.WaitKey(40);
                }
                Cv2.DestroyAllWindows();
                frame.Dispose(); fgMask.Dispose();
            }
        }
    }
}
