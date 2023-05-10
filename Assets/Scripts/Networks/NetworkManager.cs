using System;
using System.Collections.Generic;
using Framework.Core;
using Framework.EventKit;
using Framework.Extension;
using Logic.Common;
using Logic.Data;
using Logic.Manager;
using Networks.State;
using UnityEngine;

namespace Networks
{
    /// <summary>
    /// 网络层管理
    /// </summary>
    public class NetworkManager : PersistentMonoSingleton<NetworkManager>
    {
        private NetworkManager()
        {
        }

        private NetworkStateData m_StateData;

        private NetworkSM m_SM;

        public readonly Queue<MessageHead> m_MsgList = new(12);

        /// <summary>
        /// 初始化并链接服务器
        /// </summary>
        public void ConnectTo(string pServerAddress)
        {
            if (m_SM != null)
            {
                Debug.LogError("当前已经连接");
                return;
            }

            m_SM = new NetworkSM();
            m_StateData = new NetworkStateData
            {
                m_NetWorkSM = m_SM,
                m_ServerAddr = pServerAddress
            };

            if (InDummyMode())
            {
                //本地测试模式
                m_SM.Start(m_StateData, m_StateData._Dummy);
            }
            else
            {
                m_SM.Start(m_StateData, m_StateData._Connect);
            }
        }

        /// <summary>
        /// 关闭链接
        /// </summary>
        public void Close()
        {
            if (InDummyMode())
            {
            }
            else
            {
                m_SM.ToClose();
            }
        }

        private void Update()
        {
            m_SM?.Update();
            ProcessMsg();
        }

        public void Release()
        {
            m_SM?.Release();
        }

