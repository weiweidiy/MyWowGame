using Framework.GameFSM;
using Logic.UI.Common;

namespace Networks.State
{
    /// <summary>
    /// 断线重连控制
    /// </summary>
    public class Network_BreakLine : IState<NetworkState, NetworkStateData>
    {
        public Network_BreakLine(NetworkState pType) : base(pType)
        {
        }

        public override void Enter(NetworkStateData pContext)
        {
            //TODO
            //开启重连 并记录次数
            //超过重试次数 退出到登录
            UIMsgBox.ShowMsgBox(1, "警告", "网络中断\n请重新登录游戏");
        }

        public override void Update(NetworkStateData pContext)
        {
            
        }

        public override void Release(NetworkStateData pContext)
        {
            
        }
    }
}
