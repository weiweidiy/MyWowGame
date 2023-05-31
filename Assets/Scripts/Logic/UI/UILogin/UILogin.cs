
using DG.Tweening;
using Framework.Core;
using Framework.UI;
using UnityEngine;
using Logic.Common;
using Logic.Data;
using Logic.States.Game;
using Logic.UI.Common;
using Networks;
using TMPro;
using UnityEngine.UI;

namespace Logic.UI.UILogin
{
    public class UILogin : UIPage
    {
        public RectTransform BtnClear;
        public GameObject m_InputAccount;
        public TMP_InputField InputAccount;
        
        private void Awake()
        {
            m_EventGroup.Register(LogicEvent.ConnectSuccess, (i, o) => GameSM.Ins.ToLoading());

            m_InputAccount.SetActive(!GameCore.Ins.UseDummyServer);

            if (!string.IsNullOrWhiteSpace(LocalSaveManager.Ins.LocalData.Account))
            {
                InputAccount.text = LocalSaveManager.Ins.LocalData.Account;
            }
        }

        private void Start()
        {
            BtnClear.DOAnchorPosY(30, 0.5f).SetEase(Ease.OutBack).SetDelay(1.5f);
        }

        public void OnCLickEnterGame()
        {
            if (!GameCore.Ins.UseDummyServer && string.IsNullOrWhiteSpace(LocalSaveManager.Ins.LocalData.Account))
            {
                UIMsgBox.ShowMsgBox(1, "错误", "请输入账号");
                return;
            }
            
            //链接服务器
            //TODO 当前先这里配置 写死地址
            NetworkManager.Ins.ConnectTo(GameCore.Ins.ServerAddr);
        }
        
        public void AccountInput(string pAccount)
        {
            if (!string.IsNullOrWhiteSpace(pAccount))
            {
                LocalSaveManager.Ins.LocalData.Account = pAccount;
                LocalSaveManager.Ins.Save();
            }
        }
        
        public void OnClickClearFile()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
