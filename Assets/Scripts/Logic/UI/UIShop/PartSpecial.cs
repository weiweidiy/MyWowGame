using System.Collections.Generic;
using System.Linq;
using Logic.UI.Cells;
using UnityEngine;

namespace Logic.UI.UIShop
{
    public class PartSpecial : MonoBehaviour
    {
        public Transform content;
        private List<CommonShop> orderList = new List<CommonShop>();

        private void Awake()
        {
            orderList = content.GetComponentsInChildren<CommonShop>().ToList();
        }

        private void Start()
        {
            //对特殊商店面板中的礼包进行排序
            Order();
        }

        /// <summary>
        /// 特殊商店排序
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