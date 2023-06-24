using System;
using Configs;
using Framework.EventKit;
using Framework.Extension;
using Logic.Common;
using Logic.Common.RedDot;
using Logic.Manager;
using Logic.UI.Common;
using Networks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.Cells
{
    /// <summary>
    /// 游戏通用Item组件
    /// </summary>
    public class CommonItem : MonoBehaviour
    {
        [LabelText("道具类型"), ReadOnly] public ItemType m_ItemType;
        public Image m_Quality;
        public Image m_Icon;
        public GameObject m_ProcessNode;
        public Image m_CantProcess;
        public Image m_CanProcess;
        public TextMeshProUGUI m_TextProcess;
        public TextMeshProUGUI m_Level;
        public GameObject m_Mask;
        public GameObject m_Lock;
        public GameObject m_IsOn;
        public GameObject m_CanUpgrade;
        public GameObject m_Selected;

        public EventGroup m_EventGroup = new();
        public Action<CommonItem> m_ClickCB;

        public BigBoomCellRedDotMono m_RedDot;
        [ReadOnly] int m_Id;


        private void OnDestroy()
        {
            m_EventGroup.Release();
            m_ClickCB = null;
        }

        #region Item

        public ItemData m_ItemData;

        #endregion

        #region 技能

        [NonSerialized] public SkillData m_SkillData;
        private GameSkillData m_GameSkillData;


        private void Awake()
        {
            Debug.Assert(m_RedDot != null, "RedDotMono 引用丢失，请修正");
        }

        public void InitBySkill(SkillData pData)
        {
            //Debug.LogError("InitBySkill");


            m_ItemType = ItemType.Skill;
            m_ItemData = ItemCfg.GetData(pData.ID);
            m_SkillData = pData;

            UICommonHelper.LoadIcon(m_Icon, m_ItemData.Res);
            UICommonHelper.LoadQuality(m_Quality, m_SkillData.Quality);

            UpdateSkillInfo();

            m_EventGroup.Register(LogicEvent.SkillListChanged, (i, o) => { UpdateSkillInfo(); });
            m_EventGroup.Register(LogicEvent.SkillOn, (i, o) =>
            {
                var _SkillId = ((S2C_SkillOn)o).SkillID;
                if (_SkillId == m_SkillData.ID)
                    m_IsOn.Show();
            });
            m_EventGroup.Register(LogicEvent.SkillOff, (i, o) =>
            {
                var _SkillId = ((S2C_SkillOff)o).SkillID;
                if (_SkillId == m_SkillData.ID)
                    m_IsOn.Hide();
            });
        }

        private void UpdateSkillInfo()
        {
            if (m_SkillData == null)
                return;
            if (SkillManager.Ins.IsHave(m_SkillData.ID))
            {
                m_Lock.Hide();
                m_Mask.Hide();
                m_GameSkillData = SkillManager.Ins.GetSkillData(m_SkillData.ID);
                m_Level.text = "LV" + m_GameSkillData.Level;
            }
            else
            {
                m_Lock.Show();
                m_Mask.Show();
            }

            if (SkillManager.Ins.IsOn(m_SkillData.ID))
            {
                m_IsOn.Show();
            }
            else
            {
                m_IsOn.Hide();
            }

            //升级节点
            m_ProcessNode.Show();
            if (SkillManager.Ins.IsMaxLevel(m_SkillData.ID))
            {
                //满级节点
                m_CanUpgrade.Hide();
                m_CantProcess.Hide();
                m_CanProcess.Show();
                m_CanProcess.fillAmount = 1;
                m_TextProcess.text = "Max";
                m_Level.text = "LV" + GameDefine.CommonItemMaxLevel;
            }
            else
            {
                var _CurCount = SkillManager.Ins.CurCount(m_SkillData.ID);
                var _NeedCount = SkillManager.Ins.UpgradeNeedCount(m_SkillData.ID);
                if (_CurCount >= _NeedCount)
                {
                    m_CanUpgrade.Show();
                    m_CantProcess.Hide();
                    m_CanProcess.Show();
                }
                else
                {
                    m_CanUpgrade.Hide();
                    m_CantProcess.Show();
                    m_CanProcess.Hide();
                }

                float _Process = (float)_CurCount / _NeedCount;
                m_CantProcess.fillAmount = _Process;
                m_CanProcess.fillAmount = _Process;

                m_TextProcess.text = _CurCount + "/" + _NeedCount;
            }
        }

        #endregion

        #region 伙伴

        [NonSerialized] public PartnerData m_PartnerData;
        private GamePartnerData m_GamePartnerData;

        public void InitByPartner(PartnerData pData)
        {
            m_ItemType = ItemType.Partner;
            m_ItemData = ItemCfg.GetData(pData.ID);
            m_PartnerData = pData;

            UICommonHelper.LoadIcon(m_Icon, m_ItemData.Res);
            UICommonHelper.LoadQuality(m_Quality, m_PartnerData.Quality);

            UpdatePartnerInfo();

            m_EventGroup.Register(LogicEvent.PartnerListChanged, (i, o) => { UpdatePartnerInfo(); });
            m_EventGroup.Register(LogicEvent.PartnerOn, (i, o) =>
            {
                var _PartnerId = ((S2C_PartnerOn)o).PartnerID;
                if (_PartnerId == m_PartnerData.ID)
                    m_IsOn.Show();
            });
            m_EventGroup.Register(LogicEvent.PartnerOff, (i, o) =>
            {
                var _PartnerId = ((S2C_PartnerOff)o).PartnerID;
                if (_PartnerId == m_PartnerData.ID)
                    m_IsOn.Hide();
            });
        }

        private void UpdatePartnerInfo()
        {
            if (m_PartnerData == null)
                return;
            if (PartnerManager.Ins.IsHave(m_PartnerData.ID))
            {
                m_Lock.Hide();
                m_Mask.Hide();
                m_GamePartnerData = PartnerManager.Ins.GetPartnerData(m_PartnerData.ID);
                m_Level.text = "LV" + m_GamePartnerData.Level;
            }
            else
            {
                m_Lock.Show();
                m_Mask.Show();
            }

            if (PartnerManager.Ins.IsOn(m_PartnerData.ID))
            {
                m_IsOn.Show();
            }
            else
            {
                m_IsOn.Hide();
            }

            //升级节点
            m_ProcessNode.Show();
            if (PartnerManager.Ins.IsMaxLevel(m_PartnerData.ID))
            {
                //满级节点
                m_CanUpgrade.Hide();
                m_CantProcess.Hide();
                m_CanProcess.Show();
                m_CanProcess.fillAmount = 1;
                m_TextProcess.text = "Max";
                m_Level.text = "LV" + GameDefine.CommonItemMaxLevel;
            }
            else
            {
                var _CurCount = PartnerManager.Ins.CurCount(m_PartnerData.ID);
                var _NeedCount = PartnerManager.Ins.UpgradeNeedCount(m_PartnerData.ID);
                if (_CurCount >= _NeedCount)
                {
                    m_CanUpgrade.Show();
                    m_CantProcess.Hide();
                    m_CanProcess.Show();
                }
                else
                {
                    m_CanUpgrade.Hide();
                    m_CantProcess.Show();
                    m_CanProcess.Hide();
                }

                float _Process = (float)_CurCount / _NeedCount;
                m_CantProcess.fillAmount = _Process;
                m_CanProcess.fillAmount = _Process;
                m_TextProcess.text = _CurCount + "/" + _NeedCount;
            }
        }

        #endregion

        #region 装备

        private ItemType m_EquipType;
        [NonSerialized] public EquipData m_EquipData;
        private GameEquipData m_GameEquipData;

        public void InitByEquip(EquipData pData)
        {
            //Debug.LogError("InitByEquip");
            m_EquipType = (ItemType)pData.EquipType;
            m_ItemData = ItemCfg.GetData(pData.ID);
            m_Id = pData.ID;
            switch (m_EquipType)
            {
                case ItemType.Weapon:
                    m_ItemType = ItemType.Weapon;

                    //红点设置
                    m_RedDot?.AddInteresting(RedDotKey.EquipWeaponEquipable);

                    break;
                case ItemType.Armor:
                    m_ItemType = ItemType.Armor;

                    m_RedDot?.AddInteresting(RedDotKey.EquipArmorEquipable);
                    break;
            }

            //Debug.LogError("InitByEquip");
            m_RedDot.Uid = pData.ID.ToString();

            m_EquipData = pData;

            UICommonHelper.LoadIcon(m_Icon, m_ItemData.Res);
            UICommonHelper.LoadQuality(m_Quality, m_EquipData.Quality);

            UpdateEquipInfo();

            m_EventGroup.Register(LogicEvent.EquipListChanged, (i, o) => { UpdateEquipInfo(); });
            m_EventGroup.Register(LogicEvent.EquipOn, (i, o) =>
            {
                var _EquipID = ((S2C_EquipOn)o).EquipID;
                if (_EquipID == m_EquipData.ID)
                    m_IsOn.Show();
            });
            m_EventGroup.Register(LogicEvent.EquipOff, (i, o) =>
            {
                var _EquipID = ((S2C_EquipOff)o).EquipID;
                if (_EquipID == m_EquipData.ID)
                    m_IsOn.Hide();
            });
        }

        private void UpdateEquipInfo()
        {
            if (m_EquipData == null)
                return;
            if (EquipManager.Ins.IsHave(m_EquipData.ID, m_EquipType))
            {
                m_Lock.Hide();
                m_Mask.Hide();
                m_GameEquipData = EquipManager.Ins.GetEquipData(m_EquipData.ID, m_EquipType);
                m_Level.text = "LV" + m_GameEquipData.Level;
            }
            else
            {
                m_Lock.Show();
                m_Mask.Show();
            }

            if (m_EquipData.ID == EquipManager.Ins.GetOnEquip(m_EquipType))
                m_IsOn.Show();
            else
                m_IsOn.Hide();

            //升级节点
            m_ProcessNode.Show();

            if (EquipManager.Ins.IsMaxLevel(m_EquipData.ID, m_EquipType))
            {
                //满级节点
                m_CanUpgrade.Hide();
                m_CantProcess.Hide();
                m_CanProcess.Show();
                m_CanProcess.fillAmount = 1;
                m_TextProcess.text = "Max";
                m_Level.text = "LV" + GameDefine.CommonItemMaxLevel;
            }
            else
            {
                var _CurCount = EquipManager.Ins.CurCount(m_EquipData.ID, m_EquipType);
                var _NeedCount = EquipManager.Ins.NeedCount(m_EquipData.ID, m_EquipType);
                if (_CurCount >= _NeedCount)
                {
                    m_CanUpgrade.Show();
                    m_CantProcess.Hide();
                    m_CanProcess.Show();
                }
                else
                {
                    m_CanUpgrade.Hide();
                    m_CantProcess.Show();
                    m_CanProcess.Hide();
                }

                float _Process = (float)_CurCount / _NeedCount;
                m_CantProcess.fillAmount = _Process;
                m_CanProcess.fillAmount = _Process;

                m_TextProcess.text = _CurCount + "/" + _NeedCount;
            }

            //更新红点uid
            //m_RedDot.Uid = m_EquipData.ID.ToString();

            m_Id = m_EquipData.ID;
        }

        #endregion

        // 点击道具
        public void OnClickItem()
        {
            m_ClickCB?.Invoke(this);
        }

        public void ShowSelected()
        {
            m_Selected.Show();
        }

        public void HideSelected()
        {
            m_Selected.Hide();
        }
    }
}