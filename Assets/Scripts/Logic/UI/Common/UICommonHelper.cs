using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Logic.Common;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

namespace Logic.UI.Common
{
    /// <summary>
    /// UI通用逻辑处理
    /// </summary>
    public static class UICommonHelper
    {
        /// <summary>
        /// 加载Icon资源
        ///
        /// 这里的资源没有释放
        /// 
        /// </summary>
        public static async void LoadIcon(Image pImage, string pIconPath)
        {
            var _Handle = YooAssets.LoadAssetAsync<Sprite>(pIconPath);
            await _Handle.ToUniTask();
            pImage.sprite = _Handle.AssetObject as Sprite;
        }

        /// <summary>
        /// 加载Icon资源 可等待
        /// </summary>
        /// <param name="pImage"></param>
        /// <param name="pIconPath"></param>
        /// <returns></returns>
        public static async UniTask<bool> LoadIconAsync(Image pImage, string pIconPath)
        {
            var _Handle = YooAssets.LoadAssetAsync<Sprite>(pIconPath);
            await _Handle.ToUniTask();
            pImage.sprite = _Handle.AssetObject as Sprite;
            return true;
        }


        /// <summary>
        /// 加载装备品质框资源
        /// </summary>
        /// <param name="pImage"></param>
        /// <param name="pQuality"></param>
        public static void LoadQuality(Image pImage, int pQuality)
        {
            pImage.sprite = UICommonSprites.Ins.m_ItemQualityBg[pQuality];
        }

        /// <summary>
        /// 加载技能品质框资源
        /// </summary>
        /// <param name="pImage"></param>
        /// <param name="pQuality"></param>
        public static void LoadSkillQuality(Image pImage, int pQuality)
        {
            pImage.sprite = UICommonSprites.Ins.m_SkillQuality[pQuality];
        }

        /// <summary>
        /// 加载伙伴品质框资源
        /// </summary>
        /// <param name="pImage"></param>
        /// <param name="pQuality"></param>
        public static void LoadPartnerQuality(Image pImage, int pQuality)
        {
            pImage.sprite = UICommonSprites.Ins.m_PartnerQuality[pQuality];
        }

        public static string GetQualityShowText(int pQuality)
        {
            var _QualityType = (ItemQuality)pQuality;
            return _QualityType switch
            {
                ItemQuality.White => @"<color=#C7C4B6>普通</color>",
                ItemQuality.Green => @"<color=#B4DF6F>高级</color>",
                ItemQuality.Blue => @"<color=#27BAEB>稀有</color>",
                ItemQuality.Purple => @"<color=#EB27E9>史诗</color>",
                ItemQuality.Yellow => @"<color=#FA8F27>传说</color>",
                ItemQuality.Cyan => @"<color=#56ECAE>神话</color>",
                ItemQuality.Red => @"<color=#E84444>超越</color>",
                _ => ""
            };
        }

        /// <summary>
        /// 装备类型图标
        /// </summary>
        public static void LoadEquipType(Image pImage, int pEquipType)
        {
            pImage.sprite = UICommonSprites.Ins.m_EquipType[pEquipType];
        }

        /// <summary>
        /// 通过关卡ID 获取关卡名称
        /// </summary>
        public static string GetLevelNameByID(long pLevelID)
        {
            var _M = ((pLevelID - 1) / 10) % 10 + 1;
            var _N = (pLevelID - 1) % 10 + 1;
            string _XX = "";
            switch (pLevelID)
            {
                case <= 100:
                    _XX = "简单";
                    break;
                case <= 200:
                    _XX = "<color=#FDD88C>困难</color>";
                    break;
                case <= 300:
                    _XX = "<color=#FFAA00>极难Ⅰ</color>";
                    break;
                case <= 400:
                    _XX = "<color=#FFAA00>极难Ⅱ</color>";
                    break;
                case <= 500:
                    _XX = "<color=#FFAA00>极难Ⅲ</color>";
                    break;
                case <= 600:
                    _XX = "<color=#FFAA00>极难Ⅳ</color>";
                    break;
                case <= 700:
                    _XX = "<color=#FFAA00>极难Ⅴ</color>";
                    break;
                case <= 800:
                    _XX = "<color=#FFAA00>地狱Ⅰ</color>";
                    break;
                case <= 900:
                    _XX = "<color=#FFAA00>地狱Ⅱ</color>";
                    break;
                case <= 1000:
                    _XX = "<color=#FFAA00>地狱Ⅲ</color>";
                    break;
                case <= 1100:
                    _XX = "<color=#FFAA00>地狱Ⅳ</color>";
                    break;
                case <= 1200:
                    _XX = "<color=#FFAA00>地狱Ⅴ</color>";
                    break;
                default:
                    return "<color=#FF1C00>第" + pLevelID + "关</color>";
            }

            return _XX + _M + "-" + _N;
        }


