using Framework.GameFSM;
using UnityEngine;

namespace Logic.Fight.Skill.State
{
    public class ATKERSS_Emitting : IState<AttackerState, AttackerStateData>
    {
        public ATKERSS_Emitting(AttackerState pType) : base(pType)
        {
        }

        public override void Enter(AttackerStateData pContext)
        {
            base.Enter(pContext);

            //Debug.LogError("进入发射状态");
            pContext.m_Attacker.OnEmitting(pContext.m_CurrentTarget);
        }
        public override void Update(AttackerStateData pContext)
        {
            base.Update(pContext);
        }

        public override void Release(AttackerStateData pContext)
        {
            base.Release(pContext);
        }


    }
}