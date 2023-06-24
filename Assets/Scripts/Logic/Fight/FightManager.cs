
using System;
using Chronos;
using DummyServer;
using Framework.EventKit;
using Framework.Extension;
using Logic.Common;
using Logic.Data;
using Logic.Fight.GJJ;
using Logic.States.Fight;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Logic.Fight
{
    /// <summary>
    /// 战斗管理
    /// </summary>
    [DefaultExecutionOrder(100)]
    public class FightManager : MonoSingleton<FightManager>
    {
        private readonly FightSM m_FSM = new ();
        private readonly EventGroup m_EventGroup = new ();
        private FightStateData m_FSMData;

        [LabelText("当前的GJJ节点")]
        public GJJCtrl m_CurGJJ;
        [LabelText("地面节点")]
        public Transform m_GroundNode;
        [NonSerialized]
        public bool m_IsInit;
        
        //切换状态机使用
        public SwitchToType SwitchToType { get; set; }
        //当前关卡类型
        public LevelType CurLevelType => m_FSM.m_ContextData.m_LevelType;

        public Timeline m_TimeLine;

        protected override void Awake()
        {
            base.Awake();

            m_TimeLine = GetComponent<Timeline>();
        }
        
        private void Start()
        {
            //初始化状态机
            m_FSMData = new FightStateData
            {
                m_SM = m_FSM,
                m_TimeLine = m_TimeLine,
            };
            
            if (GameDataManager.Ins.CurLevelNode == 5 && GameDataManager.Ins.LevelState == LevelState.Normal) 
            {
                //一下节点是BOSS 切换到BOSS
                SwitchToType = SwitchToType.ToBoss;
                m_FSM.Start(m_FSMData, m_FSMData.m_Switch);
            }
            else
            {
                m_FSM.Start(m_FSMData, m_FSMData.m_Standby);
            }
        }

        private void Update()
        {
            if(m_IsInit)
                m_FSM.Update();
        }

        protected override void OnDestroy()
        {
            m_EventGroup.Release();
            m_FSM.Release();
            base.OnDestroy();
        }
        
        //敌方获取GJJ目标
        public GJJCtrl GetGJJTarget()
        {
            return m_CurGJJ;
        }
        
        //GJJ是否死亡
        public bool IsGJJDead()
        {
            return m_CurGJJ.IsDead();
        }
        
        //当前是否已经到了BOSS关卡 / 普通关卡BOSS关
        public bool IsBossLevel()
        {
            if (GameDataManager.Ins.CurLevelNode == 5)
                return true;
            return false;
        }
        //下一波是否已经到了BOSS关卡 / 普通关卡BOSS关
        public bool NextIsBossLevel()
        {
            if (GameDataManager.Ins.CurLevelNode + 1 == 5)
                return true;
            return false;
        }
        
        public FightState GetCurState()
        {
            return m_FSM.GetState().m_Type;
        }

        public void ToIdle()
        {
            m_FSM.ToIdle();
        }
        public void ToStandBy()
        {
            m_FSM.ToStandby();
        }

        // public bool CanSwitchState()
        // {
        //     var _State = 
        // }
        
        //TODO 临时放到这里处理 退出游戏的情况

        private bool _IsFirstGame;
        private long _LastGameDate;
        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                LocalSaveManager.Ins.Save();
                
                
                //现在使用的客户端模拟服务器DummyServer导致下线的时候无法保存下线时间
                //这里临时处理一下 以后要改成真正的服务器
                //不要再这里搞其他的逻辑了
                _IsFirstGame = DummyServerMgr.Ins.DummyGetDB().m_IsFirstLogin;
                _LastGameDate = DummyServerMgr.Ins.DummyGetDB().m_LastGameDate;
                DummyServerMgr.Ins.DummyOnExitGame();
            }
            else
            {
                DummyServerMgr.Ins.DummyGetDB().m_IsFirstLogin = _IsFirstGame;
                DummyServerMgr.Ins.DummyGetDB().m_LastGameDate = _LastGameDate;
            }
        }

        private void OnApplicationQuit()
        {
            LocalSaveManager.Ins.Save();
            
            //现在使用的客户端模拟服务器DummyServer导致下线的时候无法保存下线时间
            //这里临时处理一下 以后要改成真正的服务器
            //不要再这里搞其他的逻辑了
            DummyServerMgr.Ins.DummyOnExitGame();
        }
    }
}
