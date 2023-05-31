using Framework.EventKit;
using Logic.Manager;
using Networks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Common.RedDot
{

    public class SkillUpgradable : RedDotLogic<GameSkillData>
    {
        protected override IRedDotDataNotifier<GameSkillData> GetDataNotifier()
        {
            return new SkillUpgradableNotifier();
        }

        protected override RedDotStatus GetStatus(GameSkillData data)
        {
            return SkillManager.Ins.CanUpgrade(data.SkillID) ? RedDotStatus.Normal : RedDotStatus.Null;
        }

        protected override string GetUID(GameSkillData data)
        {
            return data.SkillID.ToString();
        }
    }


    public class SkillUpgradableNotifier : IRedDotDataNotifier<GameSkillData>
    {
        public event Action<GameSkillData, object> onDataChanged;


        protected readonly EventGroup m_EventGroup = new();

        public List<GameSkillData> GetDataList()
        {
            return SkillManager.Ins.SkillMap.Values.ToList();
        }

        public void Init()
        {
            m_EventGroup.Register(LogicEvent.SkillListChanged, (i, o) =>
            {
                foreach (var data in GetDataList())
                {
                    onDataChanged?.Invoke(data, null);
                }
   
            });
            
        }
    }

}