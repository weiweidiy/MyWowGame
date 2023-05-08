using TMPro;
using UnityEngine;

namespace Logic.UI.Cells
{
    public class CommonItemProbability : MonoBehaviour
    {
        public TextMeshProUGUI m_QualityProbability;

        public void UpdateProbabilityText(int probability)
        {
            m_QualityProbability.text = (probability / 10000f).ToString("F4") + "%";
        }
    }
}