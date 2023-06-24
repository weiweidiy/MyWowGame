using Configs;
using DG.Tweening;
using Logic.Common;
using Logic.UI.Common;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.Cells
{
    public class CommonShowComposeItem : MonoBehaviour
    {
        [LabelText("道具类型")] public ItemType m_ItemType;
        private ItemData m_ItemData;
        private EquipData m_EquipData;
        private SkillData m_SkillData;
        private PartnerData m_PartnerData;

        public Image m_Quality;
        public Image m_Icon;
        public TextMeshProUGUI m_Count;

        private float duration = 0.1f;
        private Vector3 strength = new Vector3(1.1f, 1.1f, 1f);

        public void Init(int id, int count = -1)
        {
            m_ItemData = ItemCfg.GetData(id);
            m_ItemType = (ItemType)m_ItemData.Type;
            var quality = 0;

            switch (m_ItemType)
            {
                case ItemType.Weapon:
                case ItemType.Armor:
                    m_EquipData = EquipCfg.GetData(id);
                    quality = m_EquipData.Quality;
                    break;
                case ItemType.Skill:
                    m_SkillData = SkillCfg.GetData(id);
                    quality = m_SkillData.Quality;
                    break;
                case ItemType.Partner:
                    m_PartnerData = PartnerCfg.GetData(id);
                    quality = m_PartnerData.Quality;
                    break;
            }

            UICommonHelper.LoadIcon(m_Icon, m_ItemData.Res);
            UICommonHelper.LoadQuality(m_Quality, quality);
            m_Count.text = count <= 0 ? "Max" : count.ToString();
        }

        private void Start()
        {
            transform.DOPunchScale(strength, duration);
        }
    }
}