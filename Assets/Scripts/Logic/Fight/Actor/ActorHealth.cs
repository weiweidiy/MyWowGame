using System;
using BreakInfinity;
using Logic.Fight.Data;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class UnityDamageEvent : UnityEvent<FightDamageData>
{
}

[Serializable]
public class UnityBDEvent : UnityEvent<BigDouble>
{
}

namespace Logic.Fight.Actor
{

    /// <summary>
    /// Actor血量管理
    /// 所有战斗中可被攻击拾取的目标 都必须包含此组件
    /// </summary>
    public class ActorHealth : MonoBehaviour
    {
        [LabelText("初始化")] public UnityBDEvent OnInit;
        [LabelText("血量变化")] public UnityBDEvent OnHealthChange;
        [LabelText("最大血量变化")] public UnityBDEvent OnMaxHealthChange;
        [LabelText("扣血")] public UnityDamageEvent OnHurt;
        [LabelText("回血")] public UnityBDEvent OnHeal;

        [LabelText("死亡")] public UnityEvent OnDeath;
        // [LabelText("免疫")]
        // public UnityEvent OnImmune;

        public BigDouble MaxHP { get; set; } = BigDouble.One;
        public bool IsDead => HP <= BigDouble.Zero;
        public BigDouble HP { get; protected set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(BigDouble pMax)
        {
            MaxHP = pMax;
            HP = MaxHP;

            OnInit.Invoke(MaxHP);
        }

        /// <summary>
        /// 受到攻击 扣血
        /// </summary>
        public virtual void Damage(BigDouble pDamage, bool pIsCritical = false)
        {
            if (pDamage < BigDouble.Zero)
            {
                Debug.LogWarning("Cannot take negative damage.");
                return;
            }

            var _PreHP = HP;
            if (HP - pDamage < BigDouble.Zero)
            {
                HP = BigDouble.Zero;
            }
            else
            {
                HP -= pDamage;
            }

            OnHealthChange.Invoke(HP);
            OnHurt.Invoke(new FightDamageData(pDamage, pIsCritical));
            if (_PreHP != BigDouble.Zero && HP == BigDouble.Zero)
            {
                OnDeath.Invoke();
            }
        }

        /// <summary>
        /// 恢复血量
        /// </summary>
        public void Recover(BigDouble pHeal)
        {
            if (HP + pHeal > MaxHP)
            {
                HP = MaxHP;
            }
            else
            {
                HP += pHeal;
            }

            OnHealthChange.Invoke(HP);
            OnHeal.Invoke(pHeal);
        }

        /// <summary>
        /// 立即杀死目标
        /// </summary>
        public void Kill()
        {
            if (HP != BigDouble.Zero)
            {
                HP = BigDouble.Zero;
                OnDeath.Invoke();
            }
        }
    }
}