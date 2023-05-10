using System;
using Configs;
using Framework.Helper;
using Logic.Common;
using Networks;

namespace DummyServer
{
    public partial class DummyServerMgr
    {
        //初始化创建研究属性数据
        public void InitResearch(DummyDB pDB)
        {
            pDB.m_ResearchEffectData = new GameResearchEffectData();
        }

        /// <summary>
        /// 更新所有已完成研究的属性加成数据
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void UpdateAllResearchCompleteEffect()
        {
            /*
             * 攻击力增加 %
             * 体力增加 %
             * 矿锤拥有上限增加
             * 矿石获得量增加 %
             * 矿锤补充速度增加 %
             * 研究速度增加 %
             */
            float researchATK = 0;
            float researchHP = 0;
            float researchHammerLimit = 0;
            float researchMineObtainAmount = 0;
            float researchHammerRecoverSpeed = 0;
            float researchSpeed = 0;

            foreach (var research in m_DB.m_ResearchList)
            {
                var researchData = DigResearchCfg.GetData(research.m_ResearchId);
                var researchlevel = research.m_ResearchLevel;
                var attributeId = researchData.ResearchAttrGroup;
                switch ((ResearchType)attributeId)
                {
                    case ResearchType.None:
                        break;
                    case ResearchType.IncreaseATK:
                        researchATK += researchlevel * researchData.ResearchGrow;
                        break;
                    case ResearchType.IncreaseHP:
                        researchHP += researchlevel * researchData.ResearchGrow;
                        break;
                    case ResearchType.IncreaseHammerLimit:
                        researchHammerLimit += researchlevel * researchData.ResearchGrow;
                        break;
                    case ResearchType.IncreaseMineObtainAmount:
                        researchMineObtainAmount += researchlevel * researchData.ResearchGrow;
                        break;
                    case ResearchType.IncreaseHammerRecoverSpeed:
                        researchHammerRecoverSpeed += researchlevel * researchData.ResearchGrow;
                        break;
                    case ResearchType.IncreaseResearchSpeed:
                        researchSpeed += researchlevel * researchData.ResearchGrow;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            m_DB.m_ResearchEffectData.researchATK = researchATK / 100;
            m_DB.m_ResearchEffectData.researchHP = researchHP / 100;
            m_DB.m_ResearchEffectData.researchHammerLimit = researchHammerLimit;
            m_DB.m_ResearchEffectData.researchMineObtainAmount = researchMineObtainAmount / 100;
            m_DB.m_ResearchEffectData.researchHammerRecoverSpeed = researchHammerRecoverSpeed / 100;
            m_DB.m_ResearchEffectData.researchSpeed = researchSpeed / 100;
        }

        //考古研究矿石消耗
        private int GetResearchMineCost(int id, int level)
        {
            var researchData = DigResearchCfg.GetData(id);
            return researchData.BaseCost + researchData.GrowCost * level;
        }

        //考古研究时间消耗
        private float GetResearchTimeCost(int id, int level)
        {
            var researchData = DigResearchCfg.GetData(id);
            var researchTimeCost = researchData.BaseCostTime + researchData.BaseCostTime * level;
            var researchSpeed = 1 + m_DB.m_ResearchEffectData.researchSpeed; //研究速度增加 %
            return researchTimeCost / researchSpeed;
        }

        //考古研究钻石加速消耗
        private int GetResearchDiamondCost(int id, int level)
        {
            var cost = GetResearchTimeCost(id, level) / 60;
            return (int)(MathF.Ceiling(cost) * GameDefine.ResearchDiamondCost);
        }

        /// <summary>
        /// 获取考古研究完成时间戳
        /// </summary>
        /// <param name="id"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        private long GetResearchTimeStamp(int id, int level)
        {
            return TimeHelper.GetUnixTimeStamp() + (long)GetResearchTimeCost(id, level);
        }

        private void OnUpdateResearchTime(C2S_UpdateResearchTime pMsg)
        {
            var researchId = pMsg.m_ResearchId;
            var researchLevel = 0;
            // 考古研究完成时间戳
            long researchTimeStamp = 0;
            var gameResearchData = m_DB.m_ResearchList.Find(pData =>
            {
                if (pData.m_ResearchId == researchId)
                {
                    researchLevel = pData.m_ResearchLevel;
                    pData.m_IsResearching = 1;
                    researchTimeStamp = GetResearchTimeStamp(researchId, researchLevel);
                    pData.m_researchTimeStamp = researchTimeStamp;
                    return true;
                }

                return false;
            });

            if (gameResearchData == null)
            {
                researchLevel = 0;
                researchTimeStamp = GetResearchTimeStamp(researchId, researchLevel);
                m_DB.m_ResearchList.Add(
                    new GameResearchData()
                    {
                        m_ResearchId = researchId, m_ResearchLevel = researchLevel, m_IsResearching = 1,
                        m_researchTimeStamp = researchTimeStamp,
                    });
            }

            // 考古研究消耗的考古矿石数量
            OnReduceMiningData((int)MiningType.CopperMine, -GetResearchMineCost(researchId, researchLevel));

            DummyDB.Save(m_DB);

            SendMsg(new S2C_UpdateResearchTime()
            {
                m_ResearchId = researchId,
                m_ResearchTimeStamp = researchTimeStamp,
            });
        }

        private void OnResearching(C2S_Researching pMsg)
        {
            //研究完成方式
            var researchCompleteType = (ResearchCompleteType)pMsg.m_ResearchCompleteType;

            if (researchCompleteType == ResearchCompleteType.TimeComplete)
            {
                var researchTimeStamp = m_DB.m_ResearchList.Find(pData => pData.m_ResearchId == pMsg.m_ResearchId)
                    .m_researchTimeStamp;
                if (TimeHelper.GetUnixTimeStamp() - researchTimeStamp < 0)
                {
                    return;
                }
            }

            var researchId = pMsg.m_ResearchId;
            var researchLevel = 0;
            var researchCostLevel = 0;
            var gameResearchData = m_DB.m_ResearchList.Find(pData =>
            {
                if (pData.m_ResearchId == researchId)
                {
                    researchCostLevel = pData.m_ResearchLevel;
                    pData.m_ResearchLevel++;
                    researchLevel = pData.m_ResearchLevel;
                    pData.m_IsResearching = 0; //已经完成研究
                    return true;
                }

                return false;
            });

            if (researchCompleteType == ResearchCompleteType.DiamondComplete)
            {
                // 研究钻石消耗
                var cost = GetResearchDiamondCost(researchId, researchCostLevel);
                UpdateDiamond(-cost);
            }

            //更新研究属性
            UpdateAllResearchCompleteEffect();

            DummyDB.Save(m_DB);

            SendMsg(new S2C_Researching()
            {
                m_ResearchId = researchId,
                m_ResearchLevel = researchLevel,
                m_ResearchList = m_DB.m_ResearchList,
                m_ResearchEffectData = m_DB.m_ResearchEffectData,
            });
        }
    }
}