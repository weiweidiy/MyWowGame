using Framework.EventKit;
using Framework.Extension;
using Logic.Common;
using Networks;
using System.Collections.Generic;

namespace Logic.Manager
{
    /// <summary>
    /// 放置奖励管理类
    /// </summary>
    public class RewardManager : Singleton<RewardManager>
    {
        #region 宝箱奖励

        public void SendMsgC2SMiningReward(MiningType itemType)
        {
            var pMsg = new C2S_MiningReward()
            {
                m_TreasureType = itemType,
            };
            NetworkManager.Ins.SendMsg(pMsg);
        }

        public void On_S2C_MiningReward(S2C_MiningReward pMsg)
        {
            (MiningType, int, int) data = (pMsg.m_TreasureType, pMsg.m_RewardId, pMsg.m_RewardCount);
            EventManager.Call(LogicEvent.ShowMiningReward, data);
        }

        #endregion

        #region 放置奖励

        public void SendMsgC2SPlaceReward(int count)
        {
            var pMsg = new C2S_PlaceReward()
            {
                m_PlaceRewardCount = count,
            };
            NetworkManager.Ins.SendMsg(pMsg);
        }

        public void On_S2C_PlaceReward(S2C_PlaceReward pMsg)
        {
            (List<int>, List<int>) data = (pMsg.m_PlaceRewardId, pMsg.m_PlaceRewardCount);
            EventManager.Call(LogicEvent.ShowPlaceReward, data);
        }

        public void SendMsgC2SGetPlaceReward()
        {
            var pMsg = new C2S_GetPlaceReward()
            {
            };
            NetworkManager.Ins.SendMsg(pMsg);
        }

        #endregion

        #region 通用奖励

        public void SendMsgCommonReward()
        {
            NetworkManager.Ins.SendMsg(new C2S_CommonReward());
        }

        public void On_S2C_CommonReward(S2C_CommonReward pMsg)
        {
            (List<int>, List<int>) data = (pMsg.m_CommonRewardId, pMsg.m_CommonRewardCount);
            EventManager.Call(LogicEvent.ShowCommonReward, data);
        }

        #endregion

        public void On_S2C_OilCopyReward(S2C_OilCopyReward pMsg)
        {
            //(List<int>, List<int>) data = (pMsg.m_LstRewardId, pMsg.m_LstRewardCount);
            EventManager.Call(LogicEvent.ShowOilCopyRewards, pMsg);
        }
    }
}