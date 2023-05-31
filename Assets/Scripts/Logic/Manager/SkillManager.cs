using System.Collections.Generic;
using System.Linq;
using Configs;
using Framework.EventKit;
using Framework.Extension;
using Framework.UI;
using Logic.Common;
using Logic.Fight.Common;
using Logic.UI.UIUser;
using Networks;

namespace Logic.Manager
{
    /// <summary>
    /// 技能管理类
    /// </summary>
    public class SkillManager : Singleton<SkillManager>
    {
        public Dictionary<int, GameSkillData> SkillMap { get; private set; }
        public List<int> SkillOnList { get; private set; }

        // 技能可上阵数量
        private int m_CanDoOnCount;

        //拥有战斗力加成
        public float AllHaveEffect { get; private set; }

        public void Init(List<GameSkillData> pDataSkillList, List<int> pDataSkillOnList)
        {
            SkillMap = new Dictionary<int, GameSkillData>(64);
            foreach (var _Data in pDataSkillList)
            {
                SkillMap.Add(_Data.SkillID, _Data);
            }

            SkillOnList = pDataSkillOnList;

            UpdateAllHaveEffect();
            UpdateCanDoOnCount();
        }

        #region 消息处理函数

        public void OnSkillOn(S2C_SkillOn pMsg)
        {
            SkillOnList[pMsg.Index] = pMsg.SkillID;
            EventManager.Call(LogicEvent.SkillOn, pMsg);
        }

        public void OnSkillOff(S2C_SkillOff pMsg)
        {
            SkillOnList[pMsg.Index] = 0;
            EventManager.Call(LogicEvent.SkillOff, pMsg);
        }

        public async void OnSkillIntensify(S2C_SkillIntensify pMsg)
        {
            var _TaskNeedCount = 0;
            foreach (var skillUpgradeData in pMsg.SkillList)
            {
                var _Data = GetSkillData(skillUpgradeData.SkillData.SkillID);
                if (_Data != null)
                {
                    _Data.Level = skillUpgradeData.SkillData.Level;
                    _Data.Count = skillUpgradeData.SkillData.Count;
                }

                _TaskNeedCount += skillUpgradeData.SkillData.Level - skillUpgradeData.OldLevel;
            }

            TaskManager.Ins.DoTaskUpdate(TaskType.TT_9003, _TaskNeedCount);

            if (pMsg.IsAuto)
                await UIManager.Ins.OpenUI<UIUpgradedInfo>(pMsg.SkillList);
            else
                EventManager.Call(LogicEvent.SkillUpgraded);

            EventManager.Call(LogicEvent.SkillListChanged);

            UpdateAllHaveEffect();
        }

        public void OnSkillListUpdate(S2C_SkillListUpdate pMsg)
        {
            foreach (var _Data in pMsg.SkillList)
            {
                var _SkillData = GetSkillData(_Data.SkillID);
                if (_SkillData == null)
                    SkillMap.Add(_Data.SkillID, _Data);
                else
                {
                    _SkillData.Level = _Data.Level;
                    _SkillData.Count = _Data.Count;
                }
            }

            EventManager.Call(LogicEvent.SkillListChanged);
            UpdateAllHaveEffect();
        }

        #endregion

        #region 通用接口

        //获取技能数据
        public GameSkillData GetSkillData(int pSkillID)
        {
            return SkillMap.TryGetValue(pSkillID, out var _Data) ? _Data : null;
        }

        //技能是否可以上阵
        public bool CanDoOn()
        {
            if (m_CanDoOnCount > 0) return true;
            EventManager.Call(LogicEvent.ShowTips, "当前无法上阵更多技能");
            return false;
        }

        /// <summary>
        /// 当技能栏有技能上阵时更新技能可上阵数量
        /// </summary>
        private void UpdateCanDoOnCount()
        {
            foreach (var skillId in SkillOnList.Where(skillId => skillId != 0))
            {
                m_CanDoOnCount--;
            }
        }

        /// <summary>
        /// 开放所有技能上阵
        /// </summary>
        public void UpdateAllDoOnCount()
        {
            m_CanDoOnCount += SkillOnList.Count;
        }

