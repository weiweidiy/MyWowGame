using System;

namespace Framework.EventKit
{
    public readonly struct EventTicket
    {
        public readonly Action<int, object> m_Handle;
        public readonly int m_Event;

        public EventTicket(int pEvent, Action<int, object> pHandle)
        {
            m_Handle = pHandle;
            m_Event = pEvent;
        }

        public void UnRegister()
        {
            EventManager.Ins.RemoveEvent(m_Event, m_Handle);
        }
    }
}