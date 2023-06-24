using Cysharp.Threading.Tasks;
using Networks;
using System;

namespace Logic.Manager
{
    public abstract class CopyLogicManager
    {
        protected CopyManager copyManager;
        public CopyLogicManager(CopyManager copyManager)
        {
            this.copyManager = copyManager;
        }

        public abstract UniTask OnEnter(S2C_EnterCopy pMsg);

        public abstract void OnExit(S2C_ExitCopy pMsg);

        public abstract void RequestEnterCopy();
        public abstract void RequestExitCopy(bool isWin);
    }
}