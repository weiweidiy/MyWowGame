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
    /// ����������ͨ����
    /// </summary>
    public class EnemyViceGun : MonoBehaviour
    {
        [SerializeField] Enemy enemy;

        [SerializeField] Animator m_BPAnimtor;

        /// <summary>
        /// �����ٶ�
        /// </summary>
        public float m_AttackSpeed;

        [LabelText("BPǹ��λ��")]
        public Transform m_BPMuzzlePos;
        [LabelText("BP�ӵ�Ԥ����")]
        public GameObject m_BPBulletPrefab;
        [LabelText("BPǹ����ЧԤ����")]
        public GameObject m_BPMuzzleEffectPrefab;


        [LabelText("GJJ�ܻ�Ŀ��� 0-3")]
        public int[] m_HitIndexArray;

        /// <summary>
        /// �˺�����(�������ڣ�
        /// </summary>
        public float m_DamageMult = 1f;

        GJJCtrl m_GjjGtrl;

        /// <summary>
        /// �Ƿ���빥��cd
        /// </summary>
        bool IsUnderAttackCD { get; set; }

        /// <summary>
        /// �Ƿ��ǹ���״̬
        /// </summary>
        bool IsAttackState { get; set; }

        private void Awake()
        {
            if(enemy == null)
                enemy = GetComponent<Enemy>();

            Debug.Assert(enemy != null, "ȱ��Enemy�����" + gameObject);
            enemy.onEnterAttack += Enemy_onEnterAttack;

            m_GjjGtrl = GameObject.Find("GJJNode/GJJ_1").transform.GetComponent<GJJCtrl>();
        }

        /// <summary>
        /// ���빥��״̬
        /// </summary>
        private void Enemy_onEnterAttack()
        {
            IsAttackState = true;
            //���Ź�������
            PlayAttackAnim();
            DoAttack();
        }

        public void Update()
        {
            //���CD���������ٴι���
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
        /// �����ӵ�������CD��
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
        /// �����ӵ�
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

