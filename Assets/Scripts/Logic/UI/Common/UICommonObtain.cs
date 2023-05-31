using Framework.UI;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Logic.Common;
using System;

namespace Logic.UI.Common
{
    public class UICommonObtain : UIPage
    {
        [SerializeField] Image icon;
        [SerializeField] TextMeshProUGUI txtTitle;
        [SerializeField] TextMeshProUGUI txtName;
        [SerializeField] Button btnClose;
        [SerializeField] GameObject root;

        public struct Args
        {
            public string title;
            public string resPath;
            public string name;
        }

        private void Awake()
        {
            root.SetActive(false);

            m_EventGroup.Register(LogicEvent.ShowObtain, OnShowObtain);

            btnClose.onClick.AddListener(OnCloseClick);

            //txtTitle.shadowOffset = new Vector2(1, -1);
            //txtTitle.shadowColor = new Color(0, 0, 0, 0.5f);
        }

        private void OnCloseClick()
        {
            root.SetActive(false);
        }

        private async void OnShowObtain(int i, object o)
        {
            var arg = (Args)o;
            await UICommonHelper.LoadIconAsync(icon, arg.resPath);
            txtName.text = arg.name;
            txtTitle.text = arg.title;

            root.SetActive(true);
        }

    }
}

