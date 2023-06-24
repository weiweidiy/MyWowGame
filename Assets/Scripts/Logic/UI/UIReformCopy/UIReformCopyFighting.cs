using Cysharp.Threading.Tasks;
using Framework.EventKit;
using Framework.Extension;
using Framework.UI;
using Logic.Common;
using Logic.Manager;
using Logic.UI.UIMain;
using Logic.UI.UIRole;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

namespace Logic.UI.UICopy
{
    public class UIReformCopyFighting : UIPage
    {


        public UIReformHeroSelectionController uiHeroSelectionController;
        public UIReformCardSelectionController uiCardSelectionController;

        public GameObject root;
        public GameObject escapeNode;
        public Button btnEscape;


        ReformCopyManager.ReformState reformState;

        GameObject gjj;

        GameObject frame;

        private async void Awake()
        {
            gjj = GameObject.Find("GJJNode");

            uiHeroSelectionController.onStartClicked += UiHeroSelectionController_onStartClicked;
            uiHeroSelectionController.onReturnClicked += UiHeroSelectionController_onReturnClicked;
            uiCardSelectionController.onResumeFight += UiCardSelectionController_onResumeFight;
            uiCardSelectionController.onReformListClicked += UiCardSelectionController_onReformListClicked;
            uiCardSelectionController.onRefreshClicked += UiCardSelectionController_onRefreshClicked;
            uiCardSelectionController.onReturnClicked += UiHeroSelectionController_onReturnClicked; 

            frame = await CreateGJJFrame();

            btnEscape.onClick.AddListener(OnEscapeClicked);
        }



        public override void OnShow()
        {
            base.OnShow();

            root.Show();
            var data = m_OpenData_ as Tuple<ReformCopyManager.ReformState, List<ReformCopyManager.SelectionItemVO>>;
            reformState = data.Item1;
            var arg = data.Item2;

            uiHeroSelectionController.SetNodeActive(false);
            uiCardSelectionController.SetNodeActive(false);
            escapeNode.Hide();
            switch (reformState)
            {
                case ReformCopyManager.ReformState.DriverSelection:
                    uiHeroSelectionController.SetNodeActive(true);
                    break;
                case ReformCopyManager.ReformState.CardsSelection:
                    uiCardSelectionController.SetNodeActive(true);
                    uiCardSelectionController.Init(arg);
                    break;
            }
        }

        /// <summary>
        /// 退出副本
        /// </summary>
        private void UiHeroSelectionController_onReturnClicked()
        {
            var manager = CopyManager.Ins.GetCopyLogicManager(LevelType.ReformCopy) as ReformCopyManager;
            manager.EscapFight();

            Close();
        }

        /// <summary>
        /// 开始按钮被点击了
        /// </summary>
        /// <param name="progress"></param>
        private void UiHeroSelectionController_onStartClicked(int progress)
        {
            //root.Hide();
            uiHeroSelectionController.SetNodeActive(false);
            var manager = CopyManager.Ins.GetCopyLogicManager(LevelType.ReformCopy) as ReformCopyManager;
            manager.StartFight();
        }

        /// <summary>
        /// 恢复战斗按钮被点击
        /// </summary>
        /// <param name="obj"></param>
        private void UiCardSelectionController_onResumeFight(ReformCopyManager.SelectionItemVO vo)
        {
            //root.Hide();
            uiCardSelectionController.SetNodeActive(false);

            //Debug.LogError(vo.itemType + " " + vo.id);

            var manager = CopyManager.Ins.GetCopyLogicManager(LevelType.ReformCopy) as ReformCopyManager;
            manager.ResumeFight(vo);

            escapeNode.Show();
        }

        /// <summary>
        /// 改造列表点击
        /// </summary>
        private async void UiCardSelectionController_onReformListClicked()
        {
            var manager = CopyManager.Ins.GetCopyLogicManager(LevelType.ReformCopy) as ReformCopyManager;

            await UIManager.Ins.OpenUI<UIReformList>(manager.GetSelectionList());
        }

        /// <summary>
        /// 刷新按钮被点击
        /// </summary>
        private void UiCardSelectionController_onRefreshClicked()
        {
            var manager = CopyManager.Ins.GetCopyLogicManager(LevelType.ReformCopy) as ReformCopyManager;
            manager.RefreshItems();
        }

        /// <summary>
        /// 逃跑按钮被点击
        /// </summary>
        private void OnEscapeClicked()
        {
            var manager = CopyManager.Ins.GetCopyLogicManager(LevelType.ReformCopy) as ReformCopyManager;
            manager.EscapFight();
            //Close();
        }

        async UniTask<GameObject> CreateGJJFrame()
        {
            var handle = YooAssets.LoadAssetAsync<GameObject>("GJJFrame");
            await handle.ToUniTask();
            var _goHandle = handle.InstantiateAsync(gjj.transform);
            await _goHandle;
            var _go = _goHandle.Result;
            return _go;
        }




        public override void OnDestroy()
        {
            base.OnDestroy();

            if (frame != null)
                GameObject.Destroy(frame);
        }
    }
}