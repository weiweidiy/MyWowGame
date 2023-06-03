using Chronos;
using Framework.GameFSM;
using Logic.Fight.Actor;

namespace Logic.Fight.Skill.State
{
    /// <summary>
    /// 技能状态机
    /// </summary>
    public class SkillSM : FSMachine<IState<SkillState, SkillStateData>, SkillState, SkillStateData>
    {
        #region 状态机切换

        public void ToIdle()
        {
            NextState(m_ContextData.m_Idle);
        }
        
        public void ToAuto()
        {
            NextState(m_ContextData.m_Auto);
        }

        public void ToDoing()
        {
            NextState(m_ContextData.m_Doing);
        }

        public void ToDisable()
        {
            NextState(m_ContextData.m_Disable);
        }

        #endregion
    }
    
    /// <summary>
    /// 状态机上下文
    /// </summary>
    public class SkillStateData
    {
        public SkillSM m_SM;
        public SkillBase m_SkillBase;
        public Enemy m_CurrentTarget;
        public Timeline m_TimeLine;

        //状态
        public readonly SS_Idle m_Idle = new (SkillState.Idle);
        public readonly SS_Auto m_Auto = new (SkillState.Auto);
        public readonly SS_Doing m_Doing = new (SkillState.Doing);
        public readonly SS_Disable m_Disable = new (SkillState.Disable);
    }

    /// <summary>
    /// 怪物状态定义
    /// </summary>
    public enum SkillState
    {
        //空闲
        Idle,
        //搜敌
        Auto,
        //持续释放
        Doing,
        //进入CD
        Disable,
    }
}