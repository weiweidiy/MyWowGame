using Framework.GameFSM;
using Logic.Data;
using Logic.Fight.Common;
using UnityEngine;

namespace Logic.Fight.Skill.State
{
    /// <summary>
    /// 技能自动释放状态
    /// </summary>
    public class SS_Auto : IState<SkillState, SkillStateData>
    {
        public SS_Auto(SkillState pType) : base(pType)
        {
        }

        public override void Enter(SkillStateData pContext)
        {
            //Debug.LogError("SS - SS_Auto");
            NeedSearch = false;
        }

        private bool NeedSearch = false;
        public override void Update(SkillStateData pContext)
        {
            if (!GameDataManager.Ins.AutoSkill)
            {
                pContext.m_SM.ToIdle();
            }
            
            //持续搜敌人 随时准备进入攻击状态
            if(pContext.m_CurrentTarget == null || pContext.m_CurrentTarget.IsDead())
            {
                NeedSearch = true;
            }
            
            if(NeedSearch)
            {
                //搜敌
                pContext.m_CurrentTarget = FightEnemyManager.Ins.GetOneTarget(pContext.m_SkillBase.m_AutoSkillThreshold, pContext.m_SkillBase.posType);
                if (pContext.m_CurrentTarget != null)
                {
                    NeedSearch = false;
                    pContext.m_SM.ToDoing();
                }
            }
        }
    }
}