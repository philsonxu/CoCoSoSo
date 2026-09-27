using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace CSharp20ImageProcessing
{
    public class ImageExamples
    {
        // 生成内置标准测试图，不依赖外部文件
        public static Bitmap CreateTestImage()
        {
            int w = 256, h = 256;
            Bitmap bmp = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(bmp);
            // 渐变背景
            for (int x = 0; x < w; x++)
            {
                using (Pen p = new Pen(Color.FromArgb(x, x, 255 - x / 2)))
                {
                    g.DrawLine(p, x, 0, x, h);
                }
            }
            // 彩色几何图形
            g.FillRectangle(Brushes.Red, 40, 40, 60, 60);
            g.FillEllipse(Brushes.LimeGreen, 120, 50, 80, 80);
            g.FillPolygon(Brushes.Yellow, new Point[] { new Point(80, 150), new Point(160, 150), new Point(120, 210) });
            g.FillRectangle(Brushes.Blue, 170, 140, 50, 70);
            // 灰阶条
            for (int i = 0; i < 8; i++)
            {
                int gray = i * 32;
                using (Brush b = new SolidBrush(Color.FromArgb(gray, gray, gray)))
                {
                    g.FillRectangle(b, 30 + i * 25, 220, 20, 25);
                }
            }
            g.Dispose();
            return bmp;
        }

        // 像素级灰度化（平均值法）
        public static Bitmap Grayscale(Bitmap src)
        {
            int w = src.Width, h = src.Height;
            Bitmap dst = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            BitmapData srcData = src.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData dstData = dst.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* srcPtr = (byte*)srcData.Scan0;
                byte* dstPtr = (byte*)dstData.Scan0;
                int stride = srcData.Stride;
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        byte b = srcPtr[y * stride + x * 3];
                        byte g = srcPtr[y * stride + x * 3 + 1];
                        byte r = srcPtr[y * stride + x * 3 + 2];
                        byte gray = (byte)((r + g + b) / 3);
                        dstPtr[y * stride + x * 3] = gray;
                        dstPtr[y * stride + x * 3 + 1] = gray;
                        dstPtr[y * stride + x * 3 + 2] = gray;
                    }
                }
            }
            src.UnlockBits(srcData);
            dst.UnlockBits(dstData);
            return dst;
        }

        // 加权灰度化（心理学公式：Y = 0.299R + 0.587G + 0.114B）
        public static Bitmap GrayscaleWeighted(Bitmap src)
        {
            int w = src.Width, h = src.Height;
            Bitmap dst = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            BitmapData srcData = src.LockBits(new Rectangle(0,0,w,h), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData dstData = dst.LockBits(new Rectangle(0,0,w,h), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* srcPtr = (byte*)srcData.Scan0;
                byte* dstPtr = (byte*)dstData.Scan0;
                int stride = srcData.Stride;
                for (int y=0;y<h;y++)
                    for (int x=0;x<w;x++)
                    {
                        byte b = srcPtr[y*stride+x*3];
                        byte g = srcPtr[y*stride+x*3+1];
                        byte r = srcPtr[y*stride+x*3+2];
                        byte gray = (byte)(0.299*r + 0.587*g + 0.114*b);
                        dstPtr[y*stride+x*3] = dstPtr[y*stride+x*3+1] = dstPtr[y*stride+x*3+2] = gray;
                    }
            }
            src.UnlockBits(srcData);
            dst.UnlockBits(dstData);
            return dst;
        }

        // 二值化
        public static Bitmap Binarize(Bitmap src, byte threshold)
        {
            Bitmap gray = GrayscaleWeighted(src);
            int w = gray.Width, h = gray.Height;
            Bitmap dst = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            BitmapData srcData = gray.LockBits(new Rectangle(0,0,w,h), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData dstData = dst.LockBits(new Rectangle(0,0,w,h), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* srcPtr = (byte*)srcData.Scan0;
                byte* dstPtr = (byte*)dstData.Scan0;
                int stride = srcData.Stride;
                for (int y=0;y<h;y++)
                    for (int x=0;x<w;x++)
                    {
                        byte v = srcPtr[y*stride+x*3] > threshold ? (byte)255 : (byte)0;
                        dstPtr[y*stride+x*3] = dstPtr[y*stride+x*3+1] = dstPtr[y*stride+x*3+2] = v;
                    }
            }
            gray.UnlockBits(srcData);
            dst.UnlockBits(dstData);
            gray.Dispose();
            return dst;
        }

        // 亮度调整
        public static Bitmap AdjustBrightness(Bitmap src, int delta)
        {
            int w = src.Width, h = src.Height;
            Bitmap dst = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            BitmapData srcData = src.LockBits(new Rectangle(0,0,w,h), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData dstData = dst.LockBits(new Rectangle(0,0,w,h), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* srcPtr = (byte*)srcData.Scan0;
                byte* dstPtr = (byte*)dstData.Scan0;
                int stride = srcData.Stride;
                for (int y=0;y<h;y++)
                    for (int x=0;x<w;x++)
                    {
                        for (int c=0;c<3;c++)
                        {
                            int v = srcPtr[y*stride+x*3+c] + delta;
                            if (v < 0) v = 0;
                            if (v > 255) v = 255;
                            dstPtr[y*stride+x*3+c] = (byte)v;
                        }
                    }
            }
            src.UnlockBits(srcData);
            dst.UnlockBits(dstData);
            return dst;
        }

        // 对比度调整
        public static Bitmap AdjustContrast(Bitmap src, double contrast)
        {
            int w = src.Width, h = src.Height;
            Bitmap dst = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            BitmapData srcData = src.LockBits(new Rectangle(0,0,w,h), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData dstData = dst.LockBits(new Rectangle(0,0,w,h), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* srcPtr = (byte*)srcData.Scan0;
                byte* dstPtr = (byte*)dstData.Scan0;
                int stride = srcData.Stride;
                for (int y=0;y<h;y++)
                    for (int x=0;x<w;x++)
                        for (int c=0;c<3;c++)
                        {
                            double v = ((srcPtr[y*stride+x*3+c] / 255.0 - 0.5) * contrast + 0.5) * 255;
                            if (v < 0) v = 0;
                            if (v > 255) v = 255;
                            dstPtr[y*stride+x*3+c] = (byte)v;
                        }
            }
            src.UnlockBits(srcData);
            dst.UnlockBits(dstData);
            return dst;
        }

        // 反色（底片效果）
        public static Bitmap Invert(Bitmap src)
        {
            int w = src.Width, h = src.Height;
            Bitmap dst = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            BitmapData srcData = src.LockBits(new Rectangle(0,0,w,h), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData dstData = dst.LockBits(new Rectangle(0,0,w,h), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* srcPtr = (byte*)srcData.Scan0;
                byte* dstPtr = (byte*)dstData.Scan0;
                int stride = srcData.Stride;
                for (int y=0;y<h;y++)
                    for (int x=0;x<w;x++)
                        for (int c=0;c<3;c++)
                            dstPtr[y*stride+x*3+c] = (byte)(255 - srcPtr[y*stride+x*3+c]);
            }
            src.UnlockBits(srcData);
            dst.UnlockBits(dstData);
            return dst;
        }

        // 怀旧/ Sepia 棕褐色滤镜
        public static Bitmap Sepia(Bitmap src)
        {
            int w = src.Width, h = src.Height;
            Bitmap dst = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            BitmapData srcData = src.LockBits(new Rectangle(0,0,w,h), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData dstData = dst.LockBits(new Rectangle(0,0,w,h), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* srcPtr = (byte*)srcData.Scan0;
                byte* dstPtr = (byte*)dstData.Scan0;
                int stride = srcData.Stride;
                for (int y=0;y<h;y++)
                    for (int x=0;x<w;x++)
                    {
                        int b = srcPtr[y*stride+x*3];
                        int g = srcPtr[y*stride+x*3+1];
                        int r = srcPtr[y*stride+x*3+2];
                        int nr = (int)(0.393*r + 0.769*g + 0.189*b);
                        int ng = (int)(0.349*r + 0.686*g + 0.168*b);
                        int nb = (int)(0.272*r + 0.534*g + 0.131*b);
                        if (nr > 255) nr = 255;
                        if (ng > 255) ng = 255;
                        if (nb > 255) nb = 255;
                        dstPtr[y*stride+x*3] = (byte)nb;
                        dstPtr[y*stride+x*3+1] = (byte)ng;
                        dstPtr[y*stride+x*3+2] = (byte)nr;
                    }
            }
            src.UnlockBits(srcData);
            dst.UnlockBits(dstData);
            return dst;
        }

        // 高斯模糊（3x3均值）
        public static Bitmap Blur3x3(Bitmap src)
        {
            int w = src.Width, h = src.Height;
            Bitmap dst = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            BitmapData srcData = src.LockBits(new Rectangle(0,0,w,h), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData dstData = dst.LockBits(new Rectangle(0,0,w,h), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* srcPtr = (byte*)srcData.Scan0;
                byte* dstPtr = (byte*)dstData.Scan0;
                int stride = srcData.Stride;
                // 边缘直接复制
                for (int y=0;y<h;y++)
                    for (int x=0;x<w;x++)
                    {
                        if (x == 0 || y == 0 || x == w-1 || y == h-1)
                        {
                            for (int c=0;c<3;c++)
                                dstPtr[y*stride+x*3+c] = srcPtr[y*stride+x*3+c];
                            continue;
                        }
                        int[] sum = new int[3];
                        for (int dy=-1;dy<=1;dy++)
                            for (int dx=-1;dx<=1;dx++)
                                for (int c=0;c<3;c++)
                                    sum[c] += srcPtr[(y+dy)*stride+(x+dx)*3+c];
                        for (int c=0;c<3;c++)
                            dstPtr[y*stride+x*3+c] = (byte)(sum[c]/9);
                    }
            }
            src.UnlockBits(srcData);
            dst.UnlockBits(dstData);
            return dst;
        }

        // 浮雕效果
        public static Bitmap Emboss(Bitmap src)
        {
            int w = src.Width, h = src.Height;
            Bitmap dst = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            BitmapData srcData = src.LockBits(new Rectangle(0,0,w,h), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData dstData = dst.LockBits(new Rectangle(0,0,w,h), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* srcPtr = (byte*)srcData.Scan0;
                byte* dstPtr = (byte*)dstData.Scan0;
                int stride = srcData.Stride;
                for (int y=0;y<h-1;y++)
                    for (int x=0;x<w-1;x++)
                        for (int c=0;c<3;c++)
                        {
                            int diff = srcPtr[y*stride+x*3+c] - srcPtr[(y+1)*stride+(x+1)*3+c] + 128;
                            if (diff < 0) diff = 0;
                            if (diff > 255) diff = 255;
                            dstPtr[y*stride+x*3+c] = (byte)diff;
                        }
                // 最后一行一列复制
                for (int x=0;x<w;x++)
                    for (int c=0;c<3;c++)
                        dstPtr[(h-1)*stride+x*3+c] = 128;
                for (int y=0;y<h;y++)
                    for (int c=0;c<3;c++)
                        dstPtr[y*stride+(w-1)*3+c] = 128;
            }
            src.UnlockBits(srcData);
            dst.UnlockBits(dstData);
            return dst;
        }

        // 图像缩放（最近邻插值）
        public static Bitmap ResizeNearest(Bitmap src, int newW, int newH)
        {
            Bitmap dst = new Bitmap(newW, newH, PixelFormat.Format24bppRgb);
            double sx = (double)src.Width / newW;
            double sy = (double)src.Height / newH;
            for (int y=0;y<newH;y++)
                for (int x=0;x<newW;x++)
                {
                    int sx1 = (int)(x * sx);
                    int sy1 = (int)(y * sy);
                    if (sx1 >= src.Width) sx1 = src.Width - 1;
                    if (sy1 >= src.Height) sy1 = src.Height - 1;
                    Color c = src.GetPixel(sx1, sy1);
                    dst.SetPixel(x, y, c);
                }
            return dst;
        }

        // 图像旋转90度
        public static Bitmap Rotate90(Bitmap src)
        {
            int w = src.Height, h = src.Width;
            Bitmap dst = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            for (int y=0;y<h;y++)
                for (int x=0;x<w;x++)
                    dst.SetPixel(x, y, src.GetPixel(src.Width - 1 - y, x));
            return dst;
        }

        // 水平镜像
        public static Bitmap MirrorHorizontal(Bitmap src)
        {
            int w = src.Width, h = src.Height;
            Bitmap dst = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            for (int y=0;y<h;y++)
                for (int x=0;x<w;x++)
                    dst.SetPixel(x, y, src.GetPixel(w - 1 - x, y));
            return dst;
        }

        // 计算灰度直方图
        public static int[] CalcHistogram(Bitmap gray)
        {
            int[] hist = new int[256];
            for (int i=0;i<256;i++) hist[i] = 0;
            for (int y=0;y<gray.Height;y++)
                for (int x=0;x<gray.Width;x++)
                    hist[gray.GetPixel(x,y).R]++;
            return hist;
        }

        // 绘制直方图
        public static Bitmap DrawHistogram(int[] hist)
        {
            int w = 256, h = 150;
            Bitmap bmp = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            int max = 0;
            for (int i=0;i<256;i++) if (hist[i] > max) max = hist[i];
            using (Pen p = new Pen(Color.Black))
            {
                for (int i=0;i<256;i++)
                {
                    int barH = (int)((double)hist[i] / max * (h-10));
                    g.DrawLine(p, i, h, i, h - barH);
                }
            }
            g.Dispose();
            return bmp;
        }

        // 各章节执行入口
        public static string RunChapter1()
        {
            HtmlConsole c = new HtmlConsole();
            c.WriteTitle("第1章：GDI+基础与测试图像生成");
            c.WriteSection("1.1 技术说明");
            c.WriteInfo("本章完全基于.NET Framework 2.0原生System.Drawing，无任何第三方依赖。通过LockBits+unsafe指针直接操作像素内存，性能比GetPixel/SetPixel快数十倍。");
            c.WriteCode(@"Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
Graphics g = Graphics.FromImage(bmp);
// 直接绘制图形
g.FillRectangle(Brushes.Red, 40, 40, 60, 60);
g.FillEllipse(Brushes.LimeGreen, 120, 50, 80, 80);");
            c.WriteSection("1.2 效果演示");
            Bitmap test = CreateTestImage();
            c.WriteSuccess("内置测试图像生成完成，尺寸：" + test.Width + "×" + test.Height + "像素，24位真彩色。");
            c.BeginImagePair();
            c.AddImage("标准测试图", test);
            c.AddImage("图像信息", CreateInfoBitmap(test));
            c.EndImagePair();
            test.Dispose();
            return c.GetHtml();
        }

        public static string RunChapter2()
        {
            HtmlConsole c = new HtmlConsole();
            c.WriteTitle("第2章：灰度化算法");
            c.WriteSection("2.1 算法原理");
            c.WriteInfo("灰度化是图像处理最基础操作，两种常用算法：\n1. 平均值法：Gray = (R+G+B)/3，简单但偏暗\n2. 加权法：Gray = 0.299R + 0.587G + 0.114B，人眼对绿色最敏感，加权后效果更自然。");
            c.WriteCode(@"byte gray = (byte)(0.299*r + 0.587*g + 0.114*b);
dst[y,x].R = dst[y,x].G = dst[y,x].B = gray;");
            Bitmap src = CreateTestImage();
            Bitmap avg = Grayscale(src);
            Bitmap weighted = GrayscaleWeighted(src);
            c.WriteSection("2.2 效果对比");
            c.ShowImages("原图", src, "平均值灰度", avg);
            c.ShowImages("心理学加权灰度", weighted, "两种灰度对比", CreateCompareBitmap(avg, weighted));
            src.Dispose(); avg.Dispose(); weighted.Dispose();
            return c.GetHtml();
        }

        public static string RunChapter3()
        {
            HtmlConsole c = new HtmlConsole();
            c.WriteTitle("第3章：二值化与阈值分割");
            c.WriteSection("3.1 原理");
            c.WriteInfo("二值化将图像变为只有黑白两色，常用于OCR文字识别、边缘提取、物体分割。阈值T：像素值>T设为白，否则设为黑。");
            c.WriteCode(@"byte v = gray > threshold ? (byte)255 : (byte)0;");
            Bitmap src = CreateTestImage();
            Bitmap bin128 = Binarize(src, 128);
            Bitmap bin180 = Binarize(src, 180);
            Bitmap bin80 = Binarize(src, 80);
            c.WriteSection("3.2 不同阈值效果");
            c.ShowImages("原图", src, "阈值=128（中等）", bin128);
            c.ShowImages("阈值=80（偏暗，多白色）", bin80, "阈值=180（偏亮，多黑色）", bin180);
            c.WriteWarning("阈值选择直接影响分割效果，实际项目常用Otsu大津法自动计算最佳阈值。");
            src.Dispose(); bin128.Dispose(); bin180.Dispose(); bin80.Dispose();
            return c.GetHtml();
        }

        public static string RunChapter4()
        {
            HtmlConsole c = new HtmlConsole();
            c.WriteTitle("第4章：亮度与对比度调整");
            c.WriteSection("4.1 亮度调整");
            c.WriteInfo("亮度调整：RGB每个通道统一加上偏移值delta，范围[-100, +100]。");
            c.WriteSection("4.2 对比度调整");
            c.WriteInfo("对比度调整公式：new = ((old/255 - 0.5) * contrast + 0.5) * 255\ncontrast>1增强对比，contrast<1降低对比，等于1不变。");
            Bitmap src = CreateTestImage();
            Bitmap bright = AdjustBrightness(src, 50);
            Bitmap dark = AdjustBrightness(src, -50);
            Bitmap highContrast = AdjustContrast(src, 2.0);
            Bitmap lowContrast = AdjustContrast(src, 0.5);
            c.WriteSection("4.3 效果演示");
            c.ShowImages("原图", src, "亮度+50（变亮）", bright);
            c.ShowImages("亮度-50（变暗）", dark, "对比度×2（高对比）", highContrast);
            c.AddImage("对比度×0.5（低对比）", lowContrast);
            c.EndImagePair();
            src.Dispose(); bright.Dispose(); dark.Dispose(); highContrast.Dispose(); lowContrast.Dispose();
            return c.GetHtml();
        }

        public static string RunChapter5()
        {
            HtmlConsole c = new HtmlConsole();
            c.WriteTitle("第5章：颜色特效（反色、棕褐色）");
            c.WriteSection("5.1 反色（底片效果）");
            c.WriteInfo("反色即每个通道取反：new = 255 - old，模拟彩色胶片底片效果。");
            c.WriteSection("5.2 怀旧棕褐色（Sepia）");
            c.WriteInfo("经典老照片滤镜，通过RGB矩阵变换，整体偏向黄褐色调。");
            Bitmap src = CreateTestImage();
            Bitmap invert = Invert(src);
            Bitmap sepia = Sepia(src);
            c.ShowImages("原图", src, "反色底片", invert);
            c.AddImage("怀旧棕褐色（老照片）", sepia);
            c.EndImagePair();
            src.Dispose(); invert.Dispose(); sepia.Dispose();
            return c.GetHtml();
        }

        public static string RunChapter6()
        {
            HtmlConsole c = new HtmlConsole();
            c.WriteTitle("第6章：图像滤镜（模糊、浮雕）");
            c.WriteSection("6.1 3×3均值模糊");
            c.WriteInfo("均值模糊是最简单的平滑滤镜，每个像素取周围3×3共9个像素的平均值，消除噪声。");
            c.WriteSection("6.2 浮雕效果");
            c.WriteInfo("浮雕算法：将当前像素与右下角像素做差+128，模拟光照下的凹凸浮雕效果。");
            Bitmap src = CreateTestImage();
            Bitmap blur = Blur3x3(src);
            Bitmap emboss = Emboss(src);
            c.ShowImages("原图", src, "3×3均值模糊", blur);
            c.AddImage("浮雕效果", emboss);
            c.EndImagePair();
            c.WriteWarning("注意：3×3内核处理边缘像素时需特殊处理，此处边缘直接复制原图避免黑边。");
            src.Dispose(); blur.Dispose(); emboss.Dispose();
            return c.GetHtml();
        }

        public static string RunChapter7()
        {
            HtmlConsole c = new HtmlConsole();
            c.WriteTitle("第7章：几何变换（缩放、旋转、镜像）");
            c.WriteSection("7.1 最近邻缩放");
            c.WriteInfo("最简单的缩放算法：目标像素对应回原图最近的像素，速度快但有锯齿。");
            c.WriteSection("7.2 旋转与镜像");
            c.WriteInfo("通过像素坐标映射实现几何变换：旋转90度、水平镜像翻转。");
            Bitmap src = CreateTestImage();
            Bitmap small = ResizeNearest(src, 128, 128);
            Bitmap large = ResizeNearest(src, 384, 384);
            Bitmap rotate = Rotate90(src);
            Bitmap mirror = MirrorHorizontal(src);
            c.ShowImages("原图256×256", src, "缩小到128×128（最近邻）", small);
            c.ShowImages("旋转90度", rotate, "水平镜像", mirror);
            c.AddImage("放大到384×384（可见锯齿）", large);
            c.EndImagePair();
            src.Dispose(); small.Dispose(); large.Dispose(); rotate.Dispose(); mirror.Dispose();
            return c.GetHtml();
        }

        public static string RunChapter8()
        {
            HtmlConsole c = new HtmlConsole();
            c.WriteTitle("第8章：灰度直方图");
            c.WriteSection("8.1 直方图原理");
            c.WriteInfo("直方图统计图像中每个灰度值（0-255）出现的像素次数，反映图像亮度分布：\n- 峰值偏左：图像偏暗\n- 峰值偏右：图像偏亮\n- 分布均匀：对比度好");
            Bitmap src = CreateTestImage();
            Bitmap gray = GrayscaleWeighted(src);
            int[] hist = CalcHistogram(gray);
            Bitmap histImg = DrawHistogram(hist);
            Bitmap dark = AdjustBrightness(src, -60);
            Bitmap darkGray = GrayscaleWeighted(dark);
            int[] darkHist = CalcHistogram(darkGray);
            Bitmap darkHistImg = DrawHistogram(darkHist);
            c.WriteSection("8.2 直方图对比");
            c.ShowImages("原图", src, "原图灰度直方图", histImg);
            c.ShowImages("变暗-60的图", dark, "偏暗图像直方图（峰值偏左）", darkHistImg);
            src.Dispose(); gray.Dispose(); histImg.Dispose(); dark.Dispose(); darkGray.Dispose(); darkHistImg.Dispose();
            return c.GetHtml();
        }

        public static string RunChapter9()
        {
            HtmlConsole c = new HtmlConsole();
            c.WriteTitle("第9章：LockBits高性能像素操作");
            c.WriteSection("9.1 性能对比");
            c.WriteInfo("C#图像处理两种像素访问方式：\n1. GetPixel/SetPixel：简单易用，但每次调用都有GDI+开销，处理大图极慢\n2. LockBits+unsafe指针：直接操作内存，性能接近C++，快50~100倍！");
            c.WriteCode(@"// 开启unsafe指针操作（项目属性勾选允许不安全代码）
BitmapData data = bmp.LockBits(rect, ImageLockMode.ReadWrite, format);
byte* ptr = (byte*)data.Scan0;
int stride = data.Stride; // 注意：行长度按4字节对齐！
ptr[y * stride + x*3] = 255; // B通道
bmp.UnlockBits(data);");
            Bitmap test = new Bitmap(800, 600);
            DateTime t1 = DateTime.Now;
            // 测试GetPixel
            for (int y=0;y<test.Height;y++)
                for (int x=0;x<test.Width;x++)
                    test.SetPixel(x,y,Color.White);
            TimeSpan ts1 = DateTime.Now - t1;
            DateTime t2 = DateTime.Now;
            Bitmap fast = new Bitmap(800,600,PixelFormat.Format24bppRgb);
            BitmapData d = fast.LockBits(new Rectangle(0,0,800,600), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* p = (byte*)d.Scan0;
                int s = d.Stride;
                for (int y=0;y<600;y++)
                    for (int x=0;x<800;x++)
                    {
                        p[y*s+x*3] = 255;
                        p[y*s+x*3+1] = 255;
                        p[y*s+x*3+2] = 255;
                    }
            }
            fast.UnlockBits(d);
            TimeSpan ts2 = DateTime.Now - t2;
            c.WriteSection("9.2 性能实测（800×600全白填充）");
            c.WriteResult(string.Format(
                "GetPixel/SetPixel方式耗时：{0:F2} 毫秒\nLockBits+unsafe指针方式耗时：{1:F2} 毫秒\n性能提升：{2:F1} 倍！",
                ts1.TotalMilliseconds, ts2.TotalMilliseconds, ts1.TotalMilliseconds / Math.Max(ts2.TotalMilliseconds, 0.01)));
            c.WriteWarning("重要提示：LockBits返回的Stride是扫描行长度，按4字节对齐，可能大于Width×3，绝对不能假设Stride = Width*3！");
            test.Dispose(); fast.Dispose();
            return c.GetHtml();
        }

        public static string RunChapter10()
        {
            HtmlConsole c = new HtmlConsole();
            c.WriteTitle("第10章：综合实战——简单画板");
            c.WriteSection("10.1 功能演示");
            c.WriteInfo("本章演示如何基于GDI+实现简单绘图功能：画笔、直线、矩形、椭圆、颜色选择、保存图片。所有功能均为原生GDI+实现，无任何第三方UI库。");
            Bitmap canvas = new Bitmap(256,256);
            Graphics g = Graphics.FromImage(canvas);
            g.Clear(Color.White);
            g.DrawLine(new Pen(Color.Red, 3), 20, 20, 200, 200);
            g.DrawRectangle(new Pen(Color.Blue, 2), 40, 40, 120, 80);
            g.FillEllipse(Brushes.LightGreen, 80, 100, 100, 80);
            g.DrawEllipse(new Pen(Color.Black, 1), 80, 100, 100, 80);
            using (Font f = new Font("宋体", 16))
                g.DrawString("C# GDI+ 绘图", f, Brushes.Purple, 40, 200);
            g.Dispose();
            c.WriteSection("10.2 绘图效果");
            c.AddImage("简单画板示例", canvas);
            c.EndImagePair();
            c.WriteSuccess("核心API：Graphics.DrawLine/DrawRectangle/FillEllipse/DrawString，配合Pen、Brush、Font类即可实现绝大多数2D绘图需求。");
            c.WriteDivider();
            c.WriteSuccess("恭喜完成全部10章学习！你已掌握C# 2.0原生GDI+图像处理核心技术，无需任何第三方库即可开发出完整的图像处理程序。");
            canvas.Dispose();
            return c.GetHtml();
        }

        private static Bitmap CreateInfoBitmap(Bitmap bmp)
        {
            Bitmap info = new Bitmap(240, 240);
            Graphics g = Graphics.FromImage(info);
            g.Clear(Color.FromArgb(45,45,48));
            using (Font f = new Font("Consolas", 10))
            using (Brush b = new SolidBrush(Color.FromArgb(156,220,254)))
            {
                g.DrawString("图像属性信息", f, b, 20, 20);
                g.DrawString("宽度: " + bmp.Width + " px", f, b, 20, 60);
                g.DrawString("高度: " + bmp.Height + " px", f, b, 20, 85);
                g.DrawString("像素格式: 24位RGB", f, b, 20, 110);
                g.DrawString("总像素: " + (bmp.Width*bmp.Height) + " 个", f, b, 20, 135);
                g.DrawString("单通道: 8位(0-255)", f, b, 20, 160);
                g.DrawString("总色数: 约1677万色", f, b, 20, 185);
            }
            g.Dispose();
            return info;
        }

        private static Bitmap CreateCompareBitmap(Bitmap a, Bitmap b)
        {
            int w = a.Width/2, h = a.Height;
            Bitmap cmp = new Bitmap(w*2, h);
            Graphics g = Graphics.FromImage(cmp);
            g.DrawImage(a, new Rectangle(0,0,w,h), new Rectangle(0,0,w,h), GraphicsUnit.Pixel);
            g.DrawImage(b, new Rectangle(w,0,w,h), new Rectangle(w,0,w,h), GraphicsUnit.Pixel);
            g.DrawLine(new Pen(Color.White, 2), w, 0, w, h);
            g.Dispose();
            return cmp;
        }
    }
}
