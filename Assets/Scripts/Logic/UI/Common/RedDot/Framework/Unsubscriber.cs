using System;
using System.Collections.Generic;

namespace Logic.Common.RedDot
{
    public class Unsubscriber<T> : IDisposable
    {
        private List<IObserver<T>> _observers;
        private IObserver<T> _observer;

        internal Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        public void Dispose()
        {
            if (_observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }


    public class UnSubScriberRedDot : IDisposable
    {
        private List<RedDotObserver> _observers;
        private RedDotObserver _observer;

        internal UnSubScriberRedDot(List<RedDotObserver> observers, RedDotObserver observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        public void Dispose()
        {
            if (_observers.Contains(_observer))
                _observers.Remove(_observer);
        }

        public bool HasAction(Action<string, RedDotInfo, string> action)
        {
            return this._observer.HasAction(action);
        }
    }

}