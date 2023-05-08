
using Framework.GameFSM;
using Logic.Common;
using Logic.Fight;

namespace Logic.States.Fight
{
    /// <summary>
    /// 战斗状态机
    /// </summary>
    public class FightSM : FSMachine<IState<FightState, FightStateData>, FightState, FightStateData>
    {
        #region 状态机切换

        public void ToStandby()
        {
            NextState(m_ContextData.m_Standby);
        }

        public void ToFighting()
        {
            NextState(m_ContextData.m_Fighting);
        }

        public void ToFightOver()
        {
            NextState(m_ContextData.m_FightOver);
        }
        
        public void ToSwitch(SwitchToType pSwitchToType)
        {
            FightManager.Ins.SwitchToType = pSwitchToType;
            NextState(m_ContextData.m_Switch);
        }
        
        public void ToHandUp()
        {
            NextState(m_ContextData.m_HandUp);
        }
        
        #endregion
    }
    
    //状态机上下文
    public class FightStateData
    {
        public FightSM m_SM;

        public readonly FS_Standby m_Standby = new (FightState.Standby);
        public readonly FS_Fighting m_Fighting = new (FightState.Fighting);
        public readonly FS_Over m_FightOver = new (FightState.Over);
        public readonly FS_Switch m_Switch = new (FightState.Switch);
        public readonly FS_HandUp m_HandUp = new (FightState.HandUp);

        //当前的关卡类型
        public LevelType m_LevelType = LevelType.NormalLevel;
        //战斗结果
        public bool m_IsWin = false;

        public void Reset()
        {
            
        }
    }

    //战斗状态
    public enum FightState
    {
        Standby,    //战斗预备阶段(移动 刷怪)
        Fighting,   //战斗中
        Over,       //战斗结束
        Switch,     //战斗特殊UI/清理战场/切换到特殊战斗状态(Boss/副本等)
        HandUp,     //挂机状态
    }
}