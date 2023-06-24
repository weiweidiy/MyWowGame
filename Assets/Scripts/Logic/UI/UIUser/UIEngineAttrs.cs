using System.Collections.Generic;
using Framework.Extension;
using Framework.UI;
using Logic.Manager;
using Logic.UI.Cells;
using UnityEngine;

namespace Logic.UI.UIUser
{
    public class UIEngineAttrs : UIPage
    {
        public Transform root;
        public CommonEngineAttr commonEngineAttr;
        private List<CommonEngineAttr> engineAttrs = new List<CommonEngineAttr>();

        public override void OnShow()
        {
            base.OnShow();
            InstantiateEngineAttrs();
        }

        private void InstantiateEngineAttrs()
        {
            foreach (var insID in EngineManager.Ins.gameEngineData.OnList)
            {
                if (insID == 0) continue;
                var item = Instantiate(commonEngineAttr, root);
                item.Init(insID);
                engineAttrs.Add(item);
                item.Show();
            }
        }

        private void DestroyEngineAttrs()
        {
            foreach (var item in engineAttrs)
            {
                item.gameObject.Destroy();
            }

            engineAttrs.Clear();
        }

        public void OnBtnCloseClick()
        {
            DestroyEngineAttrs();
            this.Hide();
        }
    }
}