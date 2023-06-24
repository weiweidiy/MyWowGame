using Framework.EventKit;
using Framework.Extension;
using Framework.Helper;
using Logic.Common;
using Logic.Config;
using Logic.UI.Cells;
using TMPro;
using UnityEngine;

namespace Logic.UI.UIShop
{
    /// <summary>
    /// 限时商店
    /// </summary>
    public class PartTimeLimit : MonoBehaviour
    {
        //每日礼包
        public TextMeshProUGUI daySeconds;

        //每周礼包
        public TextMeshProUGUI weekSeconds;

        //钻石等类型礼包
        public CommonShopDiamond commonShopDiamond;
        public Transform weekContent;

        //其他
        private EventGroup eventGroup = new();

        private void Awake()
        {
            //事件注册
            eventGroup.Register(LogicEvent.TimeDaySecondsChanged, OnTimeDaySecondsChanged);
            eventGroup.Register(LogicEvent.TimeWeekSecondsChanged, OnTimeWeekSecondsChanged);
        }

        private void OnDestroy()
        {
            //事件注销
            eventGroup.Release();
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            InstantiateShopDiamond();
        }

        #region 每日礼包

        private void OnTimeDaySecondsChanged(int eventId, object data)
        {
            var leftSeconds = (int)data;
            daySeconds.text = TimeHelper.FormatSecond(leftSeconds);
        }

        #endregion

        #region 每周礼包

        private void OnTimeWeekSecondsChanged(int eventId, object data)
        {
            var leftSeconds = (int)data;
            weekSeconds.text = TimeHelper.FormatSecond(leftSeconds);
        }

        #endregion

        #region 钻石等类型礼包

        //生成钻石类型的礼包
        private void InstantiateShopDiamond()
        {
            //获取钻石礼包表中的全部数据
            var shopDiamondDic = ConfigManager.Ins.m_ShopDiamondCfg.AllData;
            foreach (var shopDiamondData in shopDiamondDic)
            {
                var item = Instantiate(commonShopDiamond, weekContent);
                item.Init(shopDiamondData.Value.ID);
                item.Show();
            }
        }

        #endregion
    }
}