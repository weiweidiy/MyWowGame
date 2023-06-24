using BreakInfinity;
using DG.Tweening;
using Logic.Common;
using Logic.Fight.Actor;
using Logic.Fight.Common;
using Logic.Fight.Skill.State;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Logic.Fight.Skill.Implement
{
    public class Attacker_8 : Attacker
    {
        /// <summary>
        /// 雷击对象模板
        /// </summary>
        public GameObject m_LighningTemplate;

        public Animator m_Animator;
        /// <summary>
        /// 动画控制的根节点
        /// </summary>
        public GameObject m_Body;

        /// <summary>
        /// 攻击范围
        /// </summary>
        float m_AttackRange;

        /// <summary>
        /// 落雷延迟
        /// </summary>
        float m_AttackDelay;

        /// <summary>
        /// 造成的伤害
        /// </summary>
        BigDouble m_Damage;

        /// <summary>
        /// 无敌人时的空落点对象
        /// </summary>
        public NullTarget m_tempTarget;

        private static readonly int Launch = Animator.StringToHash("Launch");

        Coroutine m_coAnim = null;

        /// <summary>
        /// 初始化接口
        /// </summary>
        /// <param name="args"></param>
        protected override void OnInitializeParams(params object[] args)
        {
            base.OnInitializeParams(args);

            m_AttackRange = (float)args[0];
            m_Damage = (BigDouble)args[1];
            m_AttackDelay = (float)args[2];
            m_Body.SetActive(false);
            m_Animator.enabled = false;
        }

        /// <summary>
        /// 进入搜索状态
        /// </summary>
        protected override void DoSearching()
        {
            base.DoSearching();

            var target = GetTarget();
            if (target == null)
            {
                var pos = GetNullTarget();
                m_tempTarget.transform.position = new Vector3(pos.x, pos.y, 0);
                SetTarget(m_tempTarget);
            }
        }

        /// <summary>
        /// 进入攻击状态
        /// </summary>
        /// <param name="m_CurrentTarget"></param>
        protected override void DoEmitting(IDamagable m_CurrentTarget)
        {
            base.DoEmitting(m_CurrentTarget);

            m_Body.SetActive(true);
            m_Animator.enabled = true;

            transform.position = new Vector3(m_CurrentTarget.GetPos().x, transform.position.y, 0);
            var anim = GetComponent<Animator>();
            var animHelper = new AnimatorHelper();
            m_coAnim = StartCoroutine(animHelper.CheckAnimationComplete(anim, "Skill_8_bullet", () =>
            {
                transform.SetParent(FightManager.Ins.m_GroundNode);
                //StartCoroutine(WaitForLightDown(m_AttackDelay, gameObject));

                DOTweenDelay(m_AttackDelay, 1, () =>
                {
                    PlayLightningDown(gameObject);
                });
            }));
        }


        protected override void DoEnding()
        {
            base.DoEnding();
            if (m_coAnim != null)
                StopCoroutine(m_coAnim);
        }

        /// <summary>
        /// 获取空目标
        /// </summary>
        /// <returns></returns>
        Vector3 GetNullTarget()
        {
            var target = new Vector3(FightEnemyManager.Ins.MiddlePosX, FightEnemySpawnManager.Ins.m_EnemyGroundSpawnPoss[0].position.y, 0);
            var randomTarget = new Vector3(target.x + UnityEngine.Random.Range(-1f, 2f), target.y, target.z);
            return randomTarget;
        }

        /// <summary>
        /// 获取目标
        /// </summary>
        /// <returns></returns>
        public override IDamagable GetTarget()
        {
            var lst = new List<Enemy>();
            FightEnemyManager.Ins.GetRandomTargets(lst, 1);

            if (lst.Count > 0)
                return lst[0];
            else
                return null;
        }

        ///// <summary>
        ///// 等待闪电雷击下落
        ///// </summary>
        ///// <param name="interval"></param>
        ///// <param name="targets"></param>
        ///// <returns></returns>
        //IEnumerator WaitForLightDown(float interval, GameObject bullet)
        //{
        //    yield return new WaitForSeconds(interval);
        //    PlayLightningDown(bullet);
        //}


        /// <summary>
        /// 下落闪电造成伤害
        /// </summary>
        void PlayLightningDown(GameObject bullet)
        {
            var goMissile = Instantiate(m_LighningTemplate, bullet.transform);
            //找到引雷针的位置，bullet是父节点，没有跟随动画移动，所以要找动画节点
            var hitPos = bullet.transform.Find("Root").position;
            //向下调整偏移位置
            var offset = new Vector3(0, -1f, 0);
            goMissile.transform.position = hitPos + offset;
            var anim = goMissile.GetComponent<Animator>();
            goMissile.SetActive(true);
            anim.SetTrigger(Launch);

            var hitList = new List<Enemy>();
            FightEnemyManager.Ins.GetTargetByRange(hitList, goMissile.transform.position.x - m_AttackRange, goMissile.transform.position.x + m_AttackRange);

            foreach (var pTarget in hitList)
            {
                pTarget.m_Health.Damage(m_Damage);
            }

            //StartCoroutine(WaitForDestroy(1f));

            DOTweenDelay(1f, 1, () =>
            {
                Destroy(gameObject);
            });
        }

        ///// <summary>
        ///// 清理创建的对象
        ///// </summary>
        ///// <param name="interval"></param>
        ///// <returns></returns>
        //IEnumerator WaitForDestroy(float interval)
        //{
        //    yield return new WaitForSeconds(interval);
        //    Destroy(gameObject);
        //}


        public void DOTweenDelay(float delayedTimer, int loopTimes, Action action)
        {
            float timer = 0;
            //DOTwwen.To()中参数：前两个参数是固定写法，第三个是到达的最终值，第四个是渐变过程所用的时间
            Tween t = DOTween.To(() => timer, x => timer = x, 1, delayedTimer).SetUpdate(UpdateType.Manual)
                          .OnStepComplete(() =>
                          {
                              action?.Invoke();
                          })
                          .SetLoops(loopTimes);
        }

    }
}