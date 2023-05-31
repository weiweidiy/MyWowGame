using Logic.Manager;
using Networks;
using System.Collections.Generic;

namespace Logic.Common.RedDot
{
    public abstract class EquipUpgradable : RedDotLogic<KeyValuePair<ItemType, GameEquipData>>
    {

        protected override RedDotStatus GetStatus(KeyValuePair<ItemType, GameEquipData> data)
        {
            var canUpgrade = EquipManager.Ins.CanUpgrade(data.Value.EquipID, data.Key);
            return canUpgrade ? RedDotStatus.Normal : RedDotStatus.Null;
        }


        protected override string GetUID(KeyValuePair<ItemType, GameEquipData> data)
        {
            return data.Value.EquipID.ToString();
        }
    }

}