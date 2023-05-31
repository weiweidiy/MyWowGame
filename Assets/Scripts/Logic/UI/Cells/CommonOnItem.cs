using Configs;
using Framework.EventKit;
using Framework.Extension;
using Framework.UI;
using Logic.Common;
using Logic.Manager;
using Logic.UI.Common;
using Logic.UI.UIUser;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Logic.UI.Cells
{
    /// <summary>
    /// 通用上阵Item组件
    /// </summary>
    public class CommonOnItem : MonoBehaviour
    {
        [LabelText("道具类型")] public ItemType m_ItemType;
        public Image m_Quality;
        public Image m_Icon;
        public TextMeshProUGUI m_Level;
        public TextMeshProUGUI m_ItemName;
        public GameObject m_BKPlus;

        public EventGroup m_EventGroup = new();

        private void Awake()
        {
            m_EventGroup.Register(LogicEvent.SkillListChanged, (i, o) =>
            {
                if (m_ItemType == ItemType.Skill)
                {
                    if (m_SkillData == null) return;
                    var m_GameSkillData = SkillManager.Ins.GetSkillData(m_SkillData.ID);
                    m_Level.text = "LV" + m_GameSkillData.Level;
                }
            });

            m_EventGroup.Register(LogicEvent.PartnerListChanged, (i, o) =>
            {
                if (m_ItemType == ItemType.Partner)
                {
                    if (m_PartnerData == null) return;
                    var m_GamePartnerData = PartnerManager.Ins.GetPartnerData(m_PartnerData.ID);
                    m_Level.text = "LV" + m_GamePartnerData.Level;
                }
            });
        }

        private void OnDestroy()
        {
            m_EventGroup.Release();
        }

        public async void OnClick()
        {
            if (m_ItemType == ItemType.Skill)
            {
                if (m_SkillData == null) return;
                await UIManager.Ins.OpenUI<UISkillInfo>(m_SkillData.ID);
            }
            else if (m_ItemType == ItemType.Partner)
            {
                if (m_PartnerData == null) return;
                await UIManager.Ins.OpenUI<UIPartnerInfo>(m_PartnerData.ID);
            }
        }

        public void Reset()
        {
            m_Quality.Hide();
            m_Icon.Hide();
            m_Level.Hide();
            m_BKPlus.Show();
            // 当技能或伙伴解除时点击上方CommonOnItem还会弹出相应信息处理
            m_ItemType = ItemType.None;
            m_SkillData = null;
            m_PartnerData = null;
        }

        #region 技能

        private SkillData m_SkillData;

        public void InitBySkill(SkillData pSkillData)
        {
            m_SkillData = pSkillData;
            m_ItemType = ItemType.Skill;
            var itemData = ItemCfg.GetData(m_SkillData.ID);
            UICommonHelper.LoadIcon(m_Icon, itemData.ResShow);
            UICommonHelper.LoadSkillQuality(m_Quality, m_SkillData.Quality);
            var m_GameSkillData = SkillManager.Ins.GetSkillData(m_SkillData.ID);
            m_Level.text = "LV" + m_GameSkillData.Level;
            m_Icon.Show();
            m_Quality.Show();
            m_Level.Show();
            m_BKPlus.SetActive(false);
        }

        #endregion

        #region 伙伴

        private PartnerData m_PartnerData;

        public void InitByPartner(PartnerData pPartnerData)
        {
            m_PartnerData = pPartnerData;
            m_ItemType = ItemType.Partner;
            var itemData = ItemCfg.GetData(m_PartnerData.ID);
            UICommonHelper.LoadIcon(m_Icon, itemData.ResShow);
            UICommonHelper.LoadPartnerQuality(m_Quality, m_PartnerData.Quality);
            var m_GamePartnerData = PartnerManager.Ins.GetPartnerData(m_PartnerData.ID);
            m_Level.text = "LV" + m_GamePartnerData.Level;
            m_Icon.Show();
            m_Quality.Show();
            m_Level.Show();
            m_BKPlus.SetActive(false);
        }

        #endregion
    }
}