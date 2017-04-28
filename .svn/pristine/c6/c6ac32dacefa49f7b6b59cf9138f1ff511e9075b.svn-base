using UnityEngine;
using System.Collections.Generic;
using NsTcpClient;
using LuaInterface;
using System.Runtime.InteropServices;



namespace LuaFramework
{
    public class NetworkManager : Manager
    {
        //可能会有多个socket同时存在的情况，待完善
        //private Dictionary<int,ClientSocket> m_socketClients = new  Dictionary<int, ClientSocket>();

        private ClientSocket m_clientSocket = new ClientSocket(true);
        private ICRC mCrc = new Crc32();

        void Awake()
        {
            Init();
        }

        private void Init()
        {
            m_clientSocket.AddStateEvent(OnSocketStateEvt);
            m_clientSocket.AddPacketDistributeEvent(OnPacketDistributeEvt);
        }

        public void OnInit()
        {
            CallMethod("Start");
        }

        /// <summary>
        /// 执行Lua方法
        /// </summary>
        public object[] CallMethod(string func, params object[] args)
        {
            return Util.CallMethod("Network", func, args);
        }

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
                    CallMethod("OnConnect");
                    break;
                case eClientState.eClient_STATE_CONNECT_FAIL:
                    Debug.Log("connect fail");
                    CallMethod("OnConnectFailed");
                    break;
                case eClientState.eClient_STATE_ABORT:
                    Debug.Log("abort");
                    CallMethod("OnDisconnect");
                    break;
                case eClientState.eClient_STATE_DISCONNECT:
                    Debug.Log("disconnect");
                    CallMethod("OnDisconnect");
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
                //把packet.header.header，packet.data 抛到lua里面去处理
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteBytes(packet.data);
                KeyValuePair<int, ByteBuffer> _event = new KeyValuePair<int, ByteBuffer>(packet.header.header, buffer);
                facade.SendMessageCommand(NotiConst.DISPATCH_MESSAGE, _event);
            }
            else
            {
                Debug.LogError("发送方，接收方 的数据不一致");
                //抛弃还是重新请求？TODO
            }
        }

        /// <summary>
        /// 发送连接请求
        /// </summary>
        public void SendConnect()
        {
            m_clientSocket.Connect(AppConst.SocketAddress, AppConst.SocketPort);
        }

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
            Debug.LogError(protoId+":" + data.Length);
            m_clientSocket.Send(data, protoId);
        }


        public void Unload()
        {
            CallMethod("Unload");
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


}
