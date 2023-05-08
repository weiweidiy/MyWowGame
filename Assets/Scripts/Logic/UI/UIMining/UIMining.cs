using System;
using System.Collections;
using System.Collections.Generic;
using Configs;
using DG.Tweening;
using Framework.EventKit;
using Framework.Extension;
using Framework.Helper;
using Framework.UI;
using Logic.Common;
using Logic.Manager;
using Logic.UI.Cells;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityTimer;

namespace Logic.UI.UIMining
{
    public class UIMining : UIPage
    {
        public Button m_BtnClose;
        public Button m_AutoMining;
        private bool m_IsFirstMining;

        [Header("层数相关")] public TextMeshProUGUI m_Floor;

        [Header("矿锤相关")] public TextMeshProUGUI m_CountDownText;
        private Timer m_MiningTimer;
        private bool m_IsTimerStart;
        public TextMeshProUGUI m_HammerText;

        [Header("矿石相关")] public TextMeshProUGUI m_MineText;

        [Header("炸弹相关")] public TextMeshProUGUI m_BombText;

        [Header("透视镜相关")] public TextMeshProUGUI m_ScopeText;

        [Header("奖励获取相关")] public CommonMiningReward m_CommonMiningReward;
        public Transform m_CommonMiningRewardRoot;

        [Header("引擎相关")] public Image m_Engine;
        public Image m_CanProgress;
        public Image m_CantProgress;
        public TextMeshProUGUI m_EngineText;

        // TODO: GameDefine配置
        private int k_AutoMiningCount = 3;

        private void Awake()
        {
            m_BtnClose.onClick.AddListener(OnBtnCloseClick);
            m_AutoMining.onClick.AddListener(OnBtnAutoMiningClick);
            m_EventGroup.Register(LogicEvent.ShowCrossedGrid, OnShowCrossedGrid);
            m_EventGroup.Register(LogicEvent.HideCrossedGrid, OnHideCrossedGrid);
            m_EventGroup.Register(LogicEvent.ShowNineGrid, OnShowNineGrid);
            m_EventGroup.Register(LogicEvent.HideNineGrid, OnHideNineGrid);
            m_EventGroup.Register(LogicEvent.ShowMiningReward, OnShowMiningReward);
            m_EventGroup.Register(LogicEvent.MiningDataChanged, OnMiningDataChanged);
            m_EventGroup.Register(LogicEvent.EngineGet, (i, o) => { OnEngineGet(); });
        }

        private void OnEngineGet()
        {
            // TODO: 播放获取引擎动画
            m_CanProgress.Hide();
            m_CantProgress.Show();
            DOTween.Sequence(m_Engine.DOFade(1, 1f)).OnComplete(() =>
            {
                m_CanProgress.Show();
                m_CantProgress.Hide();
            });
        }

        private void OnMiningDataChanged(int eventId, object data)
        {
            var miningDataType = (MiningType)data;
            switch (miningDataType)
            {
                case MiningType.Gear:
                    OnGearChanged();
                    break;
                case MiningType.Hammer:
                    OnHammerChanged();
                    break;
                case MiningType.CopperMine:
                case MiningType.SilverMine:
                case MiningType.GoldMine:
                    OnMineChanged();
                    break;
                case MiningType.Bomb:
                    OnBombChanged();
                    break;
                case MiningType.Scope:
                    OnScopeChanged();
                    break;
                case MiningType.Door:
                    OnFloorChanged();
                    break;
            }
        }

        private void OnHammerChanged()
        {
            m_HammerText.text = $"{MiningManager.Ins.m_MiningData.m_HammerCount}/{GameDefine.MaxHammerCount}";
            UpdateMiningTimer();
        }

        private void OnMineChanged()
        {
            m_MineText.text = MiningManager.Ins.m_MiningData.m_MineCount.ToString();
        }

        private void OnBombChanged()
        {
            m_BombText.text = MiningManager.Ins.m_MiningData.m_BombCount.ToString();
        }

        private void OnScopeChanged()
        {
            m_ScopeText.text = MiningManager.Ins.m_MiningData.m_ScopeCount.ToString();
        }

        private void OnFloorChanged()
        {
            m_Floor.text = MiningManager.Ins.m_MiningData.m_FloorCount.ToString();
            // TODO: 切换层数特效
            HideAllMiningProp();
            UpdateCommonMiningGround();
            UpdateCommonMiningProp();
        }

