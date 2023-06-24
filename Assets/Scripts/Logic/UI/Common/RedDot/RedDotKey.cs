namespace Logic.Common.RedDot
{
    public enum RedDotKey
    {
        /// <summary>
        /// 主线任务完成未领取
        /// </summary>
        MainTaskComplete,

        /// <summary>
        /// 每日任务完成未领取
        /// </summary>
        DailyTaskComplete,

        /// <summary>
        /// 有空的可装备的战利品槽位
        /// </summary>
        SpoilSlotEquipable,

        /// <summary>
        /// 有装备可升级
        /// </summary>
        EquipWeaponUpgradable,

        /// <summary>
        /// 有装备可升级
        /// </summary>
        EquipArmorUpgradable,

        /// <summary>
        /// 有武器可以装备（比现有的属性好, 空的也算)
        /// </summary>
        EquipWeaponEquipable,

        /// <summary>
        /// 有武器可以装备（比现有的属性好, 空的也算)
        /// </summary>
        EquipArmorEquipable,

        /// <summary>
        /// 有可升级的技能
        /// </summary>
        SkillUpgradable,

        /// <summary>
        /// 可强化的伙伴
        /// </summary>
        PartnerUpgradable,

        /// <summary>
        /// 有可研究的考古项
        /// </summary>
        Researchable,

        /// <summary>
        /// 有可升级的英雄突破天赋树
        /// </summary>
        BreakTreeUpgradable,
    }
}