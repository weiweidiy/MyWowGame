using Framework.GameFSM;
using Logic.Fight.Common;
using UnityEngine;

namespace Logic.Fight.Actor.State
{
    /// <summary>
    /// 怪物死亡状态
    /// </summary>
    public class ES_Dead : IState<EnemyState, EnemyStateData>
    {
        private static readonly Vector3 s_DeadPos = new Vector3(1000, 0, 0);
        public ES_Dead(EnemyState pType) : base(pType)
        {
        }
     
        private EnemyStateData m_StateData;
        public override void Enter(EnemyStateData pContext)
        {
            m_StateData = pContext;
            
            //播放死亡动画
            pContext.m_Owner.PlayDead();
            pContext.m_Owner.transform.SetParent(FightManager.Ins.m_GroundNode);
            
            pContext.m_Owner.OnAni_DeadCB += OnDead;
        }

        public override void Release(EnemyStateData pContext)
        {
            base.Release(pContext);
            pContext.m_Owner.OnAni_DeadCB -= OnDead;
        }
        
        private void OnDead()
        {
            m_StateData.m_Owner.transform.position = s_DeadPos;
            m_StateData.m_SM.ToRecycle();
        }
    }
}