using System.Collections.Generic;
using System.Linq;
using Configs;
using Framework.EventKit;
using Framework.Extension;
using Framework.Helper;
using Logic.Common;
using Logic.Data;
using Networks;

namespace Logic.Manager
{
    public class EngineManager : Singleton<EngineManager>
    {
        public GameEngineData gameEngineData { get; private set; }
        public EngineData engineData { get; private set; }
        public Dictionary<int, GameEnginePartData> engineMap { get; private set; }
        public Dictionary<int, GameEnginePartData> sparkMap { get; private set; }
        public Dictionary<int, GameEnginePartData> cylinderMap { get; private set; }

        public void Init(GameEngineData pGameEngineData, List<GameEnginePartData> pGameEnginePartDataList)
        {
            // 引擎数据
            gameEngineData = pGameEngineData;
            engineData = GetEngineData(gameEngineData.Level);
            // 引擎装备数据
            engineMap = new Dictionary<int, GameEnginePartData>(64);
            sparkMap = new Dictionary<int, GameEnginePartData>(64);
            cylinderMap = new Dictionary<int, GameEnginePartData>(64);
            foreach (var gameEnginePartData in pGameEnginePartDataList)
            {
                engineMap.Add(gameEnginePartData.InsID, gameEnginePartData);
                switch ((ItemType)gameEnginePartData.Type)
                {
                    case ItemType.Spark:
                        sparkMap.Add(gameEnginePartData.InsID, gameEnginePartData);
                        break;
                    case ItemType.Cylinder:
                        cylinderMap.Add(gameEnginePartData.InsID, gameEnginePartData);
                        break;
                }
            }

            //TODO:添加引擎相关加成属性
            // UpdateEngineEffect();
        }

        #region 通用

        /// <summary>
        /// 更新引擎相关加成属性
        /// </summary>
        public void UpdateEngineEffect()
        {
            EventManager.Call(LogicEvent.EngineEffectUpdate);
        }

        /// <summary>
        /// 获取引擎装备数据
        /// </summary>
        /// <param name="pID"></param>
        /// <returns></returns>
        public GameEnginePartData GetGameEngineData(int pID)
        {
            return engineMap.TryGetValue(pID, out var data) ? data : null;
        }

        /// <summary>
        /// 获取引擎火花塞数据
        /// </summary>
        /// <param name="pID"></param>
        /// <returns></returns>
        public GameEnginePartData GetGameSparkData(int pID)
        {
            return sparkMap.TryGetValue(pID, out var data) ? data : null;
        }

        /// <summary>
        /// 获取引擎气缸数据
        /// </summary>
        /// <param name="pID"></param>
        /// <returns></returns>
        public GameEnginePartData GetGameCylinderData(int pID)
        {
            return cylinderMap.TryGetValue(pID, out var data) ? data : null;
        }

        /// <summary>
        /// 获取引擎表数据
        /// </summary>
        /// <param name="pID"></param>
        /// <returns></returns>
        public EngineData GetEngineData(int pID)
        {
            return EngineCfg.GetData(pID);
        }

        /// <summary>
        /// 创建引擎装备数据
        /// 气缸或火花塞数据
        /// </summary>
        /// <param name="pID"></param>
        /// <param name="pType"></param>
        /// <returns></returns>
        public GameEnginePartData CreateGameEnginePartData(int pID, ItemType pType = ItemType.Cylinder)
        {
            var type = (int)pType;
            var cylinderData = CylinderCfg.GetData(pID);
            var attributeRandomData = AttributeRandomCfg.GetData(cylinderData.RandomAttribute);
            var count = attributeRandomData.Attribute.Count;
            var attr1 = attributeRandomData.Attribute[RandomHelper.Range(0, count)];
            var attr2 = attributeRandomData.Attribute[RandomHelper.Range(0, count)];
            return new GameEnginePartData { CfgID = pID, Type = type, Attr1ID = attr1, Attr2ID = attr2 };
        }

        /// <summary>
        /// 引擎是否可以升级
        /// </summary>
        /// <returns></returns>
        public bool IsCanEngineUpgrade()
        {
            if (IsEngineMaxLevel())
            {
                return false;
            }

            return GameDataManager.Ins.TecPoint >= engineData.Costtech;
        }

        /// <summary>
        /// 引擎是否达到最大等级
        /// </summary>
        /// <returns></returns>
        public bool IsEngineMaxLevel()
        {
            return gameEngineData.Level >= GameDefine.EngineMaxLevel;
        }

