using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.IO;

public class NetWorkManager
{
    private byte[] tempByteBuffer = new byte[1024 * 4];
    private ByteBuffer byteBuffer = new ByteBuffer();
    private Socket socketClient;
    public Queue<byte[]> packageQueue = new Queue<byte[]>();

    private static NetWorkManager instance;
    public static NetWorkManager GetInstance()
    {
        if (instance == null)
        {
            instance = new NetWorkManager();
        }
        return instance;
    }

    public NetWorkManager()
    {
        //创建Socket对象，连接类型是TCP
        socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }

    public void ConnectServer(string ip, int port)
    {
        //服务器IP地址
        IPAddress ipAddress = IPAddress.Parse(ip);

        //服务器端口
        IPEndPoint ipEndpoint = new IPEndPoint(ipAddress, port);

        //这是一个异步的建立连接，当连接建立成功时调用connectCallback方法
        IAsyncResult result = socketClient.BeginConnect(ipEndpoint, new AsyncCallback(ConnectCallback), socketClient);

        //这里做一个超时的监测，当连接超过5秒还没成功表示超时
        bool success = result.AsyncWaitHandle.WaitOne(5000, true);
        if (!success)
        {
            Closed();
            Debug.Log("connect Time Out");
        }
        else
        {
            //与socket建立连接成功,开始接收
            this.socketClient.BeginReceive(this.tempByteBuffer, 0, this.tempByteBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
        }
    }

    private void ConnectCallback(IAsyncResult asyncConnect)
    {
        Debug.Log("connectSuccess");
    }

    private void ReceiveCallback(IAsyncResult asyncReceive)
    {
        int readLength = this.socketClient.EndReceive(asyncReceive);

        //有数据了
        if (readLength > 0)
        {
            byte[] newByte = new byte[readLength];

            Array.Copy(this.tempByteBuffer, newByte, readLength);

            this.byteBuffer.WriteBytes(newByte);

            //处理粘包
            while (true)
            {
                if ((ushort)(this.byteBuffer.Reaiming()) > 2)
                {
                    ushort packageHead = byteBuffer.ReadUShort();

                    if ((ushort)(byteBuffer.Reaiming()) >= packageHead)
                    {
                        byte[] data = byteBuffer.ReadBytes(packageHead);

                        //把数据加到数据队列，然后再另外一个消费者模式来监听产生，只要产生了，就把它解析出来，抛出去给逻辑处理
                        this.packageQueue.Enqueue(data);
                    }
                    else
                    {
                        //断包,实际获取到的小于应该获取到的，那么就等下次收到的数据在拼接在一起，然后再取出;所以break掉，继续接收
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }
        else
        {
            Debug.Log("length is zero");
        }

        this.socketClient.BeginReceive(this.tempByteBuffer, 0, this.tempByteBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
    }

    public void SendMessage(byte[] data)
    {
        if (!socketClient.Connected)
        {
            socketClient.Close();
            return;
        }

        try
        {
            //向服务端异步发送这个字节数组
            IAsyncResult asyncSend = socketClient.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socketClient);

            //监测超时
            bool success = asyncSend.AsyncWaitHandle.WaitOne(5000, true);
            if (!success)
            {
                socketClient.Close();
                Debug.Log("Time Out !");
            }
        }
        catch (Exception e)
        {
            Debug.Log("send message error: " + e);
        }
    }

    private void SendCallback(IAsyncResult asyncSend)
    {
        this.socketClient.EndSend(asyncSend);
    }

    //关闭Socket
    public void Closed()
    {
        if (socketClient != null && socketClient.Connected)
        {
            socketClient.Shutdown(SocketShutdown.Both);
            socketClient.Close();
        }
        socketClient = null;
    }
}


