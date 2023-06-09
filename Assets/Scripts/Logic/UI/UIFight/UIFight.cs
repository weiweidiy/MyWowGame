using System;
using BreakInfinity;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Framework.EventKit;
using Framework.Extension;
using Framework.Helper;
using Framework.UI;
using Logic.Common;
using Logic.Data;
using Logic.Fight;
using Logic.Manager;
using Logic.UI.Cells;
using Logic.UI.Common;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityTimer;

namespace Logic.UI.UIFight
{
    /// <summary>
    /// 切换类型
    /// </summary>
    public enum FightSwitchEvent
    {
        Close = -1, //关闭
        Normal = 0, //普通黑屏切换
        FallBack = 1, //失败 回退
        NormalBoss = 2, //遇到普通关卡Boss
        NextLevel = 3, //打完BOSS下一关
    }

    public class UIFight : UIPage
    {
        #region 关卡,副本 等根节点

        public GameObject m_NormalLevelNode;
        public GameObject m_DiamondCopyNode;
        public GameObject m_CoinCopyNode;
        public GameObject m_OilCopyNode;
        public GameObject m_SkillNode;

        #endregion
        
        
        #region 副本进度
        
        public TextMeshProUGUI m_LevelName;
        public GameObject m_ProcessNode;
        public GameObject m_BtnFightStart;
        public GameObject[] m_ProcessList;

        //普通副本BOSS
        public GameObject m_BossTimeNode;
        public Image m_BossTimeBar;

        #endregion

        #region 切换背景

        public GameObject m_SwitchNode;
        public GameObject m_SwitchSuccess;
        public GameObject m_SwitchFailed;
        public GameObject m_SwitchBoss;
        public GameObject m_SwitchNormal;

        #endregion

        #region 副本

        public GameObject m_CopyTimerNode;
        public TextMeshProUGUI m_CopyTimer;
        public TextMeshProUGUI m_DiamondLevelName;
        public TextMeshProUGUI m_DiamondCopyPro;
        public TextMeshProUGUI m_CoinLevelName;
        public TextMeshProUGUI m_OilLevelName;
        public TextMeshProUGUI m_OilBoxNumber;
        public CommonHpBar m_OilBossHpBar;

        #endregion

        public Toggle m_AutoSkill;

        public CommonFightSkill[] m_CommonFightSkills;
        