        private void OnGearChanged()
        {
            m_EngineText.text =
                $"{MiningManager.Ins.m_MiningData.m_GearCount}/{EngineManager.Ins.curEngineGetIdGearCost}";
            m_CanProgress.fillAmount = (float)MiningManager.Ins.m_MiningData.m_GearCount /
                                       EngineManager.Ins.curEngineGetIdGearCost;
        }

        public override void OnShow()
        {
            base.OnShow();
            Refresh();
            UpdateMiningTimer();
            if (!m_IsFirstMining)
            {
                m_IsFirstMining = true;
                UpdateCommonMiningGround();
                UpdateCommonMiningProp();
            }
        }

        private void Refresh()
        {
            m_Floor.text = MiningManager.Ins.m_MiningData.m_FloorCount.ToString();
            m_HammerText.text = $"{MiningManager.Ins.m_MiningData.m_HammerCount}/{GameDefine.MaxHammerCount}";
            m_MineText.text = MiningManager.Ins.m_MiningData.m_MineCount.ToString();
            m_BombText.text = MiningManager.Ins.m_MiningData.m_BombCount.ToString();
            m_ScopeText.text = MiningManager.Ins.m_MiningData.m_ScopeCount.ToString();
            m_EngineText.text =
                $"{MiningManager.Ins.m_MiningData.m_GearCount}/{EngineManager.Ins.curEngineGetIdGearCost}";
            m_CanProgress.fillAmount = (float)MiningManager.Ins.m_MiningData.m_GearCount /
                                       EngineManager.Ins.curEngineGetIdGearCost;
        }

