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
            BigDouble _Quenching = 1 + QuenchingManager.Ins.GetQuenchingATKAdd();
            BigDouble _Spoil = 1 + SpoilManager.Ins.GetAllAtkEffect();

            return _BASE * _SKILL * _PARTNER * _EQUIP * _Engine * _Research * _Quenching * _Spoil;
        }

        //获取GJJ最大血量
        public static BigDouble GetGJJHP()
        {
            BigDouble _BASE = GameDefine.GJJBaseHP + (BigDouble)(GameDefine.GJJHPGrow * GameDataManager.Ins.GJJHPLevel);
            BigDouble _EQUIP = 1 + EquipManager.Ins.GetArmorAdd();
            BigDouble _Engine = 1 + EngineManager.Ins.GetEngineHPAdd();
            BigDouble _Research = 1 + ResearchManager.Ins.GetResearchHPAdd();
            BigDouble _Quenching = 1 + QuenchingManager.Ins.GetQuenchingHPAdd();
            BigDouble _Spoil = 1 + SpoilManager.Ins.GetAllHpEffect();

            return _BASE * _EQUIP * _Engine * _Research * _Quenching * _Spoil;
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
            return GameDefine.GJJAtkSpeed + GameDataManager.Ins.GJJAtkSpeedLevel * GameDefine.GJJAtkSpeedGrow +
                   QuenchingManager.Ins.QuenchingSpeed;
        }

        //获取GJJ暴击率=GJJ基础暴击率(万分比)+(GJJ暴击率成长系数(万分比)*GJJ暴击率等级)
        public static float GetGJJCritical()
        {
            return (0.0001f * (GameDefine.GJJBaseCritical +
                               GameDataManager.Ins.GJJCriticalLevel * GameDefine.GJJCriticalGrow)) +
                   QuenchingManager.Ins.QuenchingCritical;
        }

        //获取GJJ暴击伤害=GJJ基础暴击倍数值+(GJJ暴击倍数值成长系数*GJJ暴击倍数等级)
        public static BigDouble GetGJJCriticalDamage()
        {
            return (GameDefine.GJJBaseCriticalDamage +
                    GameDataManager.Ins.GJJCriticalDamageLevel * GameDefine.GJJCriticalDamageGrow) +
                   (QuenchingManager.Ins.QuenchingCriticalDamage + QuenchingManager.Ins.QuenchingGroupCriticalDamage);
        }

        //获取GJJ两连击概率
        public static float GetGJJDoubleHit()
        {
            return GameDataManager.Ins.GJJDoubleHitLevel;
        }

        //获取GJJ三连击概率
        public static float GetGJJTripletHit()
        {
            return GameDataManager.Ins.GJJTripletHitLevel;
        }

        //获取GJJ闪避率
        public static float GetGJJEvasionRate()
        {
            return QuenchingManager.Ins.QuenchingGroupEvasionRate;
        }

        //获取同伴伤害
        public static float GetCompanionDamage()
        {
            return QuenchingManager.Ins.QuenchingCompanionDamage;
        }

        //获取同伴攻击速度
        public static float GetCompanionASPD()
        {
            //TODO:同伴攻击速度
            return 0;
        }

        //获取技能伤害
        public static float GetSkillDamage()
        {
            return QuenchingManager.Ins.QuenchingSkillDamage;
        }

        //获取技能冷却减少
        public static float GetSkillCooldown()
        {
            //TODO:技能冷却减少
            return 0;
        }

        //获取获得金币量
        public static float GetGoldObtain()
        {
            return QuenchingManager.Ins.QuenchingGoldObtain;
        }

        //获取对Boss伤害量
        public static float GetBossDamageAmount()
        {
            //TODO:对Boss伤害量
            return 0;
        }

        //获取战斗时长增加
        public static float GetBattleDuration()
        {
            //TODO:战斗时长增加
            return 0;
        }

        //获取GJJ体力恢复上限
        public static float GetGJJHPRecoverEverySecond()
        {
            return QuenchingManager.Ins.QuenchingGroupHPRecoverEverySecond;
        }

        //获取GJJ多连射
        public static float GetGJJMultipleShot()
        {
            return QuenchingManager.Ins.QuenchingMultipleShot;
        }

        //获取研究矿锤拥有上限增加
        public static float GetResearchHammerLimit()
        {
            var research = ResearchManager.Ins.ResearchHammerLimit;
            return research;
        }

        //获取矿石获得量增加
        public static float GetResearchMineObtainAmount()
        {
            var research = ResearchManager.Ins.ResearchMineObtainAmount;
            return research;
        }

        //获取矿锤补充速度增加
        public static float GetResearchHammerRecoverSpeed()
        {
            var research = ResearchManager.Ins.ResearchHammerRecoverSpeed;
            return research;
        }

        //获取研究速度增加
        public static float GetResearchSpeed()
        {
            var research = ResearchManager.Ins.ResearchSpeed;
            return research;
        }

        #endregion

        //战斗力=攻击力*(1+暴击率*(暴击值-1))*攻击速度+体力
        public static BigDouble GetGJJFightPower()
        {
            return GetGJJAtk() * GetGJJAtkSpeed() * (1 + GetGJJCritical() * (GetGJJCriticalDamage() - 1)) +
                   GetGJJHP() * 1f;
        }

        #region GJJ 房间升级消耗公式

        public static BigDouble GetGJJRoomUpgradeCost(AttributeType pType, long pRoomLevel)
        {
            switch (pType)
            {
                case AttributeType.ATK:
                    return pRoomLevel * 10;
                case AttributeType.HP:
                    return pRoomLevel * 10;
                case AttributeType.HPRecover:
                    return pRoomLevel * 10;
                case AttributeType.Critical:
                    return pRoomLevel * 10;
                case AttributeType.CriticalDamage:
                    return pRoomLevel * 10;
                case AttributeType.Speed:
                    return pRoomLevel * 10;
                case AttributeType.DoubleHit:
                    return pRoomLevel * 10;
                case AttributeType.TripletHit:
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
            var researchTimeCost = researchData.BaseCostTime + researchData.GrowCostTime * level;
            var researchSpeed = 1 + ResearchManager.Ins.ResearchSpeed;
            return researchTimeCost / researchSpeed;
        }

        public static int GetResearchDiamondCost(int id, int level)
        {
            var cost = GetResearchTimeCost(id, level) / 60;
            return (int)(MathF.Ceiling(cost) * GameDefine.ResearchDiamondCost);
        }

        #endregion

        #region 战利品 spoil相关

        public static BigDouble GetSpoilDrawCost(int drawProgress)
        {
            var spoilUnlockCfg = Configs.SpoilUnlockCfg.GetData(drawProgress);
            if (spoilUnlockCfg == null)
                return 0;

            var spoilId = spoilUnlockCfg.SpoilID;

            if (spoilId == -1)
                return 0;

            BigDouble baseCost = 0;
            try
            {
                baseCost = (BigDouble)spoilUnlockCfg.SpoilUnlockCost;
            }
            catch (Exception e)
            {
                Debug.LogError("兑换Spoil Cost异常 id:" + spoilId + " spoilDrawProgress: " + drawProgress);
                throw e;
            }

            var spoilCfg = Configs.SpoilCfg.GetData(spoilId);
            var finalCost = baseCost * BigDouble.Pow(2.5, spoilCfg.GroupID);
            
            return finalCost.Ceiling();
        }

        public static BigDouble GetSpoilUpgradeCost(int spoilId, int spoilLevel)
        {
            var cfg = Configs.SpoilLvlUpCfg.GetData(spoilLevel);
            BigDouble baseCost = 0;
            try
            {
                baseCost = (BigDouble)cfg.SpoilCost;
            }
            catch (Exception e)
            {
                Debug.LogError("获取Spoil升级Cost异常 id:" + spoilId + " level: " + spoilLevel);
                throw e;
            }

            var spoilCfg = Configs.SpoilCfg.GetData(spoilId);
            var finalCost = baseCost * BigDouble.Pow(4.5, spoilCfg.GroupID);
            return finalCost;
        }

        public static int GetSpoilBreakthroughCost(int spoilId, int spoilLevel)
        {
            return 0;
        }

        #endregion

        #region 英雄系统相关

        public static int GetRoleIntensifyTotalExp(int roleLevel)
        {
            var herosLvUpData = HerosLvUpCfg.GetData(roleLevel) ?? HerosLvUpCfg.GetData(0);
            return herosLvUpData.CostBase + (herosLvUpData.CostGrow + herosLvUpData.CostExp) * roleLevel;
        }

        public static int GetRoleBreakCost(int roleLevel)
        {
            var herosBreakUpData = HerosBreakUpCfg.GetData(roleLevel) ?? HerosBreakUpCfg.GetData(0);
            return herosBreakUpData.CostBase + (herosBreakUpData.CostGrow + herosBreakUpData.CostExp) * roleLevel;
        }

        #endregion
    }
}