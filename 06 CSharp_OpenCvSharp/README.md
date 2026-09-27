# OpenCvSharp C# 2.0 图像处理与应用源程序库

这是一套基于 **OpenCvSharp** 的 C# 图像处理示例源码合集，**全部代码采用 C# 2.0 语法**（显式类型声明、不使用 `var` / `lambda` / `LINQ` / 自动属性 / 扩展方法 / 匿名类型等 C# 3.0+ 特性），适用于 .NET Framework 2.0 及以上环境。从图像基础 IO 到高级视频/拼接/人脸检测，共 **22 个示例**，覆盖 OpenCvSharp 常用知识点。

---

## 📁 目录结构

```
OpenCvSharpExamples/
├── OpenCvSharpExamples.sln          # Visual Studio 解决方案文件
├── OpenCvSharpExamples.csproj       # 项目文件（.NET 2.0 目标框架）
├── Program.cs                       # 主入口（控制台菜单）
├── Properties/
│   └── AssemblyInfo.cs
├── Samples/
│   ├── SampleBase.cs                # 示例基类（路径/显示/公共工具）
│   ├── 01_Basic/                    # 第1章：基础操作（6 个示例）
│   ├── 02_ImageProcessing/          # 第2章：图像处理（6 个示例）
│   ├── 03_FeatureAnalysis/          # 第3章：特征分析（5 个示例）
│   └── 04_Advanced/                 # 第4章：高级应用（4 个示例）
├── Data/
│   ├── Images/                      # 测试图片（已内置 9 张）
│   ├── Video/                       # 测试视频目录（可放 sample.mp4）
│   ├── Cascade/                     # Haar 级联 XML 目录
│   └── Data/                        # 其他数据文件
├── Lib/                             # 可选：手工放置 OpenCvSharp DLL
├── Output/                          # 程序运行输出目录（自动生成）
└── README.md                        # 本说明文档
```

---

## ⚙️ 环境要求

| 项目 | 要求 |
|------|------|
| 操作系统 | Windows（OpenCvSharp4.runtime.win 支持）；跨平台版本需对应替换 runtime 包 |
| .NET Framework | **v2.0** 起（建议直接用 .NET Framework 4.6.1+ 或 .NET 6+ 编译运行，代码语法保持 C# 2.0 兼容） |
| IDE | Visual Studio 2005/2008/2010/2019/2022 或 JetBrains Rider；命令行 `msbuild` / `dotnet build` |
| OpenCvSharp | **OpenCvSharp4** (4.x) 推荐，通过 NuGet 安装；也兼容 OpenCvSharp 2.4.10 旧版（API 风格相近） |

### NuGet 包安装

在 **Visual Studio 包管理器控制台** 中执行：

```powershell
Install-Package OpenCvSharp4
Install-Package OpenCvSharp4.runtime.win
Install-Package OpenCvSharp4.Extensions
```

或通过 **NuGet 包管理器 UI** 搜索并安装：

- `OpenCvSharp4`
- `OpenCvSharp4.runtime.win`（Windows 原生运行时）

> **Linux/Mac 用户**：请安装对应平台的 runtime 包，例如 `OpenCvSharp4.runtime.ubuntu.20.04-x64`、`OpenCvSharp4.runtime.osx_arm64`。

---

## 🚀 快速开始

### 方式一：Visual Studio 打开

1. 双击 `OpenCvSharpExamples.sln` 打开解决方案；
2. 如果项目引用的 `Lib\OpenCvSharp.dll` 不存在，请删除该引用，改用 NuGet 安装 OpenCvSharp4；
3. 按 **F5** 或 **Ctrl+F5** 编译运行；
4. 控制台会显示示例菜单，输入编号即可运行对应示例。

### 方式二：升级到更高 .NET Framework（推荐）

若你的 VS 默认项目模板是 .NET Framework 4.x 或 .NET 6+：

1. 在解决方案资源管理器中右键项目 → 属性；
2. 将「目标框架」改为 `.NET Framework 4.8` 或 `.NET 6.0`；
3. 重新添加 OpenCvSharp4 NuGet 包；
4. 源码本身仍保持 **C# 2.0 语法**，编译运行不受影响。

