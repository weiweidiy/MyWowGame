using Logic.Common;
using Logic.Manager;
using Networks;

namespace DummyServer
{
    public partial class DummyServerMgr
    {
        // 第一次进游戏 初始化副本数据
        public void InitCopy(DummyDB pDB)
        {
            pDB.m_DiamondCopyData = new GameCopyData();
            pDB.m_CoinCopyData = new GameCopyData();
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
            else if (pMsg.m_LevelType == (int)LevelType.CoinCopy)
            {
                m_DB.m_CoinCopyData.m_KeyCount--;
                m_DB.m_CoinCopyData.m_Level++;
                _Msg.m_KeyCount = m_DB.m_CoinCopyData.m_KeyCount;
                _Msg.m_Level = m_DB.m_CoinCopyData.m_Level;
                //_Msg.m_RewardCount = 100;
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

            DummyDB.Save(m_DB);
            SendMsg(new S2C_UpdateCopyKeyCount
            {
                m_CoinKeyCount = m_DB.m_CoinCopyData.m_KeyCount, 
                m_DiamondKeyCount = m_DB.m_DiamondCopyData.m_KeyCount
            });
        }

        #endregion
    }
}