using System;

namespace Logic.Common
{
    public interface IEventNotifier
    {
        event Action onEventRaise;
    }

}
