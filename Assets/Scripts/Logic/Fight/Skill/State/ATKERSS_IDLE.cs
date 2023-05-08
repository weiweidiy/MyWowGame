using Framework.GameFSM;

namespace Logic.Fight.Skill.State
{
    public class ATKERSS_IDLE : IState<AttackerState, AttackerStateData>
    {
        public ATKERSS_IDLE(AttackerState pType) : base(pType)
        {

        }

        public override void Enter(AttackerStateData pContext)
        {
            base.Enter(pContext);

            //Debug.LogError("进入待机状态");
            //播放待机动画
            pContext.m_Attacker.OnIdle();
        }

        public override void Release(AttackerStateData pContext)
        {
            base.Release(pContext);
        }


    }
}