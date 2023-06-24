using System;
using System.Collections.Generic;
using System.Linq;
using Framework.EventKit;
using Logic.Manager;
using Networks;

namespace Logic.Common.RedDot
{
    public class Researchable : RedDotLogic<GameResearchData>
    {
        protected override IRedDotDataNotifier<GameResearchData> GetDataNotifier()
        {
            return new ResearchableDataNotifier();
        }

        protected override string GetUID(GameResearchData data)
        {
            return data.ResearchId.ToString();
        }

        protected override RedDotStatus GetStatus(GameResearchData data)
        {
            return ResearchManager.Ins.IsItemCanResearch(data.ResearchId) ? RedDotStatus.Normal : RedDotStatus.Null;
        }
    }

    public class ResearchableDataNotifier : IRedDotDataNotifier<GameResearchData>
    {
        public event Action<GameResearchData, object> onDataChanged;

        protected readonly EventGroup m_EventGroup = new();

        public List<GameResearchData> GetDataList()
        {
            return ResearchManager.Ins.ResearchMap.Values.ToList();
        }

        public void OnDataChanged()
        {
            foreach (var data in GetDataList())
            {
                onDataChanged?.Invoke(data, null);
            }
        }

        public void Init()
        {
            m_EventGroup.Register(LogicEvent.MineChanged, (i, o) => { OnDataChanged(); });
            m_EventGroup.Register(LogicEvent.ResearchMapChanged, (i, o) => { OnDataChanged(); });
        }
    }
}