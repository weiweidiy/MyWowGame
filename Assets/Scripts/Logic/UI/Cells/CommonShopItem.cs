using BreakInfinity;
using Configs;
using DG.Tweening;
using Logic.UI.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.Cells
{
    public class CommonShopItem : MonoBehaviour
    {
        public Image quality;
        public Image icon;
        public TextMeshProUGUI count;

        public ItemData itemData { get; private set; }

        //获取动画参数
        private float duration = 0.1f;
        private readonly Vector3 strength = new Vector3(1.3f, 1.3f, 1f);

        public void Init(int pId, int pCount)
        {
            itemData = ItemCfg.GetData(pId);
            if (itemData == null)
            {
                Debug.LogError($"/--Item表中不存在ID:{pId}");
                return;
            }

            UICommonHelper.LoadQuality(quality, itemData.Quality);

            if (itemData.Res != string.Empty)
            {
                UICommonHelper.LoadIcon(icon, itemData.Res);
            }
            else
            {
                Debug.LogError($"/--Item表中不存在ID:{pId}的Res");
            }

            //特殊商品显示处理
            BigDouble bigCount = pCount;
            count.text = bigCount.ToUIString();
        }

        //获取动画
        public void DoPunchScale()
        {
            transform.DOPunchScale(strength, duration);
        }
    }
}