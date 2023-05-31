using Framework.Pool;
using Logic.Fight.Actor;
using Logic.Fight.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 监听特殊攻击动画回调，临时做法，需要重构
/// </summary>
public class OnBPAttackEventListener : MonoBehaviour
{
    public SpecialRangeEnemy enemey;


    public void OnBPAttack()
    {
        

        for(int i = 0; i < enemey.m_BPMuzzlePos.transform.childCount; i ++)
        {
            var position = enemey.m_BPMuzzlePos.transform.GetChild(i).position;
            var _MuzzleObj = FightObjPool.Ins.Spawn(enemey.m_BPMuzzleEffectPrefab);
            _MuzzleObj.transform.position = position;

            if(i == 0)
            {
                var _BulletObj = FightObjPool.Ins.Spawn(enemey.m_BPBulletPrefab).GetComponent<EnemyBullet>();
                var _BulletObj2 = FightObjPool.Ins.Spawn(enemey.m_BPBulletPrefab).GetComponent<EnemyBullet>();
                _BulletObj.transform.position = position;
                _BulletObj2.transform.position = position + new Vector3(0, 0.2f, 0);

                Vector3 _TargetPos = new Vector3(enemey.TargetPosX, position.y, position.z);
                _BulletObj.Fire(_TargetPos, enemey.m_Attack);
                _BulletObj2.Fire(_TargetPos, enemey.m_Attack);
            }
        }

    }
}
