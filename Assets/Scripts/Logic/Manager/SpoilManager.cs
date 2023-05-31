using BreakInfinity;
using Framework.EventKit;
using Framework.Extension;
using Logic.Common;
using Logic.Config;
using Networks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using Logic.Data;

namespace Logic.Manager
{
    public class SpoilManager : Singleton<SpoilManager>
    {
        public List<int> m_SpoilSlotsUnlockData; //战利品槽位解锁数据
        public List<SpoilSlotData> m_SpoilSlotsData; //战利品槽位数据
        public List<SpoilData> m_SpoilsData; //战利品数据
        public int m_SpoilDrawProgress; //战利品抽卡池当前进度

        public void Init(S2C_Login pMsg)
        {
            m_SpoilSlotsUnlockData = pMsg.SpoilSlotsUnlockData;
            m_SpoilDrawProgress = pMsg.SpoilDrawProgress;
            m_SpoilSlotsData = pMsg.SpoilSlotsData;
            m_SpoilsData = pMsg.SpoilsData;
        }

        #region 本地缓存数据库操作方法

        /// <summary>
        /// 查询是否已解锁
        /// </summary>
        /// <param name="slotId"></param>
        /// <returns></returns>
        public bool QuerySlotUnlocked(int slotId)
        {
            var result = m_SpoilSlotsUnlockData.Where(p => p.Equals(slotId)).SingleOrDefault();
            return result != 0;
        }

        /// <summary>
        /// 解锁一个槽位
        /// </summary>
        /// <param name="slotId"></param>
        public void AddSlotUnlockData(int slotId)
        {
            m_SpoilSlotsUnlockData.Add(slotId);
        }

        /// <summary>
        /// 查询一个已解锁槽位数据，如果没有战利品则返回空
        /// </summary>
        /// <param name="slotId"></param>
        /// <returns></returns>
        public SpoilSlotData GetSlotState(int slotId)
        {
            return m_SpoilSlotsData.Where(p => p.SlotId.Equals(slotId)).SingleOrDefault();
        }

        /// <summary>
        ///  指定spoil是否已经装备
        /// </summary>
        /// <param name="spoilId"></param>
        /// <returns></returns>
        public SpoilSlotData GetSlotStateBySpoilId(int spoilId)
        {
            return m_SpoilSlotsData.Where(p => p.SpoilId.Equals(spoilId)).SingleOrDefault();
        }

        /// <summary>
        /// 添加一条槽位数据
        /// </summary>
        /// <param name="data"></param>
        public void AddSlotStateData(SpoilSlotData data)
        {
            m_SpoilSlotsData.Add(data);
        }

        /// <summary>
        /// 更新一条槽位数据
        /// </summary>
        /// <param name="data"></param>
        public void UpdateSlotStateData(SpoilSlotData data)
        {
            var slotState = GetSlotState(data.SlotId);
            var originSpoilId = slotState.SpoilId;
            Debug.Assert(slotState != null, "装备槽位为空，不能更新战利品装备数据");
            slotState.SpoilId = data.SpoilId;
        }

        /// <summary>
        /// 根据ID从数据库中获取一个Spoil数据对象，如果没有则返回null
        /// </summary>
        /// <param name="spoilId"></param>
        /// <returns></returns>
        public SpoilData GetSpoil(int spoilId)
        {
            return m_SpoilsData.Where(p => p.SpoilId.Equals(spoilId)).SingleOrDefault();
        }

        /// <summary>
        /// 获取所有Spoil
        /// </summary>
        /// <returns></returns>
        public SpoilData[] GetAllSpoils()
        {
            return m_SpoilsData.ToArray();
        }

        /// <summary>
        /// 获取已拥有数量
        /// </summary>
        /// <returns></returns>
        public int GetSpoilAmount()
        {
            return m_SpoilsData.Count;
        }

