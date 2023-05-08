using System;
using UnityEngine;

namespace Logic.Common
{
    /// <summary>
    /// 从动画帧 回调到逻辑的接口
    /// 使用此对象 确保多个地方回调使用的是同样的接口
    /// </summary>
    public class EventFromAnimation : MonoBehaviour
    {
        public Action OnAni_AttackStart;
        public Action OnAni_Attack;
        public Action OnAni_AttackEnd;
        
        public Action OnAni_SkillStart;
        public Action OnAni_Skill;
        public Action OnAni_SkillEnd;

        public Action OnAni_Dead;

        public void OnAttackStart()
        {
            OnAni_AttackStart?.Invoke();
        }
        
        public void OnAttack()
        {
            OnAni_Attack?.Invoke();
        }
        
        public void OnAttackEnd()
        {
            OnAni_AttackEnd?.Invoke();
        }

        public void OnSkillStart()
        {
            OnAni_SkillStart?.Invoke();
        }
        
        public void OnSkill()
        {
            OnAni_Skill?.Invoke();
        }
        
        public void OnSkillEnd()
        {
            OnAni_SkillEnd?.Invoke();
        }

        public void OnDead()
        {
            OnAni_Dead?.Invoke();
        }
    }
}