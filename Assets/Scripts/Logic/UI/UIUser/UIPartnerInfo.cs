using BreakInfinity;
using Configs;
using Framework.EventKit;
using Framework.Extension;
using Framework.UI;
using Logic.Common;
using Logic.Manager;
using Logic.UI.Common;
using Networks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.UIUser
{
    public class UIPartnerInfo : UIPage
    {
        public Image m_Quality;
        public TextMeshProUGUI m_QualityText;
        public Image m_Icon;
        public Image m_CantProcess;
        public Image m_CanProcess;
        public TextMeshProUGUI m_PartnerName;
        public TextMeshProUGUI m_TextProcess;
        public TextMeshProUGUI m_Level;
        public TextMeshProUGUI m_PartnerCD;
        public TextMeshProUGUI m_PartnerATK;

        public GameObject BtnOn;
        public GameObject BtnOff;
        public GameObject BtnFull;

        public GameObject BtnLeft;
        public GameObject BtnRight;

        public GameObject m_NotHaveNode;
        public TextMeshProUGUI m_HaveEffect;

        private PartnerData m_PartnerData;
        private GamePartnerData m_GamePartnerData;

        private bool IsHave = false;

        private void OnDisable()
        {
            m_EventGroup.Release();
        }

        public override void OnShow()
        {
            var _PartnerID = (int)m_OpenData_;
            m_PartnerData = PartnerCfg.GetData(_PartnerID);
            IsHave = PartnerManager.Ins.IsHave(_PartnerID);
            if (IsHave)
                m_GamePartnerData = PartnerManager.Ins.GetPartnerData(_PartnerID);
            else
                m_GamePartnerData = new GamePartnerData { PartnerID = _PartnerID, Level = 1, Count = 0 }; //没有按1级算

            var itemData = ItemCfg.GetData(m_GamePartnerData.PartnerID);
            UICommonHelper.LoadIcon(m_Icon, itemData.Res);
            UICommonHelper.LoadQuality(m_Quality, m_PartnerData.Quality);
            m_QualityText.text = UICommonHelper.GetQualityShowText(m_PartnerData.Quality);

            UpdatePartnerInfo();

            m_EventGroup.Register(LogicEvent.PartnerUpgraded, (i, o) => { OnPartnerUpgraded(); });

            if (IsHave)
            {
                bool _IsON = PartnerManager.Ins.IsOn(m_PartnerData.ID);
                if (_IsON)
                {
                    BtnOff.Show();
                }
                else
                {
                    if (PartnerManager.Ins.InOnFull())
                        BtnFull.Show();
                    else
                        BtnOn.Show();
                }
            }
            else
            {
                m_NotHaveNode.Show();
            }

            BtnLeft.Hide();
            BtnRight.Hide();
            if (_PartnerID <= 4001)
                BtnRight.Show();
            else if (_PartnerID >= GameDefine.PartnerMaxID)
                BtnLeft.Show();
            else
            {
                BtnLeft.Show();
                BtnRight.Show();
            }
        }

        private void UpdatePartnerInfo()
        {
            m_PartnerName.text = m_PartnerData.PartnerName;
            m_Level.text = "LV" + m_GamePartnerData.Level;
            m_PartnerCD.text = m_PartnerData.AtkSpeed + "秒";
            m_PartnerATK.text = ((m_PartnerData.AtkBase + (m_GamePartnerData.Level - 1) * m_PartnerData.AtkGrow) *
                                 Formula.GetGJJAtk())
                .ToUIString();

            var _CurCount = PartnerManager.Ins.CurCount(m_PartnerData.ID);
            var _NeedCount = PartnerManager.Ins.UpgradeNeedCount(m_PartnerData.ID);
            if (_CurCount >= _NeedCount)
            {
                m_CantProcess.Hide();
                m_CanProcess.Show();
            }
            else
            {
                m_CantProcess.Show();
                m_CanProcess.Hide();
            }

            float _Process = (float)_CurCount / _NeedCount;
            m_CantProcess.fillAmount = _Process;
            m_CanProcess.fillAmount = _Process;

            m_TextProcess.text = _CurCount + "/" + _NeedCount;

            m_HaveEffect.text =
                "+" + ((BigDouble)(PartnerManager.Ins.GetHaveEffect(m_PartnerData.ID) * 100f)).ToUIStringFloat() + "%";
        }

        private void OnPartnerUpgraded()
        {
            EventManager.Call(LogicEvent.ShowTips, "升级成功");
            UpdatePartnerInfo();
        }

        #region 按钮事件

        public void OnClickOn() //上
        {
            if (!PartnerManager.Ins.CanDoOn()) return;
            PartnerManager.Ins.DoOn(m_PartnerData.ID);
            OnClickClose();
        }

        public void OnClickOff() //下
        {
            PartnerManager.Ins.DoOff(m_PartnerData.ID);
            OnClickClose();
        }

        public void OnClickFull() //满
        {
            EventManager.Call(LogicEvent.ShowTips, "位置已满 请先解除一个技能");
        }

        #endregion

        public void OnClickUpgrade()
        {
            var _CurCount = PartnerManager.Ins.CurCount(m_PartnerData.ID);
            var _NeedCount = PartnerManager.Ins.UpgradeNeedCount(m_PartnerData.ID);
            if (_CurCount < _NeedCount)
            {
                EventManager.Call(LogicEvent.ShowTips, "数量不足");
                return;
            }

            PartnerManager.Ins.DoIntensify(m_PartnerData.ID, false);
        }

        public void OnClickLeft()
        {
            var _PartnerID = m_PartnerData.ID - 1;
            if (_PartnerID <= 4000)
                return;
            Reset();
            m_EventGroup.Release();
            m_OpenData_ = _PartnerID;
            OnShow();
        }

        public void OnClickRight()
        {
            var _PartnerID = m_PartnerData.ID + 1;
            if (_PartnerID > GameDefine.PartnerMaxID)
                return;
            Reset();
            m_EventGroup.Release();
            m_OpenData_ = _PartnerID;
            OnShow();
        }

        public void OnClickClose()
        {
            Reset();
            Hide();
        }

        private void Reset()
        {
            m_NotHaveNode.Hide();
            BtnOn.Hide();
            BtnOff.Hide();
            BtnFull.Hide();
        }
    }
}