        /// <summary>
        /// 添加一条Spoil数据
        /// </summary>
        /// <param name="data"></param>
        public bool AddSpoilData(SpoilData data)
        {
            if (GetSpoil(data.SpoilId) != null)
            {
                Debug.LogError("已经存在战利品 " + data.SpoilId);
                return false;
            }

            m_SpoilsData.Add(data);
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
        }

        /// <summary>
        /// 获取当前抽卡进度
        /// </summary>
        /// <returns></returns>
        public int GetSpoilDrawProgress()
        {
            return m_SpoilDrawProgress;
        }

        /// <summary>
        /// 抽卡进度递增
        /// </summary>
        public void NextProgress()
        {
            m_SpoilDrawProgress++;
        }


        /// <summary>
        /// 保存到数据库
        /// </summary>
        public void Save()
        {
        }

        #endregion


        #region 通用接口

        public List<Configs.SpoilData> GetSpoilsBySlotId(int slotId)
        {
            List<Configs.SpoilData> result = new List<Configs.SpoilData>();
            Configs.SpoilCfg.GetDataList((data) => { return data.GroupID.Equals(slotId); }, result);

            return result;
        }

        /// <summary>
        /// 获取Spoil配置的最大数量
        /// </summary>
        /// <returns></returns>
        public int GetMaxProgress()
        {
            return Configs.SpoilCfg.GetAllDataCount();
        }

        /// <summary>
        /// 当前是否已抽光了
        /// </summary>
        /// <returns></returns>
        public bool IsMaxProgress()
        {
            return GetSpoilDrawProgress() >= GetMaxProgress();
        }


        /// <summary>
        /// 在Spoil抽卡池中获取指定索引的Spoil
        /// </summary>
        /// <param name="spoilDrawProgress"></param>
        /// <returns></returns>
        public int GetSpoilIdFromDrawCfg(int spoilDrawProgress)
        {
            var cfg = Configs.SpoilUnlockCfg.GetData(spoilDrawProgress);
            if (cfg == null)
                return -1;
            return cfg.SpoilID;
        }

        /// <summary>
        /// 返回spoil最大等级
        /// </summary>
        /// <param name="spoilId"></param>
        /// <returns></returns>
        public int GetMaxUpgradeLevel()
        {
            return ConfigManager.Ins.m_SpoilLvlUpCfg.AllData.Keys.Count;
        }

        /// <summary>
        /// Icon资源路径
        /// </summary>
        /// <param name="spoilId"></param>
        /// <returns></returns>
        public string GetSpoilResPath(int spoilId)
        {
            var spoilCfg = Configs.SpoilCfg.GetData(spoilId);
            var resCfg = Configs.ResCfg.GetData(spoilCfg.ResID);
            return resCfg.Res;
        }

        /// <summary>
        /// 获取技能描述
        /// </summary>
        /// <param name="spoilId"></param>
        /// <returns></returns>
        public string GetSkillDesc(int spoilId)
        {
            var spoilCfg = Configs.SpoilCfg.GetData(spoilId);
            var attrCfg = Configs.AttributeCfg.GetData(spoilCfg.AttributeID);
            string content = attrCfg.Des;
            float arg = attrCfg.Value;
            return string.Format(content, arg);
        }

        /// <summary>
        /// 获取spoil对应的slot类型id
        /// </summary>
        /// <param name="spoilId"></param>
        /// <returns></returns>
        public int GetSpoilSlotID(int spoilId)
        {
            var spoilCfg = Configs.SpoilCfg.GetData(spoilId);
            return spoilCfg.GroupID;
        }

        /// <summary>
        /// 获取升级消耗
        /// </summary>
        /// <param name="spoilId"></param>
        /// <param name="spoilLevel"></param>
        /// <returns></returns>
        public BigDouble GetUpgradeCost(int spoilId, int spoilLevel)
        {
            return Formula.GetSpoilUpgradeCost(spoilId, spoilLevel);
        }

        /// <summary>
        /// 获取抽卡消耗
        /// </summary>
        /// <returns></returns>
        public BigDouble GetDrawCost(int spoilDrawProgress)
        {
            return Formula.GetSpoilDrawCost(spoilDrawProgress);
        }