        private void Awake()
        {
            m_EventGroup.Register(LogicEvent.Fight_LevelTypeChanged, (i, o) =>
            {
                m_NormalLevelNode.Hide();
                m_DiamondCopyNode.Hide();
                m_CoinCopyNode.Hide();
                m_OilCopyNode.Hide();
                m_OilBossHpBar.Hide();
                switch ((LevelType)o)
                {
                    case LevelType.NormalLevel:
                        m_NormalLevelNode.Show();
                        SetFightSkillConfigState(true); //只有普通副本可以配置技能
                        break;
                    case LevelType.DiamondCopy:
                        SetFightSkillConfigState(false);
                        m_DiamondCopyNode.Show();
                        m_DiamondLevelName.text = $"<color=#F3B736>钻石副本 </color>{CopyManager.Ins.CurSelectedLevel}";
                        m_DiamondCopyPro.text = $"{CopyManager.Ins.m_DiamondCopyCount}/{GameDefine.CopyDiamondCount}";
                        break;
                    case LevelType.CoinCopy:
                        SetFightSkillConfigState(false);
                        m_CoinCopyNode.Show();
                        m_CoinLevelName.text = $"<color=#F3B736>金币副本 </color>{CopyManager.Ins.CurSelectedLevel}";
                        break;
                    case LevelType.OilCopy:
                        SetFightSkillConfigState(false);
                        m_OilCopyNode.Show();
                        m_OilLevelName.text = $"<color=#F3B736>节奏冲锋 </color>";
                        break;
                    case LevelType.TrophyCopy:
                        SetFightSkillConfigState(false);
                        m_DiamondCopyNode.Show();
                        m_DiamondLevelName.text = $"<color=#F3B736>掠食冒险 </color>{CopyManager.Ins.CurSelectedLevel}";
                        m_DiamondCopyPro.text = $"{CopyManager.Ins.m_TrophyCopyCount}/{GameDefine.CopyTrophyCount}";
                        break;
                    case LevelType.ReformCopy:
                        SetFightSkillConfigState(false);
                        m_DiamondCopyNode.Show();
                        m_DiamondLevelName.text = $"<color=#F3B736>改造副本 </color>{CopyManager.Ins.CurSelectedLevel}";
                        m_DiamondCopyPro.text = $"{CopyManager.Ins.m_ReformCopyCount}/{GameDefine.CopyReformCount}";
                        break;
                }
            });
            
            m_EventGroup.Register(LogicEvent.Fight_LevelChanged, (i, o) => { UpdateFight_LevelChanged(); });
            m_EventGroup.Register(LogicEvent.Fight_LevelNodeChanged, (i, o) => { UpdateFight_LevelNodeChanged(); });
            m_EventGroup.Register(LogicEvent.Fight_LevelStateChanged, (i, o) => { UpdateLevelState(); });
            m_EventGroup.Register(LogicEvent.ShowFightSwitch, OnShowFightSwitch);
            m_EventGroup.Register(LogicEvent.Fight_ShowNormalBossTime, ((i, o) =>
            {
                if (o as bool? == true)
                {
                    m_ProcessNode.Hide();
                    m_BossTimeNode.Show();
                    m_BossTimeBar.fillAmount = 1;
                }
                else
                    m_BossTimeNode.Hide();
            }));
            m_EventGroup.Register(LogicEvent.Fight_NormalBossTimerChanged,
                ((i, o) => { m_BossTimeBar.fillAmount = 1 - (float)o / GameDefine.BOSSFightTime; }));
            m_EventGroup.Register(LogicEvent.Fight_ShowCopyBossTime, ((i, o) =>
            {
                if (o as bool? == true)
                {
                    m_ProcessNode.Hide();
                    m_CopyTimerNode.Show();
                }
                else
                    m_CopyTimerNode.Hide();
            }));
            m_EventGroup.Register(LogicEvent.Fight_CopyBossTimerChanged,
                (i, o) => { m_CopyTimer.text = TimeHelper.FormatSecond(CopyManager.Ins.CurCDTime); });
            m_EventGroup.Register(LogicEvent.Fight_CopyDiamondCountChanged, ((i, o) =>
            {
                m_DiamondCopyPro.text = $"{CopyManager.Ins.m_DiamondCopyCount}/{GameDefine.CopyDiamondCount}";
            }));

            //显示原油副本Boss血条
            m_EventGroup.Register(LogicEvent.Fight_ShowOilBossHpBar, ((i, o) =>
            {
                m_OilBossHpBar.transform.gameObject.SetActive(true);
                var hp = CopyManager.Ins.GetCopyOilBossHp().ToDouble();
                m_OilBossHpBar.ResetFill();
                m_OilBoxNumber.text = CopyManager.Ins.CurBossLevel.ToString();
            }));

            m_EventGroup.Register(LogicEvent.Fight_OilBossHpChanged, ((i, o) => {
                var args = o as object[];
                var curHp = (BigDouble)args[0];
                var maxHp = (BigDouble)args[1];
                var fillAmount = (float)(curHp / maxHp).ToDouble();
                m_OilBossHpBar.SetFill(fillAmount);
            }));

            m_EventGroup.Register(LogicEvent.Fight_OilBossLevelChanged, ((i,o) =>{
                var level = (int)o;
                m_OilBoxNumber.text = level.ToString();
                m_OilBossHpBar.ResetFill();
            }));

            m_EventGroup.Register(LogicEvent.Fight_CopyTrophyCountChanged, ((i, o) =>
            {
                m_DiamondCopyPro.text = $"{CopyManager.Ins.m_TrophyCopyCount}/{GameDefine.CopyTrophyCount}";
            }));


            //自动释放技能
            m_AutoSkill.isOn = GameDataManager.Ins.AutoSkill;
            m_AutoSkill.onValueChanged.AddListener(OnClickAuto);
        }

        /// <summary>
        /// 设置技能按钮可配置状态：是否可以打开配置界面
        /// </summary>
        void SetFightSkillConfigState(bool state)
        {
            foreach(var skill in m_CommonFightSkills)
            {
                skill.CanConfig = state;
            }
        }

