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

        [LabelText("装备品质")] public Sprite[] m_ItemQualityBg;

        [LabelText("技能品质")] public Sprite[] m_SkillQuality;

        [LabelText("伙伴品质")] public Sprite[] m_PartnerQuality;

        [LabelText("装备类型")] public Sprite[] m_EquipType;

        [LabelText("考古道具类型")] public Sprite[] m_MiningType;

        [LabelText("按钮状态")] public Sprite[] m_ButtonState;

        [LabelText("品质")] public Sprite[] m_QuenchingQuality;

        [LabelText("限时礼包底框A")] public Sprite[] m_TimeGiftBgA;
        [LabelText("限时礼包底框B")] public Sprite[] m_TimeGiftBgB;
        [LabelText("一次性礼包底框A")] public Sprite[] m_GiftBgA;
        [LabelText("一次性礼包底框B")] public Sprite[] m_GiftBgB;
        [LabelText("一次性礼包状态标签")] public Sprite[] m_GiftTag;
    }
}