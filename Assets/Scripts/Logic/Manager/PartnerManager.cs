using System.Collections.Generic;
using System.Linq;
using Configs;
using Framework.EventKit;
using Framework.Extension;
using Framework.UI;
using Logic.Common;
using Logic.UI.UIUser;
using Networks;

namespace Logic.Manager
{
    /// <summary>
    /// 伙伴管理类
    /// </summary>
    public class PartnerManager : Singleton<PartnerManager>
    {
        public Dictionary<int, GamePartnerData> PartnerMap { get; private set; }
        public List<int> PartnerOnList { get; private set; }

        // 伙伴可上阵数量
        private int m_CanDoOnCount;

        //拥有战斗力加成
        public float AllHaveEffect { get; private set; }

        public void Init(List<GamePartnerData> pDataPartnerList, List<int> pDataPartnerOnList)
        {
            PartnerMap = new Dictionary<int, GamePartnerData>(64);
            foreach (var _Data in pDataPartnerList)
            {
                PartnerMap.Add(_Data.m_PartnerID, _Data);
            }

            PartnerOnList = pDataPartnerOnList;

            UpdateAllHaveEffect();
            UpdateCanDoOnCount();
        }

        #region 协议处理接口

        public void OnPartnerOn(S2C_PartnerOn pMsg)
        {
            PartnerOnList[pMsg.m_Index] = pMsg.m_PartnerID;
            EventManager.Call(LogicEvent.PartnerOn, pMsg);
        }

        public void OnPartnerOff(S2C_PartnerOff pMsg)
        {
            PartnerOnList[pMsg.m_Index] = 0;
            EventManager.Call(LogicEvent.PartnerOff, pMsg);
        }

        public async void OnPartnerIntensify(S2C_PartnerIntensify pMsg)
        {
            var _TaskNeedCount = 0;
            foreach (var upgradeData in pMsg.m_PartnerList)
            {
                var _Data = GetPartnerData(upgradeData.m_PartnerData.m_PartnerID);
                if (_Data != null)
                {
                    _Data.m_Level = upgradeData.m_PartnerData.m_Level;
                    _Data.m_Count = upgradeData.m_PartnerData.m_Count;
                }

                _TaskNeedCount += upgradeData.m_PartnerData.m_Level - upgradeData.m_OldLevel;
            }

            TaskManager.Ins.DoTaskUpdate(TaskType.TT_9004, _TaskNeedCount);

            if (pMsg.m_IsAuto)
                await UIManager.Ins.OpenUI<UIUpgradedInfo>(pMsg.m_PartnerList);
            else
                EventManager.Call(LogicEvent.PartnerUpgraded);

            EventManager.Call(LogicEvent.PartnerListChanged);

            UpdateAllHaveEffect();
        }

        public void OnPartnerListUpdate(S2C_PartnerListUpdate pMsg)
        {
            foreach (var _Data in pMsg.m_PartnerList)
            {
                var _PData = GetPartnerData(_Data.m_PartnerID);
                if (_PData != null)
                {
                    _PData.m_Level = _Data.m_Level;
                    _PData.m_Count = _Data.m_Count;
                }
                else
                    PartnerMap.Add(_Data.m_PartnerID, _Data);
            }

            EventManager.Call(LogicEvent.PartnerListChanged);
            UpdateAllHaveEffect();
        }

        #endregion

        #region 通用接口

        //获取伙伴数据
        public GamePartnerData GetPartnerData(int pPartnerID)
        {
            return PartnerMap.TryGetValue(pPartnerID, out var _Data) ? _Data : null;
        }

        //伙伴是否可以上阵
        public bool CanDoOn()
        {
            if (m_CanDoOnCount > 0) return true;
            EventManager.Call(LogicEvent.ShowTips, "当前无法上阵更多伙伴");
            return false;
        }

        /// <summary>
        /// 当伙伴栏有伙伴上阵时更新伙伴可上阵数量
        /// </summary>
        private void UpdateCanDoOnCount()
        {
            foreach (var partnerId in PartnerOnList.Where(partnerId => partnerId != 0))
            {
                m_CanDoOnCount--;
            }
        }

        /// <summary>
        /// 开放所有伙伴上阵
        /// </summary>
        public void UpdateAllDoOnCount()
        {
            m_CanDoOnCount += PartnerOnList.Count;
        }

