using System;
using Logic.Common;
using UnityEngine.Serialization;

namespace Networks
{
    /*
     * 网络协议里 一些通用的结构定义 前后端的定义必须一致
     *
     * 现在客户端的DummyServer会直接使用这些结构
     */

    [Serializable]
    public class GameEquipData
    {
        public GameEquipData()
        {
        }

        public GameEquipData(GameEquipData pData)
        {
            EquipID = pData.EquipID;
            Level = pData.Level;
            Count = pData.Count;
        }

        public GameEquipData Clone()
        {
            return new GameEquipData(this);
        }

        public int EquipID; //装备ID
        public int Level; //装备等级
        public int Count; //当前数量
    }

    [Serializable]
    public class GameEquipUpgradeData
    {
        public GameEquipData EquipData;
        public int OldLevel;
    }

    [Serializable]
    public class GameSkillData
    {
        public GameSkillData()
        {
        }

        public GameSkillData(GameSkillData pData)
        {
            SkillID = pData.SkillID;
            Level = pData.Level;
            Count = pData.Count;
        }

        public GameSkillData Clone()
        {
            return new GameSkillData(this);
        }

        public int SkillID; //技能ID
        public int Level; //技能等级
        public int Count; //当前数量
    }

    [Serializable]
    public class GameSkillUpgradeData
    {
        public GameSkillData SkillData;
        public int OldLevel;
    }

    [Serializable]
    public class GamePartnerData
    {
        public GamePartnerData()
        {
        }

        public GamePartnerData(GamePartnerData pData)
        {
            PartnerID = pData.PartnerID;
            Level = pData.Level;
            Count = pData.Count;
        }

        public GamePartnerData Clone()
        {
            return new GamePartnerData(this);
        }

        public int PartnerID; //伙伴ID
        public int Level; //伙伴等级
        public int Count; //当前数量
    }

    [Serializable]
    public class GamePartnerUpgradeData
    {
        public GamePartnerData PartnerData;
        public int OldLevel;
    }
    
    // 到达顶级以后 自动合成的数据
    // 合成数据
    [Serializable]
    public class GameComposeData
    {
        public int FromID;   //合成者ID
        public int ToID;     //合成后ID
        public int ToAddCount; //合成新增数量
        public int ToCount; //合成后数量
    }
    

    // 引擎数据
    [Serializable]
    public class GameEngineData
    {
        public int Id; //引擎Id
        public int TypeId; //引擎TypeId
        public int IsGet; // 引擎是否获取 0/1
        public int AttrId; // 引擎随机属性Id
        public int Level; // 引擎等级
        public int Reform; // 引擎改造次数

        public GameEngineData()
        {
        }

        public GameEngineData(GameEngineData pData)
        {
            Id = pData.Id;
            TypeId = pData.TypeId;
            IsGet = pData.IsGet;
            AttrId = pData.AttrId;
            Level = pData.Level;
            Reform = pData.Reform;
        }

        public GameEngineData Clone()
        {
            return new GameEngineData(this);
        }
    }

    //任务数据
    [Serializable]
    public class GameTaskData
    {
        public int TaskID; //任务ID
        public long TaskProcess; //任务进度
        public int TaskState; //任务是否领取状态

        public GameTaskData()
        {
        }

        public GameTaskData(GameTaskData pData)
        {
            TaskID = pData.TaskID;
            TaskState = pData.TaskState;
            TaskProcess = pData.TaskProcess;
        }

        public GameTaskData Clone()
        {
            return new GameTaskData(this);
        }
    }

    // 抽卡数据
    [Serializable]
    public class GameShopSkillData
    {
        public int ID; // 卡池id
        public int Level; // 卡池等级
        public int Exp; // 卡池经验
        public int TotalExp; //卡池累计经验值

        public GameShopSkillData()
        {
        }

        public GameShopSkillData(GameShopSkillData pData)
        {
            ID = pData.ID;
            Level = pData.Level;
            Exp = pData.Exp;
            TotalExp = pData.TotalExp;
        }

        public GameShopSkillData Clone()
        {
            return new GameShopSkillData(this);
        }
    }

