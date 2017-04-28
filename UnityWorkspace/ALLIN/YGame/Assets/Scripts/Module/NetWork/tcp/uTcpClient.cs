#define USE_NETORDER
//#define USE_PROTOBUF_NET

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Runtime.CompilerServices;
#if USE_NETORDER
using System.Net;
#endif

namespace NsTcpClient
{
    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    public struct GamePackHeader
    {
        public int dataSize;
        public uint headerCrc32;
        public uint dataCrc32;
        public int header;
    }

    // 1. data is ProtoBuf
    // 2. data is json
    public class GamePacket
    {
        public GamePackHeader header;
        public byte[] data = null;

        public bool hasData()
        {
            if ((header.dataSize <= 0) || (data == null))
                return false;
            return (data.Length > 0);
        }

        public string dataToString()
        {
            if (data == null)
                return "";
            return data.ToString();
        }

#if USE_PROTOBUF_NET
		// Data为ProtoBuf，转成对象
		public T ProtoBufToObject<T> () where T: class
		{
			if (data == null)
				return null;
			System.IO.MemoryStream stream = new System.IO.MemoryStream ();
			stream.Write (data, 0, data.Length);
			stream.Seek (0, System.IO.SeekOrigin.Begin);
			T ret = ProtoBuf.Serializer.Deserialize<T> (stream);

			stream.Close ();
			stream.Dispose ();

			return ret;
		}
#endif
    }

    public delegate void OnPacketRead(GamePacket packet);

    public abstract class ClientPackageListener
    {
        public ClientPackageListener(ClientSocket socket)
        {
            mSocket = socket;
        }

        public void AddPacketListener(int header, OnPacketRead callBack)
        {
            if (mSocket != null)
                mSocket.AddPacketListener(header, callBack);
        }

        public void RemovePacketListener(int header)
        {
            if (mSocket != null)
                mSocket.RemovePacketListener(header);
        }

        private ClientSocket mSocket = null;
    }

    public class ClientSocket
    {
        static public readonly int cSocketConnWaitTime = 2000;
        private ITcpClient mTcpClient = null;
        private LinkedList<GamePacket> mPacketList = new LinkedList<GamePacket>();
        private Dictionary<int, OnPacketRead> mPacketListenerMap = new Dictionary<int, OnPacketRead>();
        private byte[] mRecvBuffer = new byte[TcpClient.MAX_TCP_CLIENT_BUFF_SIZE];
        private int mRecvSize = 0;
        private bool mConnecting = false;
        private bool mAbort = false;
        private System.Timers.Timer mTimer = null;
        private ICRC mCrc = new Crc32();
        private event OnSocketStateEvent mStateEvents;
        public delegate void OnSocketStateEvent(eClientState state);


        //add----
        public delegate void OnPacketDistributeEvent(GamePacket packet);
        private event OnPacketDistributeEvent mPacketDistribute;
        public void AddPacketDistributeEvent(OnPacketDistributeEvent evt)
        {
            if (evt == null)
                return;

            mPacketDistribute += evt;
        }

        public void RemovePacketDistributeEvent(OnPacketDistributeEvent evt)
        {
            if (evt == null)
                return;

            mPacketDistribute -= evt;
        }
        //----

        public ClientSocket(bool isUseTimer = false)
        {

            if (isUseTimer)
                mTimer = new System.Timers.Timer(1.0f);

            Stop();
            if (mTimer != null)
            {
                mTimer.Elapsed += new System.Timers.ElapsedEventHandler(delegate
                {
                    Execute();
                    Thread.Sleep(1);
                });
            }
        }

        public void AddStateEvent(OnSocketStateEvent evt)
        {
            if (evt == null)
                return;

            mStateEvents += evt;
        }

        public void RemoveStateEvent(OnSocketStateEvent evt)
        {
            if (evt == null)
                return;

            mStateEvents -= evt;
        }

        public eClientState GetState()
        {
            if (mTcpClient == null)
                return eClientState.eCLIENT_STATE_NONE;
            return mTcpClient.GetState();
        }

        private void CalcHeaderCrc(ref GamePackHeader header, byte[] dst)
        {
            int headerCrcSize = Marshal.SizeOf(header.headerCrc32);
            int sz = Marshal.SizeOf(header) - headerCrcSize;
            mCrc.Crc(dst, headerCrcSize, sz);
            header.headerCrc32 = (uint)mCrc.Value;
            byte[] crc = BitConverter.GetBytes(header.headerCrc32);
            Buffer.BlockCopy(crc, 0, dst, 0, crc.Length);
        }

#if USE_PROTOBUF_NET
		// 发送ProtoBuf T来自ProtoBuf类申明
		public void SendProtoBuf<T>(T data, int packetHandle) where T: class
		{
			if (data == null) {
				return;
			}
			System.IO.MemoryStream stream = new System.IO.MemoryStream ();
			ProtoBuf.Serializer.Serialize<T> (stream, data);
			byte[] buf = stream.ToArray ();
			stream.Close ();
			stream.Dispose ();
			Send (buf, packetHandle);
		}
#endif

