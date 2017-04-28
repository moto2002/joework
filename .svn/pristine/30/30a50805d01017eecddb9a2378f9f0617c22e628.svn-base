using System.Collections;

namespace NsTcpClient
{

    public enum eClientState
    {
        eCLIENT_STATE_NONE = 0,
        eClient_STATE_CONNECTING,       //连接中
        eClient_STATE_CONNECTED,        //连接上
        eClient_STATE_CONNECT_FAIL,     //连接失败
        eClient_STATE_ABORT,            //中断
        eClient_STATE_DISCONNECT,       //断开连接
    };

    public interface ITcpClient
    {
        void Release();
        bool Connect(string remoteIp, int remotePort, int timeOut = -1);
        eClientState GetState();
        bool Send(byte[] pData);
        bool HasReadData();
        int GetReadData(byte[] pBuffer, int offset);
    }
}
