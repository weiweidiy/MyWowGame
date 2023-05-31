using BreakInfinity;
using Framework.EventKit;
using Logic.Common;
using Logic.Manager;
using System.Collections;
using UnityEngine;

namespace Logic.Fight.Actor
{
    public class OliCopyBoss : RangeEnemy
    {
        //protected override void Awake()
        //{
        //    base.Awake();

            
        //}

        private void OnEnable()
        {
            EventManager.Call(LogicEvent.Fight_ShowOilBossHpBar);
        }


        public override bool IsDead()
        {
            return false;
        }

        public override void OnAni_Attack()
        {
            base.OnAni_Attack();

            CopyManager.Ins.CurBossAttackCount++;
            m_Attack = CopyManager.Ins.GetCopyOilBossATK();
        }

        public override void OnHpChange(BigDouble pCurHP)
        {
            object[] args = new object[2];
            args[0] = pCurHP;
            args[1] = m_Health.MaxHP;
            EventManager.Call(LogicEvent.Fight_OilBossHpChanged, args);
        }

        protected override void OnDead()
        {
            CopyManager.Ins.CurBossLevel++;

            StartCoroutine(RecoverHp());       
        }

        IEnumerator RecoverHp()
        {
            yield return new WaitForSeconds(0.1f);
            if(m_Health != null)
            {
                m_Health.MaxHP = CopyManager.Ins.GetCopyOilBossHp();
                m_Health.Recover(m_Health.MaxHP);
            }
                

        }
    }
}