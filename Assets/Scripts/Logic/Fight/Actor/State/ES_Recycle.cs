using Framework.GameFSM;
using Logic.Fight.Common;

namespace Logic.Fight.Actor.State
{
    /// <summary>
    /// 怪物回收到Pool
    /// 死亡 -> 回收
    /// 清理状态(Switch) -> 回收
    /// </summary>
    public class ES_Recycle : IState<EnemyState, EnemyStateData>
    {
        public ES_Recycle(EnemyState pType) : base(pType)
        {
        }
        
        public override void Enter(EnemyStateData pContext)
        {
            pContext.m_Owner.m_Animator.enabled = false;
            FightEnemySpawnManager.Ins.RecycleEnemy(pContext.m_Owner);
            pContext.m_SM.Release();
        }
    }
}