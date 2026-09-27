using System;
using System.IO;
using OpenCvSharp;

namespace OpenCvSharpExamples.Samples._04_Advanced
{
    /// <summary>
    /// 04-01 人脸检测：使用 Haar 级联分类器 CascadeClassifier
    /// 知识点：CascadeClassifier、DetectMultiScale、人眼检测，矩形绘制
    /// </summary>
    public class FaceDetection : SampleBase
    {
        public override string Name { get { return "人脸/人眼检测（Haar 级联）"; } }
        public override string Index { get { return "4.1"; } }
        public override string Description { get { return "使用 Haar 级联分类器（Haar Cascade）检测图像中的人脸与眼睛"; } }

        public override void Run()
        {
            // 加载人脸级联分类器
            string faceXml = GetCascadePath("haarcascade_frontalface_default.xml");
            string eyeXml = GetCascadePath("haarcascade_eye.xml");

            if (!File.Exists(faceXml))
            {
                Console.WriteLine("未找到 Haar 级联文件: " + faceXml);
                Console.WriteLine("请将 haarcascade_frontalface_default.xml 放到 Data/Cascade/ 目录下。");
                Console.WriteLine("本示例会用程序生成一个示意结果图。");
                GenerateDemoResult();
                return;
            }

            CascadeClassifier faceCascade = new CascadeClassifier(faceXml);
            CascadeClassifier eyeCascade = null;
            if (File.Exists(eyeXml))
            {
                eyeCascade = new CascadeClassifier(eyeXml);
            }

            string imgPath = GetImagePath("faces.jpg");
            Mat src = Cv2.ImRead(imgPath, ImreadModes.Color);
            if (src.Empty())
            {
                src = GenerateFaceDemoImage();
            }

            Mat gray = new Mat();
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
            Cv2.EqualizeHist(gray, gray);

            // 检测人脸
            Rect[] faces = faceCascade.DetectMultiScale(gray, 1.1, 4, HaarDetectionTypes.ScaleImage, new Size(30, 30));
            Console.WriteLine("检测到人脸数: " + faces.Length);

            Mat result = src.Clone();
            for (int i = 0; i < faces.Length; i++)
            {
                Rect face = faces[i];
                // 绘制人脸框
                Cv2.Rectangle(result, face, new Scalar(0, 255, 0), 2);
                Cv2.PutText(result, "Face #" + (i + 1),
                    new Point(face.X, face.Y - 5),
                    HersheyFonts.HersheyPlain, 1.0, new Scalar(0, 255, 0), 1);

                // 在人脸区域中检测眼睛
                if (eyeCascade != null)
                {
                    Mat faceROI = new Mat(gray, face);
                    Rect[] eyes = eyeCascade.DetectMultiScale(faceROI, 1.1, 3,
                        HaarDetectionTypes.ScaleImage, new Size(20, 20));
                    Console.WriteLine("  人脸#{0} 中检测到眼睛数: {1}", i + 1, eyes.Length);
                    for (int j = 0; j < eyes.Length; j++)
                    {
                        Rect eye = eyes[j];
                        Rect eyeRect = new Rect(face.X + eye.X, face.Y + eye.Y, eye.Width, eye.Height);
                        Cv2.Rectangle(result, eyeRect, new Scalar(255, 0, 0), 2);
                    }
                    faceROI.Dispose();
                }
            }

            Cv2.ImShow("人脸检测结果（绿框=人脸，蓝框=眼睛）", result);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();

            faceCascade.Dispose();
            if (eyeCascade != null) eyeCascade.Dispose();
            src.Dispose(); gray.Dispose(); result.Dispose();
        }

        private void GenerateDemoResult()
        {
            Mat img = GenerateFaceDemoImage();
            Cv2.CvtColor(img, img, ColorConversionCodes.BGR2GRAY);
            Cv2.ImShow("提示：缺少级联文件，显示示意人脸图", img);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
            img.Dispose();
        }

        private Mat GenerateFaceDemoImage()
        {
            Mat img = new Mat(400, 500, MatType.CV_8UC3, new Scalar(200, 200, 200));
            // 绘制简单的人脸示意
            Cv2.Ellipse(img, new Point(250, 200), new Size(120, 150), 0, 0, 360,
                new Scalar(150, 200, 240), -1);
            Cv2.Circle(img, new Point(210, 170), 12, new Scalar(255, 255, 255), -1);
            Cv2.Circle(img, new Point(290, 170), 12, new Scalar(255, 255, 255), -1);
            Cv2.Circle(img, new Point(210, 170), 5, new Scalar(0, 0, 0), -1);
            Cv2.Circle(img, new Point(290, 170), 5, new Scalar(0, 0, 0), -1);
            Cv2.Ellipse(img, new Point(250, 240), new Size(40, 20), 0, 0, 180,
                new Scalar(80, 80, 180), 2);
            Cv2.PutText(img, "Please add haarcascade XML", new Point(80, 380),
                HersheyFonts.HersheySimplex, 0.6, new Scalar(0, 0, 255), 1);
            return img;
        }
    }
}
