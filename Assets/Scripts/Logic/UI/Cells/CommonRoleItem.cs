using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using Framework.EventKit;
using Framework.Extension;
using Logic.Common;
using Logic.Manager;
using Logic.UI.Common;
using Networks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.Cells
{
    public class CommonRoleItem : MonoBehaviour
    {
        public string m_resPath { get; private set; }
        public string m_showResPath { get; private set; }
        public bool m_IsHeroHave { get; private set; }
        public GameRoleData m_RoleData { get; private set; }
        public HerosData m_HerosData { get; private set; }
        public SkillData m_CurHeroSkillData { get; private set; }
        public ItemData m_CurHeroItemData { get; private set; }
        public int[] m_AttStartLevel { get; private set; }
        public float[] m_AdditionAttGrow { get; private set; }
        public List<AttributeData> m_CurAttrsDataList { get; private set; }
        public List<ResData> m_CurAttrsResList { get; private set; }
        public AttributeData m_CurBreakAttrData { get; private set; }
        public ResData m_CurBreakResData { get; private set; }

        public Action<CommonRoleItem> m_ClickRole;
        public GameObject m_Selected;
        public Image m_RoleQuality;
        public TextMeshProUGUI m_RoleName;
        public Image m_RoleIcon;
        public GameObject m_IsOn;
        public TextMeshProUGUI m_RoleLevel;
        public GameObject m_Break;
        public TextMeshProUGUI m_BreakLevel;
        public TextMeshProUGUI m_Level;
        public GameObject m_IsHave;
        public GameObject m_HaveBottomIcon, m_HaveTopIcon;

        private EventGroup m_EventGroup = new();


        private void Awake()
        {
            m_EventGroup.Register(LogicEvent.RoleOn, OnRoleOn);
            m_EventGroup.Register(LogicEvent.RoleOff, OnRoleOff);
            m_EventGroup.Register(LogicEvent.RoleIntensify, OnRoleIntensify);
            m_EventGroup.Register(LogicEvent.RoleBreak, OnRoleBreak);
        }

        private void OnDestroy()
        {
            m_ClickRole = null;
            m_EventGroup.Release();
        }

        public void Init(HerosData herosData, bool heroHave)
        {
            Refresh(herosData, heroHave);
        }

        /// <summary>
        /// 刷新英雄数据
        /// </summary>
        /// <param name="herosData"></param>
        public void Refresh(HerosData herosData, bool heroHave)
        {
            //数据
            (m_HerosData, m_CurHeroSkillData, m_CurHeroItemData) =
                (herosData, SkillCfg.GetData(herosData.SkillID), ItemCfg.GetData(herosData.SkillID));
            var roleData = RoleManager.Ins.GetRoleData(herosData.ID);
            m_RoleData = roleData ?? new GameRoleData { RoleID = herosData.ID };
            //是否拥有
            // m_IsHeroHave = RoleManager.Ins.IsRoleHave(herosData.ID);
            m_IsHeroHave = heroHave;
            //属性解锁等级
            m_AttStartLevel = new[]
            {
                herosData.Att1StartLvl, herosData.Att2StartLvl,
                herosData.Att3StartLvl, herosData.Att4StartLvl
            };
            //拥有属性成长
            m_AdditionAttGrow = new[]
            {
                herosData.AdditionAtt1Grow, herosData.AdditionAtt2Grow,
                herosData.AdditionAtt3Grow, herosData.AdditionAtt4Grow
            };
            //拥有属性
            var additionAttrs = new[]
                { herosData.AdditionAtt1, herosData.AdditionAtt2, herosData.AdditionAtt3, herosData.AdditionAtt4 };
            m_CurAttrsDataList = additionAttrs.Select(AttributeCfg.GetData).ToList();
            //属性资源
            var additionResId = m_CurAttrsDataList.Select(x => x.Res).ToArray();
            m_CurAttrsResList = additionResId.Select(ResCfg.GetData).ToList();
            //突破属性
            m_CurBreakAttrData = AttributeCfg.GetData(herosData.BreakAtt);
            m_CurBreakResData = ResCfg.GetData(m_CurBreakAttrData.Res);
            //品质
            UICommonHelper.LoadQuenchingQuality(m_RoleQuality, herosData.Quality);
            //名字
            m_RoleName.text = herosData.Name;

            // Icon
            m_showResPath = RoleManager.Ins.GetResData(herosData.HeroShowResID).Res;
            m_resPath = RoleManager.Ins.GetResData(herosData.ResID).Res;
            UICommonHelper.LoadIcon(m_RoleIcon, m_resPath);

            if (!herosData.HeroShow)
            {
                this.Hide();
            }

            RefreshHaveState();
            RefreshRoleOnState();
        }

        /// <summary>
        /// 刷新英雄拥有状态
        /// </summary>
        private void RefreshHaveState()
        {
            if (m_IsHeroHave)
            {
                m_IsHave.Hide();
                m_RoleLevel.Show();
                UpdateRoleLevel(m_RoleData.RoleLevel, m_RoleData.RoleBreakLevel);
                m_HaveBottomIcon.Show();
                m_HaveTopIcon.Show();
            }
            else
            {
                m_IsHave.Show();
                m_RoleLevel.Hide();
                m_Break.Hide();
                m_RoleLevel.Hide();
                m_HaveBottomIcon.Hide();
                m_HaveTopIcon.Hide();
            }
        }

        /// <summary>
        /// 刷新英雄装配状态
        /// </summary>
        private void RefreshRoleOnState()
        {
            if (m_RoleData == null) return;

            if (RoleManager.Ins.IsRoleOn(m_RoleData.RoleID))
            {
                if(m_IsOn !=null)
                    m_IsOn.Show();
            }
            else
            {
                if (m_IsOn != null)
                    m_IsOn.Hide();
            }
        }

        /// <summary>
        /// 更新英雄等级
        /// </summary>
        /// <param name="roleLevel"></param>
        /// <param name="roleBreakLevel"></param>
        private void UpdateRoleLevel(int roleLevel, int roleBreakLevel)
        {
            if (roleBreakLevel > 0)
            {
                m_Break.Show();
                m_RoleLevel.Hide();
                m_BreakLevel.text = $"{roleBreakLevel}";
                m_Level.text = roleLevel.ToString();
            }
            else
            {
                m_Break.Hide();
                m_RoleLevel.Show();
                m_RoleLevel.text = $"{roleLevel}级";
            }
        }

        private void OnRoleOn(int eventId, object data)
        {
            var roleId = (int)data;
            if (roleId != m_RoleData?.RoleID) return;
            m_IsOn.Show();
        }

        private void OnRoleOff(int eventId, object data)
        {
            var roleId = (int)data;
            if (roleId != m_RoleData?.RoleID) return;
            m_IsOn.Hide();
        }

        private void OnRoleIntensify(int eventId, object data)
        {
            var (roleId, roleLevel, roleExp, roleBreakState) = (ValueTuple<int, int, int, bool>)data;
            if (roleId != m_RoleData?.RoleID) return;
            UpdateRoleLevel(roleLevel, m_RoleData.RoleBreakLevel);
        }

        private void OnRoleBreak(int eventId, object data)
        {
            var (roleId, roleBreakLevel, roleBreakState) = (ValueTuple<int, int, bool>)data;
            if (roleId != m_RoleData?.RoleID) return;
            UpdateRoleLevel(m_RoleData.RoleLevel, roleBreakLevel);
        }

        public void OnClickRole()
        {
            m_ClickRole?.Invoke(this);
        }

        public void ShowSelected()
        {
            m_Selected.Show();
        }

        public void HideSelected()
        {
            m_Selected.Hide();
        }

        public bool IsSelected()
        {
            return m_Selected.activeSelf;
        }
    }
}