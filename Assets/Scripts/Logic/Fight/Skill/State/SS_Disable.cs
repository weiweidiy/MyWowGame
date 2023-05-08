using Framework.GameFSM;
using UnityEngine;

namespace Logic.Fight.Skill.State
{
    /// <summary>
    /// 技能CD状态
    /// </summary>
    public class SS_Disable : IState<SkillState, SkillStateData>
    {
        private float m_SkillCD = 0;
        public SS_Disable(SkillState pType) : base(pType)
        {
        }

        public override void Enter(SkillStateData pContext)
        {
            //Debug.LogError("SS - SS_Disable");
            m_SkillCD = 0;
        }

        public override void Update(SkillStateData pContext)
        {
            m_SkillCD += Time.deltaTime;
            if (m_SkillCD >= pContext.m_SkillBase.m_SkillData.CD)
            {
                pContext.m_SM.ToIdle();
            }
        }
    }
}