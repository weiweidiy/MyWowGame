using DummyServer;
using Logic.Config;
using Logic.Manager;
using Networks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DummyServer
{

    public partial class DummyServerMgr
    {
        #region 数据库操作方法
        /// <summary>
        /// 查询是否已解锁
        /// </summary>
        /// <param name="slotId"></param>
        /// <returns></returns>
        public bool QuerySlotUnlocked(int slotId)
        {
            var result = m_DB.m_SpoilSlotsUnlockData.Where(p => p.Equals(slotId)).SingleOrDefault();
            return result != 0;
        }

        /// <summary>
        /// 解锁一个槽位
        /// </summary>
        /// <param name="slotId"></param>
        public void AddSlotUnlockData(int slotId)
        {
            m_DB.m_SpoilSlotsUnlockData.Add(slotId);
            SendMsg(new S2C_SpoilSlotUnlock { SlotId = slotId });      
        }

        /// <summary>
        /// 查询一个已解锁槽位数据，如果没有战利品则返回空
        /// </summary>
        /// <param name="slotId"></param>
        /// <returns></returns>
        public SpoilSlotData GetSlotState(int slotId)
        {
            return m_DB.m_SpoilSlotsData.Where(p => p.SlotId.Equals(slotId)).SingleOrDefault();
        }

        /// <summary>
        ///  指定spoil是否已经装备
        /// </summary>
        /// <param name="spoilId"></param>
        /// <returns></returns>
        public SpoilSlotData GetSlotStateBySpoilId(int spoilId)
        {
            return m_DB.m_SpoilSlotsData.Where(p => p.SpoilId.Equals(spoilId)).SingleOrDefault();
        }

        /// <summary>
        /// 添加一条槽位数据
        /// </summary>
        /// <param name="data"></param>
        public void AddSlotStateData(SpoilSlotData data)
        {
            m_DB.m_SpoilSlotsData.Add(data);
            SendMsg(new S2C_SpoilEquip { OriginSlotData = null, CurSlotData = data});
        }

        /// <summary>
        /// 更新一条槽位数据
        /// </summary>
        /// <param name="data"></param>
        public void UpdateSlotStateData(SpoilSlotData data)
        {
            var slotState = GetSlotState(data.SlotId);
            var origin = slotState.Clone();
            Debug.Assert(slotState != null, "装备槽位为空，不能更新战利品装备数据");
            slotState.SpoilId = data.SpoilId;
            SendMsg(new S2C_SpoilEquip { OriginSlotData = origin , CurSlotData = slotState});
        }

        /// <summary>
        /// 根据ID从数据库中获取一个Spoil数据对象，如果没有则返回null
        /// </summary>
        /// <param name="spoilId"></param>
        /// <returns></returns>
        public SpoilData GetSpoil(int spoilId)
        {
            return m_DB.m_SpoilsData.Where(p => p.SpoilId.Equals(spoilId)).SingleOrDefault();
        }

        /// <summary>
        /// 添加一条Spoil数据
        /// </summary>
        /// <param name="data"></param>
        public bool AddSpoilData(SpoilData data)
        {
            if(GetSpoil(data.SpoilId) != null)
            {
                Debug.LogError("已经存在战利品 " + data.SpoilId);
                return false;
            }
            m_DB.m_SpoilsData.Add(data);
            SendMsg(new S2C_SpoilDraw { Spoil = data });
            return true;
        }

        /// <summary>
        /// 更新Spoil等级
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public void UpdateSpoilData(SpoilData data)
        {
            var spoilData = GetSpoil(data.SpoilId);
            spoilData.Level = data.Level;
            SendMsg(new S2C_SpoilUpgrade { Spoil = spoilData });
        }

        /// <summary>
        /// 获取当前抽卡进度
        /// </summary>
        /// <returns></returns>
        public int GetSpoilDrawProgress()
        {
            return m_DB.m_SpoilDrawProgress;
        }

        /// <summary>
        /// 抽卡进度递增
        /// </summary>
        public void NextProgress()
        {
            m_DB.m_SpoilDrawProgress++;
        }

        /// <summary>
        /// 添加一条Spoilbreakthrough数据
        /// </summary>
        /// <param name="data"></param>
        public bool AddSpoilBreakthroughData(SpoilBreakthroughData spoilBreakthroughData)
        {
            if (GetSpoilBreakthroughData(spoilBreakthroughData.SpoilId) != null)
            {
                Debug.LogError("已经存在战利品 , 请调用 UpdateSpoilBreakthroughData 方法" + spoilBreakthroughData.SpoilId);
                return false;
            }
            m_DB.m_SpoilBreakthrough.Add(spoilBreakthroughData);
            SendMsg(new S2C_SpoilBreakthrough { SpoilBreakthrough = spoilBreakthroughData });
            return true;
        }

        /// <summary>
        /// 更新一条Spoilbreakthrough数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public void UpdateSpoilBreakthroughData(SpoilBreakthroughData spoilBreakthroughData)
        {
            var data = GetSpoilBreakthroughData(spoilBreakthroughData.SpoilId);
            if (data == null)
            {
                Debug.LogError("没有找到战利品 , 请调用 AddSpoilBreakthroughData 方法" + spoilBreakthroughData.SpoilId);
                return;
            }
            data.Count = spoilBreakthroughData.Count;

            SendMsg(new S2C_SpoilBreakthrough { SpoilBreakthrough = data });
        }

        /// <summary>
        /// 根据id获取一个spoilbreakthrough数据对象
        /// </summary>
        /// <param name="spoilId"></param>
        /// <returns></returns>
        public SpoilBreakthroughData GetSpoilBreakthroughData(int spoilId)
        {
            return m_DB.m_SpoilBreakthrough.Where((p) => p.SpoilId.Equals(spoilId)).SingleOrDefault();
        }

        /// <summary>
        /// 获取当前已突破的次数
        /// </summary>
        /// <returns></returns>
        public int GetSpoilBreakthroughCount(int spoilId)
        {
            var data = GetSpoilBreakthroughData(spoilId);
            if (data == null)
                return 0;

            return data.Count;
        }


        /// <summary>
        /// 保存到数据库
        /// </summary>
        public void Save()
        {
            DummyDB.Save(m_DB);
        }

        #endregion

        #region 业务逻辑
        /// <summary>
        /// 抽卡逻辑
        /// </summary>
        public void OnC2S_SpoilDraw(C2S_SpoilDraw pMsg)
        {
            //检查是否已经抽光了
            var curProgress = GetSpoilDrawProgress();
            if(curProgress > GetMaxProgress())
            {
                //发送ErrorCode：已经抽完了
                return;
            }

            //创建一个新的spoil对象
            var spoilId = GetSpoilIdFromConfig(curProgress);
            if(spoilId == -1) //没找到，配置错误
            {
                Debug.LogError("没有找到当前进度的Spoil" + curProgress);
                return;
            }
            var spoilData = GetNewSpoilData(spoilId, 1);//新的spoil默认等级为1
            //添加新的spoil到数据库
            if(!AddSpoilData(spoilData))
            {
                //发送ErrorCode
                return;
            }

            //抽卡池中的进度增加
            NextProgress();

            var slotId = GetSlotId(spoilId);
            //如果该Spoil对应的槽位还没有解锁，还需解锁对应的槽位
            var unlocked = QuerySlotUnlocked(slotId);
            if (!unlocked)
            {
                AddSlotUnlockData(slotId);
            }

            //扣除战功现在在客户端本地进行计算

            Save();
        }

        /// <summary>
        /// 装备逻辑
        /// </summary>
        public void OnC2S_SpoilEquip(C2S_SpoilEquip pMsg)
        {
            var spoilId = pMsg.SpoilId;
            var spoilData = GetSpoil(spoilId);
            if(spoilData == null)
            {
                Debug.LogError("没有找到Spoil 无法装备 " + spoilId);
                // 发送ErrorCode
                return;
            }

            var slotId = GetSlotId(spoilId);
            var slotStateData = GetSlotState(slotId);
            if(slotStateData == null)
            {//之前没有装备Spoil，新装备
                AddSlotStateData(GetNewSpoilSlotData(slotId, spoilId));
            }
            else
            {//之前有装备的Spoil,更新
                UpdateSlotStateData(GetNewSpoilSlotData(slotId, spoilId));
            }

            Save();
        }

        /// <summary>
        /// 升级强化逻辑
        /// </summary>
        /// <param name="pMsg"></param>
        public void OnC2S_SpoilUpgrade(C2S_SpoilUpgrade pMsg)
        {
            var spoilId = pMsg.Spoil.SpoilId;
            var spoilLevel = pMsg.Spoil.Level;
            var spoilData = GetSpoil(spoilId);
            if (spoilData == null)
            {
                Debug.LogError("没有找到Spoil 无法升级 " + spoilId);
                // 发送ErrorCode
                return;
            }

            //判断等级是否最高
            if(spoilLevel >= GetMaxUpgradeLevel())
            {
                Debug.LogError("已经最高级，无法升级" + spoilId);
                // 发送ErrorCode
                return;
            }

            //等级提升
            var newLevel = spoilLevel + 1;
            var newSpoilData = GetNewSpoilData(spoilId, newLevel);
            UpdateSpoilData(newSpoilData);

            Save();
        }

        /// <summary>
        /// 突破逻辑
        /// </summary>
        /// <param name="pMsg"></param>
        public void OnC2S_SpoilBreakthrough(C2S_SpoilBreakthrough pMsg)
        {
            var spoilId = pMsg.SpoilId;

            //判断资源够不够
            var needCost = SpoilManager.Ins.GetBreakCost(spoilId);
            if (SpoilManager.Ins.DoesCostBreakOreEngough(needCost))
            {
                // error: "突破石不足";
                return;
            }

            //判断是否已经满突破
            if (SpoilManager.Ins.IsMaxBreakthrough(spoilId))
            {
                // error: "已达最大突破次数";
                return;
            }

            //进行突破
            var spoilBreakthroughData = GetSpoilBreakthroughData(spoilId);
            if(spoilBreakthroughData == null)
            {//新数据
                AddSpoilBreakthroughData(spoilBreakthroughData);
            }
            else
            {
                UpdateSpoilBreakthroughData(spoilBreakthroughData);
            }

            Save();
        }

        /// <summary>
        /// 获取Spoil配置的最大数量
        /// </summary>
        /// <returns></returns>
        int GetMaxProgress()
        {
            return Configs.SpoilCfg.GetAllDataCount();
        }

        /// <summary>
        /// 返回spoil最大等级
        /// </summary>
        /// <param name="spoilId"></param>
        /// <returns></returns>
        int GetMaxUpgradeLevel()
        {
            return ConfigManager.Ins.m_SpoilLvlUpCfg.AllData.Keys.Count;
        }

        /// <summary>
        /// 创建一个SpoilData对象
        /// </summary>
        /// <param name="spoilId"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        SpoilData GetNewSpoilData(int spoilId, int level)
        {
            var result = new SpoilData();
            result.SpoilId = spoilId;
            result.Level = level;
            return result;
        }

        SpoilSlotData GetNewSpoilSlotData(int slotId, int spoilId)
        {
            var result = new SpoilSlotData();
            result.SlotId = slotId;
            result.SpoilId = spoilId;
            return result;
        }


        /// <summary>
        /// 在Spoil抽卡池中获取指定索引的Spoil
        /// </summary>
        /// <param name="spoilDrawProgress"></param>
        /// <returns></returns>
        int GetSpoilIdFromConfig(int spoilDrawProgress)
        {
            var cfg = Configs.SpoilUnlockCfg.GetData(spoilDrawProgress);
            if (cfg == null)
                return -1;
            return cfg.SpoilID;
        }

        /// <summary>
        /// 获取Spoil对应的SlotId
        /// </summary>
        /// <param name="spoilId"></param>
        /// <returns></returns>
        int GetSlotId(int spoilId)
        {
            var cfg = Configs.SpoilCfg.GetData(spoilId);
            Debug.Assert(cfg != null, " SpoilCfg 找不到 id " + spoilId);
            return cfg.GroupID;
        }

        #endregion
    }

}
