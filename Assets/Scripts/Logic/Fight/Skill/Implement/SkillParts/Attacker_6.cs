using BreakInfinity;
using Framework.Pool;
using Logic.Fight.Actor;
using Logic.Fight.Common;
using Logic.Fight.Skill.State;
using Logic.Fight.Weapon;
using System.Collections;
using UnityEngine;

namespace Logic.Fight.Skill.Implement
{

    public class Attacker_6 : Attacker
    {
        public Animator m_Animator;
        private static readonly int m_Appear = Animator.StringToHash("Appear");
        private static readonly int m_Attack = Animator.StringToHash("Attack");
        
        /// <summary>
        /// 实际的攻击者对象
        /// </summary>
        public Transform attacker;

        /// <summary>
        /// 炮口位置对象
        /// </summary>
        public Transform m_MuzzlePos;
        //子弹预制体
        public GameObject m_BulletPrefab;

        /// <summary>
        /// 伤害
        /// </summary>
        BigDouble m_Damage;

        /// <summary>
        /// 当前的目标
        /// </summary>
        IDamagable m_CurrentTarget;

        /// <summary>
        /// 发射次数，to do: 根据等级获取
        /// </summary>
        int m_emitCount = 0;

        /// <summary>
        /// 当前发射次数
        /// </summary>
        int m_curEmitCount = 0;

        /// <summary>
        /// 发射间隔
        /// </summary>
        float m_spawnInterval = 0f;

        /// <summary>
        /// 开火协程
        /// </summary>
        Coroutine m_coFire = null;

        /// <summary>
        /// 检查动画结束协程
        /// </summary>
        Coroutine m_coCheck = null;

        /// <summary>
        /// 设置初始化参数
        /// </summary>
        /// <param name="args"></param>
        protected override void OnInitializeParams(params object[] args)
        {
            m_emitCount = (int)args[0];
            m_spawnInterval = (float)args[1];
            m_Damage = (BigDouble)args[2];
        }

        /// <summary>
        /// 准备攻击
        /// </summary>
        /// <param name="args"></param>
        public override void Ready(params object[] args)
        {
            base.Ready(args);

            attacker.gameObject.SetActive(true);
            m_CurrentTarget = null;
            m_curEmitCount = 0;
        }

        /// <summary>
        /// 重写，进入结束阶段
        /// </summary>
        public override void End()
        {
            base.End();

            m_curEmitCount = 0;

            if (m_coFire != null)
                StopCoroutine(m_coFire);
        }

        /// <summary>
        /// 获取目标规则
        /// </summary>
        /// <returns></returns>
        public override IDamagable GetTarget()
        {
            return FightEnemyManager.Ins.GetOneTarget();
        }


        #region 进入状态接口
        protected override void DoReady()
        {
            m_Animator.enabled = true;
            m_Animator.SetTrigger(m_Appear);
            m_Animator.SetFloat("Speed", 1.0f);

            m_coCheck = StartCoroutine(CheckAnimation("Skill_6_appear", () =>
            {
                base.DoReady();
            }));
        }
        
        protected override void DoEmitting(IDamagable currentTarget)
        {
            m_CurrentTarget = currentTarget;
            m_Animator.SetTrigger(m_Attack);
            RotateToDir(currentTarget.GetPos());
        }

        protected override void DoEnding()
        {
            m_Animator.SetFloat("Speed", -1.0f);
            m_Animator.Play("Skill_6_appear",0,1f);
            m_coCheck = StartCoroutine(CheckAnimation("Skill_6_appear", () =>
            {
                attacker.gameObject.SetActive(false);
            }, true));
        }

        #endregion

        #region 动画帧事件

        /// <summary>
        /// 动画帧事件，在animation中调用
        /// </summary>
        public void OnAni_Attack()
        {
            var position = m_MuzzlePos.position;
            var _BulletObj = FightObjPool.Ins.Spawn(m_BulletPrefab).GetComponent<NormalBullet>();
            var transform1 = _BulletObj.transform;

            transform1.position = position;

            Vector2 direction = m_CurrentTarget.GetPos() - transform1.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform1.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            _BulletObj.Fire(m_CurrentTarget as ActorBase, m_CurrentTarget.GetPos(), m_Damage);
        }

        /// <summary>
        /// 动画帧事件，在animation中调用
        /// </summary>
        public void OnAni_AttackEnd()
        {
            m_curEmitCount++;
            if (m_curEmitCount < m_emitCount)
            {
                m_coFire = StartCoroutine(WaitFire());
            }
            else
            {
                End();
            }
        }
        #endregion

        /// <summary>
        /// 等待下次开火
        /// </summary>
        /// <returns></returns>
        IEnumerator WaitFire()
        {
            yield return new WaitForSeconds(m_spawnInterval);
            Emit();
        }

        /// <summary>
        /// 检测是否播放完成
        /// </summary>
        /// <param name="playAnimationName"></param>
        /// <param name="action"></param>
        /// <param name="IsBack">是否倒放</param>
        /// <returns></returns>
        private IEnumerator CheckAnimation(string playAnimationName, System.Action action, bool IsBack = false)
        {
            bool condition = true;

            while (condition)
            {
                if (IsBack)
                {
                    condition = !m_Animator.GetCurrentAnimatorStateInfo(0).IsName(playAnimationName)
                    || m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f;
                }
                else
                {
                    condition = !m_Animator.GetCurrentAnimatorStateInfo(0).IsName(playAnimationName)
                    || m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f;
                }

                yield return null;
            }
            action?.Invoke();
        }

        protected void RotateToDir(Vector3 targetPos)
        {
            Vector2 direction = targetPos - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}