
using Framework.UI;
using Logic.Common;

namespace Logic.UI.UINet
{
    /// <summary>
    /// 部分网络操作 需要弹出最高层级的屏蔽框 等待完成后 用户才能继续操作
    /// eg : 连接服务器
    ///    : 中断重连中
    /// </summary>
    public class UINetHolding : UIPage
    {
        private void Awake()
        {
            m_EventGroup.Register(LogicEvent.CloseHoldUI, OnConnectCloseHold);
        }

        private void OnConnectCloseHold(int arg1, object arg2)
        {
            Hide();
        }
    }
}
