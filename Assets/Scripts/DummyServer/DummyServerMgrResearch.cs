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
                var researchData = DigResearchCfg.GetData(research.ResearchId);
                var researchLevel = research.ResearchLevel;
                var attributeId = researchData.ResearchAttrGroup;
                var attributeType = AttributeCfg.GetData(attributeId).Type;
                switch ((AttributeType)attributeType)
                {
                    case AttributeType.ATK:
                        researchATK += researchLevel * researchData.ResearchGrow;
                        break;
                    case AttributeType.HP:
                        researchHP += researchLevel * researchData.ResearchGrow;
                        break;
                    case AttributeType.HammerLimit:
                        researchHammerLimit += researchLevel * researchData.ResearchGrow;
                        break;
                    case AttributeType.MineObtainAmount:
                        researchMineObtainAmount += researchLevel * researchData.ResearchGrow;
                        break;
                    case AttributeType.HammerRecoverSpeed:
                        researchHammerRecoverSpeed += researchLevel * researchData.ResearchGrow;
                        break;
                    case AttributeType.ResearchSpeed:
                        researchSpeed += researchLevel * researchData.ResearchGrow;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            m_DB.m_ResearchEffectData.ResearchATK = researchATK / 100;
            m_DB.m_ResearchEffectData.ResearchHP = researchHP / 100;
            m_DB.m_ResearchEffectData.ResearchHammerLimit = researchHammerLimit;
            m_DB.m_ResearchEffectData.ResearchMineObtainAmount = researchMineObtainAmount / 100;
            m_DB.m_ResearchEffectData.ResearchHammerRecoverSpeed = researchHammerRecoverSpeed / 100;
            m_DB.m_ResearchEffectData.ResearchSpeed = researchSpeed / 100;
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
            var researchTimeCost = researchData.BaseCostTime + researchData.GrowCostTime * level;
            var researchSpeed = 1 + m_DB.m_ResearchEffectData.ResearchSpeed; //研究速度增加 %
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
            var researchId = pMsg.ResearchId;
            var researchLevel = 0;
            // 考古研究完成时间戳
            long researchTimeStamp = 0;
            var gameResearchData = m_DB.m_ResearchList.Find(pData =>
            {
                if (pData.ResearchId == researchId)
                {
                    researchLevel = pData.ResearchLevel;
                    pData.IsResearching = 1;
                    researchTimeStamp = GetResearchTimeStamp(researchId, researchLevel);
                    pData.ResearchTimeStamp = researchTimeStamp;
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
                        ResearchId = researchId, ResearchLevel = researchLevel, IsResearching = 1,
                        ResearchTimeStamp = researchTimeStamp,
                    });
            }

            // 考古研究消耗的考古矿石数量
            OnReduceMiningData((int)MiningType.CopperMine, -GetResearchMineCost(researchId, researchLevel));

            DummyDB.Save(m_DB);

            SendMsg(new S2C_UpdateResearchTime()
            {
                ResearchId = researchId,
                ResearchTimeStamp = researchTimeStamp,
            });
        }

        private void OnResearching(C2S_Researching pMsg)
        {
            //研究完成方式
            var researchCompleteType = (ResearchCompleteType)pMsg.ResearchCompleteType;

            if (researchCompleteType == ResearchCompleteType.TimeComplete)
            {
                var researchTimeStamp = m_DB.m_ResearchList.Find(pData => pData.ResearchId == pMsg.ResearchId)
                    .ResearchTimeStamp;
                if (TimeHelper.GetUnixTimeStamp() - researchTimeStamp < 0)
                {
                    return;
                }
            }

            var researchId = pMsg.ResearchId;
            var researchLevel = 0;
            var researchCostLevel = 0;
            var gameResearchData = m_DB.m_ResearchList.Find(pData =>
            {
                if (pData.ResearchId == researchId)
                {
                    researchCostLevel = pData.ResearchLevel;
                    pData.ResearchLevel++;
                    researchLevel = pData.ResearchLevel;
                    pData.IsResearching = 0; //已经完成研究
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
                ResearchId = researchId,
                ResearchLevel = researchLevel,
                ResearchList = m_DB.m_ResearchList,
                ResearchEffectData = m_DB.m_ResearchEffectData,
            });
        }
    }
}