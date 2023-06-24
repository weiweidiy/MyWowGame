using System.Collections;
using System.Collections.Generic;
using Configs;
using Framework.Extension;
using Framework.UI;
using Logic.Common;
using Logic.UI.Cells;
using Logic.UI.Common;
using Networks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.UIShop
{
    public class UIShopObtain : UIPage
    {
        public CommonShopItem commonShopItem;
        public Transform content;
        private List<CommonShopItem> commonShopItemList = new List<CommonShopItem>();
        public Image icon;
        public TextMeshProUGUI count;

        private bool isOnCoroutine;
        private WaitForSeconds delay;

        private ShopData shopData;
        private ShopDiamondData shopDiamondData;

        private void Awake()
        {
            delay = new WaitForSeconds(0.02f);
            icon.gameObject.Hide();
        }

        public override void OnShow()
        {
            base.OnShow();
            StartObtainCoroutine();
        }

        private void StartObtainCoroutine()
        {
            if (m_OpenData_ is not GameShopBuyData gameShopBuyData) return;
            if ((ShopType)gameShopBuyData.Type == ShopType.Diamond)
            {
                shopDiamondData = ShopDiamondCfg.GetData(gameShopBuyData.ID);
                if (shopDiamondData == null)
                {
                    Debug.LogError($"/--ShopDiamond表中不存在ID:{gameShopBuyData.ID}");
                }
                else
                {
                    ObtainSingle(shopDiamondData, gameShopBuyData.Count);
                }
            }
            else
            {
                shopData = ShopCfg.GetData(gameShopBuyData.ID);
                if (shopData == null)
                {
                    Debug.LogError($"/--Shop表中不存在ID:{gameShopBuyData.ID}");
                }
                else
                {
                    StartCoroutine(ObtainCoroutine());
                }
            }
        }

        private IEnumerator ObtainCoroutine()
        {
            isOnCoroutine = true;
            var itemList = shopData.ItemList;
            var countList = shopData.ItemCountList;
            var length = itemList.Count;
            for (var i = 0; i < length; i++)
            {
                var item = Instantiate(commonShopItem, content);
                item.Init(itemList[i], countList[i]);
                item.DoPunchScale();
                commonShopItemList.Add(item);
                item.Show();
                yield return delay;
            }

            isOnCoroutine = false;
        }

        private void ObtainSingle(ShopDiamondData data, int buyCount)
        {
            UICommonHelper.LoadIcon(icon, data.MainGood);
            count.text = buyCount > 1 ? $"x{data.Reward}" : $"x{data.Reward + data.ExReward}";
            icon.gameObject.Show();
        }

        public void OnClickCloseBtn()
        {
            if (isOnCoroutine) return;

            foreach (var item in commonShopItemList)
            {
                Destroy(item.gameObject);
            }

            commonShopItemList.Clear();
            icon.gameObject.Hide();

            Hide();
        }
    }
}