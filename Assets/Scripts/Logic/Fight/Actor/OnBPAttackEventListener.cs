using Framework.Pool;
using Logic.Fight.Actor;
using Logic.Fight.GJJ;
using Logic.Fight.Weapon;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 监听特殊攻击动画回调，临时做法，需要重构
/// </summary>
public class OnBPAttackEventListener : MonoBehaviour
{
    //public SpecialRangeEnemy enemey;

    //[LabelText("GJJ受击目标点 0-3")]
    //public int[] m_HitIndexArray;

    //GJJCtrl m_GjjGtrl;

    //protected Transform m_HitTarget;

    //protected void Awake()
    //{


    //    m_GjjGtrl = GameObject.Find("GJJNode/GJJ_1").transform.GetComponent<GJJCtrl>();

    //}

    //public void OnBPAttack()
    //{
        
    //    for(int i = 0; i < enemey.m_BPMuzzlePos.transform.childCount; i ++)
    //    {
    //        var position = enemey.m_BPMuzzlePos.transform.GetChild(i).position;
    //        var _MuzzleObj = FightObjPool.Ins.Spawn(enemey.m_BPMuzzleEffectPrefab);
    //        _MuzzleObj.transform.position = position;

    //        if(i == 0)
    //        {

    //            if (m_HitIndexArray == null || m_HitIndexArray.Length == 0)
    //            {
    //                m_HitTarget = m_GjjGtrl.transform;
    //            }
    //            else
    //            {
    //                var randomIndex = UnityEngine.Random.Range(0, m_HitIndexArray.Length);
    //                var index = m_HitIndexArray[randomIndex];
    //                m_HitTarget = m_GjjGtrl.GetHitteePosition(index);
    //            }

    //            var _BulletObj = FightObjPool.Ins.Spawn(enemey.m_BPBulletPrefab).GetComponent<EnemyBullet>();
    //            var _BulletObj2 = FightObjPool.Ins.Spawn(enemey.m_BPBulletPrefab).GetComponent<EnemyBullet>();
    //            _BulletObj.transform.position = position;
    //            _BulletObj2.transform.position = position + new Vector3(0, 0.2f, 0);

    //            //Vector3 _TargetPos = new Vector3(enemey.TargetPosX, position.y, position.z);

    //            _BulletObj.Fire(m_HitTarget, enemey.m_Attack);
    //            _BulletObj2.Fire(m_HitTarget, enemey.m_Attack);
    //        }
    //    }

    //}
}
