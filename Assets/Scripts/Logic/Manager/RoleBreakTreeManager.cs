using System.Collections.Generic;
using BreakInfinity;
using Configs;
using Framework.EventKit;
using Framework.Extension;
using Logic.Common;
using Logic.Data;
using Networks;

namespace Logic.Manager
{
    public class RoleBreakTreeManager : Singleton<RoleBreakTreeManager>
    {
        public Dictionary<int, GameBreakTreeData> BreakTreeMap { get; private set; }

        /*
         * 英雄突破天赋树属性
         * 攻击力
         * 体力
         * 攻击速度
         * 暴击伤害
         * 金币获取量
         * 经验获取
         */
        public float BreakTreeATK { get; private set; }
        public float BreakTreeHP { get; private set; }
        public float BreakTreeSpeed { get; private set; }
        public float BreakTreeCriticalDamage { get; private set; }
        public float BreakTreeGoldObtain { get; private set; }
        public float BreakTreeRoleExpObtain { get; private set; }

        //初始化
        public void Init(List<GameBreakTreeData> pBreakTreeList)
        {
            BreakTreeMap = new Dictionary<int, GameBreakTreeData>();
            foreach (var gameBreakTreeData in pBreakTreeList)
            {
                BreakTreeMap.Add(gameBreakTreeData.Id, gameBreakTreeData);
            }

            //更新可以突破的天赋树后继节点
            foreach (var gameBreakTreeData in pBreakTreeList)
            {
                UpdateNextIdListToBreakTreeMap(gameBreakTreeData);
            }

            UpdateAllBreakTreeEffect();
        }

        #region 通用

        //更新突破天赋树所有属性加成
        public void UpdateAllBreakTreeEffect()
        {
            /*
             * 攻击力
             * 体力
             * 攻击速度
             * 暴击伤害
             * 金币获取量
             * 经验获取
             */
            var breakTreeAttributes = new Dictionary<AttributeType, float>();

            foreach (var breakTreeMapValue in BreakTreeMap.Values)
            {
                var attributeLevel = breakTreeMapValue.Level;

                var herosBreakData = GetHerosBreakData(breakTreeMapValue.Id);
                var attributeGrow = herosBreakData.ResearchGrow;

                var attributeData = GetAttributeData(herosBreakData.BreakAttrGroup);
                var attributeType = attributeData.Type;
                var attributeValue = attributeData.Value;

                var endValue = attributeValue + attributeLevel * attributeGrow;

                if (breakTreeAttributes.ContainsKey((AttributeType)attributeType))
                {
                    breakTreeAttributes[(AttributeType)attributeType] += endValue;
                }
                else
                {
                    breakTreeAttributes.Add((AttributeType)attributeType, endValue);
                }
            }

            BreakTreeATK = CalculateBreakTreeAttr(AttributeType.ATK, breakTreeAttributes);
            BreakTreeHP = CalculateBreakTreeAttr(AttributeType.HP, breakTreeAttributes);
            BreakTreeSpeed = breakTreeAttributes.TryGetValue(AttributeType.Speed, out var speed) ? speed : 0;
            BreakTreeCriticalDamage = CalculateBreakTreeAttr(AttributeType.CriticalDamage, breakTreeAttributes);
            BreakTreeGoldObtain = CalculateBreakTreeAttr(AttributeType.GoldObtain, breakTreeAttributes);
            BreakTreeRoleExpObtain = CalculateBreakTreeAttr(AttributeType.RoleExpObtain, breakTreeAttributes);

            EventManager.Call(LogicEvent.RoleBreakTreeEffectUpdate);
        }

        private float CalculateBreakTreeAttr(AttributeType attributeType,
            IDictionary<AttributeType, float> BreakTreeGroupAttrs)
        {
            return BreakTreeGroupAttrs.TryGetValue(attributeType, out var attributeValue) ? attributeValue / 100f : 0;
        }

        //获取突破天赋树攻击力加成
        public BigDouble GetBreakTreeATKAdd()
        {
            return BreakTreeATK;
        }

        //获取突破天赋树攻击力加成
        public BigDouble GetBreakTreeHPAdd()
        {
            return BreakTreeHP;
        }

        //获取GameBreakTreeData
        public GameBreakTreeData GetGameBreakTreeData(int pId)
        {
            return BreakTreeMap.TryGetValue(pId, out var data) ? data : null;
        }

        //获取HerosBreak表数据
        public HerosBreakData GetHerosBreakData(int pId)
        {
            return HerosBreakCfg.GetData(pId);
        }

        //获取Res表数据
        public ResData GetResData(int pId)
        {
            return ResCfg.GetData(pId);
        }

