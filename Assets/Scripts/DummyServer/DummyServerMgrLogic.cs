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

        #region 副本

        #endregion

        #region 引擎相关逻辑

        /// <summary>
        /// DigEngine表层数
        /// </summary>
        private int[] m_DigDeep =
        {
            10, 20, 30, 40, 50, 60, 70, 80, 90, 100,
            110, 120, 130, 140, 150, 160, 170, 180, 190, 200,
            210, 220, 230, 240, 250, 260, 270, 280, 290, 300,
            310, 320, 330, 340, 350, 360, 370, 380, 390, 400,
            410, 420, 430, 440, 450, 460, 470, 480, 490, 500,
            0,
        };

        /// <summary>
        /// DigEngine表Id
        /// </summary>
        private int[] m_DigId =
        {
            10, 20, 30, 40, 50, 60, 70, 80, 90, 100,
            110, 120, 130, 140, 150, 160, 170, 180, 190, 200,
            210, 220, 230, 240, 250, 260, 270, 280, 290, 300,
            310, 320, 330, 340, 350, 360, 370, 380, 390, 400,
            410, 420, 430, 440, 450, 460, 470, 480, 490, 500,
            0,
        };

        /// <summary>
        /// 从DigEngine表中获取随机引擎
        /// </summary>
        /// <returns></returns>
        private EngineData GetRandomEngineData()
        {
            var index = 0;
            while (index < m_DigDeep.Length && m_DB.m_MiningData.FloorCount > m_DigDeep[index])
            {
                index++;
            }

            var digEngineData = DigEngineCfg.GetData(m_DigId[index]);
            var engineGroupId = digEngineData.Name;
            var engineTypeId = engineGroupId[GetQualityValue(digEngineData.Wight.ToArray())];
            var engineData = EngineCfg.GetData(engineTypeId);
            return engineData;
        }

        /// <summary>
        /// 将要获取的引擎
        /// </summary>
        private void UpdateToGetEngine()
        {
            // 获取随机类型的引擎数据和引擎附加属性
            var engineData = GetRandomEngineData();
            var engineAttrId = engineData.EngineAttrGroup[GetQualityValue(engineData.Wight.ToArray())];

            // 获取将要获取引擎的Id
            var engineCountRange = GameDefine.MaxEngineCount + 1;
            var engineGetId = m_DB.m_EngineGetId % engineCountRange + 1;
            while (IsEngineListHave(engineGetId))
            {
                engineGetId = engineGetId % engineCountRange + 1;
            }

            m_DB.m_EngineGetId = engineGetId;

            // 添加将要获取的引擎到引擎列表中
            m_DB.m_EngineList.Add(new GameEngineData
            {
                Id = m_DB.m_EngineGetId, TypeId = engineData.ID, IsGet = 0, AttrId = engineAttrId,
                Level = 0, Reform = 0,
            });

            DummyDB.Save(m_DB);
        }

        /// <summary>
        /// 获取引擎
        /// </summary>
        private void UpdateGetEngine()
        {
            m_DB.m_EngineList.Find(i => i.Id == m_DB.m_EngineGetId).IsGet = 1;
            //临时存储的上一个将要获取的引擎Id，通知客户端该Id引擎被获得
            var lastEngineGetId = m_DB.m_EngineGetId;

            //更新获取引擎后再更新将要获取的引擎列表
            UpdateToGetEngine();

            SendMsg(new S2C_EngineGet()
            {
                EngineList = m_DB.m_EngineList,
                LastEngineGetId = lastEngineGetId,
            });

            DummyDB.Save(m_DB);
        }

        /// <summary>
        /// 判断当前引擎数据中是否存在将要获取引擎的Id
        /// </summary>
        /// <param name="engineGetId"></param>
        /// <returns></returns>
        private bool IsEngineListHave(int engineGetId)
        {
            var gameEngineData = m_DB.m_EngineList.Find(i => i.Id == engineGetId);
            return gameEngineData != null;
        }

        /// <summary>
        /// 将要获取的引擎所需要的材料数
        /// </summary>
        /// <returns></returns>
        private int GetEngineCostGear()
        {
            var typeId = m_DB.m_EngineList.Find(i => i.Id == m_DB.m_EngineGetId).TypeId;
            var costGear = EngineCfg.GetData(typeId).CostGear;
            return costGear;
        }

        //引擎强化消耗材料
        private int GetEngineIntensifyCost(int engineLevel, int engineReform = 0)
        {
            var engineLvlUpData = EngineLvlUpCfg.GetData(engineLevel) ??
                                  EngineLvlUpCfg.GetData(GameDefine.EngineFormulaId);
            return engineLvlUpData.LvlUpBaseCost
                   * (int)Mathf.Pow(engineLvlUpData.ReformAdditionCost, engineReform)
                   + engineLevel * engineLvlUpData.LvlUpAdditionCost;
        }

        //引擎分解获取材料
        private int GetEngineDecomposeGet(int engineLevel, int engineReform = 0)
        {
            var engineLvlUpData = EngineLvlUpCfg.GetData(engineLevel) ??
                                  EngineLvlUpCfg.GetData(GameDefine.EngineFormulaId);
            return engineLvlUpData.DecomposeBase
                   + engineReform * engineLvlUpData.DecomposeReformAddition
                   + engineLevel * engineLvlUpData.DecomposeLvlAddition;
        }

        /// <summary>
        /// 分解引擎
        /// </summary>
        private void DoEngineRemove(int pID)
        {
            foreach (var engine in m_DB.m_EngineList.Where(engine => engine.Id == pID))
            {
                UpdateIron(GetEngineDecomposeGet(engine.Level, engine.Reform)); //更新引擎分解材料
                m_DB.m_EngineList.Remove(engine); //分解引擎
                break;
            }

            DummyDB.Save(m_DB);
        }

        /// <summary>
        /// 引擎装备上
        /// </summary>
        /// <param name="pID"></param>
        private void DoEngineOn(int pID)
        {
            if (m_DB.m_EngineOnId != 0)
            {
                SendMsg(new S2C_EngineOff() { EngineId = m_DB.m_EngineOnId });
            }

            m_DB.m_EngineOnId = pID;
            SendMsg(new S2C_EngineOn() { EngineId = pID });
            DummyDB.Save(m_DB);
        }

        /// <summary>
        /// 引擎装备下
        /// </summary>
        /// <param name="pID"></param>
        private void DoEngineOff(int pID)
        {
            m_DB.m_EngineOnId = 0;
            SendMsg(new S2C_EngineOff() { EngineId = pID });
            DummyDB.Save(m_DB);
        }

        //引擎升级
        private void DoEngineIntensify(int pID)
        {
            var engineData = m_DB.m_EngineList.Find(pData => pData.Id == pID);
            if (engineData == null) return;
            UpdateIron(-GetEngineIntensifyCost(engineData.Level, engineData.Reform)); //更新引擎强化材料
            engineData.Level++; //强化引擎等级
            SendMsg(new S2C_EngineIntensify()
            {
                EngineId = pID,
                EngineLevel = engineData.Level,
            });
            DummyDB.Save(m_DB);
        }

        #endregion
    }
}