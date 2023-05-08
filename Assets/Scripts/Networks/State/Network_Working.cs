using Framework.GameFSM;
using UnityEngine;
using UnityWebSocket;

namespace Networks.State
{
    /// <summary>
    /// 正常工作ing
    /// </summary>
    public class Network_Working : IState<NetworkState, NetworkStateData>
    {
        private NetworkStateData m_ContextData;
        public Network_Working(NetworkState pType) : base(pType)
        {
        }

        public override void Enter(NetworkStateData pContext)
        {
            m_ContextData = pContext;
            
            NetworkManager.Ins.OnConnectSuccess();
            
            pContext.m_WebSocket.OnMessage += Socket_OnMessage;
            pContext.m_WebSocket.OnError += Socket_OnError;
        }

        public override void Update(NetworkStateData pContext)
        {
            
        }

        public override void Release(NetworkStateData pContext)
        {
            pContext.m_WebSocket.OnMessage -= Socket_OnMessage;
            pContext.m_WebSocket.OnError -= Socket_OnError;
        }
        
        //收到消息 
        private void Socket_OnMessage(object sender, MessageEventArgs e)
        {
            NetworkManager.Ins.OnReceiveMsg(e.RawData);
        }
        
        //网络错误
        private void Socket_OnError(object sender, ErrorEventArgs e)
        {
            Debug.LogError("Network Error. " + e.Message);
            m_ContextData.m_NetWorkSM.ToBreakLine();
        }
    }
}
