using Framework.UI;
using Logic.Common;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.UIMain.Parts
{
    /// <summary>
    /// 主界面底部按钮控制
    /// </summary>
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Image))]
    public class UIBottomBtn : MonoBehaviour
    {
        [LabelText("底部按钮类型")]
        public BottomBtnType m_BtnType;
        [ReadOnly]
        public Button m_Button;
        [ReadOnly]
        public Image m_Sprite;
        [Space]
        public Sprite m_OnSprite;
        public Sprite m_OffSprite;

        private void Awake()
        {
            m_Sprite = GetComponent<Image>();
            m_Button = GetComponent<Button>();
            m_Button.onClick.AddListener(OnClick);
        }

        public async void OnClick()
        {
            m_Button.interactable = false;
            m_Sprite.sprite = m_OnSprite;
            m_Sprite.SetNativeSize();
            
            UIBottomMenu.Ins.OnClick(m_BtnType);

            switch (m_BtnType)
            {
                case BottomBtnType.User:
                    await UIManager.Ins.OpenUI<UIUser.UIUser>();
                    break;
                case BottomBtnType.Copy:
                    await UIManager.Ins.OpenUI<UICopy.UICopy>();
                    break;
                case BottomBtnType.Home:
                    break;
                case BottomBtnType.Special:
                    await UIManager.Ins.OpenUI<UISpecial.UISpecial>();
                    break;
                case BottomBtnType.Shop:
                    await UIManager.Ins.OpenUI<UIShop.UIShop>();
                    break;
            }
        }

        public void OnCancel()
        {
            m_Button.interactable = true;
            m_Sprite.sprite = m_OffSprite;
            m_Sprite.SetNativeSize();
            
            switch (m_BtnType)
            {
                case BottomBtnType.User:
                    UIManager.Ins.Hide<UIUser.UIUser>();
                    break;
                case BottomBtnType.Copy:
                    UIManager.Ins.Hide<UICopy.UICopy>();
                    break;
                case BottomBtnType.Home:
                    break;
                case BottomBtnType.Special:
                    UIManager.Ins.Hide<UISpecial.UISpecial>();
                    break;
                case BottomBtnType.Shop:
                    UIManager.Ins.Hide<UIShop.UIShop>();
                    break;
            }
        }
    }
}