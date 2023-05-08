using Framework.GameFSM;
using Logic.Data;
using UnityEngine;

namespace Logic.Fight.Skill.State
{
    /// <summary>
    /// 技能空闲状态
    /// </summary>
    public class SS_Idle : IState<SkillState, SkillStateData>
    {
        public SS_Idle(SkillState pType) : base(pType)
        {
        }

        public override void Enter(SkillStateData pContext)
        {
            //Debug.LogError("SS - SS_Idle");
            pContext.m_CurrentTarget = null;
        }

        public override void Update(SkillStateData pContext)
        {
            //判断是否处于 自动释放 或者 手动释放
            if (GameDataManager.Ins.AutoSkill)
            {
                pContext.m_SM.ToAuto();
            }
        }
    }
}