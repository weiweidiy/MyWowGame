using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Extension;
using Logic.Common;
using UnityEngine;

namespace Framework.EventKit
{
    public class EventManager : Singleton<EventManager>
    {
        private EventManager()
        {
            
        }
        
        #region static
        public static EventTicket Register(int pEvent, Action<int, object> pHandler)
        {
            return Ins.RegisterEvent(pEvent, pHandler);
        }
        
        public static EventTicket Register(LogicEvent pEvent, Action<int, object> pHandler)
        {
            return Ins.RegisterEvent((int)pEvent, pHandler);
        }

        public static void UnRegister(EventTicket pEt)
        {
            Ins.RemoveEvent(pEt.m_Event, pEt.m_Handle);
        }

        public static void Call(int pEvent, object pParam = null)
        {
            Ins.CallEvent(pEvent, pParam);
        }
        
        public static void Call(LogicEvent pEvent, object pParam = null)
        {
            Ins.CallEvent((int)pEvent, pParam);
        }
        
        public static void Remove(Action<int, object> pHandler)
        {
            Ins.RemoveEvent(pHandler);
        }

        public static void Remove(int pEvent, Action<int, object> pHandler)
        {
            Ins.RemoveEvent(pEvent, pHandler);
        }
        
        public static void Remove(LogicEvent pEvent, Action<int, object> pHandler)
        {
            Ins.RemoveEvent((int)pEvent, pHandler);
        }

        public static void Remove(int pEvent)
        {
            Ins.RemoveEvent(pEvent);
        }
        
        public static void Remove(LogicEvent pEvent)
        {
            Ins.RemoveEvent((int)pEvent);
        }

        #endregion

        //Event 1 : n Handle
        private readonly Dictionary<int, List<Action<int, object>>> m_Event2Handle = new(128);
        //Handle 1 : n Event
        private readonly Dictionary<Action<int, object>, List<int>> m_Handle2Event = new(128);
        
        public EventTicket RegisterEvent(int pEvent, Action<int, object> pHandler)
        {
            //是否已存在？
            if (m_Handle2Event.ContainsKey(pHandler))
            {
                if (m_Handle2Event[pHandler].Any(pE => pE == pEvent))
                {
                    Debug.LogError($"重复注册事件! Event:{pEvent}, Handle:{pHandler}");
                    return new EventTicket(pEvent, pHandler);
                }
            }

            //登记
            
            if (m_Event2Handle.TryGetValue(pEvent, out var _HandleList))
                _HandleList.Add(pHandler);
            else
                m_Event2Handle.Add(pEvent, new List<Action<int, object>>{ pHandler });

            if (m_Handle2Event.TryGetValue(pHandler, out var _EventList))
                _EventList.Add(pEvent);
            else
                m_Handle2Event.Add(pHandler, new List<int>{ pEvent });

            return new EventTicket(pEvent, pHandler);
        }

        public void CallEvent(int pEvent, object pParam = null)
        {
            if(m_Event2Handle.TryGetValue(pEvent, out var _Handlers))
            {
                foreach(var _Handler in _Handlers)
                {
                    _Handler.Invoke(pEvent, pParam);
                }
            }
            else
            {
                Debug.LogWarning($"没有找到对应的事件处理. Event:{pEvent} : {(LogicEvent)pEvent}");
            }
        }

        //* 移除所有 关于此Handler的注册!
        public void RemoveEvent(Action<int, object> pHandler)
        {
            if(m_Handle2Event.TryGetValue(pHandler,out var _GameEvents))
            {
                foreach(var _Item in _GameEvents)
                {
                    if(m_Event2Handle.TryGetValue(_Item, out var _Handlers))
                    {
                        _Handlers.Remove(pHandler);
                        if(_Handlers.Count == 0)
                        {
                            m_Event2Handle.Remove(_Item);
                        }
                    }
                }
                m_Handle2Event.Remove(pHandler);
            }
        }

        //移除指定的Event和Handler
        public void RemoveEvent(int pEvent, Action<int, object> pHandler)
        {
            if (m_Event2Handle.TryGetValue(pEvent, out var _Handlers))
            {
                _Handlers.Remove(pHandler);
                if (_Handlers.Count == 0)
                    m_Event2Handle.Remove(pEvent);
            }

            if(m_Handle2Event.TryGetValue(pHandler,out var _GameEvents))
            {
                _GameEvents.Remove(pEvent);
                if (_GameEvents.Count == 0)
                    m_Handle2Event.Remove(pHandler);
            }
        }

        //* 移除所有pEvent注册的事件处理!
        public void RemoveEvent(int pEvent)
        {
            if (m_Event2Handle.TryGetValue(pEvent, out var _Handles))
            {
                foreach (var _Item in _Handles)
                {
                    if (m_Handle2Event.TryGetValue(_Item, out var _GameEvents))
                    {
                        _GameEvents.Remove(pEvent);
                        if (_GameEvents.Count == 0)
                        {
                            m_Handle2Event.Remove(_Item);
                        }
                    }
                }
                m_Event2Handle.Remove(pEvent);
            }
        }

        public void Dump()
        {
            foreach (var _Eh in m_Event2Handle)
            {
                string _S = _Eh.Key + "";
                foreach (var _Action in _Eh.Value)
                {
                    _S += " - " + _Action;
                }
                Debug.LogError(_S);
            }

            foreach (var _Eh in m_Handle2Event)
            {
                string _S = _Eh.Key + "";
                foreach (var _Action in _Eh.Value)
                {
                    _S += " - " + _Action;
                }
                Debug.LogError(_S);
            }
        }
    }

}