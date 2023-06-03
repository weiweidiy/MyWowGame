using UnityEngine;
using System.Collections.Generic;
using System;
using Logic.Manager;
using System.Linq;
using Networks;
using UnityEngine.UI;
using Framework.Extension;
using Logic.Common;
using Framework.UI;
using Logic.UI.UISpoil;
using TMPro;
using Logic.Data;
using BreakInfinity;
using Framework.EventKit;
using Logic.UI.Common;

namespace Logic.UI.UIUser
{
    public class PartSpoil : MonoWithEvent
    {
        /// <summary>
        /// slot ui对象
        /// </summary>
        [SerializeField] SpoilSlotView[] m_SlotViews;

        [SerializeField] Button m_BtnHandBook;
        [SerializeField] Button m_BtnExchange;
        [SerializeField] TextMeshProUGUI m_TxtHoldInfo;
        [SerializeField] TextMeshProUGUI m_TxtCostInfo;
        [SerializeField] TextMeshProUGUI m_TxtTrophyInfo;
        [SerializeField] TextMeshProUGUI m_TxtAllHoldEffect;

        Sprite m_OriginSprite;

        private void Awake()
        {
            m_EventGroup.Register(LogicEvent.OnSpoilDraw, (i, o) => { OnSpoilDraw(o as SpoilData); });
            m_EventGroup.Register(LogicEvent.OnSpoilSlotUnlock, (i, o) => { OnSpoilSlotUnlock((int)o); });
            m_EventGroup.Register(LogicEvent.OnSpoilEquipChanged,
                (i, o) => { OnSpoilEqiupChanged(o as SpoilSlotData); });
            m_EventGroup.Register(LogicEvent.OnSpoilUpgrade, (i, o) => { OnSpoilUpgrade(o as SpoilData); });
            m_EventGroup.Register(LogicEvent.TropyChanged, (i, o) => { OnTrophyChanged(); });

            Initialize();
        }


        private void OnEnable()
        {
            RefreshAllView();
        }

        /// <summary>
        /// 初始化 awake调用1次
        /// </summary>
        public void Initialize()
        {
            foreach (var view in m_SlotViews)
            {
                view.onClicked += View_onClicked;
            }

            m_BtnExchange.onClick.AddListener(() => { SpoilManager.Ins.RequestSpoilDraw(); });

            m_OriginSprite = m_BtnExchange.image.sprite;

            m_BtnHandBook.onClick.AddListener(async () => { await UIManager.Ins.OpenUI<UISpoilHandBook>(); });
        }

        #region 刷新方法

        /// <summary>
        /// 刷新视图
        /// </summary>
        public void RefreshAllView()
        {
            var arrVO = GetAllSlotsVO();
            foreach (var vo in arrVO)
            {
                RefreshSlot(vo);
            }

            RefreshSpoilInfo();
            RefreshTrophy(GetTrophy());
            RefreshSpoilEffect();
        }

        /// <summary>
        /// 刷新一个槽位
        /// </summary>
        /// <param name="data"></param>
        public void RefreshSlot(SpoilSlotVO data)
        {
            var view = GetView(data.slotId);
            view.Refresh(data);
        }

        /// <summary>
        /// 刷新一个槽位
        /// </summary>
        /// <param name="slotId"></param>
        public void RefreshSlot(int slotId)
        {
            var slotView = GetView(slotId);
            var vo = GetSlotVO(slotId);
            slotView.Refresh(vo);
        }

        /// <summary>
        /// 刷新spoil 拥有数据
        /// </summary>
        public void RefreshSpoilInfo()
        {
            var holdNumber = GetSpoilHoldNumber();
            var maxNumber = GetSpoilMaxProgress();
            m_TxtHoldInfo.text = holdNumber.ToString() + "/" + maxNumber.ToString();

            //如果满级隐藏兑换按钮
            m_BtnExchange.gameObject.SetActive(holdNumber != maxNumber);
        }

        /// <summary>
        /// 刷新Spoil拥有效果
        /// </summary>
        public void RefreshSpoilEffect()
        {
            var atk = GetAllAtkEffect() * 100;
            var hp = GetAllHpEffect() * 100;
            m_TxtAllHoldEffect.text = $"攻击力 +{atk.ToUIString()}% " +
                                      $"体力 +{hp.ToUIString()}%";
        }

