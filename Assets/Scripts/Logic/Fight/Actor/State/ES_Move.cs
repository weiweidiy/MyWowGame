using Framework.GameFSM;
using UnityEngine;

namespace Logic.Fight.Actor.State
{
    /// <summary>
    /// 朝向目标移动 直到小于攻击距离
    /// </summary>
    public class ES_Move : IState<EnemyState, EnemyStateData>
    {
        private EnemyStateData m_StateData;
        
        public ES_Move(EnemyState pType) : base(pType)
        {
        }

        public override void Enter(EnemyStateData pContext)
        {
            m_StateData = pContext;
            pContext.m_Owner.PlayMove();
        }

        public override void Update(EnemyStateData pContext)
        {
            if (m_StateData.m_Owner.IsDead())
            {
                pContext.m_SM.ToDead();
                return;
            }

            if (m_StateData.m_Owner.ArrivedAt())
            {
                pContext.m_SM.ToAttack();
                return;
            }
            //朝向目标移动
            Debug.Assert(pContext.m_TimeLine != null, "没有找到timeline" + pContext.m_Owner.name);
            float deltaTime = pContext.m_TimeLine ? pContext.m_TimeLine.deltaTime : Time.deltaTime;
            Move(deltaTime * Time.timeScale);
        }

        private void Move(float deltaTime)
        {
            //TODO 这里是通用直线移动, 如果有特殊处理 这里可以回调Owner 让它自己处理移动
            float _FinalSpeed = m_StateData.m_Owner.m_MoveSpeed * deltaTime;// Time.deltaTime;
            var _Dir = Vector2.left; //默认向左边移动
            m_StateData.m_Owner.transform.Translate(_Dir * _FinalSpeed, Space.World);
        }
    }
}