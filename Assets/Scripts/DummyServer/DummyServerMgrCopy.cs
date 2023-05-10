using BreakInfinity;
using Configs;
using Logic.Common;
using Logic.Manager;
using Networks;
using System;
using System.Collections.Generic;

namespace DummyServer
{
    public partial class DummyServerMgr
    {
        // 第一次进游戏 初始化副本数据
        public void InitCopy(DummyDB pDB)
        {
            pDB.m_DiamondCopyData = new GameCopyData();
            pDB.m_CoinCopyData = new GameCopyData();
            pDB.m_OilCopyData = new GameCopyOilData();
        }

        #region 消息处理

        public void On_C2S_EnterCopy(C2S_EnterCopy pMsg)
        {
            var _Msg = new S2C_EnterCopy
            {
                m_LevelType = pMsg.m_LevelType
            };
            SendMsg(_Msg);
        }
        
        public void On_C2S_ExitCopy(C2S_ExitCopy pMsg)
        {
            var _Msg = new S2C_ExitCopy
            {
                m_LevelType = pMsg.m_LevelType
            };
            if (pMsg.m_LevelType == (int)LevelType.DiamondCopy)
            {
                m_DB.m_DiamondCopyData.m_KeyCount--;
                m_DB.m_DiamondCopyData.m_Level++;
                _Msg.m_KeyCount = m_DB.m_DiamondCopyData.m_KeyCount;
                _Msg.m_Level = m_DB.m_DiamondCopyData.m_Level;
                _Msg.m_RewardCount = CopyManager.Ins.GetCopyDiamondReward(m_DB.m_DiamondCopyData.m_Level - 1);
                m_DB.m_Diamond += _Msg.m_RewardCount;
                SendMsg(new S2C_DiamondUpdate(){m_Diamond = m_DB.m_Diamond});
            }
            else if (pMsg.m_LevelType == (int)LevelType.CoinCopy) //金币在客户端更新
            {
                m_DB.m_CoinCopyData.m_KeyCount--;
                m_DB.m_CoinCopyData.m_Level++;
                _Msg.m_KeyCount = m_DB.m_CoinCopyData.m_KeyCount;
                _Msg.m_Level = m_DB.m_CoinCopyData.m_Level;
                //_Msg.m_RewardCount = 100;
            }
            else if (pMsg.m_LevelType == (int)LevelType.OilCopy)
            {
                //更新原油副本相关数据
                if(BigDouble.Parse(pMsg.m_CurTotalDamage) > BigDouble.Parse(m_DB.m_OilCopyData.m_BestDamageRecord))
                {
                    m_DB.m_OilCopyData.m_BestDamageRecord = pMsg.m_CurTotalDamage;
                }
                if(pMsg.m_CurBossLevel > m_DB.m_OilCopyData.m_BestLevelRecord)
                {
                    m_DB.m_OilCopyData.m_BestLevelRecord = pMsg.m_CurBossLevel;
                }

                m_DB.m_OilCopyData.m_KeyCount--;
                _Msg.m_KeyCount = m_DB.m_OilCopyData.m_KeyCount;
                _Msg.m_Level = m_DB.m_OilCopyData.m_Level;
                _Msg.m_CurBossLevel = m_DB.m_OilCopyData.m_BestLevelRecord;
                _Msg.m_CurTotalDamage = m_DB.m_OilCopyData.m_BestDamageRecord;


                //原油结算奖励             
                var copyCfg = CopyOilCfg.GetData(pMsg.m_CurBossLevel);
                if (copyCfg == null)
                    copyCfg = CopyOilCfg.GetData(0);

                int oilValue = copyCfg.RewardOilBase + pMsg.m_CurBossLevel * (copyCfg.RewardOilGrow + copyCfg.RewardOilExp);
                //更新DB
                UpdateOil(oilValue);

                SendMsg(new S2C_OilUpdate() { m_Oil = oilValue });
                


              //装备道具结算奖励               
                var tuple = GetOilCopyRewards(pMsg.m_CurBossLevel);
                for (int i = 0; i < tuple.Item1.Count; i++)
                {
                    //更新DB
                    UpdateEquipData(tuple.Item1[i], tuple.Item2[i]);
                }

                SendMsg(new S2C_OilCopyReward() {m_Oil = oilValue, m_LstRewardId = tuple.Item1, m_LstRewardCount = tuple.Item2 });
                

            }

            DummyDB.Save(m_DB);
            SendMsg(_Msg);
        }
        
        public void On_C2S_UpdateCopyKeyCount(C2S_UpdateCopyKeyCount pMsg)
        {
            if (m_DB.m_DiamondCopyData.m_KeyCount < 2)
                m_DB.m_DiamondCopyData.m_KeyCount = 2;
            if (m_DB.m_CoinCopyData.m_KeyCount < 2)
                m_DB.m_CoinCopyData.m_KeyCount = 2;
            if (m_DB.m_OilCopyData.m_KeyCount < 2)
                m_DB.m_OilCopyData.m_KeyCount = 2;

            DummyDB.Save(m_DB);
            SendMsg(new S2C_UpdateCopyKeyCount
            {
                m_CoinKeyCount = m_DB.m_CoinCopyData.m_KeyCount,
                m_DiamondKeyCount = m_DB.m_DiamondCopyData.m_KeyCount,
                m_OilKeyCount = m_DB.m_OilCopyData.m_KeyCount
            }) ;
        }


        /// <summary>
        /// 生成宝箱组的奖励
        /// </summary>
        /// <param name="bossLevel"></param>
        /// <returns></returns>
        public Tuple<List<int>, List<int>> GetOilCopyRewards(int bossLevel)
        {
            // 获取groupId个数
            var groupIdList = new List<int>();
            for (var i = 1; i <= bossLevel; i++)
            {
                var data = CopyOilCfg.GetData(i);
                if (data == null)
                    data = CopyOilCfg.GetData(0);

                var groupId = data.RewardGroup;
                groupIdList.Add(groupId);
            }

            return BoxRewardGeneraterHelper.GenerateByGroups(groupIdList);
        }

        #endregion
    }
}