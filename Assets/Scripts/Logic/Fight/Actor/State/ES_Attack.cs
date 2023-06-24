using Framework.GameFSM;
using Logic.Common;
using UnityEngine;

namespace Logic.Fight.Actor.State
{
    /// <summary>
    /// 怪物攻击状态
    /// </summary>
    public class ES_Attack : IState<EnemyState, EnemyStateData>
    {
        public ES_Attack(EnemyState pType) : base(pType)
        {
        }
        
        private EnemyStateData m_Context;
        public override void Enter(EnemyStateData pContext)
        {
            m_Context = pContext;

            pContext.m_Owner.OnEnterAttack();

            if(pContext.m_Owner.m_Attack > 0.01)
                pContext.m_Owner.PlayAttack();
            m_EventGroup.Register(LogicEvent.Fight_Over, OnFightOver);
        }




        private void OnFightOver(int arg1, object arg2)
        {
            if(!m_Context.m_Owner.IsDead())
                m_Context.m_SM.ToIdle();
        }

        public override void Update(EnemyStateData pContext)
        {
            base.Update(pContext);

            //如果CD结束，则再次攻击
            if (!pContext.m_Owner.IsUnderAttackCD && pContext.m_Owner.m_Attack > 0.01)
            {
                //Debug.LogError("怪物攻击CD结束，再次攻击！");
                //pContext.m_SM.ToAttack();
                pContext.m_Owner.PlayAttack();
            }
             
            
        }
    }
}