        //获取Attribute表数据
        public AttributeData GetAttributeData(int pId)
        {
            return AttributeCfg.GetData(pId);
        }

        /// <summary>
        /// 该方法用于红点系统的监听
        /// 判断突破天赋点是否满足该项升级条件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsCanUpgradeLevel(int id)
        {
            // 1.判断当前项需要再BreakTree的Map中
            var curBreakData = GetGameBreakTreeData(id);
            if (curBreakData == null)
            {
                return false;
            }

            // 2.判断当前项是否达到最大等级
            if (curBreakData.Level >= GetHerosBreakData(curBreakData.Id).LvlMax)
            {
                return false;
            }

            // 3.判断升级所需的突破天赋点消耗需满足需求
            var cost = GetHerosBreakData(id).BaseCost;
            return GameDataManager.Ins.BreakTP >= cost;
        }

        /// <summary>
        /// 判断钻石数量是否满足天赋树重置
        /// </summary>
        public bool IsCanResetBreakTree()
        {
            // 1.判断默认天赋项需要被强化
            if (BreakTreeMap[GameDefine.RoleBreakTreeDefaultId].Level <= 0)
            {
                return false;
            }

            // 2.判断重置天赋树所需的钻石消耗需满足需求
            var cost = GameDefine.RoleBreakTreeDiamondCost;
            return GameDataManager.Ins.Diamond >= cost;
        }

        /// <summary>
        /// 判断该后继节点对应的前驱节点是否都达到最大等级
        /// </summary>
        /// <param name="priorIdList"></param>
        /// <returns></returns>
        private bool IsPriorIdListLevelMax(List<int> priorIdList)
        {
            foreach (var priorId in priorIdList)
            {
                var priorBreakData = GetGameBreakTreeData(priorId);
                if (priorBreakData == null)
                {
                    return false;
                }

                if (priorBreakData.Level < GetHerosBreakData(priorBreakData.Id).LvlMax)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 更新可以突破的天赋树后继节点
        /// </summary>
        /// <param name="curBreakData"></param>
        private void UpdateNextIdListToBreakTreeMap(GameBreakTreeData curBreakData)
        {
            var herosBreakData = GetHerosBreakData(curBreakData.Id);
            //如果突破天赋树当前项等级达到最大级，解锁该项的后继天赋项
            if (curBreakData.Level >= herosBreakData.LvlMax)
            {
                var nextIdList = herosBreakData.NextID;
                foreach (var nextId in nextIdList)
                {
                    if (nextId != 0)
                    {
                        //找到后继节点对应的前驱节点
                        var priorIdList = GetHerosBreakData(nextId).PreID;
                        //判断该后继节点对应的前驱节点是否都都达到最大等级
                        if (IsPriorIdListLevelMax(priorIdList))
                        {
                            //更新整个突破天赋树数据
                            if (!BreakTreeMap.ContainsKey(nextId))
                            {
                                BreakTreeMap.Add(nextId, new GameBreakTreeData { Id = nextId });
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region 接收消息

        //接收英雄天赋树重置成功消息
        private void OnReset()
        {
            BreakTreeMap.Clear();
            BreakTreeMap.Add(GameDefine.RoleBreakTreeDefaultId,
                new GameBreakTreeData() { Id = GameDefine.RoleBreakTreeDefaultId });

            UpdateAllBreakTreeEffect();
            EventManager.Call(LogicEvent.RoleBreakTreeReset);
        }


        //接收英雄天赋树强化成功消息
        public void OnIntensify(S2C_BreakTreeIntensify pMsg)
        {
            var curBreakData = GetGameBreakTreeData(pMsg.CurBreakData.Id);

            if (curBreakData != null)
            {
                //更新强化等级
                curBreakData.Level = pMsg.CurBreakData.Level;
                //更新可以突破的天赋树后继节点
                UpdateNextIdListToBreakTreeMap(curBreakData);
            }
            else
            {
                BreakTreeMap.Add(pMsg.CurBreakData.Id, pMsg.CurBreakData);
            }

            UpdateAllBreakTreeEffect();
            EventManager.Call(LogicEvent.RoleBreakTreeIntensify);
        }

        #endregion

        #region 发送消息

        //发送英雄天赋树重置消息
        public void DoReset()
        {
            NetworkManager.Ins.SendMsg(new C2S_BreakTreeReset());
            OnReset();
        }

        //发送英雄天赋树强化消息
        public void DoIntensify(int pIntensifyId)
        {
            NetworkManager.Ins.SendMsg(new C2S_BreakTreeIntensify()
            {
                Id = pIntensifyId,
            });
        }

        #endregion
    }
}