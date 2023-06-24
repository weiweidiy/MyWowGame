using Configs;
using Logic.Common;
using Logic.Config;
using Networks;

namespace DummyServer
{
    public partial class DummyServerMgr
    {
        public void InitRole(DummyDB pDB)
        {
            //初始英雄 TODO:7101最好是配置表配置
            // pDB.m_RoleList.Add(new GameRoleData { RoleID = 7101 });

            var hexoList = ConfigManager.Ins.m_HerosCfg.AllData;
            foreach (var hexo in hexoList)
            {
                pDB.m_RoleList.Add(new GameRoleData { RoleID = hexo.Value.ID });
            }
        }

        #region 通用

        //获取英雄当前等级升级需要的总经验
        public static int GetRoleIntensifyTotalExp(int roleLevel)
        {
            var herosLvUpData = HerosLvUpCfg.GetData(roleLevel) ?? HerosLvUpCfg.GetData(0);
            return herosLvUpData.CostBase + (herosLvUpData.CostGrow + herosLvUpData.CostExp) * roleLevel;
        }

        //获取英雄当前突破等级需要的消耗
        public static int GetRoleBreakCost(int roleLevel)
        {
            var herosBreakUpData = HerosBreakUpCfg.GetData(roleLevel) ?? HerosBreakUpCfg.GetData(0);
            return herosBreakUpData.CostBase + (herosBreakUpData.CostGrow + herosBreakUpData.CostExp) * roleLevel;
        }

        #endregion

        #region 操作

        private void DoRoleOn(int pId)
        {
            if (m_DB.m_RoleOnId != 0)
            {
                SendMsg(new S2C_RoleOff { RoleId = m_DB.m_RoleOnId });
            }

            m_DB.m_RoleOnId = pId;

            SendMsg(new S2C_RoleOn { RoleId = pId });
            DummyDB.Save(m_DB);
        }

        private void DoRoleIntensify(int pId)
        {
            var roleData = m_DB.m_RoleList.Find(pData => pData.RoleID == pId);
            if (roleData == null) return;

            //消耗英雄升级所需的蘑菇
            UpdateMushRoom(-1);
            //英雄等级提升
            roleData.RoleExp += (int)GameDefine.RoleMushRoomExp;
            var roleTotalExp = GetRoleIntensifyTotalExp(roleData.RoleLevel);

            while (roleData.RoleExp >= roleTotalExp)
            {
                roleData.RoleExp -= roleTotalExp;
                roleData.RoleLevel++;
                roleTotalExp = GetRoleIntensifyTotalExp(roleData.RoleLevel);

                //英雄突破条件
                if (roleData.RoleLevel % GameDefine.RoleBreakEveryLevel != 0) continue;
                roleData.RoleBreakState = true;
                break;
            }

            SendMsg(new S2C_RoleIntensify()
            {
                RoleId = pId,
                RoleLevel = roleData.RoleLevel,
                RoleExp = roleData.RoleExp,
                RoleBreakState = roleData.RoleBreakState,
            });
            DummyDB.Save(m_DB);
        }

        private void DoRoleBreak(int pId)
        {
            var roleData = m_DB.m_RoleList.Find(pData => pData.RoleID == pId);
            if (roleData == null) return;

            //消耗英雄突破所需的矿石
            var cost = GetRoleBreakCost(roleData.RoleLevel);
            UpdateBreakOre(-cost);
            //英雄突破等级提升
            roleData.RoleBreakLevel++;
            roleData.RoleBreakState = false;

            SendMsg(new S2C_RoleBreak()
            {
                RoleId = pId,
                RoleBreakLevel = roleData.RoleBreakLevel,
                RoleBreakState = roleData.RoleBreakState,
            });
            DummyDB.Save(m_DB);
        }

        #endregion
    }
}