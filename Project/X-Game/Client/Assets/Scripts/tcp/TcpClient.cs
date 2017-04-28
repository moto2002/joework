// ------------------------------------------------------------------------------
// NsTcpClient tcp Client Library
// 		by zengyi
// ------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;

using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace NsTcpClient
{

    public class TcpClient : ITcpClient, IDisposable
    {
        static public readonly int MAX_TCP_CLIENT_BUFF_SIZE = (64 * 1024);

        private byte[] mSendBuffer = new byte[MAX_TCP_CLIENT_BUFF_SIZE];//发送缓存
        private byte[] mReadBuffer = new byte[MAX_TCP_CLIENT_BUFF_SIZE];//读取缓存
        private int mWaitSendSize = 0;
        private int mHasReadSize = 0;
        private object mMutex = new object();
        private Socket mSocket = null;
        //private IList mSocketList = new List<Socket> (1);
        private eClientState mState = eClientState.eCLIENT_STATE_NONE;
        private ManualResetEvent mWaiting = new ManualResetEvent(false);
        private Thread mThread = null;
        private LinkedList<tReqHead> mQueueReq = new LinkedList<tReqHead>();
        private bool mIsDispose = false;

        public TcpClient()
        {
            mThread = new Thread(ThreadProc);
            // Thread start run
            mThread.Start();
        }

        ~TcpClient()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

      
        //public
        public void Release()
        {
            Dispose();
        }

        public eClientState GetState()
        {
            lock (mMutex)
            {
                return mState;
            }
        }

        public bool HasReadData()
        {
            lock (mMutex)
            {
                return (mHasReadSize > 0);
            }
        }

        public int GetReadData(byte[] pBuffer, int offset)
        {
            if (pBuffer == null)
                return 0;

            int bufSize = pBuffer.Length;
            if (bufSize <= 0)
                return 0;

            lock (mMutex)
            {
                int ret = bufSize - offset;

                if (ret <= 0)
                {
                    // mei you chu li wan
                    ret = 0;
                    return ret;
                }

                if (ret > mHasReadSize)
                    ret = mHasReadSize;

                Buffer.BlockCopy(mReadBuffer, 0, pBuffer, offset, ret);
                int uLast = mHasReadSize - ret;

                //剩余的数据往前移
                Buffer.BlockCopy(mReadBuffer, ret, mReadBuffer, 0, uLast);
                mHasReadSize = uLast;

                return ret;
            }
        }

        public bool Connect(string pRemoteIp, int uRemotePort, int mTimeOut = -1)
        {
            eClientState state = GetState();
            if ((state == eClientState.eClient_STATE_CONNECTING) || (state == eClientState.eClient_STATE_CONNECTED))
                return false;

            AddConnectReq(pRemoteIp, uRemotePort, mTimeOut);
            return true;
        }

        public bool Send(byte[] pData)
        {
            if ((pData == null) || (pData.Length <= 0))
                return false;

            eClientState state = GetState();
            if (state != eClientState.eClient_STATE_CONNECTED)
                return false;

            AddSendReq(pData);
            return true;
        }

        //private
        private void Dispose(bool Diposing)
        {
            if (!mIsDispose)
            {

                if (mWaiting != null)
                {
                    mWaiting.Set();
                }

                if (mThread != null)
                {
                    mThread.Abort();
                    mThread.Join();
                }

                FreeSendQueue();

                if (Diposing)
                {

                    // 释放托管对象资源

                    CloseSocket();

                    mWaitSendSize = 0;
                    mHasReadSize = 0;
                    mWaiting = null;
                    mThread = null;
                    mMutex = null;
                    //	mSocketList = null;
                    mSendBuffer = null;
                    mReadBuffer = null;
                    mQueueReq = null;
                }

                // 释放非托管对象资源

                mIsDispose = true;
            }
        }

        private void SetClientState(eClientState uState)
        {
            lock (mMutex)
            {
                mState = uState;
            }
        }

        private void AddConnectReq(string pRemoteIp, int uRemotePort, int mTimeOut)
        {
            tReqConnect pReq = new tReqConnect(pRemoteIp, uRemotePort, mTimeOut);
            lock (mMutex)
            {
                mState = eClientState.eClient_STATE_CONNECTING;
                LinkedListNode<tReqHead> node = new LinkedListNode<tReqHead>(pReq);
                mQueueReq.AddLast(node);
            }
        }

        private void AddSendReq(byte[] pData)
        {
            tReqSend pReq = new tReqSend(pData);
            lock (mMutex)
            {
                LinkedListNode<tReqHead> node = new LinkedListNode<tReqHead>(pReq);
                mQueueReq.AddLast(node);
            }
        }

        private tReqHead GetFirstReq()
        {
            tReqHead pReq = null;

            lock (mMutex)
            {
                if (mQueueReq.Count > 0)
                {
                    LinkedListNode<tReqHead> node = mQueueReq.First;
                    if (node != null)
                        pReq = node.Value;
                }
            }

            return pReq;
        }

        private void RemoveFirstReq()
        {
            lock (mMutex)
            {
                mQueueReq.RemoveFirst();
            }
        }

        private void HandleConnect(tReqHead pReq)
        {
            if (pReq == null)
                return;
            tReqConnect pConnect = (tReqConnect)pReq;

            mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            if (mSocket == null)
            {
                // not used
                SetClientState(eClientState.eClient_STATE_CONNECT_FAIL);
                return;
            }

            //	mSocketList[0] = mSocket;

            mSocket.SendTimeout = 0;
            mSocket.ReceiveTimeout = 0;
            //mSocket.SetSocketOption (SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 0);
            //mSocket.SetSocketOption (SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 0);

            bool non = pConnect.timeOut >= 0;

            /*
            if (non)
                mSocket.Blocking = false;
            else
                mSocket.Blocking = true;
            */
            if (non)
            {
                // set waiting = true
                mWaiting.Reset();

                AsyncCallback callBack = new AsyncCallback(OnConnectCallBack);
                mSocket.BeginConnect(pConnect.szRemoteIp, pConnect.uRemotePort, callBack, mSocket);

				//阻塞当前线程，直到当前的 WaitHandle 收到信号
				//返回值:如果当前实例收到信号，则为 true；否则为 false。 
                if (mWaiting.WaitOne(pConnect.timeOut))
                {
                    if (mSocket.Connected && mSocket.Poll(0, SelectMode.SelectWrite))
                    {
                        SetClientState(eClientState.eClient_STATE_CONNECTED);
                    }
                    else
                    {
                        CloseSocket();
                        SetClientState(eClientState.eClient_STATE_CONNECT_FAIL);
                    }
                }
                else
                {
                    if (mSocket.Connected && mSocket.Poll(0, SelectMode.SelectWrite))
                    {
                        SetClientState(eClientState.eClient_STATE_CONNECTED);
                    }
                    else
                    {
                        CloseSocket();
                        SetClientState(eClientState.eClient_STATE_CONNECT_FAIL);
                    }
                }
            }
            else
            {
				//阻塞式
                // yi zhi waiting...
                try
                {
                    mSocket.Connect(pConnect.szRemoteIp, pConnect.uRemotePort);
                    if (mSocket.Connected && mSocket.Poll(0, SelectMode.SelectWrite))
                    {
                        SetClientState(eClientState.eClient_STATE_CONNECTED);
                    }
                    else
                    {
                        CloseSocket();
                        SetClientState(eClientState.eClient_STATE_CONNECT_FAIL);
                    }
                }
                catch (Exception e)
                {
                    CloseSocket();
                    SetClientState(eClientState.eClient_STATE_CONNECT_FAIL);

                    Console.WriteLine(e.Message);
                }
            }

        }

        private void OnConnectCallBack(IAsyncResult result)
        {
            try
            {
                Socket socket = (Socket)result.AsyncState;
                if (socket != null)
                {
                    if (socket.Connected && mSocket.Poll(0, SelectMode.SelectWrite))
                        socket.EndConnect(result);
                }

            }
            finally
            {
                // send waiting = false
                if (mWaiting != null)
                    mWaiting.Set();
            }

        }

        private bool HandleSendReq(tReqHead pReq)
        {
            if (pReq == null)
                return false;

            tReqSend pSend = (tReqSend)pReq;

            if (pSend.pSendData == null)
                return true;
            if (pSend.pSendData.Length <= 0)
                return true;

            int uFreeSize = mSendBuffer.Length - mWaitSendSize;
            int uSendSize = pSend.pSendData.Length;
            if (uFreeSize > uSendSize)
            {
                Buffer.BlockCopy(pSend.pSendData, 0, mSendBuffer, mWaitSendSize, uSendSize);
                mWaitSendSize += uSendSize;
                return true;
            }

            return false;
        }

        private void DoSend()
        {
            if (mState != eClientState.eClient_STATE_CONNECTED)
                return;

            if (mWaitSendSize > 0)
            {
                try
                {
                    int nRet = mSocket.Send(mSendBuffer, mWaitSendSize, SocketFlags.None);
                    if (nRet < 0)
                    {
                        CloseSocket();
                        SetClientState(eClientState.eClient_STATE_ABORT);
                    }
                    else
                    {
                        lock (mMutex)
                        {
                            mWaitSendSize -= nRet;
                            if (mWaitSendSize > 0)
                            {
                                Buffer.BlockCopy(mSendBuffer, nRet, mSendBuffer, 0, mWaitSendSize);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    CloseSocket();
                    SetClientState(eClientState.eClient_STATE_ABORT);

                    Console.WriteLine(e.Message);
                }
            }
        }

        private void DoRead()
        {
            if (mState != eClientState.eClient_STATE_CONNECTED)
            {
                return;
            }

            try
            {
                // 读取数据
                if (mSocket.Poll(0, SelectMode.SelectRead))
                {
                    lock (mMutex)
                    {
                        int nRet = mSocket.Receive(mReadBuffer, mHasReadSize, mReadBuffer.Length - mHasReadSize, SocketFlags.None);
                        if (nRet <= 0)
                        {
                            CloseSocket();
                            SetClientState(eClientState.eClient_STATE_ABORT);
                        }
                        else
                        {
                            mHasReadSize += nRet;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                CloseSocket();
                SetClientState(eClientState.eClient_STATE_ABORT);

                Console.WriteLine(e.Message);
            }
        }

        private void FreeSendQueue()
        {
            lock (mMutex)
            {
                if (mQueueReq != null)
                    mQueueReq.Clear();
            }
        }

        // Thread Runing
        private void ThreadProc()
        {
            while (mThread.ThreadState == ThreadState.Running)
            {
                Execute();
            }
        }

        private void Execute()
        {
            // 没有在连接状态
            if ((mState != eClientState.eClient_STATE_CONNECTED) && (mState != eClientState.eClient_STATE_CONNECTING))
            {
                Thread.Sleep(1);
                return;
            }

            tReqHead pHead = GetFirstReq();
            if (pHead != null)
            {
                if (pHead.uReqType == eReqType.eREQ_TYPE_CONNECT)
                {
                    HandleConnect(pHead);
                    RemoveFirstReq();
                }
                else if (pHead.uReqType == eReqType.eREQ_TYPE_SEND)
                {
                    if (HandleSendReq(pHead))
                        RemoveFirstReq();
                }
            }

            DoSend();
            DoRead();

            Thread.Sleep(1);
        }

        private void CloseSocket()
        {
            if (mSocket == null)
                return;
            //	mSocketList[0] = null;
            if (mSocket.Connected)
                mSocket.Shutdown(SocketShutdown.Both);
            mSocket.Close();
            mSocket = null;
        }

        
    }

}