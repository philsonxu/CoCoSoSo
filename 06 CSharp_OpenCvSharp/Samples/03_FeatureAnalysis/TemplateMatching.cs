using System;
using OpenCvSharp;

namespace OpenCvSharpExamples.Samples._03_FeatureAnalysis
{
    /// <summary>
    /// 03-02 模板匹配：MatchTemplate、MinMaxLoc、六种匹配方法
    /// 知识点：MatchTemplate (SQDIFF/SQDIFF_NORMED/CCORR/CCORR_NORMED/CCOEFF/CCOEFF_NORMED)、MinMaxLoc
    /// </summary>
    public class TemplateMatching : SampleBase
    {
        public override string Name { get { return "模板匹配"; } }
        public override string Index { get { return "3.2"; } }
        public override string Description { get { return "演示使用模板匹配在大图中寻找小图案（6 种匹配方法对比）"; } }

        public override void Run()
        {
            string imgPath = GetImagePath("board.png");
            Mat src = Cv2.ImRead(imgPath, ImreadModes.Color);
            Mat templ;
            if (src.Empty())
            {
                // 生成测试场景
                src = new Mat(400, 600, MatType.CV_8UC3, new Scalar(220, 220, 220));
                for (int i = 0; i < 10; i++)
                {
                    int x = (i * 67 + 30) % 520;
                    int y = (i * 43 + 50) % 340;
                    Cv2.Rectangle(src, new Point(x, y), new Point(x + 40, y + 40),
                        new Scalar(50, 50, 200), -1);
                }
                // 截取模板（从其中一个方块）
                templ = new Mat(src, new Rect(30, 50, 40, 40)).Clone();
            }
            else
            {
                // 若有原图，从左上角区域裁切一块作为模板
                int tw = Math.Min(80, src.Cols / 6);
                int th = Math.Min(80, src.Rows / 6);
                templ = new Mat(src, new Rect(20, 20, tw, th)).Clone();
            }

            Console.WriteLine("原图: {0}x{1}, 模板: {2}x{3}", src.Cols, src.Rows, templ.Cols, templ.Rows);

            TemplateMatchModes[] methods = new TemplateMatchModes[] {
                TemplateMatchModes.SqDiff,
                TemplateMatchModes.SqDiffNormed,
                TemplateMatchModes.CCorr,
                TemplateMatchModes.CCorrNormed,
                TemplateMatchModes.CCoeff,
                TemplateMatchModes.CCoeffNormed
            };

            string[] methodNames = new string[] {
                "SQDIFF 平方差",
                "SQDIFF_NORMED 归一化平方差",
                "CCORR 相关",
                "CCORR_NORMED 归一化相关",
                "CCOEFF 相关系数",
                "CCOEFF_NORMED 归一化相关系数"
            };

            for (int m = 0; m < methods.Length; m++)
            {
                Mat result = new Mat();
                Cv2.MatchTemplate(src, templ, result, methods[m]);
                Cv2.Normalize(result, result, 0, 1, NormTypes.MinMax);

                double minVal, maxVal;
                Point minLoc, maxLoc;
                Cv2.MinMaxLoc(result, out minVal, out maxVal, out minLoc, out maxLoc);

                // 对于 SQDIFF / SQDIFF_NORMED，最小值为最佳匹配；其他方法最大值最佳
                Point matchLoc;
                if (methods[m] == TemplateMatchModes.SqDiff ||
                    methods[m] == TemplateMatchModes.SqDiffNormed)
                {
                    matchLoc = minLoc;
                }
                else
                {
                    matchLoc = maxLoc;
                }

                Mat display = src.Clone();
                Cv2.Rectangle(display, matchLoc,
                    new Point(matchLoc.X + templ.Cols, matchLoc.Y + templ.Rows),
                    new Scalar(0, 0, 255), 2);
                Cv2.PutText(display, methodNames[m], new Point(10, 30),
                    HersheyFonts.HersheySimplex, 0.7, new Scalar(255, 0, 0), 2);

                Cv2.ImShow("模板", templ);
                Cv2.ImShow("模板匹配结果: " + methodNames[m], display);
                Console.WriteLine("  {0}: minVal={1:F4}, maxVal={2:F4}, 匹配位置=({3},{4})",
                    methodNames[m], minVal, maxVal, matchLoc.X, matchLoc.Y);

                // 每种方法暂停一下便于观察
                int key = Cv2.WaitKey(0);
                Cv2.DestroyWindow("模板匹配结果: " + methodNames[m]);
                display.Dispose();
                result.Dispose();
            }

            Cv2.DestroyAllWindows();
            src.Dispose();
            templ.Dispose();
        }
    }
}
