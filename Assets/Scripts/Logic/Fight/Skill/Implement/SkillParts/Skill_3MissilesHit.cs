using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Logic.Fight.Skill.Implement.SkillParts
{
    public class Skill_3MissilesHit : MonoBehaviour
    {
        public Animator m_Animator;
        public Transform m_Root;
        private static readonly int Play = Animator.StringToHash("Play");

        public void PlayAni(Vector3 pPos)
        {
            transform.SetParent(FightManager.Ins.m_GroundNode);
            transform.position = pPos;
            m_Animator.enabled = true;
            m_Animator.SetTrigger(Play);
            //StartCoroutine(WaitDestroy(1f));
            DOTweenDelay(1f, 1);
        }

        public void Destroy()
        {
            GameObject.Destroy(gameObject);
        }


        IEnumerator WaitDestroy(float interval)
        {
            yield return new WaitForSeconds(interval);
            GameObject.Destroy(gameObject);
        }

        /// <summary>
        /// 废弃，不用了
        /// </summary>
        public void OnAniEnd()
        {
            m_Animator.enabled = false;
            transform.SetParent(m_Root);
            transform.position = Vector3.zero;
            gameObject.SetActive(false);
        }

        public void DOTweenDelay(float delayedTimer, int loopTimes)
        {
            float timer = 0;
            //DOTwwen.To()中参数：前两个参数是固定写法，第三个是到达的最终值，第四个是渐变过程所用的时间
            Tween t = DOTween.To(() => timer, x => timer = x, 1, delayedTimer).SetUpdate(UpdateType.Manual)
                          .OnStepComplete(() =>
                          {
                              GameObject.Destroy(gameObject);
                          })
                          .SetLoops(loopTimes);
        }
    }
}