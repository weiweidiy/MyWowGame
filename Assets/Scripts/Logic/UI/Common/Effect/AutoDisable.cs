using System.Collections;
using UnityEngine;

namespace Logic.UI.Common.Effect
{
    public class AutoDisable : MonoBehaviour
    {
        public float m_Delay = 1f;
        private void OnEnable()
        {

            StartCoroutine(WaitForSeconds());
        }

        IEnumerator WaitForSeconds()
        {
            yield return new WaitForSeconds(m_Delay);
            gameObject.SetActive(false);
        }
    }
}