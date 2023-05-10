using UnityEngine;
using UnityEngine.Rendering;

namespace Logic.Fight.Skill.Implement
{
    public class SkillSubObject : MonoBehaviour
    {
        public Transform m_body;
        public Transform m_hitEffect;

        SortingGroup[] sortingGroups;
        public SortingGroup[] SortingGroups => sortingGroups ?? (sortingGroups = GetComponentsInChildren<SortingGroup>());


        void OnEnable()
        {
            foreach (SortingGroup sortingGroup in SortingGroups)
                sortingGroup.enabled = true;
        }

        void OnDisable()
        {
            foreach (SortingGroup sortingGroup in SortingGroups)
                sortingGroup.enabled = false;
        }
    }
}