### 方式三：命令行运行示例

程序支持命令行传入示例编号直接运行：

```bash
OpenCvSharpExamples.exe 1.1    # 运行 Hello OpenCV
OpenCvSharpExamples.exe 3.1    # 运行轮廓检测
OpenCvSharpExamples.exe 4.2    # 运行视频采集
```

---

## 📚 示例清单（共 22 个）

### 第一章：基础操作（01_Basic）

| 编号 | 文件名 | 知识点 |
|------|--------|--------|
| 1.1 | `HelloOpenCV.cs` | 环境验证、版本信息、`Mat` 构造、`PutText`、`Line`、窗口显示 |
| 1.2 | `ImageIO.cs` | `ImRead` / `ImWrite` / `ImShow`、`ImreadModes`、图像属性（Rows/Cols/Channels/Depth/Type/Total/ElemSize） |
| 1.3 | `PixelAccess.cs` | 像素访问 `Get<Vec3b>`/`Set<Vec3b>`、像素遍历、反相、亮度/对比度调整（`ConvertTo`）、通道拆分 `Split`/合并 `Merge` |
| 1.4 | `Drawing.cs` | 基本绘图：`Line`/`Rectangle`/`Circle`/`Ellipse`/`FillConvexPoly`/`Polylines`/`ArrowedLine`/`PutText`、`HersheyFonts`、`LineTypes` |
| 1.5 | `ROIAndMask.cs` | ROI 子区域（`Mat[Rect]`）、Mask 掩码、`CopyTo(mask)`、`AddWeighted` 图像混合、`BitwiseAnd/Not` 位运算 |
| 1.6 | `ColorSpace.cs` | 颜色空间转换 `CvtColor`（BGR↔GRAY、BGR↔HSV）、`InRange` 颜色分割、多区间合并提取红色 |

### 第二章：图像处理（02_ImageProcessing）

| 编号 | 文件名 | 知识点 |
|------|--------|--------|
| 2.1 | `Blurring.cs` | 图像滤波：`Blur`（均值）、`GaussianBlur`（高斯）、`MedianBlur`（中值去椒盐噪声）、`BilateralFilter`（双边保边）、`BoxFilter`、`Filter2D` 自定义卷积核（锐化） |
| 2.2 | `EdgeDetection.cs` | 边缘检测：`Sobel`、`Scharr`、`Laplacian`、`Canny`，含 `ConvertScaleAbs`、`AddWeighted` 合并梯度 |
| 2.3 | `Morphology.cs` | 形态学：`Erode` 腐蚀、`Dilate` 膨胀、`MorphologyEx`（开/闭/梯度/顶帽/黑帽）、`GetStructuringElement` 结构元素 |
| 2.4 | `Threshold.cs` | 阈值分割：固定阈值（`Binary/BinaryInv/Trunc/Tozero`）、Otsu 自动阈值、`AdaptiveThreshold` 自适应阈值（Mean/Gaussian） |
| 2.5 | `GeometricTransform.cs` | 几何变换：`Resize` 缩放、`WarpAffine` 平移、`GetRotationMatrix2D` 旋转、`Flip` 翻转、仿射变换 `GetAffineTransform`+`WarpAffine`、透视变换 `GetPerspectiveTransform`+`WarpPerspective` |
| 2.6 | `Histogram.cs` | 直方图：`CalcHist`、`Normalize`、三通道彩色直方图绘制、`EqualizeHist` 直方图均衡化、`CompareHist` 直方图比较 |

### 第三章：特征分析（03_FeatureAnalysis）

