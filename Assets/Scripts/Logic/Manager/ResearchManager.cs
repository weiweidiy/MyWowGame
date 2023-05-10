using System.Collections.Generic;
using BreakInfinity;
using Configs;
using Framework.EventKit;
using Framework.Extension;
using Logic.Common;
using Logic.Data;
using Networks;

namespace Logic.Manager
{
    public class ResearchManager : Singleton<ResearchManager>
    {
        public Dictionary<int, GameResearchData> ResearchMap { get; private set; }

        //研究属性加成
        public float ResearchATK { get; private set; }
        public float ResearchHP { get; private set; }
        public float ResearchHammerLimit { get; private set; }
        public float ResearchMineObtainAmount { get; private set; }
        public float ResearchHammerRecoverSpeed { get; private set; }
        public float ResearchSpeed { get; private set; }

        public void Init(List<GameResearchData> researchList, GameResearchEffectData researchEffectData)
        {
            ResearchMap = new Dictionary<int, GameResearchData>(64);
            foreach (var gameResearchData in researchList)
            {
                ResearchMap.Add(gameResearchData.m_ResearchId, gameResearchData);
            }

            UpdateAllResearchCompleteEffect(researchEffectData);
        }

        #region 研究属性加成

        //研究攻击力增加
        public BigDouble GetResearchATKAdd()
        {
            return ResearchATK;
        }

        //研究体力增加
        public BigDouble GetResearchHPAdd()
        {
            return ResearchHP;
        }

        //研究矿锤拥有上限增加
        public float GetResearchHammerLimitAdd()
        {
            return ResearchHammerLimit;
        }

        //研究矿石获得量增加
        public float GetResearchMineObtainAmountAdd()
        {
            return ResearchMineObtainAmount;
        }

        //研究矿锤补充速度增加
        public float GetResearchHammerRecoverSpeedAdd()
        {
            return ResearchHammerRecoverSpeed;
        }

        //研究速度增加
        public float GetResearchSpeedAdd()
        {
            return ResearchSpeed;
        }

        #endregion

        #region 通用

        /// <summary>
        /// 获取DigResearch表数据
        /// </summary>
        /// <param name="rId"></param>
        /// <returns></returns>
        public DigResearchData GetResearchData(int rId)
        {
            return DigResearchCfg.GetData(rId);
        }

        /// <summary>
        /// 获取Attribute表数据
        /// </summary>
        /// <param name="aId"></param>
        /// <returns></returns>
        public AttributeData GetAttributeData(int aId)
        {
            return AttributeCfg.GetData(aId);
        }

        /// <summary>
        /// 获取研究数据
        /// </summary>
        /// <param name="researchId"></param>
        /// <returns></returns>
        public GameResearchData GetGameResearchData(int researchId)
        {
            return ResearchMap.TryGetValue(researchId, out var data) ? data : null;
        }

        /// <summary>
        /// 是否能够强化研究
        /// </summary>
        /// <param name="cost"></param>
        /// <returns></returns>
        public bool IsCanResearch(int cost)
        {
            return MiningManager.Ins.m_MiningData.m_MineCount >= cost;
        }

        /// <summary>
        /// 是否能够加速研究
        /// </summary>
        /// <param name="cost"></param>
        /// <returns></returns>
        public bool IsCanAccelerate(int cost)
        {
            return GameDataManager.Ins.Diamond >= cost;
        }

        /// <summary>
        /// 研究拥有加成更新
        /// </summary>
        private void UpdateAllResearchCompleteEffect(GameResearchEffectData researchEffectData)
        {
            ResearchATK = researchEffectData.researchATK;
            ResearchHP = researchEffectData.researchHP;
            ResearchHammerLimit = researchEffectData.researchHammerLimit;
            ResearchMineObtainAmount = researchEffectData.researchMineObtainAmount;
            ResearchHammerRecoverSpeed = researchEffectData.researchHammerRecoverSpeed;
            ResearchSpeed = researchEffectData.researchSpeed;
            EventManager.Call(LogicEvent.ResearchCompleteEffectUpdate);
        }

        #endregion

        #region 消息发送

        public void DoUpdateResearchTime(int pResearchId)
        {
            NetworkManager.Ins.SendMsg(new C2S_UpdateResearchTime()
            {
                m_ResearchId = pResearchId,
            });
        }

        public void DoResearching(int pResearchId, int researchCompleteType)
        {
            NetworkManager.Ins.SendMsg(new C2S_Researching()
            {
                m_ResearchId = pResearchId,
                m_ResearchCompleteType = researchCompleteType,
            });
        }

        #endregion

        #region 消息接受

        public void OnUpdateResearchTime(S2C_UpdateResearchTime pMsg)
        {
            var data = (pMsg.m_ResearchId, pMsg.m_ResearchTimeStamp);
            EventManager.Call(LogicEvent.OnUpdateResearchTime, data);
        }

        public void OnResearching(S2C_Researching pMsg)
        {
            foreach (var gameResearchData in pMsg.m_ResearchList)
            {
                //更新研究数据
                var data = GetGameResearchData(gameResearchData.m_ResearchId);
                if (data != null)
                {
                    data.m_ResearchId = gameResearchData.m_ResearchId;
                    data.m_ResearchLevel = gameResearchData.m_ResearchLevel;
                    data.m_IsResearching = gameResearchData.m_IsResearching;
                    data.m_researchTimeStamp = gameResearchData.m_researchTimeStamp;
                }
                else
                {
                    ResearchMap.Add(gameResearchData.m_ResearchId, gameResearchData);
                }
            }

            UpdateAllResearchCompleteEffect(pMsg.m_ResearchEffectData);

            var eventData = (pMsg.m_ResearchId, pMsg.m_ResearchLevel);
            EventManager.Call(LogicEvent.OnResearching, eventData);
        }

        #endregion
    }
}