        /// <summary>
        /// 开放解锁更新伙伴可上阵数量
        /// </summary>
        /// <param name="lockTypeId"></param>
        public void UpdateCanDoOnCount(int lockTypeId)
        {
            var lockType = (LockType)lockTypeId;
            switch (lockType)
            {
                case LockType.LT_1300:
                case LockType.LT_1400:
                case LockType.LT_1500:
                case LockType.LT_1600:
                case LockType.LT_1700:
                    m_CanDoOnCount++;
                    break;
            }
        }

        //是否上阵
        public bool IsOn(int pPartnerID)
        {
            return PartnerOnList.Contains(pPartnerID);
        }

        //上阵是否满了
        public bool InOnFull()
        {
            foreach (var ID in PartnerOnList)
            {
                if (ID == 0) return false;
            }

            return true;
        }

        //是否拥有
        public bool IsHave(int pPartnerID)
        {
            return PartnerMap.ContainsKey(pPartnerID);
        }

        //当前数量
        public int CurCount(int pPartnerID)
        {
            return PartnerMap.TryGetValue(pPartnerID, out var _Data) ? _Data.m_Count : 0;
        }

        //升级需要的数量
        public int UpgradeNeedCount(int pPartnerID)
        {
            if (!IsHave(pPartnerID))
                return PartnerLvlUpCfg.GetData(1).Cost;

            var _Data = GetPartnerData(pPartnerID);
            return _Data.m_Level > 10 ? PartnerLvlUpCfg.GetData(10).Cost : PartnerLvlUpCfg.GetData(_Data.m_Level).Cost;
        }

        //是否可以升级
        public bool CanUpgrade(int pPartnerID)
        {
            if (!IsHave(pPartnerID)) return false;

            var _Data = GetPartnerData(pPartnerID);
            var _NeedCount = _Data.m_Level > 10
                ? PartnerLvlUpCfg.GetData(10).Cost
                : PartnerLvlUpCfg.GetData(_Data.m_Level).Cost;
            return _Data.m_Count >= _NeedCount;
        }

        //是否有可以升级
        public bool HasOneCanUpgrade()
        {
            foreach (var partnerData in PartnerMap.Values)
            {
                var _NeedCount = partnerData.m_Level > 10
                    ? PartnerLvlUpCfg.GetData(10).Cost
                    : PartnerLvlUpCfg.GetData(partnerData.m_Level).Cost;
                if (partnerData.m_Count >= _NeedCount)
                    return true;
            }

            return false;
        }

        //获取某个伙伴的拥有效果
        public float GetHaveEffect(int pPartnerID)
        {
            var _Level = 1;
            if (IsHave(pPartnerID))
                _Level = GetPartnerData(pPartnerID).m_Level;

            var _CfgData = PartnerCfg.GetData(pPartnerID);
            return _CfgData.HasAdditionBase + (_Level - 1) * _CfgData.HasAdditionGrow;
        }

        //获取所有已有伙伴的加成值
        public void UpdateAllHaveEffect()
        {
            float _AllEffect = 0;
            foreach (var _MapData in PartnerMap)
            {
                var _CfgData = PartnerCfg.GetData(_MapData.Key);
                var _GameData = _MapData.Value;
                _AllEffect += (_CfgData.HasAdditionBase + (_GameData.m_Level - 1) * _CfgData.HasAdditionGrow);
            }

            AllHaveEffect = _AllEffect;
            EventManager.Call(LogicEvent.PartnerAllEffectUpdate);
        }

        #endregion

        #region 操作接口

        public void DoOn(int pPartner)
        {
            m_CanDoOnCount--;
            NetworkManager.Ins.SendMsg(new C2S_PartnerOn()
            {
                m_PartnerID = pPartner,
            });
        }

        public void DoOff(int pPartner)
        {
            m_CanDoOnCount++;
            NetworkManager.Ins.SendMsg(new C2S_PartnerOff()
            {
                m_PartnerID = pPartner,
            });
        }

        public void DoIntensify(int pPartner, bool pIsAuto)
        {
            NetworkManager.Ins.SendMsg(new C2S_PartnerIntensify()
            {
                m_PartnerID = pPartner,
                m_IsAuto = pIsAuto,
            });
        }

        #endregion
    }
}