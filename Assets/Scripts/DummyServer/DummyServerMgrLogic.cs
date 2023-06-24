using System.Collections.Generic;
using System.Linq;
using Configs;
using Framework.Helper;
using Logic.Common;
using Networks;
using UnityEngine;

namespace DummyServer
{
    /// <summary>
    /// 部分消息处理逻辑写在这里
    /// </summary>
    public partial class DummyServerMgr
    {
        #region 技能相关逻辑

        public void OnDrawSkill(int type)
        {
            var count = GetDrawCardCount((DrawCardCostType)type);
            List<int> _SkillList = new List<int>(10);
            for (int i = 0; i < count; i++)
            {
                var _id = GetRandomItemID(GetGroupIDFromSummon(m_DB.m_ShopSkillData.ID));
                _SkillList.Add(_id);
            }

            // 更新技能商店数据
            UpdateGameShopSkillData(m_DB.m_ShopSkillData.ID, count);

            SendMsg(new S2C_DrawCard()
            {
                DrawCardType = (int)DrawCardType.Skill,
                List = _SkillList,
            });

            //更新技能数据
            foreach (var id in _SkillList)
            {
                var gameSkillData = m_DB.m_SkillList.Find(pData =>
                {
                    if (pData.SkillID == id)
                    {
                        pData.Count++;
                        return true;
                    }

                    return false;
                });

                if (gameSkillData == null)
                {
                    m_DB.m_SkillList.Add(new GameSkillData { SkillID = id, Count = 0, Level = 1 });
                }
            }

            SendMsg(new S2C_SkillListUpdate()
            {
                SkillList = m_DB.m_SkillList
            });

            DummyDB.Save(m_DB);
        }

        //单次升级
        public void SkillIntensify(int pID)
        {
            var _SkillData = m_DB.m_SkillList.Find(pData => pData.SkillID == pID);
            if (_SkillData == null)
            {
                return;
            }

            var _OldLevel = _SkillData.Level;
            DoSkillIntensify(_SkillData, false);
            if (_OldLevel == _SkillData.Level)
                return;

            SendMsg(new S2C_SkillIntensify
            {
                IsAuto = false,
                SkillList = new List<GameSkillUpgradeData>()
                    { new() { SkillData = _SkillData, OldLevel = _OldLevel } }
            });

            DummyDB.Save(m_DB);
        }

        //批量升满
        public void SkillIntensifyAuto()
        {
            List<GameSkillUpgradeData> _SkillList = new List<GameSkillUpgradeData>();
            for (int i = 1; i <= GameDefine.SkillMaxID; i++)
            {
                var _SkillData = m_DB.m_SkillList.Find(pData => pData.SkillID == i);
                if (_SkillData == null)
                    continue;

                var _OldLevel = _SkillData.Level;
                DoSkillIntensify(_SkillData);
                if (_OldLevel == _SkillData.Level)
                    continue;

                _SkillList.Add(new() { SkillData = _SkillData, OldLevel = _OldLevel });
            }

            if (_SkillList.Count > 0)
                SendMsg(new S2C_SkillIntensify { IsAuto = true, SkillList = _SkillList });

            DummyDB.Save(m_DB);
        }

        private void DoSkillIntensify(GameSkillData pData, bool pAuto = true)
        {
            var _CurCount = pData.Count;
            var _NeedCount = pData.Level > 10
                ? SkillLvlUpCfg.GetData(10).Cost
                : SkillLvlUpCfg.GetData(pData.Level).Cost;
            if (_CurCount >= _NeedCount)
            {
                pData.Level++;
                pData.Count -= _NeedCount;
                if (pAuto)
                    DoSkillIntensify(pData);
            }
        }

        private void DoSkillOn(int pID)
        {
            for (int i = 0; i < 6; i++)
            {
                if (m_DB.m_SkillOnList[i] == 0)
                {
                    m_DB.m_SkillOnList[i] = pID;
                    SendMsg(new S2C_SkillOn { SkillID = pID, Index = i });
                    DummyDB.Save(m_DB);
                    return;
                }
            }
        }

        private void DoSkillOff(int pID)
        {
            for (int i = 0; i < 6; i++)
            {
                if (m_DB.m_SkillOnList[i] == pID)
                {
                    m_DB.m_SkillOnList[i] = 0;
                    SendMsg(new S2C_SkillOff { SkillID = pID, Index = i });
                    DummyDB.Save(m_DB);
                    return;
                }
            }
        }

        #endregion

        #region 伙伴

