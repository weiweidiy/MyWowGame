using System.Collections.Generic;
using System.Linq;
using BreakInfinity;
using Configs;
using Framework.EventKit;
using Framework.Extension;
using Logic.Common;
using Logic.Data;
using Networks;

namespace Logic.Manager
{
    public class QuenchingManager : Singleton<QuenchingManager>
    {
        public Dictionary<int, GameQuenchingData> QuenchingMap { get; private set; }

        /* 淬炼属性
         * 攻击力
         * 体力
         * 多连射
         * 攻击速度
         * 暴击率
         * 暴击伤害
         * 技能伤害
         * 同伴伤害
         * 金币获取量
         */
        public float QuenchingATK { get; private set; }
        public float QuenchingHP { get; private set; }
        public float QuenchingMultipleShot { get; private set; }
        public float QuenchingSpeed { get; private set; }
        public float QuenchingCritical { get; private set; }
        public float QuenchingCriticalDamage { get; private set; }
        public float QuenchingSkillDamage { get; private set; }
        public float QuenchingCompanionDamage { get; private set; }
        public float QuenchingGoldObtain { get; private set; }

        /* 淬炼组合属性
         * 体力
         * 体力恢复上限
         * 攻击力
         * 暴击伤害
         * 闪避率
         */
        public float QuenchingGroupHP { get; private set; }
        public float QuenchingGroupHPRecoverEverySecond { get; private set; }
        public float QuenchingGroupATK { get; private set; }
        public float QuenchingGroupCriticalDamage { get; private set; }
        public float QuenchingGroupEvasionRate { get; private set; }


        //淬炼攻击力增加
        public BigDouble GetQuenchingATKAdd()
        {
            return QuenchingATK + QuenchingGroupATK;
        }

        //淬炼体力增加
        public BigDouble GetQuenchingHPAdd()
        {
            return QuenchingHP + QuenchingGroupHP;
        }

        public void Init(List<GameQuenchingData> quenchingList)
        {
            QuenchingMap = new Dictionary<int, GameQuenchingData>(64);
            foreach (var quenchingData in quenchingList)
            {
                QuenchingMap.Add(quenchingData.QuenchingId, quenchingData);
            }

            // 数据更新
            UpdateAllQuenchingEffect();
        }

        //更新淬炼词条属性加成
        private void UpdateAllQuenchingEffect()
        {
            /*
             * 攻击力
             * 体力
             * 多连射
             * 攻击速度
             * 暴击率
             * 暴击伤害
             * 技能伤害
             * 同伴伤害
             * 金币获取量
             */
            var quenchingAttributes = new Dictionary<AttributeType, float>();

            foreach (var quenchingMapValue in QuenchingMap.Values)
            {
                var attributeData = GetAttributeData(quenchingMapValue.AttributeId);
                var attributeType = attributeData.Type;
                var attributeValue = attributeData.Value;

                if (quenchingAttributes.ContainsKey((AttributeType)attributeType))
                {
                    quenchingAttributes[(AttributeType)attributeType] += attributeValue;
                }
                else
                {
                    quenchingAttributes.Add((AttributeType)attributeType, attributeValue);
                }
            }

            QuenchingATK = CalculateQuenchingAttr(AttributeType.ATK, quenchingAttributes);
            QuenchingHP = CalculateQuenchingAttr(AttributeType.HP, quenchingAttributes);
            QuenchingMultipleShot = CalculateQuenchingAttr(AttributeType.MultipleShot, quenchingAttributes);
            QuenchingSpeed = quenchingAttributes.TryGetValue(AttributeType.Speed, out var speed) ? speed : 0;
            QuenchingCritical = CalculateQuenchingAttr(AttributeType.Critical, quenchingAttributes);
            QuenchingCriticalDamage = CalculateQuenchingAttr(AttributeType.CriticalDamage, quenchingAttributes);
            QuenchingSkillDamage = CalculateQuenchingAttr(AttributeType.SkillDamage, quenchingAttributes);
            QuenchingCompanionDamage = CalculateQuenchingAttr(AttributeType.CompanionDamage, quenchingAttributes);
            QuenchingGoldObtain = CalculateQuenchingAttr(AttributeType.GoldObtain, quenchingAttributes);

            UpdateAllQuenchingGroupEffect();

            EventManager.Call(LogicEvent.QuenchingEffectUpdate);
        }

