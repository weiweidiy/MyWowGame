using Framework.UI;
using Logic.Common;
using TMPro;

namespace Logic.UI.UIMain
{
    public class UIRoom : UIPage
    {
        public TextMeshProUGUI m_FightPower;

        private void Awake()
        {
            //战斗力
            m_EventGroup.Register(LogicEvent.RoomUpgraded, ((i, o) => { ShowFightPower(); }));
            m_EventGroup.Register(LogicEvent.SkillAllEffectUpdate, ((i, o) => { ShowFightPower(); }));
            m_EventGroup.Register(LogicEvent.PartnerAllEffectUpdate, ((i, o) => { ShowFightPower(); }));
            m_EventGroup.Register(LogicEvent.EquipAllATKEffectUpdate, ((i, o) => { ShowFightPower(); }));
            m_EventGroup.Register(LogicEvent.ResearchCompleteEffectUpdate, (i, o) => { ShowFightPower(); });
            m_EventGroup.Register(LogicEvent.QuenchingEffectUpdate, (i, o) => { ShowFightPower(); });
            m_EventGroup.Register(LogicEvent.OnSpoilDraw, (i, o) => { ShowFightPower(); });
            m_EventGroup.Register(LogicEvent.OnSpoilUpgrade, (i, o) => { ShowFightPower(); });
            m_EventGroup.Register(LogicEvent.RoleBreakTreeEffectUpdate, (i, o) => { ShowFightPower(); });
        }

        public override void OnShow()
        {
            ShowFightPower();
        }

        private void ShowFightPower()
        {
            m_FightPower.text = Formula.GetGJJFightPower().ToUIString();
        }
    }
}