        public void OnDrawPartner(int type)
        {
            var count = GetDrawCardCount((DrawCardCostType)type);
            List<int> _PartnerList = new List<int>(10);
            for (int i = 0; i < count; i++)
            {
                var _id = GetRandomItemID(GetGroupIDFromSummon(m_DB.m_ShopPartnerData.ID));
                _PartnerList.Add(_id);
            }

            // 更新伙伴商店数据
            UpdateGameShopPartnerData(m_DB.m_ShopPartnerData.ID, count);

            SendMsg(new S2C_DrawCard()
            {
                DrawCardType = (int)DrawCardType.Partner,
                List = _PartnerList
            });

            //更新技能数据
            foreach (var id in _PartnerList)
            {
                var gamePartnerData = m_DB.m_PartnerList.Find(pData =>
                {
                    if (pData.PartnerID == id)
                    {
                        pData.Count++;
                        return true;
                    }

                    return false;
                });

                if (gamePartnerData == null)
                {
                    m_DB.m_PartnerList.Add(new GamePartnerData { PartnerID = id, Count = 0, Level = 1 });
                }
            }

            SendMsg(new S2C_PartnerListUpdate()
            {
                PartnerList = m_DB.m_PartnerList
            });

            DummyDB.Save(m_DB);
        }

        //单次升级
        public void PartnerIntensify(int pID)
        {
            var _PartnerData = m_DB.m_PartnerList.Find(pData => pData.PartnerID == pID);
            if (_PartnerData == null)
            {
                return;
            }

            var _OldLevel = _PartnerData.Level;
            DoPartnerIntensify(_PartnerData, false);
            if (_OldLevel == _PartnerData.Level)
                return;

            SendMsg(new S2C_PartnerIntensify
            {
                IsAuto = false,
                PartnerList = new List<GamePartnerUpgradeData>()
                    { new() { PartnerData = _PartnerData, OldLevel = _OldLevel } }
            });

            DummyDB.Save(m_DB);
        }

        //批量升满
        public void PartnerIntensifyAuto()
        {
            List<GamePartnerUpgradeData> _PartnerList = new List<GamePartnerUpgradeData>();
            for (int i = 1; i <= GameDefine.PartnerMaxID; i++)
            {
                var _PartnerData = m_DB.m_PartnerList.Find(pData => pData.PartnerID == i);
                if (_PartnerData == null)
                    continue;

                var _OldLevel = _PartnerData.Level;
                DoPartnerIntensify(_PartnerData);
                if (_OldLevel == _PartnerData.Level)
                    continue;

                _PartnerList.Add(new() { PartnerData = _PartnerData, OldLevel = _OldLevel });
            }

            if (_PartnerList.Count > 0)
                SendMsg(new S2C_PartnerIntensify { IsAuto = true, PartnerList = _PartnerList });

            DummyDB.Save(m_DB);
        }

        private void DoPartnerIntensify(GamePartnerData pData, bool pAuto = true)
        {
            var _CurCount = pData.Count;
            var _NeedCount = pData.Level > 10
                ? PartnerLvlUpCfg.GetData(10).Cost
                : PartnerLvlUpCfg.GetData(pData.Level).Cost;
            if (_CurCount >= _NeedCount)
            {
                pData.Level++;
                pData.Count -= _NeedCount;
                if (pAuto)
                    DoPartnerIntensify(pData);
            }
        }

        private void DoPartnerOn(int pID)
        {
            for (int i = 0; i < 5; i++)
            {
                if (m_DB.m_PartnerOnList[i] == 0)
                {
                    m_DB.m_PartnerOnList[i] = pID;
                    SendMsg(new S2C_PartnerOn { PartnerID = pID, Index = i });
                    DummyDB.Save(m_DB);
                    return;
                }
            }
        }

        private void DoPartnerOff(int pID)
        {
            for (int i = 0; i < 5; i++)
            {
                if (m_DB.m_PartnerOnList[i] == pID)
                {
                    m_DB.m_PartnerOnList[i] = 0;
                    SendMsg(new S2C_PartnerOff { PartnerID = pID, Index = i });
                    DummyDB.Save(m_DB);
                    return;
                }
            }
        }

        #endregion

        #region 装备相关

        public ItemType GetEquipType(int pEquipID)
        {
            return (ItemType)ItemCfg.GetData(pEquipID).Type;
        }

