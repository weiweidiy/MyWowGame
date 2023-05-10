using Framework.Extension;
using Logic.Fight.Common;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Logic.Fight.Skill.Implement.SkillParts
{
    public class Skill_3Missiles : MonoBehaviour
    {
        public Skill_3 m_Owner;
        [LabelText("攻击范围")]
        public EnemyRange pRange;
        [LabelText("序列号")]
        public int pIndex;

        public Animator m_Hit;
        public Transform m_HitPos;

        public Skill_3MissilesHit m_HitEffect;

        Animator m_MissleAnimator;

        private void Awake()
        {
            m_MissleAnimator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            var animHelper = new AnimatorHelper();
            StartCoroutine(animHelper.CheckAnimationComplete(m_MissleAnimator, "Skill_3_Down", () =>
            {
                var _goHit =Instantiate(m_Hit.gameObject, transform);
                _goHit.transform.position = transform.position + new Vector3(0,-1f,0);
                _goHit.Show();

                //检查爆炸动画
                var _hitAnimator = _goHit.GetComponent<Animator>();
                StartCoroutine(animHelper.CheckAnimationComplete(_hitAnimator, "Skill_3_Down", () =>
                {
                    Destroy(_goHit);
                }));

            }));
        }

        ///// <summary>
        ///// 下落动画的帧事件
        ///// </summary>
        //public void OnMissileATK()
        //{
        //    m_HitEffect.Show();
        //    m_HitEffect.PlayAni(m_HitPos.position);

        //    //var animHelper = new AnimatorHelper();
        //    //StartCoroutine(animHelper.CheckAnimationComplete(m_HitEffect.m_Animator, "Skill_3_Down", () =>
        //    //{
        //    //    m_HitEffect.Hide();
        //    //    m_HitEffect.m_Animator.enabled = false;
        //    //}));

        //    m_Owner.OnAni_Attack(pRange);
        //}
    }
}
