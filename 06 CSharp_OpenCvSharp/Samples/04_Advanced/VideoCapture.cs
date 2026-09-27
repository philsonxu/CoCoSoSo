using System;
using System.IO;
using OpenCvSharp;

namespace OpenCvSharpExamples.Samples._04_Advanced
{
    /// <summary>
    /// 04-02 视频/摄像头采集：VideoCapture、帧读取、实时处理
    /// 知识点：VideoCapture（文件/摄像头）、Read、Get(CapProperty)、VideoWriter 保存视频
    /// 注意：类名使用 VideoCaptureSample 以避免与 OpenCvSharp.VideoCapture 冲突
    /// </summary>
    public class VideoCaptureSample : SampleBase
    {
        public override string Name { get { return "视频/摄像头采集与实时处理"; } }
        public override string Index { get { return "4.2"; } }
        public override string Description { get { return "演示从视频文件或摄像头读取帧，实时进行 Canny 边缘检测并可保存结果视频"; } }

        public override void Run()
        {
            Console.WriteLine("视频采集示例：");
            Console.WriteLine("  1) 若 Data/Video/ 下存在视频文件，将播放并实时处理该视频");
            Console.WriteLine("  2) 否则尝试打开默认摄像头（设备号0），按 q 退出");
            Console.WriteLine("  3) 若摄像头不可用，将生成动画帧进行演示");

            string videoPath = GetVideoPath("sample.mp4");
            OpenCvSharp.VideoCapture capture = null;
            bool isCamera = false;

            if (File.Exists(videoPath))
            {
                Console.WriteLine("打开视频文件: " + videoPath);
                capture = new OpenCvSharp.VideoCapture(videoPath);
            }
            else
            {
                Console.WriteLine("尝试打开默认摄像头...");
                capture = new OpenCvSharp.VideoCapture(0);
                isCamera = true;
            }

            if (!capture.IsOpened())
            {
                Console.WriteLine("无法打开摄像头或视频文件，将使用合成动画演示。");
                capture.Dispose();
                RunSyntheticDemo();
                return;
            }

            double fps = capture.Get(FrameProperty.Fps);
            if (fps <= 0 || double.IsNaN(fps)) fps = 30;
            int width = (int)capture.Get(FrameProperty.FrameWidth);
            int height = (int)capture.Get(FrameProperty.FrameHeight);
            Console.WriteLine("视频信息: {0}x{1}, {2:F1} fps", width, height, fps);

            string outPath = Path.Combine(OutputDir, "video_canny.mp4");
            OpenCvSharp.VideoWriter writer = null;
            try
            {
                writer = new OpenCvSharp.VideoWriter(outPath,
                    FourCC.XVID, fps, new Size(width, height), false);
            }
            catch (Exception)
            {
                writer = null;
            }

            Console.WriteLine("按 q 或 ESC 停止采集...");
            int frameCount = 0;
            Mat frame = new Mat();
            Mat gray = new Mat();
            Mat edges = new Mat();

            while (true)
            {
                if (!capture.Read(frame) || frame.Empty())
                {
                    break;
                }
                frameCount++;

                // 实时处理：灰度+Canny
                Cv2.CvtColor(frame, gray, ColorConversionCodes.BGR2GRAY);
                Cv2.Canny(gray, edges, 50, 150);

                // 在彩色帧上叠加边缘（黄色）
                Mat colorEdges = new Mat();
                Cv2.CvtColor(edges, colorEdges, ColorConversionCodes.GRAY2BGR);
                Mat overlay = new Mat();
                Cv2.AddWeighted(frame, 0.7, colorEdges, 0.5, 0, overlay);

                // 叠加帧号和FPS提示
                Cv2.PutText(overlay, "Frame: " + frameCount + "  FPS: " + fps.ToString("F1"),
                    new Point(10, 25), HersheyFonts.HersheyPlain, 1.5,
                    new Scalar(0, 255, 255), 2);

                if (writer != null && writer.IsOpened())
                {
                    writer.Write(edges);
                }

                Cv2.ImShow("原始帧", frame);
                Cv2.ImShow("Canny 边缘叠加", overlay);

                int key = Cv2.WaitKey(isCamera ? 1 : (int)(1000 / fps));
                if (key == 'q' || key == 'Q' || key == 27) break;

                colorEdges.Dispose();
                overlay.Dispose();
            }

            Console.WriteLine("共处理 {0} 帧。", frameCount);
            if (writer != null && writer.IsOpened())
            {
                Console.WriteLine("已保存处理后的视频: " + outPath);
            }

            Cv2.DestroyAllWindows();
            capture.Release();
            if (writer != null) writer.Release();
            frame.Dispose(); gray.Dispose(); edges.Dispose();
            capture.Dispose();
            if (writer != null) writer.Dispose();
        }

        private void RunSyntheticDemo()
        {
            Console.WriteLine("合成动画演示（50帧）...");
            int w = 400, h = 300;
            int frameCount = 0;
            Mat frame = new Mat(h, w, MatType.CV_8UC3);
            Mat gray = new Mat();
            Mat edges = new Mat();

            for (int f = 0; f < 50; f++)
            {
                frame.SetTo(new Scalar(30, 30, 30));
                Cv2.Circle(frame, new Point(w / 2 + (int)(Math.Cos(f * 0.1) * 80),
                    h / 2 + (int)(Math.Sin(f * 0.1) * 60)), 50, new Scalar(0, 255, 0), -1);
                Cv2.Rectangle(frame, new Point(20 + f * 2, 20),
                    new Point(120 + f * 2, 120), new Scalar(255, 0, 0), 2);
                Cv2.Line(frame, new Point(0, h - 1), new Point(w, (int)(h - 1 - f * 3)),
                    new Scalar(0, 0, 255), 2);

                Cv2.CvtColor(frame, gray, ColorConversionCodes.BGR2GRAY);
                Cv2.Canny(gray, edges, 50, 150);

                Cv2.ImShow("合成动画-彩色", frame);
                Cv2.ImShow("合成动画-Canny", edges);
                Cv2.WaitKey(30);
                frameCount++;
            }
            Console.WriteLine("演示完成，共 {0} 帧。", frameCount);
            Cv2.DestroyAllWindows();
            frame.Dispose(); gray.Dispose(); edges.Dispose();
        }
    }
}
