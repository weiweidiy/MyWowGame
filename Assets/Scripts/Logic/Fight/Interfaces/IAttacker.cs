using Logic.Fight.Actor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Logic.Fight.Skill.State
{
    public interface IAttacker
    {
        event Action<IAttacker> onReady;
        event Action<IAttacker> onIdle;
        event Action<IAttacker> onSearching;
        event Action<IAttacker,IDamagable> onEmitting;
        event Action<IAttacker> onEnding;

        void Ready(params object[] args);
        void Idle();
        void Search();
        void Emit();
        void End();


        void OnReady();
        void OnIdle();
        void OnSearching();
        void OnEmitting(IDamagable m_CurrentTarget);
        void OnEnding();

        IDamagable GetTarget();
        void SetTarget(IDamagable target);
    }
}