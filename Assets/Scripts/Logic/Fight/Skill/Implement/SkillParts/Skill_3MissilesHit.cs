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
            StartCoroutine(WaitDestroy(1f));
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
    }
}