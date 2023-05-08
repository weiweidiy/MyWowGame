using System.Collections.Generic;
using DummyServer;
using Framework.GameFSM;
using UnityEngine;

namespace Networks.State
{
    /// <summary>
    /// 在本地模拟一个服务器 收发处理消息, 开发阶段方便测试 调试
    /// DummyServer NetworkMgr 之间的桥梁
    /// </summary>
    public class Network_Dummy : IState<NetworkState, NetworkStateData>
    {
        //模拟收发队列
        public readonly Queue<byte[]> m_SendQueue = new(4);
        public readonly Queue<byte[]> m_ReceiveQueue = new(4);
        
        public Network_Dummy(NetworkState pType) : base(pType)
        {
        }

        public override void Enter(NetworkStateData pContext)
        {
            DummyServerMgr.Ins.Connect(OnReceiveMsg);
            NetworkManager.Ins.OnConnectSuccess();
        }

        public override void Update(NetworkStateData pContext)
        {
            if (m_SendQueue.Count > 0)
            {
                DummyServerMgr.Ins.ProcessMsg(m_SendQueue.Dequeue());
            }
            
            if (m_ReceiveQueue.Count > 0)
            {
                NetworkManager.Ins.OnReceiveMsg(m_ReceiveQueue.Dequeue());
            }
        }

        public override void Release(NetworkStateData pContext)
        {
            DummyServerMgr.Ins.Stop();
        }
        
        public void SendMsgToServer(byte[] pMsg)
        {
            m_SendQueue.Enqueue(pMsg);
        }
        
        // public void SendMsgToServerImmediately(byte[] pMsg)
        // {
        //     DummyServerMgr.Ins.ProcessMsg(pMsg);
        // }
        
        public void OnReceiveMsg(byte[] pMsg)
        {
            m_ReceiveQueue.Enqueue(pMsg);
        }
    }
}