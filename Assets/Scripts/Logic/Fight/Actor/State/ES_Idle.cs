using Framework.GameFSM;

namespace Logic.Fight.Actor.State
{
    /// <summary>
    /// 怪物待机状态
    /// </summary>
    public class ES_Idle : IState<EnemyState, EnemyStateData>
    {
        public ES_Idle(EnemyState pType) : base(pType)
        {
        }        
        
        public override void Enter(EnemyStateData pContext)
        {
            pContext.m_Owner.PlayIdle();
        }
    }
}