using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.UISpoil
{
    public class SpoilHandBookContentView : MonoBehaviour
    {
        [SerializeField] SpoilUnitView unitView;
        [SerializeField] TextMeshProUGUI txtName;
        [SerializeField] TextMeshProUGUI txtSkillDesc;
        [SerializeField] Image imgBgEquiped;

        public int SpoilId { get; set; }

        public void Refresh(SpoilHandBookContentVO vo)
        {
            txtName.text = vo.name;
            txtSkillDesc.text = vo.skillDesc;
            unitView.Refresh(vo.unitVO);
            imgBgEquiped.gameObject.SetActive(vo.unitVO.equiped == true);
        }
    }
}