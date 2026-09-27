using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

using Newtonsoft.Json;

namespace CSharpNetworkProgramming.Chapter09_Utility
{
    /// <summary>
    /// 9.2 二进制序列化 (用于网络消息传输)
    /// 知识点：[Serializable]、BinaryFormatter、MemoryStream序列化/反序列化、
    ///         序列化版本兼容、网络消息对象设计（消息头+消息体）、自定义消息协议
    /// </summary>
    public class _02_Serialization : ISample
    {
        public string Id { get { return "9.2"; } }
        public string Title { get { return "二进制序列化(网络消息对象)"; } }
        public string Description { get { return "演示[Serializable]+BinaryFormatter将消息对象序列化为字节数组，便于在TCP/UDP网络上传输；演示自定义消息头+消息体的协议设计思路。"; } }

        public void Run()
        {
            // 1) 构造一个消息对象
            NetMessage msg = new NetMessage();
            msg.MsgType = 1; // 1=文本消息
            msg.MsgId = Guid.NewGuid();
            msg.From = "Alice";
            msg.To = "Bob";
            msg.Timestamp = DateTime.Now;
            msg.Body = Encoding.UTF8.GetBytes("你好，这是一条通过BinaryFormatter序列化的网络消息！");
            msg.Headers["Content-Type"] = "text/plain; charset=utf-8";
            msg.Headers["Priority"] = "Normal";
            Console.WriteLine("[1] 原始消息对象:");
            PrintMessage(msg);

            // 2) 序列化为字节数组
            byte[] bytes = Serialize(msg);
            Console.WriteLine("[2] 序列化后字节数: {0}", bytes.Length);
            Console.WriteLine("    前32字节(HEX): {0}", BitConverter.ToString(bytes, 0, Math.Min(32, bytes.Length)));

            // 3) 反序列化
            NetMessage msg2 = Deserialize(bytes);
            Console.WriteLine("[3] 反序列化后消息对象:");
            PrintMessage(msg2);

            // 4) 验证
            Console.WriteLine("[4] 一致性验证:");
            Console.WriteLine("    MsgId 一致: {0}", msg.MsgId == msg2.MsgId);
            Console.WriteLine("    From  一致: {0}", msg.From == msg2.From);
            Console.WriteLine("    Body  一致: {0}", Encoding.UTF8.GetString(msg.Body) == Encoding.UTF8.GetString(msg2.Body));
            Console.WriteLine("    Headers数:  {0} vs {1}", msg.Headers.Count, msg2.Headers.Count);

            // 5) 不同消息类型演示
            Console.WriteLine();
            Console.WriteLine("[5] 文件消息类型演示:");
            NetMessage fileMsg = new NetMessage();
            fileMsg.MsgType = 2; // 2=文件
            fileMsg.MsgId = Guid.NewGuid();
            fileMsg.From = "Server";
            fileMsg.To = "Client";
            fileMsg.Timestamp = DateTime.Now;
            fileMsg.Body = new byte[] { 0x00, 0x01, 0x02, 0x03, 0xFF };
            fileMsg.Headers["FileName"] = "test.bin";
            fileMsg.Headers["FileSize"] = "5";
            byte[] fb = Serialize(fileMsg);
            NetMessage fileMsg2 = Deserialize(fb);
            Console.WriteLine("    序列化/反序列化完成，Body长度 = {0}", fileMsg2.Body.Length);

            // 6) 网络发送/接收流程演示（MemoryStream模拟）
            Console.WriteLine();
            Console.WriteLine("[6] 模拟网络传输(MemoryStream):");
            using (MemoryStream ms = new MemoryStream())
            {
                // 发送方：先写4字节长度，再写序列化内容
                byte[] payload = Serialize(msg);
                byte[] lenBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(payload.Length));
                ms.Write(lenBytes, 0, 4);
                ms.Write(payload, 0, payload.Length);
                Console.WriteLine("    发送方：写入长度头 {0} + 消息体 {1} 字节，共 {2} 字节",
                    4, payload.Length, ms.Length);

                // 接收方：先读4字节长度，再读消息体
                ms.Position = 0;
                byte[] rLen = new byte[4];
                ms.Read(rLen, 0, 4);
                int bodyLen = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(rLen, 0));
                byte[] rBody = new byte[bodyLen];
                int read = 0;
                while (read < bodyLen) read += ms.Read(rBody, read, bodyLen - read);
                NetMessage rMsg = Deserialize(rBody);
                Console.WriteLine("    接收方：长度={0}, 反序列化后消息: {1} -> {2}, Body={3}",
                    bodyLen, rMsg.From, rMsg.To, Encoding.UTF8.GetString(rMsg.Body));
            }

            Console.WriteLine();
            Console.WriteLine("提示：.NET 2.0下BinaryFormatter是最便捷的二进制序列化方案；");
            Console.WriteLine("如果需要跨平台/跨语言，建议自定义Trivia-Length-Value(TLV)协议或使用XML/JSON序列化。");
        }

        private byte[] Serialize(NetMessage msg)
        {
#if __OLD__
            BinaryFormatter fmt = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                fmt.Serialize(ms, msg);
                return ms.ToArray();
            }
#endif
            string json = JsonConvert.SerializeObject(msg);
            return Encoding.UTF8.GetBytes(json);
        }

        private NetMessage? Deserialize(byte[] data)
        {
#if __OLD__
            BinaryFormatter fmt = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(data))
            {
                return (NetMessage)fmt.Deserialize(ms);
            }
#endif
            string json = Encoding.UTF8.GetString(data);
            NetMessage? msg = JsonConvert.DeserializeObject<NetMessage>(json);
            return msg;
        }

        private void PrintMessage(NetMessage msg)
        {
            Console.WriteLine("    MsgType:   {0}  ({1})", msg.MsgType,
                msg.MsgType == 1 ? "文本" : msg.MsgType == 2 ? "文件" : msg.MsgType.ToString());
            Console.WriteLine("    MsgId:     {0}", msg.MsgId);
            Console.WriteLine("    From:      {0}", msg.From);
            Console.WriteLine("    To:        {0}", msg.To);
            Console.WriteLine("    Timestamp: {0:yyyy-MM-dd HH:mm:ss.fff}", msg.Timestamp);
            Console.WriteLine("    BodyLen:   {0} 字节", msg.Body != null ? msg.Body.Length : 0);
            if (msg.Body != null && msg.MsgType == 1)
                Console.WriteLine("    Body:      {0}", Encoding.UTF8.GetString(msg.Body));
            Console.WriteLine("    Headers:   {0} 项", msg.Headers.Count);
            foreach (string k in msg.Headers.Keys)
                Console.WriteLine("      {0} = {1}", k, msg.Headers[k]);
        }
    }

    /// <summary>
    /// 网络消息类（可序列化，演示消息体设计）
    /// </summary>
    [Serializable]
    public class NetMessage
    {
        public int MsgType;
        public Guid MsgId;
        public string From;
        public string To;
        public DateTime Timestamp;
        public byte[] Body;
        public System.Collections.Hashtable Headers;

        public NetMessage()
        {
            Headers = new System.Collections.Hashtable();
            Timestamp = DateTime.Now;
            MsgId = Guid.NewGuid();
        }
    }
}
