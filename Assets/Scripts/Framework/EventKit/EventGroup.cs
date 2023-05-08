using System;
using System.Collections.Generic;
using Logic.Common;

namespace Framework.EventKit
{
    /*
     * 方便注册和反注册事件
     */
    public class EventGroup
    {
        private readonly HashSet<EventTicket> m_EventTickets = new HashSet<EventTicket>(2);

        public EventGroup Register(EventTicket pTicket)
        {
            m_EventTickets.Add(pTicket);
            return this;
        }
        
        public EventGroup Register(int pEvent, Action<int, object> pHandler)
        {
            Register(EventManager.Ins.RegisterEvent(pEvent, pHandler));
            return this;
        }
        
        public EventGroup Register(LogicEvent pEvent, Action<int, object> pHandler)
        {
            Register(EventManager.Ins.RegisterEvent((int)pEvent, pHandler));
            return this;
        }
        
        public void Release()
        {
            foreach(var _Event in m_EventTickets)
                _Event.UnRegister();
                
            m_EventTickets.Clear();
        }

        ~EventGroup()
        {
            Release();
        }
    }
}