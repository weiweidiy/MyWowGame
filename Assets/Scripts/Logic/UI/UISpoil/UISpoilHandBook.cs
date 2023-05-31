using Framework.UI;
using Logic.Config;
using Logic.Manager;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.UISpoil
{

    public class UISpoilHandBook : UIPage
    {
        [SerializeField] SpoilHandBookHeadView tempHead;
        [SerializeField] SpoilHandBookContentView tempContent;
        [SerializeField] Transform root;
        [SerializeField] Button btnClose;


        Dictionary<int, List<SpoilHandBookContentView>> dicContents = new Dictionary<int, List<SpoilHandBookContentView>>();

        private void Awake()
        {
            Initialize();
        }


        public void Initialize()
        {
            btnClose.onClick.AddListener(()=> {
                Hide();
            });

            //获取所有数据
            var keys = ConfigManager.Ins.m_SpoilCfg.AllData.Keys;
            int curGroup = 0;
            foreach(var key in keys)
            {
                var cfg = ConfigManager.Ins.m_SpoilCfg.AllData[key];
                var groupId = cfg.GroupID;
                var groupName = cfg.GroupName;
                var spoilId = cfg.ID;
                var resId = cfg.ResID;
                var groupResId = cfg.GroupResID;
                if(groupId != curGroup)
                {
                    var headView = GetHead();
                    headView.onSwitchClick += HeadView_onSwitchClick;
                    var headVO = GetHeadVO(groupId, groupName, groupResId);
                    headView.Refresh(headVO);
                    curGroup = groupId;
                }

                //创建一个contentView
                var contentView = GetContent();
                contentView.SpoilId = spoilId;

                //根据组缓存view
                if (!dicContents.ContainsKey(groupId))
                {
                    var lst = new List<SpoilHandBookContentView>();
                    lst.Add(contentView);
                    dicContents.Add(groupId, lst);
                }
                else
                {
                    dicContents[groupId].Add(contentView);
                }
            }
    
        }


        private void OnEnable()
        {
            RefreshContent();
        }


        /// <summary>
        /// 刷新spoil content view
        /// </summary>
        public void RefreshContent()
        {
            //获取数据
            var keys = dicContents.Keys;
            foreach(var key in keys)
            {
                var lst = dicContents[key];
                foreach(var view in lst)
                {
                    var contentVO = GetHandBookContentVO(view.SpoilId);
                    view.Refresh(contentVO);
                }
            }

            

        }

        #region view 交互响应方法
        /// <summary>
        /// 点击了switch按钮
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void HeadView_onSwitchClick(SpoilHandBookHeadView view, bool isOn)
        {
            var groupId = view.GetVO().groupId;
            SetActive(groupId, isOn);
        }

        /// <summary>
        /// 设置组内容的显示和隐藏
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="active"></param>
        void SetActive(int groupId, bool active)
        {
            foreach(var view in dicContents[groupId])
            {
                view.gameObject.SetActive(active);
            }
        }

        #endregion


        #region Get方法组
        SpoilHandBookHeadVO GetHeadVO(int groupId, string typeName, int groupResId)
        {
            var vo = new SpoilHandBookHeadVO();
            var resCfg = Configs.ResCfg.GetData(groupResId);
            vo.iconPath = resCfg != null? resCfg.Res : "";
            vo.typeName = typeName;
            vo.groupId = groupId;
            return vo;
        }


        SpoilHandBookContentVO GetHandBookContentVO(int spoilId)
        {
            var vo = new SpoilHandBookContentVO();
            var unitVO = new SpoilUnitVO();
            unitVO.spoilId = spoilId;
            var spoil = SpoilManager.Ins.GetSpoil(unitVO.spoilId);
            unitVO.hold = spoil == null ? false : true;
            unitVO.level = spoil == null ? 0 : spoil.Level;
            var slot = SpoilManager.Ins.GetSlotStateBySpoilId(unitVO.spoilId);
            unitVO.equiped = slot == null ? false : true;
            unitVO.iconPath = SpoilManager.Ins.GetSpoilResPath(spoilId);
            vo.unitVO = unitVO;

            var cfg = Configs.SpoilCfg.GetData(spoilId);
            vo.name = cfg.SpoilName;
            vo.skillDesc = SpoilManager.Ins.GetSkillDesc(unitVO.spoilId);

            return vo;
        }

        SpoilHandBookHeadView GetHead()
        {
            return Instantiate(tempHead, root);
        }

        SpoilHandBookContentView GetContent()
        {
            return Instantiate(tempContent, root);
        }

        #endregion
    }
}