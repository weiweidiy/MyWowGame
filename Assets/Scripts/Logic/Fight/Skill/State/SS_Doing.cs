using Framework.GameFSM;
using UnityEngine;

namespace Logic.Fight.Skill.State
{
    /// <summary>
    /// 技能释放 - 持续中
    /// </summary>
    public class SS_Doing : IState<SkillState, SkillStateData>
    {
        float m_DurationTime = 0;
        public SS_Doing(SkillState pType) : base(pType)
        {
        }

        public override void Enter(SkillStateData pContext)
        {
            //Debug.LogError("SS - SS_Doing");
            
            NeedSearch = false;
            m_DurationTime = 0;

            pContext.m_CurrentTarget = null;
            pContext.m_SkillBase.OnStartSkill();
        }

        private bool NeedSearch = false;
        public override void Update(SkillStateData pContext)
        {
            m_DurationTime += Time.deltaTime;
            if (m_DurationTime >= pContext.m_SkillBase.m_SkillData.DurationTime)
            {
                //技能正常结束 才会调用这里
                pContext.m_SkillBase.OnStopSkill();
                pContext.m_SM.ToDisable();
                return;
            }

            if (!pContext.m_SkillBase.NeedSearchTarget()) //技能是否要搜敌
                return;

            //当前是否有有效目标 搜索敌人
            if(pContext.m_CurrentTarget == null || pContext.m_CurrentTarget.IsDead())
            {
                NeedSearch = true;
            }

            if(NeedSearch)
            {
                //搜敌
                pContext.m_CurrentTarget = pContext.m_SkillBase.GetSkillTarget();
                if (pContext.m_CurrentTarget != null)
                {
                    pContext.m_SkillBase.OnFindTarget(pContext.m_CurrentTarget);
                    NeedSearch = false;
                }
                else
                {
                    //没有搜索到

                }
            }
        }

        public override void Release(SkillStateData pContext)
        {
            base.Release(pContext);
            m_DurationTime = 0;
        }
    }
}