    [Serializable]
    public class GameShopPartnerData
    {
        public int ID; // 卡池id
        public int Level; // 卡池等级
        public int Exp; // 卡池经验
        public int TotalExp; //卡池累计经验值

        public GameShopPartnerData()
        {
        }

        public GameShopPartnerData(GameShopPartnerData pData)
        {
            ID = pData.ID;
            Level = pData.Level;
            Exp = pData.Exp;
            TotalExp = pData.TotalExp;
        }

        public GameShopPartnerData Clone()
        {
            return new GameShopPartnerData(this);
        }
    }

    [Serializable]
    public class GameShopEquipData
    {
        public int ID; // 卡池id
        public int Level; // 卡池等级
        public int Exp; // 卡池经验
        public int TotalExp; //卡池累计经验值

        public GameShopEquipData()
        {
        }

        public GameShopEquipData(GameShopEquipData pData)
        {
            ID = pData.ID;
            Level = pData.Level;
            Exp = pData.Exp;
            TotalExp = pData.TotalExp;
        }

        public GameShopEquipData Clone()
        {
            return new GameShopEquipData(this);
        }
    }

    //副本数据
    [Serializable]
    public class GameCopyData
    {
        public int Level; //当前等级
        public int KeyCount; //当前钥匙数量

        public GameCopyData()
        {
            Level = 1;
            KeyCount = 2;
        }

        public GameCopyData(GameCopyData pData)
        {
            Level = pData.Level;
            KeyCount = pData.KeyCount;
        }

        public GameCopyData Clone()
        {
            return new GameCopyData(this);
        }
    }

    [Serializable]
    public class GameCopyOilData : GameCopyData
    {
        public string BestDamageRecord;
        public int BestLevelRecord;

        public GameCopyOilData() : base()
        {
        }

        public GameCopyOilData(GameCopyOilData pData)
        {
            KeyCount = pData.KeyCount;
            Level = pData.Level;
            BestDamageRecord = pData.BestDamageRecord;
            BestLevelRecord = pData.BestLevelRecord;
        }

        public GameCopyOilData Clone()
        {
            return new GameCopyOilData(this);
        }
    }

    //考古数据
    [Serializable]
    public class GameMiningData
    {
        public int GearCount; // 引擎零件
        public int HammerCount; // 矿锤数量
        public int MineCount; // 矿石数量
        public int BombCount; // 炸弹数量
        public int ScopeCount; // 透视镜数量
        public int FloorCount; // 层数

        public GameMiningData()
        {
            GearCount = 0;
            HammerCount = GameDefine.MaxHammerCount;
            MineCount = 0;
            BombCount = 0;
            ScopeCount = 0;
            FloorCount = 1;
        }

        public GameMiningData(GameMiningData pData)
        {
            GearCount = pData.GearCount;
            HammerCount = pData.HammerCount;
            MineCount = pData.MineCount;
            BombCount = pData.BombCount;
            ScopeCount = pData.ScopeCount;
            FloorCount = pData.FloorCount;
        }

        public GameMiningData Clone()
        {
            return new GameMiningData(this);
        }
    }

    //开放剧情系统
    [Serializable]
    public class GameLockStoryData
    {
        public int LockType; // Lock表Id
        public int LockState; // 是否已经开放

        public GameLockStoryData()
        {
        }

        public GameLockStoryData(GameLockStoryData pData)
        {
            LockType = pData.LockType;
            LockState = pData.LockState;
        }

        public GameLockStoryData Clone()
        {
            return new GameLockStoryData(this);
        }
    }

    //考古研究
    [Serializable]
    public class GameResearchData
    {
        public int ResearchId; //研究Id
        public int ResearchLevel; //研究等级
        public int IsResearching; //是否在研究 0 false 1 true
        public long ResearchTimeStamp; //研究完成时间戳

        public GameResearchData()
        {
        }

        public GameResearchData(GameResearchData pData)
        {
            ResearchId = pData.ResearchId;
            ResearchLevel = pData.ResearchLevel;
            IsResearching = pData.IsResearching;
            ResearchTimeStamp = pData.ResearchTimeStamp;
        }

        public GameResearchData Clone()
        {
            return new GameResearchData(this);
        }
    }

