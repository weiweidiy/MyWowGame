using Framework.Extension;
using Framework.GameFSM;
using UnityWebSocket;

namespace Networks.State
{
    /// <summary>
    /// 网络状态机
    /// </summary>
    public class NetworkSM : FSMachine<IState<NetworkState, NetworkStateData>, NetworkState, NetworkStateData>
    {
        #region 状态机切换
        
        public void ToConnect()
        {
            NextState(m_ContextData._Connect);
        }
        
        public void ToWorking()
        {
            NextState(m_ContextData._Working);
        }
        
        public void ToBreakLine()
        {
            NextState(m_ContextData._BreakLine);
        }
        
        public void ToClose()
        {
            NextState(m_ContextData._Close);
        }
        
        public void ToDummy()
        {
            NextState(m_ContextData._Dummy);
        }
        
        #endregion
    }
    
    //状态机上下文
    public class NetworkStateData
    {
        //状态机
        public NetworkSM m_NetWorkSM;
        
        //状态
        public Network_Connect _Connect = new Network_Connect(NetworkState.Connect);
        public Network_Working _Working = new Network_Working(NetworkState.Working);
        public Network_BreakLine _BreakLine = new Network_BreakLine(NetworkState.BreakLine);
        public Network_Close _Close = new Network_Close(NetworkState.Close);
        public Network_Dummy _Dummy = new Network_Dummy(NetworkState.Dummy);
        
        /// <summary>
        /// 客户端WebSocket实例
        /// </summary>
        public WebSocket m_WebSocket;
        
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string m_ServerAddr;
    }

    //游戏全局状态机
    public enum NetworkState
    {
        Connect,    //连接服务器
        Working,    //工作中
        BreakLine,  //断线(通信异常)
        Close,      //主动关闭
        Dummy,      //本地模拟服务器
    }
}