| 编号 | 文件名 | 知识点 |
|------|--------|--------|
| 3.1 | `Contours.cs` | 轮廓检测：`FindContours`（检索模式/近似方法）、`DrawContours`、轮廓属性：`ContourArea`/`ArcLength`/`BoundingRect`/`MinEnclosingCircle`/`ConvexHull`/`ApproxPolyDP` 多边形近似与形状识别 |
| 3.2 | `TemplateMatching.cs` | 模板匹配：`MatchTemplate` 六种方法（SQDIFF/SQDIFF_NORMED/CCORR/CCORR_NORMED/CCOEFF/CCOEFF_NORMED）、`MinMaxLoc` 最优匹配点定位 |
| 3.3 | `HoughTransform.cs` | 霍夫变换：`HoughLines` 标准直线、`HoughLinesP` 概率线段、`HoughCircles` 圆检测（含高斯模糊预处理） |
| 3.4 | `CornerDetection.cs` | 角点检测：`CornerHarris`（Harris 角点）、`GoodFeaturesToTrack`（Shi-Tomasi）、`CornerSubPix` 亚像素精度、`FAST` 关键点 |
| 3.5 | `ConnectedComponents.cs` | 连通域分析：`ConnectedComponentsWithStats`（标签图 + 统计信息 + 质心）、颗粒计数/着色/属性标注 |

### 第四章：高级应用（04_Advanced）

| 编号 | 文件名 | 知识点 |
|------|--------|--------|
| 4.1 | `FaceDetection.cs` | Haar 级联分类器：`CascadeClassifier`、`DetectMultiScale`、人脸检测 + 人眼检测、矩形绘制 |
| 4.2 | `VideoCapture.cs` | 视频/摄像头：`VideoCapture`（文件 & 摄像头）、`Read` 帧读取、`FrameProperty`、`VideoWriter` 保存视频、实时 Canny 边缘叠加、合成动画回退机制 |
| 4.3 | `BackgroundSubtraction.cs` | 背景减除：`BackgroundSubtractorMOG2`、`BackgroundSubtractorKNN`、前景掩码、形态学去噪 + 轮廓框运动目标、阴影检测 |
| 4.4 | `ImageStitching.cs` | 图像金字塔：`PyrDown`/`PyrUp` 高斯-拉普拉斯金字塔；图像拼接：`Stitcher` 全景拼接（含横向 HConcat 回退演示） |

---

## 🖼️ 测试数据

所有测试图片均已内置在 `Data/Images/` 目录下（程序自动生成，无需下载）：

| 文件 | 尺寸 | 用途 |
|------|------|------|
| `lena.png` | 512×512 | 通用测试图（类人像合成图，含丰富色彩/纹理） |
| `colors.png` | 300×500 | 红/绿/蓝/橙/粉/黄彩色方块，颜色空间提取示例 |
| `shapes.png` | 400×600 | 矩形/圆/三角/五边形/椭圆/星形，轮廓/形状分析 |
| `text.png` | 300×500 | 渐变背景+文字，阈值分割/自适应阈值 |
| `chessboard.png` | 400×500 | 7×9 棋盘格，角点检测 |
| `blobs.png` | 400×600 | 多颗粒/斑点，连通域/颗粒计数 |
| `board.png` | 400×600 | 重复方块图案，模板匹配 |
| `lines.png` | 400×500 | 多条直线和圆，霍夫变换 |
| `faces.jpg` | 400×600 | 多个人脸示意，人脸检测 |

### 可选外部数据

