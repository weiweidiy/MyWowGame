using Cysharp.Threading.Tasks;
using Framework.UI;
using Logic.Common;
using Logic.UI.Common;
using Logic.UI.UIMain.Parts;


namespace Logic.UI.UIMain
{
    public class UIBottomMenu : UIPage
    {
        public static UIBottomMenu Ins;
        public UIBottomBtn[] m_Btns;
        public UIBottomBtn m_HomeBtn;
        public BottomBtnType m_BottomBtnType;

        private void Awake()
        {
            Ins = this;
        }

        private async void OnEnable()
        {
            await UniTask.NextFrame();
            m_HomeBtn.OnClick();
        }

        /// <summary>
        /// 提供给外部 跳转接口
        /// </summary>
        /// <param name="pType"></param>
        public void ClickBtn(BottomBtnType pType)
        {
            m_BottomBtnType = pType;
            m_Btns[(int)pType].OnClick();
        }

        public void OnClick(BottomBtnType pType)
        {
            m_BottomBtnType = pType;
            for (int i = 0; i < m_Btns.Length; i++)
            {
                if (i != (int)pType)
                    m_Btns[i].OnCancel();
            }
        }
    }
}