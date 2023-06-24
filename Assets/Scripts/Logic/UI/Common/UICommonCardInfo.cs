using System;
using BreakInfinity;
using Configs;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Framework.Extension;
using Framework.UI;
using Logic.Common;
using Logic.Manager;
using Networks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.Common
{
    public class UICommonCardInfo : UIPage
    {
        public Button m_BtnCloseInfoBg;
        private ItemType m_ItemType;
        private ItemData m_ItemData;
        private SkillData m_SkillData;
        private PartnerData m_PartnerData;
        private EquipData m_EquipData;
        private GameSkillData m_GameSkillData;
        private GamePartnerData m_GamePartnerData;
        private GameEquipData m_GameEquipData;

        public GameObject m_SkillDetailInfo;
        public GameObject m_PartnerDetailInfo;
        public GameObject m_EquipDetailInfo;
        private bool IsHave = false;
        private bool IsMaxLevel = false;

        // 技能信息
        [Header("技能信息")] public Image m_SkillQuality;
        public TextMeshProUGUI m_SkillQualityText;
        public Image m_SkillIcon;
        public Image m_SkillCantProcess;
        public Image m_SkillCanProcess;
        public TextMeshProUGUI m_SkillName;
        public TextMeshProUGUI m_SkillTextProcess;
        public TextMeshProUGUI m_SkillLevel;
        public TextMeshProUGUI m_SkillCD;
        public TextMeshProUGUI m_SkillInfo;
        public TextMeshProUGUI m_SkillHaveEffect;

        // 伙伴信息
        [Header("伙伴信息")] public Image m_PartnerQuality;
        public TextMeshProUGUI m_PartnerQualityText;
        public Image m_PartnerIcon;
        public Image m_PartnerCantProcess;
        public Image m_PartnerCanProcess;
        public TextMeshProUGUI m_PartnerName;
        public TextMeshProUGUI m_PartnerTextProcess;
        public TextMeshProUGUI m_PartnerLevel;
        public TextMeshProUGUI m_PartnerCD;
        public TextMeshProUGUI m_PartnerATK;
        public TextMeshProUGUI m_PartnerHaveEffect;

        // 装备信息
        [Header("装备信息")] public Image m_EquipQuality;
        public TextMeshProUGUI m_EquipQualityText;
        public Image m_EquipIcon;
        public Image m_EquipCantProcess;
        public Image m_EquipCanProcess;
        public TextMeshProUGUI m_EquipTextProcess;
        public TextMeshProUGUI m_EquipLevel;
        public TextMeshProUGUI m_EquipName;
        public TextMeshProUGUI m_EquipHaveEffect;
        public TextMeshProUGUI m_EquipEffect;
        public GameObject m_UpArrow;
        public TextMeshProUGUI m_UpEquipEffect;
        public GameObject m_DownArrow;
        public TextMeshProUGUI m_DownEquipEffect;

        private void Awake()
        {
            m_BtnCloseInfoBg.onClick.AddListener(OnBtnCloseInfoClick);
            m_SkillDetailInfo.transform.localScale = Vector3.zero;
            m_PartnerDetailInfo.transform.localScale = Vector3.zero;
            m_EquipDetailInfo.transform.localScale = Vector3.zero;
        }

        public override void OnShow()
        {
            base.OnShow();
            var (itemType, id) = (ValueTuple<ItemType, int>)m_OpenData_;
            m_ItemType = itemType;
            GetCfgData(itemType, id);
        }

        private void GetCfgData(ItemType itemType, int id)
        {
            m_ItemData = ItemCfg.GetData(id);
            switch (itemType)
            {
                case ItemType.Weapon:
                case ItemType.Armor:
                    m_EquipData = EquipCfg.GetData(id);
                    IsHave = EquipManager.Ins.IsHave(id, itemType);
                    m_GameEquipData = IsHave
                        ? EquipManager.Ins.GetEquipData(id, itemType)
                        : new GameEquipData { EquipID = id, Level = 1, Count = 0 };
                    UICommonHelper.LoadIcon(m_EquipIcon, m_ItemData.Res);
                    UICommonHelper.LoadQuality(m_EquipQuality, m_EquipData.Quality);
                    m_EquipQualityText.text = UICommonHelper.GetQualityShowText(m_EquipData.Quality);
                    IsMaxLevel = EquipManager.Ins.IsMaxLevel(id, itemType);
                    UpdateEquipInfo(itemType);
                    m_EquipDetailInfo.Show();
                    m_EquipDetailInfo.transform.DOScale(new Vector3(1, 1, 1), 0.2f);
                    break;
                case ItemType.Skill:
                    m_SkillData = SkillCfg.GetData(id);
                    IsHave = SkillManager.Ins.IsHave(id);
                    m_GameSkillData = IsHave
                        ? SkillManager.Ins.GetSkillData(id)
                        : new GameSkillData { SkillID = id, Level = 1, Count = 0 };
                    UICommonHelper.LoadIcon(m_SkillIcon, m_ItemData.Res);
                    UICommonHelper.LoadQuality(m_SkillQuality, m_SkillData.Quality);
                    m_SkillQualityText.text = UICommonHelper.GetQualityShowText(m_SkillData.Quality);
                    IsMaxLevel = SkillManager.Ins.IsMaxLevel(id);
                    UpdateSkillInfo();
                    m_SkillDetailInfo.Show();
                    m_SkillDetailInfo.transform.DOScale(new Vector3(1, 1, 1), 0.2f);
                    break;
                case ItemType.Partner:
                    m_PartnerData = PartnerCfg.GetData(id);
                    IsHave = PartnerManager.Ins.IsHave(id);
                    m_GamePartnerData = IsHave
                        ? PartnerManager.Ins.GetPartnerData(id)
                        : new GamePartnerData() { PartnerID = id, Level = 1, Count = 0 };
                    UICommonHelper.LoadIcon(m_PartnerIcon, m_ItemData.Res);
                    UICommonHelper.LoadQuality(m_PartnerQuality, m_PartnerData.Quality);
                    m_PartnerQualityText.text = UICommonHelper.GetQualityShowText(m_PartnerData.Quality);
                    IsMaxLevel = PartnerManager.Ins.IsMaxLevel(id);
                    UpdatePartnerInfo();
                    m_PartnerDetailInfo.Show();
                    m_PartnerDetailInfo.transform.DOScale(new Vector3(1, 1, 1), 0.2f);
                    break;
            }
        }

        public void OnBtnCloseInfoClick()
        {
            DOTween.Sequence().Append(DoScaleDetailInfo()).AppendInterval(0.2f).AppendCallback(() =>
            {
                m_SkillDetailInfo.Hide();
                m_EquipDetailInfo.Hide();
                m_PartnerDetailInfo.Hide();
                this.Hide();
            });
        }

        private Tween DoScaleDetailInfo()
        {
            TweenerCore<Vector3, Vector3, VectorOptions> tween = null;
            switch (m_ItemType)
            {
                case ItemType.Weapon:
                case ItemType.Armor:
                    tween = m_EquipDetailInfo.transform.DOScale(new Vector3(0, 0, 1), 0.2f);
                    break;
                case ItemType.Skill:
                    tween = m_SkillDetailInfo.transform.DOScale(new Vector3(0, 0, 1), 0.2f);
                    break;
                case ItemType.Partner:
                    tween = m_PartnerDetailInfo.transform.DOScale(new Vector3(0, 0, 1), 0.2f);
                    break;
            }

            return tween;
        }

        #region 更新技能信息

        private void UpdateSkillInfo()
        {
            m_SkillName.text = m_SkillData.SkillName;
            m_SkillLevel.text = "LV" + m_GameSkillData.Level;
            m_SkillCD.text = m_SkillData.CD + "秒";
            m_SkillInfo.text = string.Format(m_SkillData.SkillDes,
                m_SkillData.DamageBase + (m_GameSkillData.Level - 1) * m_SkillData.DamageGrow);

            var needCount = 0;
            var curCount = SkillManager.Ins.CurCount(m_SkillData.ID);
            if (IsMaxLevel)
            {
                needCount = SkillManager.Ins.ComposeNeedCount(m_SkillData.ID);
                //服务器处理，客户端可以不处理
                m_SkillLevel.text = "LV" + GameDefine.CommonItemMaxLevel;
            }
            else
            {
                needCount = SkillManager.Ins.UpgradeNeedCount(m_SkillData.ID);
            }

            if (curCount >= needCount)
            {
                m_SkillCantProcess.Hide();
                m_SkillCanProcess.Show();
            }
            else
            {
                m_SkillCantProcess.Show();
                m_SkillCanProcess.Hide();
            }

            var process = (float)curCount / needCount;
            m_SkillCantProcess.fillAmount = process;
            m_SkillCanProcess.fillAmount = process;

            m_SkillTextProcess.text = curCount + "/" + needCount;

            m_SkillHaveEffect.text =
                "+" + ((BigDouble)(SkillManager.Ins.GetHaveEffect(m_SkillData.ID) * 100f)).ToUIStringFloat() + "%";
        }

        #endregion

        #region 更新伙伴信息

        private void UpdatePartnerInfo()
        {
            m_PartnerName.text = m_PartnerData.PartnerName;
            m_PartnerLevel.text = "LV" + m_GamePartnerData.Level;
            m_PartnerCD.text = m_PartnerData.AtkSpeed + "秒";
            m_PartnerATK.text = ((m_PartnerData.AtkBase + (m_GamePartnerData.Level - 1) * m_PartnerData.AtkGrow) *
                                 Formula.GetGJJAtk())
                .ToUIString();


            var needCount = 0;
            var curCount = PartnerManager.Ins.CurCount(m_PartnerData.ID);
            if (IsMaxLevel)
            {
                needCount = PartnerManager.Ins.ComposeNeedCount(m_PartnerData.ID);
                //服务器处理，客户端可以不处理
                m_PartnerLevel.text = "LV" + GameDefine.CommonItemMaxLevel;
            }
            else
            {
                needCount = PartnerManager.Ins.UpgradeNeedCount(m_PartnerData.ID);
            }

            if (curCount >= needCount)
            {
                m_PartnerCantProcess.Hide();
                m_PartnerCanProcess.Show();
            }
            else
            {
                m_PartnerCantProcess.Show();
                m_PartnerCanProcess.Hide();
            }

            var process = (float)curCount / needCount;
            m_PartnerCantProcess.fillAmount = process;
            m_PartnerCanProcess.fillAmount = process;

            m_PartnerTextProcess.text = curCount + "/" + needCount;

            m_PartnerHaveEffect.text =
                "+" + ((BigDouble)(PartnerManager.Ins.GetHaveEffect(m_PartnerData.ID) * 100f)).ToUIStringFloat() + "%";
        }

        #endregion

        #region 装备信息

        private void UpdateEquipInfo(ItemType equipType)
        {
            m_EquipName.text = m_EquipData.EquipName;
            m_EquipLevel.text = "LV" + m_GameEquipData.Level;

            var needCount = 0;
            var curCount = EquipManager.Ins.CurCount(m_EquipData.ID, equipType);
            if (IsMaxLevel)
            {
                needCount = EquipManager.Ins.ComposeNeedCount(m_EquipData.ID, equipType);
                //服务器处理，客户端可以不处理
                m_EquipLevel.text = "LV" + GameDefine.CommonItemMaxLevel;
            }
            else
            {
                needCount = EquipManager.Ins.NeedCount(m_EquipData.ID, equipType);
            }

            if (curCount >= needCount)
            {
                m_EquipCantProcess.Hide();
                m_EquipCanProcess.Show();
            }
            else
            {
                m_EquipCantProcess.Show();
                m_EquipCanProcess.Hide();
            }

            var process = (float)curCount / needCount;
            m_EquipCantProcess.fillAmount = process;
            m_EquipCanProcess.fillAmount = process;

            m_EquipTextProcess.text = curCount + "/" + needCount;

            m_EquipHaveEffect.text =
                "+" + ((BigDouble)(EquipManager.Ins.GetHaveEffect(m_EquipData.ID, equipType) * 100f))
                .ToUIStringFloat() + "%";
            m_EquipEffect.text =
                "+" + ((BigDouble)(EquipManager.Ins.GetEquipEffect(m_EquipData.ID, equipType) * 100f))
                .ToUIStringFloat() + "%";


            var curEquipOnID = 0;
            switch (equipType)
            {
                case ItemType.Weapon:
                    curEquipOnID = EquipManager.Ins.CurWeaponOnID;
                    break;
                case ItemType.Armor:
                    curEquipOnID = EquipManager.Ins.CurArmorOnID;
                    break;
            }

            // 未装备武器或防具处理
            if (curEquipOnID == 0)
            {
                Debug.Log("当前未装备武器或防具");
                return;
            }

            var curEffect = EquipManager.Ins.GetEquipEffect(m_EquipData.ID, equipType);
            var equipEffect = EquipManager.Ins.GetEquipEffect(curEquipOnID, equipType);
            if (curEffect > equipEffect)
            {
                m_UpArrow.Show();
                m_DownArrow.Hide();
                m_UpEquipEffect.text = ((BigDouble)((curEffect - equipEffect) * 100f)).ToUIStringFloat() + "%";
            }
            else
            {
                m_UpArrow.Hide();
                m_DownArrow.Show();
                m_DownEquipEffect.text =
                    ((BigDouble)((equipEffect - curEffect) * 100f)).ToUIStringFloat() + "%";
            }
        }

        #endregion

        #region 动画效果

        #endregion
    }
}