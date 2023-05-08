using System;
using System.Threading.Tasks;
using Framework.EventKit;


namespace Framework.GameFSM
{
    //StateType : 状态类型标识 Enum
    //StateContext : 状态上下文
    public abstract class IState<StateType, StateContext> where StateType : Enum
    {
        public readonly StateType m_Type;
        
        //事件处理接口对象
        protected readonly EventGroup m_EventGroup = new();
        protected IState(StateType pType)
        {
            m_Type = pType;
        }
        public virtual void Enter(StateContext pContext){}
        public virtual void Update(StateContext pContext){}

        public virtual void Release(StateContext pContext)
        {
            m_EventGroup.Release();
        }
    }
}
