
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BreakInfinity;
using DG.Tweening;
using Framework.EventKit;
using Framework.Extension;
using Logic.Common;
using Logic.Fight.Actor;
using Logic.Fight.Common;
using Logic.Fight.Skill.Implement.SkillParts;
using Logic.Fight.Skill.State;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Logic.Fight.Skill.Implement
{

    public class Skill_4 : SkillBase
    {
        [Tooltip("技能body动画对象，用于创建多个实例")]
        public Attacker m_Attacker;

        public Transform[] m_SpawnPositions;
        public Transform[] m_IdlePositions;
        

        [Tooltip("释放的导弹个数")]
        public int missileCount;
        [Tooltip("每颗导弹发出间隔")]
        public float spawnInterval;

        //[Tooltip("导弹飞行路径")]
        //public Transform[] path;
        [Tooltip("飞行曲线幅度")]
        public float pathHight;

        [Tooltip("入场飞行速度")]
        public float m_EnterSpeed;
        [Tooltip("攻击飞行速度")]
        public float m_AtkSpeed;

        [Tooltip("索敌延迟")]
        public float m_SearchDelay;

        /// <summary>
        /// 创建动画的协程
        /// </summary>
        Coroutine coCreateSkill = null;
        /// <summary>
        /// 攻击动画协程
        /// </summary>
        Coroutine coSkillAtk = null;

        /// <summary>
        /// 只存储一次技能 攻击者队列
        /// </summary>
        List<IAttacker> m_Attackers = new List<IAttacker>();

        /// <summary>
        /// 所有攻击者对象队列:因为可能出现2轮技能重复的情况，所以用一个队列存储所有的对象
        /// </summary>
        List<IAttacker> m_AllAttackers = new List<IAttacker>();

        bool m_needSearchTarget = false;

        public override void Init(int pSkillId)
        {
            base.Init(pSkillId);

            m_EventGroup.Register(LogicEvent.Fight_Switch, OnFightSwitch);
        }

        /// <summary>
        /// 战斗切换换了
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void OnFightSwitch(int arg1, object arg2)
        {
            foreach (var atker in m_AllAttackers)
            {
                atker.End();
            }
        }



        public override void OnStartSkill()
        {
            base.OnStartSkill();
            CreateAttackers();
        }

        /// <summary>
        /// 创建所有攻击者
        /// </summary>
        void CreateAttackers()
        {
            m_Attackers.Clear();

            for (int i = 0; i < missileCount; i++)
            {
                var pos = m_SpawnPositions[i].position;
                var go = CreateAttacker(pos);
                var attacker = go.GetComponent<Attacker>();
                m_Attackers.Add(attacker);
                m_AllAttackers.Add(attacker);
                var index = m_Attackers.Count - 1;
                attacker.Ready(m_IdlePositions[i], index, Formula.GetGJJAtk() * GetSkillBaseDamage() / 100, m_EnterSpeed, pathHight,m_AtkSpeed);
                
                attacker.onIdle += Attacker_onIdle;
                attacker.onEmitting += Attacker_onEmitting;
                attacker.onEnding += Attacker_onEnding;
            }
        }

        /// <summary>
        /// 攻击者攻击结束了
        /// </summary>
        /// <param name="attacker"></param>
        private void Attacker_onEnding(IAttacker attacker)
        {
            if(!m_AllAttackers.Remove(attacker as Attacker))
            {
                Debug.LogError("找不到要删除的攻击者！");
            }
            Destroy((attacker as Attacker).gameObject);
        }

        /// <summary>
        /// 攻击者进入待机了
        /// </summary>
        /// <param name="attacker"></param>
        private void Attacker_onIdle(IAttacker attacker)
        {
            var atker = attacker as Attacker_4;
            if(atker.Index == 0)
            {
                atker.Search();
            }
        }

        /// <summary>
        /// 攻击者进入了攻击状态
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="obj"></param>
        private void Attacker_onEmitting(IAttacker attacker, State.IDamagable obj)
        {
            var atker = attacker as Attacker_4;
            var index = atker.Index;

            //如果不是最后一个attacker，则获取后一个攻击者，并使其进入索敌状态
            if(index < m_Attackers.Count - 1)
            {
                var nextAttacker = GetAttacker(index + 1);

                StartCoroutine(WaitSearching(nextAttacker));
            }    
        }

        /// <summary>
        /// 根据索引获取攻击者接口
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        IAttacker GetAttacker(int index)
        {
            return m_Attackers.ElementAt(index);
        }

        /// <summary>
        /// 延迟索敌
        /// </summary>
        /// <param name="atker"></param>
        /// <returns></returns>
        IEnumerator WaitSearching(IAttacker atker)
        {
            yield return new WaitForSeconds(m_SearchDelay);
            atker.Search();

        }

        /// <summary>
        /// 创建攻击者
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        GameObject CreateAttacker(Vector3 position)
        {
            var go = Instantiate(m_Attacker.gameObject, transform);
            go.transform.position = position;
            return go;
        }


        //Vector3[] RandomPath(int k)
        //{
        //    var pathPos = new Vector3[6];
        //    pathPos[0] = path[0].position;
        //    pathPos[1] = path[1].position + k * new Vector3(0, pathHight, 0);
        //    pathPos[2] = path[2].position - k * new Vector3(0, pathHight, 0); 
        //    pathPos[3] = path[3].position;
        //    pathPos[4] = path[4].position + k * new Vector3(0, pathHight, 0); 
        //    pathPos[5] = path[5].position - k * new Vector3(0, pathHight, 0); 
        //    return pathPos;
        //}





        public override void OnStopSkill()
        {
            if (coCreateSkill != null)
                StopCoroutine(coCreateSkill);

            if (coSkillAtk != null)
                StopCoroutine(coSkillAtk);
        }

        public override void OnSkillReset()
        {

            OnStopSkill();
            m_SM.ToIdle();
        }
        
        public override bool NeedSearchTarget()
        {
            return m_needSearchTarget;
        }
        

        //public void DoPathMove(Transform missile, Vector3[] pathPos)
        //{
        //    var tweenPath = missile.DOPath(pathPos, flyDuration, PathType.CubicBezier);
        //    tweenPath.onComplete = () =>
        //    {
        //        //m_needSearchTarget = true;
        //        var _hitList = new List<Enemy>();
        //        FightEnemyManager.Ins.GetTargetByDistance(_hitList, EnemyRange.Middle);

        //        if(_hitList.Count == 0)
        //            FightEnemyManager.Ins.GetTargetByDistance(_hitList, EnemyRange.Far);

        //        if (_hitList.Count > 0)
        //        {
        //            //Debug.LogError("找到敌人");
        //            DoAtk(missile, _hitList[0].transform.position);
        //        }
        //        else
        //        {
        //            //DoAtk(missile, nullPosition);
        //        }
        //    };
        //}


        //public void DoAtk(Transform missile, Vector3 targetPos)
        //{
        //    //动态创建一个曲线路径
        //    var path = new Vector3[3];
        //    path[0] = missile.position; //0.8 5.6
        //    path[1] = missile.position + new Vector3(3f, -0.3f, 0);
        //    path[2] = targetPos; //10 5
        //    var tweenPath = missile.DOPath(path, 20f, PathType.CatmullRom).SetSpeedBased();
        //    tweenPath.onComplete = () =>
        //    {
        //        missile.Find("Root").gameObject.SetActive(false);
        //        OnAni_ATK(missile);
        //    };
        //}

        //public void OnAni_ATK(Transform missile)
        //{
        //    var _hitEffect = missile.Find("Skill_3_Down_Hit_1").GetComponent<Skill_3MissilesHit>();
        //    _hitEffect.Show();
        //    //m_HitEffect.PlayAni(new Vector3(enemyTarget.position.x, enemyTarget.position.y - 0.5f, enemyTarget.position.z));
        //    //_hitEffect.PlayAni(hitPosition - new Vector3(0,0.5f,0));
        //    List<Enemy> m_HitList = new List<Enemy>(6);
        //    FightEnemyManager.Ins.GetTargetByRange(m_HitList, -10, 10);
        //    BigDouble _ATK = Formula.GetGJJAtk() * GetSkillBaseDamage()/100;
        //    foreach (var enemy in m_HitList)
        //    {
        //        if(enemy != null && enemy.CanAttack())
        //            enemy.m_Health.Damage(_ATK);
        //    }
        //}

        //public override Enemy GetSkillTarget()
        //{
        //    var lst = new List<Enemy>();
        //    FightEnemyManager.Ins.GetTargetByDistance(lst, EnemyRange.Middle);
        //    if(lst.Count == 0)
        //    {
        //        FightEnemyManager.Ins.GetTargetByDistance(lst, EnemyRange.Far);
        //    }
        //    if (lst.Count == 0)
        //        return null;

        //    return lst[0];
        //}
    }
}