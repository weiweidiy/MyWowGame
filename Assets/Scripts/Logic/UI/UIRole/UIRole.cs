using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using Framework.EventKit;
using Framework.Extension;
using Framework.UI;
using Logic.Common;
using Logic.Config;
using Logic.Data;
using Logic.Manager;
using Logic.UI.Cells;
using Logic.UI.Common;
using Networks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.UIRole
{
    public class UIRole : UIPage
    {
        [Header("角色生成相关")] public Transform m_ItemListRoot;
        public CommonRoleItem m_CommonRoleItem;
        [Header("角色升级突破相关")] public TextMeshProUGUI m_MushRoom;
        public TextMeshProUGUI m_BreakOre;
        [Header("角色面板相关")] public TextMeshProUGUI m_RoleName;
        public Image m_RoleQuality;
        public Image m_RoleIcon;
        public GameObject m_IsOn;
        public TextMeshProUGUI m_IsHave;
        public GameObject m_LevelNode;
        public Image m_CanProcess;
        public TextMeshProUGUI m_TextProcess;
        public TextMeshProUGUI m_Level;
        public GameObject m_BreakIcon, m_CantBreakImage, m_BreakImage;
        public TextMeshProUGUI m_BreakLevel;
        public GameObject m_BtnOn, m_BtnOnCan, m_BtnOnCant;
        public GameObject m_BtnIntensify, m_BtnIntensifyCan, m_BtnIntensifyCant;
        public TextMeshProUGUI m_IntensifyCost, m_BreakCost;
        public GameObject m_BtnBreak, m_BtnBreakCan, m_BtnBreakCant;
        public GameObject m_BtnShopGet;
        [Header("角色固有技能相关")] public Image m_RoleSkillIcon;
        public TextMeshProUGUI m_RoleSkillName;
        public TextMeshProUGUI m_RoleSkillDes;
        [Header("角色属性相关")] public Image[] m_AdditionAttIcon;
        public TextMeshProUGUI[] m_AdditionAttUnLockLevel;
        public TextMeshProUGUI[] m_AdditionAttDes;
        public TextMeshProUGUI[] m_AdditionAttNextDes;
        public Image m_BreakAttrIcon;
        public TextMeshProUGUI m_BreakAttrLevel;
        public TextMeshProUGUI m_BreakAttrDes;
        public GameObject[] m_AttrLock;

        //其他
        private CommonRoleItem m_CurSelectRole;
        private bool m_CurIsHave;
        private GameRoleData m_CurRoleData;
        private HerosData m_CurHeroData;
        private SkillData m_CurHeroSkillData;
        private List<AttributeData> m_CurAttrsDataList;
        public List<ResData> m_CurAttrsResList;
        public ResData m_CurBreakResData;
        private AttributeData m_CurBreakAttrData;
        private int[] m_AttStartLevel;
        private float[] m_AdditionAttGrow;

        //英雄排序
        private List<CommonRoleItem> m_RoleItemList = new List<CommonRoleItem>();
        private List<SortRoleData> m_SortRoleList = new List<SortRoleData>();

        //强化按钮长按
        public ButtonEx m_BtnExIntensify;

        [Serializable]
        public class SortRoleData
        {
            public bool Have;
            public HerosData HerosData;

            public SortRoleData(bool have, HerosData herosData)
            {
                Have = have;
                HerosData = herosData;
            }
        }

        private void Awake()
        {
            m_EventGroup.Register(LogicEvent.RoleMushRoomChanged, (i, o) => { OnRoleMushRoomChanged(); });
            m_EventGroup.Register(LogicEvent.RoleBreakOreChanged, (i, o) => { OnRoleBreakOreChanged(); });
            m_EventGroup.Register(LogicEvent.RoleOn, OnRoleOn);
            m_EventGroup.Register(LogicEvent.RoleOff, OnRoleOff);
            m_EventGroup.Register(LogicEvent.RoleIntensify, OnRoleIntensify);
            m_EventGroup.Register(LogicEvent.RoleBreak, OnRoleBreak);

            m_BtnExIntensify.onClick.AddListener(OnBtnExIntensifyClick);
            m_BtnExIntensify.onLongClick.AddListener(OnBtnExIntensifyClick);
        }

        public override void OnShow()
        {
            base.OnShow();
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            var herosData = ConfigManager.Ins.m_HerosCfg.AllData;
            foreach (var sortRoleData in from heroData in herosData
                     let have = RoleManager.Ins.IsRoleHave(heroData.Value.ID)
                     select new SortRoleData(have, heroData.Value))
            {
                m_SortRoleList.Add(sortRoleData);
            }

            var sortRoleDatas = m_SortRoleList
                .OrderByDescending(x => x.Have)
                .ThenByDescending(x => x.HerosData.Quality);

            foreach (var sortRoleData in sortRoleDatas)
            {
                var roleItem = Instantiate(m_CommonRoleItem, m_ItemListRoot);
                roleItem.Init(sortRoleData.HerosData, sortRoleData.Have);
                roleItem.m_ClickRole += OnClickRoleItem;
                m_RoleItemList.Add(roleItem);
                roleItem.gameObject.Show();
                if (RoleManager.Ins.IsRoleOn(sortRoleData.HerosData.ID))
                {
                    roleItem.OnClickRole();
                }
            }

            OnRoleMushRoomChanged();
            OnRoleBreakOreChanged();
        }

        /// <summary>
        /// 英雄排序
        /// 排序优先级：已拥有 > 品质 > 未拥有 > ID
        /// </summary>
        /// <param name="roleIdList"></param>
        private void SortRoleItemList(IReadOnlyCollection<int> roleIdList)
        {
            //更新英雄拥有状态
            foreach (var sortRoleData in from sortRoleData in m_SortRoleList
                     from roleId in roleIdList
                     where sortRoleData.HerosData.ID == roleId
                     select sortRoleData)
            {
                sortRoleData.Have = true;
            }

            //根据英雄排序优先级进行排序
            var sortRoleDatas = m_SortRoleList
                .OrderByDescending(x => x.Have)
                .ThenByDescending(x => x.HerosData.Quality).ToList();

            //刷新英雄数据
            for (var i = 0; i < m_RoleItemList.Count; i++)
            {
                m_RoleItemList[i].Refresh(sortRoleDatas[i].HerosData, sortRoleDatas[i].Have);
                if (RoleManager.Ins.IsRoleOn(sortRoleDatas[i].HerosData.ID))
                {
                    m_RoleItemList[i].OnClickRole();
                }
            }
        }

        /// <summary>
        /// 点击英雄更新英雄相关信息
        /// </summary>
        /// <param name="cItem"></param>
        private void UpdateRole(CommonRoleItem cItem)
        {
            //角色属性相关
            m_CurIsHave = cItem.m_IsHeroHave;
            m_CurRoleData = cItem.m_RoleData;
            m_CurHeroData = cItem.m_HerosData;
            m_CurHeroSkillData = cItem.m_CurHeroSkillData;

            //角色面板相关
            m_RoleName.text = m_CurHeroData.Name;
            UICommonHelper.LoadQuenchingQuality(m_RoleQuality, m_CurHeroData.Quality);
            UICommonHelper.LoadIcon(m_RoleIcon, cItem.m_showResPath);
            m_IsHave.text = m_CurHeroData.GetHeroDes;

            //角色固有技能相关
            UICommonHelper.LoadIcon(m_RoleSkillIcon, cItem.m_CurHeroItemData.Res);
            m_RoleSkillName.text = m_CurHeroSkillData.SkillName;
            m_RoleSkillDes.text = string.Format(m_CurHeroSkillData.SkillDes, m_CurHeroSkillData.DamageBase);

            //角色属性相关
            m_AttStartLevel = cItem.m_AttStartLevel;
            m_AdditionAttGrow = cItem.m_AdditionAttGrow;
            m_CurAttrsDataList = cItem.m_CurAttrsDataList;
            m_CurAttrsResList = cItem.m_CurAttrsResList;
            m_CurBreakResData = cItem.m_CurBreakResData;
            m_CurBreakAttrData = cItem.m_CurBreakAttrData;
            UpdateRoleAttrDes(m_CurRoleData.RoleLevel);
            UpdateRoleBreakAttrDes(m_CurRoleData.RoleBreakLevel);
            RefreshHaveState();
            RefreshRoleOnState();
        }

        /// <summary>
        /// 更新英雄等级
        /// </summary>
        /// <param name="roleLevel"></param>
        /// <param name="roleExp"></param>
        /// <param name="roleBreakState"></param>
        private void UpdateRoleLevel(int roleLevel, int roleExp, bool roleBreakState)
        {
            var roleTotalExp = Formula.GetRoleIntensifyTotalExp(roleLevel);
            m_Level.text = $"{roleLevel}级";
            m_TextProcess.text = $"{roleExp}/{roleTotalExp}";
            m_CanProcess.fillAmount = (float)roleExp / roleTotalExp;
            RefreshBtnBreakState(roleLevel, roleBreakState);
            RefreshAdditionAttrState(roleLevel);
        }

        /// <summary>
        /// 更新英雄突破等级
        /// </summary>
        /// <param name="roleBreakLevel"></param>
        /// <param name="roleBreakState"></param>
        private void UpdateRoleBreakLevel(int roleBreakLevel, bool roleBreakState)
        {
            m_BreakLevel.text = roleBreakLevel.ToString();
            m_BreakAttrLevel.text = roleBreakLevel.ToString();
            RefreshBreakIcon(roleBreakLevel);
            RefreshBtnBreakState(m_CurRoleData.RoleLevel, roleBreakState);
        }

        /// <summary>
        /// 更新英雄属性
        /// </summary>
        /// <param name="roleLevel"></param>
        private void UpdateRoleAttrDes(int roleLevel)
        {
            var count = m_CurAttrsDataList.Count;
            for (var i = 0; i < count; i++)
            {
                float attrValue = 0;
                if (roleLevel < m_AttStartLevel[i])
                {
                    attrValue = m_CurAttrsDataList[i].Value + (m_AttStartLevel[i] * m_AdditionAttGrow[i]);
                }
                else
                {
                    var attrMaxLevel = roleLevel >= GameDefine.RoleAttrMaxLevel
                        ? GameDefine.RoleAttrMaxLevel
                        : roleLevel;
                    attrValue = m_CurAttrsDataList[i].Value + (attrMaxLevel * m_AdditionAttGrow[i]);
                }

                m_AdditionAttUnLockLevel[i].text = $"{m_AttStartLevel[i]}级";
                m_AdditionAttDes[i].text = string.Format(m_CurAttrsDataList[i].Des, attrValue);
                m_AdditionAttNextDes[i].text =
                    roleLevel >= GameDefine.RoleAttrMaxLevel ? "Max" : $"+{m_AdditionAttGrow[i]}%";
                UICommonHelper.LoadIcon(m_AdditionAttIcon[i], m_CurAttrsResList[i].Res);
            }
        }

        /// <summary>
        /// 更新英雄突破属性
        /// </summary>
        /// <param name="roleBreakLevel"></param>
        private void UpdateRoleBreakAttrDes(int roleBreakLevel)
        {
            var breakAttrValue = roleBreakLevel > 0
                ? roleBreakLevel * m_CurHeroData.BreakAttGrow
                : m_CurBreakAttrData.Value;
            m_BreakAttrDes.text = string.Format(m_CurBreakAttrData.Des, breakAttrValue);
            UICommonHelper.LoadIcon(m_BreakAttrIcon, m_CurBreakResData.Res);
        }

        /// <summary>
        /// 刷新英雄拥有状态
        /// </summary>
        private void RefreshHaveState()
        {
            if (m_CurIsHave)
            {
                m_IsHave.Hide();
                m_LevelNode.Show();
                m_BtnOn.Show();
                m_BtnIntensify.Show();
                m_BtnShopGet.Hide();
                m_CantBreakImage.Show();
                UpdateRoleLevel(m_CurRoleData.RoleLevel, m_CurRoleData.RoleExp, m_CurRoleData.RoleBreakState);
                UpdateRoleBreakLevel(m_CurRoleData.RoleBreakLevel, m_CurRoleData.RoleBreakState);
                RefreshBtnIntensifyState();
            }
            else
            {
                m_IsHave.Show();
                m_LevelNode.Hide();
                m_BtnOn.Hide();
                m_BtnIntensify.Hide();
                m_BtnShopGet.Show();
                m_CantBreakImage.Hide();
                m_BtnBreak.Hide();
                m_BreakLevel.text = string.Empty;
                m_BreakAttrLevel.text = string.Empty;
                foreach (var attrLock in m_AttrLock)
                {
                    attrLock.Show();
                }
            }
        }

        /// <summary>
        /// 刷新英雄装配状态
        /// </summary>
        private void RefreshRoleOnState()
        {
            if (m_CurRoleData == null) return;

            if (RoleManager.Ins.IsRoleOn(m_CurRoleData.RoleID))
            {
                m_IsOn.Show();
                m_BtnOnCan.Hide();
                m_BtnOnCant.Show();
            }
            else
            {
                m_IsOn.Hide();
                m_BtnOnCan.Show();
                m_BtnOnCant.Hide();
            }
        }

        /// <summary>
        /// 刷新强化状态
        /// </summary>
        private void RefreshBtnIntensifyState()
        {
            if (RoleManager.Ins.IsCanRoleIntensify(1))
            {
                m_BtnIntensifyCan.Show();
                m_BtnIntensifyCant.Hide();
            }
            else
            {
                m_BtnIntensifyCan.Hide();
                m_BtnIntensifyCant.Show();
            }
        }

        /// <summary>
        /// 刷新突破状态
        /// </summary>
        /// <param name="roleLevel"></param>
        /// <param name="roleBreakState"></param>
        private void RefreshBtnBreakState(int roleLevel, bool roleBreakState)
        {
            if (roleBreakState)
            {
                m_BtnBreak.Show();
                m_BtnIntensify.Hide();

                var cost = Formula.GetRoleBreakCost(roleLevel);
                m_BreakCost.text = $"{GameDataManager.Ins.BreakOre}/{cost}";
                if (RoleManager.Ins.IsCanRoleBreak(cost))
                {
                    m_BtnBreakCan.Show();
                    m_BtnBreakCant.Hide();
                }
                else
                {
                    m_BtnBreakCan.Hide();
                    m_BtnBreakCant.Show();
                }
            }
            else
            {
                m_BtnBreak.Hide();
                m_BtnIntensify.Show();
            }
        }

        /// <summary>
        /// 刷新突破Icon状态
        /// </summary>
        /// <param name="roleBreakLevel"></param>
        private void RefreshBreakIcon(int roleBreakLevel)
        {
            if (roleBreakLevel >= 1)
            {
                m_BreakIcon.Show();
                m_BreakImage.Show();
            }
            else
            {
                m_BreakIcon.Hide();
                m_BreakImage.Hide();
            }
        }

        /// <summary>
        /// 刷新拥有属性状态
        /// </summary>
        /// <param name="roleLevel"></param>
        private void RefreshAdditionAttrState(int roleLevel)
        {
            foreach (var attrLock in m_AttrLock)
            {
                attrLock.Show();
            }

            var startLevels = new int[]
            {
                m_CurHeroData.Att1StartLvl, m_CurHeroData.Att2StartLvl, m_CurHeroData.Att3StartLvl,
                m_CurHeroData.Att4StartLvl, m_CurHeroData.BreakStartLvl
            };

            for (var i = 0; i < startLevels.Length; i++)
            {
                if (roleLevel < startLevels[i]) continue;
                var numToShow = Math.Min(i + 1, m_AttrLock.Length);
                for (var j = 0; j < numToShow; j++)
                {
                    m_AttrLock[j].Hide();
                }
            }
        }

        #region 事件

        public void OnBtnCloseClick()
        {
            this.Hide();
        }

        public void OnBtnOnClick()
        {
            if (m_CurSelectRole == null) return;

            if (RoleManager.Ins.IsRoleOn(m_CurRoleData.RoleID))
            {
                EventManager.Call(LogicEvent.ShowTips, "当前角色已装配");
            }
            else
            {
                RoleManager.Ins.DoRoleOn(m_CurRoleData.RoleID);
            }
        }

        public void OnBtnExIntensifyClick()
        {
            if (RoleManager.Ins.IsCanRoleIntensify(1))
            {
                RoleManager.Ins.DoRoleIntensify(m_CurRoleData.RoleID);
            }
            else
            {
                EventManager.Call(LogicEvent.ShowTips, "所需数量未满足需求");
            }
        }

        public void OnBtnBreakClick()
        {
            if (m_CurSelectRole == null) return;

            var cost = Formula.GetRoleBreakCost(m_CurRoleData.RoleLevel);
            if (RoleManager.Ins.IsCanRoleBreak(cost))
            {
                RoleManager.Ins.DoRoleBreak(m_CurRoleData.RoleID);
            }
            else
            {
                EventManager.Call(LogicEvent.ShowTips, "所需数量未满足需求");
            }
        }

        public void OnBtnShopGetClick()
        {
        }

        private void OnClickRoleItem(CommonRoleItem cItem)
        {
            if (m_CurSelectRole == cItem)
            {
                return;
            }

            if (m_CurSelectRole != null)
            {
                m_CurSelectRole.HideSelected();
            }

            m_CurSelectRole = cItem;
            m_CurSelectRole.ShowSelected();
            UpdateRole(m_CurSelectRole);
        }

        private void OnRoleMushRoomChanged()
        {
            m_MushRoom.text = GameDataManager.Ins.MushRoom.ToString();
            m_IntensifyCost.text = $"{GameDataManager.Ins.MushRoom}/1";
            RefreshBtnIntensifyState();
        }

        private void OnRoleBreakOreChanged()
        {
            m_BreakOre.text = GameDataManager.Ins.BreakOre.ToString();
            if (m_CurRoleData != null)
            {
                RefreshBtnBreakState(m_CurRoleData.RoleLevel, m_CurRoleData.RoleBreakState);
            }
        }

        private void OnRoleOn(int eventId, object data)
        {
            var roleId = (int)data;
            if (roleId != m_CurRoleData.RoleID) return;
            m_IsOn.Show();
            m_BtnOnCan.Hide();
            m_BtnOnCant.Show();
        }

        private void OnRoleOff(int eventId, object data)
        {
            var roleId = (int)data;
            if (roleId != m_CurRoleData.RoleID) return;
            m_IsOn.Hide();
            m_BtnOnCan.Show();
            m_BtnOnCant.Hide();
        }

        private void OnRoleIntensify(int eventId, object data)
        {
            var (roleId, roleLevel, roleExp, roleBreakState) = (ValueTuple<int, int, int, bool>)data;
            if (roleId != m_CurRoleData.RoleID) return;
            UpdateRoleLevel(roleLevel, roleExp, roleBreakState);
            UpdateRoleAttrDes(roleLevel);
        }

        private void OnRoleBreak(int eventId, object data)
        {
            var (roleId, roleBreakLevel, roleBreakState) = (ValueTuple<int, int, bool>)data;
            if (roleId != m_CurRoleData.RoleID) return;
            UpdateRoleBreakLevel(roleBreakLevel, roleBreakState);
            UpdateRoleBreakAttrDes(roleBreakLevel);
        }

        #endregion
    }
}