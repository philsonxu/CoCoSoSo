# Haar 级联分类器目录

本目录用于存放 OpenCV 官方提供的 Haar/LBP 级联分类器 XML 文件，人脸/人眼检测示例需要这些文件。

## 需要放入的文件

请将 OpenCV 安装目录下 `data/haarcascades/` 中的以下 XML 文件复制到本目录：

| 文件名 | 用途 |
|--------|------|
| `haarcascade_frontalface_default.xml` | 正面人脸检测（必需） |
| `haarcascade_eye.xml` | 眼睛检测（可选） |
| `haarcascade_smile.xml` | 微笑检测（可选） |
| `haarcascade_fullbody.xml` | 人体检测（可选） |
| `haarcascade_upperbody.xml` | 上半身检测（可选） |

## 获取方式

1. 从 OpenCV 官方仓库下载：https://github.com/opencv/opencv/tree/master/data/haarcascades
2. 或者从已安装 OpenCvSharp NuGet 包的目录中查找（通常在输出目录或 NuGet 缓存中）
3. 如果使用 OpenCvSharp4 + OpenCvSharp4.runtime.win，这些 XML 通常不会被自动包含，需要手动下载

## 注意

- 如果本目录缺少 `haarcascade_frontalface_default.xml`，人脸检测示例会显示提示信息并使用合成示意图，不会影响其他示例运行。
- 推荐至少放置人脸检测分类器以体验完整示例。
