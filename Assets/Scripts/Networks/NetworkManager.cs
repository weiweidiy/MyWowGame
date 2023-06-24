using System;
using System.Collections.Generic;
using Framework.Core;
using Framework.EventKit;
using Framework.Extension;
using Framework.UI;
using Logic.Common;
using Logic.Data;
using Logic.Manager;
using Logic.UI.Common;
using Logic.UI.UINet;
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

        private void OnDestroy()
        {
            Release();
        }

        public void Release()
        {
            m_SM?.Release();
        }

        private bool InDummyMode()
        {
            return GameCore.Ins.UseDummyServer;
        }

        public static long ClientMsgId = 0;

        /// <summary>
        /// 发送消息
        /// </summary>
        public void SendMsg(MessageHead pMsg)
        {
            pMsg.MsgID = ++ClientMsgId;
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

        public void OnReceiveMsg(byte[] pMsg)
        {
            var _Msg = MessageProcess.Decode(pMsg);
            //Debug.Log($"===> Client-ReceiveMsg : [{_Msg.MsgType}]");
            m_MsgList.Enqueue(_Msg);
        }

        public void OnConnectSuccess()
        {
            //通知其他逻辑 链接成功
            EventManager.Call(LogicEvent.ConnectSuccess);
        }

        public void OnConnectFailed()
        {
            m_SM.Release();
            m_SM = null;
            HideHold();
            UIMsgBox.ShowMsgBox(1, "提示", "连接服务器失败\n请检查网络");
            //通知其他逻辑 链接服务器失败
            EventManager.Call(LogicEvent.ConnectFailed);
        }

        public async void ShowHold()
        {
            await UIManager.Ins.OpenUI<UINetHolding>();
        }

        public void HideHold()
        {
            EventManager.Call(LogicEvent.CloseHoldUI);
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
            switch (pMsg.MsgType)
            {
                case NetWorkMsgType.S2C_Login:
                {
                    GameDataManager.Ins.OnReceiveLoginData(pMsg as S2C_Login);
                }
                    break;
                case NetWorkMsgType.S2C_ST:
                {
                    GameTimeManager.Ins.OnReceiveServerTimes(pMsg as S2C_ST);
                }
                    break;
                case NetWorkMsgType.S2C_DiamondUpdate:
                {
                    GameDataManager.Ins.Diamond = ((S2C_DiamondUpdate)pMsg).Diamond;
                }
                    break;
                case NetWorkMsgType.S2C_OilUpdate:
                {
                    GameDataManager.Ins.Oil = ((S2C_OilUpdate)pMsg).Oil;
                }
                    break;
                case NetWorkMsgType.S2C_TecPointUpdate:
                {
                    GameDataManager.Ins.TecPoint = ((S2C_TecPointUpdate)pMsg).TecPoint;
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
                    var type = (ItemType)_Msg.Type;
                    if (type == ItemType.Weapon)
                    {
                        EquipManager.Ins.OnWeaponIntensify(_Msg.EquipList, _Msg.IsAuto, _Msg.ComposeList);
                    }
                    else if (type == ItemType.Armor)
                    {
                        EquipManager.Ins.OnArmorIntensify(_Msg.EquipList, _Msg.IsAuto, _Msg.ComposeList);
                    }
                }
                    break;
                case NetWorkMsgType.S2C_EquipListUpdate:
                {
                    EquipManager.Ins.OnEquipListUpdate(pMsg as S2C_EquipListUpdate);
                }
                    break;
                case NetWorkMsgType.S2S_EquipCompose:
                {
                    EquipManager.Ins.OnEquipCompose(pMsg as S2S_EquipCompose);
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
                case NetWorkMsgType.S2S_PartnerCompose:
                {
                    PartnerManager.Ins.OnPartnerCompose(pMsg as S2S_PartnerCompose);
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
                case NetWorkMsgType.S2S_SkillCompose:
                {
                    SkillManager.Ins.OnSkillCompose(pMsg as S2S_SkillCompose);
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
                case NetWorkMsgType.S2C_ShopBuyOrder:
                {
                    ShopManager.Ins.OnShopBuyOrder(pMsg as S2C_ShopBuyOrder);
                }
                    break;
                case NetWorkMsgType.S2C_ShopBuy:
                {
                    ShopManager.Ins.OnShopBuy(pMsg as S2C_ShopBuy);
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
                case NetWorkMsgType.S2C_ReformCopyReward:
                {
                    RewardManager.Ins.On_S2C_ReformCopyReward(pMsg as S2C_ReformCopyReward);
                }
                    break;
                case NetWorkMsgType.S2C_MiningReward:
                {
                    RewardManager.Ins.On_S2C_MiningReward(pMsg as S2C_MiningReward);
                }
                    break;
                case NetWorkMsgType.S2C_UpdateMiningData:
                {
                    MiningManager.Ins.On_S2C_UpdateMiningData(pMsg as S2C_UpdateMiningData);
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
                case NetWorkMsgType.S2C_SpoilDraw:
                {
                    SpoilManager.Ins.OnSpoilDrawResult(pMsg as S2C_SpoilDraw);
                    break;
                }
                case NetWorkMsgType.S2C_SpoilSlotUnlock:
                {
                    SpoilManager.Ins.OnSpoilSlotUnlock(pMsg as S2C_SpoilSlotUnlock);
                    break;
                }
                case NetWorkMsgType.S2C_SpoilEquip:
                {
                    SpoilManager.Ins.OnSpoilEquip(pMsg as S2C_SpoilEquip);
                    break;
                }
                case NetWorkMsgType.S2C_SpoilUpgrade:
                {
                    SpoilManager.Ins.OnSpoilUpgrade(pMsg as S2C_SpoilUpgrade);
                    break;
                }
                case NetWorkMsgType.S2C_SpoilBreakthrough:
                {
                    SpoilManager.Ins.OnSpoilBreakthrough(pMsg as S2C_SpoilBreakthrough);
                    break;
                }
                case NetWorkMsgType.S2C_RoleOn:
                {
                    RoleManager.Ins.OnRoleOn(pMsg as S2C_RoleOn);
                }
                    break;
                case NetWorkMsgType.S2C_RoleOff:
                {
                    RoleManager.Ins.OnRoleOff(pMsg as S2C_RoleOff);
                }
                    break;
                case NetWorkMsgType.S2C_RoleIntensify:
                {
                    RoleManager.Ins.OnRoleIntensify(pMsg as S2C_RoleIntensify);
                }
                    break;
                case NetWorkMsgType.S2C_RoleBreak:
                {
                    RoleManager.Ins.OnRoleBreak(pMsg as S2C_RoleBreak);
                }
                    break;
                case NetWorkMsgType.S2C_RoleListUpdate:
                {
                    RoleManager.Ins.OnRoleListUpdate(pMsg as S2C_RoleListUpdate);
                }
                    break;
                case NetWorkMsgType.S2C_MushRoomUpdate:
                {
                    GameDataManager.Ins.MushRoom = ((S2C_MushRoomUpdate)pMsg).MushRoom;
                }
                    break;
                case NetWorkMsgType.S2C_BreakOreUpdate:
                {
                    GameDataManager.Ins.BreakOre = ((S2C_BreakOreUpdate)pMsg).BreakOre;
                }
                    break;
                case NetWorkMsgType.S2C_BreakTreeIntensify:
                {
                    RoleBreakTreeManager.Ins.OnIntensify(pMsg as S2C_BreakTreeIntensify);
                }
                    break;
                case NetWorkMsgType.S2C_BreakTPUpdate:
                {
                    GameDataManager.Ins.BreakTP = ((S2C_BreakTPUpdate)pMsg).BreakTP;
                }
                    break;
                case NetWorkMsgType.S2C_EngUpgrade:
                {
                    EngineManager.Ins.OnEngineUpgrade(pMsg as S2C_EngUpgrade);
                }
                    break;
                case NetWorkMsgType.S2C_EngPartOn:
                {
                    EngineManager.Ins.OnEngineOn(pMsg as S2C_EngPartOn);
                }
                    break;
                case NetWorkMsgType.S2C_EngPartOff:
                {
                    EngineManager.Ins.OnEngineOff(pMsg as S2C_EngPartOff);
                }
                    break;
                case NetWorkMsgType.S2C_EngResolve:
                {
                    EngineManager.Ins.OnEngineResolve(pMsg as S2C_EngResolve);
                }
                    break;
                case NetWorkMsgType.S2C_UpdateEngParts:
                {
                    EngineManager.Ins.OnEnginePartsUpdate(pMsg as S2C_UpdateEngParts);
                }
                    break;
                default:
                {
                    Debug.LogError(pMsg.MsgType);
                    throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}