- **Haar 级联 XML**：请把 `haarcascade_frontalface_default.xml`（以及 `haarcascade_eye.xml`）放入 `Data/Cascade/` 目录即可启用完整人脸检测。文件可从 [OpenCV 官方仓库](https://github.com/opencv/opencv/tree/master/data/haarcascades) 下载。
- **测试视频**：将任意短视频命名为 `sample.mp4` 放入 `Data/Video/` 目录，否则示例会尝试打开摄像头或使用合成动画。

---

## 🔑 C# 2.0 语法注意事项

本示例库所有源文件严格遵循 **C# 2.0 语法规范**，便于在旧项目/旧环境中复用：

| ❌ 不使用 | ✅ 使用 C# 2.0 替代写法 |
|----------|----------------------|
| `var img = new Mat();` | `Mat img = new Mat();` |
| `lambda: (s,e) => { ... }` | 显式命名委托方法 / `CvTrackbarCallback2` 等 |
| `LINQ: list.Where(...).Select(...)` | `for/foreach` 循环 + `List<T>` |
| 自动属性 `public string Name { get; set; }` | 传统带后备字段的属性，或在抽象基类中用非自动 getter |
| 扩展方法 `img.Clone().Save(...)` | 传统调用 `Mat tmp = img.Clone(); ... tmp.Dispose();` |
| 匿名类型 `new { x=1, y=2 }` | 定义具名类/结构体或用 `Point`/`Scalar` 等 OpenCvSharp 类型 |
| 对象初始化器 `new Mat { ... }` | 先 new 再逐字段/方法赋值 |
| `using (var x = ...) { ... }` 带 var | `using (Mat x = ...) { ... }` |

### 资源管理最佳实践

OpenCvSharp 的 `Mat`、`Window`、`VideoCapture`、`CascadeClassifier` 等都实现了 `IDisposable`，示例中每个示例的 `Run()` 方法都在结尾显式 `Dispose()` 了所有创建的 `Mat` 对象；使用窗口时推荐 `using (new Window(...)) { ... }` 写法以确保资源释放。

---

## 🧩 扩展建议

你可以基于本示例库继续扩展以下方向：

1. **相机标定**：`Cv2.CalibrateCamera`、`FindChessboardCorners`、`Undistort`，结合 `FileStorage` 读写 XML/YAML；
2. **特征点匹配**：SIFT/SURF/ORB 特征检测 + BFMatcher/FlannBasedMatcher（需 OpenCvSharp4 contrib 模块）；
3. **图像分割**：GrabCut、Watershed 分水岭算法；
4. **光流**：`CalcOpticalFlowFarneback`、`CalcOpticalFlowPyrLK`；
5. **机器学习**：`Cv2.KNearest`、SVM、ANN_MLP；
6. **DNN 深度学习**：`Dnn` 模块加载 YOLO/ResNet 等预训练模型（OpenCvSharp4 支持）；
7. **WinForms/WPF 可视化**：将 `Mat` 转 `Bitmap`（`OpenCvSharp.Extensions.BitmapConverter`），在界面中嵌入 PictureBox 实时显示。

---

## ❓ 常见问题

**Q1: 运行时提示「无法加载 DLL『OpenCvSharpExtern』」？**  
A: 请确保已安装 `OpenCvSharp4.runtime.win` NuGet 包；或手动将原生 DLL 拷贝到输出目录。

**Q2: Cv2.ImShow 弹不出窗口？**  
A: 请确认程序是控制台/Windows 桌面应用（非无界面服务）；同时需要调用 `Cv2.WaitKey()` 消息泵，窗口才能正常响应。

**Q3: 编译提示找不到 OpenCvSharp 命名空间？**  
A: 通过 NuGet 安装 OpenCvSharp4 即可；若使用 csproj 中默认的 `Lib\OpenCvSharp.dll` 引用，请按 Lib/README.md 说明放入对应 DLL。

**Q4: 人脸检测示例提示找不到 XML？**  
A: 请按 `Data/Cascade/README.md` 下载 haarcascade_frontalface_default.xml；未放置时示例会显示提示并不会崩溃。

**Q5: 在 Linux/Mac 上能运行吗？**  
A: 可以。C# 代码本身与平台无关，只需安装对应平台的 OpenCvSharp4 runtime 包即可（参见「环境要求」）。

---

## 📜 许可说明

本示例库代码基于学习目的公开，可自由使用、修改、分发；OpenCvSharp 本体遵循 BSD 3-Clause License（见其官方仓库）。测试图片均由程序合成生成，无版权限制。

---

## 🔗 参考资源

- OpenCvSharp 官方仓库：<https://github.com/shimat/opencvsharp>
- OpenCV 官方文档：<https://docs.opencv.org/>
- OpenCvSharp Wiki & 样例：<https://github.com/shimat/opencvsharp/tree/master/test>
- OpenCV 教程（中文）：<https://www.woshicver.com/>

---

祝学习愉快！🎉
