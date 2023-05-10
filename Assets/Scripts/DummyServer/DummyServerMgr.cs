using System;
using System.Globalization;
using Framework.Extension;
using Logic.Common;
using Networks;
using UnityEngine;

namespace DummyServer
{
    /// <summary>
    /// 本地模拟服务器 用来处理客户端请求 获取数据
    /// 数据存储在本地PlayerPrefs内 ---> DummyDB
    /// </summary>
    public partial class DummyServerMgr : Singleton<DummyServerMgr>
    {
        private Action<byte[]> m_SendMsgCB;
        private DummyDB m_DB;

        public void Connect(Action<byte[]> pSendMsgCB)
        {
            m_SendMsgCB = pSendMsgCB;
        }

        public void Stop()
        {
            m_SendMsgCB = null;
            //DummyDB.Save(m_DB);
        }

        /// <summary>
        /// 客户端本地模拟服务器进行消息处理
        /// </summary>
        public void ProcessMsg(byte[] pByteMsg)
        {
            var _Msg = MessageProcess.Decode(pByteMsg);

            Debug.Log($"===> Dummy-ReceiveMsg : [{_Msg.m_MsgType}]");

            switch (_Msg.m_MsgType)
            {
                case NetWorkMsgType.C2S_Login:
                    On_C2S_Login();
                    break;
                case NetWorkMsgType.C2S_GMAccount:
                    // 临时增加客户端向服务器请求GM数据功能
                    UpdateGMAccount();
                    break;
                case NetWorkMsgType.C2S_SyncPlayerData:
                    On_C2S_SyncPlayerData(_Msg as C2S_SyncPlayerData);
                    break;
                case NetWorkMsgType.C2S_EquipOn:
                    On_C2S_EquipOn(_Msg as C2S_EquipOn);
                    break;
                case NetWorkMsgType.C2S_EquipIntensify:
                    On_C2S_EquipIntensify(_Msg as C2S_EquipIntensify);
                    break;
                case NetWorkMsgType.C2S_PartnerOn:
                    On_C2S_PartnerOn(_Msg as C2S_PartnerOn);
                    break;
                case NetWorkMsgType.C2S_PartnerOff:
                    On_C2S_PartnerOff(_Msg as C2S_PartnerOff);
                    break;
                case NetWorkMsgType.C2S_PartnerIntensify:
                    On_C2S_PartnerIntensify(_Msg as C2S_PartnerIntensify);
                    break;
                case NetWorkMsgType.C2S_SkillOn:
                    On_C2S_SkillOn(_Msg as C2S_SkillOn);
                    break;
                case NetWorkMsgType.C2S_SkillOff:
                    On_C2S_SkillOff(_Msg as C2S_SkillOff);
                    break;
                case NetWorkMsgType.C2S_SkillIntensify:
                    On_C2S_SkillIntensify(_Msg as C2S_SkillIntensify);
                    break;
                case NetWorkMsgType.C2S_EngineIntensify:
                    On_C2S_EngineIntensify(_Msg as C2S_EngineIntensify);
                    break;
                case NetWorkMsgType.C2S_EngineRemove:
                    On_C2S_EngineRemove(_Msg as C2S_EngineRemove);
                    break;
                case NetWorkMsgType.C2S_EngineOn:
                    On_C2S_EngineOn(_Msg as C2S_EngineOn);
                    break;
                case NetWorkMsgType.C2S_EngineOff:
                    On_C2S_EngineOff(_Msg as C2S_EngineOff);
                    break;
                case NetWorkMsgType.C2S_DrawCard:
                    On_C2S_DrawCard(_Msg as C2S_DrawCard);
                    break;
                case NetWorkMsgType.C2S_UpdateDrawCardData:
                    On_C2S_UpdateDrawCardData(_Msg as C2S_UpdateDrawCardData);
                    break;
                case NetWorkMsgType.C2S_MiningReward:
                    On_C2S_MiningReward(_Msg as C2S_MiningReward);
                    break;
                case NetWorkMsgType.C2S_PlaceReward:
                    On_C2S_PlaceReward(_Msg as C2S_PlaceReward);
                    break;
                case NetWorkMsgType.C2S_GetPlaceReward:
                    On_C2S_GetPlaceReward(_Msg as C2S_GetPlaceReward);
                    break;
                case NetWorkMsgType.C2S_CommonReward:
                    On_C2S_CommonReward(_Msg as C2S_CommonReward);
                    break;
                case NetWorkMsgType.C2S_UpdateMiningData:
                    On_C2S_UpdateMiningData(_Msg as C2S_UpdateMiningData);
                    break;
                case NetWorkMsgType.C2S_UpdateTaskProcess:
                    On_C2S_UpdateTaskProcess(_Msg as C2S_UpdateTaskProcess);
                    break;
                case NetWorkMsgType.C2S_TaskGetReward:
                    On_C2S_TaskGetReward(_Msg as C2S_TaskGetReward);
                    break;
                case NetWorkMsgType.C2S_RequestDailyTaskList:
                    On_C2S_RequestDailyTaskList(_Msg as C2S_RequestDailyTaskList);
                    break;
                case NetWorkMsgType.C2S_EnterCopy:
                    On_C2S_EnterCopy(_Msg as C2S_EnterCopy);
                    break;
                case NetWorkMsgType.C2S_ExitCopy:
                    On_C2S_ExitCopy(_Msg as C2S_ExitCopy);
                    break;
                case NetWorkMsgType.C2S_UpdateCopyKeyCount:
                    On_C2S_UpdateCopyKeyCount(_Msg as C2S_UpdateCopyKeyCount);
                    break;
                case NetWorkMsgType.C2S_UpdateLockStoryData:
                    On_C2S_UpdateLockStoryData(_Msg as C2S_UpdateLockStoryData);
                    break;
                case NetWorkMsgType.C2S_UpdateResearchTime:
                    On_C2S_UpdateResearchTime(_Msg as C2S_UpdateResearchTime);
                    break;
                case NetWorkMsgType.C2S_Researching:
                    On_C2S_Researching(_Msg as C2S_Researching);
                    break;
                default:
                {
                    Debug.LogError(_Msg.m_MsgType);
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        #region 消息处理

        private void On_C2S_Login()
        {
            m_DB = DummyDB.Load();
            if (m_DB.m_IsFirstLogin)
            {
                m_DB.Init();
                DummyDB.Save(m_DB);
            }

            /*
             *
             * !!! 由于现在使用DummyServerMgr是在客户端实现
             * !!! 这里赋值的时候 要注意进行深拷贝 !!!
             * 
             */
            var _Data = new S2C_Login()
            {
                m_IsFirstLogin = m_DB.m_IsFirstLogin,
                m_LastGameDate = m_DB.m_LastGameDate,

                m_Coin = m_DB.m_Coin,
                m_Diamond = m_DB.m_Diamond,
                m_Iron = m_DB.m_Iron,
                m_Oil = m_DB.m_Oil,

                m_GJJAtkLevel = m_DB.m_GJJAtkLevel,
                m_GJJHPLevel = m_DB.m_GJJHPLevel,
                m_GJJHPRecoverLevel = m_DB.m_GJJHPRecoverLevel,
                m_GJJCriticalLevel = m_DB.m_GJJCriticalLevel,
                m_GJJCriticalDamageLevel = m_DB.m_GJJCriticalDamageLevel,
                m_GJJAtkSpeedLevel = m_DB.m_GJJAtkSpeedLevel,
                m_GJJDoubleHitLevel = m_DB.m_GJJDoubleHitLevel,
                m_GJJTripletHitLevel = m_DB.m_GJJTripletHitLevel,

                m_AutoSkill = m_DB.m_AutoSkill,
                m_IsMusicOn = m_DB.m_IsMusicOn,
                m_IsSoundOn = m_DB.m_IsSoundOn,

                m_CurLevelID = m_DB.m_CurLevelID,
                m_CurLevelNode = m_DB.m_CurLevelNode,
                m_CurLevelState = m_DB.m_CurLevelState,

                m_BtnPlaceRewardClickTime = m_DB.m_BtnPlaceRewardClickTime,
                m_BtnPlaceRewardShowTime = m_DB.m_BtnPlaceRewardShowTime,
                m_PopPlaceRewardTime = m_DB.m_PopPlaceRewardTime,

                m_WeaponOnID = m_DB.m_WeaponOnID,
                m_ClothesOnID = m_DB.m_ArmorOnID,
                m_EngineOnId = m_DB.m_EngineOnId,
                m_EngineGetId = m_DB.m_EngineGetId,

                m_MainTaskCount = m_DB.m_MainTaskCount,
            };

            _Data.m_PartnerOnList.AddRange(m_DB.m_PartnerOnList);
            _Data.m_SkillOnList.AddRange(m_DB.m_SkillOnList);

            //!!! 引用类型 深拷贝 !!!

            foreach (var partnerData in m_DB.m_PartnerList)
            {
                _Data.m_PartnerList.Add(partnerData.Clone());
            }

            foreach (var data in m_DB.m_WeaponList)
            {
                _Data.m_WeaponList.Add(data.Clone());
            }

            foreach (var data in m_DB.m_ArmorList)
            {
                _Data.m_ArmorList.Add(data.Clone());
            }

            foreach (var skillData in m_DB.m_SkillList)
            {
                _Data.m_SkillList.Add(skillData.Clone());
            }

            foreach (var engineData in m_DB.m_EngineList)
            {
                _Data.m_EngineList.Add(engineData.Clone());
            }

            // 抽卡
            _Data.m_ShopSkillData = m_DB.m_ShopSkillData.Clone();
            _Data.m_ShopPartnerData = m_DB.m_ShopPartnerData.Clone();
            _Data.m_ShopEquipData = m_DB.m_ShopEquipData.Clone();

            //任务
            _Data.m_CurMainTask = m_DB.m_CurMainTask.Clone();
            foreach (var _Task in m_DB.m_DailyTaskList)
            {
                _Data.m_DailyTaskList.Add(_Task.Clone());
            }

            //副本
            _Data.m_DiamondCopyData = m_DB.m_DiamondCopyData.Clone();
            _Data.m_CoinCopyData = m_DB.m_CoinCopyData.Clone();
            _Data.m_OilCopyData = m_DB.m_OilCopyData.Clone();

            //考古
            _Data.m_MiningData = m_DB.m_MiningData.Clone();

            //开放剧情
            foreach (var lockStoryData in m_DB.m_LockStoryList)
            {
                _Data.m_LockStoryList.Add(lockStoryData.Clone());
            }

            //考古研究
            foreach (var researchData in m_DB.m_ResearchList)
            {
                _Data.m_ResearchList.Add(researchData.Clone());
            }

            _Data.m_ResearchEffectData = m_DB.m_ResearchEffectData.Clone();

            SendMsg(_Data);
        }

        private void On_C2S_SyncPlayerData(C2S_SyncPlayerData pMsg)
        {
            m_DB.m_Coin = pMsg.m_Coin;

            m_DB.m_GJJAtkLevel = pMsg.m_GJJAtkLevel;
            m_DB.m_GJJHPLevel = pMsg.m_GJJHPLevel;
            m_DB.m_GJJHPRecoverLevel = pMsg.m_GJJHPRecoverLevel;
            m_DB.m_GJJCriticalLevel = pMsg.m_GJJCriticalLevel;
            m_DB.m_GJJCriticalDamageLevel = pMsg.m_GJJCriticalDamageLevel;
            m_DB.m_GJJAtkSpeedLevel = pMsg.m_GJJAtkSpeedLevel;
            m_DB.m_GJJDoubleHitLevel = pMsg.m_GJJDoubleHitLevel;
            m_DB.m_GJJTripletHitLevel = pMsg.m_GJJTripletHitLevel;

            m_DB.m_AutoSkill = pMsg.m_AutoSkill;

            m_DB.m_CurLevelID = pMsg.m_CurLevelID;
            m_DB.m_CurLevelNode = pMsg.m_CurLevelNode;
            m_DB.m_CurLevelState = pMsg.m_CurLevelState;

            m_DB.m_BtnPlaceRewardClickTime = pMsg.m_BtnPlaceRewardClickTime;
            m_DB.m_BtnPlaceRewardShowTime = pMsg.m_BtnPlaceRewardShowTime;
            m_DB.m_PopPlaceRewardTime = pMsg.m_PopPlaceRewardTime;

            DummyDB.Save(m_DB);
            Debug.Log("                                  === SAVE PLAYER DATA ===");
        }

        private void On_C2S_EquipOn(C2S_EquipOn pMsg)
        {
            DoEquipOn(pMsg.m_EquipID, pMsg.m_Type);
        }

        private void On_C2S_EquipIntensify(C2S_EquipIntensify pMsg)
        {
            if (pMsg.m_IsAuto)
                EquipIntensifyAuto(pMsg.m_Type);
            else
                EquipIntensify(pMsg.m_EquipID, pMsg.m_Type);
        }

        private void On_C2S_PartnerOn(C2S_PartnerOn pMsg)
        {
            DoPartnerOn(pMsg.m_PartnerID);
        }

        private void On_C2S_PartnerOff(C2S_PartnerOff pMsg)
        {
            DoPartnerOff(pMsg.m_PartnerID);
        }

        private void On_C2S_PartnerIntensify(C2S_PartnerIntensify pMsg)
        {
            if (pMsg.m_IsAuto)
                PartnerIntensifyAuto();
            else
                PartnerIntensify(pMsg.m_PartnerID);
        }

        private void On_C2S_SkillOn(C2S_SkillOn pMsg)
        {
            DoSkillOn(pMsg.m_SkillID);
        }

        private void On_C2S_SkillOff(C2S_SkillOff pMsg)
        {
            DoSkillOff(pMsg.m_SkillID);
        }

        private void On_C2S_SkillIntensify(C2S_SkillIntensify pMsg)
        {
            if (pMsg.m_IsAuto)
                SkillIntensifyAuto();
            else
                SkillIntensify(pMsg.m_SkillID);
        }

        private void On_C2S_EngineIntensify(C2S_EngineIntensify pMsg)
        {
            DoEngineIntensify(pMsg.m_EngineIntensifyId);
        }

        private void On_C2S_EngineRemove(C2S_EngineRemove pMsg)
        {
            DoEngineRemove(pMsg.m_EngineRemoveId);
        }

        private void On_C2S_EngineOn(C2S_EngineOn pMsg)
        {
            DoEngineOn(pMsg.m_EngineOnId);
        }

        private void On_C2S_EngineOff(C2S_EngineOff pMsg)
        {
            DoEngineOff(pMsg.m_EngineOffId);
        }

        private void On_C2S_DrawCard(C2S_DrawCard pMsg)
        {
            switch ((DrawCardType)pMsg.m_DrawCardType)
            {
                case DrawCardType.Skill:
                    OnDrawSkill(pMsg.m_DrawCardCostType);
                    break;
                case DrawCardType.Partner:
                    OnDrawPartner(pMsg.m_DrawCardCostType);
                    break;
                case DrawCardType.Equip:
                    OnDrawEquip(pMsg.m_DrawCardCostType);
                    break;
            }
        }

        private void On_C2S_UpdateDrawCardData(C2S_UpdateDrawCardData pMsg)
        {
            switch ((DrawCardType)pMsg.m_DrawCardType)
            {
                case DrawCardType.Skill:
                    OnUpdateGameShopSkillData();
                    break;
                case DrawCardType.Partner:
                    OnUpdateGameShopPartnerData();
                    break;
                case DrawCardType.Equip:
                    OnUpdateGameShopEquipData();
                    break;
            }
        }

        public void On_C2S_MiningReward(C2S_MiningReward pMsg)
        {
            OnMiningReward(pMsg.m_TreasureType);
        }

        public void On_C2S_PlaceReward(C2S_PlaceReward pMsg)
        {
            OnPlaceReward(pMsg.m_PlaceRewardCount);
        }

        public void On_C2S_GetPlaceReward(C2S_GetPlaceReward pMsg)
        {
            OnGetPlaceReward();
        }

        public void On_C2S_CommonReward(C2S_CommonReward pMsg)
        {
            OnCommonReward(pMsg);
        }

        public void On_C2S_UpdateMiningData(C2S_UpdateMiningData pMsg)
        {
            var miningUpdateType = (MiningUpdateType)pMsg.m_UpdateType;
            switch (miningUpdateType)
            {
                case MiningUpdateType.Reduce:
                    OnReduceMiningData(pMsg.m_MiningDataType);
                    break;
                case MiningUpdateType.Increase:
                    OnIncreaseMiningData(pMsg.m_MiningDataType);
                    break;
            }
        }

        public void On_C2S_UpdateLockStoryData(C2S_UpdateLockStoryData pMsg)
        {
            OnUpdateLockStoryData(pMsg);
        }

        public void On_C2S_UpdateResearchTime(C2S_UpdateResearchTime pMsg)
        {
            OnUpdateResearchTime(pMsg);
        }

        public void On_C2S_Researching(C2S_Researching pMsg)
        {
            OnResearching(pMsg);
        }

        #endregion

        private void SendMsg(object pMsg)
        {
            var _Msg = MessageProcess.Encode(pMsg);
            m_SendMsgCB?.Invoke(_Msg);
        }

        /// <summary>
        /// 更新元宝/钻石 并通知服务器
        /// </summary>
        public void UpdateDiamond(long pDiamond)
        {
            m_DB.m_Diamond += pDiamond;
            DummyDB.Save(m_DB);
            SendMsg(new S2C_DiamondUpdate { m_Diamond = m_DB.m_Diamond });
        }

        public void UpdateOil(int pOil)
        {
            m_DB.m_Oil += pOil;
            DummyDB.Save(m_DB);
            SendMsg(new S2C_OilUpdate { m_Oil = m_DB.m_Oil });
        }

        /// <summary>
        /// 更新引擎强化分解材料
        /// </summary>
        /// <param name="pIron"></param>
        public void UpdateIron(int pIron)
        {
            m_DB.m_Iron += pIron;
            DummyDB.Save(m_DB);
            SendMsg(new S2C_EngineIronUpdate { m_Iron = m_DB.m_Iron });
        }

        /// <summary>
        /// 更新副本钥匙
        /// </summary>
        public void UpdateKey(int pKey)
        {
            m_DB.m_CoinCopyData.m_KeyCount += pKey;
            m_DB.m_DiamondCopyData.m_KeyCount += pKey;
            m_DB.m_OilCopyData.m_KeyCount += pKey;
            DummyDB.Save(m_DB);
            SendMsg(new S2C_UpdateCopyKeyCount
            {
                m_CoinKeyCount = m_DB.m_CoinCopyData.m_KeyCount,
                m_DiamondKeyCount = m_DB.m_DiamondCopyData.m_KeyCount,
                m_OilKeyCount = m_DB.m_OilCopyData.m_KeyCount
                
            });
        }

        private void UpdateGMAccount()
        {
            UpdateDiamond(10000); //增加钻石数量
            UpdateIron(10000); //增加引擎强化分解材料数量
            OnIncreaseMiningData((int)MiningType.GoldMine, 10000); //增加考古研究矿石数量
            OnIncreaseMiningData((int)MiningType.Hammer, 100); //增加考古矿锤数量
            OnIncreaseMiningData((int)MiningType.Bomb, 100); //增加考古炸弹数量
            OnIncreaseMiningData((int)MiningType.Scope, 100); //增加透视镜数量
            UpdateKey(100); //增加副本钥匙数量
        }

        /// <summary>
        /// 模拟玩家退出游戏
        /// </summary>
        public void DummyOnExitGame()
        {
            m_DB.m_IsFirstLogin = false;
            m_DB.m_LastGameDate = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            DummyDB.Save(m_DB);
        }

        public DummyDB DummyGetDB()
        {
            return m_DB;
        }
    }
}