        private void OnShowMiningReward(int eventId, object data)
        {
            var (treasureType, rewardId, rewardCount) =
                (ValueTuple<MiningType, int, int>)data;
            var position = MiningManager.Ins.GetThreeMatchPosition(treasureType);
            var rewardItem = Instantiate(m_CommonMiningReward, position, Quaternion.identity,
                m_CommonMiningRewardRoot);
            rewardItem.Init(treasureType, rewardId, rewardCount);
            rewardItem.Show();
            rewardItem.transform.DOMoveY(rewardItem.transform.position.y + 1f, 0.5f)
                .SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    Destroy(rewardItem.gameObject);
                    MiningManager.Ins.m_PropDoTweenCount--;
                });
        }

        private void OnBtnCloseClick()
        {
            if (MiningManager.Ins.m_PropDoTweenCount != 0)
            {
                EventManager.Call(LogicEvent.ShowTips, "当前状态不可退出考古");
                return;
            }

            m_MiningTimer?.Cancel();
            m_IsTimerStart = false;
            this.Hide();
        }

        #region 一键考古

        private void OnBtnAutoMiningClick()
        {
            // TODO: 一键考古可使用条件

            if (MiningManager.Ins.m_PropDoTweenCount != 0)
            {
                EventManager.Call(LogicEvent.ShowTips, "当前状态不可使用一键考古");
                return;
            }

            StartCoroutine(AutoMiningCoroutine(k_AutoMiningCount));
            MiningManager.Ins.m_PropDoTweenCount += k_AutoMiningCount;
        }

        private IEnumerator AutoMiningCoroutine(int count)
        {
            var delay = new WaitForSeconds(0.1f);
            for (var i = 0; i < 20; i++)
            {
                if (!m_CommonMiningGround[i].m_GroundMask.activeSelf)
                {
                    continue;
                }

                m_CommonMiningGround[i].HideMiningGround();
                yield return delay;
            }

            yield return new WaitForSeconds(3f);
            count--;
            MiningManager.Ins.m_PropDoTweenCount--;
            MiningManager.Ins.SendMsgC2SUpdateMiningData(MiningType.Door, MiningUpdateType.Increase);
            if (count > 0)
            {
                yield return new WaitForSeconds(1f);
                StartCoroutine(AutoMiningCoroutine(count));
            }
        }

        #endregion

        #region 矿锤次数回复时间逻辑

        private void UpdateMiningTimer()
        {
            // TODO:处理时间计算问题
            if (MiningManager.Ins.m_MiningData.m_HammerCount < GameDefine.MaxHammerCount)
            {
                if (!m_IsTimerStart)
                {
                    m_IsTimerStart = true;
                    m_CountDownText.Show();
                    var second = GameDefine.AddHammerTime;
                    m_MiningTimer = Timer.Register(1f,
                        () =>
                        {
                            second -= 1;
                            m_CountDownText.text = TimeHelper.FormatSecond(second);
                            if (second <= 0)
                            {
                                m_MiningTimer.Cancel();
                                m_IsTimerStart = false;
                                //TODO:添加矿锤增加数量
                                MiningManager.Ins.SendMsgC2SUpdateMiningData(MiningType.Hammer,
                                    MiningUpdateType.Increase);
                            }
                        },
                        isLooped: true, useRealTime: true);
                }
            }
            else
            {
                m_MiningTimer?.Cancel();
                m_IsTimerStart = false;
                m_CountDownText.Hide();
            }
        }

        #endregion

        #region 考古道具生成逻辑

        public List<CommonMiningProp> m_CommonMiningProp = new List<CommonMiningProp>();

        private void HideAllMiningProp()
        {
            foreach (var commonMiningProp in m_CommonMiningProp)
            {
                commonMiningProp.Hide();
            }
        }


        private void UpdateCommonMiningProp()
        {
            /*
             * 进入前清空Manager中的数据
             * 根据权重生成6个奖励
             * 将6个奖励添加到list中
             * 将list分配给6个组
             * 生成
             */
            MiningManager.Ins.Clear();

            var pID = MiningManager.Ins.m_MiningData.m_FloorCount * 10;
            if (MiningManager.Ins.m_MiningData.m_FloorCount > GameDefine.MiningLoopStartFloor)
            {
                var groupId = RandomHelper.Range(GameDefine.MiningLoopStartFloor + 1,
                    GameDefine.MiningLoopEndFloor + 1);
                pID = groupId * 10;
            }

            var propType = MiningManager.Ins.GetMiningPropType(pID + 1);
            var defaultOpenList = MiningManager.Ins.GetDefaultOpenMiningProp(pID + 1);

            var grid = new List<int>();
            for (var i = 1; i <= 5; i++)
            {
                grid.Add(DigMapCfg.GetData(pID + i).FirstC);
                grid.Add(DigMapCfg.GetData(pID + i).SecondC);
                grid.Add(DigMapCfg.GetData(pID + i).ThridC);
                grid.Add(DigMapCfg.GetData(pID + i).ForthC);
            }

            for (var i = 0; i < grid.Count; i++)
            {
                switch ((MiningType)grid[i])
                {
                    case MiningType.Door:
                        m_CommonMiningProp[i].Init(grid[i], i);
                        m_CommonMiningGround[i].m_TreasureType = grid[i];
                        break;
                    case MiningType.SpecialProp:
                        var itemDetails = MiningManager.Ins.GetItemDetails(grid[i]);
                        var itemId = itemDetails[0];
                        m_CommonMiningProp[i].Init(itemId, i);
                        m_CommonMiningGround[i].m_TreasureType = itemId;
                        break;
                    default:
                        m_CommonMiningProp[i].Init(propType[grid[i] - 1], i);
                        m_CommonMiningGround[i].m_TreasureType = propType[grid[i] - 1];

                        if (!MiningManager.Ins.m_PropCountDictionary.ContainsKey(propType[grid[i] - 1]))
                        {
                            MiningManager.Ins.m_PropCountDictionary.Add(propType[grid[i] - 1], 3);
                        }

                        break;
                }
            }

            MiningManager.Ins.m_CommonMiningProp = m_CommonMiningProp;


            // 考古默认被挖开地块
            for (var i = 0; i < defaultOpenList.Count; i++)
            {
                if (defaultOpenList[i] == 1)
                {
                    m_CommonMiningGround[i].HideMiningGround();
                }
            }
        }

        #endregion

        #region 考古地面被特殊道具显示逻辑

        public List<CommonMiningGround> m_CommonMiningGround = new List<CommonMiningGround>();

        private void UpdateCommonMiningGround()
        {
            for (int i = 0; i < m_CommonMiningGround.Count; i++)
            {
                m_CommonMiningGround[i].Init(i);
            }
        }

        private void OnShowCrossedGrid(int eventId, object data)
        {
            var index = (int)data;

            // 将该格子对应的index转化成行列
            var row = index / 4;
            var column = index % 4;

            //center
            m_CommonMiningGround[index].m_GeenMask.Show();

            //left
            var indexLeftColumn = column - 1;
            if (indexLeftColumn >= 0)
            {
                m_CommonMiningGround[index - 1].m_GeenMask.Show();
            }

            //right
            var indexRightColumn = column + 1;
            if (indexRightColumn <= 3)
            {
                m_CommonMiningGround[index + 1].m_GeenMask.Show();
            }

            //top
            var indexTopRow = row - 1;
            if (indexTopRow >= 0)
            {
                m_CommonMiningGround[index - 4].m_GeenMask.Show();
            }

            //bottom
            var indexBottomRow = row + 1;
            if (indexBottomRow <= 4)
            {
                m_CommonMiningGround[index + 4].m_GeenMask.Show();
            }
        }

        private void OnHideCrossedGrid(int eventId, object data)
        {
            var (index, isHideGround) = (ValueTuple<int, bool>)data;

            // 将该格子对应的index转化成行列
            var row = index / 4;
            var column = index % 4;

            //center
            m_CommonMiningGround[index].m_GeenMask.Hide();
            if (isHideGround)
            {
                m_CommonMiningGround[index].HideMiningGround();
            }

            //left
            var indexLeftColumn = column - 1;
            if (indexLeftColumn >= 0)
            {
                var indexLeft = index - 1;
                m_CommonMiningGround[indexLeft].m_GeenMask.Hide();
                if (isHideGround)
                {
                    m_CommonMiningGround[indexLeft].HideMiningGround();
                }
            }

            //right
            var indexRightColumn = column + 1;
            if (indexRightColumn <= 3)
            {
                var indexRight = index + 1;
                m_CommonMiningGround[indexRight].m_GeenMask.Hide();
                if (isHideGround)
                {
                    m_CommonMiningGround[indexRight].HideMiningGround();
                }
            }

            //top
            var indexTopRow = row - 1;
            if (indexTopRow >= 0)
            {
                var indexTop = index - 4;
                m_CommonMiningGround[indexTop].m_GeenMask.Hide();
                if (isHideGround)
                {
                    m_CommonMiningGround[indexTop].HideMiningGround();
                }
            }

            //bottom
            var indexBottomRow = row + 1;
            if (indexBottomRow <= 4)
            {
                var indexBottom = index + 4;
                m_CommonMiningGround[indexBottom].m_GeenMask.Hide();
                if (isHideGround)
                {
                    m_CommonMiningGround[indexBottom].HideMiningGround();
                }
            }
        }

        private void OnShowNineGrid(int eventId, object data)
        {
            var index = (int)data;

            // 将该格子对应的index转化成行列
            var row = index / 4;
            var column = index % 4;

            //center
            m_CommonMiningGround[index].m_BlueMask.Show();

            //left
            var indexLeftColumn = column - 1;
            if (indexLeftColumn >= 0)
            {
                m_CommonMiningGround[index - 1].m_BlueMask.Show();
            }

            //right
            var indexRightColumn = column + 1;
            if (indexRightColumn <= 3)
            {
                m_CommonMiningGround[index + 1].m_BlueMask.Show();
            }
        }

        private void OnHideNineGrid(int eventId, object data)
        {
            var (index, isFadeGround) = (ValueTuple<int, bool>)data;

            // 将该格子对应的index转化成行列
            var row = index / 4;
            var column = index % 4;

            //center
            m_CommonMiningGround[index].m_BlueMask.Hide();
            if (isFadeGround)
            {
                OnFadeNineGrid(m_CommonMiningGround[index].m_Image);
            }

            //left
            var indexLeftColumn = column - 1;
            if (indexLeftColumn >= 0)
            {
                var indexLeft = index - 1;
                m_CommonMiningGround[indexLeft].m_BlueMask.Hide();
                if (isFadeGround)
                {
                    OnFadeNineGrid(m_CommonMiningGround[indexLeft].m_Image);
                }
            }

            //right
            var indexRightColumn = column + 1;
            if (indexRightColumn <= 3)
            {
                var indexRight = index + 1;
                m_CommonMiningGround[indexRight].m_BlueMask.Hide();
                if (isFadeGround)
                {
                    OnFadeNineGrid(m_CommonMiningGround[indexRight].m_Image);
                }
            }
        }

        private void OnFadeNineGrid(Image image)
        {
            DOTween.Sequence()
                .Append(image.DOFade(0, 1f))
                .Append(image.DOFade(1, 1f));
        }

        #endregion
    }
}