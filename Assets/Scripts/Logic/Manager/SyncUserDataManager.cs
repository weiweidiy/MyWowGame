using System;
using Framework.EventKit;
using Framework.Extension;
using Logic.Common;
using Networks;
using UnityEngine;

namespace Logic.Manager
{
    /// <summary>
    /// 向服务器同步用户数据
    /// 当玩家数据产生变化时
    /// </summary>
    public class SyncUserDataManager : MonoBehaviour, ISingleton
    {
        private readonly EventGroup m_EventGroup = new EventGroup();

        private bool m_NeedSend;
        private float m_LastSendTime;
        
        public void OnSingletonInit()
        { 
            m_EventGroup.Register(LogicEvent.SyncUserData, OnSyncUserData);  //只要数据发生变化就会收到消息，1秒后发送，会有重复数据
        }

        void OnDestroy()
        {
            m_EventGroup.Release();
            //TODO: 退出游戏时发送一次数据
            // if (m_NeedSend)
            //     SendSyncUserData();
        }
        
        private void OnSyncUserData(int arg1, object arg2)
        {
            m_NeedSend = true;
            m_LastSendTime = Time.timeSinceLevelLoad + 1f;
        }

        private void Update()
        {
            if (!m_NeedSend)
                return;

            if (Time.timeSinceLevelLoad >= m_LastSendTime)
            {
                m_NeedSend = false;
                SendSyncUserData();
            }
        }

        private C2S_SyncPlayerData _Msg = new ();
        private void SendSyncUserData()
        {
            //Debug.Log("发送同步数据");
            _Msg.Init();
            NetworkManager.Ins.SendMsg(_Msg);
        }
    }
}