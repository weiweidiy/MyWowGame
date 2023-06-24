using System;
using System.Collections.Generic;
using System.Linq;
using Framework.EventKit;
using Logic.Manager;
using Networks;

namespace Logic.Common.RedDot
{
    public class BreakTreeUpgradable : RedDotLogic<GameBreakTreeData>
    {
        protected override IRedDotDataNotifier<GameBreakTreeData> GetDataNotifier()
        {
            return new BreakTreeUpgradableDataNotifier();
        }

        protected override string GetUID(GameBreakTreeData data)
        {
            return data.Id.ToString();
        }

        protected override RedDotStatus GetStatus(GameBreakTreeData data)
        {
            var canUpgradeLevel = RoleBreakTreeManager.Ins.IsCanUpgradeLevel(data.Id);
            return canUpgradeLevel ? RedDotStatus.Normal : RedDotStatus.Null;
        }
    }

    public class BreakTreeUpgradableDataNotifier : IRedDotDataNotifier<GameBreakTreeData>
    {
        public event Action<GameBreakTreeData, object> onDataChanged;

        protected readonly EventGroup m_EventGroup = new();

        public List<GameBreakTreeData> GetDataList()
        {
            return RoleBreakTreeManager.Ins.BreakTreeMap.Values.ToList();
        }

        private void OnDataChanged()
        {
            foreach (var data in GetDataList())
            {
                onDataChanged?.Invoke(data, null);
            }
        }

        public void Init()
        {
            m_EventGroup.Register(LogicEvent.DiamondChanged, (i, o) => { OnDataChanged(); });
            m_EventGroup.Register(LogicEvent.RoleBreakTPChanged, (i, o) => { OnDataChanged(); });
            m_EventGroup.Register(LogicEvent.RoleBreakTreeReset, (i, o) => { OnDataChanged(); });
            m_EventGroup.Register(LogicEvent.RoleBreakTreeIntensify, (i, o) => { OnDataChanged(); });
        }
    }
}