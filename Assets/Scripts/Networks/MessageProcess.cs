using System;
using System.Text;
using LitJson;
using UnityEngine;

namespace Networks
{
    /// <summary>
    /// 消息打包 / 解包
    /// </summary>
    public class MessageProcess
    {
        public static byte[] Encode(object pMsg)
        {
            var _Msg = JsonMapper.ToJson(pMsg);
            var _ByteMsg = Encoding.UTF8.GetBytes(_Msg);
            return _ByteMsg;
        }

        public static MessageHead Decode(byte[] pMsg)
        {
            var _Msg = Encoding.UTF8.GetString(pMsg);
            var _T = JsonMapper.ToObject<MessageHead>(_Msg);
            switch (_T.m_MsgType)
            {
                case NetWorkMsgType.C2S_Login:
                    return JsonMapper.ToObject<C2S_Login>(_Msg);
                case NetWorkMsgType.S2C_Login:
                    //Debug.LogError("Login MSG : " + _Msg);
                    Debug.LogError("Login Size : " + pMsg.Length);
                    return JsonMapper.ToObject<S2C_Login>(_Msg);
                case NetWorkMsgType.C2S_GMAccount:
                    return JsonMapper.ToObject<C2S_GMAccount>(_Msg);
                case NetWorkMsgType.S2C_DiamondUpdate:
                    return JsonMapper.ToObject<S2C_DiamondUpdate>(_Msg);
                case NetWorkMsgType.S2C_OilUpdate:
                    return JsonMapper.ToObject<S2C_OilUpdate>(_Msg);
                case NetWorkMsgType.C2S_SyncPlayerData:
                    return JsonMapper.ToObject<C2S_SyncPlayerData>(_Msg);
                case NetWorkMsgType.C2S_EquipOn:
                    return JsonMapper.ToObject<C2S_EquipOn>(_Msg);
                case NetWorkMsgType.S2C_EquipOn:
                    return JsonMapper.ToObject<S2C_EquipOn>(_Msg);
                case NetWorkMsgType.S2C_EquipOff:
                    return JsonMapper.ToObject<S2C_EquipOff>(_Msg);
                case NetWorkMsgType.C2S_EquipIntensify:
                    return JsonMapper.ToObject<C2S_EquipIntensify>(_Msg);
                case NetWorkMsgType.S2C_EquipIntensify:
                    return JsonMapper.ToObject<S2C_EquipIntensify>(_Msg);
                case NetWorkMsgType.S2C_EquipListUpdate:
                    return JsonMapper.ToObject<S2C_EquipListUpdate>(_Msg);
                case NetWorkMsgType.C2S_PartnerOn:
                    return JsonMapper.ToObject<C2S_PartnerOn>(_Msg);
                case NetWorkMsgType.S2C_PartnerOn:
                    return JsonMapper.ToObject<S2C_PartnerOn>(_Msg);
                case NetWorkMsgType.C2S_PartnerOff:
                    return JsonMapper.ToObject<C2S_PartnerOff>(_Msg);
                case NetWorkMsgType.S2C_PartnerOff:
                    return JsonMapper.ToObject<S2C_PartnerOff>(_Msg);
                case NetWorkMsgType.C2S_PartnerIntensify:
                    return JsonMapper.ToObject<C2S_PartnerIntensify>(_Msg);
                case NetWorkMsgType.S2C_PartnerIntensify:
                    return JsonMapper.ToObject<S2C_PartnerIntensify>(_Msg);
                case NetWorkMsgType.S2C_PartnerListUpdate:
                    return JsonMapper.ToObject<S2C_PartnerListUpdate>(_Msg);
                case NetWorkMsgType.C2S_SkillOn:
                    return JsonMapper.ToObject<C2S_SkillOn>(_Msg);
                case NetWorkMsgType.S2C_SkillOn:
                    return JsonMapper.ToObject<S2C_SkillOn>(_Msg);
                case NetWorkMsgType.C2S_SkillOff:
                    return JsonMapper.ToObject<C2S_SkillOff>(_Msg);
                case NetWorkMsgType.S2C_SkillOff:
                    return JsonMapper.ToObject<S2C_SkillOff>(_Msg);
                case NetWorkMsgType.C2S_SkillIntensify:
                    return JsonMapper.ToObject<C2S_SkillIntensify>(_Msg);
                case NetWorkMsgType.S2C_SkillIntensify:
                    return JsonMapper.ToObject<S2C_SkillIntensify>(_Msg);
                case NetWorkMsgType.S2C_SkillListUpdate:
                    return JsonMapper.ToObject<S2C_SkillListUpdate>(_Msg);
                case NetWorkMsgType.S2C_EngineGet:
                    return JsonMapper.ToObject<S2C_EngineGet>(_Msg);
                case NetWorkMsgType.C2S_EngineIntensify:
                    return JsonMapper.ToObject<C2S_EngineIntensify>(_Msg);
                case NetWorkMsgType.S2C_EngineIntensify:
                    return JsonMapper.ToObject<S2C_EngineIntensify>(_Msg);
                case NetWorkMsgType.C2S_EngineRemove:
                    return JsonMapper.ToObject<C2S_EngineRemove>(_Msg);
                case NetWorkMsgType.S2C_EngineIronUpdate:
                    return JsonMapper.ToObject<S2C_EngineIronUpdate>(_Msg);
                case NetWorkMsgType.C2S_EngineOn:
                    return JsonMapper.ToObject<C2S_EngineOn>(_Msg);
                case NetWorkMsgType.S2C_EngineOn:
                    return JsonMapper.ToObject<S2C_EngineOn>(_Msg);
                case NetWorkMsgType.C2S_EngineOff:
                    return JsonMapper.ToObject<C2S_EngineOff>(_Msg);
                case NetWorkMsgType.S2C_EngineOff:
                    return JsonMapper.ToObject<S2C_EngineOff>(_Msg);
                case NetWorkMsgType.C2S_DrawCard:
                    return JsonMapper.ToObject<C2S_DrawCard>(_Msg);
                case NetWorkMsgType.S2C_DrawCard:
                    return JsonMapper.ToObject<S2C_DrawCard>(_Msg);
                case NetWorkMsgType.C2S_UpdateDrawCardData:
                    return JsonMapper.ToObject<C2S_UpdateDrawCardData>(_Msg);
                case NetWorkMsgType.S2C_UpdateDrawCardData:
                    return JsonMapper.ToObject<S2C_UpdateDrawCardData>(_Msg);
                case NetWorkMsgType.C2S_MiningReward:
                    return JsonMapper.ToObject<C2S_MiningReward>(_Msg);
                case NetWorkMsgType.S2C_MiningReward:
                    return JsonMapper.ToObject<S2C_MiningReward>(_Msg);
                case NetWorkMsgType.C2S_PlaceReward:
                    return JsonMapper.ToObject<C2S_PlaceReward>(_Msg);
                case NetWorkMsgType.S2C_PlaceReward:
                    return JsonMapper.ToObject<S2C_PlaceReward>(_Msg);
                case NetWorkMsgType.C2S_GetPlaceReward:
                    return JsonMapper.ToObject<C2S_GetPlaceReward>(_Msg);
                case NetWorkMsgType.C2S_CommonReward:
                    return JsonMapper.ToObject<C2S_CommonReward>(_Msg);
                case NetWorkMsgType.S2C_CommonReward:
                    return JsonMapper.ToObject<S2C_CommonReward>(_Msg);
                case NetWorkMsgType.S2C_OilCopyReward:
                    return JsonMapper.ToObject<S2C_OilCopyReward>(_Msg);
                case NetWorkMsgType.C2S_UpdateMiningData:
                    return JsonMapper.ToObject<C2S_UpdateMiningData>(_Msg);
                case NetWorkMsgType.S2C_UpdateMiningData:
                    return JsonMapper.ToObject<S2C_UpdateMiningData>(_Msg);
                case NetWorkMsgType.C2S_UpdateTaskProcess:
                    return JsonMapper.ToObject<C2S_UpdateTaskProcess>(_Msg);
                case NetWorkMsgType.S2C_UpdateTaskState:
                    return JsonMapper.ToObject<S2C_UpdateTaskState>(_Msg);
                case NetWorkMsgType.C2S_TaskGetReward:
                    return JsonMapper.ToObject<C2S_TaskGetReward>(_Msg);
                case NetWorkMsgType.S2C_UpdateMainTask:
                    return JsonMapper.ToObject<S2C_UpdateMainTask>(_Msg);
                case NetWorkMsgType.S2C_UpdateDailyTask:
                    return JsonMapper.ToObject<S2C_UpdateDailyTask>(_Msg);
                case NetWorkMsgType.C2S_RequestDailyTaskList:
                    return JsonMapper.ToObject<C2S_RequestDailyTaskList>(_Msg);
                case NetWorkMsgType.S2C_RequestDailyTaskList:
                    return JsonMapper.ToObject<S2C_RequestDailyTaskList>(_Msg);
                case NetWorkMsgType.C2S_EnterCopy:
                    return JsonMapper.ToObject<C2S_EnterCopy>(_Msg);
                case NetWorkMsgType.S2C_EnterCopy:
                    return JsonMapper.ToObject<S2C_EnterCopy>(_Msg);
                case NetWorkMsgType.C2S_ExitCopy:
                    return JsonMapper.ToObject<C2S_ExitCopy>(_Msg);
                case NetWorkMsgType.S2C_ExitCopy:
                    return JsonMapper.ToObject<S2C_ExitCopy>(_Msg);
                case NetWorkMsgType.C2S_UpdateCopyKeyCount:
                    return JsonMapper.ToObject<C2S_UpdateCopyKeyCount>(_Msg);
                case NetWorkMsgType.S2C_UpdateCopyKeyCount:
                    return JsonMapper.ToObject<S2C_UpdateCopyKeyCount>(_Msg);
                case NetWorkMsgType.C2S_UpdateLockStoryData:
                    return JsonMapper.ToObject<C2S_UpdateLockStoryData>(_Msg);
                case NetWorkMsgType.C2S_UpdateResearchTime:
                    return JsonMapper.ToObject<C2S_UpdateResearchTime>(_Msg);
                case NetWorkMsgType.S2C_UpdateResearchTime:
                    return JsonMapper.ToObject<S2C_UpdateResearchTime>(_Msg);
                case NetWorkMsgType.C2S_Researching:
                    return JsonMapper.ToObject<C2S_Researching>(_Msg);
                case NetWorkMsgType.S2C_Researching:
                    return JsonMapper.ToObject<S2C_Researching>(_Msg);
                default:
                    throw new Exception($"MessageProcess 没有实现! 请实现一下! {_T.m_MsgType}");
            }
        }
    }
}