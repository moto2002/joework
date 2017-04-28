namespace Mogo.Util
{
    using Mogo.RPC;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Sockets;
    using System.Threading;

    public class TCPClientWorker
    {
        private int bytesRead;
        private bool m_asynSendSwitch = true;
        private uint m_ConnectChecker = 0;
        private readonly object m_connectCheckerLocker = new object();
        private VObject m_headLengthConverter = VUInt32.Instance;
        private int m_nRecvBufferSize = 0;
        private Thread m_receiveThread;
        private byte[] m_recvBuffer = new byte[0xffff];
        private Queue<byte[]> m_recvQueue = new Queue<byte[]>();
        private readonly object m_recvQueueLocker = new object();
        private byte[] m_sendBuffer = new byte[0xffff];
        private Queue<byte[]> m_sendQueue = new Queue<byte[]>();
        private readonly object m_sendQueueLocker = new object();
        private Thread m_sendThread;
        private Socket m_socket = null;
        private readonly object m_tcpClientLocker = new object();
        private const int MAX_BUFFER_SIZE = 0xffff;
        public Action OnNetworkDisconnected;
        private const int RESERVE_SIZE = 2;

        public TCPClientWorker()
        {
            if (this.m_sendThread == null)
            {
                LoggerHelper.Debug("init AsynSend", true, 0);
                this.m_sendThread = new Thread(new ThreadStart(this.AsynSend));
                this.m_sendThread.IsBackground = true;
            }
            if (!this.m_sendThread.IsAlive)
            {
                LoggerHelper.Debug("Start AsynSend: " + this.m_asynSendSwitch, true, 0);
                this.m_sendThread.Start();
            }
        }

        private void AsynSend()
        {
            while (this.m_asynSendSwitch)
            {
                this.DoSend();
                Thread.Sleep(100);
            }
        }

        public void Close()
        {
            object obj2;
            lock ((obj2 = this.m_connectCheckerLocker))
            {
                this.m_ConnectChecker++;
            }
            if (this.m_socket != null)
            {
                this.m_socket.Close();
            }
            lock ((obj2 = this.m_tcpClientLocker))
            {
                if ((this.m_socket != null) && this.m_socket.Connected)
                {
                    this.m_socket.Close();
                }
                this.m_socket = null;
            }
            this.m_receiveThread = null;
            GC.Collect();
        }

        public void Connect(string IP, int Port)
        {
            lock (this.m_tcpClientLocker)
            {
                if ((this.m_socket != null) && this.m_socket.Connected)
                {
                    throw new Exception("Exception. the tcpClient has Connectted.");
                }
                try
                {
                    LoggerHelper.Debug("Connect.m_ConnectChecker: " + this.m_ConnectChecker, true, 0);
                    this.m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    this.m_socket.ReceiveBufferSize = 0xffff;
                    this.m_socket.NoDelay = true;
                    this.m_socket.Connect(IP, Port);
                    if (this.m_socket != null)
                    {
                        if (this.m_receiveThread == null)
                        {
                            this.m_receiveThread = new Thread(new ThreadStart(this.DoReceive));
                            this.m_receiveThread.IsBackground = true;
                        }
                        if (!this.m_receiveThread.IsAlive)
                        {
                            this.m_receiveThread.Start();
                        }
                    }
                }
                catch (Exception exception)
                {
                    throw new Exception("Exception. the tcpClient do connecting error." + exception);
                }
            }
        }

        public bool Connected()
        {
            return ((this.m_socket != null) && this.m_socket.Connected);
        }

        private void DoReceive()
        {
            do
            {
                this.bytesRead = 0;
                try
                {
                    int size = 0xffff - this.m_nRecvBufferSize;
                    if (size > 0)
                    {
                        this.bytesRead = this.m_socket.Receive(this.m_recvBuffer, this.m_nRecvBufferSize, size, SocketFlags.None);
                        this.m_nRecvBufferSize += this.bytesRead;
                        if (this.bytesRead == 0)
                        {
                            this.bytesRead = 1;
                        }
                    }
                    else
                    {
                        this.bytesRead = 1;
                        LoggerHelper.Warning("buffer not enough", true);
                    }
                }
                catch (ObjectDisposedException)
                {
                    LoggerHelper.Error("tcp close", true);
                }
                catch (IOException exception)
                {
                    LoggerHelper.Except(exception, null);
                }
                catch (Exception exception2)
                {
                    LoggerHelper.Except(exception2, null);
                }
                this.SplitPackets();
            }
            while (this.bytesRead > 0);
            lock (this.m_connectCheckerLocker)
            {
                if (this.m_ConnectChecker > 0)
                {
                    this.m_ConnectChecker--;
                    LoggerHelper.Debug("Disconnected.m_ConnectChecker: " + this.m_ConnectChecker, true, 0);
                }
                else
                {
                    LoggerHelper.Error("receive bytes " + this.bytesRead, true);
                    TimerHeap.AddTimer(0x3e8, 0, this.OnNetworkDisconnected);
                }
            }
            LoggerHelper.Debug("DataReceiveComplete", true, 0);
        }

        private void DoSend()
        {
            object obj2;
            lock ((obj2 = this.m_tcpClientLocker))
            {
                if (!((this.m_socket != null) && this.m_socket.Connected))
                {
                    return;
                }
            }
            int index = 0;
            lock ((obj2 = this.m_sendQueueLocker))
            {
                if (this.m_sendQueue.Count == 0)
                {
                    return;
                }
                while ((index < 0xffff) && (this.m_sendQueue.Count > 0))
                {
                    byte[] buffer = this.m_sendQueue.Peek();
                    if (((index + 2) + buffer.Length) < 0xffff)
                    {
                        byte[] buffer2 = this.m_headLengthConverter.Encode((uint) ((buffer.Length + 2) + this.m_headLengthConverter.VTypeLength));
                        buffer2.CopyTo(this.m_sendBuffer, index);
                        index += buffer2.Length;
                        index += 2;
                        buffer.CopyTo(this.m_sendBuffer, index);
                        index += buffer.Length;
                        this.m_sendQueue.Dequeue();
                    }
                    else
                    {
                        goto Label_012A;
                    }
                }
            }
        Label_012A:;
            try
            {
                this.m_socket.Send(this.m_sendBuffer, 0, index, SocketFlags.None);
                Array.Clear(this.m_sendBuffer, 0, 0xffff);
            }
            catch (Exception exception)
            {
                LoggerHelper.Debug("stream write error : " + exception.ToString(), true, 0);
            }
        }

        public void Process()
        {
        }

        public byte[] Recv()
        {
            if (this.m_recvQueue.Count > 0)
            {
                lock (this.m_recvQueueLocker)
                {
                    return this.m_recvQueue.Dequeue();
                }
            }
            return null;
        }

        public void Release()
        {
            this.m_asynSendSwitch = false;
        }

        public void Send(byte[] bytes)
        {
            lock (this.m_sendQueueLocker)
            {
                this.m_sendQueue.Enqueue(bytes);
            }
        }

        private void SplitPackets()
        {
            Exception exception;
            try
            {
                int index = 0;
                while (this.m_nRecvBufferSize > this.m_headLengthConverter.VTypeLength)
                {
                    try
                    {
                        int num2 = (int) ((uint) this.m_headLengthConverter.Decode(this.m_recvBuffer, ref index));
                        if (this.m_nRecvBufferSize >= num2)
                        {
                            int count = (num2 - this.m_headLengthConverter.VTypeLength) - 2;
                            index += 2;
                            byte[] dst = new byte[count];
                            Buffer.BlockCopy(this.m_recvBuffer, index, dst, 0, count);
                            lock (this.m_recvQueueLocker)
                            {
                                this.m_recvQueue.Enqueue(dst);
                            }
                            this.m_nRecvBufferSize -= num2;
                            index += count;
                            continue;
                        }
                        index -= this.m_headLengthConverter.VTypeLength;
                    }
                    catch (Exception exception1)
                    {
                        exception = exception1;
                        LoggerHelper.Except(exception, null);
                    }
                    break;
                }
                Buffer.BlockCopy(this.m_recvBuffer, index, this.m_recvBuffer, 0, this.m_nRecvBufferSize);
            }
            catch (Exception exception2)
            {
                exception = exception2;
                LoggerHelper.Except(exception, null);
                LoggerHelper.Critical("SplitPackets error.", true);
                this.Close();
            }
        }
    }
}

