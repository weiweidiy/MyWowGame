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
        public List<SpoilBreakthroughData> m_SpoilBreakthrough; //战利品已突破的次数

        public void Init(S2C_Login pMsg)
        {
            m_SpoilSlotsUnlockData = pMsg.SpoilSlotsUnlockData;
            m_SpoilDrawProgress = pMsg.SpoilDrawProgress;
            m_SpoilSlotsData = pMsg.SpoilSlotsData;
            m_SpoilsData = pMsg.SpoilsData;
            m_SpoilBreakthrough = pMsg.SpoilBreakthroughData;
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
        /// 添加一条Spoilbreakthrough数据
        /// </summary>
        /// <param name="data"></param>
        public bool AddSpoilBreakthroughData(SpoilBreakthroughData spoilBreakthroughData)
        {
            if(GetSpoilBreakthroughData(spoilBreakthroughData.SpoilId) != null)
            {
                Debug.LogError("已经存在战利品 , 请调用 UpdateSpoilBreakthroughData 方法" + spoilBreakthroughData.SpoilId);
                return false;
            }
            m_SpoilBreakthrough.Add(spoilBreakthroughData);
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
            if(data == null)
            {
                Debug.LogError("没有找到战利品 , 请调用 AddSpoilBreakthroughData 方法" + spoilBreakthroughData.SpoilId);
                return;
            }
            data.Count = spoilBreakthroughData.Count;
            
        }

        /// <summary>
        /// 根据id获取一个spoilbreakthrough数据对象
        /// </summary>
        /// <param name="spoilId"></param>
        /// <returns></returns>
        public SpoilBreakthroughData GetSpoilBreakthroughData(int spoilId)
        {
            return m_SpoilBreakthrough.Where((p) => p.SpoilId.Equals(spoilId)).SingleOrDefault();
        }

        /// <summary>
        /// 获取当前已突破的次数
        /// </summary>
        /// <returns></returns>
        public int GetSpoilBreakthroughLevel(int spoilId)
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

            return GetSkillDesc(spoilId, GetSpoilBreakthroughLevel(spoilId));
        }


        public string GetSkillDesc(int spoilId, int breakLevel)
        {
            var spoilCfg = Configs.SpoilCfg.GetData(spoilId);
            var attrCfg = Configs.AttributeCfg.GetData(spoilCfg.AttributeID);
            string content = attrCfg.Des;
            float arg = GetSkillSkillEffect(spoilId);
            var breakEffect = GetSkillBreakEffect(spoilId, breakLevel);
            arg += breakEffect * breakLevel;

            return string.Format(content, arg);
        }

        /// <summary>
        /// 获取技能效果
        /// </summary>
        /// <param name="spoilId"></param>
        /// <returns></returns>
        public float GetSkillSkillEffect(int spoilId)
        {
            var spoilCfg = Configs.SpoilCfg.GetData(spoilId);
            var attrCfg = Configs.AttributeCfg.GetData(spoilCfg.AttributeID);
            string content = attrCfg.Des;
            return attrCfg.Value;
        }

        /// <summary>
        /// 获取技能突破效果
        /// </summary>
        /// <param name="spoilId"></param>
        /// <param name="breakLevel"></param>
        /// <returns></returns>
        public float GetSkillBreakEffect(int spoilId, int breakLevel)
        {
            if (breakLevel == 0)
                return 0;

            List<Configs.SpoilBreakUpData> result = new List<Configs.SpoilBreakUpData>();
            Configs.SpoilBreakUpCfg.GetDataList((p) => p.SpoilID.Equals(spoilId)
                && p.BreakLvl.Equals(breakLevel), result);

            if (result.Count == 0)
                return 0;

            return result[0].AttributeGrow;
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
        /// 获取突破消耗
        /// </summary>
        /// <param name="spoilId"></param>
        /// <param name="spoilLevel"></param>
        /// <returns></returns>
        public int GetBreakCost(int spoilId)
        {
            var breakCount = GetSpoilBreakthroughLevel(spoilId);
            var costLevel = breakCount + 1;
            List<Configs.SpoilBreakUpData> result = new List<Configs.SpoilBreakUpData>();
            Configs.SpoilBreakUpCfg.GetDataList((p) => p.SpoilID.Equals(spoilId), result);
            if (costLevel >= result[0].BreakMaxlvl)
                return 0;
            return Formula.GetSpoilBreakthroughCost(spoilId, costLevel);
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
        /// 获取攻击加成(要加上突破的加成）
        /// </summary>
        /// <param name="spoilId"></param>
        /// <param name="spoilLevel"></param>
        /// <returns></returns>
        public float GetAtkEffect(int spoilId, int spoilLevel)
        {
            var cfg = Configs.SpoilCfg.GetData(spoilId);
            var breakLevel = GetSpoilBreakthroughLevel(spoilId);
            return cfg.HasAtkAdditionBase + spoilLevel * cfg.HasATKAdditionGrow  + GetBreakAtkEffect(spoilId, breakLevel);
        }

        /// <summary>
        /// 获取突破攻击属性
        /// </summary>
        /// <param name="spoilId"></param>
        /// <returns></returns>
        public float GetBreakAtkEffect(int spoilId, int breakLevel)
        {
            List<Configs.SpoilBreakUpData> result = new List<Configs.SpoilBreakUpData>();
            //var count = GetSpoilBreakthroughCount(spoilId);
            Configs.SpoilBreakUpCfg.GetDataList((p) => p.SpoilID.Equals(spoilId)
                && p.BreakLvl.Equals(breakLevel), result);

            if(result.Count == 0)
            {
                //Debug.LogError("没有找到spoilbreakupdata " + spoilId);
                return 0;
            }
            var breakEffect = result[0].HasAtkAdditionBase + breakLevel * result[0].HasATKAdditionGrow;

            return breakEffect;
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
            var breakLevel = GetSpoilBreakthroughLevel(spoilId);
            return cfg.HasHPAdditionBase + spoilLevel * cfg.HasHPAdditionGrow + GetBreakHpEffect(spoilId, breakLevel);
        }

        /// <summary>
        /// 获取突破HP加成
        /// </summary>
        /// <param name="spoilId"></param>
        /// <returns></returns>
        public float GetBreakHpEffect(int spoilId, int breakLevel)
        {
            List<Configs.SpoilBreakUpData> result = new List<Configs.SpoilBreakUpData>();
            //var count = GetSpoilBreakthroughCount(spoilId);

            Configs.SpoilBreakUpCfg.GetDataList((p) => p.SpoilID.Equals(spoilId)
                && p.BreakLvl.Equals(breakLevel), result);

            if (result.Count == 0)
            {
                //Debug.LogError("没有找到spoilbreakupdata " + spoilId);
                return 0;
            }
            var breakEffect = result[0].HasHPAdditionBase + breakLevel * result[0].HasHPAdditionGrow;

            return breakEffect;
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
        public bool DoesCostTrophyEnough(BigDouble needCost)
        {
            //判断战功是否足够
            return needCost <= GameDataManager.Ins.Trophy;
        }

        /// <summary>
        /// 突破石是否足够
        /// </summary>
        /// <param name="needCost"></param>
        /// <returns></returns>
        public bool DoesCostBreakOreEngough(int needCost)
        {
            return needCost <= GameDataManager.Ins.BreakOre;
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

            return spoil.Level >= GetMaxLevel(spoilId);
        }

        /// <summary>
        /// 是否能突破
        /// </summary>
        /// <returns></returns>
        public bool CanBreakthrough(SpoilData spoilData)
        {
            var spoilId = spoilData.SpoilId;  
            var level = spoilData.Level;
            var nextBreakLevel = GetNextBreakthroughLevel(spoilId);
            return level == nextBreakLevel ;
        }

        /// <summary>
        /// 是否已达最大突破次数
        /// </summary>
        /// <param name="spoilId"></param>
        /// <returns></returns>
        public bool IsMaxBreakthrough(int spoilId)
        {
            int maxBreakcount = 9;
            var count = GetSpoilBreakthroughLevel(spoilId);
            return count >= maxBreakcount;
        }

        /// <summary>
        ///获取下一个突破等级
        /// </summary>
        /// <param name="spoilId"></param>
        /// <returns></returns>
        public int GetNextBreakthroughLevel(int spoilId)
        {
            List<Configs.SpoilBreakUpData> result = new List<Configs.SpoilBreakUpData>();
            var breakLevel = GetSpoilBreakthroughLevel(spoilId);
            Configs.SpoilBreakUpCfg.GetDataList((p) => p.SpoilID.Equals(spoilId)
                && p.BreakLvl.Equals(breakLevel + 1), result);

            if (result.Count == 0)
                return 0;

            return result[0].Lvl;
        }



        #endregion

        public int GetMaxLevel(int spoilId)
        {
            return ConfigManager.Ins.m_SpoilLvlUpCfg.AllData.Keys.Count;

            //List<Configs.SpoilBreakUpData> result = new List<Configs.SpoilBreakUpData>();
            //Configs.SpoilBreakUpCfg.GetDataList((p) => p.SpoilID.Equals(spoilId) 
            // && p.BreakLvl.Equals(p.BreakMaxlvl), result);

            //if (result.Count == 0)
            //{
            //    Debug.LogError("没有找到spoilId" + spoilId);
            //    return 0;
            //}

            //return result[0].Lvl;
        }


        #region 业务接口

        /// <summary>
        /// 抽Spoil
        /// </summary>
        public void RequestSpoilDraw()
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
            if (!DoesCostTrophyEnough(needCost))
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
        public void RequestSpoilEquip(int spoilId)
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
        public void RequestSpoilUpgrade(int spoilId)
        {
            var spoilData = GetSpoil(spoilId);
            if (spoilData == null)
            {
                Debug.LogError("没有找到Spoil 无法升级 " + spoilId);
                // 发送ErrorCode
                return;
            }

            if(!CanBreakthrough(spoilData))
            {
                DoRequestSpoilUpgrade(spoilData);
            }
            else
            {
                DoRequestSpoilBreakthrough(spoilData);
            }
           
        }

        /// <summary>
        /// 强化
        /// </summary>
        /// <param name="spoilData"></param>
        void DoRequestSpoilUpgrade(SpoilData spoilData)
        {
            var spoilLevel = spoilData.Level;
            var spoilId = spoilData.SpoilId;

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
            if (!DoesCostTrophyEnough(needCost))
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

        /// <summary>
        /// 突破
        /// </summary>
        /// <param name="spoilData"></param>
        private void DoRequestSpoilBreakthrough(SpoilData spoilData)
        {
            //判断资源够不够
            if(!DoesCostBreakOreEngough(GetBreakCost(spoilData.SpoilId)))
            {
                EventManager.Call(LogicEvent.ShowTips, "突破石不足");
                return;
            }

            //判断是否已经满突破
            if(IsMaxBreakthrough(spoilData.SpoilId))
            {
                EventManager.Call(LogicEvent.ShowTips, "已达最大突破次数");
                return;
            }

            //向服务器请求突破
            NetworkManager.Ins.SendMsg(new C2S_SpoilBreakthrough()
            {
                SpoilId = spoilData.SpoilId
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

        public void OnSpoilBreakthrough(S2C_SpoilBreakthrough pMsg)
        {
            UpdateLocalSpoilBreakthroughData(pMsg.SpoilBreakthrough);

            EventManager.Call(LogicEvent.OnSpoilBreakthrough, pMsg.SpoilBreakthrough);
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

        private void UpdateLocalSpoilBreakthroughData(SpoilBreakthroughData spoilBreakthrough)
        {
            if(GetSpoilBreakthroughData(spoilBreakthrough.SpoilId) == null)
            {
                AddSpoilBreakthroughData(spoilBreakthrough);
            }
            else
            {
                UpdateSpoilBreakthroughData(spoilBreakthrough);
            }
        }

        #endregion
    }
}