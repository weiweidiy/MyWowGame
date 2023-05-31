using Logic.Manager;
using Logic.UI.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.Cells
{
    public class CommonQuenchingInfo : MonoBehaviour
    {
        public Image m_Quality;
        public TextMeshProUGUI m_AttrText;

        public void Init(int cId)
        {
            var djDesData = QuenchingManager.Ins.GetDjDesData(cId);
            var attrValue = QuenchingManager.Ins.GetAttributeData(djDesData.AttributeID).Value;
            UICommonHelper.LoadQuenchingQuality(m_Quality, djDesData.Quality);
            m_AttrText.text = string.Format(djDesData.DJDes, attrValue);
        }
    }
}