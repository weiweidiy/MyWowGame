using System.Collections.Generic;
using System.Linq;
using Configs;
using Framework.EventKit;
using Framework.Extension;
using Framework.UI;
using Logic.Common;
using Logic.UI.UIMain;
using Logic.UI.UIStory;
using Logic.UI.UIUser;
using Networks;

namespace Logic.Manager
{
    public class LockStoryManager : Singleton<LockStoryManager>
    {
        public bool m_IsUnlockAll;

        public Dictionary<int, int> LockStoryMap { get; private set; }

        /// <summary>
        /// 登录时获取游戏开放剧情解锁数据
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

        #region 消息发送

        /// <summary>
        /// TODO: 有好的优化方法时优化掉该写法
        /// 向服务器更新开放剧情解锁数据
        /// </summary>
        /// <param name="levelId"></param>
        public void DoLockStoryUpdate(long levelId)
        {
            //解锁所有开放功能
            if (m_IsUnlockAll)
            {
                return;
            }

            var listLockType = new List<LockType>();
            switch (levelId)
            {
                case 2:
                    listLockType.Add(LockType.LT_100);
                    break;
                case 3:
                    listLockType.Add(LockType.LT_200);
                    break;
                case 4:
                    listLockType.Add(LockType.LT_1800);
                    listLockType.Add(LockType.LT_1900);
                    listLockType.Add(LockType.LT_2700);
                    listLockType.Add(LockType.LT_3200);
                    listLockType.Add(LockType.LT_3300);
                    listLockType.Add(LockType.LT_3400);

                    if (!IsLockTypeUnlock(LockType.LT_1800))
                    {
                        UIUser.m_OpenType = UserPageType.Role;
                        UIBottomMenu.Ins.ClickBtn(BottomBtnType.User);
                    }

                    break;
                case 5:
                    listLockType.Add(LockType.LT_300);
                    UIBottomMenu.Ins.ClickBtn(BottomBtnType.Home);
                    break;
                case 6:
                    listLockType.Add(LockType.LT_400);
                    UIBottomMenu.Ins.ClickBtn(BottomBtnType.Home);
                    break;
                case 7:
                    listLockType.Add(LockType.LT_700);
                    listLockType.Add(LockType.LT_2800);
                    listLockType.Add(LockType.LT_3500);

                    if (!IsLockTypeUnlock(LockType.LT_700))
                    {
                        UIUser.m_OpenType = UserPageType.Skill;
                        UIBottomMenu.Ins.ClickBtn(BottomBtnType.User);
                    }

                    break;
                case 8:
                    listLockType.Add(LockType.LT_500);
                    UIBottomMenu.Ins.ClickBtn(BottomBtnType.Home);
                    break;
                case 9:
                    listLockType.Add(LockType.LT_600);
                    UIBottomMenu.Ins.ClickBtn(BottomBtnType.Home);
                    break;
                case 10:
                    listLockType.Add(LockType.LT_1300);
                    listLockType.Add(LockType.LT_2900);
                    listLockType.Add(LockType.LT_3600);

                    if (!IsLockTypeUnlock(LockType.LT_1300))
                    {
                        UIUser.m_OpenType = UserPageType.Partner;
                        UIBottomMenu.Ins.ClickBtn(BottomBtnType.User);
                    }

                    break;
                case 12:
                    listLockType.Add(LockType.LT_800);
                    break;
                case 14:
                    listLockType.Add(LockType.LT_2100);
                    listLockType.Add(LockType.LT_3100);

                    if (!IsLockTypeUnlock(LockType.LT_2100))
                    {
                        UIBottomMenu.Ins.ClickBtn(BottomBtnType.Copy);
                    }

                    break;
                case 17:
                    listLockType.Add(LockType.LT_1400);
                    break;
                case 19:
                    listLockType.Add(LockType.LT_2200);

                    if (!IsLockTypeUnlock(LockType.LT_2200))
                    {
                        UIBottomMenu.Ins.ClickBtn(BottomBtnType.Copy);
                    }

                    break;
                case 22:
                    listLockType.Add(LockType.LT_900);
                    break;
                case 26:
                    listLockType.Add(LockType.LT_1500);
                    break;
                case 30:
                    listLockType.Add(LockType.LT_2000);
                    listLockType.Add(LockType.LT_2600);
                    listLockType.Add(LockType.LT_2601);
                    listLockType.Add(LockType.LT_3000);

                    if (!IsLockTypeUnlock(LockType.LT_2600))
                    {
                        UIBottomMenu.Ins.ClickBtn(BottomBtnType.Special);
                    }

                    break;
                case 41:
                    listLockType.Add(LockType.LT_1000);
                    break;
                case 55:
                    listLockType.Add(LockType.LT_1600);
                    break;
                case 65:
                    listLockType.Add(LockType.LT_2300);
                    listLockType.Add(LockType.LT_3700);
                    break;
                case 71:
                    listLockType.Add(LockType.LT_1100);
                    break;
                case 73:
                    listLockType.Add(LockType.LT_2611);
                    break;
                case 85:
                    listLockType.Add(LockType.LT_1700);
                    break;
                case 90:
                    listLockType.Add(LockType.LT_2400);
                    listLockType.Add(LockType.LT_3800);
                    break;
                case 101:
                    listLockType.Add(LockType.LT_1200);
                    break;
                case 150:
                    listLockType.Add(LockType.LT_2500);
                    break;
            }

            // 如果listLockType中没有值则return
            if (listLockType.Count == 0)
            {
                return;
            }

            // 如何listLockType有值则进行判断和更新
            foreach (var lockTypeId in listLockType.Select(lockType => (int)lockType))
            {
                if (LockStoryMap.ContainsKey(lockTypeId)) return;
                LockStoryMap.Add(lockTypeId, 1);

                // 更新技能和伙伴可上阵数量
                SkillManager.Ins.UpdateCanDoOnCount(lockTypeId);
                PartnerManager.Ins.UpdateCanDoOnCount(lockTypeId);

                //解锁剧情
                UpdateStory(lockTypeId, 1);
            }
        }

