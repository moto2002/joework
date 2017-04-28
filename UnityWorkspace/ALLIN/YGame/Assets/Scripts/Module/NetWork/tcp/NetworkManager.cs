using UnityEngine;
using System.Collections.Generic;
using NsTcpClient;
using System.Runtime.InteropServices;

public class NetworkManager : Singleton<NetworkManager>
{
    private ClientSocket m_clientSocket = new ClientSocket(true);
    private ICRC mCrc = new Crc32();

    void Start()
    {
        Init();
    }

    private void Init()
    {
        m_clientSocket.AddStateEvent(OnSocketStateEvt);
        m_clientSocket.AddPacketDistributeEvent(OnPacketDistributeEvt);
    }

    #region Callback

    /// <summary>
    /// 响应socket连接状态
    /// </summary>
    /// <param name="state"></param>
    private void OnSocketStateEvt(eClientState state)
    {
        switch (state)
        {
            case eClientState.eCLIENT_STATE_NONE:
                Debug.Log("None");
                break;
            case eClientState.eClient_STATE_CONNECTING:
                Debug.Log("connecting");
                break;
            case eClientState.eClient_STATE_CONNECTED:
                Debug.Log("connected");
                break;
            case eClientState.eClient_STATE_CONNECT_FAIL:
                Debug.Log("connect fail");
                break;
            case eClientState.eClient_STATE_ABORT:
                Debug.Log("abort");
                break;
            case eClientState.eClient_STATE_DISCONNECT:
                Debug.Log("disconnect");
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 消息分发
    /// </summary>
    /// <param name="packet"></param>
    private void OnPacketDistributeEvt(GamePacket packet)
    {
        //校验crc
        //headcrc和datacrc都需要校验 TODO
        //这里对datacrc校验一下
        byte[] data = packet.data;
        mCrc.Crc(data);
        uint dataCrc = (uint)mCrc.Value;
        if (packet.header.dataCrc32 == dataCrc)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(packet.data);
            KeyValuePair<int, ByteBuffer> _event = new KeyValuePair<int, ByteBuffer>(packet.header.header, buffer);
            //facade.SendMessageCommand(NotiConst.DISPATCH_MESSAGE, _event);

        }
        else
        {
            Debug.LogError("发送方，接收方 的数据不一致");
            //抛弃还是重新请求？TODO
        }
    }

    #endregion

    #region Connect
    /// <summary>
    /// 发送连接请求
    /// </summary>
    public void SendConnect()
    {
        m_clientSocket.Connect(GameConst.SocketAddress, GameConst.SocketPort);
    }
    #endregion

    #region Send
    /// <summary>
    /// 发送SOCKET消息
    /// </summary>
    public void SendMessage(ByteBuffer buffer)
    {
        //if (buffer != null)
        //{
        //    ushort id = buffer.ReadShort();
        //    Debug.LogError("id:" + id);
        //}
        //else
        //{
        //    Debug.LogError("buffer null");
        //}


        byte[] data = buffer.ToBytes();
        Debug.LogError("net work send!!" + data.Length);

        //ByteBuffer buf = new ByteBuffer(data);
        //ushort protoId = buf.ReadShort();
        //Debug.LogError("protoId:" + protoId);
        //byte[] data2 = buf.ToBytes();
        //Debug.LogError("datasize:" + data2.Length);

        // m_clientSocket.Send(data2, protoId);
    }

    public void SendMessage(string str_protoId, ByteBuffer buffer)
    {
        byte[] data = buffer.ToBytes();
        int protoId = int.Parse(str_protoId.Trim());
        Debug.LogError(protoId + ":" + data.Length);
        m_clientSocket.Send(data, protoId);
    }


    #endregion

    public void Unload()
    {

    }

    public void OnDestroy()
    {
        Unload();
        m_clientSocket.DisConnect();
        m_clientSocket.RemoveStateEvent(OnSocketStateEvt);
        m_clientSocket.RemovePacketDistributeEvent(OnPacketDistributeEvt);
        Debug.Log("~NetworkManager was destroy");
    }

}

