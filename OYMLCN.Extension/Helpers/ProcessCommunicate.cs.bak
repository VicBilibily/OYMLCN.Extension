using System;
#if NET461
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;
#endif
using System.IO.Pipes;
using System.Threading;
using System.IO;
using System.Security.Principal;
using System.IO.MemoryMappedFiles;
using OYMLCN.Extensions;

namespace OYMLCN.Helpers
{
    /// <summary>
    /// 进程通讯相关方法封装
    /// </summary>
    public static partial class ProcessHelpers
    {
        /// <summary>
        /// 通过内存共享的方式通讯
        /// </summary>
        public class MemoryMapped : IDisposable
        {
            string ShareName { get; set; }
            long MaxLength { get; set; }

            MemoryMappedFile MMF;

            /// <summary>
            /// 通过内存共享的方式通讯
            /// </summary>
            /// <param name="shareName">共享内存公用名</param>
            /// <param name="maxLength">最大容量</param>
            public MemoryMapped(string shareName = "memorymap", long maxLength = 1024)
            {
                ShareName = shareName;
                MaxLength = maxLength;
            }

            bool mutexCreated;
            Mutex mutex;
            string prefore = string.Empty;
            Thread watcher;
            /// <summary>
            /// 创建内存区域并监控共享内存变更以执行任务
            /// </summary>
            /// <param name="action">当接收到数据后的处理方法</param>
            /// <param name="flush">标记是否处理完后清空数据</param>
            public void Create(Action<string> action, bool flush = true)
            {
                mutex = new Mutex(true, $"{ShareName}Mutex", out mutexCreated);
                mutex.ReleaseMutex();
                MMF = MemoryMappedFile.CreateOrOpen(ShareName, MaxLength, MemoryMappedFileAccess.ReadWrite);
                watcher = new Thread(() =>
                {
                    while (true)
                    {
                        try
                        {
                            Thread.Sleep(100);
                            mutex.WaitOne();
                            var stream = MMF.CreateViewStream();
                            string data = stream.ReadToEnd();
                            if (!data.IsNullOrEmpty() && data != prefore)
                            {
                                prefore = data;
                                action.Invoke(data);
                                stream.Dispose();
                                if (flush)
                                {
                                    stream = MMF.CreateViewStream();
                                    var writer = new BinaryWriter(stream);
                                    writer.Write(new byte[MaxLength]);
                                    stream.Dispose();
                                    prefore = string.Empty;
                                }
                            }
                            mutex.ReleaseMutex();
                        }
                        catch
                        {
                            mutex = new Mutex(true, $"{ShareName}Mutex", out mutexCreated);
                            mutex.ReleaseMutex();
                        }
                    }
                });
                watcher.IsBackground = true;
                watcher.Start();
            }
            /// <summary>
            /// 向共享区域写入数据
            /// </summary>
            /// <param name="data"></param>
            public void Write(string data)
            {
                var bytes = data.StringToBytes();
                var mutex = Mutex.OpenExisting($"{ShareName}Mutex");
                mutex.WaitOne();
                MMF = MemoryMappedFile.CreateOrOpen(ShareName, MaxLength, MemoryMappedFileAccess.ReadWrite);
                using (MemoryMappedViewStream stream = MMF.CreateViewStream())
                {
                    var writer = new BinaryWriter(stream);
                    var write = new byte[MaxLength];
                    bytes.CopyTo(write, 0);
                    writer.Write(write);
                }
                mutex.ReleaseMutex();
            }

            /// <summary>
            /// 执行清理任务
            /// </summary>
            public void Dispose()
            {
                if (MMF != null)
                    MMF.Dispose();
                if (watcher != null)
                    watcher.Abort();
            }
        }

#if NET461
        /// <summary>
        /// IpcChannel通讯管道
        /// </summary>
        public static class IpcChannel
        {
            /// <summary>
            /// 注册服务端通讯管道
            /// </summary>
            /// <param name="type">必须继承 MarshalByRefObject</param>
            /// <param name="portName">服务名</param>
            /// <param name="mode"></param>
            public static void RegisterServiceChannel(Type type, string portName, WellKnownObjectMode mode = WellKnownObjectMode.Singleton)
            {
                ChannelServices.RegisterChannel(new System.Runtime.Remoting.Channels.Ipc.IpcChannel(portName), false);
                RemotingConfiguration.RegisterWellKnownServiceType(type, type.FullName, mode);
            }

            /// <summary>
            /// 注册接收端通讯管道
            /// </summary>
            /// <param name="type">必须继承 MarshalByRefObject</param>
            /// <param name="portName">服务名</param>
            public static void RegisterClientChannel(Type type, string portName)
            {
                ChannelServices.RegisterChannel(new System.Runtime.Remoting.Channels.Ipc.IpcChannel(), false);
                RemotingConfiguration.RegisterWellKnownClientType(new WellKnownClientTypeEntry(type, $"ipc://{portName}/{type.FullName}"));
            }
        }
#endif

        /// <summary>
        /// 命名管道通信
        /// </summary>
        public static class NamedPipe
        {
            static NamedPipeServerStream pipeServer = null;
            /// <summary>
            /// 建立服务
            /// </summary>
            /// <param name="pipeName"></param>
            /// <param name="action"></param>
            public static void Service(string pipeName, Action<string> action)
            {
                pipeServer = new NamedPipeServerStream(pipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
                ThreadPool.QueueUserWorkItem(delegate
                {
                    pipeServer.BeginWaitForConnection((o) =>
                    {
                        NamedPipeServerStream pServer = (NamedPipeServerStream)o.AsyncState;
                        pServer.EndWaitForConnection(o);
                        StreamReader sr = new StreamReader(pServer);
                        while (true)
                        {
                            string data = sr.ReadLine();
                            if (pServer.IsConnected)
                                action.Invoke(data);
                            else
                                break;
                        }
                    }, pipeServer);
                });
            }

            static NamedPipeClientStream pipeClient = null;
            static StreamWriter sw = null;
            /// <summary>
            /// 初始化管道
            /// </summary>
            /// <param name="pipeName"></param>
            /// <param name="hostName"></param>
            public static void InitClient(string pipeName, string hostName = "localhost")
            {
                pipeClient = new NamedPipeClientStream(hostName, pipeName, PipeDirection.InOut, PipeOptions.Asynchronous, TokenImpersonationLevel.None);
                pipeClient.Connect(5000);
                sw = new StreamWriter(pipeClient);
                sw.AutoFlush = true;
            }
            /// <summary>
            /// 发送消息
            /// </summary>
            /// <param name="text"></param>
            public static void ClientSend(string text) => sw.WriteLine(text);
        }
    }
}