        private static Dictionary<MiningType, Sprite> m_MiningTypeSprites = new Dictionary<MiningType, Sprite>()
        {
            { MiningType.WeaponTreasure, UICommonSprites.Ins.m_MiningType[0] },
            { MiningType.ArmorTreasure, UICommonSprites.Ins.m_MiningType[1] },
            { MiningType.EquipTreasure, UICommonSprites.Ins.m_MiningType[2] },
            { MiningType.RandomTreasure, UICommonSprites.Ins.m_MiningType[3] },
            { MiningType.Coin, UICommonSprites.Ins.m_MiningType[4] },
            { MiningType.BigCoin, UICommonSprites.Ins.m_MiningType[5] },
            { MiningType.Honor, UICommonSprites.Ins.m_MiningType[6] },
            { MiningType.Gear, UICommonSprites.Ins.m_MiningType[7] },
            { MiningType.CopperMine, UICommonSprites.Ins.m_MiningType[8] },
            { MiningType.SilverMine, UICommonSprites.Ins.m_MiningType[9] },
            { MiningType.GoldMine, UICommonSprites.Ins.m_MiningType[10] },
            { MiningType.Diamond, UICommonSprites.Ins.m_MiningType[11] },
            { MiningType.Hammer, UICommonSprites.Ins.m_MiningType[12] },
            { MiningType.Bomb, UICommonSprites.Ins.m_MiningType[13] },
            { MiningType.Scope, UICommonSprites.Ins.m_MiningType[14] },
        };

        /// <summary>
        /// 考古道具图标
        /// </summary>
        /// <param name="pImage"></param>
        /// <param name="treasureType"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void LoadMiningType(Image pImage, MiningType treasureType)
        {
            if (m_MiningTypeSprites.ContainsKey(treasureType))
            {
                pImage.sprite = m_MiningTypeSprites[treasureType];
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(treasureType), treasureType, null);
            }
        }

        /// <summary>
        /// 淬炼系统品质
        /// 0-F 1-E 2-D 3-C 4-B 5-A 6-S 7-SS
        /// </summary>
        /// <param name="pImage"></param>
        /// <param name="qualityType"></param>
        public static void LoadQuenchingQuality(Image pImage, int qualityType)
        {
            pImage.sprite = UICommonSprites.Ins.m_QuenchingQuality[qualityType];
        }

        /// <summary>
        /// 礼包底框A
        /// </summary>
        /// <param name="pImage"></param>
        /// <param name="type"></param>
        public static void LoadTimeGiftBgA(Image pImage, int type)
        {
            pImage.sprite = UICommonSprites.Ins.m_TimeGiftBgA[type];
        }

        /// <summary>
        /// 限时礼包底框B
        /// </summary>
        /// <param name="pImage"></param>
        /// <param name="type"></param>
        public static void LoadTimeGiftBgB(Image pImage, int type)
        {
            pImage.sprite = UICommonSprites.Ins.m_TimeGiftBgB[type];
        }

        /// <summary>
        /// 一次性礼包底框A
        /// </summary>
        /// <param name="pImage"></param>
        /// <param name="type"></param>
        public static void LoadGiftBgA(Image pImage, int type)
        {
            pImage.sprite = UICommonSprites.Ins.m_GiftBgA[type];
        }

        /// <summary>
        /// 一次性礼包底框B
        /// </summary>
        /// <param name="pImage"></param>
        /// <param name="type"></param>
        public static void LoadGiftBgB(Image pImage, int type)
        {
            pImage.sprite = UICommonSprites.Ins.m_GiftBgB[type];
        }

        /// <summary>
        /// 一次性礼包标签
        /// </summary>
        /// <param name="pImage"></param>
        /// <param name="type"></param>
        public static void LoadGiftTag(Image pImage, int type)
        {
            pImage.sprite = UICommonSprites.Ins.m_GiftTag[type];
        }
    }
}