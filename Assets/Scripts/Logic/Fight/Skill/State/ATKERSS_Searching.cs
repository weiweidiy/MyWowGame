using Framework.GameFSM;
using Logic.Fight.Actor;
using Logic.Fight.Common;
using System.Collections.Generic;
using UnityEngine;

namespace Logic.Fight.Skill.State
{
    public class ATKERSS_Searching : IState<AttackerState, AttackerStateData>
    {
        public ATKERSS_Searching(AttackerState pType) : base(pType)
        {

        }

        public override void Enter(AttackerStateData pContext)
        {
            base.Enter(pContext);
            //Debug.LogError("进入搜索状态");
            //播放搜敌人
            pContext.m_Attacker.OnSearching();
        }


        public override void Update(AttackerStateData pContext)
        {
            base.Update(pContext);

            if (pContext.m_CurrentTarget != null && !pContext.m_CurrentTarget.IsDead())
                return;

            //搜敌
            var target = pContext.m_Attacker.GetTarget();
            if (target != null)
            {
                pContext.m_Attacker.SetTarget(target);
            }
                     
        }


        public override void Release(AttackerStateData pContext)
        {
            base.Release(pContext);
        }


    }
}