        /// <summary>
        /// 判断ID是否已上阵
        /// </summary>
        /// <param name="pID"></param>
        /// <returns></returns>
        public bool IsEngineOn(int pID)
        {
            return pID != 0 && gameEngineData.OnList.Any(onID => pID == onID);
        }

        /// <summary>
        /// 判断ID是否是该上阵位装配ID
        /// </summary>
        /// <param name="pID"></param>
        /// <param name="pSlotID"></param>
        /// <returns></returns>
        public bool IsIDOnSlot(int pID, int pSlotID)
        {
            return gameEngineData.OnList[pSlotID - 1] == pID;
        }

        #endregion

        #region 接收消息

        /// <summary>
        /// 接收引擎强化消息
        /// </summary>
        /// <param name="pMsg"></param>
        public void OnEngineUpgrade(S2C_EngUpgrade pMsg)
        {
            gameEngineData.Level = pMsg.Level;
            gameEngineData.Exp = pMsg.Exp;
            engineData = GetEngineData(gameEngineData.Level);
            EventManager.Call(LogicEvent.EngineUpgrade);
        }

        /// <summary>
        /// 接收引擎装配消息
        /// </summary>
        /// <param name="pMsg"></param>
        public void OnEngineOn(S2C_EngPartOn pMsg)
        {
            var data = (pMsg.InsID, pMsg.SlotID);
            gameEngineData.OnList[pMsg.SlotID - 1] = pMsg.InsID;
            EventManager.Call(LogicEvent.EngineOn, data);
        }

        /// <summary>
        /// 接收引擎解除消息
        /// </summary>
        /// <param name="pMsg"></param>
        public void OnEngineOff(S2C_EngPartOff pMsg)
        {
            var data = (pMsg.InsID, pMsg.SlotID);
            gameEngineData.OnList[pMsg.SlotID - 1] = 0;
            EventManager.Call(LogicEvent.EngineOff, data);
        }

        /// <summary>
        /// 接收引擎分解消息
        /// </summary>
        /// <param name="pMsg"></param>
        public void OnEngineResolve(S2C_EngResolve pMsg)
        {
            foreach (var insID in pMsg.InsID)
            {
                if (engineMap.ContainsKey(insID))
                {
                    engineMap.Remove(insID);
                }

                if (sparkMap.ContainsKey(insID))
                {
                    sparkMap.Remove(insID);
                }

                if (cylinderMap.ContainsKey(insID))
                {
                    cylinderMap.Remove(insID);
                }
            }

            EventManager.Call(LogicEvent.EngineResolve, pMsg.InsID);
        }

        public void OnEnginePartsUpdate(S2C_UpdateEngParts pMsg)
        {
            foreach (var gameEnginePartData in pMsg.Parts)
            {
                engineMap.Add(gameEnginePartData.InsID, gameEnginePartData);
                switch ((ItemType)gameEnginePartData.Type)
                {
                    case ItemType.Spark:
                        sparkMap.Add(gameEnginePartData.InsID, gameEnginePartData);
                        break;
                    case ItemType.Cylinder:
                        cylinderMap.Add(gameEnginePartData.InsID, gameEnginePartData);
                        break;
                }
            }

            EventManager.Call(LogicEvent.EnginePartsUpdate, pMsg.Parts);
        }

        #endregion

        #region 发送消息

        /// <summary>
        /// 发送引擎强化消息
        /// </summary>
        public void DoEngineUpgrade()
        {
            NetworkManager.Ins.SendMsg(new C2S_EngUpgrade());
        }

        /// <summary>
        /// 发送引擎装配消息
        /// </summary>
        /// <param name="pID"></param>
        /// <param name="pSlotID"></param>
        public void DoEngineOn(int pID, int pSlotID)
        {
            NetworkManager.Ins.SendMsg(new C2S_EngPartOn()
            {
                InsID = pID,
                SlotID = pSlotID,
            });
        }

        /// <summary>
        /// 发送引擎解除消息
        /// </summary>
        /// <param name="pID"></param>
        public void DoEngineOff(int pID)
        {
            NetworkManager.Ins.SendMsg(new C2S_EngPartOff()
            {
                InsID = pID,
            });
        }

        /// <summary>
        /// 发送引擎分解消息
        /// </summary>
        /// <param name="pIDList"></param>
        public void DoEngineResolve(List<int> pIDList)
        {
            NetworkManager.Ins.SendMsg(new C2S_EngResolve()
            {
                InsID = pIDList,
            });
        }

        #endregion
    }
}