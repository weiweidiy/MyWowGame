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
    public class RoleManager : Singleton<RoleManager>
    {
        public int CurRoleOnID { get; private set; }

        public Dictionary<int, GameRoleData> RoleMap { get; private set; }

        //拥有属性加成
        public float RoleATKEffect { get; private set; }
        public float RoleHPEffect { get; private set; }

        public void Init(List<GameRoleData> pRoleList, int pRoleOnId)
        {
            CurRoleOnID = pRoleOnId;

            RoleMap = new Dictionary<int, GameRoleData>(64);
            foreach (var roleData in pRoleList)
            {
                RoleMap.Add(roleData.RoleID, roleData);
            }

            UpdateRoleEffect();
        }

        #region 通用

        public GameRoleData GetRoleData(int pRoleId)
        {
            return RoleMap.TryGetValue(pRoleId, out var roleData) ? roleData : null;
        }

        public ResData GetResData(int rId)
        {
            return ResCfg.GetData(rId);
        }

        public bool IsRoleHave(int pRoleId)
        {
            return RoleMap.ContainsKey(pRoleId);
        }

        public bool IsRoleOn(int pRoleId)
        {
            return CurRoleOnID == pRoleId;
        }

        public bool IsCanRoleIntensify(int cost)
        {
            return GameDataManager.Ins.MushRoom >= cost;
        }

        public bool IsCanRoleBreak(int cost)
        {
            return GameDataManager.Ins.BreakOre >= cost;
        }

        public BigDouble GetRoleATKAdd()
        {
            return 0;
        }

        public BigDouble GetRoleHPAdd()
        {
            return 0;
        }

        public void UpdateRoleEffect()
        {
        }

        #endregion

        #region 消息接收

        public void OnRoleOn(S2C_RoleOn pMsg)
        {
            CurRoleOnID = pMsg.RoleId;
            EventManager.Call(LogicEvent.RoleOn, pMsg.RoleId);
        }

        public void OnRoleOff(S2C_RoleOff pMsg)
        {
            CurRoleOnID = 0;
            EventManager.Call(LogicEvent.RoleOff, pMsg.RoleId);
        }

        public void OnRoleIntensify(S2C_RoleIntensify pMsg)
        {
            var data = (pMsg.RoleId, pMsg.RoleLevel, pMsg.RoleExp, pMsg.RoleBreakState);
            RoleMap[data.RoleId].RoleLevel = data.RoleLevel;
            RoleMap[data.RoleId].RoleExp = data.RoleExp;
            RoleMap[data.RoleId].RoleBreakState = data.RoleBreakState;
            EventManager.Call(LogicEvent.RoleIntensify, data);
        }

        public void OnRoleBreak(S2C_RoleBreak pMsg)
        {
            var data = (pMsg.RoleId, pMsg.RoleBreakLevel, pMsg.RoleBreakState);
            RoleMap[data.RoleId].RoleBreakLevel = data.RoleBreakLevel;
            RoleMap[data.RoleId].RoleBreakState = data.RoleBreakState;
            EventManager.Call(LogicEvent.RoleBreak, data);
        }

        public void OnRoleListUpdate(S2C_RoleListUpdate pMsg)
        {
        }

        #endregion

        #region 消息发送

        public void DoRoleOn(int pRoleId)
        {
            NetworkManager.Ins.SendMsg(new C2S_RoleOn()
            {
                RoleId = pRoleId,
            });
        }

        public void DoRoleIntensify(int pRoleId)
        {
            NetworkManager.Ins.SendMsg(new C2S_RoleIntensify()
            {
                RoleId = pRoleId,
            });
        }

        public void DoRoleBreak(int pRoleId)
        {
            NetworkManager.Ins.SendMsg(new C2S_RoleBreak()
            {
                RoleId = pRoleId,
            });
        }

        #endregion
    }
}