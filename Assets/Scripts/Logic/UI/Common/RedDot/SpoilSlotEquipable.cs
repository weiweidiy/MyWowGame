using Framework.EventKit;
using Logic.Manager;
using Networks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Common.RedDot
{
    public class SpoilSlotEquipable : RedDotLogic<int>
    {
        protected override RedDotInfo GetNewInfo(int newData)
        {
            RedDotInfo result = new RedDotInfo(GetUID(newData), RedDotStatus.Normal, 1);
            return result;
        }

        protected override RedDotStatus GetStatus(int slotId)
        {
            var spoilSlotData = SpoilManager.Ins.GetSlotState(slotId);
            return spoilSlotData == null ? RedDotStatus.Normal : RedDotStatus.Null;
        }


        protected override IRedDotDataNotifier<int> GetDataNotifier()
        {
            return new SpoilSlotEquipableDataNotifier();
        }


        protected override string GetUID(int data)
        {
            return data.ToString();
        }

    }


    public class SpoilSlotEquipableDataNotifier : IRedDotDataNotifier<int>
    {
        public event Action<int, object> onDataChanged;

        protected readonly EventGroup m_EventGroup = new();

        public List<int> GetDataList()
        {
            //var result = new List<int>();
            //解锁表有的，装备表里没有的数据
            var result = from x in SpoilManager.Ins.m_SpoilSlotsUnlockData
                         where !SpoilManager.Ins.m_SpoilSlotsData.Any(y => y.SlotId == x)
                         select x;

            return result.ToList();
        }

        public void Init()
        {
            //解锁了
            m_EventGroup.Register(LogicEvent.OnSpoilSlotUnlock, (i, o) =>
            {
                var slotId = (int)o;
                onDataChanged?.Invoke(slotId, null);

            });

            //装备了
            m_EventGroup.Register(LogicEvent.OnSpoilEquipChanged, (i, o) =>
            {
                var spoilSlotData = o as SpoilSlotData;
                var slotId = spoilSlotData.SlotId;
                onDataChanged?.Invoke(slotId, null);

            });
        }
    }

}