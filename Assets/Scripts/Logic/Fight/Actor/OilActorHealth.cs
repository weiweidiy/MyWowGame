using BreakInfinity;
using Logic.Fight.Data;
using Logic.Manager;
using UnityEngine;

namespace Logic.Fight.Actor
{
    public class OilActorHealth : ActorHealth
    {
        public override void Damage(BigDouble pDamage ,bool pIsCritical = false, Transform target = null)
        {
            base.Damage(pDamage, pIsCritical, target);

            CopyManager.Ins.CurTotalDamage += pDamage;
        }


    }
}