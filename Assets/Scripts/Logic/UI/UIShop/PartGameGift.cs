using System.Collections.Generic;
using System.Linq;
using Logic.UI.Cells;
using UnityEngine;

namespace Logic.UI.UIShop
{
    public class PartGameGift : MonoBehaviour
    {
        public Transform content;
        private List<CommonShop> orderList = new List<CommonShop>();

        private void Awake()
        {
            orderList = content.GetComponentsInChildren<CommonShop>().ToList();
        }

        private void Start()
        {
            //对礼包商店面板中的礼包进行排序
            Order();
        }

        /// <summary>
        /// 礼包商店排序
        /// </summary>
        private void Order()
        {
            orderList = orderList.OrderBy(x => x.shopData?.Order).ToList();
            foreach (var commonShop in orderList)
            {
                commonShop.transform.SetAsLastSibling();
            }
        }
    }
}