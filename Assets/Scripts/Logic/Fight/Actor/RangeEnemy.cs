using Framework.Pool;
using Logic.Fight.GJJ;
using Logic.Fight.Weapon;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Logic.Fight.Actor
{
    /// <summary>
    /// 远程怪物实现
    /// </summary>
    public class RangeEnemy : Enemy
    {
        [LabelText("枪口位置")]
        public Transform m_MuzzlePos;
        [LabelText("子弹预制体")]
        public GameObject m_BulletPrefab;
        [LabelText("枪口特效预制体")]
        public GameObject m_MuzzleEffectPrefab;

        [LabelText("GJJ受击目标点 0-3")]
        public int[] m_HitIndexArray;

        GJJCtrl m_GjjGtrl;

        protected Transform m_HitTarget;

        protected override void Awake()
        {
            base.Awake();

            m_GjjGtrl = GameObject.Find("GJJNode/GJJ_1").transform.GetComponent<GJJCtrl>();

            if (m_HitIndexArray == null || m_HitIndexArray.Length == 0)
            {
                m_HitTarget = m_GjjGtrl.transform;
                //m_HitTarget  = new Vector3(TargetPosX, m_MuzzlePos.position.y, m_MuzzlePos.position.z);
            }
            else
            {
                var randomIndex = UnityEngine.Random.Range(0, m_HitIndexArray.Length);
                var index = m_HitIndexArray[randomIndex];
                m_HitTarget = m_GjjGtrl.GetHitteePosition(index);
            }

        }


        public override void OnAni_Attack()
        {
            var _MuzzleObj = FightObjPool.Ins.Spawn(m_MuzzleEffectPrefab);
            var position = m_MuzzlePos.position;
            _MuzzleObj.transform.position = position;

            var _BulletObj = FightObjPool.Ins.Spawn(m_BulletPrefab).GetComponent<EnemyBullet>();
            _BulletObj.transform.position = position;


            var randomX = UnityEngine.Random.Range(-0.2f, 0.2f);
            var randomY = UnityEngine.Random.Range(-0.2f, 0.2f);

            RotateToDir(_BulletObj.gameObject, m_HitTarget.position);
            //var pos = new Vector3(m_HitTarget.position.x + randomX, m_HitTarget.position.y + randomY, m_HitTarget.position.z);
            _BulletObj.Fire(m_HitTarget, m_Attack);


        }

        protected void RotateToDir(GameObject target,  Vector3 targetPos)
        {
            Vector2 direction = targetPos - target.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            target.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

    }
}