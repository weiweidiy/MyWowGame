using System;
using Framework.Extension;
using Framework.Helper;
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

            //Debug.Log($"===> Dummy-ReceiveMsg : [{_Msg.MsgType}]");

            switch (_Msg.MsgType)
            {
                case NetWorkMsgType.C2S_Login:
                    On_C2S_Login();
                    break;
                case NetWorkMsgType.C2S_GMCommand:
                    // 临时增加客户端向服务器请求GM数据功能
                    UpdateGMAccount();
                    break;
                case NetWorkMsgType.C2S_SyncCoin:
                    var _C2S_SyncCoin = _Msg as C2S_SyncCoin;
                    m_DB.m_Coin = _C2S_SyncCoin.Coin;
                    //Debug.LogError("=====RECEIVE : C2S_SyncCoin=====");
                    DummyDB.Save(m_DB);
                    break;
                case NetWorkMsgType.C2S_SyncTrophy:
                    var _C2S_SyncTrophy = _Msg as C2S_SyncTrophy;
                    m_DB.m_Trophy = _C2S_SyncTrophy.Trophy;
                    //Debug.LogError("=====RECEIVE : C2S_SyncTrophy=====");
                    DummyDB.Save(m_DB);
                    break;
                case NetWorkMsgType.C2S_SyncRoomData:
                    On_C2S_SyncRoomData(_Msg as C2S_SyncRoomData);
                    break;
                case NetWorkMsgType.C2S_SyncSettingData:
                    On_C2S_SyncSettingData(_Msg as C2S_SyncSettingData);
                    break;
                case NetWorkMsgType.C2S_SyncLevelData:
                    On_C2S_SyncLevelData(_Msg as C2S_SyncLevelData);
                    break;
                case NetWorkMsgType.C2S_SyncPlaceRewardData:
                    On_C2S_SyncPlaceRewardData(_Msg as C2S_SyncPlaceRewardData);
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
                case NetWorkMsgType.C2S_QuenchingLock:
                    On_C2S_QuenchingLock(_Msg as C2S_QuenchingLock);
                    break;
                case NetWorkMsgType.C2S_Quenching:
                    On_C2S_Quenching(_Msg as C2S_Quenching);
                    break;
                case NetWorkMsgType.C2S_SpoilDraw:
                    On_C2S_SpoilDraw(_Msg as C2S_SpoilDraw);
                    break;
                case NetWorkMsgType.C2S_SpoilEquip:
                    On_C2S_SpoilEquip(_Msg as C2S_SpoilEquip);
                    break;
                case NetWorkMsgType.C2S_SpoilUpgrade:
                    On_C2S_SpoilUpgrade(_Msg as C2S_SpoilUpgrade);
                    break;
                case NetWorkMsgType.C2S_SpoilBreakthrough:
                    On_C2S_SpoilBreakthrough(_Msg as C2S_SpoilBreakthrough);
                    break;
                case NetWorkMsgType.C2S_RoleOn:
                    On_C2S_RoleOn(_Msg as C2S_RoleOn);
                    break;
                case NetWorkMsgType.C2S_RoleIntensify:
                    On_C2S_RoleIntensify(_Msg as C2S_RoleIntensify);
                    break;
                case NetWorkMsgType.C2S_RoleBreak:
                    On_C2S_RoleBreak(_Msg as C2S_RoleBreak);
                    break;
                default:
                {
                    Debug.LogError(_Msg.MsgType);
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
                LastGameDate = m_DB.m_LastGameDate,

                Coin = m_DB.m_Coin,
                Diamond = m_DB.m_Diamond,
                Iron = m_DB.m_Iron,
                Oil = m_DB.m_Oil,
                Trophy = m_DB.m_Trophy,
                MushRoom = m_DB.m_MushRoom,
                BreakOre = m_DB.m_BreakOre,

                GJJAtkLevel = m_DB.m_GJJAtkLevel,
                GJJHPLevel = m_DB.m_GJJHPLevel,
                GJJHPRecoverLevel = m_DB.m_GJJHPRecoverLevel,
                GJJCriticalLevel = m_DB.m_GJJCriticalLevel,
                GJJCriticalDamageLevel = m_DB.m_GJJCriticalDamageLevel,
                GJJAtkSpeedLevel = m_DB.m_GJJAtkSpeedLevel,
                GJJDoubleHitLevel = m_DB.m_GJJDoubleHitLevel,
                GJJTripletHitLevel = m_DB.m_GJJTripletHitLevel,

                AutoSkill = m_DB.m_AutoSkill,
                IsMusicOn = m_DB.m_IsMusicOn,
                IsSoundOn = m_DB.m_IsSoundOn,

                CurLevelID = m_DB.m_CurLevelID,
                CurLevelNode = m_DB.m_CurLevelNode,
                CurLevelState = m_DB.m_CurLevelState,

                PlaceRewardClickTime = m_DB.m_BtnPlaceRewardClickTime,
                PlaceRewardShowTime = m_DB.m_BtnPlaceRewardShowTime,
                PlaceRewardPopTime = m_DB.m_PopPlaceRewardTime,

                WeaponOnID = m_DB.m_WeaponOnID,
                ArmorOnID = m_DB.m_ArmorOnID,
                EngineOnId = m_DB.m_EngineOnId,
                EngineGetId = m_DB.m_EngineGetId,

                MainTaskCount = m_DB.m_MainTaskCount,

                RoleOnId = m_DB.m_RoleOnId,
            };

            _Data.PartnerOnList.AddRange(m_DB.m_PartnerOnList);
            _Data.SkillOnList.AddRange(m_DB.m_SkillOnList);

            //!!! 引用类型 深拷贝 !!!

            foreach (var partnerData in m_DB.m_PartnerList)
            {
                _Data.PartnerList.Add(partnerData.Clone());
            }

            foreach (var data in m_DB.m_WeaponList)
            {
                _Data.WeaponList.Add(data.Clone());
            }

            foreach (var data in m_DB.m_ArmorList)
            {
                _Data.ArmorList.Add(data.Clone());
            }

            foreach (var skillData in m_DB.m_SkillList)
            {
                _Data.SkillList.Add(skillData.Clone());
            }

            foreach (var engineData in m_DB.m_EngineList)
            {
                _Data.EngineList.Add(engineData.Clone());
            }

            // 抽卡
            _Data.ShopSkillData = m_DB.m_ShopSkillData.Clone();
            _Data.ShopPartnerData = m_DB.m_ShopPartnerData.Clone();
            _Data.ShopEquipData = m_DB.m_ShopEquipData.Clone();

            //任务
            _Data.CurMainTask = m_DB.m_CurMainTask.Clone();
            foreach (var _Task in m_DB.m_DailyTaskList)
            {
                _Data.DailyTaskList.Add(_Task.Clone());
            }

            //副本
            _Data.DiamondCopyData = m_DB.m_DiamondCopyData.Clone();
            _Data.CoinCopyData = m_DB.m_CoinCopyData.Clone();
            _Data.OilCopyData = m_DB.m_OilCopyData.Clone();
            _Data.TrophyCopyData = m_DB.m_TropyCopyData.Clone();

            //考古
            _Data.MiningData = m_DB.m_MiningData.Clone();

            //开放剧情
            foreach (var lockStoryData in m_DB.m_LockStoryList)
            {
                _Data.LockStoryList.Add(lockStoryData.Clone());
            }

            //考古研究
            foreach (var researchData in m_DB.m_ResearchList)
            {
                _Data.ResearchList.Add(researchData.Clone());
            }

            _Data.ResearchEffectData = m_DB.m_ResearchEffectData.Clone();

            //淬炼数据
            foreach (var quenchingData in m_DB.m_QuenchingList)
            {
                _Data.QuenchingList.Add(quenchingData.Clone());
            }


            //战利品数据
            _Data.SpoilDrawProgress = m_DB.m_SpoilDrawProgress;
            _Data.SpoilSlotsUnlockData.AddRange(m_DB.m_SpoilSlotsUnlockData);
            foreach (var item in m_DB.m_SpoilSlotsData)
            {
                _Data.SpoilSlotsData.Add(item.Clone());
            }

            foreach (var item in m_DB.m_SpoilsData)
            {
                _Data.SpoilsData.Add(item.Clone());
            }

            foreach(var item in m_DB.m_SpoilBreakthrough)
            {
                _Data.SpoilBreakthroughData.Add(item.Clone());
            }


            //英雄
            foreach (var roleData in m_DB.m_RoleList)
            {
                _Data.RoleList.Add(roleData.Clone());
            }


            SendMsg(_Data);
        }

        private void On_C2S_SyncRoomData(C2S_SyncRoomData pMsg)
        {
            m_DB.m_GJJAtkLevel = pMsg.GJJAtkLevel;
            m_DB.m_GJJHPLevel = pMsg.GJJHPLevel;
            m_DB.m_GJJHPRecoverLevel = pMsg.GJJHPRecoverLevel;
            m_DB.m_GJJCriticalLevel = pMsg.GJJCriticalLevel;
            m_DB.m_GJJCriticalDamageLevel = pMsg.GJJCriticalDamageLevel;
            m_DB.m_GJJAtkSpeedLevel = pMsg.GJJAtkSpeedLevel;
            m_DB.m_GJJDoubleHitLevel = pMsg.GJJDoubleHitLevel;
            m_DB.m_GJJTripletHitLevel = pMsg.GJJTripletHitLevel;

            //Debug.LogError("=====RECEIVE : C2S_SyncRoomData=====");
            DummyDB.Save(m_DB);
        }

        private void On_C2S_SyncSettingData(C2S_SyncSettingData pMsg)
        {
            m_DB.m_AutoSkill = pMsg.AutoSkill;
            m_DB.m_IsMusicOn = pMsg.IsMusicOn;
            m_DB.m_IsSoundOn = pMsg.IsSoundOn;

            //Debug.LogError("=====RECEIVE : C2S_SyncSettingData=====");
            DummyDB.Save(m_DB);
        }

        private void On_C2S_SyncLevelData(C2S_SyncLevelData pMsg)
        {
            m_DB.m_CurLevelID = pMsg.CurLevelID;
            m_DB.m_CurLevelNode = pMsg.CurLevelNode;
            m_DB.m_CurLevelState = pMsg.CurLevelState;

            //Debug.LogError("=====RECEIVE : C2S_SyncLevelData=====");
            DummyDB.Save(m_DB);
        }

        private void On_C2S_SyncPlaceRewardData(C2S_SyncPlaceRewardData pMsg)
        {
            m_DB.m_BtnPlaceRewardClickTime = pMsg.PlaceRewardClickTime;
            m_DB.m_BtnPlaceRewardShowTime = pMsg.PlaceRewardShowTime;
            m_DB.m_PopPlaceRewardTime = pMsg.PlaceRewardPopTime;

            //Debug.LogError("=====RECEIVE : C2S_SyncPlaceRewardData=====");
            DummyDB.Save(m_DB);
        }

        private void On_C2S_EquipOn(C2S_EquipOn pMsg)
        {
            DoEquipOn(pMsg.EquipID, pMsg.Type);
        }

        private void On_C2S_EquipIntensify(C2S_EquipIntensify pMsg)
        {
            if (pMsg.IsAuto)
                EquipIntensifyAuto(pMsg.Type);
            else
                EquipIntensify(pMsg.EquipID, pMsg.Type);
        }

        private void On_C2S_PartnerOn(C2S_PartnerOn pMsg)
        {
            DoPartnerOn(pMsg.PartnerID);
        }

        private void On_C2S_PartnerOff(C2S_PartnerOff pMsg)
        {
            DoPartnerOff(pMsg.PartnerID);
        }

        private void On_C2S_PartnerIntensify(C2S_PartnerIntensify pMsg)
        {
            if (pMsg.IsAuto)
                PartnerIntensifyAuto();
            else
                PartnerIntensify(pMsg.PartnerID);
        }

        private void On_C2S_SkillOn(C2S_SkillOn pMsg)
        {
            DoSkillOn(pMsg.SkillID);
        }

        private void On_C2S_SkillOff(C2S_SkillOff pMsg)
        {
            DoSkillOff(pMsg.SkillID);
        }

        private void On_C2S_SkillIntensify(C2S_SkillIntensify pMsg)
        {
            if (pMsg.IsAuto)
                SkillIntensifyAuto();
            else
                SkillIntensify(pMsg.SkillID);
        }

        private void On_C2S_EngineIntensify(C2S_EngineIntensify pMsg)
        {
            DoEngineIntensify(pMsg.EngineId);
        }

        private void On_C2S_EngineRemove(C2S_EngineRemove pMsg)
        {
            DoEngineRemove(pMsg.EngineId);
        }

        private void On_C2S_EngineOn(C2S_EngineOn pMsg)
        {
            DoEngineOn(pMsg.EngineId);
        }

        private void On_C2S_EngineOff(C2S_EngineOff pMsg)
        {
            DoEngineOff(pMsg.EngineId);
        }

        private void On_C2S_DrawCard(C2S_DrawCard pMsg)
        {
            switch ((DrawCardType)pMsg.DrawCardType)
            {
                case DrawCardType.Skill:
                    OnDrawSkill(pMsg.DrawCardCostType);
                    break;
                case DrawCardType.Partner:
                    OnDrawPartner(pMsg.DrawCardCostType);
                    break;
                case DrawCardType.Equip:
                    OnDrawEquip(pMsg.DrawCardCostType);
                    break;
            }
        }

        private void On_C2S_UpdateDrawCardData(C2S_UpdateDrawCardData pMsg)
        {
            switch ((DrawCardType)pMsg.DrawCardType)
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
            OnMiningReward(pMsg.TreasureType);
        }

        public void On_C2S_PlaceReward(C2S_PlaceReward pMsg)
        {
            OnPlaceReward(pMsg.PlaceRewardCount);
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
            var miningUpdateType = (MiningUpdateType)pMsg.UpdateType;
            switch (miningUpdateType)
            {
                case MiningUpdateType.Reduce:
                    OnReduceMiningData(pMsg.MiningDataType);
                    break;
                case MiningUpdateType.Increase:
                    OnIncreaseMiningData(pMsg.MiningDataType);
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

        public void On_C2S_QuenchingLock(C2S_QuenchingLock pMsg)
        {
            OnQuenchingLock(pMsg);
        }

        public void On_C2S_Quenching(C2S_Quenching pMsg)
        {
            OnQuenching(pMsg);
        }


        private void On_C2S_SpoilDraw(C2S_SpoilDraw pMsg)
        {
            OnC2S_SpoilDraw(pMsg);
        }

        private void On_C2S_SpoilEquip(C2S_SpoilEquip pMsg)
        {
            OnC2S_SpoilEquip(pMsg);
        }

        private void On_C2S_SpoilUpgrade(C2S_SpoilUpgrade pMsg)
        {
            OnC2S_SpoilUpgrade(pMsg);
        }

        private void On_C2S_SpoilBreakthrough(C2S_SpoilBreakthrough c2S_SpoilBreakthrough)
        {
            OnC2S_SpoilBreakthrough(c2S_SpoilBreakthrough);
        }


        public void On_C2S_RoleOn(C2S_RoleOn pMsg)
        {
            DoRoleOn(pMsg.RoleId);
        }

        public void On_C2S_RoleIntensify(C2S_RoleIntensify pMsg)
        {
            DoRoleIntensify(pMsg.RoleId);
        }

        public void On_C2S_RoleBreak(C2S_RoleBreak pMsg)
        {
            DoRoleBreak(pMsg.RoleId);
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
            SendMsg(new S2C_DiamondUpdate { Diamond = m_DB.m_Diamond });
        }

        public void UpdateOil(int pOil)
        {
            m_DB.m_Oil += pOil;
            DummyDB.Save(m_DB);
            SendMsg(new S2C_OilUpdate { Oil = m_DB.m_Oil });
        }

        /// <summary>
        /// 更新引擎强化分解材料
        /// </summary>
        /// <param name="pIron"></param>
        public void UpdateIron(int pIron)
        {
            m_DB.m_Iron += pIron;
            DummyDB.Save(m_DB);
            SendMsg(new S2C_EngineIronUpdate { Iron = m_DB.m_Iron });
        }

        /// <summary>
        /// 更新副本钥匙
        /// </summary>
        public void UpdateKey(int pKey)
        {
            m_DB.m_CoinCopyData.KeyCount += pKey;
            m_DB.m_DiamondCopyData.KeyCount += pKey;
            m_DB.m_OilCopyData.KeyCount += pKey;
            m_DB.m_TropyCopyData.KeyCount += pKey;
            DummyDB.Save(m_DB);
            SendMsg(new S2C_UpdateCopyKeyCount
            {
                CoinKeyCount = m_DB.m_CoinCopyData.KeyCount,
                DiamondKeyCount = m_DB.m_DiamondCopyData.KeyCount,
                OilKeyCount = m_DB.m_OilCopyData.KeyCount,
                TrophyKeyCount = m_DB.m_TropyCopyData.KeyCount
            });
        }

        /// <summary>
        /// 更新英雄升级材料
        /// </summary>
        public void UpdateMushRoom(int pMushRoom)
        {
            m_DB.m_MushRoom += pMushRoom;
            DummyDB.Save(m_DB);
            SendMsg(new S2C_MushRoomUpdate { MushRoom = m_DB.m_MushRoom });
        }

        /// <summary>
        /// 更新英雄突破材料
        /// </summary>
        public void UpdateBreakOre(int pBreakOre)
        {
            m_DB.m_BreakOre += pBreakOre;
            DummyDB.Save(m_DB);
            SendMsg(new S2C_BreakOreUpdate() { BreakOre = m_DB.m_BreakOre });
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
            UpdateOil(10000); //增加原油数量
            UpdateMushRoom(100); //增加英雄升级材料
            UpdateBreakOre(1000); //增加英雄突破材料
        }

        /// <summary>
        /// 模拟玩家退出游戏
        /// </summary>
        public void DummyOnExitGame()
        {
            m_DB.m_IsFirstLogin = false;
            m_DB.m_LastGameDate = TimeHelper.GetUnixTimeStamp();
            // DummyDB.Save(m_DB);
        }

        public DummyDB DummyGetDB()
        {
            return m_DB;
        }
    }
}