using System;
using Framework.EventKit;
using Framework.UI;
using Logic.Common;
using Logic.Fight.Common;
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
            m_EventGroup.Register(LogicEvent.EngineAllEffectUpdate, ((i, o) => { ShowFightPower(); }));
            m_EventGroup.Register(LogicEvent.ResearchCompleteEffectUpdate, (i, o) => { ShowFightPower(); });
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