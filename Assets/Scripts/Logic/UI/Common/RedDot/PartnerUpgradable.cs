using Framework.EventKit;
using Logic.Manager;
using Networks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Common.RedDot
{
    public class PartnerUpgradable : RedDotLogic<GamePartnerData>
    {
        protected override IRedDotDataNotifier<GamePartnerData> GetDataNotifier()
        {
            return new PartnerUpgradableDataNotifier();
        }

        protected override RedDotStatus GetStatus(GamePartnerData data)
        {
            return PartnerManager.Ins.CanUpgrade(data.PartnerID) ? RedDotStatus.Normal : RedDotStatus.Null;
        }

        protected override string GetUID(GamePartnerData data)
        {
            return data.PartnerID.ToString();
        }
    }


    public class PartnerUpgradableDataNotifier : IRedDotDataNotifier<GamePartnerData>
    {
        public event Action<GamePartnerData, object> onDataChanged;


        protected readonly EventGroup m_EventGroup = new();

        public List<GamePartnerData> GetDataList()
        {
            return PartnerManager.Ins.PartnerMap.Values.ToList();
        }

        public void Init()
        {
            m_EventGroup.Register(LogicEvent.PartnerListChanged, (i, o) =>
            {
                foreach (var data in GetDataList())
                {
                    onDataChanged?.Invoke(data, null);
                }

            });

        }
    }

}