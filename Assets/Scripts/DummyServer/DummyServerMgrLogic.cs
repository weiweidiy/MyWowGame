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
                var _id = GetRandomItemID(GetGroupIDFromSummon(m_DB.m_ShopSkillData.m_ID));
                _SkillList.Add(_id);
            }

            // 更新技能商店数据
            UpdateGameShopSkillData(m_DB.m_ShopSkillData.m_ID, count);

            SendMsg(new S2C_DrawCard()
            {
                m_DrawCardType = (int)DrawCardType.Skill,
                m_List = _SkillList,
            });

            //更新技能数据
            foreach (var id in _SkillList)
            {
                var gameSkillData = m_DB.m_SkillList.Find(pData =>
                {
                    if (pData.m_SkillID == id)
                    {
                        pData.m_Count++;
                        return true;
                    }

                    return false;
                });

                if (gameSkillData == null)
                {
                    m_DB.m_SkillList.Add(new GameSkillData { m_SkillID = id, m_Count = 0, m_Level = 1 });
                }
            }

            SendMsg(new S2C_SkillListUpdate()
            {
                m_SkillList = m_DB.m_SkillList
            });

            DummyDB.Save(m_DB);
        }

        //单次升级
        public void SkillIntensify(int pID)
        {
            var _SkillData = m_DB.m_SkillList.Find(pData => pData.m_SkillID == pID);
            if (_SkillData == null)
            {
                return;
            }

            var _OldLevel = _SkillData.m_Level;
            DoSkillIntensify(_SkillData, false);
            if (_OldLevel == _SkillData.m_Level)
                return;

            SendMsg(new S2C_SkillIntensify
            {
                m_IsAuto = false,
                m_SkillList = new List<GameSkillUpgradeData>()
                    { new() { m_SkillData = _SkillData, m_OldLevel = _OldLevel } }
            });

            DummyDB.Save(m_DB);
        }

        //批量升满
        public void SkillIntensifyAuto()
        {
            List<GameSkillUpgradeData> _SkillList = new List<GameSkillUpgradeData>();
            for (int i = 1; i <= GameDefine.SkillMaxID; i++)
            {
                var _SkillData = m_DB.m_SkillList.Find(pData => pData.m_SkillID == i);
                if (_SkillData == null)
                    continue;

                var _OldLevel = _SkillData.m_Level;
                DoSkillIntensify(_SkillData);
                if (_OldLevel == _SkillData.m_Level)
                    continue;

                _SkillList.Add(new() { m_SkillData = _SkillData, m_OldLevel = _OldLevel });
            }

            if (_SkillList.Count > 0)
                SendMsg(new S2C_SkillIntensify { m_IsAuto = true, m_SkillList = _SkillList });

            DummyDB.Save(m_DB);
        }

        private void DoSkillIntensify(GameSkillData pData, bool pAuto = true)
        {
            var _CurCount = pData.m_Count;
            var _NeedCount = pData.m_Level > 10
                ? SkillLvlUpCfg.GetData(10).Cost
                : SkillLvlUpCfg.GetData(pData.m_Level).Cost;
            if (_CurCount >= _NeedCount)
            {
                pData.m_Level++;
                pData.m_Count -= _NeedCount;
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
                    SendMsg(new S2C_SkillOn { m_SkillID = pID, m_Index = i });
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
                    SendMsg(new S2C_SkillOff { m_SkillID = pID, m_Index = i });
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
                var _id = GetRandomItemID(GetGroupIDFromSummon(m_DB.m_ShopPartnerData.m_ID));
                _PartnerList.Add(_id);
            }

            // 更新伙伴商店数据
            UpdateGameShopPartnerData(m_DB.m_ShopPartnerData.m_ID, count);

            SendMsg(new S2C_DrawCard()
            {
                m_DrawCardType = (int)DrawCardType.Partner,
                m_List = _PartnerList
            });

            //更新技能数据
            foreach (var id in _PartnerList)
            {
                var gamePartnerData = m_DB.m_PartnerList.Find(pData =>
                {
                    if (pData.m_PartnerID == id)
                    {
                        pData.m_Count++;
                        return true;
                    }

                    return false;
                });

                if (gamePartnerData == null)
                {
                    m_DB.m_PartnerList.Add(new GamePartnerData { m_PartnerID = id, m_Count = 0, m_Level = 1 });
                }
            }

            SendMsg(new S2C_PartnerListUpdate()
            {
                m_PartnerList = m_DB.m_PartnerList
            });

            DummyDB.Save(m_DB);
        }

        //单次升级
        public void PartnerIntensify(int pID)
        {
            var _PartnerData = m_DB.m_PartnerList.Find(pData => pData.m_PartnerID == pID);
            if (_PartnerData == null)
            {
                return;
            }

            var _OldLevel = _PartnerData.m_Level;
            DoPartnerIntensify(_PartnerData, false);
            if (_OldLevel == _PartnerData.m_Level)
                return;

            SendMsg(new S2C_PartnerIntensify
            {
                m_IsAuto = false,
                m_PartnerList = new List<GamePartnerUpgradeData>()
                    { new() { m_PartnerData = _PartnerData, m_OldLevel = _OldLevel } }
            });

            DummyDB.Save(m_DB);
        }

        //批量升满
        public void PartnerIntensifyAuto()
        {
            List<GamePartnerUpgradeData> _PartnerList = new List<GamePartnerUpgradeData>();
            for (int i = 1; i <= GameDefine.PartnerMaxID; i++)
            {
                var _PartnerData = m_DB.m_PartnerList.Find(pData => pData.m_PartnerID == i);
                if (_PartnerData == null)
                    continue;

                var _OldLevel = _PartnerData.m_Level;
                DoPartnerIntensify(_PartnerData);
                if (_OldLevel == _PartnerData.m_Level)
                    continue;

                _PartnerList.Add(new() { m_PartnerData = _PartnerData, m_OldLevel = _OldLevel });
            }

            if (_PartnerList.Count > 0)
                SendMsg(new S2C_PartnerIntensify { m_IsAuto = true, m_PartnerList = _PartnerList });

            DummyDB.Save(m_DB);
        }

        private void DoPartnerIntensify(GamePartnerData pData, bool pAuto = true)
        {
            var _CurCount = pData.m_Count;
            var _NeedCount = pData.m_Level > 10
                ? PartnerLvlUpCfg.GetData(10).Cost
                : PartnerLvlUpCfg.GetData(pData.m_Level).Cost;
            if (_CurCount >= _NeedCount)
            {
                pData.m_Level++;
                pData.m_Count -= _NeedCount;
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
                    SendMsg(new S2C_PartnerOn { m_PartnerID = pID, m_Index = i });
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
                    SendMsg(new S2C_PartnerOff { m_PartnerID = pID, m_Index = i });
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
                    var _id = GetRandomItemID(GetGroupIDFromSummon(m_DB.m_ShopEquipData.m_ID));
                    _Weapon.Add(_id);
                }
                else
                {
                    // TODO: 防具ID与武器ID相差1000，目前这里待进一步优化处理
                    var _id = GetRandomItemID(GetGroupIDFromSummon(m_DB.m_ShopEquipData.m_ID + 1000));
                    _Weapon.Add(_id);
                }
            }

            // 更新装备商店数据
            UpdateGameShopEquipData(m_DB.m_ShopEquipData.m_ID, count);

            SendMsg(new S2C_DrawCard()
            {
                m_DrawCardType = (int)DrawCardType.Equip,
                m_List = _Weapon
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
                    if (pData.m_EquipID == id)
                    {
                        pData.m_Count++;
                        return true;
                    }

                    return false;
                });

                if (_Data == null)
                {
                    _Temp.Add(new GameEquipData { m_EquipID = id, m_Count = 0, m_Level = 1 });
                }
            }

            SendMsg(new S2C_EquipListUpdate()
            {
                m_WeaponList = m_DB.m_WeaponList,
                m_ArmorList = m_DB.m_ArmorList
            });

            DummyDB.Save(m_DB);
        }

        //单次升级
        public void EquipIntensify(int pID, ItemType pType)
        {
            List<GameEquipData> _Temp = null;
            if (pType == ItemType.Weapon)
                _Temp = m_DB.m_WeaponList;
            else if (pType == ItemType.Armor)
                _Temp = m_DB.m_ArmorList;

            var _Equip = _Temp.Find(pData => pData.m_EquipID == pID);
            if (_Equip == null)
            {
                return;
            }

            var _OldLevel = _Equip.m_Level;
            DoEquipIntensify(_Equip, false);
            if (_OldLevel == _Equip.m_Level)
                return;

            SendMsg(new S2C_EquipIntensify
            {
                m_IsAuto = false, m_Type = pType,
                m_EquipList = new List<GameEquipUpgradeData>()
                    { new() { m_EquipData = _Equip, m_OldLevel = _OldLevel } }
            });

            DummyDB.Save(m_DB);
        }

        //批量升满
        public void EquipIntensifyAuto(ItemType pType)
        {
            List<GameEquipUpgradeData> _EquipList = new List<GameEquipUpgradeData>();
            int iStartID = 0;
            int iMaxID = 0;
            List<GameEquipData> _Temp = null;
            if (pType == ItemType.Weapon)
            {
                iStartID = 1001;
                iMaxID = GameDefine.WeaponMaxID;
                _Temp = m_DB.m_WeaponList;
            }
            else if (pType == ItemType.Armor)
            {
                iStartID = 2001;
                iMaxID = GameDefine.ArmorMaxID;
                _Temp = m_DB.m_ArmorList;
            }

            for (int i = iStartID; i <= iMaxID; i++)
            {
                var _EquipData = _Temp.Find(pData => pData.m_EquipID == i);
                if (_EquipData == null)
                    continue;

                var _OldLevel = _EquipData.m_Level;
                DoEquipIntensify(_EquipData);
                if (_OldLevel == _EquipData.m_Level)
                    continue;

                _EquipList.Add(new() { m_EquipData = _EquipData, m_OldLevel = _OldLevel });
            }

            if (_EquipList.Count > 0)
                SendMsg(new S2C_EquipIntensify { m_IsAuto = true, m_Type = pType, m_EquipList = _EquipList });

            DummyDB.Save(m_DB);
        }

        private void DoEquipIntensify(GameEquipData pData, bool pAuto = true)
        {
            var _CurCount = pData.m_Count;
            var _NeedCount = pData.m_Level > 10
                ? EquipLvlUpCfg.GetData(10).Cost
                : EquipLvlUpCfg.GetData(pData.m_Level).Cost;
            if (_CurCount >= _NeedCount)
            {
                pData.m_Level++;
                pData.m_Count -= _NeedCount;
                if (pAuto)
                    DoEquipIntensify(pData);
            }
        }

        private void DoEquipOn(int pID, ItemType pType)
        {
            if (pType == ItemType.Weapon)
            {
                if (m_DB.m_WeaponOnID != 0)
                {
                    SendMsg(new S2C_EquipOff { m_EquipID = m_DB.m_WeaponOnID, m_Type = pType });
                }

                m_DB.m_WeaponOnID = pID;
                SendMsg(new S2C_EquipOn { m_EquipID = pID, m_Type = pType });
            }
            else if (pType == ItemType.Armor)
            {
                if (m_DB.m_ArmorOnID != 0)
                {
                    SendMsg(new S2C_EquipOff { m_EquipID = m_DB.m_ArmorOnID, m_Type = pType });
                }

                m_DB.m_ArmorOnID = pID;
                SendMsg(new S2C_EquipOn { m_EquipID = pID, m_Type = pType });
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
            while (index < m_DigDeep.Length && m_DB.m_MiningData.m_FloorCount > m_DigDeep[index])
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
                m_Id = m_DB.m_EngineGetId, m_TypeId = engineData.ID, m_IsGet = 0, m_AttrId = engineAttrId,
                m_Level = 0, m_Reform = 0,
            });

            DummyDB.Save(m_DB);
        }

        /// <summary>
        /// 获取引擎
        /// </summary>
        private void UpdateGetEngine()
        {
            m_DB.m_EngineList.Find(i => i.m_Id == m_DB.m_EngineGetId).m_IsGet = 1;
            //临时存储的上一个将要获取的引擎Id，通知客户端该Id引擎被获得
            var lastEngineGetId = m_DB.m_EngineGetId;

            //更新获取引擎后再更新将要获取的引擎列表
            UpdateToGetEngine();

            SendMsg(new S2C_EngineGet()
            {
                m_EngineList = m_DB.m_EngineList,
                m_LastEngineGetId = lastEngineGetId,
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
            var gameEngineData = m_DB.m_EngineList.Find(i => i.m_Id == engineGetId);
            return gameEngineData != null;
        }

        /// <summary>
        /// 将要获取的引擎所需要的材料数
        /// </summary>
        /// <returns></returns>
        private int GetEngineCostGear()
        {
            var typeId = m_DB.m_EngineList.Find(i => i.m_Id == m_DB.m_EngineGetId).m_TypeId;
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
            foreach (var engine in m_DB.m_EngineList.Where(engine => engine.m_Id == pID))
            {
                UpdateIron(GetEngineDecomposeGet(engine.m_Level, engine.m_Reform)); //更新引擎分解材料
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
                SendMsg(new S2C_EngineOff() { m_EngineOffId = m_DB.m_EngineOnId });
            }

            m_DB.m_EngineOnId = pID;
            SendMsg(new S2C_EngineOn() { m_EngineOnId = pID });
            DummyDB.Save(m_DB);
        }

        /// <summary>
        /// 引擎装备下
        /// </summary>
        /// <param name="pID"></param>
        private void DoEngineOff(int pID)
        {
            m_DB.m_EngineOnId = 0;
            SendMsg(new S2C_EngineOff() { m_EngineOffId = pID });
            DummyDB.Save(m_DB);
        }

        //引擎升级
        private void DoEngineIntensify(int pID)
        {
            var engineData = m_DB.m_EngineList.Find(pData => pData.m_Id == pID);
            if (engineData == null) return;
            UpdateIron(-GetEngineIntensifyCost(engineData.m_Level, engineData.m_Reform)); //更新引擎强化材料
            engineData.m_Level++; //强化引擎等级
            SendMsg(new S2C_EngineIntensify()
            {
                m_EngineIntensifyId = pID,
                m_EngineLevel = engineData.m_Level,
            });
            DummyDB.Save(m_DB);
        }

        #endregion
    }
}