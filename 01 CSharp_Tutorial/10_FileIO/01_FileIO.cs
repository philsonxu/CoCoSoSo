using System;
using System.IO;
using System.Text;

namespace CSharp20Tutorial.FileIO
{
    /// <summary>
    /// 文件与IO操作示例
    /// 演示File类、FileInfo、目录操作、流读写、文本文件与二进制文件操作
    /// </summary>
    class FileIODemo
    {
        static void Main(string[] args)
        {
            string testDir = @"C:\CSharpIOTest";
            string testFile = Path.Combine(testDir, "test.txt");
            string copyFile = Path.Combine(testDir, "test_copy.txt");
            string binaryFile = Path.Combine(testDir, "data.bin");
            
            try
            {
                Console.WriteLine("===== 目录操作 =====");
                // 创建目录
                if (!Directory.Exists(testDir))
                {
                    Directory.CreateDirectory(testDir);
                    Console.WriteLine("创建目录：{0}", testDir);
                }
                else
                {
                    Console.WriteLine("目录已存在：{0}", testDir);
                }
                
                // 获取目录信息
                DirectoryInfo dirInfo = new DirectoryInfo(testDir);
                Console.WriteLine("目录全名：{0}", dirInfo.FullName);
                Console.WriteLine("创建时间：{0}", dirInfo.CreationTime);
                
                Console.WriteLine("\n===== File类静态方法：文本文件写入 =====");
                // 写入文本文件（会覆盖原有内容）
                string content = "这是第一行文本\n这是第二行文本\nC#文件IO操作示例\n";
                File.WriteAllText(testFile, content, Encoding.UTF8);
                Console.WriteLine("写入文件成功：{0}", testFile);
                
                // 追加文本
                File.AppendAllText(testFile, "这是追加的第四行\n这是追加的第五行\n");
                Console.WriteLine("追加文本成功");
                
                // 读取文件所有内容
                Console.WriteLine("\n读取文件内容：");
                string readContent = File.ReadAllText(testFile, Encoding.UTF8);
                Console.WriteLine(readContent);
                
                // 按行读取
                Console.WriteLine("按行读取：");
                string[] lines = File.ReadAllLines(testFile, Encoding.UTF8);
                for (int i = 0; i < lines.Length; i++)
                {
                    Console.WriteLine("  行{0}: {1}", i + 1, lines[i]);
                }
                
                Console.WriteLine("\n===== FileInfo类 =====");
                FileInfo fileInfo = new FileInfo(testFile);
                if (fileInfo.Exists)
                {
                    Console.WriteLine("文件名：{0}", fileInfo.Name);
                    Console.WriteLine("文件大小：{0} 字节", fileInfo.Length);
                    Console.WriteLine("扩展名：{0}", fileInfo.Extension);
                    Console.WriteLine("创建时间：{0}", fileInfo.CreationTime);
                    Console.WriteLine("最后修改时间：{0}", fileInfo.LastWriteTime);
                }
                
                // 复制文件
                fileInfo.CopyTo(copyFile, true);  // true表示覆盖已存在文件
                Console.WriteLine("\n文件已复制到：{0}", copyFile);
                
                // 移动文件（重命名）
                string moveFile = Path.Combine(testDir, "test_moved.txt");
                if (File.Exists(moveFile))
                {
                    File.Delete(moveFile);
                }
                File.Move(copyFile, moveFile);
                Console.WriteLine("文件已移动到：{0}", moveFile);
                
                Console.WriteLine("\n===== 使用流读写文本文件 =====");
                // StreamWriter写入
                using (StreamWriter sw = new StreamWriter(Path.Combine(testDir, "stream.txt"), false, Encoding.UTF8))
                {
                    sw.WriteLine("使用StreamWriter写入第一行");
                    sw.WriteLine("使用StreamWriter写入第二行");
                    sw.WriteLine("数字：{0}，字符串：{1}", 100, "测试");
                    Console.WriteLine("StreamWriter写入成功");
                }
                
                // StreamReader读取
                Console.WriteLine("\n使用StreamReader读取：");
                using (StreamReader sr = new StreamReader(Path.Combine(testDir, "stream.txt"), Encoding.UTF8))
                {
                    string line;
                    int lineNum = 1;
                    while ((line = sr.ReadLine()) != null)
                    {
                        Console.WriteLine("  {0}: {1}", lineNum++, line);
                    }
                }
                
                Console.WriteLine("\n===== 二进制文件读写 =====");
                // BinaryWriter写入二进制数据
                using (FileStream fs = new FileStream(binaryFile, FileMode.Create))
                using (BinaryWriter bw = new BinaryWriter(fs, Encoding.UTF8))
                {
                    // 写入不同类型的数据
                    bw.Write(12345);           // int
                    bw.Write(3.14159);         // double
                    bw.Write("Hello Binary"); // string
                    bw.Write(true);            // bool
                    bw.Write('A');             // char
                    
                    // 写入数组
                    int[] numbers = { 10, 20, 30, 40, 50 };
                    bw.Write(numbers.Length);  // 先写入数组长度
                    foreach (int num in numbers)
                    {
                        bw.Write(num);
                    }
                    Console.WriteLine("二进制文件写入成功");
                }
                
                // BinaryReader读取二进制文件
                Console.WriteLine("读取二进制文件内容：");
                using (FileStream fs = new FileStream(binaryFile, FileMode.Open))
                using (BinaryReader br = new BinaryReader(fs, Encoding.UTF8))
                {
                    int intVal = br.ReadInt32();
                    double doubleVal = br.ReadDouble();
                    string strVal = br.ReadString();
                    bool boolVal = br.ReadBoolean();
                    char charVal = br.ReadChar();
                    
                    Console.WriteLine("  整数：{0}", intVal);
                    Console.WriteLine("  双精度：{0}", doubleVal);
                    Console.WriteLine("  字符串：{0}", strVal);
                    Console.WriteLine("  布尔：{0}", boolVal);
                    Console.WriteLine("  字符：{0}", charVal);
                    
                    // 读取数组
                    int arrLen = br.ReadInt32();
                    Console.Write("  数组：");
                    for (int i = 0; i < arrLen; i++)
                    {
                        Console.Write("{0} ", br.ReadInt32());
                    }
                    Console.WriteLine();
                }
                
                Console.WriteLine("\n===== FileSystemWatcher：文件系统监控 =====");
                // 创建一个监控器，监控目录变化
                using (FileSystemWatcher watcher = new FileSystemWatcher(testDir))
                {
                    watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Size;
                    watcher.Filter = "*.txt";
                    
                    // 添加事件处理
                    watcher.Created += (sender, e) => 
                        Console.WriteLine("文件创建：{0}", e.Name);
                    watcher.Deleted += (sender, e) => 
                        Console.WriteLine("文件删除：{0}", e.Name);
                    watcher.Changed += (sender, e) => 
                        Console.WriteLine("文件修改：{0}", e.Name);
                    watcher.Renamed += (sender, e) => 
                        Console.WriteLine("文件重命名：{0} -> {1}", e.OldName, e.Name);
                    
                    watcher.EnableRaisingEvents = true;
                    
                    // 触发一些文件操作来演示监控
                    string tempFile = Path.Combine(testDir, "temp.txt");
                    File.WriteAllText(tempFile, "临时文件");
                    System.Threading.Thread.Sleep(200);
                    File.AppendAllText(tempFile, "添加内容");
                    System.Threading.Thread.Sleep(200);
                    File.Move(tempFile, Path.Combine(testDir, "temp_renamed.txt"));
                    System.Threading.Thread.Sleep(200);
                    File.Delete(Path.Combine(testDir, "temp_renamed.txt"));
                    System.Threading.Thread.Sleep(200);
                    
                    watcher.EnableRaisingEvents = false;
                }
                
                Console.WriteLine("\n===== 路径操作Path类 =====");
                string samplePath = @"C:\Windows\System32\notepad.exe";
                Console.WriteLine("路径：{0}", samplePath);
                Console.WriteLine("文件名：{0}", Path.GetFileName(samplePath));
                Console.WriteLine("不带扩展名的文件名：{0}", Path.GetFileNameWithoutExtension(samplePath));
                Console.WriteLine("扩展名：{0}", Path.GetExtension(samplePath));
                Console.WriteLine("目录名：{0}", Path.GetDirectoryName(samplePath));
                Console.WriteLine("组合路径：{0}", Path.Combine(@"C:\Test", "file.txt"));
                Console.WriteLine("临时文件路径：{0}", Path.GetTempFileName());
                Console.WriteLine("临时目录：{0}", Path.GetTempPath());
                
                Console.WriteLine("\n按任意键清理测试文件并退出...");
                Console.ReadKey();
            }
            catch (IOException ex)
            {
                Console.WriteLine("IO异常：{0}", ex.Message);
            }
            finally
            {
                // 清理测试文件
                try
                {
                    if (Directory.Exists(testDir))
                    {
                        Directory.Delete(testDir, true);
                        Console.WriteLine("测试目录已清理：{0}", testDir);
                    }
                }
                catch
                {
                    Console.WriteLine("清理测试目录失败，请手动删除：{0}", testDir);
                }
            }
        }
    }
}
