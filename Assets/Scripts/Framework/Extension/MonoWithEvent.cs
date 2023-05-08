
using Framework.EventKit;
using UnityEngine;

namespace Framework.Extension
{
    /// <summary>
    /// 普通Mono包含事件处理
    /// </summary>
    public class MonoWithEvent : MonoBehaviour
    {
        protected readonly EventGroup m_EventGroup = new ();
        
        protected virtual void OnDestroy()
        {
            m_EventGroup.Release();
        }
    }
}