        /// <summary>
        /// 刷新Trophy数值
        /// </summary>
        public void RefreshTrophy(BigDouble trophy)
        {
            m_TxtTrophyInfo.text = trophy.ToUIString();

            //战功值变化，同时需要刷新抽卡按钮
            RefreshButtonDraw(trophy);
        }

        /// <summary>
        /// 刷新抽卡按钮
        /// </summary>
        /// <param name="trophy"></param>
        public void RefreshButtonDraw(BigDouble trophy)
        {
            //设置消耗
            var cost = GetDrawCost();
            m_TxtCostInfo.text = cost.ToUIString();

            //设置按钮状态
            if (cost > trophy)
            {
                m_TxtCostInfo.color = Color.red;
                m_BtnExchange.image.sprite = UICommonSprites.Ins.m_ButtonState[0];
            }
            else
            {
                m_TxtCostInfo.color = Color.white;
                m_BtnExchange.image.sprite = m_OriginSprite;
            }
        }

        #endregion

        #region view交互响应

        /// <summary>
        /// 战利品槽位被点击
        /// </summary>
        /// <param name="obj"></param>
        private async void View_onClicked(SpoilSlotVO slotVO)
        {
            if (slotVO.state == SpoilSlotVO.State.Locked)
            {
                //飘字提示
                EventManager.Call(LogicEvent.ShowTips, "还没有解锁！");
                return;
            }

            //打开uispoilDetail
            await UIManager.Ins.OpenUI<UISpoilDetail>(slotVO);
        }

        #endregion

        #region Get获取方法

        /// <summary>
        /// 获取所有slots的vo对象
        /// </summary>
        /// <returns></returns>
        private SpoilSlotVO[] GetAllSlotsVO()
        {
            var count = m_SlotViews.Length;
            var result = new SpoilSlotVO[count];
            for (int i = 0; i < count; i++)
            {
                var vo = GetSlotVO(m_SlotViews[i].GetId());
                result[i] = vo;
            }

            return result;
        }

        /// <summary>
        /// 获取 slot 值对象
        /// </summary>
        /// <param name="slotId"></param>
        /// <returns></returns>
        private SpoilSlotVO GetSlotVO(int slotId)
        {
            var vo = new SpoilSlotVO();
            vo.slotId = slotId;
            vo.state = GetSlotState(slotId);
            var slotData = GetSpoilSlotBySlotId(slotId);
            vo.spoilId = slotData == null ? 0 : slotData.SpoilId;
            var spoilData = GetSpoil(vo.spoilId);
            vo.spoilLevel = spoilData == null ? 0 : spoilData.Level;
            vo.iconPath = spoilData == null ? null : GetSpoilResPath(slotData.SpoilId);
            return vo;
        }


        /// <summary>
        /// 获取一个槽位
        /// </summary>
        /// <param name="slotId"></param>
        /// <returns></returns>
        private SpoilSlotView GetView(int slotId)
        {
            return m_SlotViews.Where(p => p.GetId().Equals(slotId)).SingleOrDefault();
        }

        /// <summary>
        /// 获取状态
        /// </summary>
        /// <param name="slotId"></param>
        /// <returns></returns>
        private SpoilSlotVO.State GetSlotState(int slotId)
        {
            if (!SpoilManager.Ins.QuerySlotUnlocked(slotId))
                return SpoilSlotVO.State.Locked;

            if (SpoilManager.Ins.GetSlotState(slotId) == null)
                return SpoilSlotVO.State.Unlocked;

            return SpoilSlotVO.State.Equiped;
        }

        /// <summary>
        /// 获取已拥有的Spoil数量
        /// </summary>
        /// <returns></returns>
        private int GetSpoilHoldNumber()
        {
            return SpoilManager.Ins.GetSpoilAmount();
        }

        /// <summary>
        /// 最大数量
        /// </summary>
        /// <returns></returns>
        private int GetSpoilMaxProgress()
        {
            return SpoilManager.Ins.GetMaxProgress();
        }

