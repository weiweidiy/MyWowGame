using Framework.Extension;
using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.UIShop
{
    public enum ShopPageType
    {
        None = -1,
        DrawCard = 0,
        TimeLimit,
        GameGift,
        Special,
    }

    public class UIShop : UIPage
    {
        public GameObject DrawCardNode;
        public GameObject TimeLimitNode;
        public GameObject GameGiftNode;
        public GameObject SpecialNode;

        public Toggle[] Toggles;

        public static ShopPageType m_OpenType = ShopPageType.None;

        public override void OnShow()
        {
            if (m_OpenType == ShopPageType.None) return;
            Toggles[(int)m_OpenType].isOn = true;
            m_OpenType = ShopPageType.None;
        }


        public void OnToggleDrawCard(bool pOn)
        {
            if (pOn)
            {
                DrawCardNode.Show();
            }
            else
            {
                DrawCardNode.Hide();
            }
        }

        public void OnToggleTimeLimit(bool pOn)
        {
            if (pOn)
            {
                TimeLimitNode.Show();
            }
            else
            {
                TimeLimitNode.Hide();
            }
        }

        public void OnToggleGameGift(bool pOn)
        {
            if (pOn)
            {
                GameGiftNode.Show();
            }
            else
            {
                GameGiftNode.Hide();
            }
        }

        public void OnToggleSpecial(bool pOn)
        {
            if (pOn)
            {
                SpecialNode.Show();
            }
            else
            {
                SpecialNode.Hide();
            }
        }
    }
}