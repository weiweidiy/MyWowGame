using Logic.Fight.Skill.State;
using UnityEngine;

namespace Logic.Fight.Skill.Implement
{
    /// <summary>
    /// 可以让技能找到的空目标
    /// </summary>
    public class NullTarget : MonoBehaviour, IDamagable
    {
        public Vector3 GetPos(bool pNeedRandom = true)
        {
            return transform.position;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public bool IsDead()
        {
            return false;
        }
    }
}