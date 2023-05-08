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
            //transform.SetParent(FightManager.Ins.m_GroundNode);
            transform.position = pPos;
            m_Animator.enabled = true;
            m_Animator.SetTrigger(Play);
        }
        
        public void OnAniEnd()
        {
            m_Animator.enabled = false;
            transform.SetParent(m_Root);
            transform.position = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
}