        public override void OnShow()
        {
            UpdateFight_LevelChanged();
            UpdateFight_LevelNodeChanged();
            UpdateLevelState();

            // if (FightManager.Ins.IsBossLevel())
            // {
            //     OnShowFightSwitch(0, FightSwitchEvent.NormalBoss);
            // }
            // else
            // {
            //     OnShowFightSwitch(0, FightSwitchEvent.Normal);
            // }
        }

        private void UpdateFight_LevelChanged()
        {
            m_LevelName.text = UICommonHelper.GetLevelNameByID(GameDataManager.Ins.CurLevelID);
            LockStoryManager.Ins.DoUnLock(GameDataManager.Ins.CurLevelID); // 更新关卡开放
        }

        private void UpdateFight_LevelNodeChanged()
        {
            foreach (var Obj in m_ProcessList)
            {
                Obj.Hide();
            }

            m_ProcessNode.Show();
            for (int i = 0; i < GameDataManager.Ins.CurLevelNode; i++)
            {
                m_ProcessList[i].Show();
            }
        }

        private void UpdateLevelState()
        {
            if (GameDataManager.Ins.LevelState == LevelState.Normal)
            {
                m_BtnFightStart.Hide();
                if (!FightManager.Ins.IsBossLevel())
                    m_ProcessNode.Show();
            }
            else if (GameDataManager.Ins.LevelState == LevelState.HandUp)
            {
                m_BtnFightStart.Show();
                m_ProcessNode.Hide();
            }
            else
            {
                throw new Exception("!!!!");
            }
        }

        private async void OnShowFightSwitch(int arg1, object arg2)
        {
            var _Type = (FightSwitchEvent)arg2;
            switch (_Type)
            {
                case FightSwitchEvent.Close:
                    await UniTask.DelayFrame(20);
                    m_SwitchNode.Hide();
                    m_SwitchSuccess.Hide();
                    m_SwitchFailed.Hide();
                    m_SwitchBoss.Hide();
                    m_SwitchNormal.Hide();
                    break;
                case FightSwitchEvent.FallBack:
                    //await UniTask.Delay(500);
                    await UniTask.DelayFrame(1);
                    m_SwitchNode.Show();
                    m_SwitchFailed.Show();

                    break;
                case FightSwitchEvent.NormalBoss:
                    //await UniTask.Delay(500); //不知道原先为什么要延迟，所以避免注释引发问题，仍然延迟了1帧
                    await UniTask.DelayFrame(1);
                    m_SwitchNode.Show();
                    m_SwitchBoss.Show();
                    m_ProcessNode.Hide();
                    break;
                case FightSwitchEvent.NextLevel:
                    //await UniTask.Delay(500);
                    await UniTask.DelayFrame(1);
                    m_SwitchNode.Show();
                    m_SwitchSuccess.Show();
                    break;
                case FightSwitchEvent.Normal:
                    m_SwitchNode.Show();
                    m_SwitchNormal.Show();
                    break;
            }
        }


        public void OnClickBtnFightStart()
        {
            GameDataManager.Ins.LevelState = LevelState.Normal;
            
            var _Para = new FightSwitchTo();
            if (FightManager.Ins.IsBossLevel())
                _Para.m_SwitchToType = SwitchToType.ToBoss;
            else
                _Para.m_SwitchToType =  SwitchToType.ToNormalLevel;
            
            EventManager.Call(LogicEvent.Fight_Switch, _Para);
            if (_Para.m_CanSwitchToNextNode == false)
                return;

            UpdateLevelState();
        }

        public void OnClickAuto(bool b)
        {
            GameDataManager.Ins.AutoSkill = b;
        }

        public void HideSkillNode()
        {
            m_SkillNode.Hide();
        }

        public void ShowSkillNode(int sortingOrder = 0)
        {
            m_SkillNode.Show();
            var canvas = m_SkillNode.GetComponent<Canvas>();
            canvas.sortingOrder = sortingOrder;
        }

        public void HideNormalLevelNode()
        {
            m_NormalLevelNode.Hide();
        }

        public void ShowNormalLevelNode()
        {
            m_NormalLevelNode.Show();
        }
    }
}