    //考古研究属性
    [Serializable]
    public class GameResearchEffectData
    {
        public float ResearchATK; // 攻击力增加%
        public float ResearchHP; //体力增加%
        public float ResearchHammerLimit; //矿锤拥有上限增加
        public float ResearchMineObtainAmount; //矿石获得量增加%
        public float ResearchHammerRecoverSpeed; //矿锤补充速度增加%
        public float ResearchSpeed; //研究速度增加%

        public GameResearchEffectData()
        {
            ResearchATK = 0;
            ResearchHP = 0;
            ResearchHammerLimit = 0;
            ResearchMineObtainAmount = 0;
            ResearchHammerRecoverSpeed = 0;
            ResearchSpeed = 0;
        }

        public GameResearchEffectData(GameResearchEffectData pData)
        {
            ResearchATK = pData.ResearchATK;
            ResearchHP = pData.ResearchHP;
            ResearchHammerLimit = pData.ResearchHammerLimit;
            ResearchMineObtainAmount = pData.ResearchMineObtainAmount;
            ResearchHammerRecoverSpeed = pData.ResearchHammerRecoverSpeed;
            ResearchSpeed = pData.ResearchSpeed;
        }

        public GameResearchEffectData Clone()
        {
            return new GameResearchEffectData(this);
        }
    }

    [Serializable]
    public class GameQuenchingData
    {
        public int QuenchingId;
        public int AttributeId;
        public int MelodyId;
        public int UnlockType;

        public GameQuenchingData()
        {
            UnlockType = 1;
        }

        public GameQuenchingData(GameQuenchingData pData)
        {
            QuenchingId = pData.QuenchingId;
            AttributeId = pData.AttributeId;
            MelodyId = pData.MelodyId;
            UnlockType = pData.UnlockType;
        }

        public GameQuenchingData Clone()
        {
            return new GameQuenchingData(this);
        }
    }


    /// <summary>
    /// 战利品槽位数据
    /// </summary>
    [Serializable]
    public class SpoilSlotData
    {
        public int SlotId;
        public int SpoilId;

        public SpoilSlotData()
        {
        }

        public SpoilSlotData(SpoilSlotData pData)
        {
            SlotId = pData.SlotId;
            SpoilId = pData.SpoilId;
        }

        public SpoilSlotData Clone()
        {
            return new SpoilSlotData(this);
        }
    }

    /// <summary>
    /// 战利品数据
    /// </summary>
    [Serializable]
    public class SpoilData
    {
        public int SpoilId;
        public int Level;

        public SpoilData()
        {
        }

        public SpoilData(SpoilData pData)
        {
            SpoilId = pData.SpoilId;
            Level = pData.Level;
        }

        public SpoilData Clone()
        {
            return new SpoilData(this);
        }
    }

    /// <summary>
    /// 战利品突破数据
    /// </summary>
    public class SpoilBreakthroughData
    {
        public int SpoilId;
        public int Count;//已突破的次数

        public SpoilBreakthroughData() { }

        public SpoilBreakthroughData(SpoilBreakthroughData pData)
        {
            SpoilId = pData.SpoilId;
            Count = pData.Count;
        }

        public SpoilBreakthroughData Clone()
        {
            return new SpoilBreakthroughData(this);
        }
    }

    //英雄
    [Serializable]
    public class GameRoleData
    {
        public int RoleID; //英雄ID
        public int RoleLevel; //英雄等级
        public int RoleExp; //英雄经验
        public int RoleBreakLevel; //英雄突破等级
        public bool RoleBreakState; //英雄突破按钮状态

        public GameRoleData()
        {
            RoleExp = 0;
            RoleLevel = 1;
            RoleBreakLevel = 0;
            RoleBreakState = false;
        }

        public GameRoleData(GameRoleData pData)
        {
            RoleID = pData.RoleID;
            RoleLevel = pData.RoleLevel;
            RoleExp = pData.RoleExp;
            RoleBreakLevel = pData.RoleBreakLevel;
            RoleBreakState = pData.RoleBreakState;
        }

        public GameRoleData Clone()
        {
            return new GameRoleData(this);
        }
    }
}