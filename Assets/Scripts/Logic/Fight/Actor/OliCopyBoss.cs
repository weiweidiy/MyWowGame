using BreakInfinity;
using Framework.EventKit;
using Logic.Common;
using Logic.Manager;

namespace Logic.Fight.Actor
{
    public class OliCopyBoss : RangeEnemy
    {
        protected override void Awake()
        {
            base.Awake();

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
            //(float)(pCurHP / m_Health.MaxHP)
        }

        protected override void OnDead()
        {
            CopyManager.Ins.CurBossLevel++;

            //单次死亡
            m_Health.Recover(CopyManager.Ins.GetCopyOilBossHp());
        }
    }
}