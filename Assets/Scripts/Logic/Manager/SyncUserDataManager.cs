using System;
using System.Collections.Generic;
using Framework.EventKit;
using Framework.Extension;
using Logic.Common;
using Logic.Data;
using Networks;
using UnityEngine;

namespace Logic.Manager
{
    public enum SyncDataType
    {
        CoinData,
        RoomData,
        SettingData,
        LevelData,
        PlaceReward,
        TrophyData,
    }
    
    /// <summary>
    /// 当玩家数据产生变化时 向服务器同步用户数据
    /// 主要是客户端 游戏币 相关 产生的数据变化
    /// </summary>
    public class SyncUserDataManager : MonoBehaviour, ISingleton
    {
        private readonly EventGroup m_EventGroup = new EventGroup();

        private bool m_NeedSend;
        private float m_LastSendTime;

        private readonly UpdateCell CoinDataCell = new (6);
        private readonly UpdateCell TrophyDataCell = new(6);
        private readonly UpdateCell RoomDataCell = new (3);
        private readonly UpdateCell SettingDataCell = new (5);
        private readonly UpdateCell LevelDataCell = new (5f);
        private readonly UpdateCell PlaceRewardCell = new (5f);
        
        public void OnSingletonInit()
        { 
            m_EventGroup.Register(LogicEvent.SyncUserData, OnSyncUserData);
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
            var _Type = (SyncDataType) arg2;
            switch (_Type)
            {
                case SyncDataType.CoinData:
                    if (!CoinDataCell.NeedSend)
                    {
                        CoinDataCell.NeedSend = true;
                        CoinDataCell.SendTime = CoinDataCell.TimeThreshold + Time.realtimeSinceStartup;
                    }
                    break;
                case SyncDataType.TrophyData:
                    if (!TrophyDataCell.NeedSend)
                    {
                        TrophyDataCell.NeedSend = true;
                        TrophyDataCell.SendTime = TrophyDataCell.TimeThreshold + Time.realtimeSinceStartup;
                    }
                    break;
                case SyncDataType.RoomData:
                    if (!RoomDataCell.NeedSend)
                    {
                        RoomDataCell.NeedSend = true;
                        RoomDataCell.SendTime = RoomDataCell.TimeThreshold + Time.realtimeSinceStartup;
                    }
                    break;
                case SyncDataType.SettingData:
                    if (!SettingDataCell.NeedSend)
                    {
                        SettingDataCell.NeedSend = true;
                        SettingDataCell.SendTime = SettingDataCell.TimeThreshold + Time.realtimeSinceStartup;
                    }
                    break;
                case SyncDataType.LevelData:
                    if (!LevelDataCell.NeedSend)
                    {
                        LevelDataCell.NeedSend = true;
                        LevelDataCell.SendTime = LevelDataCell.TimeThreshold + Time.realtimeSinceStartup;
                    }
                    break;
                case SyncDataType.PlaceReward:
                    if (!PlaceRewardCell.NeedSend)
                    {
                        PlaceRewardCell.NeedSend = true;
                        PlaceRewardCell.SendTime = PlaceRewardCell.TimeThreshold + Time.realtimeSinceStartup;
                    }
                    break;
            }
        }

        float _FrameCont;
        private void Update()
        {
            _FrameCont++;
            if (_FrameCont % 15 != 0)
                return;

            _FrameCont = 0;
            var _Time = Time.realtimeSinceStartup;
            
            if (CoinDataCell.NeedSend && _Time > CoinDataCell.SendTime)
            {
                CoinDataCell.NeedSend = false;
                _SyncCoinMsg.Coin = GameDataManager.Ins.Coin.ToString();
                NetworkManager.Ins.SendMsg(_SyncCoinMsg);
            }

            if (TrophyDataCell.NeedSend && _Time > TrophyDataCell.SendTime)
            {
                TrophyDataCell.NeedSend = false;
                _SyncTrophyMsg.Trophy = GameDataManager.Ins.Trophy.ToString();
                NetworkManager.Ins.SendMsg(_SyncTrophyMsg);
            }

            if (RoomDataCell.NeedSend && _Time > RoomDataCell.SendTime)
            {
                RoomDataCell.NeedSend = false;
                _SyncRoomDataMsg.Init();
                NetworkManager.Ins.SendMsg(_SyncRoomDataMsg);
            }
            
            if (SettingDataCell.NeedSend && _Time > SettingDataCell.SendTime)
            {
                SettingDataCell.NeedSend = false;
                _SyncSettingDataMsg.Init();
                NetworkManager.Ins.SendMsg(_SyncSettingDataMsg);
            }
            
            if (LevelDataCell.NeedSend && _Time > LevelDataCell.SendTime)
            {
                LevelDataCell.NeedSend = false;
                _SyncLevelDataMsg.Init();
                NetworkManager.Ins.SendMsg(_SyncLevelDataMsg);
            }
            
            if (PlaceRewardCell.NeedSend && _Time > PlaceRewardCell.SendTime)
            {
                PlaceRewardCell.NeedSend = false;
                _SyncPlaceRewardDataMsg.Init();
                NetworkManager.Ins.SendMsg(_SyncPlaceRewardDataMsg);
            }
        }

        private readonly C2S_SyncCoin _SyncCoinMsg = new ();
        private readonly C2S_SyncRoomData _SyncRoomDataMsg = new ();
        private readonly C2S_SyncSettingData _SyncSettingDataMsg = new ();
        private readonly C2S_SyncLevelData _SyncLevelDataMsg = new ();
        private readonly C2S_SyncPlaceRewardData _SyncPlaceRewardDataMsg = new ();
        private readonly C2S_SyncTrophy _SyncTrophyMsg = new();

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                if (RoomDataCell.NeedSend)
                {
                    RoomDataCell.NeedSend = false;
                    _SyncRoomDataMsg.Init();
                    NetworkManager.Ins.SendMsg(_SyncRoomDataMsg);
                }
            
                if (SettingDataCell.NeedSend)
                {
                    SettingDataCell.NeedSend = false;
                    _SyncSettingDataMsg.Init();
                    NetworkManager.Ins.SendMsg(_SyncSettingDataMsg);
                }
            
                if (LevelDataCell.NeedSend)
                {
                    LevelDataCell.NeedSend = false;
                    _SyncLevelDataMsg.Init();
                    NetworkManager.Ins.SendMsg(_SyncLevelDataMsg);
                }
            
                if (PlaceRewardCell.NeedSend)
                {
                    PlaceRewardCell.NeedSend = false;
                    _SyncPlaceRewardDataMsg.Init();
                    NetworkManager.Ins.SendMsg(_SyncPlaceRewardDataMsg);
                }
            }
        }
    }
    
    
    /// <summary>
    /// 控制更新频率
    /// 次数达到一定阈值
    /// 时间达到一定阈值
    /// </summary>
    public class UpdateCell
    {
        public UpdateCell(float timeThreshold)
        {
            TimeThreshold = timeThreshold;
        }
        public float TimeThreshold; //时间阈值
        public float SendTime; //最后还一次请求更新时间 
        public bool NeedSend; //是否已经发送过更新请求
    }
}