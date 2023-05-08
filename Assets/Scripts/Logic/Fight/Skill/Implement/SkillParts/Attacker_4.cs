using BreakInfinity;
using DG.Tweening;
using Framework.Extension;
using Logic.Common;
using Logic.Fight.Actor;
using Logic.Fight.Common;
using Logic.Fight.Skill.Implement.SkillParts;
using Logic.Fight.Skill.State;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Logic.Fight.Skill.Implement
{
    public class Attacker_4 : Attacker
    {
        public Transform m_body;

        public Skill_3MissilesHit hitEffect;
        /// <summary>
        /// 伴飞坐标点
        /// </summary>
        Transform m_tranIdle;

        /// <summary>
        /// 伴飞队列索引
        /// </summary>
        public int Index { get; private set; }

        
        BigDouble m_Damage;


        SortingGroup[] sortingGroups;
        SortingGroup[] SortingGroups => sortingGroups ?? (sortingGroups = GetComponentsInChildren<SortingGroup>());


        Coroutine m_coAnim = null;


        public override IDamagable GetTarget()
        {
            var lst = new List<Enemy>();
            FightEnemyManager.Ins.GetTargetByDistance(lst, EnemyRange.Middle);
            if (lst.Count == 0)
            {
                FightEnemyManager.Ins.GetTargetByDistance(lst, EnemyRange.Far);
            }
            if (lst.Count == 0)
                return null;

            return lst[0];
        }


        protected override void OnInitializeParams(params object[] args)
        {
            base.OnInitializeParams(args);

            m_tranIdle = args[0] as Transform;
            Index = (int)args[1];
            m_Damage = (BigDouble)args[2];

            transform.localScale = m_tranIdle.localScale;
        }

        /// <summary>
        /// 播放准备动画
        /// </summary>
        protected override void DoReady()
        {
            transform.DOMove(m_tranIdle.position, 1f).onComplete = () => {

                Idle();
            };
        }


        protected override void DoEmitting(IDamagable m_CurrentTarget)
        {
            base.DoEmitting(m_CurrentTarget);

            PlayAtk(m_CurrentTarget.GetPos());
        }


        protected override void DoEnding()
        {
            base.DoEnding();

            if (m_coAnim != null)
                StopCoroutine(m_coAnim);
        }




        /// <summary>
        /// 播放攻击动画
        /// </summary>
        /// <param name="targetPos"></param>
        private void PlayAtk(Vector3 targetPos)
        {
            //动态创建一个曲线路径
            var path = new Vector3[3];
            path[0] = transform.position; //0.8 5.6
            path[1] = transform.position + new Vector3(3f, -0.3f, 0);
            path[2] = targetPos; //10 5
            var tweenPath = transform.DOPath(path, 20f, PathType.CatmullRom).SetSpeedBased();
            tweenPath.onComplete = () =>
            {
                m_body.gameObject.SetActive(false);
                OnHitting(targetPos);
            };
        }

        /// <summary>
        /// 播放命中动画
        /// </summary>
        /// <param name="targetPos"></param>
        private void OnHitting(Vector3 targetPos)
        {
            //播放爆炸特效
            hitEffect.Show();
            hitEffect.PlayAni(targetPos - new Vector3(0,0.5f,0));

            //伤害
            List<Enemy> m_HitList = new List<Enemy>(6);
            FightEnemyManager.Ins.GetTargetByRange(m_HitList, -10, 10);
            foreach (var enemy in m_HitList)
            {
                if (enemy != null && enemy.CanAttack())
                    enemy.m_Health.Damage(m_Damage);
            }

            //监听动画结束
            var animHelper = new AnimatorHelper();
            m_coAnim = StartCoroutine(animHelper.CheckAnimationComplete(hitEffect.m_Animator, "Skill_3_Down", () =>
            {
                //进入结束状态
                End();
            }));
            
        }



        void OnEnable()
        {
            foreach (SortingGroup sortingGroup in SortingGroups)
                sortingGroup.enabled = true;
        }

        void OnDisable()
        {
            foreach (SortingGroup sortingGroup in SortingGroups)
                sortingGroup.enabled = false;
        }
    }
}