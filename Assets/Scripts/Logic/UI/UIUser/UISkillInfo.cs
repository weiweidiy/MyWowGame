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
    public class UISkillInfo : UIPage
    {
        public Image m_Quality;
        public TextMeshProUGUI m_QualityText;
        public Image m_Icon;
        public Image m_CantProcess;
        public Image m_CanProcess;
        public TextMeshProUGUI m_SkillName;
        public TextMeshProUGUI m_TextProcess;
        public TextMeshProUGUI m_Level;
        public TextMeshProUGUI m_SkillCD;
        public TextMeshProUGUI m_SkillInfo;

        public GameObject BtnOn;
        public GameObject BtnOff;
        public GameObject BtnFull;
        public GameObject m_BtnUpgrade;
        public GameObject m_BtnCompose, m_BtnCanCompose, m_BtnCantCompose;

        public GameObject BtnLeft;
        public GameObject BtnRight;

        public GameObject m_NotHaveNode;
        public TextMeshProUGUI m_HaveEffect;

        private SkillData m_SkillData;
        private GameSkillData m_GameSkillData;

        private bool IsHave = false;
        private bool IsMaxLevel = false;

        private void OnDisable()
        {
            m_EventGroup.Release();
        }

        public override void OnShow()
        {
            var _SkillID = (int)m_OpenData_;
            m_SkillData = SkillCfg.GetData(_SkillID);
            IsHave = SkillManager.Ins.IsHave(_SkillID);
            m_GameSkillData = IsHave
                ? SkillManager.Ins.GetSkillData(_SkillID)
                : new GameSkillData { SkillID = _SkillID, Level = 1, Count = 0 }; //没有按1级算

            var itemData = ItemCfg.GetData(m_SkillData.ID);
            UICommonHelper.LoadIcon(m_Icon, itemData.Res);
            UICommonHelper.LoadQuality(m_Quality, m_SkillData.Quality);
            m_QualityText.text = UICommonHelper.GetQualityShowText(m_SkillData.Quality);

            IsMaxLevel = SkillManager.Ins.IsMaxLevel(_SkillID);

            UpdateSkillInfo();

            m_EventGroup.Register(LogicEvent.SkillUpgraded, (i, o) => { OnSkillUpgraded(); });
            m_EventGroup.Register(LogicEvent.SkillListChanged, (i, o) => { UpdateSkillInfo(); });

            if (IsHave)
            {
                m_NotHaveNode.Hide();

                //强化合成
                if (IsMaxLevel)
                {
                    m_BtnUpgrade.Hide();
                    m_BtnCompose.Show();
                }
                else
                {
                    m_BtnUpgrade.Show();
                    m_BtnCompose.Hide();
                }

                var isOn = SkillManager.Ins.IsOn(m_SkillData.ID);
                if (isOn)
                {
                    BtnOff.Show();
                }
                else
                {
                    if (SkillManager.Ins.InOnFull())
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
            if (_SkillID <= 3001)
                BtnRight.Show();
            else if (_SkillID >= GameDefine.SkillMaxID)
                BtnLeft.Show();
            else
            {
                BtnLeft.Show();
                BtnRight.Show();
            }
        }

        private void UpdateSkillInfo()
        {
            m_SkillName.text = m_SkillData.SkillName;
            m_Level.text = "LV" + m_GameSkillData.Level;
            m_SkillCD.text = m_SkillData.CD + "秒";
            m_SkillInfo.text = string.Format(m_SkillData.SkillDes,
                m_SkillData.DamageBase + (m_GameSkillData.Level - 1) * m_SkillData.DamageGrow);

            var needCount = 0;
            var curCount = SkillManager.Ins.CurCount(m_SkillData.ID);
            if (IsMaxLevel)
            {
                needCount = SkillManager.Ins.ComposeNeedCount(m_SkillData.ID);
                //服务器处理，客户端可以不处理
                m_Level.text = "LV" + GameDefine.CommonItemMaxLevel;
            }
            else
            {
                needCount = SkillManager.Ins.UpgradeNeedCount(m_SkillData.ID);
            }

            if (curCount >= needCount)
            {
                m_CantProcess.Hide();
                m_CanProcess.Show();
            }
            else
            {
                m_CantProcess.Show();
                m_CanProcess.Hide();
            }

            var process = (float)curCount / needCount;
            m_CantProcess.fillAmount = process;
            m_CanProcess.fillAmount = process;
            m_TextProcess.text = curCount + "/" + needCount;

            m_HaveEffect.text =
                "+" + ((BigDouble)(SkillManager.Ins.GetHaveEffect(m_SkillData.ID) * 100f)).ToUIStringFloat() + "%";
        }

        private void OnSkillUpgraded()
        {
            EventManager.Call(LogicEvent.ShowTips, "升级成功");
            UpdateSkillInfo();
        }

        #region 按钮事件

        public void OnClickOn() //上
        {
            //0502:技能优化成动态加载，不需要判断状态机
            //if (!SkillManager.Ins.CheckCanOperation(m_SkillData.ID))
            //{
            //    EventManager.Call(LogicEvent.ShowTips, "当前状态无法操作");
            //    return;
            //}

            if (!SkillManager.Ins.CanDoOn()) return;
            SkillManager.Ins.DoOn(m_SkillData.ID);
            OnClickClose();
        }

        public void OnClickOff() //下
        {
            if (!SkillManager.Ins.CheckCanOperation(m_SkillData.ID))
            {
                EventManager.Call(LogicEvent.ShowTips, "当前状态无法操作");
                return;
            }

            SkillManager.Ins.DoOff(m_SkillData.ID);
            OnClickClose();
        }

        public void OnClickFull() //满
        {
            EventManager.Call(LogicEvent.ShowTips, "位置已满 请先解除一个技能");
        }

        #endregion

        public void OnClickUpgrade()
        {
            var _CurCount = SkillManager.Ins.CurCount(m_SkillData.ID);
            var _NeedCount = SkillManager.Ins.UpgradeNeedCount(m_SkillData.ID);
            if (_CurCount < _NeedCount)
            {
                EventManager.Call(LogicEvent.ShowTips, "数量不足");
                return;
            }

            SkillManager.Ins.DoIntensify(m_SkillData.ID, false);
        }

        public void OnBtnComposeClick()
        {
            var curCount = SkillManager.Ins.CurCount(m_SkillData.ID);
            var needCount = SkillManager.Ins.ComposeNeedCount(m_SkillData.ID);
            if (curCount < needCount)
            {
                EventManager.Call(LogicEvent.ShowTips, "合成所需数量不足");
                return;
            }

            SkillManager.Ins.DoCompose(m_SkillData.ID);
        }

        public void OnClickLeft()
        {
            var _SkillID = m_SkillData.ID - 1;
            if (_SkillID <= 3000)
                return;
            Reset();
            m_EventGroup.Release();
            m_OpenData_ = _SkillID;
            OnShow();
        }

        public void OnClickRight()
        {
            var _SkillID = m_SkillData.ID + 1;
            if (_SkillID > GameDefine.SkillMaxID)
                return;
            Reset();
            m_EventGroup.Release();
            m_OpenData_ = _SkillID;
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