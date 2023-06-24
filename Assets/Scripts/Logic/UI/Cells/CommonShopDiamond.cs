using Configs;
using Framework.EventKit;
using Framework.Extension;
using Logic.Common;
using Logic.Manager;
using Logic.UI.Common;
using Networks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.Cells
{
    public class CommonShopDiamond : MonoBehaviour
    {
        //数据
        public int ID { get; private set; }
        public ShopDiamondData shopDiamondData { get; private set; }

        //视图
        public TextMeshProUGUI shopName;
        public GameObject tip;
        public Image icon;
        public TextMeshProUGUI count;
        public TextMeshProUGUI price;
        public TextMeshProUGUI exReward;

        //其他
        private EventGroup eventGroup = new();
        private const int shopType = (int)ShopType.Diamond;

        private void Awake()
        {
            //注册
            eventGroup.Register(LogicEvent.OnShopBuy, OnShopBuy);
            eventGroup.Register(LogicEvent.TimeWeekChanged, (i, o) => OnTimeWeekChanged());
        }

        private void OnDestroy()
        {
            //注销
            eventGroup.Release();
        }

        //钻石礼包初始化
        public void Init(int id)
        {
            ID = id;
            shopDiamondData = ShopDiamondCfg.GetData(id);
            if (shopDiamondData == null)
            {
                Debug.LogError($"/--ShopDiamond表中不存在ID:{ID}");
                return;
            }

            UpdateShopDiamond();
            UpdateShopDiscount();
        }

        //更新钻石礼包相关信息
        private void UpdateShopDiamond()
        {
            shopName.text = shopDiamondData.Name;
            UICommonHelper.LoadIcon(icon, shopDiamondData.MainGood);
            count.text = $"x{shopDiamondData.Reward}";
            // price.text = $"￥{shopDiamondData.Price}";
            price.text = $"${shopDiamondData.Price}";
            exReward.text = $"+{shopDiamondData.ExReward}";
        }

        private void UpdateShopDiscount()
        {
            var gameShopBuyData = ShopManager.Ins.GetGameShopBuyData(ID);
            RefreshShopDiscount(gameShopBuyData?.Count > 0);
        }

        //刷新折扣相关状态
        private void RefreshShopDiscount(bool isBuy)
        {
            if (isBuy)
            {
                tip.Hide();
                exReward.gameObject.Hide();
            }
            else
            {
                tip.Show();
                exReward.gameObject.Show();
            }
        }

        //接收钻石礼包购买完成消息
        private void OnShopBuy(int eventId, object data)
        {
            if (data is not S2C_ShopBuy pMsg) return;
            if (pMsg.Data?.Type != shopType) return;
            if (pMsg.Data.ID == ID)
            {
                RefreshShopDiscount(pMsg.Data.Count > 0);
            }
        }

        //接收商店跨周变化
        private void OnTimeWeekChanged()
        {
            RefreshShopDiscount(false);
        }

        //点击购买按钮
        public void OnBtnBuyClick()
        {
            //TODO:点击购买后进入其他流程，完成后进入购买流程
            ShopManager.Ins.DoShopBuy(ID, ShopType.Diamond);
        }
    }
}