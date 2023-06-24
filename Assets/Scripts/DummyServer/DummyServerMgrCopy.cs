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
            pDB.m_TropyCopyData = new GameCopyData();
            pDB.m_ReformCopyData = new GameCopyData();
        }

        #region 消息处理

        public void On_C2S_EnterCopy(C2S_EnterCopy pMsg)
        {
            var _Msg = new S2C_EnterCopy
            {
                LevelType = pMsg.LevelType
            };
            SendMsg(_Msg);
        }
        
        public void On_C2S_ExitCopy(C2S_ExitCopy pMsg)
        {
            var _Msg = new S2C_ExitCopy
            {
                LevelType = pMsg.LevelType
            };
            if (pMsg.LevelType == (int)LevelType.DiamondCopy)
            {
                m_DB.m_DiamondCopyData.KeyCount--;
                m_DB.m_DiamondCopyData.Level++;
                _Msg.KeyCount = m_DB.m_DiamondCopyData.KeyCount;
                _Msg.Level = m_DB.m_DiamondCopyData.Level;
                _Msg.RewardCount = CopyManager.Ins.GetCopyDiamondReward(m_DB.m_DiamondCopyData.Level - 1);
                m_DB.m_Diamond += _Msg.RewardCount;
                SendMsg(new S2C_DiamondUpdate(){Diamond = m_DB.m_Diamond});
            }
            else if (pMsg.LevelType == (int)LevelType.CoinCopy) //金币在客户端更新
            {
                m_DB.m_CoinCopyData.KeyCount--;
                m_DB.m_CoinCopyData.Level++;
                _Msg.KeyCount = m_DB.m_CoinCopyData.KeyCount;
                _Msg.Level = m_DB.m_CoinCopyData.Level;
                //_Msg.m_RewardCount = 100;
            }
            else if (pMsg.LevelType == (int)LevelType.OilCopy)
            {
                //更新原油副本相关数据 
                //to do:服务器没有大数字库，不能解析，要改成客户端判断并同步数据,发过来的一定是最大的记录
                //if(BigDouble.Parse(pMsg.CurTotalDamage) > BigDouble.Parse(m_DB.m_OilCopyData.BestDamageRecord))
                //{
                //保存最高伤害记录
                m_DB.m_OilCopyData.BestDamageRecord = pMsg.CurTotalDamage;
                //}
                //if(pMsg.CurBossLevel > m_DB.m_OilCopyData.BestLevelRecord)
                //{
                    //保存最高等级记录
                m_DB.m_OilCopyData.BestLevelRecord = pMsg.CurBossLevel;
                //}

                m_DB.m_OilCopyData.KeyCount--;
                _Msg.KeyCount = m_DB.m_OilCopyData.KeyCount;
                _Msg.Level = m_DB.m_OilCopyData.Level;
                _Msg.CurBossLevel = m_DB.m_OilCopyData.BestLevelRecord;
                _Msg.CurTotalDamage = m_DB.m_OilCopyData.BestDamageRecord;


                //原油结算奖励             
                var copyCfg = CopyOilCfg.GetData(pMsg.CurBossLevel);
                if (copyCfg == null)
                    copyCfg = CopyOilCfg.GetData(0);

                int oilValue = copyCfg.RewardOilBase + pMsg.CurBossLevel * (copyCfg.RewardOilGrow + copyCfg.RewardOilExp);
                //更新DB
                UpdateOil(oilValue);

              //装备道具结算奖励，item1 = itemID  , item2 = 数量               
                var tuple = GetOilCopyRewards(pMsg.CurBossLevel);
                for (int i = 0; i < tuple.Item1.Count; i++)
                {
                    //更新DB
                    UpdateEquipData(tuple.Item1[i], tuple.Item2[i]);
                }

                SendMsg(new S2C_OilCopyReward() {Oil = oilValue, LstRewardId = tuple.Item1, LstRewardCount = tuple.Item2 });
            }
            else if(pMsg.LevelType == (int)LevelType.TrophyCopy)
            {
                //和金币副本一样，在客户端计算
                m_DB.m_TropyCopyData.KeyCount--;
                m_DB.m_TropyCopyData.Level++;
                _Msg.KeyCount = m_DB.m_TropyCopyData.KeyCount;
                _Msg.Level = m_DB.m_TropyCopyData.Level;
            }
            else if(pMsg.LevelType == (int)LevelType.ReformCopy)
            {
                _Msg.RewardCount = CopyManager.Ins.GetCopyReformReward(m_DB.m_ReformCopyData.Level);

                //如果是胜利的，则增加等级
                if (pMsg.IsWin)
                {
                    m_DB.m_ReformCopyData.Level++;                  
                }

                _Msg.Level = m_DB.m_ReformCopyData.Level;
                m_DB.m_ReformCopyData.KeyCount--;
                _Msg.KeyCount = m_DB.m_ReformCopyData.KeyCount;

                //科技点计算        
                long technologyPoint = _Msg.RewardCount;
                //更新科技点并保存数据库
                UpdateTechnologyPoint(technologyPoint);

                //获取选择的引擎列表
                var needToCreateCylinders = pMsg.LstEnginePartData;
                //服务器实现：创建引擎数据对象，并保存
                var lstCylinders = CreateAndSaveCylinders(needToCreateCylinders);

                //副本奖励消息
                SendMsg(new S2C_ReformCopyReward() { TechnologyPoint = technologyPoint,LstCylinders = lstCylinders });

            }

            DummyDB.Save(m_DB);
            SendMsg(_Msg);
        }

        private List<GameEnginePartData> CreateAndSaveCylinders(List<GameEnginePartData> needToCreateCylinders)
        {
            //这里只是简单的把客户端发送的数据返回给客户端，实际需要服务器创建新的实例，并且保存到数据库
            return needToCreateCylinders;
        }

        public void On_C2S_UpdateCopyKeyCount(C2S_UpdateCopyKeyCount pMsg)
        {
            if (m_DB.m_DiamondCopyData.KeyCount < 2)
                m_DB.m_DiamondCopyData.KeyCount = 2;
            if (m_DB.m_CoinCopyData.KeyCount < 2)
                m_DB.m_CoinCopyData.KeyCount = 2;
            if (m_DB.m_OilCopyData.KeyCount < 2)
                m_DB.m_OilCopyData.KeyCount = 2;
            if (m_DB.m_TropyCopyData.KeyCount < 2)
                m_DB.m_TropyCopyData.KeyCount = 2;
            if (m_DB.m_ReformCopyData.KeyCount < 2)
                m_DB.m_ReformCopyData.KeyCount = 2;

            DummyDB.Save(m_DB);
            SendMsg(new S2C_UpdateCopyKeyCount
            {
                CoinKeyCount = m_DB.m_CoinCopyData.KeyCount,
                DiamondKeyCount = m_DB.m_DiamondCopyData.KeyCount,
                OilKeyCount = m_DB.m_OilCopyData.KeyCount,
                TrophyKeyCount = m_DB.m_TropyCopyData.KeyCount,
                ReformKeyCount = m_DB.m_ReformCopyData.KeyCount
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

            return GroupRewardGeneraterHelper.GenerateByGroups(groupIdList);
        }

        #endregion
    }
}