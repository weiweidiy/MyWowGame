using BreakInfinity;
using Logic.Fight.Data;
using UnityEngine;

namespace Logic.Fight.Actor
{
    public class OilActorHealth : ActorHealth
    {
        public override void Damage(BigDouble pDamage, bool pIsCritical = false)
        {
            base.Damage(pDamage, pIsCritical);
        }


    }
}