        public void OnDrawEquip(int type)
        {
            var count = GetDrawCardCount((DrawCardCostType)type);
            List<int> _Weapon = new List<int>(10);
            for (int i = 0; i < count; i++)
            {
                if (RandomHelper.NextBool())
                {
                    var _id = GetRandomItemID(GetGroupIDFromSummon(m_DB.m_ShopEquipData.ID));
                    _Weapon.Add(_id);
                }
                else
                {
                    // TODO: 防具ID与武器ID相差1000，目前这里待进一步优化处理
                    var _id = GetRandomItemID(GetGroupIDFromSummon(m_DB.m_ShopEquipData.ID + 1000));
                    _Weapon.Add(_id);
                }
            }

            // 更新装备商店数据
            UpdateGameShopEquipData(m_DB.m_ShopEquipData.ID, count);

            SendMsg(new S2C_DrawCard()
            {
                DrawCardType = (int)DrawCardType.Equip,
                List = _Weapon
            });

            //更新技能数据
            foreach (var id in _Weapon)
            {
                List<GameEquipData> _Temp = null;
                if (GetEquipType(id) == ItemType.Weapon)
                    _Temp = m_DB.m_WeaponList;
                else if (GetEquipType(id) == ItemType.Armor)
                    _Temp = m_DB.m_ArmorList;

                var _Data = _Temp.Find(pData =>
                {
                    if (pData.EquipID == id)
                    {
                        pData.Count++;
                        return true;
                    }

                    return false;
                });

                if (_Data == null)
                {
                    _Temp.Add(new GameEquipData { EquipID = id, Count = 0, Level = 1 });
                }
            }

            SendMsg(new S2C_EquipListUpdate()
            {
                WeaponList = m_DB.m_WeaponList,
                ArmorList = m_DB.m_ArmorList
            });

            DummyDB.Save(m_DB);
        }

        //单次升级
        public void EquipIntensify(int pID, int pType)
        {
            var equipType = (ItemType)pType;
            List<GameEquipData> _Temp = null;
            if (equipType == ItemType.Weapon)
                _Temp = m_DB.m_WeaponList;
            else if (equipType == ItemType.Armor)
                _Temp = m_DB.m_ArmorList;

            var _Equip = _Temp.Find(pData => pData.EquipID == pID);
            if (_Equip == null)
            {
                return;
            }

            var _OldLevel = _Equip.Level;
            DoEquipIntensify(_Equip, false);
            if (_OldLevel == _Equip.Level)
                return;

            SendMsg(new S2C_EquipIntensify
            {
                IsAuto = false, Type = pType,
                EquipList = new List<GameEquipUpgradeData>()
                    { new() { EquipData = _Equip, OldLevel = _OldLevel } }
            });

            DummyDB.Save(m_DB);
        }

        //批量升满
        public void EquipIntensifyAuto(int pType)
        {
            var equipType = (ItemType)pType;
            List<GameEquipUpgradeData> _EquipList = new List<GameEquipUpgradeData>();
            int iStartID = 0;
            int iMaxID = 0;
            List<GameEquipData> _Temp = null;
            if (equipType == ItemType.Weapon)
            {
                iStartID = 1001;
                iMaxID = GameDefine.WeaponMaxID;
                _Temp = m_DB.m_WeaponList;
            }
            else if (equipType == ItemType.Armor)
            {
                iStartID = 2001;
                iMaxID = GameDefine.ArmorMaxID;
                _Temp = m_DB.m_ArmorList;
            }

            for (int i = iStartID; i <= iMaxID; i++)
            {
                var _EquipData = _Temp.Find(pData => pData.EquipID == i);
                if (_EquipData == null)
                    continue;

                var _OldLevel = _EquipData.Level;
                DoEquipIntensify(_EquipData);
                if (_OldLevel == _EquipData.Level)
                    continue;

                _EquipList.Add(new() { EquipData = _EquipData, OldLevel = _OldLevel });
            }

            if (_EquipList.Count > 0)
                SendMsg(new S2C_EquipIntensify { IsAuto = true, Type = pType, EquipList = _EquipList });

            DummyDB.Save(m_DB);
        }

        private void DoEquipIntensify(GameEquipData pData, bool pAuto = true)
        {
            var _CurCount = pData.Count;
            var _NeedCount = pData.Level > 10
                ? EquipLvlUpCfg.GetData(10).Cost
                : EquipLvlUpCfg.GetData(pData.Level).Cost;
            if (_CurCount >= _NeedCount)
            {
                pData.Level++;
                pData.Count -= _NeedCount;
                if (pAuto)
                    DoEquipIntensify(pData);
            }
        }

        private void DoEquipOn(int pID, int pType)
        {
            var equipType = (ItemType)pType;
            if (equipType == ItemType.Weapon)
            {
                if (m_DB.m_WeaponOnID != 0)
                {
                    SendMsg(new S2C_EquipOff { EquipID = m_DB.m_WeaponOnID, Type = pType });
                }

                m_DB.m_WeaponOnID = pID;
                SendMsg(new S2C_EquipOn { EquipID = pID, Type = pType });
            }
            else if (equipType == ItemType.Armor)
            {
                if (m_DB.m_ArmorOnID != 0)
                {
                    SendMsg(new S2C_EquipOff { EquipID = m_DB.m_ArmorOnID, Type = pType });
                }

                m_DB.m_ArmorOnID = pID;
                SendMsg(new S2C_EquipOn { EquipID = pID, Type = pType });
            }

            DummyDB.Save(m_DB);
        }

        #endregion
    }
}