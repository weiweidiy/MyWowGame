using Framework.Pool;
using Logic.Fight.Weapon;
using Sirenix.OdinInspector;
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
        
        public override void OnAni_Attack()
        {
            var _MuzzleObj = FightObjPool.Ins.Spawn(m_MuzzleEffectPrefab);
            var position = m_MuzzlePos.position;
            _MuzzleObj.transform.position = position;

            var _BulletObj = FightObjPool.Ins.Spawn(m_BulletPrefab).GetComponent<EnemyBullet>();
            _BulletObj.transform.position = position;

            Vector3 _TargetPos = new Vector3(TargetPosX, position.y, position.z);
            _BulletObj.Fire(_TargetPos, m_Attack);
        }
    }
}