        private bool InDummyMode()
        {
            return GameCore.Ins.UseDummyServer;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        public void SendMsg(object pMsg)
        {
            var _ByteMsg = MessageProcess.Encode(pMsg);

            if (InDummyMode())
            {
                m_StateData._Dummy.SendMsgToServer(_ByteMsg);
            }
            else
            {
                m_StateData.m_WebSocket.SendAsync(_ByteMsg);
            }
        }

        // /// <summary>
        // /// 发送消息 - 特殊情况 立刻发送
        // /// </summary>
        // public void SendMsgImmediately(object pMsg)
        // {
        //     var _ByteMsg = MessageProcess.Encode(pMsg);
        //     m_StateData._Dummy.SendMsgToServerImmediately(_ByteMsg);
        // }

        public void OnReceiveMsg(byte[] pMsg)
        {
            var _Msg = MessageProcess.Decode(pMsg);
            Debug.Log($"===> Client-ReceiveMsg : [{_Msg.m_MsgType}]");
            m_MsgList.Enqueue(_Msg);
        }

        public void OnConnectSuccess()
        {
            //通知其他逻辑 链接成功
            EventManager.Call(LogicEvent.ConnectSuccess);
        }

        public void OnConnectFailed()
        {
            //通知其他逻辑 链接服务器失败
            EventManager.Call(LogicEvent.ConnectFailed);
        }

        //消息处理
        private void ProcessMsg()
        {
            if (m_MsgList.Count <= 0)
                return;

            var _Msg = m_MsgList.Dequeue();
            ProcessMsg(_Msg);
        }

        private void ProcessMsg(MessageHead pMsg)
        {
            switch (pMsg.m_MsgType)
            {
                case NetWorkMsgType.S2C_Login:
                {
                    GameDataManager.Ins.OnReceiveLoginData(pMsg as S2C_Login);
                }
                    break;
                case NetWorkMsgType.S2C_DiamondUpdate:
                {
                    GameDataManager.Ins.Diamond = ((S2C_DiamondUpdate)pMsg).m_Diamond;
                }
                    break;
                case NetWorkMsgType.S2C_OilUpdate:
                    {
                        GameDataManager.Ins.Oil = ((S2C_OilUpdate)pMsg).m_Oil;
                    }
                    break;
                case NetWorkMsgType.S2C_EquipOn:
                {
                    EquipManager.Ins.OnEquipOn(pMsg as S2C_EquipOn);
                }
                    break;
                case NetWorkMsgType.S2C_EquipOff:
                {
                    EquipManager.Ins.OnEquipOff(pMsg as S2C_EquipOff);
                }
                    break;
                case NetWorkMsgType.S2C_EquipIntensify:
                {
                    var _Msg = pMsg as S2C_EquipIntensify;
                    if (_Msg.m_Type == ItemType.Weapon)
                        EquipManager.Ins.OnWeaponIntensify(_Msg.m_EquipList, _Msg.m_IsAuto);
                    else if (_Msg.m_Type == ItemType.Armor)
                        EquipManager.Ins.OnArmorIntensify(_Msg.m_EquipList, _Msg.m_IsAuto);
                }
                    break;
                case NetWorkMsgType.S2C_EquipListUpdate:
                {
                    EquipManager.Ins.OnEquipListUpdate(pMsg as S2C_EquipListUpdate);
                }
                    break;
                case NetWorkMsgType.S2C_PartnerOn:
                {
                    PartnerManager.Ins.OnPartnerOn(pMsg as S2C_PartnerOn);
                }
                    break;
                case NetWorkMsgType.S2C_PartnerOff:
                {
                    PartnerManager.Ins.OnPartnerOff(pMsg as S2C_PartnerOff);
                }
                    break;
                case NetWorkMsgType.S2C_PartnerIntensify:
                {
                    PartnerManager.Ins.OnPartnerIntensify(pMsg as S2C_PartnerIntensify);
                }
                    break;
                case NetWorkMsgType.S2C_PartnerListUpdate:
                {
                    PartnerManager.Ins.OnPartnerListUpdate(pMsg as S2C_PartnerListUpdate);
                }
                    break;
                case NetWorkMsgType.S2C_SkillOn:
                {
                    SkillManager.Ins.OnSkillOn(pMsg as S2C_SkillOn);
                }
                    break;
                case NetWorkMsgType.S2C_SkillOff:
                {
                    SkillManager.Ins.OnSkillOff(pMsg as S2C_SkillOff);
                }
                    break;
                case NetWorkMsgType.S2C_SkillIntensify:
                {
                    SkillManager.Ins.OnSkillIntensify(pMsg as S2C_SkillIntensify);
                }
                    break;
                case NetWorkMsgType.S2C_SkillListUpdate:
                {
                    SkillManager.Ins.OnSkillListUpdate(pMsg as S2C_SkillListUpdate);
                }
                    break;
                case NetWorkMsgType.S2C_EngineGet:
                {
                    EngineManager.Ins.OnEngineGet(pMsg as S2C_EngineGet);
                }
                    break;
                case NetWorkMsgType.S2C_EngineIntensify:
                {
                    EngineManager.Ins.OnEngineIntensify(pMsg as S2C_EngineIntensify);
                }
                    break;
                case NetWorkMsgType.S2C_EngineIronUpdate:
                {
                    GameDataManager.Ins.Iron = ((S2C_EngineIronUpdate)pMsg).m_Iron;
                }
                    break;
                case NetWorkMsgType.S2C_EngineOn:
                {
                    EngineManager.Ins.OnEngineOn(pMsg as S2C_EngineOn);
                }
                    break;
                case NetWorkMsgType.S2C_EngineOff:
                {
                    EngineManager.Ins.OnEngineOff(pMsg as S2C_EngineOff);
                }
                    break;
                case NetWorkMsgType.S2C_DrawCard:
                {
                    ShopManager.Ins.OnDrawResult(pMsg as S2C_DrawCard);
                }
                    break;
                case NetWorkMsgType.S2C_UpdateDrawCardData:
                {
                    ShopManager.Ins.On_S2C_UpdateShopData(pMsg as S2C_UpdateDrawCardData);
                }
                    break;
                case NetWorkMsgType.S2C_MiningReward:
                {
                    RewardManager.Ins.On_S2C_MiningReward(pMsg as S2C_MiningReward);
                }
                    break;
                case NetWorkMsgType.S2C_PlaceReward:
                {
                    RewardManager.Ins.On_S2C_PlaceReward(pMsg as S2C_PlaceReward);
                }
                    break;
                case NetWorkMsgType.S2C_CommonReward:
                {
                    RewardManager.Ins.On_S2C_CommonReward(pMsg as S2C_CommonReward);
                }
                    break;
                case NetWorkMsgType.S2C_OilCopyReward:
                    {
                        RewardManager.Ins.On_S2C_OilCopyReward(pMsg as S2C_OilCopyReward);
                    }
                    break;
                case NetWorkMsgType.S2C_UpdateMiningData:
                {
                    MiningManager.Ins.On_S2C_UpdateMiningData(pMsg as S2C_UpdateMiningData);
                }
                    break;
                case NetWorkMsgType.S2C_UpdateTaskState:
                {
                    TaskManager.Ins.OnUpdateTaskState(pMsg as S2C_UpdateTaskState);
                }
                    break;
                case NetWorkMsgType.S2C_UpdateMainTask:
                {
                    TaskManager.Ins.OnUpdateMainTask(pMsg as S2C_UpdateMainTask);
                }
                    break;
                case NetWorkMsgType.S2C_UpdateDailyTask:
                {
                    TaskManager.Ins.OnUpdateDailyTask(pMsg as S2C_UpdateDailyTask);
                }
                    break;
                case NetWorkMsgType.S2C_RequestDailyTaskList:
                {
                    TaskManager.Ins.OnRequestDailyTaskList(pMsg as S2C_RequestDailyTaskList);
                }
                    break;
                case NetWorkMsgType.S2C_EnterCopy:
                {
                    CopyManager.Ins.OnEnterCopy(pMsg as S2C_EnterCopy);
                }
                    break;
                case NetWorkMsgType.S2C_ExitCopy:
                {
                    CopyManager.Ins.OnExitCopy(pMsg as S2C_ExitCopy);
                }
                    break;
                case NetWorkMsgType.S2C_UpdateCopyKeyCount:
                {
                    CopyManager.Ins.OnUpdateCopyKeyCount(pMsg as S2C_UpdateCopyKeyCount);
                }
                    break;
                case NetWorkMsgType.S2C_UpdateResearchTime:
                {
                    ResearchManager.Ins.OnUpdateResearchTime(pMsg as S2C_UpdateResearchTime);
                }
                    break;
                case NetWorkMsgType.S2C_Researching:
                {
                    ResearchManager.Ins.OnResearching(pMsg as S2C_Researching);
                }
                    break;
                default:
                {
                    Debug.LogError(pMsg.m_MsgType);
                    throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}