        // 支持发送buf为null
        public void Send(byte[] buf, int packetHandle)
        {
            if (mConnecting || (mTcpClient == null))
                return;

            if (mTcpClient.GetState() != eClientState.eClient_STATE_CONNECTED)
                return;

            bool hasBufData = (buf != null) && (buf.Length > 0);

            GamePackHeader header = new GamePackHeader();

            if (hasBufData)
                header.dataSize = buf.Length;
            else
                header.dataSize = 0;

            header.header = packetHandle;

            if (hasBufData)
            {
                mCrc.Crc(buf);
                header.dataCrc32 = (uint)mCrc.Value;
            }
            else
                header.dataCrc32 = 0;

            int headerSize = Marshal.SizeOf(header);
            int dstSize = headerSize;
            if (hasBufData)
                dstSize += buf.Length;
            byte[] dstBuffer = new byte[dstSize];

            IntPtr pStruct = Marshal.AllocHGlobal(headerSize);
            try
            {
                Marshal.StructureToPtr(header, pStruct, false);
                Marshal.Copy(pStruct, dstBuffer, 0, headerSize);

#if USE_NETORDER
                // Calc header Crc
                CalcHeaderCrc(ref header, dstBuffer);

                // used net  将存储在主机系统上的多字节整数值由主机使用的字节顺序转换为网络使用的字节顺序。
                header.headerCrc32 = (uint)IPAddress.HostToNetworkOrder(header.headerCrc32);
                header.dataCrc32 = (uint)IPAddress.HostToNetworkOrder(header.dataCrc32);
                header.header = IPAddress.HostToNetworkOrder(header.header);
                header.dataSize = IPAddress.HostToNetworkOrder(header.dataSize);

                Marshal.StructureToPtr(header, pStruct, false);
                Marshal.Copy(pStruct, dstBuffer, 0, headerSize);
#endif
            }
            finally
            {
                Marshal.FreeHGlobal(pStruct);
            }

#if USE_NETORDER
#else
			// Calc header Crc
			CalcHeaderCrc (ref header, dstBuffer);
#endif
            if (hasBufData)
                Buffer.BlockCopy(buf, 0, dstBuffer, headerSize, buf.Length);
            mTcpClient.Send(dstBuffer);
        }

        public void SendString(string str, int packetHandle)
        {
            if (string.IsNullOrEmpty(str))
                return;
            byte[] buf = System.Text.Encoding.UTF8.GetBytes(str);
            Send(buf, packetHandle);
        }

        public bool Connect(string ip, int port)
        {
            DisConnect();
            mTcpClient = new TcpClient();
            bool ret = mTcpClient.Connect(ip, port, cSocketConnWaitTime);
            if (ret)
            {
                mConnecting = true;
                Start();
            }
            else
            {
                mTcpClient.Release();
                mTcpClient = null;
            }

            return ret;
        }

        public void DisConnect()
        {
            Stop();
            ClearAllProcessPackets();
            if (mTcpClient != null)
            {
                mTcpClient.Release();
                mTcpClient = null;
            }

            mRecvSize = 0;
            mConnecting = false;
            mAbort = false;
        }

        private void Start()
        {
            if ((mTimer != null) && (!mTimer.Enabled))
            {
                mTimer.Enabled = true;
                mTimer.Start();
            }
        }

        private void Stop()
        {
            if ((mTimer != null) && mTimer.Enabled)
            {
                mTimer.Enabled = false;
                mTimer.Stop();
            }

        }

