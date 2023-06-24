using BreakInfinity;

namespace Logic.UI.UISpoil
{
    /// <summary>
    /// Spoil 槽位View的值对象, 用于显示槽位
    /// </summary>
    public struct SpoilSlotVO
    {
        public enum State
        {
            Locked,
            Unlocked,
            Equiped,
        }

        public int slotId;
        public int spoilId;
        public int spoilLevel;
        public string iconPath;
        public State state;
        public int breakCount;
    }

    /// <summary>
    /// Spoil实体 view 的值对象 , 用于显示Spoil实体
    /// </summary>
    public struct SpoilUnitVO
    {
        public int spoilId;
        public bool hold; //是否拥有
        public bool equiped; //是否装备
        public int level;
        public string iconPath;
    }

    /// <summary>
    /// Spoil 细节信息 值对象
    /// </summary>
    public struct SpoilDetailVO
    {
        public int spoilId;
        public string name;
        public string txtLevel;
        public string skillDesc;
        public bool hold;
        public string holdEffect;
        public BigDouble cost;
        public long breakthroughCost;
        public string txtEquipState;
        public bool btnEquipInteractable;
        public string iconPath;
        public string slotName;
        public bool isMaxLevel;
        public bool canBreakthrough; //只要等级满足就行，和货币无关
        public int breakCount;
    }

    /// <summary>
    /// 图鉴内容值对象
    /// </summary>
    public struct SpoilHandBookContentVO
    {
        public string name;
        public string skillDesc;
        public SpoilUnitVO unitVO;
    }
}