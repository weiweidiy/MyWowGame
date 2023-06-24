using Framework.Pool;
using Logic.Common;
using Logic.Fight.Actor;
using Logic.Fight.GJJ;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Logic.Fight.Weapon
{
    /// <summary>
    /// 负责额外的普通攻击
    /// </summary>
    public class EnemyViceGun : MonoBehaviour
    {
        [SerializeField] Enemy enemy;

        [SerializeField] Animator m_BPAnimtor;

        /// <summary>
        /// 攻击速度
        /// </summary>
        public float m_AttackSpeed;

        [LabelText("BP枪口位置")]
        public Transform m_BPMuzzlePos;
        [LabelText("BP子弹预制体")]
        public GameObject m_BPBulletPrefab;
        [LabelText("BP枪口特效预制体")]
        public GameObject m_BPMuzzleEffectPrefab;


        [LabelText("GJJ受击目标点 0-3")]
        public int[] m_HitIndexArray;

        /// <summary>
        /// 伤害倍率(对于主炮）
        /// </summary>
        public float m_DamageMult = 1f;

        GJJCtrl m_GjjGtrl;

        /// <summary>
        /// 是否进入攻击cd
        /// </summary>
        bool IsUnderAttackCD { get; set; }

        /// <summary>
        /// 是否是攻击状态
        /// </summary>
        bool IsAttackState { get; set; }

        private void Awake()
        {
            if(enemy == null)
                enemy = GetComponent<Enemy>();

            Debug.Assert(enemy != null, "缺少Enemy组件！" + gameObject);
            enemy.onEnterAttack += Enemy_onEnterAttack;

            m_GjjGtrl = GameObject.Find("GJJNode/GJJ_1").transform.GetComponent<GJJCtrl>();
        }

        /// <summary>
        /// 进入攻击状态
        /// </summary>
        private void Enemy_onEnterAttack()
        {
            IsAttackState = true;
            //播放攻击动画
            PlayAttackAnim();
            DoAttack();
        }

        public void Update()
        {
            //如果CD结束，则再次攻击
            if (!IsUnderAttackCD && IsAttackState)
            {
                DoAttack();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void PlayAttackAnim()
        {
            m_BPAnimtor.SetTrigger(AniTrigger.ToAtk);
        }

        /// <summary>
        /// 发射子弹，进入CD中
        /// </summary>
        private void DoAttack()
        {
            Fire();

            IsUnderAttackCD = true;
            DotweenManager.Ins.DOTweenDelay(1 / m_AttackSpeed, 1, () =>
            {
                IsUnderAttackCD = false;
            });

        }

        /// <summary>
        /// 发射子弹
        /// </summary>
        public void Fire()
        {
            var position = m_BPMuzzlePos.position;
            var _MuzzleObj = FightObjPool.Ins.Spawn(m_BPMuzzleEffectPrefab);
            _MuzzleObj.transform.position = position;

            Transform m_HitTarget = null;
            if (m_HitIndexArray == null || m_HitIndexArray.Length == 0)
            {
                m_HitTarget = m_GjjGtrl.transform;
            }
            else
            {
                var randomIndex = UnityEngine.Random.Range(0, m_HitIndexArray.Length);
                var index = m_HitIndexArray[randomIndex];
                m_HitTarget = m_GjjGtrl.GetHitteePosition(index);
            }


            var _BulletObj = FightObjPool.Ins.Spawn(m_BPBulletPrefab).GetComponent<EnemyBullet>();
            _BulletObj.transform.position = position;
            _BulletObj.Fire(m_HitTarget, enemy.m_Attack * m_DamageMult);

        }
    }
}

