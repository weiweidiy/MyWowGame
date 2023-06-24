
using BreakInfinity;
using DG.Tweening;
using Framework.Extension;
using Logic.Common;
using Logic.Data;
using Logic.Fight.Actor;
using Logic.Fight.Common;
using Logic.Fight.Skill.State;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Logic.Fight.Skill.Implement
{
    public class Skill_8 : SkillBase
    {
        /// <summary>
        /// 发射器动画控制器
        /// </summary>
        public Animator m_Animator;

        /// <summary>
        /// 发射器对象
        /// </summary>
        public GameObject m_SkillObj;

        /// <summary>
        /// 引雷针模板
        /// </summary>
        public GameObject m_LightBulletTemplate;

        [Tooltip("发射到落针的延迟")]
        public float m_LaunchDelay ;

        [Tooltip("攻击延迟")]
        public float m_AttackDelay;


        [Tooltip("雷击攻击范围")]
        public float m_AttackRange = 1f;

        private static readonly int Launch = Animator.StringToHash("Launch");

        Coroutine m_co;

        List<Attacker> attackers = new List<Attacker>();

        /// <summary>
        /// 固定2个
        /// </summary>
        int m_LightBulletCount = 2;

        public override void Init(int pSkillId)
        {
            base.Init(pSkillId);
            m_Animator.enabled = false;
        }

        private bool m_NeedSearch = false;

        public override void OnStartSkill()
        {
            base.OnStartSkill();

            m_NeedSearch = false;

            PlayLaunch();
        }


        public override void OnStopSkill()
        {
            m_NeedSearch = false;
            m_Animator.ResetTrigger(Launch);
            m_Animator.enabled = false;
            m_SkillObj.Hide();
            if (m_co != null)
                StopCoroutine(m_co);

            foreach(var attack in attackers)
            {
                if(attack != null)
                    Destroy(attack.gameObject);
            }

            attackers.Clear();
        }

        public override bool NeedSearchTarget()
        {
            return m_NeedSearch;
        }

        public override void OnSkillReset()
        {
            OnStopSkill();
        }


        /// <summary>
        /// 发射动画
        /// </summary>
        void PlayLaunch()
        {
            m_SkillObj.Show();
            m_Animator.enabled = true;
            m_Animator.SetTrigger(Launch);

            //StartCoroutine(WaitForBulletsDown(m_LaunchDelay));

            DOTweenDelay(m_LaunchDelay, 1, ()=> {
                //StartCoroutine(PlayBulletsDown(0.3f));
                var interval = 0.3f;
                int i = 0;
                DOTweenDelay(interval, m_LightBulletCount, () =>
                {
                    var goBullet = Instantiate(m_LightBulletTemplate);
                    goBullet.transform.position = new Vector3(0, m_LightBulletTemplate.transform.position.y, 0);
                    goBullet.SetActive(true);
                    var attacker = goBullet.GetComponent<Attacker>();
                    attacker.Ready(m_AttackRange, Formula.GetGJJAtk() * GetSkillBaseDamage() / 100, m_AttackDelay);
                    attackers.Add(attacker);
                    i++;
                });
            });
        }

        ///// <summary>
        ///// 等待引雷针下落
        ///// </summary>
        ///// <param name="interval"></param>
        ///// <param name="targets"></param>
        ///// <returns></returns>
        //IEnumerator WaitForBulletsDown(float interval)
        //{
        //    yield return new WaitForSeconds(interval);
        //    StartCoroutine(PlayBulletsDown(0.3f));
        //}

        public void DOTweenDelay(float delayedTimer, int loopTimes , Action action)
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

        ///// <summary>
        ///// 创建attacker，让其自行搜索目标
        ///// </summary>
        //IEnumerator PlayBulletsDown(float interval)
        //{
        //    for (int i = 0; i < m_LightBulletCount; i++)
        //    {
        //        yield return new WaitForSeconds(interval);

        //        var goBullet = Instantiate(m_LightBulletTemplate);
        //        goBullet.transform.position = new Vector3(0, m_LightBulletTemplate.transform.position.y, 0);
        //        goBullet.SetActive(true);
        //        var attacker = goBullet.GetComponent<Attacker>();
        //        attacker.Ready(m_AttackRange, Formula.GetGJJAtk() * GetSkillBaseDamage() / 100, m_AttackDelay);
        //    }
        //}




        /// <summary>
        /// 投射器播放完成回调
        /// </summary>
        public void OnAniEnd()
        {
            m_NeedSearch = true;

            m_Animator.ResetTrigger(Launch);
            m_Animator.enabled = false;
            m_SkillObj.Hide();
        }


    }
}
