using Framework.EventKit;
using Logic.Manager;
using Networks;
using System;
using System.Collections.Generic;

namespace Logic.Common.RedDot
{
    public class EquipArmorUpgradable : EquipUpgradable
    {
        protected override IRedDotDataNotifier<KeyValuePair<ItemType, GameEquipData>> GetDataNotifier()
        {
            return new EquipArmorUpgradableDataNotifier();
        }
    }



    /// <summary>
    /// 消息通知
    /// </summary>
    public class EquipArmorUpgradableDataNotifier : IRedDotDataNotifier<KeyValuePair<ItemType, GameEquipData>>
    {
        public event Action<KeyValuePair<ItemType, GameEquipData>, object> onDataChanged;

        protected readonly EventGroup m_EventGroup = new();

        public List<KeyValuePair<ItemType, GameEquipData>> GetDataList()
        {
            var result = new List<KeyValuePair<ItemType, GameEquipData>>();

            //var weapon = EquipManager.Ins.WeaponMap;
            var armors = EquipManager.Ins.ArmorMap;

            foreach (var key in armors.Keys)
            {
                result.Add(new KeyValuePair<ItemType, GameEquipData>(ItemType.Armor, armors[key]));
            }


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
        }



    }
}