        //更新淬炼词条组合属性加成
        private void UpdateAllQuenchingGroupEffect()
        {
            /*
             * 体力
             * 体力恢复上限
             * 攻击力
             * 暴击伤害
             * 闪避率
             */

            var quenchingLevels = new Dictionary<QuenchingType, int>
            {
                { QuenchingType.Do, 0 },
                { QuenchingType.Re, 0 },
                { QuenchingType.Mi, 0 },
                { QuenchingType.Fa, 0 },
                { QuenchingType.Sol, 0 },
            };

            var quenchingMapValues = QuenchingMap.Values;
            foreach (var quenchingType in quenchingMapValues.Select(quenchingData =>
                         (QuenchingType)quenchingData.MelodyId))
            {
                quenchingLevels[quenchingType]++;
            }

            var quenchingTypeToDjComboDataParam = new Dictionary<QuenchingType, int>
            {
                { QuenchingType.Do, 1 },
                { QuenchingType.Re, 2 },
                { QuenchingType.Mi, 3 },
                { QuenchingType.Fa, 4 },
                { QuenchingType.Sol, 5 },
            };

            var quenchingGroupAttrs = new Dictionary<AttributeType, float>();
            foreach (var quenchingLevel in quenchingLevels)
            {
                var quenchingType = quenchingLevel.Key;
                var djComboDataParam = quenchingTypeToDjComboDataParam[quenchingType];
                var djComboData = quenchingLevel.Value switch
                {
                    5 => GetDJComboData(djComboDataParam * 10 + 3),
                    >= 3 and < 5 => GetDJComboData(djComboDataParam * 10 + 2),
                    >= 2 and < 3 => GetDJComboData(djComboDataParam * 10 + 1),
                    _ => null
                };
                if (djComboData != null)
                {
                    var attributeData = GetAttributeData(djComboData.AttributeID);
                    var attributeType = (AttributeType)attributeData.Type;
                    var attributeValue = attributeData.Value;
                    quenchingGroupAttrs.Add(attributeType, attributeValue);
                }
            }

            QuenchingGroupHP = CalculateQuenchingAttr(AttributeType.HP, quenchingGroupAttrs);
            QuenchingGroupHPRecoverEverySecond =
                CalculateQuenchingAttr(AttributeType.HPRecoverEverySecond, quenchingGroupAttrs);
            QuenchingGroupATK = CalculateQuenchingAttr(AttributeType.ATK, quenchingGroupAttrs);
            QuenchingGroupCriticalDamage = CalculateQuenchingAttr(AttributeType.CriticalDamage, quenchingGroupAttrs);
            QuenchingGroupEvasionRate = CalculateQuenchingAttr(AttributeType.EvasionRate, quenchingGroupAttrs);
        }

        private float CalculateQuenchingAttr(AttributeType attributeType,
            IDictionary<AttributeType, float> quenchingGroupAttrs)
        {
            return quenchingGroupAttrs.TryGetValue(attributeType, out var attributeValue) ? attributeValue / 100f : 0;
        }

        #region 通用

        public DJDesData GetDjDesData(int dId)
        {
            return DJDesCfg.GetData(dId);
        }

        public DJComboData GetDJComboData(int dId)
        {
            return DJComboCfg.GetData(dId);
        }

        public DJData GetDJData(int dId)
        {
            return DJCfg.GetData(dId);
        }

        public AttributeData GetAttributeData(int aId)
        {
            return AttributeCfg.GetData(aId);
        }

        public GameQuenchingData GetGameQuenchingData(int qId)
        {
            return QuenchingMap.TryGetValue(qId, out var data) ? data : null;
        }

        public bool IsCanQuenching(int cost)
        {
            return GameDataManager.Ins.Oil >= cost;
        }

        #endregion

        #region 消息接收

        public void OnQuenching(List<int> qIdList)
        {
            //数据更新
            UpdateAllQuenchingEffect();

            EventManager.Call(LogicEvent.OnQuenching, qIdList);
        }

        #endregion

        #region 消息发送

        public void DoQuenchingLock(int qId, int unlockType)
        {
            if (QuenchingMap.TryGetValue(qId, out var gameQuenchingData))
            {
                gameQuenchingData.UnlockType = unlockType;
            }

            NetworkManager.Ins.SendMsg(new C2S_QuenchingLock()
            {
                QuenchingId = qId,
                QuenchingUnLockType = unlockType,
            });
        }

        public void DoQuenching(List<int> qIdList, int qLockCount)
        {
            //在客户端实现属性变化
            var djData = GetDJData(qLockCount);
            foreach (var qId in qIdList)
            {
                var attributeId = CommonHelper.GetIdFromWeight(djData.AttributeID, djData.AttributeWight);
                var melodyId = CommonHelper.GetIdFromWeight(djData.MelodyType, djData.MelodyTypeWight);

                if (QuenchingMap.TryGetValue(qId, out var gameQuenchingData))
                {
                    gameQuenchingData.AttributeId = attributeId;
                    gameQuenchingData.MelodyId = melodyId;
                }
                else
                {
                    gameQuenchingData = new GameQuenchingData()
                        { QuenchingId = qId, AttributeId = attributeId, MelodyId = melodyId };
                    QuenchingMap.Add(qId, gameQuenchingData);
                }
            }

            //客户端通知服务器存储变更的属性
            NetworkManager.Ins.SendMsg(new C2S_Quenching()
            {
                QuenchingList = QuenchingMap.Values.ToList(),
            });

            //客户端直接接收属性变更通知
            OnQuenching(qIdList);
        }

        #endregion
    }
}