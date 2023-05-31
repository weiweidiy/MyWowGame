using Logic.UI.Common;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.UISpoil
{
    public struct SpoilHandBookHeadVO
    {
        public int groupId;
        public string iconPath;
        public string typeName;
    }

    public class SpoilHandBookHeadView : MonoBehaviour
    {
        [SerializeField] Image imgIcon;
        [SerializeField] TextMeshProUGUI txtType;
        [SerializeField] Button btnSwitchOnOff;

        public event Action<SpoilHandBookHeadView, bool> onSwitchClick;

        bool switchOn = true;

        SpoilHandBookHeadVO vo;

        private void Awake()
        {
            btnSwitchOnOff.onClick.AddListener(OnSwitchClick);
        }

        private void OnSwitchClick()
        {
            switchOn = !switchOn;
            onSwitchClick?.Invoke(this, switchOn);
            Switch(switchOn);
        }

        public void Refresh(SpoilHandBookHeadVO vo)
        {
            this.vo = vo;

            if (vo.iconPath != "")
                UICommonHelper.LoadIcon(imgIcon, vo.iconPath);

            txtType.text = vo.typeName;
            Switch(switchOn);
        }

        public void Switch(bool isOn)
        {
            var z = isOn == true ? 90 : -90;
            btnSwitchOnOff.transform.rotation = Quaternion.Euler(0, 0, z);
        }

        public SpoilHandBookHeadVO GetVO()
        {
            return vo;
        }
    }
}