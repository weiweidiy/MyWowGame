using System.Collections.Generic;
using System.Linq;
using Framework.EventKit;
using Framework.Extension;
using Logic.Common;
using Logic.Config;
using Networks;

namespace Logic.Manager
{
    public class LockStoryManager : Singleton<LockStoryManager>
    {
        //开放所有功能
        public bool m_IsUnlockAll;

        public Dictionary<int, int> LockStoryMap { get; private set; }

        /// <summary>
        /// 初始化开放解锁数据
        /// </summary>
        /// <param name="lockStoryList"></param>
        public void Init(List<GameLockStoryData> lockStoryList)
        {
            LockStoryMap = new Dictionary<int, int>(64);

            foreach (var gameLockStoryData in lockStoryList)
            {
                LockStoryMap.Add(gameLockStoryData.LockType, gameLockStoryData.LockState);

                // 更新技能和伙伴可上阵数量
                SkillManager.Ins.UpdateCanDoOnCount(gameLockStoryData.LockType);
                PartnerManager.Ins.UpdateCanDoOnCount(gameLockStoryData.LockType);
            }
        }

        /// <summary>
        /// 获取当前关卡需要解锁的ID
        /// </summary>
        /// <param name="levelID"></param>
        /// <returns></returns>
        public List<int> GetUnLockID(long levelID)
        {
            var lockDic = ConfigManager.Ins.m_LockCfg.AllData;
            var unLockIDList = new List<int>();
            foreach (var lockData in lockDic)
            {
                if (lockData.Value.UnlockLvl == levelID)
                {
                    unLockIDList.Add(lockData.Value.ID);
                }
            }

            return unLockIDList;
        }

        /// <summary>
        /// 解锁开放
        /// </summary>
        /// <param name="levelID"></param>
        public void DoUnLock(long levelID)
        {
            var unLockIDList = GetUnLockID(levelID);
            if (unLockIDList.Count == 0) return;

            foreach (var unLockID in unLockIDList.Where(unLockID => !LockStoryMap.ContainsKey(unLockID)))
            {
                //向Map中添加解锁开放数据
                LockStoryMap.Add(unLockID, 1);
                //更新技能和伙伴可上阵数量
                SkillManager.Ins.UpdateCanDoOnCount(unLockID);
                PartnerManager.Ins.UpdateCanDoOnCount(unLockID);
                //通知进行解锁开放
                var data = ((LockType)unLockID, 1);
                EventManager.Call(LogicEvent.UpdateLockState, data);
                //向服务器发送已经解锁开放的数据进行存储
                NetworkManager.Ins.SendMsg(new C2S_UpdateLockStoryData()
                {
                    LockStoryData = new GameLockStoryData()
                    {
                        LockType = unLockID,
                        LockState = 1,
                    }
                });
            }
        }
    }
}