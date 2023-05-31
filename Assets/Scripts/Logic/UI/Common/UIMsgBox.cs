using Framework.Extension;
using Framework.UI;
using TMPro;
using UnityEngine;

namespace Logic.UI.Common
{
    /// <summary>
    /// 通用弹出框
    /// </summary>
    public class UIMsgBox : UIPage
    {
        /// <summary>
        /// 打开通用的对话框
        /// type = 1 : 只显示确定按钮
        /// type = 2 : 显示确定和取消按钮
        /// </summary>
        public static async void ShowMsgBox(int type, string title, string content, string confirmText = "",
            string cancelText = "", System.Action confirmAction = null, System.Action cancelAction = null)
        {
            UIMsgBoxData data = new UIMsgBoxData
            {
                m_OpenType = type,
                m_Title = title,
                m_Content = content,
                m_ConfirmText = confirmText,
                m_CancelText = cancelText,
                m_ConfirmAction = confirmAction,
                m_CancelAction = cancelAction
            };
            await UIManager.Ins.OpenUI<UIMsgBox>(data);
        }

        private UIMsgBoxData _Data;

        public override void OnShow()
        {
            _Data = (UIMsgBoxData)m_OpenData_;
            if (_Data != null)
            {
                m_Title.text = _Data.m_Title;
                m_Content.text = _Data.m_Content;
                if (!string.IsNullOrWhiteSpace(_Data.m_ConfirmText))
                    m_ConfirmText.text = _Data.m_ConfirmText;
                if (!string.IsNullOrWhiteSpace(_Data.m_CancelText))
                    m_CancelText.text = _Data.m_CancelText;
                switch (_Data.m_OpenType)
                {
                    case 1:
                        m_BtnConfirm.Show();
                        m_BtnCancel.Hide();
                        break;
                    case 2:
                        m_BtnConfirm.Show();
                        m_BtnCancel.Show();
                        break;
                }
            }
        }

        public TextMeshProUGUI m_Title;
        public TextMeshProUGUI m_Content;
        public GameObject m_BtnConfirm;
        public TextMeshProUGUI m_ConfirmText;
        public GameObject m_BtnCancel;
        public TextMeshProUGUI m_CancelText;

        public void OnClickConfirm()
        {
            _Data?.m_ConfirmAction?.Invoke();
            Hide();
        }

        public void OnClickCancel()
        {
            _Data?.m_CancelAction?.Invoke();
            Hide();
        }
    }

    /// <summary>
    /// 打开UIMsgBox的参数
    /// </summary>
    public class UIMsgBoxData
    {
        public int m_OpenType;
        public string m_Title;
        public string m_Content;
        public string m_ConfirmText;
        public string m_CancelText;
        public System.Action m_ConfirmAction;
        public System.Action m_CancelAction;
    }
}