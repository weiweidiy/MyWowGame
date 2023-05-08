using System;
using Configs;
using DG.Tweening;
using Logic.Common;
using Logic.UI.Common;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.Cells
{
    public class CommonDrawItem : MonoBehaviour
    {
        [LabelText("道具类型")] public ItemType m_ItemType;

        public Image m_Quality;
        public Image m_Icon;
        public Action<CommonDrawItem, ItemType> m_ClickCB;

        public SkillData m_SkillData;
        public PartnerData m_PartnerData;
        public EquipData m_EquipData;
        public ItemData m_ItemData;

        public ItemType GetEquipType(int pEquipID)
        {
            return (ItemType)ItemCfg.GetData(pEquipID).Type;
        }

        public void InitBySkill(int pID)
        {
            m_ItemType = ItemType.Skill;
            m_ItemData = ItemCfg.GetData(pID);
            m_SkillData = SkillCfg.GetData(pID);
            UICommonHelper.LoadIcon(m_Icon, m_ItemData.Res);
            UICommonHelper.LoadQuality(m_Quality, m_SkillData.Quality);
        }

        public void InitByPartner(int pID)
        {
            m_ItemType = ItemType.Partner;
            m_ItemData = ItemCfg.GetData(pID);
            m_PartnerData = PartnerCfg.GetData(pID);
            UICommonHelper.LoadIcon(m_Icon, m_ItemData.Res);
            UICommonHelper.LoadQuality(m_Quality, m_PartnerData.Quality);
        }

        public void InitByEquip(int pID)
        {
            if (GetEquipType(pID) == ItemType.Weapon)
            {
                m_ItemType = ItemType.Weapon;
            }
            else if (GetEquipType(pID) == ItemType.Armor)
            {
                m_ItemType = ItemType.Armor;
            }

            m_ItemData = ItemCfg.GetData(pID);
            m_EquipData = EquipCfg.GetData(pID);
            UICommonHelper.LoadIcon(m_Icon, m_ItemData.Res);
            UICommonHelper.LoadQuality(m_Quality, m_EquipData.Quality);
        }

        private float duration = 0.1f;
        private Vector3 strength = new Vector3(1.1f, 1.1f, 1f);

        private void Start()
        {
            // TODO:动画效果优化
            transform.DOPunchScale(strength, duration);
        }

        private void OnDestroy()
        {
            m_ClickCB = null;
        }

        public void OnClickItem()
        {
            m_ClickCB?.Invoke(this, m_ItemType);
        }
    }
}