        /// <summary>
        /// 获取攻击加成
        /// </summary>
        /// <param name="spoilId"></param>
        /// <param name="spoilLevel"></param>
        /// <returns></returns>
        public float GetAtkEffect(int spoilId, int spoilLevel)
        {
            var cfg = Configs.SpoilCfg.GetData(spoilId);
            return cfg.HasAtkAdditionBase + spoilLevel * cfg.HasATKAdditionGrow;
        }

        /// <summary>
        /// 获取spoil名字
        /// </summary>
        /// <param name="spoilId"></param>
        /// <returns></returns>
        public string GetSpoilName(int spoilId)
        {
            var cfg = Configs.SpoilCfg.GetData(spoilId);
            return cfg.SpoilName;
        }

        /// <summary>
        /// 获取生命加成
        /// </summary>
        /// <param name="spoilId"></param>
        /// <param name="spoilLevel"></param>
        /// <returns></returns>
        public float GetHpEffect(int spoilId, int spoilLevel)
        {
            var cfg = Configs.SpoilCfg.GetData(spoilId);
            return cfg.HasHPAdditionBase + spoilLevel * cfg.HasHPAdditionGrow;
        }

        /// <summary>
        /// 获取所有攻击加成
        /// </summary>
        /// <returns></returns>
        public float GetAllAtkEffect()
        {
            float result = 0;
            var spoils = GetAllSpoils();
            foreach (var spoil in spoils)
            {
                result += GetAtkEffect(spoil.SpoilId, spoil.Level);
            }

            var allAtkEffect = result / 100;

            return allAtkEffect;
        }

        /// <summary>
        /// 获取所有生命加成
        /// </summary>
        /// <returns></returns>
        public float GetAllHpEffect()
        {
            float result = 0;
            var spoils = GetAllSpoils();
            foreach (var spoil in spoils)
            {
                result += GetHpEffect(spoil.SpoilId, spoil.Level);
            }

            var allHpEffect = result / 100;

            return allHpEffect;
        }

        /// <summary>
        /// 查询兑换战功资源是否足够
        /// </summary>
        /// <returns></returns>
        public bool DrawCostEnough(BigDouble needCost)
        {
            //判断战功是否足够
            return needCost <= GameDataManager.Ins.Trophy;
        }

        /// <summary>
        /// 是否是最大等级
        /// </summary>
        /// <param name="spoilId"></param>
        /// <returns></returns>
        public bool IsMaxLevel(int spoilId)
        {
            var spoil = GetSpoil(spoilId);
            if (spoil == null)
                return false;
            return spoil.Level >= GetMaxUpgradeLevel();
        }

        #endregion


        #region 业务接口

        /// <summary>
        /// 抽Spoil
        /// </summary>
        public void SpoilDraw()
        {
            //检查是否已经抽光了
            var curProgress = GetSpoilDrawProgress();
            if (curProgress > GetMaxProgress())
            {
                //发送ErrorCode：已经抽完了
                return;
            }

            //创建一个新的spoil对象
            var spoilId = GetSpoilIdFromDrawCfg(curProgress);
            if (spoilId == -1) //没找到，配置错误
            {
                Debug.LogError("没有找到当前进度的Spoil" + curProgress);
                return;
            }

            //检查cost
            var needCost = GetDrawCost(curProgress);
            if (!DrawCostEnough(needCost))
            {
                EventManager.Call(LogicEvent.ShowTips, "战功不足");
                //Debug.LogError("战功不足");
                return;
            }

            //扣除战功
            GameDataManager.Ins.Trophy -= needCost;
            //立即同步Trophy
            NetworkManager.Ins.SendMsg(new C2S_SyncTrophy()
            {
                Trophy = GameDataManager.Ins.Trophy.ToString()
            });
            //请求抽取Spoil
            NetworkManager.Ins.SendMsg(new C2S_SpoilDraw()
            {
            });
        }

