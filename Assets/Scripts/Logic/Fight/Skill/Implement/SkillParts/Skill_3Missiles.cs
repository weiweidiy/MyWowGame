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

        public Transform m_HitPos;

        public Skill_3MissilesHit m_HitEffect;
        
        public void OnMissileATK()
        {
            m_HitEffect.Show();
            m_HitEffect.PlayAni(m_HitPos.position);
            m_Owner.OnAni_Attack(pRange);
        }
    }
}
