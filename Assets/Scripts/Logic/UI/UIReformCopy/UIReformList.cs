using Configs;
using Framework.UI;
using System.Collections.Generic;
using Logic.UI.Cells;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Logic.Manager.ReformCopyManager;

namespace Logic.UI.UIRole
{
    public class UIReformList : UIPage
    {
        [SerializeField] Transform scrollerCylinderContent;
        [SerializeField] Transform scrollerAttrContent;

        [SerializeField] GameObject attrUnit;
        [SerializeField] GameObject cylinderUnit;

        [SerializeField] Button btnClose;

        private void Awake()
        {
            btnClose.onClick.AddListener(Close);
        }

        public override void OnShow()
        {
            base.OnShow();

            var lst = m_OpenData_ as List<SelectionItemVO>;

            foreach (var item in lst)
            {
                var go = CreateUnit(item);
            }
        }

        GameObject CreateUnit(SelectionItemVO vo)
        {
            switch (vo.itemType)
            {
                case ItemType.Attr:
                    return CreateAttr(vo.id);
                case ItemType.Cylinder:
                    return CreateCylinder(vo);
            }

            return null;
        }

        private GameObject CreateCylinder(SelectionItemVO vo)
        {
            var go = Instantiate(cylinderUnit, scrollerCylinderContent);

            var cylinder = go.GetComponent<CommonEngineItem>();
            cylinder.Init(vo.partData);

            return go;
        }

        private GameObject CreateAttr(int id)
        {
            var go = Instantiate(attrUnit, scrollerAttrContent);
            TextMeshProUGUI txt = go.transform.Find("Text").GetComponent<TextMeshProUGUI>();

            var cfg = AttributeCfg.GetData(id);

            txt.text = string.Format(cfg.Des, cfg.Value);
            return go;
        }
    }
}