using BreakInfinity;
using Framework.UI;
using Logic.Common;
using Logic.Data;
using Logic.Manager;
using Logic.UI.Common;
using Logic.UI.UIUser;
using Networks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.UISpoil
{
    public class UISpoilDetail : UIPage
    {
        [SerializeField] Button m_BtnClose;
        [SerializeField] TextMeshProUGUI m_TxtTrophy;
        [SerializeField] SpoilUnitView[] m_SpoilUnits;

        [SerializeField] TextMeshProUGUI m_TxtSpoilName;
        [SerializeField] TextMeshProUGUI m_TxtSkillDesc;
        [SerializeField] TextMeshProUGUI m_TxtHoldEffect;
        [SerializeField] TextMeshProUGUI m_TxtEquip;
        [SerializeField] TextMeshProUGUI m_TxtCost;
        [SerializeField] TextMeshProUGUI m_TxtSlotName;
        [SerializeField] TextMeshProUGUI m_TxtLevel;
        [SerializeField] TextMeshProUGUI m_TxtBreakthroughCost;
        [SerializeField] TextMeshProUGUI m_TxtBreakCount;
        [SerializeField] TextMeshProUGUI m_TxtPowerSauce;

        [SerializeField] Image m_IconSpoil;
        [SerializeField] Button m_BtnLeft;
        [SerializeField] Button m_BtnRight;
        [SerializeField] Button m_BtnUpgrade;
        [SerializeField] Button m_BtnEquip;
        [SerializeField] Button m_BtnUnHold;
        [SerializeField] Button m_BtnBreakthrough;

        [SerializeField] GameObject m_GoMax;
        [SerializeField] GameObject m_GoUpgrade;
        [SerializeField] GameObject m_GoBreak;


        SpoilSlotVO m_SlotVO;

        /// <summary>
        /// 当前选中Spoil索引
        /// </summary>
        int m_CurSpoilIndex = -1;

        /// <summary>
        /// 当前选中Spoil ID
        /// </summary>
        int m_CurSpoilId;

        /// <summary>
        /// 默认的sprite用于更换sprite后换回来
        /// </summary>
        Sprite m_OriginSprite;

        private void Awake()
        {
            m_BtnClose.onClick.AddListener(OnClose);
            m_BtnLeft.onClick.AddListener(OnLeftClick);
            m_BtnRight.onClick.AddListener(OnRightClick);
            m_BtnEquip.onClick.AddListener(OnEquipClick);
            m_BtnUpgrade.onClick.AddListener(OnUpgradeClick);
            m_BtnBreakthrough.onClick.AddListener(OnBreakthrough);

            m_OriginSprite = m_BtnUpgrade.image.sprite;

            foreach (var unit in m_SpoilUnits)
            {
                unit.onSelected += Unit_onSelected;
            }

            m_EventGroup.Register(LogicEvent.OnSpoilEquipChanged, (i, o) => { OnSpoilEqiup(o as SpoilSlotData); });
            m_EventGroup.Register(LogicEvent.OnSpoilUpgrade, (i, o) => { OnSpoilUpgrade(o as SpoilData); });
            m_EventGroup.Register(LogicEvent.TropyChanged, (i, o) => { OnTrophyChanged(); });
            m_EventGroup.Register(LogicEvent.RoleBreakOreChanged, (i, o) => { OnPowerSauceChanged(); });
            m_EventGroup.Register(LogicEvent.OnSpoilBreakthrough, (i, o) => { OnSpoilBreakthrough(o as SpoilBreakthroughData); });
        }



        public override void OnShow()
        {
            m_SlotVO = (SpoilSlotVO)m_OpenData_;
            
            //刷新Units
            RefreshSpoilUnits(SpoilManager.Ins.GetSpoilsBySlotId(m_SlotVO.slotId));
            SelectDefault();
            //刷新战功
            RefreshTrophy(GetTrophy());
            RefreshPowerSauce(GetPowerSauce());
        }

        private void OnClose()
        {
            Close();
        }

        /// <summary>
        /// 选中默认Spoil
        /// </summary>
        public void SelectDefault()
        {
            var index = GetDefualtIndex();
            SelectSpoilUnit(index);
        }

        /// <summary>
        /// 选中索引
        /// </summary>
        /// <param name="index"></param>
        private void SelectSpoilUnit(int index)
        {
            m_SpoilUnits[index].Select(true);
        }


        #region 刷新view

        /// <summary>
        /// 刷新Trophy值
        /// </summary>
        void RefreshTrophy(BigDouble value)
        {
            m_TxtTrophy.text = value.ToUIString();
            RefreshButtonUpgrade(value);

        }

        /// <summary>
        /// 刷新突破石
        /// </summary>
        /// <param name="value"></param>
        void RefreshPowerSauce(float value)
        {
            m_TxtPowerSauce.text = value.ToString();
            RefreshButtonBreakthrough(GetPowerSauce());
        }

        /// <summary>
        /// 刷新Spoil units（下半部UI）
        /// </summary>
        /// <param name="unitsCfg"></param>
        void RefreshSpoilUnits(List<Configs.SpoilData> unitsCfg)
        {
            var count = unitsCfg.Count;
            for(int i = 0; i < count; i ++)
            {
                var data = unitsCfg[i];
                m_SpoilUnits[i].Refresh(GetSpoilUnitVO(data));
            }
        }

        /// <summary>
        /// 刷新单个spoilunit
        /// </summary>
        /// <param name="spoilId"></param>
        void RefreshSpoilUnit(int spoilId)
        {
            var view = GetUnitViewById(spoilId);
            var vo = GetSpoilUnitVO(spoilId);
            view.Refresh(vo);
        }

        /// <summary>
        /// 刷新Spoil详细信息部分
        /// </summary>
        /// <param name="detailVO"></param>
        void RefreshSpoilDetail(SpoilDetailVO detailVO)
        {
            m_BtnEquip.gameObject.SetActive(detailVO.hold);
            m_BtnUpgrade.gameObject.SetActive(detailVO.hold && !detailVO.canBreakthrough);
            m_BtnUnHold.gameObject.SetActive(!detailVO.hold);
            m_BtnBreakthrough.gameObject.SetActive(detailVO.hold && detailVO.canBreakthrough);

            m_TxtSpoilName.text = detailVO.name;
            m_TxtHoldEffect.text = detailVO.holdEffect;
            m_TxtEquip.text = detailVO.txtEquipState;
            m_TxtEquip.text = detailVO.txtEquipState;
            m_BtnEquip.interactable = detailVO.btnEquipInteractable;
            m_TxtLevel.text = detailVO.txtLevel;
            m_TxtSkillDesc.text = detailVO.skillDesc;
            m_TxtCost.text = detailVO.cost.ToUIString();
            m_TxtSlotName.text = detailVO.slotName;
            m_TxtBreakthroughCost.text = detailVO.breakthroughCost.ToString();
            m_TxtBreakCount.text = detailVO.breakCount.ToString();

            m_GoMax.SetActive(detailVO.isMaxLevel);
            m_GoUpgrade.SetActive(!detailVO.isMaxLevel);
            m_GoBreak.SetActive(detailVO.breakCount > 0);


            UICommonHelper.LoadIcon(m_IconSpoil, detailVO.iconPath);
            RefreshButtonUpgrade(GetTrophy());
            RefreshButtonBreakthrough(GetPowerSauce());
        }



        /// <summary>
        /// 刷新Spoil详细信息部分
        /// </summary>
        /// <param name="spoilId"></param>
        void RefreshSpoilDetail(int spoilId)
        {
            var unitVO = GetSpoilUnitVO(spoilId);
            var detailVO = GetSpoilDetailVO(unitVO);
            RefreshSpoilDetail(detailVO);
        }

        /// <summary>
        /// 刷新抽卡按钮
        /// </summary>
        /// <param name="trophy"></param>
        public void RefreshButtonUpgrade(BigDouble trophy)
        {
            //设置消耗
            var cost = GetUpgradeCost(m_CurSpoilId);
            m_TxtCost.text = cost.ToUIString();

            //设置按钮状态
            if (cost > trophy)
            {
                m_TxtCost.color = Color.red;
                m_BtnUpgrade.image.sprite = UICommonSprites.Ins.m_ButtonState[0];
            }
            else
            {
                m_TxtCost.color = Color.white;
                m_BtnUpgrade.image.sprite = m_OriginSprite;
            }
        }

        public void RefreshButtonBreakthrough(int breakthroughOre)
        {
            var cost = GetBreakthroughCost(m_CurSpoilId);
            m_TxtBreakthroughCost.text = cost.ToString();

            //设置按钮状态
            if (cost > breakthroughOre)
            {
                m_TxtBreakthroughCost.color = Color.red;
                m_BtnBreakthrough.image.sprite = UICommonSprites.Ins.m_ButtonState[0];
            }
            else
            {
                m_TxtBreakthroughCost.color = Color.white;
                m_BtnBreakthrough.image.sprite = m_OriginSprite;
            }
        }

        #endregion

        #region view交互回调
        /// <summary>
        /// Unit 选中响应方法
        /// </summary>
        /// <param name="vo"></param>
        private void Unit_onSelected(SpoilUnitVO vo)
        {
            var selectedIndex = GetSpoilIndexById(vo.spoilId);
            if(m_CurSpoilIndex == selectedIndex)
                return;

            //如果之前有选中，则反选
            if(m_CurSpoilIndex != -1)
                m_SpoilUnits[m_CurSpoilIndex].Select(false);

            m_CurSpoilIndex = selectedIndex;
            m_CurSpoilId = vo.spoilId;

            //刷新详细信息
            RefreshSpoilDetail(GetSpoilDetailVO(vo));
        }

        /// <summary>
        /// 向右切换按钮被点击
        /// </summary>
        private void OnRightClick()
        {
            int nextIndex = GetNextSpoilIndex(m_CurSpoilIndex);
            SelectSpoilUnit(nextIndex);
        }

        /// <summary>
        /// 向左切换按钮被点击
        /// </summary>
        private void OnLeftClick()
        {
            int nextIndex = GetPreSpoilIndex(m_CurSpoilIndex);
            SelectSpoilUnit(nextIndex);
        }

        /// <summary>
        /// 升级强化按钮被点击
        /// </summary>
        private void OnUpgradeClick()
        {
            SpoilManager.Ins.RequestSpoilUpgrade(m_CurSpoilId);
        }

        /// <summary>
        /// 装备按钮被点击
        /// </summary>
        private void OnEquipClick()
        {
            SpoilManager.Ins.RequestSpoilEquip(m_CurSpoilId);
        }

        /// <summary>
        /// 突破按钮被点击
        /// </summary>
        private async void OnBreakthrough()
        {
            await UIManager.Ins.OpenUI<UISpoilBreak>(m_CurSpoilId);
            //SpoilManager.Ins.RequestSpoilUpgrade(m_CurSpoilId);
        }

        #endregion

        #region manager消息
        private void OnSpoilEqiup(SpoilSlotData slotData)
        {
            //刷新Units
            RefreshSpoilUnits(SpoilManager.Ins.GetSpoilsBySlotId(m_SlotVO.slotId));
            //刷新详细信息
            RefreshSpoilDetail(slotData.SpoilId);
        }

        private void OnSpoilUpgrade(SpoilData spoilData)
        {
            //刷新战功
            RefreshTrophy(GetTrophy());

            //刷新unit
            RefreshSpoilUnit(spoilData.SpoilId);
            //刷新详细信息
            RefreshSpoilDetail(spoilData.SpoilId);
        }

        private void OnTrophyChanged()
        {
            //刷新战功
            RefreshTrophy(GetTrophy());
        }

        private void OnPowerSauceChanged()
        {
            RefreshPowerSauce(GetPowerSauce());
        }

        private void OnSpoilBreakthrough(SpoilBreakthroughData spoilBreakthroughData)
        {
            //刷新unit
            RefreshSpoilUnit(spoilBreakthroughData.SpoilId);
            //刷新详细信息
            RefreshSpoilDetail(spoilBreakthroughData.SpoilId);
        }

        #endregion

        #region Get获取方法

        /// <summary>
        /// 获取默认的选中索引
        /// </summary>
        /// <returns></returns>
        int GetDefualtIndex()
        {
            int index = -1;
            for (int i = 0; i < m_SpoilUnits.Length; i++)
            {
                var unit = m_SpoilUnits[i];
                if (unit.GetEquiped())
                {
                    index = i;
                    break;
                }
            }

            if (index != -1)
                return index;

            for (int i = 0; i < m_SpoilUnits.Length; i++)
            {
                var unit = m_SpoilUnits[i];
                if (unit.GetHold())
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        /// <summary>
        /// 获取下一个spoil索引
        /// </summary>
        /// <returns></returns>
        private int GetNextSpoilIndex(int curIndex)
        {
            curIndex++;

            if (curIndex >= m_SpoilUnits.Length)
                curIndex = 0;

            return curIndex;
        }

        /// <summary>
        /// 获取上一个spoil索引
        /// </summary>
        /// <param name="curIndex"></param>
        /// <returns></returns>
        private int GetPreSpoilIndex(int curIndex)
        {
            curIndex--;

            if (curIndex < 0)
                curIndex = m_SpoilUnits.Length - 1;

            return curIndex;
        }

        /// <summary>
        /// 根据spoilId获取当前索引
        /// </summary>
        /// <param name="spoilId"></param>
        /// <returns></returns>
        int GetSpoilIndexById(int spoilId)
        {
            for(int i = 0; i < m_SpoilUnits.Length; i ++)
            {
                var unit = m_SpoilUnits[i];
                if (unit.GetId().Equals(spoilId))
                    return i;
            }
            throw new KeyNotFoundException("没有找到SpoilId " + spoilId);
        }

        /// <summary>
        /// 获取指定索引unitView对象
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        SpoilUnitView GetUnitViewByIndex(int index)
        {
            return m_SpoilUnits[index];
        }

        SpoilUnitView GetUnitViewById(int spoilId)
        {
            return m_SpoilUnits.Where((p) => p.GetId().Equals(spoilId)).SingleOrDefault();
        }


        /// <summary>
        /// 获取Spoil详细数据
        /// </summary>
        /// <param name="unitVO"></param>
        /// <returns></returns>
        private SpoilDetailVO GetSpoilDetailVO(SpoilUnitVO unitVO)
        {
            var vo = new SpoilDetailVO();
            var cfg = Configs.SpoilCfg.GetData(unitVO.spoilId);
            var slotState = SpoilManager.Ins.GetSlotStateBySpoilId(unitVO.spoilId);
            var spoilData = SpoilManager.Ins.GetSpoil(unitVO.spoilId);
            vo.skillDesc = SpoilManager.Ins.GetSkillDesc(unitVO.spoilId);
            BigDouble atk = SpoilManager.Ins.GetAtkEffect(unitVO.spoilId, unitVO.level);
            BigDouble hp = SpoilManager.Ins.GetHpEffect(unitVO.spoilId, unitVO.level);
            vo.cost = unitVO.hold == true ? SpoilManager.Ins.GetUpgradeCost(unitVO.spoilId, unitVO.level) : 0;
            vo.spoilId = unitVO.spoilId;
            vo.holdEffect = $"攻击力 +{atk.ToUIString()}% \n" +
                            $"体力 +{hp.ToUIString()}%";
            vo.btnEquipInteractable = slotState == null ? true : false;
            vo.txtEquipState = slotState == null ? "装配" : "已装配";
            vo.txtLevel = unitVO.hold == true? "Lv. " + unitVO.level.ToString() : "未拥有";
            vo.iconPath = unitVO.iconPath;
            vo.hold = unitVO.hold;            
            vo.name = cfg.SpoilName;
            vo.slotName = cfg.GroupName;
            vo.isMaxLevel = SpoilManager.Ins.IsMaxLevel(unitVO.spoilId);
            vo.canBreakthrough = spoilData == null ? false : SpoilManager.Ins.CanBreakthrough(spoilData);
            vo.breakthroughCost = SpoilManager.Ins.GetBreakCost(unitVO.spoilId);
            vo.breakCount = SpoilManager.Ins.GetSpoilBreakthroughLevel(unitVO.spoilId);
            return vo;
        }



        /// <summary>
        /// 单数值，就不创建vo了
        /// </summary>
        /// <returns></returns>
        BigDouble GetTrophy()
        {
            return GameDataManager.Ins.Trophy;
        }

        /// <summary>
        /// 获取突破石
        /// </summary>
        /// <returns></returns>
        private int GetPowerSauce()
        {
            return GameDataManager.Ins.BreakOre;
        }

        /// <summary>
        /// 获取当前抽卡消费
        /// </summary>
        /// <returns></returns>
        private BigDouble GetUpgradeCost(int spoilId)
        {
            var spoilData = SpoilManager.Ins.GetSpoil(spoilId);
            var level = spoilData == null ? 1 : spoilData.Level;
            return SpoilManager.Ins.GetUpgradeCost(spoilId, level);
        }

        private int GetBreakthroughCost(int spoilId)
        {
            return SpoilManager.Ins.GetBreakCost(spoilId);
        }

        /// <summary>
        /// 获取 spoil unit 值对象
        /// </summary>
        /// <param name="cfgData"></param>
        /// <returns></returns>
        SpoilUnitVO GetSpoilUnitVO(Configs.SpoilData cfgData)
        {
            return GetSpoilUnitVO(cfgData.ID);
        }

        /// <summary>
        /// 根据id 获取 spoil unit值对象
        /// </summary>
        /// <param name="spoilId"></param>
        /// <returns></returns>
        SpoilUnitVO GetSpoilUnitVO(int spoilId)
        {
            var vo = new SpoilUnitVO();

            vo.spoilId = spoilId;
            var spoil = SpoilManager.Ins.GetSpoil(vo.spoilId);
            vo.hold = spoil == null ? false : true;
            vo.level = spoil == null ? 0 : spoil.Level;
            var slot = SpoilManager.Ins.GetSlotStateBySpoilId(vo.spoilId);
            vo.equiped = slot == null ? false : true;
            vo.iconPath = SpoilManager.Ins.GetSpoilResPath(spoilId);
            return vo;
        }


        #endregion



    }
}