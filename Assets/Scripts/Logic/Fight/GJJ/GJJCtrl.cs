using System;
using BreakInfinity;
using DG.Tweening;
using Framework.EventKit;
using Framework.Extension;
using Logic.Common;
using Logic.Fight.Actor;
using Logic.Fight.Common;
using Logic.Manager;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityTimer;

namespace Logic.Fight.GJJ
{
    /// <summary>
    /// 控制GJJ
    /// </summary>
    [RequireComponent(typeof(ActorHealth))]
    public class GJJCtrl : MonoWithEvent
    {
        //初始X坐标位置
        private float StartPosX;
        [LabelText("移动目标点")] public float MoveToPosX;
        [LabelText("胜利移动目标点")] public float WinMoveToPosX = 6f;
        [LabelText("向前移动时间")] public float MoveToTime = 1;
        [LabelText("回退移动时间")] public float MoveBackTime = 0.8f;
        [Space(10)] [LabelText("血条控制")] public HPBarCtrl m_HPCtrl;

        [LabelText("兽人伙伴挂载点")] public GJJPartner[] m_Partners;
        [LabelText("GJJ动画组件")] public Animator m_GJJAnimator;

        [NonSerialized] public ActorHealth m_Health;

        //控制满血时血条显示
        private bool m_IsShowHPBar;

        //臂炮 普通攻击控制
        public NormalGunCtrl m_NormalGunCtrl;

        //主炮
        public Animator m_MainGunAnimator;

        private void Awake()
        {
            StartPosX = transform.localPosition.x;
            m_Health = GetComponent<ActorHealth>();

            m_Health.Init(Formula.GetGJJHP());
            m_Health.OnDeath.AddListener(OnGJJDead);
            m_Health.OnHealthChange.AddListener(OnHpChange);
            m_Health.OnHurt.AddListener(pDamageData =>
            {
                FightDamageManager.Ins.ShowDamage(m_HPCtrl != null ? m_HPCtrl.transform : transform, pDamageData);
            });

            m_EventGroup.Register(LogicEvent.Fight_Standby, OnStandBy)
                .Register(LogicEvent.Fight_Fighting, (i, o) => ToFighting())
                .Register(LogicEvent.Fight_Over, (i, o) => ToStandby()).Register(LogicEvent.RoomUpgraded, OnRoomUpgrade)
                .Register(LogicEvent.Fight_Win, OnFightWinMove)
                .Register(LogicEvent.PartnerOn, (i, o) => RefreshPartner())
                .Register(LogicEvent.PartnerOff, (i, o) => RefreshPartner());

            RefreshPartner();

            //定时器
            Timer.Register(1, RecoverHP, null, true, false, this);
        }

        /// <summary>
        /// GJJ 是否死亡
        /// </summary>
        /// <returns>true:死亡</returns>
        public bool IsDead()
        {
            return m_Health.IsDead;
        }

        public void OnHpChange(BigDouble pCurHP)
        {
            if (pCurHP >= m_Health.MaxHP)
            {
                if (!m_IsShowHPBar)
                {
                    m_IsShowHPBar = true;
                    Timer.Register(1.5f, () => m_HPCtrl.Hide());
                }
            }
            else
            {
                m_HPCtrl.Show();
            }

            m_HPCtrl.SetHP((float)(pCurHP / m_Health.MaxHP).ToDouble());
        }

        public void OnGJJDead()
        {
            //GJJ 死亡
        }

        //复活GJJ 回复满血量
        public void RecoverMaxHP()
        {
            m_IsShowHPBar = false;
            m_Health.Recover(Formula.GetGJJHP());
        }

        public void RecoverHP()
        {
            if (!IsDead())
            {
                m_Health.Recover(Formula.GetGJJHPRecover());
            }
        }

        private void RefreshPartner()
        {
            for (int i = 0; i < PartnerManager.Ins.PartnerOnList.Count; i++)
            {
                if (PartnerManager.Ins.PartnerOnList[i] == 0)
                    m_Partners[i].Hide();
                else
                    m_Partners[i].Show();
            }
        }

        private static readonly int Attack = Animator.StringToHash("Attack");

        public void PlayMainGunAni()
        {
            m_MainGunAnimator.SetTrigger(Attack);
        }

        #region 事件处理

        private Sequence _Sequence;
        private Tweener _Tweener;

        private void OnStandBy(int arg1, object arg2)
        {
            //移动
            _Sequence = DOTween.Sequence();
            m_GJJAnimator.SetTrigger(AniTrigger.ToMove);
            _Sequence.Append(transform.DOLocalMoveX(MoveToPosX, MoveToTime).SetEase(Ease.InSine)); //向前走
            _Sequence.AppendCallback(() => { m_GJJAnimator.SetTrigger(AniTrigger.ToIdle); });
            _Sequence.Append(transform.DOLocalMoveX(StartPosX, MoveBackTime).SetEase(Ease.OutSine)); //回退
            _Sequence.onComplete += () =>
            {
                _Sequence = null;
                //动画播放完成 切换到战斗状态
                EventManager.Call(LogicEvent.Fight_MapStop);
                EventManager.Call(LogicEvent.Fight_Start);
            };

            EventManager.Call(LogicEvent.Fight_MapMove); //场景背景移动
        }

        /// <summary>
        /// 切换状态
        /// </summary>
        public void OnSwitching()
        {
            _Sequence?.Kill();
            _Tweener?.Kill();
            transform.LocalPositionX(StartPosX);
        }

        public void OnFightWinMove(int arg1, object arg2)
        {
            _Tweener = transform.DOLocalMoveX(WinMoveToPosX, 4).SetEase(Ease.InSine);
        }

        private void OnRoomUpgrade(int arg1, object arg2)
        {
            var _RoomType = (RoomType)arg2;
            switch (_RoomType)
            {
                case RoomType.ATK:
                    break;
                case RoomType.HP:
                {
                    var _Max = Formula.GetGJJHP();
                    var _Change = _Max - m_Health.MaxHP;
                    m_Health.MaxHP = _Max;
                    if (_Change > 0.001f)
                    {
                        m_Health.Recover(_Change);
                    }

                    m_HPCtrl.SetHP((float)(m_Health.HP / m_Health.MaxHP).ToDouble());
                }
                    break;
                case RoomType.HPRecover:
                    break;
                case RoomType.Critical:
                    break;
                case RoomType.CriticalDamage:
                    break;
                case RoomType.Speed:
                    break;
                case RoomType.DoubleHit:
                    break;
                case RoomType.TripletHit:
                    break;
            }
        }

        /// <summary>
        /// 普通攻击 伙伴 切换到攻击状态
        /// </summary>
        private void ToFighting()
        {
            m_NormalGunCtrl.StartAttack();
            foreach (var partner in m_Partners)
            {
                if (partner.gameObject.activeSelf)
                    partner.StartAttack();
            }
        }

        /// <summary>
        /// 切换到待命状态
        /// </summary>
        private void ToStandby()
        {
            m_IsShowHPBar = false;
            m_NormalGunCtrl.StartStandby();
            foreach (var partner in m_Partners)
            {
                if (partner.gameObject.activeSelf)
                    partner.StartStandby();
            }
        }

        #endregion
    }
}