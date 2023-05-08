using System;
using Logic.Common;
using Logic.Fight.Actor;
using Logic.Fight.Common;
using Logic.Fight.Weapon;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityTimer;

namespace Logic.Fight.GJJ
{
    /// <summary>
    /// 臂炮控制
    /// </summary>
    public class NormalGunCtrl : MonoBehaviour
    {
        [LabelText("攻击阈值"), Tooltip("怪物与玩家的距离小于等于这个值时会自动释放技能")]
        public float m_AttackThreshold = 4.5f;
        
        public NormalGun m_RightGun;
        public NormalGun m_LeftGun;
        
        //当前目标
        [NonSerialized] public Enemy m_CurrentTarget;
        private Timer m_AttackTimer;
        private bool m_IsRightGun = true;
        public void StartStandby()
        {
            m_CurrentTarget = null;
            m_AttackTimer?.Cancel();
            
            m_RightGun.ToStandby();
            m_LeftGun.ToStandby();
        }
        
        public void StartAttack()
        {
            m_CurrentTarget = null;
            var _Time = 1f / Formula.GetGJJAtkSpeed();
            m_AttackTimer = Timer.Register(_Time, () =>
            {
                if(!HasOneTarget())
                    return;
                if (m_IsRightGun)
                    m_RightGun.ToAttack(m_CurrentTarget, m_TargetPos);
                else
                    m_LeftGun.ToAttack(m_CurrentTarget, m_TargetPos);
                m_IsRightGun = !m_IsRightGun;
            }, null, true);
        }
        
        //当前目标的坐标信息(坐标会随机变化 以第一次获取目标后的坐标为准)
        private Vector3 m_TargetPos;
        //尝试获取一个目标
        private bool HasOneTarget()
        {
            if (IsInvalidTarget())
                m_CurrentTarget = FightEnemyManager.Ins.GetOneTarget(m_AttackThreshold);
            if (IsInvalidTarget())
                return false;
            m_TargetPos = m_CurrentTarget.GetPos();
            return true;
        }
        
        private bool IsInvalidTarget()
        {
            if (m_CurrentTarget != null && !m_CurrentTarget.IsDead())
                return false;
            return true;
        }
    }
}