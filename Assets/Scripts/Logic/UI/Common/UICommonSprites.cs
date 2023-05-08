using Framework.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Logic.UI.Common
{
    /// <summary>
    /// 全局一些通用图片资源 直接挂载在这里方便其他逻辑使用
    /// eg: 道具类型, 品质背景框 等
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public class UICommonSprites : UIPage
    {
        public static UICommonSprites Ins;

        private void Awake()
        {
            Ins = this;
        }

        [LabelText("品质背景")] public Sprite[] m_ItemQualityBg;

        [LabelText("装备类型")] public Sprite[] m_EquipType;

        [LabelText("考古道具类型")] public Sprite[] m_MiningType;
    }
}