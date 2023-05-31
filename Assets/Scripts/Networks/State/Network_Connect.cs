using Cysharp.Threading.Tasks;
using Framework.EventKit;
using Framework.GameFSM;
using Logic.Common;
using UnityEngine;
using UnityWebSocket;

namespace Networks.State
{
    /// <summary>
    /// 连接ing状态
    /// </summary>
    public class Network_Connect : IState<NetworkState, NetworkStateData>
    {
        private NetworkStateData m_ContextData;
        public Network_Connect(NetworkState pType) : base(pType)
        {
        }

        public override async void Enter(NetworkStateData pContext)
        {
            NetworkManager.Ins.ShowHold();
            m_ContextData = pContext;
            
            pContext.m_WebSocket = new WebSocket(pContext.m_ServerAddr);

            pContext.m_WebSocket.OnOpen += Socket_OnOpen;
            pContext.m_WebSocket.OnError += Socket_OnError;

            await UniTask.DelayFrame(10);
            pContext.m_WebSocket.ConnectAsync();
        }

        public override void Update(NetworkStateData pContext)
        {
            
        }

        public override void Release(NetworkStateData pContext)
        {
            pContext.m_WebSocket.OnOpen -= Socket_OnOpen;
            pContext.m_WebSocket.OnError -= Socket_OnError;
            NetworkManager.Ins.HideHold();
        }
        
        //链接成功
        private void Socket_OnOpen(object sender, OpenEventArgs e)
        {
            m_ContextData.m_NetWorkSM.ToWorking();
        }
        
        //链接失败
        private void Socket_OnError(object sender, ErrorEventArgs e)
        {
            Debug.LogError($"Connect Error. [{e.Message}]");
            NetworkManager.Ins.OnConnectFailed();
        }
    }
}
