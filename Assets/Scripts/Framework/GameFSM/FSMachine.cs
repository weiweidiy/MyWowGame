using System;

namespace Framework.GameFSM
{
    //状态机基类
    public class FSMachine<State, StateType, StateContext> where State : IState<StateType, StateContext> where StateType : Enum
    {
        protected State m_CurState;
        protected State m_NextState;
        protected State m_PreState;

        //状态机上下文
        public StateContext m_ContextData { get; private set; }
        
        //启动状态机
        public virtual void Start(StateContext pContextData)
        {
            m_ContextData = pContextData;
        }

        public virtual void Start(StateContext pContextData, State pState)
        {
            m_ContextData = pContextData;
            NextState(pState);
        }

        //状态机销毁
        public virtual void Release()
        {
            if (m_CurState != null)
            {
                m_CurState.Release(m_ContextData);
                m_CurState = null;
            }

            m_PreState = null;
            m_NextState = null;
        }

        //状态更新
        public virtual void Update()
        {
            if (m_NextState != null)
            {
                m_PreState = m_CurState;
                m_CurState?.Release(m_ContextData);
                m_CurState = m_NextState;
                m_NextState = null;

                m_CurState.Enter(m_ContextData);
            }
            else
            {
                m_CurState?.Update(m_ContextData);
            }
        }

        //状态切换
        public virtual void NextState(State pNextState)
        {
            if(pNextState == null)
                return;
            m_NextState = pNextState;
        }

        //获取当前状态
        public virtual State GetState()
        {
            return m_CurState ?? m_NextState;
        }

        //获取即将切换的状态
        public virtual State GetNextState()
        {
            return m_NextState;
        }
    }
}
