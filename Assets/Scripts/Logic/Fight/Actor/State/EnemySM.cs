using Framework.GameFSM;

namespace Logic.Fight.Actor.State
{
    /// <summary>
    /// 怪物状态机
    /// </summary>
    public class EnemySM : FSMachine<IState<EnemyState, EnemyStateData>, EnemyState, EnemyStateData>
    {
        #region 状态机切换

        public void ToIdle()
        {
            NextState(m_ContextData.m_Idle);
        }

        public void ToAttack()
        {
            NextState(m_ContextData.m_Attack);
        }

        public void ToDead()
        {
            NextState(m_ContextData.m_Dead);
        }
        
        public void ToMove()
        {
            NextState(m_ContextData.m_Move);
        }
        
        public void ToRecycle()
        {
            NextState(m_ContextData.m_Recycle);
        }

        #endregion
    }
    
    /// <summary>
    /// 状态机上下文
    /// </summary>
    public class EnemyStateData
    {
        public EnemySM m_SM;
        public Enemy m_Owner;
        
        //状态
        public readonly ES_Idle m_Idle = new (EnemyState.Idle);
        public readonly ES_Attack m_Attack = new (EnemyState.Attack);
        public readonly ES_Dead m_Dead = new (EnemyState.Dead);
        public readonly ES_Move m_Move = new (EnemyState.Move);
        public readonly ES_Recycle m_Recycle = new (EnemyState.Recycle);
    }

    /// <summary>
    /// 怪物状态定义
    /// </summary>
    public enum EnemyState
    {
        //待机
        Idle,
        //攻击
        Attack,
        //死亡
        Dead,
        //移动
        Move,
        //回收
        Recycle,
    }
}