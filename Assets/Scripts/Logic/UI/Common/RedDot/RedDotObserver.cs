using System;

namespace Logic.Manager
{
    public class RedDotObserver : IObserver<RedDotInfo>
    {
        Action<RedDotInfo> action;
        public RedDotObserver(Action<RedDotInfo> action)
        {
            this.action = action;
        }

        public void OnCompleted()
        {
            //throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            //throw new NotImplementedException();
        }

        public void OnNext(RedDotInfo info)
        {
            action.Invoke(info);
        }
    }

}