using Framework.EventKit;
using Logic.Common;
using Logic.Manager;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Logic.Manager.ReformCopyManager;

namespace Logic.UI.UIRole
{
    public class UIReformCardSelectionController : MonoBehaviour
    {
        public event Action<ReformCopyManager.SelectionItemVO> onResumeFight;
        public event Action onReformListClicked;
        public event Action onRefreshClicked;
        public event Action onReturnClicked;

        [SerializeField] GameObject root;

        [SerializeField] Transform scrollerContent;

        [SerializeField] UIReformSelectionItem reformSelectionItem;

        [SerializeField] Transform lstParent;

        [SerializeField] Button btnResume;

        [SerializeField] Button btnReformList;

        [SerializeField] Button btnRefresh;

        [SerializeField] Button btnReturn;


        UIReformSelectionItem curSelectItem = null;

        protected readonly EventGroup m_EventGroup = new();

        private void Awake()
        {
            btnResume.onClick.AddListener(OnResumeClicked);
            btnReformList.onClick.AddListener(OnRefromListClicked);
            btnRefresh.onClick.AddListener(OnRefresh);
            btnReturn.onClick.AddListener(OnReturn);

            m_EventGroup.Register(LogicEvent.RefreshReformItems, (i, o) =>
            {
                OnItemsRefresh(o as List<SelectionItemVO>);
            });
        }

        private void OnReturn()
        {
            onReturnClicked?.Invoke();
        }

        private void OnDestroy()
        {
            m_EventGroup.Release();
        }


        public void Init(List<ReformCopyManager.SelectionItemVO> selectionItemVOs)
        {
            ClearSelectionItems();

            CreateSelectionItems(selectionItemVOs);

        }

        /// <summary>
        /// 道具要刷新了
        /// </summary>
        /// <param name="selectionItemVOs"></param>
        private void OnItemsRefresh(List<SelectionItemVO> selectionItemVOs)
        {
            ClearSelectionItems();

            CreateSelectionItems(selectionItemVOs);
        }

        /// <summary>
        /// 创建选择道具对象
        /// </summary>
        /// <param name="selectionItemVOs"></param>
        void CreateSelectionItems(List<SelectionItemVO> selectionItemVOs)
        {
            foreach (var vo in selectionItemVOs)
            {
                var item = CreateSelectionItem();
                item.Init(vo);
                item.onSelected += Item_onSelected;
                item.onClicked += Item_onClicked;
            }
        }


        /// <summary>
        /// 点击了道具
        /// </summary>
        /// <param name="item"></param>
        /// <param name="obj"></param>
        private void Item_onClicked(UIReformSelectionItem item, ReformCopyManager.SelectionItemVO obj)
        {
            item.Select(true);
            curSelectItem = item;
        }

        /// <summary>
        /// 被点击了
        /// </summary>
        /// <param name="obj"></param>
        private void Item_onSelected(UIReformSelectionItem item, ReformCopyManager.SelectionItemVO obj)
        {
            if (curSelectItem != null && curSelectItem != item)
                curSelectItem.Select(false);
        }

        /// <summary>
        /// 点击了改造列表
        /// </summary>
        private void OnRefromListClicked()
        {
            onReformListClicked?.Invoke();
        }

        /// <summary>
        /// 点击了刷新
        /// </summary>
        private void OnRefresh()
        {
            onRefreshClicked?.Invoke();
        }

        /// <summary>
        /// 恢复战斗按钮被点击
        /// </summary>
        private void OnResumeClicked()
        {
            if(curSelectItem == null)
            {
                Debug.LogError("请选择一个改造");
                return;
            }    
            onResumeFight?.Invoke(curSelectItem.GetVO());

            ClearSelectionItems();
        }

        /// <summary>
        /// 设置是否激活
        /// </summary>
        /// <param name="active"></param>
        public void SetNodeActive(bool active)
        {
            root.SetActive(active);
        }

        /// <summary>
        /// 创建改造选择项
        /// </summary>
        /// <returns></returns>
        public UIReformSelectionItem CreateSelectionItem()
        {
            var component = Instantiate(reformSelectionItem, scrollerContent);
            return component;
        }

        /// <summary>
        /// 清除所有改造选项
        /// </summary>
        public void ClearSelectionItems()
        {
            foreach(var child in scrollerContent)
            {
                Destroy((child as Transform).gameObject);
            }

            curSelectItem = null;
        }
    }
}