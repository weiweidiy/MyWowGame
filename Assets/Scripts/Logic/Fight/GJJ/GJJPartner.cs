using System;
using BreakInfinity;
using Configs;
using Framework.Pool;
using Logic.Common;
using Logic.Fight.Actor;
using Logic.Fight.Common;
using Logic.Fight.Weapon;
using Logic.Manager;
using Networks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityTimer;

namespace Logic.Fight.GJJ
{
    /// <summary>
    /// GJJ 伙伴
    /// </summary>
    public class GJJPartner : MonoBehaviour
    {
        [LabelText("伙伴位置索引")] 
        public int m_Index;
        [LabelText("攻击阈值"), Tooltip("怪物与玩家的距离小于等于这个值时会自动释放技能")]
        public float m_AttackThreshold = 4.5f;
        [ReadOnly]
        public int m_PartnerId;
        //枪口位置
        public Transform m_MuzzlePos;
        //子弹预制体
        public GameObject m_BulletPrefab;
        //枪口特效预制体
        public GameObject m_MuzzleEffectPrefab;
        
        private PartnerData m_PartnerData;
        private GamePartnerData m_GamePartnerData;
        
        //当前目标
        [NonSerialized] public Enemy m_CurrentTarget;

        private Animator m_Animator;
        private Timer m_AttackTimer;
        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            m_PartnerId = PartnerManager.Ins.PartnerOnList[m_Index];
            
            m_PartnerData = PartnerCfg.GetData(m_PartnerId);
            m_GamePartnerData = PartnerManager.Ins.GetPartnerData(m_PartnerId);

            m_Animator.enabled = true;
        }

        private void OnDisable()
        {
            m_AttackTimer?.Cancel();
            m_Animator.enabled = false;
        }

        public void StartStandby()
        {
            m_CurrentTarget = null;
            m_Animator.SetTrigger(AniTrigger.ToIdle);
            m_AttackTimer?.Cancel();
        }
        
        public void StartAttack()
        {
            //m_Animator.SetTrigger(AniTrigger.ToAtk);
            m_CurrentTarget = null;
            m_AttackTimer = Timer.Register(1f / m_PartnerData.AtkSpeed, () =>
            {
                if(!HasOneTarget())
                    return;
                m_Animator.SetTrigger(AniTrigger.ToAtk);
            }, null, true);
        }
        
        //当前目标的坐标信息(坐标会随机变化 以第一次获取目标后的坐标为准)
        private Vector3 m_TargetPos;
        //尝试获取一个目标
        protected bool HasOneTarget()
        {
            if (IsInvalidTarget())
                m_CurrentTarget = FightEnemyManager.Ins.GetOneTarget(m_AttackThreshold);
            if (IsInvalidTarget())
                return false;
            m_TargetPos = m_CurrentTarget.GetPos();
            return true;
        }
        
        protected bool IsInvalidTarget()
        {
            if (m_CurrentTarget != null && !m_CurrentTarget.IsDead())
                return false;
            return true;
        }
        
        protected void RotateToDir(Vector3 dir, GameObject pObj)
        {
            Vector2 direction = dir - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            pObj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        
        private BigDouble GetPartnerATK()
        {
            return (m_PartnerData.AtkBase + (m_GamePartnerData.m_Level - 1) * m_PartnerData.AtkGrow) * Formula.GetGJJAtk();
        }
        
        #region 动画

        //攻击动画开始 回调
        public virtual void OnAni_AttackStart()
        {

        }
        
        //攻击动画触发 回调
        public virtual void OnAni_Attack()
        {
            var _MuzzleObj = FightObjPool.Ins.Spawn(m_MuzzleEffectPrefab);
            var position = m_MuzzlePos.position;
            //var rotation = m_MuzzlePos.rotation;
            
            _MuzzleObj.transform.position = position;
            RotateToDir(m_TargetPos, _MuzzleObj);
            //_MuzzleObj.transform.rotation = rotation;

            var _BulletObj = FightObjPool.Ins.Spawn(m_BulletPrefab).GetComponent<NormalBullet>();

            var transform1 = _BulletObj.transform;
            transform1.position = position;
            RotateToDir(m_TargetPos, _BulletObj.gameObject);
            //transform1.rotation = rotation;
            
            _BulletObj.Fire(m_CurrentTarget, m_TargetPos, GetPartnerATK());
        }
        
        //攻击动画结束 回调
        public virtual  void OnAni_AttackEnd()
        {
            
        }

        #endregion
    }
}