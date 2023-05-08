using Framework.GameFSM;
using UnityWebSocket;

namespace Networks.State
{
    /// <summary>
    /// 主动关闭
    /// </summary>
    public class Network_Close : IState<NetworkState, NetworkStateData>
    {
        public Network_Close(NetworkState pType) : base(pType)
        {
        }

        public override void Enter(NetworkStateData pContext)
        {
            if (pContext.m_WebSocket != null && pContext.m_WebSocket.ReadyState == WebSocketState.Open)
            {
                pContext.m_WebSocket.CloseAsync();
            }

            DoClose();
        }
        
        public override void Update(NetworkStateData pContext)
        {
            
        }

        public override void Release(NetworkStateData pContext)
        {
            
        }

        private void DoClose()
        {
            //TODO 退出到登录界面
        }
    }
}
