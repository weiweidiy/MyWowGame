using System.Linq;
using Configs;
using Networks;

namespace DummyServer
{
    public partial class DummyServerMgr
    {
        public void OnQuenchingLock(C2S_QuenchingLock pMsg)
        {
            //更新属性锁定状态
            var qId = pMsg.QuenchingId;
            var unLockType = pMsg.QuenchingUnLockType;

            foreach (var quenchingData in m_DB.m_QuenchingList.Where(
                         quenchingData => quenchingData.QuenchingId == qId))
            {
                quenchingData.UnlockType = unLockType;
            }

            DummyDB.Save(m_DB);
        }

        public void OnQuenching(C2S_Quenching pMsg)
        {
            // 更新并保存属性列表
            m_DB.m_QuenchingList = pMsg.QuenchingList;

            //更新原油
            var lockCount = m_DB.m_QuenchingList.Count(quenchingData => quenchingData.UnlockType == 0);
            var djData = DJCfg.GetData(lockCount);
            UpdateOil(-djData.Cost);

            //存储
            DummyDB.Save(m_DB);
        }
    }
}