# C# 网络编程示例库

> 基于 **.NET Framework 2.0 / C# 2.0** 语法编写的网络编程学习示例集，涵盖 Socket/TCP/UDP/HTTP/Mail/FTP/异步/并发/工具类 等核心知识点，包含可直接编译运行的完整源码与测试数据。

---

## 📋 目录

- [项目概述](#项目概述)
- [环境要求](#环境要求)
- [编译运行](#编译运行)
- [目录结构](#目录结构)
- [示例清单](#示例清单)
- [测试数据](#测试数据)
- [C# 2.0 语法要点](#c-20-语法要点)
- [知识点速查](#知识点速查)
- [扩展建议](#扩展建议)
- [常见问题](#常见问题)

---

## 📖 项目概述

本示例库包含 **28 个可独立运行的示例程序**，分为 **9 大章节**，覆盖 C# 网络编程从入门到进阶的主要知识点：

- 从 DNS 解析、IPEndPoint 等基础概念开始；
- 深入 TCP 同步/异步服务器与客户端、多线程并发、聊天室、文件传输；
- 覆盖 UDP 无连接通信、广播、停等可靠传输；
- 演示 HTTP 协议级实现（GET/POST/下载/简易Web服务器）；
- 包含 SMTP/POP3 邮件收发、FTP 客户端操作；
- 系统讲解 APM（Begin/End）异步编程模型；
- 提供 Web代理、端口扫描、Ping 等实用工具；
- 以及 URL/HTML编解码、二进制序列化等工具类。

所有示例通过统一菜单驱动，运行后在控制台选择编号即可执行。

---

## 🛠 环境要求

| 项目 | 要求 |
|------|------|
| 操作系统 | Windows 7/10/11 或 Windows Server |
| 运行时 | .NET Framework **2.0** 或更高版本（3.5/4.x 均可向后兼容） |
| 编译器 | Visual Studio 2008/2010/2012/2013/2015/2017/2019/2022（任何支持C# 2.0以上版本的VS均可） |
| 引用 | System、System.Data、System.Web、System.Xml（均为 .NET Framework 自带） |

> **注意**：C# 2.0 语法代码可在更高版本 .NET Framework/.NET 上直接运行，也可由新版 Visual Studio 自动迁移升级。

---

## 🚀 编译运行

### 方法1：使用 Visual Studio

1. 打开 `CSharpNetworkProgramming.sln`；
2. 确认项目引用中包含 `System`、`System.Web`（用于 HttpUtility）；
3. 按 `Ctrl+F5`（开始执行不调试）；
4. 在控制台输入示例编号（如 `1.1`、`3.1`）即可运行对应示例；
   - 输入 `0` 依次运行全部示例；
   - 输入 `q` 退出程序。

### 方法2：命令行编译（csc.exe）

```cmd
:: 进入项目目录
cd CSharpNetworkProgramming

:: 编译（使用 .NET Framework 2.0 csc）
C:\Windows\Microsoft.NET\Framework\v2.0.50727\csc.exe
    /out:CSharpNetworkProgramming.exe
    /target:exe
    /reference:System.dll
    /reference:System.Web.dll
    /recurse:*.cs
    Properties\AssemblyInfo.cs
```

### 方法3：命令行参数运行单个示例

```cmd
:: 直接运行指定编号示例，跳过菜单
CSharpNetworkProgramming.exe 2.1
CSharpNetworkProgramming.exe 4.4
```

---

## 📂 目录结构

```
CSharpNetworkProgramming/
├── CSharpNetworkProgramming.sln         # Visual Studio 解决方案文件
├── CSharpNetworkProgramming.csproj      # C# 项目文件(.NET 2.0)
├── README.md                            # 本说明文档
├── Program.cs                           # 主程序入口(菜单调度)
├── Properties/
│   └── AssemblyInfo.cs                  # 程序集信息
├── Chapter01_Basics/                    # 第一章 网络编程基础
│   ├── 01_DnsResolve.cs                 # 1.1 DNS 域名解析
│   ├── 02_IPEndPoint.cs                 # 1.2 IPEndPoint 端点
│   └── 03_NetworkStream.cs              # 1.3 NetworkStream 基础流
├── Chapter02_Tcp/                       # 第二章 TCP 编程
│   ├── 01_TcpEchoServer.cs              # 2.1 TCP Echo 同步服务器
│   ├── 02_TcpEchoClient.cs              # 2.2 TCP Echo 客户端
│   ├── 03_TcpChatServer.cs              # 2.3 TCP 多线程群聊服务器
│   ├── 04_TcpChatClient.cs              # 2.4 TCP 群聊客户端
│   ├── 05_TcpFileTransfer.cs            # 2.5 TCP 文件上传/下载
│   └── 06_TcpMultiThreadServer.cs       # 2.6 ThreadPool 并发服务器+压测
├── Chapter03_Udp/                       # 第三章 UDP 编程
│   ├── 01_UdpEcho.cs                    # 3.1 UDP Echo 无连接收发
│   ├── 02_UdpChat.cs                    # 3.2 UDP 无连接群聊
│   ├── 03_UdpFileTransfer.cs            # 3.3 UDP 停等协议文件传输
│   └── 04_UdpBroadcast.cs               # 3.4 UDP 广播
├── Chapter04_Http/                      # 第四章 HTTP 编程
│   ├── 01_HttpGet.cs                    # 4.1 HTTP GET 请求
│   ├── 02_HttpPost.cs                   # 4.2 HTTP POST 表单提交
│   ├── 03_HttpDownload.cs               # 4.3 HTTP 文件下载(进度/断点)
│   └── 04_SimpleHttpServer.cs           # 4.4 简易 HTTP 服务器(协议级实现)
├── Chapter05_Mail/                      # 第五章 邮件编程
│   ├── 01_SmtpSendMail.cs               # 5.1 SMTP 发送邮件(HTML+附件)
│   └── 02_Pop3ReceiveMail.cs            # 5.2 POP3 收邮件(协议级演示)
├── Chapter06_Ftp/                       # 第六章 FTP 编程
│   └── 01_FtpClient.cs                  # 6.1 FTP 客户端(FtpWebRequest)
├── Chapter07_Async/                     # 第七章 异步编程
│   ├── 01_AsyncTcpServer.cs             # 7.1 APM 异步 TCP 服务器
│   ├── 02_AsyncTcpClient.cs             # 7.2 APM 异步 TCP 客户端
│   └── 03_AsyncUdp.cs                   # 7.3 APM 异步 UDP
├── Chapter08_Advanced/                  # 第八章 高级主题
│   ├── 01_WebProxy.cs                   # 8.1 Web 代理设置
│   ├── 02_PortScanner.cs                # 8.2 端口扫描器
│   └── 03_Ping.cs                       # 8.3 Ping (ICMP)
├── Chapter09_Utility/                   # 第九章 工具类
│   ├── 01_UrlEncoding.cs                # 9.1 URL/HTML 编解码
│   └── 02_Serialization.cs             # 9.2 二进制序列化
└── TestData/                            # 测试数据
    ├── test.txt                         # 文本测试文件(中英文混排)
    ├── test.bin                         # 二进制测试文件(10KB)
    └── index.html                       # HTTP 测试用静态网页
```

---

## 📚 示例清单

### 第一章 · 网络编程基础（3 个）

| 编号 | 名称 | 核心知识点 |
|------|------|-----------|
| **1.1** | DNS 域名解析 | `Dns.GetHostName`/`GetHostEntry`/`IPHostEntry`、`IPAddress` 常用属性与方法（Loopback/Broadcast/Any/Parse/TryParse/GetAddressBytes）、IPv6 地址族判断 |
| **1.2** | IPEndPoint 网络端点 | `IPEndPoint` 构造与属性、`MinPort/MaxPort` 端口范围、`Serialize`/`Create` 端点序列化、常用端口对照表、手动解析 `IP:Port` 字符串 |
| **1.3** | NetworkStream 网络流 | `TcpListener`/`TcpClient` 建立连接、`NetworkStream.Read/Write` 字节数组读写、`ReadByte/WriteByte` 单字节读写、`StreamReader/StreamWriter` 包装、TCP流式特点 |

### 第二章 · TCP 编程（6 个）

| 编号 | 名称 | 核心知识点 |
|------|------|-----------|
| **2.1** | TCP Echo 同步服务器 | `TcpListener.Start/AcceptTcpClient/Stop`、同步阻塞Accept、行协议（`\n`分隔）、回声逻辑、单客户端一次连接模型 |
| **2.2** | TCP Echo 客户端 | `TcpClient.Connect`、`NetworkStream` 读写、**双线程收发模型**（收/发分离）、控制台交互输入、exit/quit命令 |
| **2.3** | TCP 多线程群聊服务器 | `Thread` 每客户端一线程、`ArrayList` + `lock(SyncRoot)` 线程安全客户端列表、消息**广播(Broadcast)**、昵称管理、加入/离开通知 |
| **2.4** | TCP 群聊客户端 | 配合2.3使用、发送/接收双线程、`name:xxx` 改名命令、`quit` 退出命令 |
| **2.5** | TCP 文件传输 | **自定义协议**：`[1字节命令][4字节长度][N字节数据]`；`BinaryReader/Writer`；文件分块（8KB）上传/下载；客户端/服务器双向流；上传后下载验证一致性 |
| **2.6** | ThreadPool 并发服务器 | `ThreadPool.QueueUserWorkItem`、`NoDelay/ReceiveTimeout/SendTimeout`、`Interlocked` 原子计数、20并发×5请求内置压测、time/count/echo 多命令路由 |

### 第三章 · UDP 编程（4 个）

| 编号 | 名称 | 核心知识点 |
|------|------|-----------|
| **3.1** | UDP Echo | `UdpClient` 构造（绑定端口/随机端口）、`Receive(ref IPEndPoint)` 与 `Send(byte[],len,ip:port)`、UDP 无连接特性、消息边界保留、自启动服务器-客户端 |
| **3.2** | UDP 无连接群聊 | 服务器维护 `Hashtable` 客户端 EndPoint 列表、消息循环广播、"心跳"过期清理、多客户端模拟、无连接模型下客户端管理 |
| **3.3** | UDP 可靠文件传输 | 在 UDP 之上实现**停等协议**：数据分包（512B/块）、序号（seq）、ACK确认、超时重传（1s超时，最多5次）、开始/数据/结束帧 |
| **3.4** | UDP 广播 | `UdpClient.EnableBroadcast=true`、`IPAddress.Broadcast(255.255.255.255)`、`ReuseAddress`/`ExclusiveAddressUse` 端口复用、多监听者、定向广播原理 |

### 第四章 · HTTP 编程（4 个）

| 编号 | 名称 | 核心知识点 |
|------|------|-----------|
| **4.1** | HTTP GET | `HttpWebRequest`/`HttpWebResponse`、`Method="GET"`、`UserAgent/Accept` 头、`GetResponseStream` 读取、`StatusCode/Headers/Server/ContentType` 读取 |
| **4.2** | HTTP POST | `Method="POST"`、`ContentType="application/x-www-form-urlencoded"`、`GetRequestStream()` 写入请求体、`HttpUtility.UrlEncode` 表单参数编码、表单解析回显 |
| **4.3** | HTTP 文件下载 | 分块（4KB）下载、下载进度/速度统计、`AddRange()` Range 头**断点续传**、`FileMode.Append` 追加写入、206 Partial Content 处理 |
| **4.4** | 简易 HTTP 服务器 | 基于 `TcpListener` 从零实现 HTTP/1.0；解析请求行/请求头/空行；状态行+响应头+响应体；支持 GET/POST；路由：`/` 欢迎页、`/index` 静态页、`/file` 二进制下载、`/post` 回显表单；404响应 |

### 第五章 · 邮件编程（2 个）

| 编号 | 名称 | 核心知识点 |
|------|------|-----------|
| **5.1** | SMTP 发送邮件 | `SmtpClient`/`MailMessage`、`MailAddress`（From/To/CC/Bcc）、`AlternateView` 纯文本+HTML双视图、`Attachment` 附件（文件/内存流）、`Priority`优先级、`DeliveryNotificationOptions`、SMTP服务器配置（EnableSsl/端口/认证）|
| **5.2** | POP3 收邮件 | 基于 `TcpClient`+`NetworkStream` 手动实现POP3协议、命令脚本演示：USER/PASS/STAT/LIST/RETR/DELE/QUIT、`+OK/-ERR` 响应、邮件头编码（Base64/QuotedPrintable）、POP3命令速查与代码模板 |

### 第六章 · FTP 编程（1 个）

| 编号 | 名称 | 核心知识点 |
|------|------|-----------|
| **6.1** | FTP 客户端 | `FtpWebRequest`/`FtpWebResponse`、`WebRequestMethods.Ftp.*`方法枚举（ListDirectory/DownloadFile/UploadFile/DeleteFile/MakeDirectory/RemoveDirectory/Rename等）、`NetworkCredential`、`UseBinary/UsePassive/KeepAlive`、匿名FTP连通性测试、上传/下载代码模板 |

### 第七章 · 异步编程（3 个）

| 编号 | 名称 | 核心知识点 |
|------|------|-----------|
| **7.1** | APM 异步 TCP 服务器 | **APM 异步编程模型**：`BeginAcceptTcpClient`→AcceptCallback→`BeginRead`→ReadCallback→`BeginWrite`→WriteCallback 回调链、AsyncState 对象封装状态、无阻塞线程、多客户端异步Accept |
| **7.2** | APM 异步 TCP 客户端 | `TcpClient.BeginConnect/EndConnect`、`NetworkStream.BeginRead/BeginWrite`、`ManualResetEvent` 同步主线程等待异步完成、回调链状态传递 |
| **7.3** | APM 异步 UDP | `UdpClient.BeginReceive/EndReceive`、`BeginSend/EndSend`、异步回调+状态对象、异步收发全流程 |

### 第八章 · 高级主题（3 个）

| 编号 | 名称 | 核心知识点 |
|------|------|-----------|
| **8.1** | Web 代理设置 | `WebProxy`、`WebRequest.DefaultWebProxy` 系统默认代理、`Proxy=null` 直连、`BypassProxyOnLocal`、`BypassList` 绕过列表、代理 `Credentials` 认证、自动代理检测 |
| **8.2** | 端口扫描器 | `TcpClient.BeginConnect`+`WaitOne(超时)` 端口扫描、`ThreadPool` 并发扫描、端口开放/关闭判断、常用端口服务名对照、扫描结果排序汇总（仅扫描本机 127.0.0.1）|
| **8.3** | Ping (ICMP) | `System.Net.NetworkInformation.Ping`、`PingOptions`(TTL/DontFragment)、`PingReply`(Status/RoundtripTime/Address/Options/Buffer)、连续Ping统计（发送/接收/丢失率/最小/最大/平均RTT）|

### 第九章 · 工具类（2 个）

| 编号 | 名称 | 核心知识点 |
|------|------|-----------|
| **9.1** | URL/HTML 编解码 | `HttpUtility.UrlEncode/UrlDecode`、`UrlPathEncode`（路径编码）、`HtmlEncode/HtmlDecode`（XSS防护）、`ParseQueryString` 解析查询字符串、UTF-8 vs GB2312编码差异 |
| **9.2** | 二进制序列化 | `[Serializable]` 特性、`BinaryFormatter.Serialize/Deserialize`、`MemoryStream` 中转、自定义 `NetMessage` 网络消息对象设计、**4字节长度头+消息体**的网络帧协议模拟、Hashtable承载Headers |

---

## 📁 测试数据

| 文件 | 大小 | 说明 |
|------|------|------|
| `TestData/test.txt` | ~500B | 中英文混排文本文件，用于TCP/UDP/HTTP/FTP文件传输示例测试 |
| `TestData/test.bin` | 10 KB | 带4字节文件头(`CST1`+版本+大小)的二进制模式数据，用于二进制文件传输、序列化测试 |
| `TestData/index.html` | ~2.4 KB | 含CSS样式/表单/链接的静态HTML页面，用于4.4简易HTTP服务器的 `/index` 路由演示 |

测试数据可通过 `TestData.GetPath("filename")` 方法自动定位，兼容从 `bin/Debug/` 运行和从项目根目录运行两种场景。

> ✅ **纯 C# 2.0 自动生成**：所有测试数据均由 `TestDataGenerator.cs` 在程序启动时通过纯 C# 2.0 代码自动生成（文件不存在才会创建），**不依赖任何外部脚本**（无 Python、无 PowerShell、无 Bat、无外部命令），项目完全自包含，克隆到本地即可直接编译运行。

---

## ⚙ C# 2.0 语法要点

本示例库**严格遵守 C# 2.0 语法**，所有代码均可在 Visual Studio 2008（.NET 2.0/3.5）下直接编译通过，未使用任何 C# 3.0+ 新特性：

| 特性 | 本项目处理 |
|------|-----------|
| `var` 隐式类型 | ❌ 不使用，全部显式声明类型 `TcpClient client = new TcpClient();` |
| `Lambda` 表达式（`=>`） | ❌ 不使用，使用显式 `new ThreadStart(Method)` / `new WaitCallback(Method)` / 匿名方法 |
| 匿名类型 / 对象初始化器 | ❌ 不使用 |
| LINQ / 扩展方法 | ❌ 不使用，集合操作全部用传统 `foreach`/`for` |
| 自动属性 `{ get; set; }` | ❌ 不使用，全部使用公共字段或传统属性 |
| `async/await` (C#5) | ❌ 不使用，异步采用 **APM（Begin/End + 回调）** 模式 |
| 泛型集合（C#2.0 可用） | ✅ 使用 `List<T>`/`SortedList<K,V>`/`Dictionary<K,V>` |
| 匿名方法（C#2.0 可用） | ✅ 部分Thread/ThreadPool回调使用 `delegate() { ... }` |
| 可空类型、迭代器（C#2.0） | ✅ 可用 |

---

## 🎯 知识点速查

### 网络编程核心类速查

```
System.Net
├── Dns                          域名解析
├── IPAddress                    IP 地址
├── IPEndPoint                   IP+端口端点
├── EndPoint                     端点抽象基类
├── WebRequest / WebResponse     Web 请求/响应抽象类
├── HttpWebRequest/HttpWebResponse  HTTP 专用
├── FtpWebRequest/FtpWebResponse    FTP 专用
├── WebProxy / IWebProxy         Web 代理
├── NetworkCredential            网络凭据
├── Socket                       Socket 底层类
├── System.Net.Mail              邮件 (SmtpClient/MailMessage)
├── System.Net.Sockets           Socket/TcpClient/TcpListener/UdpClient/NetworkStream
├── System.Net.NetworkInformation 网络信息 (Ping/IPGlobalProperties/NetworkInterface)
└── System.Web.HttpUtility       Url/Html 编解码

System.Runtime.Serialization.Formatters.Binary
└── BinaryFormatter              二进制序列化
```

### 端口使用列表（本项目）

| 端口 | 用途 | 示例 |
|------|------|------|
| 9100 | NetworkStream基础演示 | 1.3 |
| 9201 | TCP Echo 服务器 | 2.1/2.2 |
| 9203 | TCP 群聊服务器 | 2.3/2.4 |
| 9205 | TCP 文件传输 | 2.5 |
| 9206 | TCP 并发服务器 | 2.6 |
| 9301 | UDP Echo | 3.1 |
| 9302 | UDP 群聊 | 3.2 |
| 9303 | UDP 文件传输 | 3.3 |
| 9304 | UDP 广播 | 3.4 |
| 9404 | 简易 HTTP 服务器 | 4.1-4.4 |
| 9701 | APM 异步TCP服务器 | 7.1/7.2 |
| 9703 | APM 异步UDP | 7.3 |

> 所有端口均选用 9xxx 段（非特权端口），无需管理员权限即可绑定。

---

## 🔧 扩展建议

学完本示例库后，建议在以下方向继续深入：

1. **Socket 底层编程**：从 `TcpClient/UdpClient` 进一步学习 `Socket` 类，完成更底层的 Socket 通信；
2. **IOCP 高性能服务器**：学习 `SocketAsyncEventArgs`（.NET 3.5+）实现 IOCP 模型，支持数千并发；
3. **WebSocket**：基于 HTTP Upgrade 握手实现 WebSocket 协议，构建实时 Web 应用；
4. **协议序列化演进**：从 BinaryFormatter 升级到 ProtoBuf、JSON、MessagePack 等跨平台方案；
5. **安全通信**：学习 `SslStream` 实现 TLS/SSL 加密传输；
6. **多协议服务器**：尝试将 HTTP/SMTP/POP3 等协议集成到同一服务器；
7. **WCF / WebAPI**：在 .NET 3.0+ 上学习 WCF、ASP.NET Web API 等高层框架；
8. **跨平台 .NET**：迁移到 .NET Core/.NET 5+，体验跨平台网络编程。

---

## ❓ 常见问题

### Q1：编译提示找不到 `System.Web.HttpUtility`？
在项目引用中**添加 `System.Web`** 程序集引用即可（`右键项目→添加引用→程序集→框架→System.Web`）。

### Q2：运行邮件/HTTP/POP3示例时需要真实账号吗？
- SMTP/POP3 (5.1/5.2)：5.1 仅构造 MailMessage 并打印，不真实发送；5.2 以脚本方式演示POP3协议流程，均**无需真实账号**。
- HTTP (4.1-4.3)：可先运行 4.4 启动本地 HTTP 服务器（15秒），在这15秒内运行 4.1-4.3 即可得到真实响应；若服务器未启动，示例会自动回退到本地文件演示。
- FTP (6.1)：会尝试连接 `ftp.gnu.org` 公共FTP服务器，如果网络不通会自动打印代码模板并模拟上传/下载。
- Ping (8.3)：会真实 Ping 127.0.0.1/localhost/baidu.com，需要网络支持ICMP。

### Q3：端口被占用怎么办？
修改对应示例文件中的 `Port` 常量即可（如 `_01_TcpEchoServer.Port = 9201`）。由于所有端口都在9xxx段，正常情况下不会与常用服务冲突。

### Q4：能否多终端启动客户端？
可以！2.3 群聊服务器、7.1 异步服务器支持多客户端。可在一个终端运行服务器（如2.3），另起多个终端运行客户端（2.4）即可体验多人聊天。

### Q5：UDP文件传输(3.3)为什么用512字节的小包？
教学演示用。小包便于观察序号、ACK、重传等逻辑；实际应用中UDP包大小建议控制在 1400 字节左右（考虑MTU 1500 - IP头20 - UDP头8 = 1472）。

### Q6：为什么需要对 `ArrayList.SyncRoot` 加锁？
`ArrayList.Synchronized()` 返回的是线程安全包装，但在**遍历(Foreach)**时仍需手动加锁，否则在遍历过程中其他线程修改集合会抛异常。这是 .NET 1.x/2.0 集合的经典多线程编程注意事项。

---

## 📜 许可证

本示例库仅供学习交流使用，测试数据均为程序合成，无第三方版权限制。

---

> 💡 **学习建议**：建议按章节顺序从 1.1 开始逐个运行、阅读源码，理解每个示例的"运行效果→源码结构→关键API→底层原理"四个层次。遇到不熟悉的 API 可在代码中设置断点，单步调试观察数据流动。
