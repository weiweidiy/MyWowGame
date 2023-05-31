using Networks;

namespace DummyServer
{
    public partial class DummyServerMgr
    {
        private void OnUpdateLockStoryData(C2S_UpdateLockStoryData pMsg)
        {
            var lockStoryList = m_DB.m_LockStoryList;
            var lockStoryData = pMsg.LockStoryData;

            if (!lockStoryList.Contains(lockStoryData))
            {
                m_DB.m_LockStoryList.Add(lockStoryData);
            }

            DummyDB.Save(m_DB);
        }
    }
}