        /// <summary>
        /// 开放
        /// </summary>
        /// <param name="lockTypeId"></param>
        /// <param name="lockStateId"></param>
        private void UpdateLock(int lockTypeId, int lockStateId)
        {
            /*
             * 解锁开放
             */
            var data = ((LockType)lockTypeId, lockStateId);
            EventManager.Call(LogicEvent.UpdateLockState, data);
        }

        /// <summary>
        /// 剧情
        /// </summary>
        /// <param name="lockTypeId"></param>
        /// <param name="lockStateId"></param>
        private async void UpdateStory(int lockTypeId, int lockStateId)
        {
            /*
             * TODO：
             * 解锁剧情
             * 当前默认取一个获取StoryID
             * 有多个StoryID时特殊处理
             */

            var storyId = GetLockData(lockTypeId).StoryID;

            if (storyId != 0)
            {
                await UIManager.Ins.OpenUI<UIStory>(storyId);
            }

            // 解锁开放
            UpdateLock(lockTypeId, lockStateId);


            NetworkManager.Ins.SendMsg(new C2S_UpdateLockStoryData()
            {
                LockStoryData = new GameLockStoryData()
                {
                    LockType = lockTypeId,
                    LockState = lockStateId,
                }
            });
        }

        #endregion

        #region 消息接收

        #endregion

        #region 通用

        /// <summary>
        /// 获取LockCfg表数据
        /// </summary>
        /// <param name="lId"></param>
        /// <returns></returns>
        public LockData GetLockData(int lId)
        {
            return LockCfg.GetData(lId);
        }

        /// <summary>
        /// 获取StoryCfg数据
        /// </summary>
        /// <param name="sId"></param>
        /// <returns></returns>
        public StoryData GetStoryData(int sId)
        {
            return StoryCfg.GetData(sId);
        }

        /// <summary>
        /// 判断该功能是否开放解锁
        /// </summary>
        /// <param name="lockType"></param>
        /// <returns></returns>
        public bool IsLockTypeUnlock(LockType lockType)
        {
            var lockTypeId = (int)lockType;

            if (!LockStoryMap.ContainsKey(lockTypeId)) return false;
            return LockStoryMap[lockTypeId] == 1;
        }

        #endregion
    }
}