using System;
using Configs;
using Framework.EventKit;
using Framework.Extension;
using Framework.Helper;
using Logic.Common;
using Logic.Manager;
using Logic.UI.Common;
using Networks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityTimer;

namespace Logic.UI.Cells
{
    public class CommonShop : MonoBehaviour
    {
        //数据
        public int ID;
        public ShopData shopData { get; private set; }
        public ShopGiftType shopGiftType { get; private set; }
        public ShopGiftTimeType shopGiftTimeType { get; private set; }
        public GameShopBuyData shopBuyData { get; private set; }
        public int buyCount { get; private set; }

        //视图
        public Image bgA; //底框A
        public Image bgB; //底框B
        public Image shopTag; //礼包标签

        public Image icon;
        public TextMeshProUGUI shopName;
        public CommonShopItem commonShopItem;
        public Transform content;
        public TextMeshProUGUI price;
        public TextMeshProUGUI value; //折扣价值

        public GameObject timer;
        public TextMeshProUGUI time;

        //其他
        private EventGroup eventGroup = new();
        private const int shopType = (int)ShopType.Gift;

        private void Awake()
        {
            //事件注册
            eventGroup.Register(LogicEvent.OnShopBuy, OnShopBuy);
            eventGroup.Register(LogicEvent.TimeDayChanged, (i, o) => OnTimeDayChanged());
            eventGroup.Register(LogicEvent.TimeWeekChanged, (i, o) => OnTimeWeekChanged());
            Init();
        }

        private void OnDestroy()
        {
            //事件注销
            eventGroup.Release();
        }

        //礼包初始化
        private void Init()
        {
            //获取客户端shop表数据
            shopData = ShopCfg.GetData(ID);
            if (shopData == null)
            {
                Debug.LogError($"/--Shop表中不存在ID:{ID}");
                return;
            }

            //获取服务器数据
            shopBuyData = ShopManager.Ins.GetGameShopBuyData(ID);

            //更新视图
            InstantiateShop();
            UpdateShop();
            RefreshShop();
        }

        //生成礼包物品
        private void InstantiateShop()
        {
            var itemList = shopData.ItemList;
            var countList = shopData.ItemCountList;
            var length = itemList.Count;
            for (var i = 0; i < length; i++)
            {
                var item = Instantiate(commonShopItem, content);
                item.Init(itemList[i], countList[i]);
                item.Show();
            }
        }

        //更新礼包相关信息
        private void UpdateShop()
        {
            //礼包类型
            shopGiftType = (ShopGiftType)shopData.Type;
            //礼包时间类型
            shopGiftTimeType = (ShopGiftTimeType)shopData.TimeType;
            //礼包Icon
            UpdateIcon();
            //礼包名称和次数
            UpdateNameAndCount(shopBuyData);
            //礼包价格
            UpdatePrice();
            //礼包折扣价值
            value.text = $"{shopData.Value}00%";
        }

        //更新礼包Icon
        private void UpdateIcon()
        {
            if (icon != null)
            {
                UICommonHelper.LoadIcon(icon, shopData.MainGood);
            }
        }

        //更新礼包名称和购买次数
        private void UpdateNameAndCount(GameShopBuyData gameShopBuyData)
        {
            buyCount = gameShopBuyData?.Count ?? 0;
            shopName.text = shopData.LimitCount > 0
                ? $"{shopData.Name}({buyCount}/{shopData.LimitCount})"
                : $"{shopData.Name}";
        }

        //礼包价格
        private void UpdatePrice()
        {
            //TODO:增加其他逻辑 1免费 4钻石
            price.text = shopData.PayType == 1 ? $"免费" : $"${shopData.PayNum}";
        }

        //刷新礼包相关状态
        private void RefreshShop()
        {
            RefreshTimer();
            RefreshValidity();
            RefreshBg();
            RefreshDiscount();
            RefreshTag();
        }

