using UnityEngine;

namespace Logic.Fight.Skill.Implement.SkillParts
{
    public class Skill_9Splash : MonoBehaviour
    {
        public Skill_9 m_Owner;

        public void OnAniATK()
        {
            m_Owner.OnDoATK();
        }
        
        public void OnAniEnd()
        {
            m_Owner.OnAni_AttackEnd();
        }
    }
}