        /// <summary>
        /// 开放解锁更新技能可上阵数量
        /// </summary>
        /// <param name="lockTypeId"></param>
        public void UpdateCanDoOnCount(int lockTypeId)
        {
            var lockType = (LockType)lockTypeId;
            switch (lockType)
            {
                case LockType.LT_700:
                case LockType.LT_800:
                case LockType.LT_900:
                case LockType.LT_1000:
                case LockType.LT_1100:
                case LockType.LT_1200:
                    m_CanDoOnCount++;
                    break;
            }
        }

        //是否上阵
        public bool IsOn(int pSkillID)
        {
            return SkillOnList.Contains(pSkillID);
        }

        //上阵是否满了
        public bool InOnFull()
        {
            foreach (var ID in SkillOnList)
            {
                if (ID == 0) return false;
            }

            return true;
        }

        //是否拥有
        public bool IsHave(int pSkillID)
        {
            return SkillMap.ContainsKey(pSkillID);
        }

        //当前数量
        public int CurCount(int pSkillID)
        {
            return SkillMap.TryGetValue(pSkillID, out var _Data) ? _Data.Count : 0;
        }

        //升级需要的数量
        public int UpgradeNeedCount(int pSkillID)
        {
            if (!IsHave(pSkillID)) return SkillLvlUpCfg.GetData(1).Cost;

            var _Data = GetSkillData(pSkillID);
            return _Data.Level > 10 ? SkillLvlUpCfg.GetData(10).Cost : SkillLvlUpCfg.GetData(_Data.Level).Cost;
        }

        //技能是否可以升级
        public bool CanUpgrade(int pSkillID)
        {
            if (!IsHave(pSkillID)) return false;

            var _Data = GetSkillData(pSkillID);
            var _NeedCount = _Data.Level > 10
                ? SkillLvlUpCfg.GetData(10).Cost
                : SkillLvlUpCfg.GetData(_Data.Level).Cost;
            return _Data.Count >= _NeedCount;
        }

        //是否有技能可以升级
        public bool HasOneCanUpgrade()
        {
            foreach (var skillData in SkillMap.Values)
            {
                var _NeedCount = skillData.Level > 10
                    ? SkillLvlUpCfg.GetData(10).Cost
                    : SkillLvlUpCfg.GetData(skillData.Level).Cost;
                if (skillData.Count >= _NeedCount)
                    return true;
            }

            return false;
        }

        //获取某个技能的拥有效果
        public float GetHaveEffect(int pSkillID)
        {
            var _Level = 1;
            if (IsHave(pSkillID))
                _Level = GetSkillData(pSkillID).Level;

            var _CfgData = SkillCfg.GetData(pSkillID);
            return _CfgData.HasAdditionBase + (_Level - 1) * _CfgData.HasAdditionGrow;
        }

        //获取所有已有技能的加成值
        public void UpdateAllHaveEffect()
        {
            float _AllEffect = 0;
            foreach (var _MapData in SkillMap)
            {
                var _CfgData = SkillCfg.GetData(_MapData.Key);
                var _GameData = _MapData.Value;
                _AllEffect += (_CfgData.HasAdditionBase + (_GameData.Level - 1) * _CfgData.HasAdditionGrow);
            }

            AllHaveEffect = _AllEffect;
            EventManager.Call(LogicEvent.SkillAllEffectUpdate);
        }

        #endregion

        #region 操作接口

        //检查当前技能是否可以上下阵
        //Doing状态不能操作
        public bool CheckCanOperation(int pSkillID)
        {
            var _skillBase = FightSkillManager.Ins.GetSkillObject(pSkillID);
            if (_skillBase == null) return false;
            return _skillBase.CanOperation();
        }

        public void DoOn(int pSkillID)
        {
            m_CanDoOnCount--;
            NetworkManager.Ins.SendMsg(new C2S_SkillOn()
            {
                SkillID = pSkillID,
            });
        }

        public void DoOff(int pSkillID)
        {
            m_CanDoOnCount++;
            NetworkManager.Ins.SendMsg(new C2S_SkillOff()
            {
                SkillID = pSkillID,
            });
        }

        public void DoIntensify(int pSkillID, bool pIsAuto)
        {
            NetworkManager.Ins.SendMsg(new C2S_SkillIntensify()
            {
                SkillID = pSkillID,
                IsAuto = pIsAuto,
            });
        }

        #endregion
    }
}