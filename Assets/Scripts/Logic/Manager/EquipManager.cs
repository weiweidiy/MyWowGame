using System.Collections.Generic;
using BreakInfinity;
using Configs;
using Framework.EventKit;
using Framework.Extension;
using Framework.UI;
using Logic.Common;
using Logic.UI.UIUser;
using Networks;
using UnityEngine;

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

        /// <summary>
        /// 缓存当前最高攻击属性的武器id
        /// </summary>
        public int BestWeaponId { get; private set; }

        public int BestArmorId { get; private set; }

        public void Init(List<GameEquipData> pWeaponList, List<GameEquipData> pArmorList, int pDataWeaponOnID,
            int pDataArmorOnID)
        {
            CurWeaponOnID = pDataWeaponOnID;
            CurArmorOnID = pDataArmorOnID;

            WeaponMap = new Dictionary<int, GameEquipData>(64);
            foreach (var _Data in pWeaponList)
            {
                WeaponMap.Add(_Data.EquipID, _Data);
            }

            ArmorMap = new Dictionary<int, GameEquipData>(64);
            foreach (var _Data in pArmorList)
            {
                ArmorMap.Add(_Data.EquipID, _Data);
            }

            UpdateAllHaveATKEffect();
            UpdateAllHaveHPEffect();
        }

        #region 消息处理函数

        public void OnEquipOn(S2C_EquipOn pMsg)
        {
            var itemType = (ItemType)pMsg.Type;
            if (itemType == ItemType.Weapon)
            {
                CurWeaponOnID = pMsg.EquipID;
                EventManager.Call(LogicEvent.EquipAllATKEffectUpdate);
            }
            else if (itemType == ItemType.Armor)
            {
                CurArmorOnID = pMsg.EquipID;
                EventManager.Call(LogicEvent.EquipAllHPEffectUpdate);
            }

            EventManager.Call(LogicEvent.EquipOn, pMsg);
        }

        public void OnEquipOff(S2C_EquipOff pMsg)
        {
            var itemType = (ItemType)pMsg.Type;
            if (itemType == ItemType.Weapon)
                CurWeaponOnID = 0;
            else if (itemType == ItemType.Armor)
                CurArmorOnID = 0;
            EventManager.Call(LogicEvent.EquipOff, pMsg);
        }

        public async void OnWeaponIntensify(List<GameEquipUpgradeData> pEquipList, bool pAuto)
        {
            var _TaskNeedCount = 0;
            foreach (var equipUpgradeData in pEquipList)
            {
                var _Data = GetEquipData(equipUpgradeData.EquipData.EquipID, ItemType.Weapon);
                if (_Data != null)
                {
                    _Data.Level = equipUpgradeData.EquipData.Level;
                    _Data.Count = equipUpgradeData.EquipData.Count;
                }

                _TaskNeedCount += equipUpgradeData.EquipData.Level - equipUpgradeData.OldLevel;
            }

            TaskManager.Ins.DoTaskUpdate(TaskType.TT_9001, _TaskNeedCount);

            if (pAuto)
                await UIManager.Ins.OpenUI<UIUpgradedInfo>(pEquipList);
            else
                EventManager.Call(LogicEvent.EquipWeaponUpgraded);

            UpdateAllHaveATKEffect();
            EventManager.Call(LogicEvent.EquipListChanged, ItemType.Weapon);
        }

        public async void OnArmorIntensify(List<GameEquipUpgradeData> pEquipList, bool pAuto)
        {
            var _TaskNeedCount = 0;
            foreach (var equipUpgradeData in pEquipList)
            {
                var _Data = GetEquipData(equipUpgradeData.EquipData.EquipID, ItemType.Armor);
                if (_Data != null)
                {
                    _Data.Level = equipUpgradeData.EquipData.Level;
                    _Data.Count = equipUpgradeData.EquipData.Count;
                }

                _TaskNeedCount += equipUpgradeData.EquipData.Level - equipUpgradeData.OldLevel;
            }

            TaskManager.Ins.DoTaskUpdate(TaskType.TT_9002, _TaskNeedCount);

            if (pAuto)
                await UIManager.Ins.OpenUI<UIUpgradedInfo>(pEquipList);
            else
                EventManager.Call(LogicEvent.EquipArmorUpgraded);

            UpdateAllHaveHPEffect();
            EventManager.Call(LogicEvent.EquipListChanged, ItemType.Armor);
        }

        public void OnEquipListUpdate(S2C_EquipListUpdate pMsg)
        {
            foreach (var equipData in pMsg.WeaponList)
            {
                var _Data = GetEquipData(equipData.EquipID, ItemType.Weapon);
                if (_Data != null)
                {
                    _Data.Count = equipData.Count;
                    _Data.Level = equipData.Level;
                }
                else
                {
                    WeaponMap.Add(equipData.EquipID, equipData);
                }
            }

            foreach (var equipData in pMsg.ArmorList)
            {
                var _Data = GetEquipData(equipData.EquipID, ItemType.Armor);
                if (_Data != null)
                {
                    _Data.Count = equipData.Count;
                    _Data.Level = equipData.Level;
                }
                else
                {
                    ArmorMap.Add(equipData.EquipID, equipData);
                }
            }

            UpdateAllHaveATKEffect();
            UpdateAllHaveHPEffect();
            EventManager.Call(LogicEvent.EquipListChanged, ItemType.Weapon);
            EventManager.Call(LogicEvent.EquipListChanged, ItemType.Armor);
        }

        // 装备合成
        public void OnEquipCompose(S2S_EquipCompose pMsg)
        {
            Debug.LogError($"/Type------{pMsg.Type}------");
            Debug.LogError($"/FromID------{pMsg.FromID}------");
            Debug.LogError($"/FromCount------{pMsg.FromCount}------");
            Debug.LogError($"/ToID------{pMsg.ToID}------");
            Debug.LogError($"/ToCount------{pMsg.ToCount}------");
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
                return WeaponMap.TryGetValue(pEquipID, out var _Data) ? _Data.Count : 0;
            if (pType == ItemType.Armor)
                return ArmorMap.TryGetValue(pEquipID, out var _Data) ? _Data.Count : 0;
            return 0;
        }

        //升级需要的数量
        public int NeedCount(int pEquipID, ItemType pType)
        {
            if (!IsHave(pEquipID, pType))
                return EquipLvlUpCfg.GetData(1).Cost;
            var _Data = GetEquipData(pEquipID, pType);
            return _Data.Level > 10 ? EquipLvlUpCfg.GetData(10).Cost : EquipLvlUpCfg.GetData(_Data.Level).Cost;
        }

        //装备合成需要的数量
        public int ComposeNeedCount(int pEquipID, ItemType pType)
        {
            var data = EquipCfg.GetData(pEquipID);
            return data.EquipCombineNum;
        }

        //装备是否可以升级
        public bool CanUpgrade(int pEquipID, ItemType pType)
        {
            if (!IsHave(pEquipID, pType))
                return false;
            var _Data = GetEquipData(pEquipID, pType);
            var _NeedCount =
                _Data.Level > 10 ? EquipLvlUpCfg.GetData(10).Cost : EquipLvlUpCfg.GetData(_Data.Level).Cost;
            return _Data.Count >= _NeedCount;
        }

        //是否有可升级的装备
        public bool HaveCanUpgradeEquip(ItemType pType)
        {
            if (pType == ItemType.Weapon)
            {
                foreach (var _MapData in WeaponMap)
                {
                    var _NeedCount = _MapData.Value.Level > 10
                        ? EquipLvlUpCfg.GetData(10).Cost
                        : EquipLvlUpCfg.GetData(_MapData.Value.Level).Cost;
                    if (_MapData.Value.Count >= _NeedCount)
                        return true;
                }
            }

            if (pType == ItemType.Armor)
            {
                foreach (var _MapData in ArmorMap)
                {
                    var _NeedCount = _MapData.Value.Level > 10
                        ? EquipLvlUpCfg.GetData(10).Cost
                        : EquipLvlUpCfg.GetData(_MapData.Value.Level).Cost;
                    if (_MapData.Value.Count >= _NeedCount)
                        return true;
                }
            }

            return false;
        }

        //装备是否满级
        public bool IsMaxLevel(int pEquipID, ItemType pType)
        {
            var level = 1;
            if (IsHave(pEquipID, pType))
            {
                level = GetEquipData(pEquipID, pType).Level;
            }

            return level >= GameDefine.CommonItemMaxLevel;
        }

        //获取某个装备的拥有效果
        public float GetHaveEffect(int pEquipID, ItemType pType)
        {
            var _Level = 1;
            if (IsHave(pEquipID, pType))
                _Level = GetEquipData(pEquipID, pType).Level;
            var _CfgData = EquipCfg.GetData(pEquipID);
            return _CfgData.HasAdditionBase + (_Level - 1) * _CfgData.HasAdditionGrow;
        }

        //获取某个装备的装备效果
        public float GetEquipEffect(int pEquipID, ItemType pType)
        {
            var _Level = 1;
            if (IsHave(pEquipID, pType))
                _Level = GetEquipData(pEquipID, pType).Level;
            var _CfgData = EquipCfg.GetData(pEquipID);
            return _CfgData.ValueBase + (_Level - 1) * _CfgData.ValueGrow;
        }

        //获取所有已有的加成值
        public void UpdateAllHaveATKEffect()
        {
            float bestAtk = 0;
            float _AllEffect = 0;
            foreach (var _MapData in WeaponMap)
            {
                var _CfgData = EquipCfg.GetData(_MapData.Key);
                var _GameData = _MapData.Value;
                _AllEffect += (_CfgData.HasAdditionBase + (_GameData.Level - 1) * _CfgData.HasAdditionGrow);
                var atk = GetEquipEffect(_MapData.Key, ItemType.Weapon);
                if (atk > bestAtk)
                {
                    bestAtk = atk;
                    BestWeaponId = _MapData.Key;
                }
            }

            AllHaveATKEffect = _AllEffect;
            EventManager.Call(LogicEvent.EquipAllATKEffectUpdate);
        }

        public void UpdateAllHaveHPEffect()
        {
            float bestHp = 0;
            float _AllEffect = 0;
            foreach (var _MapData in ArmorMap)
            {
                var _CfgData = EquipCfg.GetData(_MapData.Key);
                var _GameData = _MapData.Value;
                _AllEffect += (_CfgData.HasAdditionBase + (_GameData.Level - 1) * _CfgData.HasAdditionGrow);

                var hp = GetEquipEffect(_MapData.Key, ItemType.Armor);
                if (hp > bestHp)
                {
                    bestHp = hp;
                    BestArmorId = _MapData.Key;
                }
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
            if (CurWeaponOnID == 0)
                return AllHaveATKEffect;
            return GetEquipEffect(CurWeaponOnID, ItemType.Weapon) + AllHaveATKEffect;
        }

        //血量 总加成 (当前装备 + 所有获取的效果)
        public BigDouble GetArmorAdd()
        {
            if (CurArmorOnID == 0)
                return AllHaveHPEffect;
            return GetEquipEffect(CurArmorOnID, ItemType.Armor) + AllHaveHPEffect;
        }

        #endregion

        #region 操作接口

        public void DoOn(int pEquipID, int pType)
        {
            NetworkManager.Ins.SendMsg(new C2S_EquipOn()
            {
                EquipID = pEquipID, Type = pType
            });
        }


        public void DoIntensify(int pEquipID, int pType, bool pIsAuto)
        {
            NetworkManager.Ins.SendMsg(new C2S_EquipIntensify()
            {
                EquipID = pEquipID, Type = pType, IsAuto = pIsAuto,
            });
        }

        public void DoCompose(int pEquipID, int pType)
        {
            NetworkManager.Ins.SendMsg(new C2S_EquipCompose()
            {
                EquipID = pEquipID, Type = pType
            });
        }

        #endregion
    }
}