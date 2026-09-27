using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CSharpNetworkProgramming.Chapter02_Tcp
{
    /// <summary>
    /// 2.5 TCP 文件传输（服务器 ← 客户端上传 + 服务器 → 客户端下载）
    /// 知识点：自定义协议（命令+长度头+正文）、文件分块读写、FileStream、BinaryReader/BinaryWriter
    ///         协议格式：[1字节命令][4字节长度][N字节数据]
    /// </summary>
    public class _05_TcpFileTransfer : ISample
    {
        public string Id { get { return "2.5"; } }
        public string Title { get { return "TCP 文件传输（上传/下载）"; } }
        public string Description { get { return "演示基于TCP的文件上传与下载：自定义命令长度头协议，服务器接收文件保存到临时目录，客户端可上传TestData中的文件。"; } }

        public const int Port = 9205;
        private string saveDir;

        public void Run()
        {
            saveDir = Path.Combine(Path.GetTempPath(), "TcpFileTransferDemo");
            Directory.CreateDirectory(saveDir);
            Console.WriteLine("服务器接收目录: {0}", saveDir);

            Thread srv = new Thread(new ThreadStart(RunServer));
            srv.IsBackground = true;
            srv.Start();
            Thread.Sleep(300);

            RunClient();

            Thread.Sleep(500);
            Console.WriteLine("文件传输演示结束。");
        }

        private void RunServer()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, Port);
            listener.Start();
            Console.WriteLine("[Server] 文件服务器监听 {0}", Port);

            using (TcpClient client = listener.AcceptTcpClient())
            using (NetworkStream ns = client.GetStream())
            {
                BinaryReader br = new BinaryReader(ns);
                BinaryWriter bw = new BinaryWriter(ns);

                while (true)
                {
                    byte cmdByte;
                    try { cmdByte = br.ReadByte(); }
                    catch { break; } // 客户端断开

                    int len = br.ReadInt32();
                    byte[] payload = br.ReadBytes(len);
                    string cmd = Encoding.UTF8.GetString(payload);
                    Console.WriteLine("[Server] 收到命令: {0}", cmd);

                    if (cmd.ToLower() == "bye") break;

                    if (cmd.ToUpper().StartsWith("UPLOAD "))
                    {
                        string fileName = cmd.Substring(7);
                        long fileLen = br.ReadInt64();
                        string savePath = Path.Combine(saveDir, Path.GetFileName(fileName));
                        Console.WriteLine("[Server] 开始接收 {0}，大小 {1} 字节", fileName, fileLen);
                        using (FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                        {
                            byte[] buf = new byte[8192];
                            long remaining = fileLen;
                            while (remaining > 0)
                            {
                                int toRead = (int)Math.Min(buf.Length, remaining);
                                int n = ns.Read(buf, 0, toRead);
                                if (n <= 0) break;
                                fs.Write(buf, 0, n);
                                remaining -= n;
                            }
                        }
                        string resp = "OK " + savePath;
                        bw.Write((byte)1);
                        byte[] rb = Encoding.UTF8.GetBytes(resp);
                        bw.Write(rb.Length);
                        bw.Write(rb);
                        Console.WriteLine("[Server] 文件已保存到 {0}", savePath);
                    }
                    else if (cmd.ToUpper().StartsWith("DOWNLOAD "))
                    {
                        string fileName = cmd.Substring(9);
                        string fullPath = Path.Combine(saveDir, Path.GetFileName(fileName));
                        if (File.Exists(fullPath))
                        {
                            FileInfo fi = new FileInfo(fullPath);
                            bw.Write((byte)1);
                            string hdr = "FILE " + fi.Name;
                            byte[] hb = Encoding.UTF8.GetBytes(hdr);
                            bw.Write(hb.Length);
                            bw.Write(hb);
                            bw.Write(fi.Length);
                            using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                            {
                                byte[] buf = new byte[8192];
                                int n;
                                while ((n = fs.Read(buf, 0, buf.Length)) > 0)
                                {
                                    ns.Write(buf, 0, n);
                                }
                            }
                            Console.WriteLine("[Server] 文件 {0} 已发送", fi.Name);
                        }
                        else
                        {
                            string err = "ERR file not found";
                            bw.Write((byte)2);
                            byte[] eb = Encoding.UTF8.GetBytes(err);
                            bw.Write(eb.Length);
                            bw.Write(eb);
                        }
                    }
                }
            }
            listener.Stop();
            Console.WriteLine("[Server] 已停止");
        }

        private void RunClient()
        {
            string srcFile = TestData.GetPath("test.txt");
            if (!File.Exists(srcFile))
            {
                Console.WriteLine("[Client] 测试文件不存在: {0}", srcFile);
                // 创建一个测试文件
                srcFile = Path.Combine(saveDir, "demo_created.txt");
                File.WriteAllText(srcFile, "这是由演示程序自动生成的测试文件内容。\nHello TCP File Transfer!\n");
            }
            else
            {
                // 复制一份到临时目录作为上传源
                string tmp = Path.Combine(saveDir, Path.GetFileName(srcFile));
                File.Copy(srcFile, tmp, true);
                srcFile = tmp;
            }

            using (TcpClient client = new TcpClient("127.0.0.1", Port))
            using (NetworkStream ns = client.GetStream())
            {
                BinaryReader br = new BinaryReader(ns);
                BinaryWriter bw = new BinaryWriter(ns);

                // --- 上传文件 ---
                Console.WriteLine("[Client] 准备上传: {0}", srcFile);
                string upCmd = "UPLOAD " + Path.GetFileName(srcFile);
                byte[] upBytes = Encoding.UTF8.GetBytes(upCmd);
                bw.Write((byte)0x01);
                bw.Write(upBytes.Length);
                bw.Write(upBytes);
                FileInfo fi = new FileInfo(srcFile);
                bw.Write(fi.Length);
                using (FileStream fs = new FileStream(srcFile, FileMode.Open, FileAccess.Read))
                {
                    byte[] buf = new byte[8192];
                    int n;
                    while ((n = fs.Read(buf, 0, buf.Length)) > 0)
                    {
                        ns.Write(buf, 0, n);
                    }
                }
                // 读响应
                byte code = br.ReadByte();
                int rLen = br.ReadInt32();
                string resp = Encoding.UTF8.GetString(br.ReadBytes(rLen));
                Console.WriteLine("[Client] 上传响应({0}): {1}", code, resp);

                // --- 下载同一个文件 ---
                string dnCmd = "DOWNLOAD " + Path.GetFileName(srcFile);
                byte[] dnBytes = Encoding.UTF8.GetBytes(dnCmd);
                bw.Write((byte)0x02);
                bw.Write(dnBytes.Length);
                bw.Write(dnBytes);

                code = br.ReadByte();
                int hLen = br.ReadInt32();
                string hdr = Encoding.UTF8.GetString(br.ReadBytes(hLen));
                Console.WriteLine("[Client] 下载响应: {0}", hdr);
                if (hdr.StartsWith("FILE "))
                {
                    long fLen = br.ReadInt64();
                    string dlPath = Path.Combine(saveDir, "downloaded_" + Path.GetFileName(srcFile));
                    using (FileStream fs = new FileStream(dlPath, FileMode.Create, FileAccess.Write))
                    {
                        byte[] buf = new byte[8192];
                        long remaining = fLen;
                        while (remaining > 0)
                        {
                            int toRead = (int)Math.Min(buf.Length, remaining);
                            int n = ns.Read(buf, 0, toRead);
                            if (n <= 0) break;
                            fs.Write(buf, 0, n);
                            remaining -= n;
                        }
                    }
                    Console.WriteLine("[Client] 文件已下载到 {0}", dlPath);
                    Console.WriteLine("[Client] 大小验证：原始 {0}，下载 {1}", fi.Length, new FileInfo(dlPath).Length);
                }

                // bye
                byte[] byeBytes = Encoding.UTF8.GetBytes("bye");
                bw.Write((byte)0xFF);
                bw.Write(byeBytes.Length);
                bw.Write(byeBytes);
            }
        }
    }
}
