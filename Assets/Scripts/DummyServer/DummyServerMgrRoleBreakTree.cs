using System.Linq;
using Configs;
using Logic.Common;
using Networks;

namespace DummyServer
{
    public partial class DummyServerMgr
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="pDB"></param>
        public void InitRoleBreakTree(DummyDB pDB)
        {
            pDB.BreakTreeList.Add(new GameBreakTreeData() { Id = GameDefine.RoleBreakTreeDefaultId });
        }

        /// <summary>
        /// 接收C2S_BreakTreeReset协议
        /// </summary>
        /// <param name="pMsg"></param>
        public void OnBreakTreeReset(C2S_BreakTreeReset pMsg)
        {
            //突破天赋树重置钻石消耗
            var cost = GameDefine.RoleBreakTreeDiamondCost;
            if (!IsCanResetBreakTree(cost))
            {
                return;
            }

            //消耗重置所需的钻石数量
            UpdateDiamond(-GameDefine.RoleBreakTreeDiamondCost);

            //重置突破天赋树
            m_DB.BreakTreeList.Clear();
            m_DB.BreakTreeList.Add(new GameBreakTreeData() { Id = GameDefine.RoleBreakTreeDefaultId });

            //返还消耗的突破天赋点
            UpdateBreakTP(m_DB.BreakTPTotal);
            //重置返回的突破天赋点
            m_DB.BreakTPTotal = 0;

            //保存数据
            DummyDB.Save(m_DB);
        }

        /// <summary>
        /// 接收C2S_BreakTreeIntensify协议
        /// </summary>
        /// <param name="pMsg"></param>
        public void OnBreakTreeIntensify(C2S_BreakTreeIntensify pMsg)
        {
            //接收到的当前突破项Id
            var id = pMsg.Id;

            //获取当前突破项数据
            var curBreakData = GetGameBreakTreeData(id);
            //如果收到数据库中没有的天赋项ID强化消息处理
            if (curBreakData == null)
            {
                m_DB.BreakTreeList.Add(new GameBreakTreeData() { Id = id });
                curBreakData = GetGameBreakTreeData(id);
            }

            //更新英雄突破天赋点
            var herosBreakData = GetHerosBreakData(id);
            var cost = herosBreakData.BaseCost;
            //如果突破天赋点不满足满足该项升级数量处理
            if (!IsCanUpgradeLevel(cost))
            {
                return;
            }

            //消耗突破天赋点
            UpdateBreakTP(-cost);
            //存储返还的突破天赋点
            m_DB.BreakTPTotal += cost;

            //突破天赋树当前项等级提升
            curBreakData.Level++;
            //如果当前项异常升级到时等级大于当前项最大等级处理
            if (curBreakData.Level > herosBreakData.LvlMax)
            {
                curBreakData.Level = herosBreakData.LvlMax;
                return;
            }

            //保存数据
            DummyDB.Save(m_DB);

            //发送当前项和当前后继项数据
            SendMsg(new S2C_BreakTreeIntensify()
            {
                CurBreakData = curBreakData,
            });
        }

        #region 通用

        /// <summary>
        /// 获取突破天赋树数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GameBreakTreeData GetGameBreakTreeData(int id)
        {
            return m_DB.BreakTreeList.FirstOrDefault(gameBreakTreeData => gameBreakTreeData.Id == id);
        }

        /// <summary>
        /// 获取HerosBreakData表数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HerosBreakData GetHerosBreakData(int id)
        {
            return HerosBreakCfg.GetData(id);
        }

        /// <summary>
        /// 判断突破天赋点是否满足该项升级数量
        /// </summary>
        /// <param name="cost"></param>
        /// <returns></returns>
        private bool IsCanUpgradeLevel(int cost)
        {
            return m_DB.BreakTP >= cost;
        }

        /// <summary>
        /// 判断钻石数量是否满足天赋树重置
        /// </summary>
        /// <param name="cost"></param>
        /// <returns></returns>
        private bool IsCanResetBreakTree(int cost)
        {
            return m_DB.m_Diamond >= cost;
        }

        #endregion
    }
}