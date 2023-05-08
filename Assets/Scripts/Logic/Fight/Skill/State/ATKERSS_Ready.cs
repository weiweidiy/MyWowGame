using Framework.GameFSM;
using UnityEngine;

namespace Logic.Fight.Skill.State
{
    public class ATKERSS_Ready : IState<AttackerState, AttackerStateData>
    {
        public ATKERSS_Ready(AttackerState pType) : base(pType)
        {
        }

        public override void Enter(AttackerStateData pContext)
        {
            base.Enter(pContext);

            //Debug.LogError("进入准备状态");
            //播放准备动画
            pContext.m_Attacker.OnReady();
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