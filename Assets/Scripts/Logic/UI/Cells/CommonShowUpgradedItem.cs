using System;
using Configs;
using DG.Tweening;
using Logic.Common;
using Logic.UI.Common;
using Networks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.Cells
{
    public class CommonShowUpgradedItem : MonoBehaviour
    {
        [LabelText("道具类型"), ReadOnly] public ItemType m_ItemType;
        public Image m_Quality;
        public Image m_Icon;
        public TextMeshProUGUI m_OldLevel;
        public TextMeshProUGUI m_NewLevel;

        private float duration = 0.1f;
        private readonly Vector3 strength = new Vector3(1.3f, 1.3f, 1f);

        private void Start()
        {
            transform.DOPunchScale(strength, duration);
        }

        public void InitBySkill(GameSkillUpgradeData pData)
        {
            m_ItemType = ItemType.Skill;
            var itemData = ItemCfg.GetData(pData.SkillData.SkillID);
            var skillData = SkillCfg.GetData(pData.SkillData.SkillID);
            UICommonHelper.LoadIcon(m_Icon, itemData.Res);
            UICommonHelper.LoadQuality(m_Quality, skillData.Quality);

            m_OldLevel.text = pData.OldLevel.ToString();
            m_NewLevel.text = pData.SkillData.Level.ToString();
        }

        public void InitByPartner(GamePartnerUpgradeData pData)
        {
            m_ItemType = ItemType.Skill;
            var itemData = ItemCfg.GetData(pData.PartnerData.PartnerID);
            var partnerData = PartnerCfg.GetData(pData.PartnerData.PartnerID);
            UICommonHelper.LoadIcon(m_Icon, itemData.Res);
            UICommonHelper.LoadQuality(m_Quality, partnerData.Quality);

            m_OldLevel.text = pData.OldLevel.ToString();
            m_NewLevel.text = pData.PartnerData.Level.ToString();
        }

        public void InitByEquip(GameEquipUpgradeData pData)
        {
            var equipData = EquipCfg.GetData(pData.EquipData.EquipID);
            var itemData = ItemCfg.GetData(pData.EquipData.EquipID);
            switch ((ItemType)equipData.EquipType)
            {
                case ItemType.Weapon:
                    m_ItemType = ItemType.Weapon;
                    break;
                case ItemType.Armor:
                    m_ItemType = ItemType.Armor;
                    break;
            }

            UICommonHelper.LoadIcon(m_Icon, itemData.Res);
            UICommonHelper.LoadQuality(m_Quality, equipData.Quality);

            m_OldLevel.text = pData.OldLevel.ToString();
            m_NewLevel.text = pData.EquipData.Level.ToString();
        }
    }
}