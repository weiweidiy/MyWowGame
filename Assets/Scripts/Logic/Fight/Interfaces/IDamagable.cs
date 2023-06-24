using UnityEngine;

namespace Logic.Fight.Skill.State
{
    public interface IDamagable
    {
        bool IsDead();

        Vector3 GetPos(bool pNeedRandom = true);

        Transform GetTransform();
    }
}