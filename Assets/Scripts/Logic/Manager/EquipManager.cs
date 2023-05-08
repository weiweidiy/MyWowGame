using System.Collections.Generic;
using BreakInfinity;
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
    /// 装备管理类
    /// </summary>
    public class EquipManager : Singleton<EquipManager>
    {
        public int CurWeaponOnID { get; private set; }

        public int CurArmorOnID { get; private set; }
        
        public Dictionary<int, GameEquipData> WeaponMap { get; private set; }
        public Dictionary<int, GameEquipData> ArmorMap { get; private set; }
        
        //拥有战斗力加成
        public float AllHaveATKEffect { get; private set; }
        public float AllHaveHPEffect { get; private set; }

        public void Init(List<GameEquipData> pWeaponList, List<GameEquipData> pArmorList, int pDataWeaponOnID, int pDataArmorOnID)
        {
            CurWeaponOnID = pDataWeaponOnID;
            CurArmorOnID = pDataArmorOnID;
            
            WeaponMap = new Dictionary<int, GameEquipData>(64);
            foreach (var _Data in pWeaponList)
            {
                WeaponMap.Add(_Data.m_EquipID, _Data);
            }
            
            ArmorMap = new Dictionary<int, GameEquipData>(64);
            foreach (var _Data in pArmorList)
            {
                ArmorMap.Add(_Data.m_EquipID, _Data);
            }

            UpdateAllHaveATKEffect();
            UpdateAllHaveHPEffect();
        }

        #region 消息处理函数

        public void OnEquipOn(S2C_EquipOn pMsg)
        {
            if (pMsg.m_Type == ItemType.Weapon)
            {
                CurWeaponOnID = pMsg.m_EquipID;
                EventManager.Call(LogicEvent.EquipAllATKEffectUpdate);
            }
            else if (pMsg.m_Type == ItemType.Armor)
            {
                CurArmorOnID = pMsg.m_EquipID;
                EventManager.Call(LogicEvent.EquipAllHPEffectUpdate);
            }
            EventManager.Call(LogicEvent.EquipOn, pMsg);
        }
        
        public void OnEquipOff(S2C_EquipOff pMsg)
        {
            if (pMsg.m_Type == ItemType.Weapon)
                CurWeaponOnID = 0;
            else if (pMsg.m_Type == ItemType.Armor)
                CurArmorOnID = 0;
            EventManager.Call(LogicEvent.EquipOff, pMsg);
        }
        
        public async void OnWeaponIntensify(List<GameEquipUpgradeData> pEquipList, bool pAuto)
        {
            var _TaskNeedCount = 0;
            foreach (var equipUpgradeData in pEquipList)
            {
                var _Data = GetEquipData(equipUpgradeData.m_EquipData.m_EquipID, ItemType.Weapon);
                if (_Data != null)
                {
                    _Data.m_Level = equipUpgradeData.m_EquipData.m_Level;
                    _Data.m_Count = equipUpgradeData.m_EquipData.m_Count;
                }
                
                _TaskNeedCount += equipUpgradeData.m_EquipData.m_Level - equipUpgradeData.m_OldLevel;
            }
            
            TaskManager.Ins.DoTaskUpdate(TaskType.TT_9001, _TaskNeedCount);
            
            if(pAuto)
                await UIManager.Ins.OpenUI<UIUpgradedInfo>(pEquipList);
            else
                EventManager.Call(LogicEvent.EquipWeaponUpgraded);
            
            EventManager.Call(LogicEvent.EquipListChanged, ItemType.Weapon);
            UpdateAllHaveATKEffect();
        }
 
        public async void OnArmorIntensify(List<GameEquipUpgradeData> pEquipList, bool pAuto)
        {
            var _TaskNeedCount = 0;
            foreach (var equipUpgradeData in pEquipList)
            {
                var _Data = GetEquipData(equipUpgradeData.m_EquipData.m_EquipID, ItemType.Armor);
                if (_Data != null)
                {
                    _Data.m_Level = equipUpgradeData.m_EquipData.m_Level;
                    _Data.m_Count = equipUpgradeData.m_EquipData.m_Count;
                }
                
                _TaskNeedCount += equipUpgradeData.m_EquipData.m_Level - equipUpgradeData.m_OldLevel;
            }
            
            TaskManager.Ins.DoTaskUpdate(TaskType.TT_9002, _TaskNeedCount);
            
            if(pAuto)
                await UIManager.Ins.OpenUI<UIUpgradedInfo>(pEquipList);
            else
                EventManager.Call(LogicEvent.EquipArmorUpgraded);
            
            EventManager.Call(LogicEvent.EquipListChanged, ItemType.Armor);
            UpdateAllHaveHPEffect();
        }
        
        public void OnEquipListUpdate(S2C_EquipListUpdate pMsg)
        {
            foreach (var equipData in pMsg.m_WeaponList)
            {
                var _Data = GetEquipData(equipData.m_EquipID, ItemType.Weapon);
                if (_Data != null)
                {
                    _Data.m_Count = equipData.m_Count;
                    _Data.m_Level = equipData.m_Level;
                }
                else
                {
                    WeaponMap.Add(equipData.m_EquipID, equipData);
                }
            }
            foreach (var equipData in pMsg.m_ArmorList)
            {
                var _Data = GetEquipData(equipData.m_EquipID, ItemType.Armor);
                if (_Data != null)
                {
                    _Data.m_Count = equipData.m_Count;
                    _Data.m_Level = equipData.m_Level;
                }
                else
                {
                    ArmorMap.Add(equipData.m_EquipID, equipData);
                }
            }
            EventManager.Call(LogicEvent.EquipListChanged, ItemType.Weapon);
            EventManager.Call(LogicEvent.EquipListChanged, ItemType.Armor);
            UpdateAllHaveATKEffect();
            UpdateAllHaveHPEffect();
        }

        #endregion

        #region 通用接口
        
        //获取装备数据
        public GameEquipData GetEquipData(int pEquipID, ItemType pType)
        {
            if (pType == ItemType.Weapon)
                return WeaponMap.TryGetValue(pEquipID, out var _Data) ? _Data : null;
            if (pType == ItemType.Armor)
                return ArmorMap.TryGetValue(pEquipID, out var _Data) ? _Data : null;
            return null;
        }
        
        //是否拥有
        public bool IsHave(int pEquipID, ItemType pType)
        {
            if (pType == ItemType.Weapon)
                return WeaponMap.ContainsKey(pEquipID);
            if (pType == ItemType.Armor)
                return ArmorMap.ContainsKey(pEquipID);
            return false;
        }
        
        //当前拥有的数量
        public int CurCount(int pEquipID, ItemType pType)
        {
            if (pType == ItemType.Weapon)
                return WeaponMap.TryGetValue(pEquipID, out var _Data) ? _Data.m_Count : 0;
            if (pType == ItemType.Armor)
                return ArmorMap.TryGetValue(pEquipID, out var _Data) ? _Data.m_Count : 0;
            return 0;
        }
        
        //升级需要的数量
        public int NeedCount(int pEquipID, ItemType pType)
        {
            if(!IsHave(pEquipID,  pType))
                return EquipLvlUpCfg.GetData(1).Cost;
            var _Data = GetEquipData(pEquipID, pType);
            return _Data.m_Level > 10 ? EquipLvlUpCfg.GetData(10).Cost : EquipLvlUpCfg.GetData(_Data.m_Level).Cost;
        }
        
        //装备是否可以升级
        public bool CanUpgrade(int pEquipID, ItemType pType)
        {
            if(!IsHave(pEquipID,  pType))
                return false;
            var _Data = GetEquipData(pEquipID, pType);
            var _NeedCount = _Data.m_Level > 10 ? EquipLvlUpCfg.GetData(10).Cost : EquipLvlUpCfg.GetData(_Data.m_Level).Cost;
            return _Data.m_Count >= _NeedCount;
        }
        
        //是否有可升级的装备
        public bool HaveCanUpgradeEquip(ItemType pType)
        {
            if(pType == ItemType.Weapon)
            {
                foreach (var _MapData in WeaponMap)
                {
                    var _NeedCount = _MapData.Value.m_Level > 10 ? EquipLvlUpCfg.GetData(10).Cost : EquipLvlUpCfg.GetData(_MapData.Value.m_Level).Cost;
                    if (_MapData.Value.m_Count >= _NeedCount)
                        return true;
                } 
            }
            
            if(pType == ItemType.Armor)
            {
                foreach (var _MapData in ArmorMap)
                {
                    var _NeedCount = _MapData.Value.m_Level > 10 ? EquipLvlUpCfg.GetData(10).Cost : EquipLvlUpCfg.GetData(_MapData.Value.m_Level).Cost;
                    if (_MapData.Value.m_Count >= _NeedCount)
                        return true;
                } 
            }

            return false;
        }
        
        //获取某个装备的拥有效果
        public float GetHaveEffect(int pEquipID, ItemType pType)
        {
            var _Level = 1;
            if(IsHave(pEquipID,  pType))
                _Level = GetEquipData(pEquipID, pType).m_Level;
            var _CfgData = EquipCfg.GetData(pEquipID);
            return _CfgData.HasAdditionBase + (_Level - 1) * _CfgData.HasAdditionGrow;
        }
        
        //获取某个装备的装备效果
        public float GetEquipEffect(int pEquipID, ItemType pType)
        {
            var _Level = 1;
            if(IsHave(pEquipID,  pType))
                _Level = GetEquipData(pEquipID, pType).m_Level;
            var _CfgData = EquipCfg.GetData(pEquipID);
            return _CfgData.ValueBase + (_Level - 1) * _CfgData.ValueGrow;
        }

        //获取所有已有的加成值
        public void UpdateAllHaveATKEffect()
        {
            float _AllEffect = 0;
            foreach (var _MapData in WeaponMap)
            {
                var _CfgData = EquipCfg.GetData(_MapData.Key);
                var _GameData = _MapData.Value;
                _AllEffect += (_CfgData.HasAdditionBase + (_GameData.m_Level - 1) * _CfgData.HasAdditionGrow);
            }
            
            AllHaveATKEffect = _AllEffect;
            EventManager.Call(LogicEvent.EquipAllATKEffectUpdate);
        }
        
        public void UpdateAllHaveHPEffect()
        {
            float _AllEffect = 0;
            foreach (var _MapData in ArmorMap)
            {
                var _CfgData = EquipCfg.GetData(_MapData.Key);
                var _GameData = _MapData.Value;
                _AllEffect += (_CfgData.HasAdditionBase + (_GameData.m_Level - 1) * _CfgData.HasAdditionGrow);
            }
            
            AllHaveHPEffect = _AllEffect;
            EventManager.Call(LogicEvent.EquipAllHPEffectUpdate);
        }

        public int GetOnEquip(ItemType pEquipType)
        {
            if (pEquipType == ItemType.Weapon)
                return CurWeaponOnID;
            if (pEquipType == ItemType.Armor)
                return CurArmorOnID;
            return 0;
        }

        //攻击力 总加成 (当前装备 + 所有获取的效果)
        public BigDouble GetWeaponAdd()
        {
            if(CurWeaponOnID == 0)
                return AllHaveATKEffect;
            return GetEquipEffect(CurWeaponOnID, ItemType.Weapon) + AllHaveATKEffect;
        }
        
        //血量 总加成 (当前装备 + 所有获取的效果)
        public BigDouble GetArmorAdd()
        {
            if(CurArmorOnID == 0)
                return AllHaveHPEffect;
            return GetEquipEffect(CurArmorOnID, ItemType.Armor) + AllHaveHPEffect;
        }
        
        #endregion
        
        #region 操作接口

        public void DoOn(int pEquipID, ItemType pType)
        {
            NetworkManager.Ins.SendMsg(new C2S_EquipOn()
            {
                m_EquipID = pEquipID, m_Type = pType
            });
        }
        
        
        public void DoIntensify(int pEquipID, ItemType pType, bool pIsAuto)
        { 
            NetworkManager.Ins.SendMsg(new C2S_EquipIntensify()
            {
                m_EquipID = pEquipID, m_Type = pType, m_IsAuto = pIsAuto,
            });
        }

        #endregion
    }
}