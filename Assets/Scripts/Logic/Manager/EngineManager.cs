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
    public class EngineManager : Singleton<EngineManager>
    {
        public int curEngineOnId { get; private set; }
        public int curEngineGetIdGearCost { get; private set; }

        public int curEngineCount { get; private set; }

        public Dictionary<int, GameEngineData> EngineMap { get; private set; }

        //拥有攻击力和血量加成
        public float AllHaveATKEffect { get; private set; }
        public float AllHaveHPEffect { get; private set; }

        public void Init(List<GameEngineData> pDataEngineList, int pDataEngineOnId, int pDataEngineGetId)
        {
            EngineMap = new Dictionary<int, GameEngineData>(64);
            foreach (var data in pDataEngineList)
            {
                EngineMap.Add(data.Id, data);
            }

            curEngineOnId = pDataEngineOnId;
            curEngineGetIdGearCost = GetEngineGearCost(pDataEngineGetId);
            curEngineCount = EngineMap.Count - 1;
            UpdateAllHaveEffect();
        }

        #region 消息接口

        public void OnEngineGet(S2C_EngineGet pMsg)
        {
            foreach (var gameEngineData in pMsg.EngineList)
            {
                //更新获取引擎的数据
                var data = GetGameEngineData(gameEngineData.Id);
                if (data != null)
                {
                    data.Id = gameEngineData.Id;
                    data.TypeId = gameEngineData.TypeId;
                    data.IsGet = gameEngineData.IsGet;
                    data.AttrId = gameEngineData.AttrId;
                    data.Level = gameEngineData.Level;
                    data.Reform = gameEngineData.Reform;
                }
                else
                {
                    //将要获取的引擎添加
                    EngineMap.Add(gameEngineData.Id, gameEngineData);
                    curEngineGetIdGearCost = GetEngineGearCost(gameEngineData.Id);
                }
            }

            curEngineCount++;
            EventManager.Call(LogicEvent.EngineGet, pMsg.LastEngineGetId);
        }

        public void OnEngineIntensify(S2C_EngineIntensify pMsg)
        {
            var data = ((m_EngineIntensifyId: pMsg.EngineId, m_EngineLevel: pMsg.EngineLevel));
            EngineMap[data.m_EngineIntensifyId].Level = data.m_EngineLevel;
            EventManager.Call(LogicEvent.EngineIntensify, data);
        }

        public void OnEngineRemove(int engineRemoveId)
        {
            foreach (var gameEngineData in EngineMap.Where(
                         gameEngineData => gameEngineData.Key == engineRemoveId))
            {
                EngineMap.Remove(gameEngineData.Key);
                curEngineCount--;
                EventManager.Call(LogicEvent.EngineRemove, engineRemoveId);
                break;
            }
        }

        public void OnEngineOn(S2C_EngineOn pMsg)
        {
            curEngineOnId = pMsg.EngineId;
            EventManager.Call(LogicEvent.EngineOn, pMsg.EngineId);
            UpdateAllHaveEffect();
        }

        public void OnEngineOff(S2C_EngineOff pMsg)
        {
            curEngineOnId = 0;
            EventManager.Call(LogicEvent.EngineOff, pMsg.EngineId);
            UpdateAllHaveEffect();
        }

        #endregion

        #region 通用接口

        // 获取引擎数据
        public GameEngineData GetGameEngineData(int engineId)
        {
            return EngineMap.TryGetValue(engineId, out var data) ? data : null;
        }

        // 获取引擎表数据
        public EngineData GetEngineData(int engineTypeId)
        {
            return EngineCfg.GetData(engineTypeId);
        }

        /// <summary>
        /// 获取引擎随机属性表数据
        /// </summary>
        /// <param name="attrId"></param>
        /// <returns></returns>
        public AttributeData GetAttributeData(int attrId)
        {
            return AttributeCfg.GetData(attrId);
        }
        
        /// <summary>
        /// 获取Res表数据
        /// </summary>
        /// <param name="rId"></param>
        /// <returns></returns>
        public ResData GetResData(int rId)
        {
            return ResCfg.GetData(rId);
        }

        // 引擎是否装备
        public bool IsOn(int pEngineId)
        {
            return curEngineOnId == pEngineId;
        }

        public bool IsCanIntensify(int cost)
        {
            return GameDataManager.Ins.Iron >= cost;
        }

        //获取引擎所有已有的加成值
        public void UpdateAllHaveEffect()
        {
            if (curEngineOnId == 0)
            {
                AllHaveATKEffect = 0;
                AllHaveHPEffect = 0;
                EventManager.Call(LogicEvent.EngineAllEffectUpdate);
            }
            else
            {
                var gameEngineData = GetGameEngineData(curEngineOnId);
                var engineData = GetEngineData(gameEngineData.TypeId);
                var engineLevel = gameEngineData.Level;
                //TODO:公式计算
                var atkEffect = (engineData.HasAdditionATK + engineLevel) / 100;
                var hpEffect = (engineData.HasAdditionHP + engineLevel) / 100;
                AllHaveATKEffect = atkEffect;
                AllHaveHPEffect = hpEffect;
                EventManager.Call(LogicEvent.EngineAllEffectUpdate);
            }
        }

        //引擎攻击力加成
        public BigDouble GetEngineATKAdd()
        {
            return AllHaveATKEffect;
        }

        //引擎血量加成
        public BigDouble GetEngineHPAdd()
        {
            return AllHaveHPEffect;
        }

        //获取将要获取引擎的齿轮消耗
        public int GetEngineGearCost(int pEngineGetId)
        {
            var costGear = EngineCfg.GetData(EngineMap[pEngineGetId].TypeId).CostGear;
            return costGear;
        }

        #endregion

        #region 操作接口

        public void DoEngineOn(int pEngineOnId)
        {
            NetworkManager.Ins.SendMsg(new C2S_EngineOn()
            {
                EngineId = pEngineOnId,
            });
        }

        public void DoEngineOff(int pEngineOffId)
        {
            NetworkManager.Ins.SendMsg(new C2S_EngineOff()
            {
                EngineId = pEngineOffId,
            });
        }

        public void DoEngineRemove(int pEngineRemoveId)
        {
            NetworkManager.Ins.SendMsg(new C2S_EngineRemove()
            {
                EngineId = pEngineRemoveId,
            });
        }

        public void DoEngineIntensify(int pEngineIntensifyId)
        {
            NetworkManager.Ins.SendMsg(new C2S_EngineIntensify()
            {
                EngineId = pEngineIntensifyId,
            });
        }

        #endregion
    }
}