        // 声明为同步函数
        [MethodImplAttribute(MethodImplOptions.Synchronized)]
        public bool Execute()
        {
            //	string threadId = Thread.CurrentThread.ManagedThreadId.ToString ();
            //	Console.WriteLine (threadId);

            if (mTcpClient == null)
            {
                Stop();
                return false;
            }

            if (mConnecting)
            {
                eClientState state = mTcpClient.GetState();
                if ((state == eClientState.eClient_STATE_CONNECTING) ||
                    (state == eClientState.eCLIENT_STATE_NONE))
                    return true;

                if ((state == eClientState.eClient_STATE_CONNECT_FAIL) ||
                    (state == eClientState.eClient_STATE_ABORT))
                {
                    mConnecting = false;
                    mAbort = false;
                    mTcpClient.Release();
                    mTcpClient = null;

                    // Call Event Error
                    if (mStateEvents != null)
                    {
                        mStateEvents(state);
                    }

                    return false;
                }
                else if (state == eClientState.eClient_STATE_CONNECTED)
                {
                    mConnecting = false;
                    mAbort = false;

                    // Call Event Success
                    if (mStateEvents != null)
                    {
                        mStateEvents(state);
                    }

                    return true;
                }

                mConnecting = false;
            }

            if (mTcpClient.HasReadData())
            {
                int recvsize = mTcpClient.GetReadData(mRecvBuffer, mRecvSize);
                if (recvsize > 0)
                {
                    mRecvSize += recvsize;
                    int recvBufSz = mRecvSize;
                    int i = 0;
                    GamePackHeader header = new GamePackHeader();

                    int headerSize = Marshal.SizeOf(header);
                    IntPtr headerBuffer = Marshal.AllocHGlobal(headerSize);
                    try
                    {
                        //可能粘包，用循环来切包
                        while (recvBufSz - i >= headerSize)
                        {
                            Marshal.Copy(mRecvBuffer, i, headerBuffer, headerSize);
                            header = (GamePackHeader)Marshal.PtrToStructure(headerBuffer, typeof(GamePackHeader));
#if USE_NETORDER
                            // used Net 网络字节序转换为本机字节序
                            header.headerCrc32 = (uint)IPAddress.NetworkToHostOrder(header.headerCrc32);
                            header.dataCrc32 = (uint)IPAddress.NetworkToHostOrder(header.dataCrc32);
                            header.header = IPAddress.NetworkToHostOrder(header.header);
                            header.dataSize = IPAddress.NetworkToHostOrder(header.dataSize);
#endif
                            //断包，break掉，继续接收
                            if ((recvBufSz - i) < (header.dataSize + headerSize))
                                break; 

                            GamePacket packet = new GamePacket();
                            packet.header = header;
                            if (packet.header.dataSize <= 0)
                            {
                                packet.header.dataSize = 0;
                                packet.data = null;
                            }
                            else
                            {
                                packet.data = new byte[packet.header.dataSize];
                                Buffer.BlockCopy(mRecvBuffer, i + headerSize, packet.data, 0, packet.header.dataSize);
                            }

                            LinkedListNode<GamePacket> node = new LinkedListNode<GamePacket>(packet);
                            mPacketList.AddLast(node);

                            i += headerSize + header.dataSize;//当前包已处理完，偏移量移动，开始处理下一个包
                        }
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(headerBuffer);
                    }

                    recvBufSz -= i;
                    mRecvSize = recvBufSz;
                    if (recvBufSz > 0)
                        Buffer.BlockCopy(mRecvBuffer, i, mRecvBuffer, 0, recvBufSz); //剩余的数据往前移

                    ProcessPackets();
                    return true;
                }
            }
            else
            {
                if (!mAbort)
                {
                    eClientState state = mTcpClient.GetState();
                    if (state == eClientState.eClient_STATE_ABORT)
                    {
                        mAbort = true;

                        // Call Event Abort
                        if (mStateEvents != null)
                        {
                            mStateEvents(state);
                        }

                        return false;
                    }
                }
            }

            return true;
        }

        private void ClearAllProcessPackets()
        {
            if (mPacketList != null)
                mPacketList.Clear();
        }

        private void ProcessPacket(GamePacket packet)
        {
            OnPacketRead onRead;
            if (mPacketListenerMap.TryGetValue(packet.header.header, out onRead))
            {
                if (onRead != null)
                {
                    onRead(packet);
                }
            }

            if (mPacketDistribute != null)
                mPacketDistribute(packet);

        }

        private void ProcessPackets()
        {
            LinkedListNode<GamePacket> node = mPacketList.First;
            while (node != null)
            {

                GamePacket packet = node.Value;
                LinkedListNode<GamePacket> next = node.Next;
                mPacketList.Remove(node);

                if (packet != null)
                {
                    ProcessPacket(packet);
                    //	if (packet.data != null)
                    //		packet.data = null;
                }

                node = next;
            }
        }

        public void AddPacketListener(int header, OnPacketRead callBack)
        {
            if (mPacketListenerMap.ContainsKey(header))
            {
                throw (new Exception());
            }
            else
            {
                mPacketListenerMap.Add(header, callBack);
            }
        }

        public void RemovePacketListener(int header)
        {
            if (mPacketListenerMap.ContainsKey(header))
            {
                mPacketListenerMap.Remove(header);
            }
        }

        
    }
}
