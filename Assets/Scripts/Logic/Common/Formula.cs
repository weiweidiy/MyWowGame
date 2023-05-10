using System;
using BreakInfinity;
using Configs;
using Logic.Data;
using Logic.Manager;
using UnityEngine;

namespace Logic.Common
{
    /// <summary>
    /// 所有公式计算相关
    ///
    ///   GameDefine 为配置表定义的基础之
    ///   GameDataManager 为运行时玩家数据
    /// 
    /// </summary>
    public static class Formula
    {
        #region GJJ 属性相关

        //获取GJJ攻击力
        public static BigDouble GetGJJAtk()
        {
            BigDouble _BASE = GameDefine.GJJBaseAtk +
                              (BigDouble)(GameDefine.GJJAtkGrow * GameDataManager.Ins.GJJAtkLevel);
            BigDouble _SKILL = 1 + SkillManager.Ins.AllHaveEffect;
            BigDouble _PARTNER = 1 + PartnerManager.Ins.AllHaveEffect;
            BigDouble _EQUIP = 1 + EquipManager.Ins.GetWeaponAdd();
            BigDouble _Engine = 1 + EngineManager.Ins.GetEngineATKAdd();
            BigDouble _Research = 1 + ResearchManager.Ins.GetResearchATKAdd();

            return _BASE * _SKILL * _PARTNER * _EQUIP * _Engine * _Research;
        }

        //获取GJJ最大血量
        public static BigDouble GetGJJHP()
        {
            BigDouble _BASE = GameDefine.GJJBaseHP + (BigDouble)(GameDefine.GJJHPGrow * GameDataManager.Ins.GJJHPLevel);
            BigDouble _EQUIP = 1 + EquipManager.Ins.GetArmorAdd();
            BigDouble _Engine = 1 + EngineManager.Ins.GetEngineHPAdd();
            BigDouble _Research = 1 + ResearchManager.Ins.GetResearchHPAdd();

            return _BASE * _EQUIP * _Engine * _Research;
        }

        //获取GJJ血量恢复速度
        public static BigDouble GetGJJHPRecover()
        {
            return GameDefine.GJJBaseHPRecover +
                   (BigDouble)(GameDefine.GJJHPRecoverGrow * GameDataManager.Ins.GJJHPRecoverLevel);
        }

        //获取GJJ攻击速度
        public static float GetGJJAtkSpeed()
        {
            return GameDefine.GJJAtkSpeed + GameDataManager.Ins.GJJAtkSpeedLevel * GameDefine.GJJAtkSpeedGrow;
        }

        //获取GJJ暴击率=GJJ基础暴击率(万分比)+(GJJ暴击率成长系数(万分比)*GJJ暴击率等级)
        public static float GetGJJCritical()
        {
            return 0.0001f * (GameDefine.GJJBaseCritical +
                              GameDataManager.Ins.GJJCriticalLevel * GameDefine.GJJCriticalGrow);
        }

        //获取GJJ暴击伤害=GJJ基础暴击倍数值+(GJJ暴击倍数值成长系数*GJJ暴击倍数等级)
        public static BigDouble GetGJJCriticalDamage()
        {
            return GameDefine.GJJBaseCriticalDamage +
                   GameDataManager.Ins.GJJCriticalDamageLevel * GameDefine.GJJCriticalDamageGrow;
        }

        //获取GJJ两连击概率
        public static float GetGJJDoubleHit()
        {
            return 0.0f + GameDataManager.Ins.GJJDoubleHitLevel * 0.01f;
        }

        //获取GJJ三连击概率
        public static float GetGJJTripletHit()
        {
            return 0.0f + GameDataManager.Ins.GJJTripletHitLevel * 0.01f;
        }

        #endregion

        //战斗力=攻击力*(1+暴击率*(暴击值-1))*攻击速度+体力
        public static BigDouble GetGJJFightPower()
        {
            return GetGJJAtk() * GetGJJAtkSpeed() * (1 + GetGJJCritical() * (GetGJJCriticalDamage() - 1)) +
                   GetGJJHP() * 1f;
        }

        #region GJJ 房间升级消耗公式

