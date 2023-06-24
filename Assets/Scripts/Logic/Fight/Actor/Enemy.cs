using System;
using BreakInfinity;
using Framework.EventKit;
using Framework.Extension;
using Framework.Pool;
using Logic.Common;
using Logic.Data;
using Logic.Fight.Actor.State;
using Logic.Fight.Common;
using Logic.Fight.Data;
using Logic.Fight.GJJ;
using Logic.Fight.Skill.State;
using Logic.Manager;
using Logic.UI.Common.Effect;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Logic.Fight.Actor
{
    /// <summary>
    /// 怪物基类
    /// </summary>
    public class Enemy : ActorBase, IPoolAssets, IDamagable , IEventNotifier
    {
        public event Action onEnterAttack;
        public enum EnemyType
        {
            Normal, //普通小怪
            Elite,
            Boss
        }

        public enum PositionType
        {
            Ground = 1<<0, //地面
            Sky = 1 << 1, //空中
        }

        [LabelText("血条控制")] [ChildGameObjectsOnly]
        public HPBarCtrl m_HPCtrl;

        [NonSerialized] public ActorBase m_Target;

        [LabelText("移动速度")] public float MoveSpeed = 1f;
        [NonSerialized] public float m_MoveSpeed = 1.0f;

        public float TargetPosX;
        public float AttackRange = 2;

        [ReadOnly, LabelText("攻击力")] public BigDouble m_Attack;
        [ReadOnly, LabelText("掉落金币")] public BigDouble m_DropCoin;

        /// <summary>
        /// 空中单位或者地面单位
        /// </summary>
        public PositionType positionType;

        //怪物实例ID
        public long m_EnemyInsID { get; private set; }

        //状态机
        protected EnemySM m_EnemySM;

        /// <summary>
        /// 受击点
        /// </summary>
        public Transform[] m_HitteePoints;

        //事件处理
        protected readonly EventGroup m_EventGroup = new();

        

        protected override void Awake()
        {
            base.Awake();

            m_Health.OnHealthChange.AddListener(OnHpChange);
            m_Health.OnHurt.AddListener(OnHurt);
            m_EnemySM = new EnemySM();
        }

        protected void Update()
        {
            m_EnemySM.Update();
        }

        private void OnDestroy()
        {
            m_EventGroup.Release();
        }

        public void Init(long pInsID, BigDouble pMaxHP, BigDouble pAttack, BigDouble pDropCoin,float attackSpeed,
            float pMoveSpeedMult = 1f)
        {
            m_EnemyInsID = pInsID;
            if (m_Animator.enabled == false)
                m_Animator.enabled = true;

            var _Data = new EnemyStateData
            {
                m_SM = m_EnemySM,
                m_Owner = this,
                m_TimeLine = m_TimeLine
            };
            m_Health.Init(pMaxHP);
            m_Attack = pAttack;
            m_DropCoin = pDropCoin;
            m_MoveSpeed = MoveSpeed * pMoveSpeedMult;
            m_AttackSpeed = attackSpeed;
            m_EnemySM.Start(_Data, _Data.m_Move);
        }

        //死亡事件
        protected override void OnDead()
        {
            TaskManager.Ins.DoTaskUpdate(TaskType.TT_7001);

            m_HPCtrl.Hide();
            FightEnemyManager.Ins.RemoveEnemy(m_EnemyInsID);

            //死亡掉钱
            if (m_DropCoin > 0.01)
            {
                // GameDataManager.Ins.Coin += m_DropCoin;
                CoinEffectMgr.Ins.StartEffect(transform.position, m_DropCoin);
            }

            m_EnemySM.ToDead();
        }

        //立即销毁并回收
        public void ImmediatelyRecycle()
        {
            m_HPCtrl.Hide();
            //FightEnemyManager.Ins.RemoveEnemy(m_EnemyInsID);

            m_EnemySM.ToRecycle();
        }

        public virtual void OnHpChange(BigDouble pCurHP)
        {
            m_HPCtrl.Show();
            m_HPCtrl.SetHP((float)(pCurHP / m_Health.MaxHP).ToDouble());
        }

        //获取目标(GJJ)
        public GJJCtrl GetTargetGJJ()
        {
            return FightManager.Ins.GetGJJTarget();
        }

        private void OnHurt(FightDamageData pDamageData, Transform target)
        {
            if (FightManager.Ins.IsGJJDead())
                return;

            FightDamageManager.Ins.ShowDamage(target != null ? target : m_HPCtrl.transform, pDamageData, true);
            //FightDamageManager.Ins.ShowDamage(m_HPCtrl != null ? m_HPCtrl.transform : transform, pDamageData, true);

            onEventRaise?.Invoke();
        }

        //是否到达攻击位置
        public bool ArrivedAt()
        {
            if (transform.position.x <= TargetPosX + AttackRange)
                return true;
            return false;
        }

        public PositionType GetPositionType()
        {
            return positionType;
        }

        /// <summary>
        /// 获取指定的受击点坐标
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Transform GetHitteePosition(int index)
        {
            if (m_HitteePoints == null || m_HitteePoints.Length == 0)
                return null;

            return m_HitteePoints[index];
        }

        public Transform GetRandomHitteePosition()
        {
            if (m_HitteePoints == null || m_HitteePoints.Length == 0)
                return null;

            return m_HitteePoints[UnityEngine.Random.Range(0, m_HitteePoints.Length)];
        }

        public Transform GetTransform()
        {
            return transform;
        }

        #region 动画状态机事件回调 处理攻击逻辑

        //攻击动画开始 回调
        public virtual void OnAni_AttackStart()
        {
            
        }

        //攻击动画触发 回调
        public virtual void OnAni_Attack()
        {
        }

        //攻击动画结束 回调
        public virtual void OnAni_AttackEnd()
        {
        }

        public Action OnAni_DeadCB;

        public event Action onEventRaise;

        //死亡动画结束 回调
        public virtual void OnAni_Dead()
        {
            OnAni_DeadCB?.Invoke();
        }

        #endregion

        #region IPoolAssets

        public string PoolObjName { get; set; }

        public void OnSpawn()
        {
            m_HPCtrl.Hide();
        }

        public void OnRecycle()
        {
        }

        public override void OnEnterAttack()
        {
            base.OnEnterAttack();

            onEnterAttack?.Invoke();
        }



        #endregion
    }
}