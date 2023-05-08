using Framework.EventKit;
using Framework.UI;
using Logic.Common;
using Logic.Manager;
using Logic.UI.UIMain;

namespace Logic.UI.UICopy
{
    public class UICopyFighting : UIPage
    {
        public void OnClickEscape()
        {
            var _Para = new FightSwitchTo { m_SwitchToType = SwitchToType.ToNormalLevel };
            EventManager.Call(LogicEvent.Fight_Switch, _Para);
            if (_Para.m_CanSwitchToNextNode == false)
                return;
            
            CopyManager.Ins.StopCopyTimer();
            Hide();
            UIManager.Ins.Show<UIMainLeft>();
            UIManager.Ins.Show<UIMainRight>();
        }
    }
}