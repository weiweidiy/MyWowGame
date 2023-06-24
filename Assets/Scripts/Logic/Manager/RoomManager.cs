using Framework.EventKit;
using Framework.Extension;
using Logic.Common;
using Logic.Config;
using Logic.UI.Cells;
using Logic.UI.UIMain.Room;
using Networks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Logic.Manager
{
    public class RoomManager : Singleton<RoomManager>
    {
        /// <summary>
        /// 已解锁的房间
        /// </summary>
        HashSet<int> unlockedRooms = new HashSet<int>();

        /// <summary>
        /// 解锁room映射表
        /// </summary>
        Dictionary<LockType, int> lockMap = new Dictionary<LockType, int>();

        private EventGroup m_EventGroup = new();

        public void Init(S2C_Login pMsg)
        {
            InitLockMap();

            //解锁数据（删选出room相关的解锁数据)
            foreach (var data in pMsg.LockStoryList)
            {
                var lockType = (LockType)data.LockType;
                var lockState = data.LockState;

                //如果是room类型，则修改数据
                if (IsRoomType(lockType))
                {
                    //添加到room映射
                    int roomId = GetRoomIdByLockType(lockType);
                    unlockedRooms.Add(roomId);
                }
            }

            //解锁消息
            m_EventGroup.Register(LogicEvent.UpdateLockState, (i, o) =>
            {
                var (lockType, lockState) = (ValueTuple<LockType, int>)o;
                if (lockState != 1) return;

                // Debug.LogError("lockType " + lockType + " lockState" + lockState);
                if (IsRoomType(lockType))
                {
                    //添加到room映射
                    int roomId = GetRoomIdByLockType(lockType);
                    unlockedRooms.Add(roomId);
                    EventManager.Call(LogicEvent.RoomUnlocked, roomId);
                }

            });

            //GM指令，临时全部打开
            m_EventGroup.Register(LogicEvent.UpdateUnlockAll, (i, o) =>
            {
                foreach(var key in lockMap.Keys)
                {
                    unlockedRooms.Add(lockMap[key]);
                    EventManager.Call(LogicEvent.RoomUnlocked, lockMap[key]);
                }

            });
        }

        /// <summary>
        /// 初始化room 和 lockType映射表
        /// </summary>
        void InitLockMap()
        {
            var keys = ConfigManager.Ins.m_RoomCfg.AllData.Keys;
            foreach (var key in keys)
            {
                var cfg = ConfigManager.Ins.m_RoomCfg.AllData[key];
                lockMap.Add((LockType)cfg.LockID, cfg.ID);
            }
        }

        /// <summary>
        /// 是否是房间解锁类型
        /// </summary>
        /// <param name="lockType"></param>
        /// <returns></returns>
        bool IsRoomType(LockType lockType)
        {
            return lockMap.ContainsKey(lockType);
        }



        /// <summary>
        /// 根据解锁等级获取roomId
        /// </summary>
        /// <param name="lockType"></param>
        /// <returns></returns>
        private int GetRoomIdByLockType(LockType lockType)
        {
            return lockMap[lockType];
        }

        public HashSet<int> GetUnlockedRooms()
        {
            return unlockedRooms;
        }
    }
}