        public static BigDouble GetGJJRoomUpgradeCost(RoomType pType, long pRoomLevel)
        {
            switch (pType)
            {
                case RoomType.ATK:
                    return pRoomLevel * 10;
                case RoomType.HP:
                    return pRoomLevel * 10;
                case RoomType.HPRecover:
                    return pRoomLevel * 10;
                case RoomType.Critical:
                    return pRoomLevel * 10;
                case RoomType.CriticalDamage:
                    return pRoomLevel * 10;
                case RoomType.Speed:
                    return pRoomLevel * 10;
                case RoomType.DoubleHit:
                    return pRoomLevel * 10;
                case RoomType.TripletHit:
                    return pRoomLevel * 10;
            }

            return BigDouble.Zero;
        }

        #endregion

        #region 关卡相关

        public static BigDouble GetLevelAtk()
        {
            var _Cfg = LevelCfg.GetLevelData(GameDataManager.Ins.CurLevelID);
            return (BigDouble)(_Cfg.AtkBase + (_Cfg.AtkGrow + _Cfg.AtkFeel) * GameDataManager.Ins.CurLevelID) *
                   (BigDouble)_Cfg.AtkAdds[GameDataManager.Ins.CurLevelNode - 1];
        }

        public static BigDouble GetLevelHP()
        {
            var _Cfg = LevelCfg.GetLevelData(GameDataManager.Ins.CurLevelID);
            return (BigDouble)(_Cfg.HPBase + (_Cfg.HPGrow + _Cfg.HpFeel) * GameDataManager.Ins.CurLevelID) *
                   (BigDouble)_Cfg.HPAdds[GameDataManager.Ins.CurLevelNode - 1];
        }

        public static BigDouble GetLevelDrop()
        {
            var _Cfg = LevelCfg.GetLevelData(GameDataManager.Ins.CurLevelID);
            return (BigDouble)(_Cfg.DropBase + (_Cfg.DropGrow + _Cfg.DropFeel) * GameDataManager.Ins.CurLevelID);
        }

        public static long GetLevelNormalResGroupID()
        {
            return 100000 + GameDataManager.Ins.CurLevelID % 500;
        }

        public static long GetLevelEliteResGroupID()
        {
            return 200000 + GameDataManager.Ins.CurLevelID % 500;
        }

        public static long GetLevelResID()
        {
            return GameDataManager.Ins.CurLevelID % 500;
        }

        public static int GetEngineIntensifyCost(int engineLevel, int engineReform = 0)
        {
            var engineLvlUpData = EngineLvlUpCfg.GetData(engineLevel) ??
                                  EngineLvlUpCfg.GetData(GameDefine.EngineFormulaId);

            return engineLvlUpData.LvlUpBaseCost
                   * (int)Mathf.Pow(engineLvlUpData.ReformAdditionCost, engineReform)
                   + engineLevel * engineLvlUpData.LvlUpAdditionCost;
        }

        public static int GetEngineDecomposeGet(int engineLevel, int engineReform = 0)
        {
            var engineLvlUpData = EngineLvlUpCfg.GetData(engineLevel) ??
                                  EngineLvlUpCfg.GetData(GameDefine.EngineFormulaId);
            return engineLvlUpData.DecomposeBase
                   + engineReform * engineLvlUpData.DecomposeReformAddition
                   + engineLevel * engineLvlUpData.DecomposeLvlAddition;
        }

        public static int GetResearchMineCost(int id, int level)
        {
            var researchData = DigResearchCfg.GetData(id);
            return researchData.BaseCost + researchData.GrowCost * level;
        }

        public static float GetResearchTimeCost(int id, int level)
        {
            var researchData = DigResearchCfg.GetData(id);
            var researchTimeCost = researchData.BaseCostTime + researchData.BaseCostTime * level;
            var researchSpeed = 1 + ResearchManager.Ins.ResearchSpeed; //研究速度增加 %
            return researchTimeCost / researchSpeed;
        }

        public static int GetResearchDiamondCost(int id, int level)
        {
            var cost = GetResearchTimeCost(id, level) / 60;
            return (int)(MathF.Ceiling(cost) * GameDefine.ResearchDiamondCost);
        }

        #endregion
    }
}