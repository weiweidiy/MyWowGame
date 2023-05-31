using Framework.EventKit;
using Logic.Manager;
using Networks;
using System;
using System.Collections.Generic;

namespace Logic.Common.RedDot
{
    public class EquipArmorEquipable : RedDotLogic<KeyValuePair<ItemType, GameEquipData>>
    {
        protected override IRedDotDataNotifier<KeyValuePair<ItemType, GameEquipData>> GetDataNotifier()
        {
            return new EquipArmorEquipableDataNotifier();
        }

        protected override RedDotStatus GetStatus(KeyValuePair<ItemType, GameEquipData> data)
        {
            //判断是否是最高效果的装备
            if (EquipManager.Ins.BestArmorId.Equals(data.Value.EquipID) && !EquipManager.Ins.CurArmorOnID.Equals(data.Value.EquipID))
                return RedDotStatus.Normal;

            return RedDotStatus.Null;
        }

        protected override string GetUID(KeyValuePair<ItemType, GameEquipData> data)
        {
            return data.Value.EquipID.ToString();
        }
    }


    public class EquipArmorEquipableDataNotifier : IRedDotDataNotifier<KeyValuePair<ItemType, GameEquipData>>
    {
        public event Action<KeyValuePair<ItemType, GameEquipData>, object> onDataChanged;

        protected readonly EventGroup m_EventGroup = new();

        public List<KeyValuePair<ItemType, GameEquipData>> GetDataList()
        {
            var result = new List<KeyValuePair<ItemType, GameEquipData>>();

            var weapon = EquipManager.Ins.ArmorMap;

            float bestHp = 0f;

            int resultKey = 0;

            foreach (var key in weapon.Keys)
            {
                var hp = EquipManager.Ins.GetEquipEffect(key, ItemType.Armor);
                if (hp > bestHp)
                {
                    bestHp = hp;
                    resultKey = key;
                }
            }

            if (resultKey != 0)
                result.Add(new KeyValuePair<ItemType, GameEquipData>(ItemType.Armor, weapon[resultKey]));

            return result;
        }

        public void Init()
        {
            m_EventGroup.Register(LogicEvent.EquipListChanged, (i, o) => {

                var itemType = (ItemType)o;

                var dic = itemType == ItemType.Weapon ? null : EquipManager.Ins.ArmorMap;

                if (dic == null)
                    return;

                foreach (var key in dic.Keys)
                {
                    onDataChanged?.Invoke(new KeyValuePair<ItemType, GameEquipData>(itemType, dic[key]), null);
                }

            });


            m_EventGroup.Register(LogicEvent.EquipOn, (i, o) => {

                var data = (S2C_EquipOn)o;
                var itemType = (ItemType)data.Type;
                var equipData = EquipManager.Ins.GetEquipData(data.EquipID, itemType);


                onDataChanged?.Invoke(new KeyValuePair<ItemType, GameEquipData>(itemType, equipData), null);

            });

            m_EventGroup.Register(LogicEvent.EquipOff, (i, o) => {

                var data = (S2C_EquipOff)o;
                var itemType = (ItemType)data.Type;
                var equipData = EquipManager.Ins.GetEquipData(data.EquipID, itemType);


                onDataChanged?.Invoke(new KeyValuePair<ItemType, GameEquipData>(itemType, equipData), null);

            });
        }



    }

}