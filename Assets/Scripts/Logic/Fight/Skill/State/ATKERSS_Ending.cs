using Framework.GameFSM;
using UnityEngine;

namespace Logic.Fight.Skill.State
{

    public class ATKERSS_Ending : IState<AttackerState, AttackerStateData>
    {
        public ATKERSS_Ending(AttackerState pType) : base(pType)
        {

        }

        public override void Enter(AttackerStateData pContext)
        {
            base.Enter(pContext);

           // Debug.LogError("进入结束状态");

            pContext.m_Attacker.OnEnding();
        }

        public override void Release(AttackerStateData pContext)
        {
            base.Release(pContext);
        }


    }
}