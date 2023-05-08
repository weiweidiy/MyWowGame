using Logic.Common;
using Networks;

namespace DummyServer
{
    public partial class DummyServerMgr
    {
        #region 考古数据

        /// <summary>
        /// 初始化考古数据
        /// 更新将要获取齿轮的ID
        /// </summary>
        /// <param name="pDB"></param>
        public void InitMining(DummyDB pDB)
        {
            pDB.m_MiningData = new GameMiningData();
            UpdateToGetEngine();
        }

        /// <summary>
        /// 更新获取引擎所需要的齿轮数量
        /// </summary>
        /// <param name="pGear"></param>
        public void UpdateGear(int pGear)
        {
            if (m_DB.m_EngineList.Count >= GameDefine.MaxEngineCount + 1)
            {
                // TODO:通知客户端引擎已达到拥有上限，无法添加新的引擎数据
                return;
            }

            m_DB.m_MiningData.m_GearCount += pGear;
            if (m_DB.m_MiningData.m_GearCount >= GetEngineCostGear())
            {
                m_DB.m_MiningData.m_GearCount -= GetEngineCostGear();
                UpdateGetEngine();
            }
        }

        #endregion

        #region 消息处理

        /// <summary>
        /// 考古道具减少
        /// </summary>
        /// <param name="miningDataType"></param>
        /// <param name="count"></param>
        public void OnReduceMiningData(int miningDataType, int count = -1)
        {
            switch ((MiningType)miningDataType)
            {
                case MiningType.Hammer:
                    m_DB.m_MiningData.m_HammerCount += count;
                    break;
                case MiningType.Bomb:
                    m_DB.m_MiningData.m_BombCount += count;
                    break;
                case MiningType.Scope:
                    m_DB.m_MiningData.m_ScopeCount += count;
                    break;
            }

            SendS2CUpdateMiningData(miningDataType);
        }

        /// <summary>
        /// 考古道具增加
        /// </summary>
        /// <param name="miningDataType"></param>
        /// <param name="count"></param>
        public void OnIncreaseMiningData(int miningDataType, int count = 1)
        {
            switch ((MiningType)miningDataType)
            {
                case MiningType.Gear:
                    UpdateGear(20); //TODO: Delete测试使用
                    // UpdateGear(count);
                    break;
                case MiningType.CopperMine:
                case MiningType.SilverMine:
                case MiningType.GoldMine:
                    m_DB.m_MiningData.m_MineCount += count;
                    break;
                case MiningType.Hammer:
                    m_DB.m_MiningData.m_HammerCount += count;
                    break;
                case MiningType.Bomb:
                    m_DB.m_MiningData.m_BombCount += count;
                    break;
                case MiningType.Scope:
                    m_DB.m_MiningData.m_ScopeCount += count;
                    break;
                case MiningType.Door:
                    m_DB.m_MiningData.m_FloorCount += count;
                    break;
            }

            SendS2CUpdateMiningData(miningDataType);
        }

        private void SendS2CUpdateMiningData(int miningDataType)
        {
            SendMsg(new S2C_UpdateMiningData()
            {
                m_MiningDataType = miningDataType,
                m_MiningData = m_DB.m_MiningData,
            });

            DummyDB.Save(m_DB);
        }

        #endregion
    }
}