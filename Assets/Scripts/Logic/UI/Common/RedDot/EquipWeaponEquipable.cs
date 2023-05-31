using Framework.EventKit;
using Logic.Manager;
using Networks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Logic.Common.RedDot
{
    /// <summary>
    /// 可装备（最高属性），可升级
    /// </summary>
    public class EquipWeaponEquipable : RedDotLogic<KeyValuePair<ItemType, GameEquipData>>
    {
        protected override IRedDotDataNotifier<KeyValuePair<ItemType, GameEquipData>> GetDataNotifier()
        {
            return new EquipWeaponEquipableDataNotifier();
        }

        protected override RedDotStatus GetStatus(KeyValuePair<ItemType, GameEquipData> data)
        {
            //判断是否是最高效果的装备
            if(EquipManager.Ins.BestWeaponId.Equals(data.Value.EquipID) && !EquipManager.Ins.CurWeaponOnID.Equals(data.Value.EquipID))
                return RedDotStatus.Normal;

            return RedDotStatus.Null;
            
        }

        protected override string GetUID(KeyValuePair<ItemType, GameEquipData> data)
        {
            return data.Value.EquipID.ToString();
        }
    }


    public class EquipWeaponEquipableDataNotifier : IRedDotDataNotifier<KeyValuePair<ItemType, GameEquipData>>
    {
        public event Action<KeyValuePair<ItemType, GameEquipData>, object> onDataChanged;

        protected readonly EventGroup m_EventGroup = new();

        public List<KeyValuePair<ItemType, GameEquipData>> GetDataList()
        {
            var result = new List<KeyValuePair<ItemType, GameEquipData>>();

            var weapon = EquipManager.Ins.WeaponMap;

            float bestAttack = 0f;

            int resultKey = 0;

            foreach (var key in weapon.Keys)
            {
                var atk = EquipManager.Ins.GetEquipEffect(key, ItemType.Weapon);
                if (atk > bestAttack)
                {
                    bestAttack = atk;
                    resultKey = key;
                }
            }

            if (resultKey != 0)
                result.Add(new KeyValuePair<ItemType, GameEquipData>(ItemType.Weapon, weapon[resultKey]));

            return result;
        }

        public void Init()
        {
            m_EventGroup.Register(LogicEvent.EquipListChanged, (i, o) => {

                var itemType = (ItemType)o;

                var dic = itemType == ItemType.Weapon ? EquipManager.Ins.WeaponMap : null;

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