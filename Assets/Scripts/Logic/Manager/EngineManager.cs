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
                EngineMap.Add(data.m_Id, data);
            }

            curEngineOnId = pDataEngineOnId;
            curEngineGetIdGearCost = GetEngineGearCost(pDataEngineGetId);
            curEngineCount = EngineMap.Count - 1;
            UpdateAllHaveEffect();
        }

        #region 消息接口

        public void OnEngineGet(S2C_EngineGet pMsg)
        {
            foreach (var gameEngineData in pMsg.m_EngineList)
            {
                //更新获取引擎的数据
                var data = GetGameEngineData(gameEngineData.m_Id);
                if (data != null)
                {
                    data.m_Id = gameEngineData.m_Id;
                    data.m_TypeId = gameEngineData.m_TypeId;
                    data.m_IsGet = gameEngineData.m_IsGet;
                    data.m_AttrId = gameEngineData.m_AttrId;
                    data.m_Level = gameEngineData.m_Level;
                    data.m_Reform = gameEngineData.m_Reform;
                }
                else
                {
                    //将要获取的引擎添加
                    EngineMap.Add(gameEngineData.m_Id, gameEngineData);
                    curEngineGetIdGearCost = GetEngineGearCost(gameEngineData.m_Id);
                }
            }

            curEngineCount++;
            EventManager.Call(LogicEvent.EngineGet, pMsg.m_LastEngineGetId);
        }

        public void OnEngineIntensify(S2C_EngineIntensify pMsg)
        {
            var data = ((pMsg.m_EngineIntensifyId, pMsg.m_EngineLevel));
            EngineMap[data.m_EngineIntensifyId].m_Level = data.m_EngineLevel;
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
            curEngineOnId = pMsg.m_EngineOnId;
            EventManager.Call(LogicEvent.EngineOn, pMsg.m_EngineOnId);
            UpdateAllHaveEffect();
        }

        public void OnEngineOff(S2C_EngineOff pMsg)
        {
            curEngineOnId = 0;
            EventManager.Call(LogicEvent.EngineOff, pMsg.m_EngineOffId);
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
                var engineData = GetEngineData(gameEngineData.m_TypeId);
                var engineLevel = gameEngineData.m_Level;
                //TODO:公式计算
                var atkEffect = (engineData.HasAdditionATK + engineLevel) / 100;
                var hpEffect = (engineData.HasAdditionHP + engineLevel) / 100;
                AllHaveATKEffect = atkEffect;
                AllHaveHPEffect = hpEffect;
                EventManager.Call(LogicEvent.EngineAllEffectUpdate);
            }
        }

        //引擎攻击力加成
        public BigDouble GetEngineAddATK()
        {
            return AllHaveATKEffect;
        }

        //引擎血量加成
        public BigDouble GetEngineAddHP()
        {
            return AllHaveHPEffect;
        }

        //获取将要获取引擎的齿轮消耗
        public int GetEngineGearCost(int pEngineGetId)
        {
            var costGear = EngineCfg.GetData(EngineMap[pEngineGetId].m_TypeId).CostGear;
            return costGear;
        }

        #endregion

        #region 操作接口

        public void DoEngineOn(int pEngineOnId)
        {
            NetworkManager.Ins.SendMsg(new C2S_EngineOn()
            {
                m_EngineOnId = pEngineOnId,
            });
        }

        public void DoEngineOff(int pEngineOffId)
        {
            NetworkManager.Ins.SendMsg(new C2S_EngineOff()
            {
                m_EngineOffId = pEngineOffId,
            });
        }

        public void DoEngineRemove(int pEngineRemoveId)
        {
            NetworkManager.Ins.SendMsg(new C2S_EngineRemove()
            {
                m_EngineRemoveId = pEngineRemoveId,
            });
        }

        public void DoEngineIntensify(int pEngineIntensifyId)
        {
            NetworkManager.Ins.SendMsg(new C2S_EngineIntensify()
            {
                m_EngineIntensifyId = pEngineIntensifyId,
            });
        }

        #endregion
    }
}