using Logic.Manager;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.UIRole
{
    public class UIReformSelectionItem : MonoBehaviour
    {
        public event Action<UIReformSelectionItem,ReformCopyManager.SelectionItemVO> onClicked;
        public event Action<UIReformSelectionItem, ReformCopyManager.SelectionItemVO> onSelected;

        [SerializeField] Button btnClick;
        [SerializeField] GameObject goSelection;

        [SerializeField] TextMeshProUGUI txtTitle;
        [SerializeField] TextMeshProUGUI txtName;
        [SerializeField] TextMeshProUGUI txtDesc;

        ReformCopyManager.SelectionItemVO vo;

        public void Init(ReformCopyManager.SelectionItemVO vo)
        {
            btnClick.onClick.AddListener(OnClicked);

            Refresh(vo);
        }


        public void Refresh(ReformCopyManager.SelectionItemVO vo)
        {
            this.vo = vo;
            txtTitle.text = vo.title;
            txtName.text = vo.name;
            txtDesc.text = vo.desc;
        }

        private void OnClicked()
        {
            onClicked?.Invoke(this,this.vo);
        }

        public void Select(bool select)
        {
            goSelection.SetActive(select);

            if (select)
                onSelected?.Invoke(this, this.vo);
        }

        public bool Selected()
        {
            return goSelection.activeSelf;
        }

        public ReformCopyManager.SelectionItemVO GetVO()
        {
            return vo;
        }
    }
}