        //刷新礼包计时状态
        private void RefreshTimer()
        {
            if (timer == null) return;

            /* 获取相关时间
             * 服务器当前时间 serverTime
             * 礼包起始时间 beginTime
             * 礼包终止时间 endTime
             * 礼包持续时间 continueTime
             * 礼包剩余时间 leftSeconds
             */
            var serverTime = GameTimeManager.Ins.m_ServerTimes.ServerTimer;
            var beginTime = TimeHelper.GetUnixTimeStamp(shopData.BeginTime);
            var endTime = TimeHelper.GetUnixTimeStamp(shopData.EndTime);
            var continueTime = shopData.ContinueTime;
            int leftSeconds;

            switch (shopGiftTimeType)
            {
                //无时间
                case ShopGiftTimeType.None:
                    timer.Hide();
                    break;
                //固定时间间隔 读 起始时间 终止时间
                case ShopGiftTimeType.Fixed:
                    //如果不在礼包购买时限内隐藏该礼包
                    if (serverTime < beginTime || serverTime > endTime)
                    {
                        this.gameObject.Hide();
                    }
                    else
                    {
                        timer.Show();
                        leftSeconds = (int)(endTime - serverTime);
                        StartTimer(leftSeconds);
                    }

                    break;
                //无时间限制 读 起始时间
                case ShopGiftTimeType.Start:
                    if (serverTime < beginTime)
                    {
                        this.gameObject.Hide();
                    }
                    else
                    {
                        timer.Hide();
                    }

                    break;
                //解锁条件 持续时间后终止 读 持续时间
                case ShopGiftTimeType.UnLockTime:
                    //TODO:添加解锁条件，达到解锁条件后显示计时
                    this.gameObject.Hide();
                    // timer.Show();
                    // leftSeconds = continueTime;
                    // StartTimer(leftSeconds);
                    break;
                //解锁条件 无时间限制
                case ShopGiftTimeType.UnLock:
                    timer.Hide();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        //刷新礼包显示有效性
        private void RefreshValidity()
        {
            if (!shopData.Validity)
            {
                this.Hide();
            }
        }

        //刷新礼包底框状态
        private void RefreshBg()
        {
            if (shopGiftType == ShopGiftType.Once)
            {
                UICommonHelper.LoadGiftBgA(bgA, shopData.Background);
                UICommonHelper.LoadGiftBgB(bgB, shopData.Background);
            }
            else
            {
                UICommonHelper.LoadTimeGiftBgA(bgA, shopData.Background);
                UICommonHelper.LoadTimeGiftBgB(bgB, shopData.Background);
            }
        }

        //刷新礼包折扣标签状态
        private void RefreshDiscount()
        {
            if (shopData.DiscountTag != 1)
            {
                value.transform.parent.Hide();
            }
        }

        //刷新礼包标签状态
        private void RefreshTag()
        {
            //一次性礼包标签
            // 0不显示 1新增 2人气 3特惠 4限时
            if (shopGiftType == ShopGiftType.Once)
            {
                if (shopData.StatusTag == 0)
                {
                    shopTag.Hide();
                }
                else
                {
                    UICommonHelper.LoadGiftTag(shopTag, shopData.StatusTag);
                    shopTag.Show();
                }
            }
            else
            {
                //限时礼包增加标签
            }
        }

        //是否可以购买礼包
        private bool IsCanBuy()
        {
            // 不限购
            if (shopData.LimitCount == 0)
            {
                return true;
            }

            //限购
            return buyCount < shopData.LimitCount;
        }

        //启用计时器
        private void StartTimer(int leftSeconds)
        {
            time.text = TimeHelper.FormatSecond(leftSeconds);
            Timer.Register(1f, () =>
            {
                leftSeconds--;
                time.text = TimeHelper.FormatSecond(leftSeconds);
                if (leftSeconds <= 0)
                {
                    this.gameObject.Hide();
                }
            }, isLooped: true, useRealTime: true);
        }

        //接收礼包购买完成消息
        private void OnShopBuy(int eventId, object data)
        {
            if (data is not S2C_ShopBuy pMsg) return;
            if (pMsg.Data?.Type != shopType) return;
            if (pMsg.Data.ID == ID)
            {
                UpdateNameAndCount(pMsg.Data);
            }
        }

        //接收商店跨天变化
        private void OnTimeDayChanged()
        {
            if (shopGiftType != ShopGiftType.Day) return;
            UpdateNameAndCount(null);
        }

        //接收商店跨周变化
        private void OnTimeWeekChanged()
        {
            if (shopGiftType != ShopGiftType.Week) return;
            UpdateNameAndCount(null);
        }

        //点击购买按钮
        public void OnBtnBuyClick()
        {
            //TODO:点击购买后进入其他流程，完成后进入购买流程
            if (IsCanBuy())
            {
                ShopManager.Ins.DoShopBuy(ID, ShopType.Gift);
            }
            else
            {
                EventManager.Call(LogicEvent.ShowTips, "超出购买次数");
            }
        }
    }
}