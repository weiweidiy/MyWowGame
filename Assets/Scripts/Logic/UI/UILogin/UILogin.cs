
using DG.Tweening;
using Framework.UI;
using UnityEngine;
using Logic.Common;
using Logic.States.Game;
using Networks;

namespace Logic.UI.UILogin
{
    public class UILogin : UIPage
    {
        public RectTransform BtnClear;
        
        private void Awake()
        {
            m_EventGroup.Register(LogicEvent.ConnectSuccess, (i, o) => GameSM.Ins.ToLoading());
        }

        private void Start()
        {
            BtnClear.DOAnchorPosY(30, 0.5f).SetEase(Ease.OutBack).SetDelay(1.5f);
        }

        public void OnCLickEnterGame()
        {
            //链接服务器
            NetworkManager.Ins.ConnectTo("TODO TEST");
        }
        
        public void OnClickClearFile()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