        /// <summary>
        /// 装备Spoil
        /// </summary>
        /// <param name="spoilId"></param>
        public void SpoilEquip(int spoilId)
        {
            var spoilData = GetSpoil(spoilId);
            if (spoilData == null)
            {
                Debug.LogError("没有找到Spoil 无法装备 " + spoilId);
                // 发送ErrorCode
                return;
            }

            NetworkManager.Ins.SendMsg(new C2S_SpoilEquip()
            {
                SpoilId = spoilId
            });
        }

        /// <summary>
        /// 升级
        /// </summary>
        /// <param name="spoilId"></param>
        public void SpoilUpgrade(int spoilId)
        {
            var spoilData = GetSpoil(spoilId);
            if (spoilData == null)
            {
                Debug.LogError("没有找到Spoil 无法升级 " + spoilId);
                // 发送ErrorCode
                return;
            }

            var spoilLevel = spoilData.Level;

            //判断等级是否最高
            if (spoilLevel >= GetMaxUpgradeLevel())
            {
                EventManager.Call(LogicEvent.ShowTips, "已经最高级，无法升级");
                //Debug.LogError("已经最高级，无法升级" + spoilId);
                // 发送ErrorCode
                return;
            }

            //判断战功是否足够
            var needCost = GetUpgradeCost(spoilId, spoilLevel);
            if (!DrawCostEnough(needCost))
            {
                EventManager.Call(LogicEvent.ShowTips, "战功不足");
                //Debug.LogError("战功不足");
                return;
            }

            //扣除战功
            GameDataManager.Ins.Trophy -= needCost;
            //立即同步Trophy
            NetworkManager.Ins.SendMsg(new C2S_SyncTrophy()
            {
                Trophy = GameDataManager.Ins.Trophy.ToString()
            });
            //请求升级Spoil
            NetworkManager.Ins.SendMsg(new C2S_SpoilUpgrade()
            {
                Spoil = spoilData
            });
        }

        #endregion

        #region 消息响应

        public void OnSpoilDrawResult(S2C_SpoilDraw pMsg)
        {
            //更新本地数据
            UpdateLocalSpoilData(pMsg.Spoil);

            NextProgress();

            //通知ui
            EventManager.Call(LogicEvent.OnSpoilDraw, pMsg.Spoil);
        }

        public void OnSpoilSlotUnlock(S2C_SpoilSlotUnlock pMsg)
        {
            UpdateLocalSpoilSlotUnlockData(pMsg.SlotId);

            EventManager.Call(LogicEvent.OnSpoilSlotUnlock, pMsg.SlotId);
        }

        public void OnSpoilEquip(S2C_SpoilEquip pMsg)
        {
            UpdateLocalSpoilSlotStateData(pMsg.OriginSlotData, pMsg.CurSlotData);

            EventManager.Call(LogicEvent.OnSpoilEquipChanged, pMsg.CurSlotData);
        }

        public void OnSpoilUpgrade(S2C_SpoilUpgrade pMsg)
        {
            UpdateLocalSpoilData(pMsg.Spoil);

            EventManager.Call(LogicEvent.OnSpoilUpgrade, pMsg.Spoil);
        }

        #endregion

        #region 更新本地数据

        void UpdateLocalSpoilData(SpoilData spoilData)
        {
            var localData = GetSpoil(spoilData.SpoilId);
            if (localData == null)
            {
                AddSpoilData(spoilData);
            }
            else
            {
                UpdateSpoilData(spoilData);
            }
        }

        void UpdateLocalSpoilSlotUnlockData(int slotId)
        {
            AddSlotUnlockData(slotId);
        }

        void UpdateLocalSpoilSlotStateData(SpoilSlotData origin, SpoilSlotData cur)
        {
            //如果原来没有，则新增，否则更新
            if (origin == null)
            {
                AddSlotStateData(cur);
            }
            else
            {
                UpdateSlotStateData(cur);
            }
        }

        #endregion
    }
}