        /// <summary>
        /// 是否已抽完
        /// </summary>
        /// <returns></returns>
        private bool IsMaxProgress()
        {
            return SpoilManager.Ins.IsMaxProgress();
        }

        /// <summary>
        /// 获取当前抽卡消费
        /// </summary>
        /// <returns></returns>
        private BigDouble GetDrawCost()
        {
            return SpoilManager.Ins.GetDrawCost(SpoilManager.Ins.GetSpoilDrawProgress());
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
        /// 获取SpoilSlot数据
        /// </summary>
        /// <param name="slotId"></param>
        /// <returns></returns>
        private SpoilSlotData GetSpoilSlotBySlotId(int slotId)
        {
            return SpoilManager.Ins.GetSlotState(slotId);
        }

        /// <summary>
        /// 获取Slot数据
        /// </summary>
        /// <param name="spoilId"></param>
        /// <returns></returns>
        private SpoilSlotData GetSpoilSlotBySpoilId(int spoilId)
        {
            return SpoilManager.Ins.GetSlotStateBySpoilId(spoilId);
        }

        /// <summary>
        /// 获取spoil数据
        /// </summary>
        /// <param name="spoilId"></param>
        /// <returns></returns>
        private SpoilData GetSpoil(int spoilId)
        {
            return SpoilManager.Ins.GetSpoil(spoilId);
        }

        /// <summary>
        /// 获取spoil资源路径
        /// </summary>
        /// <param name="spoilId"></param>
        /// <returns></returns>
        public string GetSpoilResPath(int spoilId)
        {
            return SpoilManager.Ins.GetSpoilResPath(spoilId);
        }

        /// <summary>
        /// 获取spoilName
        /// </summary>
        /// <param name="m_spoilId"></param>
        /// <returns></returns>
        private string GetSpoilName(int m_spoilId)
        {
            return SpoilManager.Ins.GetSpoilName(m_spoilId);
        }

        /// <summary>
        /// 获取攻击加成效果
        /// </summary>
        /// <returns></returns>
        public BigDouble GetAllAtkEffect()
        {
            return SpoilManager.Ins.GetAllAtkEffect();
        }

        /// <summary>
        /// 获取生命加成效果
        /// </summary>
        /// <returns></returns>
        public BigDouble GetAllHpEffect()
        {
            return SpoilManager.Ins.GetAllHpEffect();
        }

        #endregion

        #region manager发送消息的响应方法,更新ui

        /// <summary>
        /// 抽卡成功
        /// </summary>
        private void OnSpoilDraw(SpoilData spoilData)
        {
            RefreshSpoilInfo();
            RefreshSpoilEffect();
            RefreshTrophy(GetTrophy());

            var arg = new UICommonObtain.Args();
            arg.resPath = GetSpoilResPath(spoilData.SpoilId);
            arg.name = GetSpoilName(spoilData.SpoilId);
            arg.title = "恭喜获得";

            EventManager.Call(LogicEvent.ShowObtain, arg);
        }


        /// <summary>
        /// 槽位解锁
        /// </summary>
        /// <param name="slotId"></param>
        private void OnSpoilSlotUnlock(int slotId)
        {
            RefreshSlot(slotId);
        }

        /// <summary>
        /// 装备变更了
        /// </summary>
        /// <param name="spoilSlotData"></param>
        private void OnSpoilEqiupChanged(SpoilSlotData spoilSlotData)
        {
            RefreshSlot(spoilSlotData.SlotId);
        }

        /// <summary>
        /// Spoil升级了
        /// </summary>
        /// <param name="spoilData"></param>
        private void OnSpoilUpgrade(SpoilData spoilData)
        {
            var spoilSlotData = GetSpoilSlotBySpoilId(spoilData.SpoilId);
            if (spoilSlotData != null)
            {
                RefreshSlot(spoilSlotData.SlotId);
            }

            RefreshSpoilEffect();
        }

        /// <summary>
        /// 战功发生改变了
        /// </summary>
        private void OnTrophyChanged()
        {
            RefreshTrophy(GetTrophy());
        }

        #endregion
    }
}