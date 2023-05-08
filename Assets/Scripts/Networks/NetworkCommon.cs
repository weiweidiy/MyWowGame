using System;
using Logic.Common;

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
            m_EquipID = pData.m_EquipID;
            m_Level = pData.m_Level;
            m_Count = pData.m_Count;
        }

        public GameEquipData Clone()
        {
            return new GameEquipData(this);
        }

        public int m_EquipID; //装备ID
        public int m_Level; //装备等级
        public int m_Count; //当前数量
    }

    [Serializable]
    public class GameEquipUpgradeData
    {
        public GameEquipData m_EquipData;
        public int m_OldLevel;
    }

    [Serializable]
    public class GameSkillData
    {
        public GameSkillData()
        {
        }

        public GameSkillData(GameSkillData pData)
        {
            m_SkillID = pData.m_SkillID;
            m_Level = pData.m_Level;
            m_Count = pData.m_Count;
        }

        public GameSkillData Clone()
        {
            return new GameSkillData(this);
        }

        public int m_SkillID; //技能ID
        public int m_Level; //技能等级
        public int m_Count; //当前数量
    }

    [Serializable]
    public class GameSkillUpgradeData
    {
        public GameSkillData m_SkillData;
        public int m_OldLevel;
    }

    [Serializable]
    public class GamePartnerData
    {
        public GamePartnerData()
        {
        }

        public GamePartnerData(GamePartnerData pData)
        {
            m_PartnerID = pData.m_PartnerID;
            m_Level = pData.m_Level;
            m_Count = pData.m_Count;
        }

        public GamePartnerData Clone()
        {
            return new GamePartnerData(this);
        }

        public int m_PartnerID; //伙伴ID
        public int m_Level; //伙伴等级
        public int m_Count; //当前数量
    }

    [Serializable]
    public class GamePartnerUpgradeData
    {
        public GamePartnerData m_PartnerData;
        public int m_OldLevel;
    }

    // 引擎数据
    [Serializable]
    public class GameEngineData
    {
        public int m_Id; //引擎Id
        public int m_TypeId; //引擎TypeId
        public int m_IsGet; // 引擎是否获取 0/1
        public int m_AttrId; // 引擎随机属性Id
        public int m_Level; // 引擎等级
        public int m_Reform; // 引擎改造次数

        public GameEngineData()
        {
        }

        public GameEngineData(GameEngineData pData)
        {
            m_Id = pData.m_Id;
            m_TypeId = pData.m_TypeId;
            m_IsGet = pData.m_IsGet;
            m_AttrId = pData.m_AttrId;
            m_Level = pData.m_Level;
            m_Reform = pData.m_Reform;
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
        public int m_TaskID; //任务ID
        public long m_TaskProcess; //任务进度
        public int m_TaskState; //任务是否领取状态

        public GameTaskData()
        {
        }

        public GameTaskData(GameTaskData pData)
        {
            m_TaskID = pData.m_TaskID;
            m_TaskState = pData.m_TaskState;
            m_TaskProcess = pData.m_TaskProcess;
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
        public int m_ID; // 卡池id
        public int m_Level; // 卡池等级
        public int m_Exp; // 卡池经验
        public int m_TotalExp; //卡池累计经验值

        public GameShopSkillData()
        {
        }

        public GameShopSkillData(GameShopSkillData pData)
        {
            m_ID = pData.m_ID;
            m_Level = pData.m_Level;
            m_Exp = pData.m_Exp;
            m_TotalExp = pData.m_TotalExp;
        }

        public GameShopSkillData Clone()
        {
            return new GameShopSkillData(this);
        }
    }

    [Serializable]
    public class GameShopPartnerData
    {
        public int m_ID; // 卡池id
        public int m_Level; // 卡池等级
        public int m_Exp; // 卡池经验
        public int m_TotalExp; //卡池累计经验值

        public GameShopPartnerData()
        {
        }

        public GameShopPartnerData(GameShopPartnerData pData)
        {
            m_ID = pData.m_ID;
            m_Level = pData.m_Level;
            m_Exp = pData.m_Exp;
            m_TotalExp = pData.m_TotalExp;
        }

        public GameShopPartnerData Clone()
        {
            return new GameShopPartnerData(this);
        }
    }

    [Serializable]
    public class GameShopEquipData
    {
        public int m_ID; // 卡池id
        public int m_Level; // 卡池等级
        public int m_Exp; // 卡池经验
        public int m_TotalExp; //卡池累计经验值

        public GameShopEquipData()
        {
        }

        public GameShopEquipData(GameShopEquipData pData)
        {
            m_ID = pData.m_ID;
            m_Level = pData.m_Level;
            m_Exp = pData.m_Exp;
            m_TotalExp = pData.m_TotalExp;
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
        public int m_Level; //当前等级
        public int m_KeyCount; //当前钥匙数量

        public GameCopyData()
        {
            m_Level = 1;
            m_KeyCount = 2;
        }

        public GameCopyData(GameCopyData pData)
        {
            m_Level = pData.m_Level;
            m_KeyCount = pData.m_KeyCount;
        }

        public GameCopyData Clone()
        {
            return new GameCopyData(this);
        }
    }

    //考古数据
    [Serializable]
    public class GameMiningData
    {
        public int m_GearCount; // 引擎零件
        public int m_HammerCount; // 矿锤数量
        public int m_MineCount; // 矿石数量
        public int m_BombCount; // 炸弹数量
        public int m_ScopeCount; // 透视镜数量
        public int m_FloorCount; // 层数

        public GameMiningData()
        {
            m_GearCount = 0;
            m_HammerCount = GameDefine.MaxHammerCount;
            m_MineCount = 0;
            m_BombCount = 0;
            m_ScopeCount = 0;
            m_FloorCount = 1;
        }

        public GameMiningData(GameMiningData pData)
        {
            m_GearCount = pData.m_GearCount;
            m_HammerCount = pData.m_HammerCount;
            m_MineCount = pData.m_MineCount;
            m_BombCount = pData.m_BombCount;
            m_ScopeCount = pData.m_ScopeCount;
            m_FloorCount = pData.m_FloorCount;
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
        public int m_LockType; // Lock表Id
        public int m_LockState; // 是否已经开放

        public GameLockStoryData()
        {
        }

        public GameLockStoryData(GameLockStoryData pData)
        {
            m_LockType = pData.m_LockType;
            m_LockState = pData.m_LockState;
        }

        public GameLockStoryData Clone()
        {
            return new GameLockStoryData(this);
        }
    }
}