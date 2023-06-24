using Framework.EventKit;
using Framework.Extension;
using Framework.UI;
using Logic.Common;
using Logic.Config;
using Logic.Manager;
using Logic.UI.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Logic.UI.UIRole.UIRole;

namespace Logic.UI.UIRole
{
    public class UIReformHeroSelectionController : MonoBehaviour
    {
        public event Action<int> onStartClicked;
        public event Action onReturnClicked;

        [Header("角色生成相关")]
        [SerializeField] Transform m_ItemListRoot;
        [SerializeField] CommonRoleItem commonRoleItem;
        [SerializeField] GameObject root;
        [SerializeField] Image[] imgProgress;
        [SerializeField] Image[] imgIcons;
        [SerializeField] GameObject[] goTips;

        [SerializeField] Button btnStart;
        [SerializeField] Button btnReturn;

        [SerializeField] Material matGrey;
        [SerializeField] Material matNormal;

        private List<CommonRoleItem> roleItemList = new List<CommonRoleItem>();
        private List<SortRoleData> sortRoleList = new List<SortRoleData>();

        int maxCount = 9;
        int curCount = 0;



        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            var herosData = ConfigManager.Ins.m_HerosCfg.AllData;
            foreach (var sortRoleData in from heroData in herosData
                                         let have = RoleManager.Ins.IsRoleHave(heroData.Value.ID)
                                         select new SortRoleData(have, heroData.Value))
            {
                if (sortRoleData.Have)
                    sortRoleList.Add(sortRoleData);
            }

            var sortRoleDatas = sortRoleList
                .OrderByDescending(x => x.Have)
                .ThenByDescending(x => x.HerosData.Quality);


            CommonRoleItem equipRole = null;
            foreach (var sortRoleData in sortRoleDatas)
            {
                var roleItem = Instantiate(commonRoleItem, m_ItemListRoot);
                roleItem.Init(sortRoleData.HerosData, sortRoleData.Have);
                roleItem.m_ClickRole += OnClickRoleItem;
                roleItemList.Add(roleItem);
                roleItem.gameObject.Show();

                if (roleItem.m_IsOn.activeSelf)
                    equipRole = roleItem;
            }

            btnStart.onClick.AddListener(OnStartClicked);
            btnReturn.onClick.AddListener(OnReturnClicked);

            RefreshParts(curCount);

            OnClickRoleItem(equipRole);

            //部位按钮监听
            for (int i = 0; i < imgIcons.Length; i++)
            {
                int index = i;
                var btn = imgIcons[i].GetComponent<Button>();
                btn.onClick.AddListener(() =>
                {
                    OnPartClicked(index);
                });
            }
        }

        private void OnPartClicked(int i)
        {
            //Debug.LogError(i);
            goTips[i].SetActive(!goTips[i].activeSelf);
        }

        private void OnReturnClicked()
        {
            onReturnClicked?.Invoke();
        }

        private void OnStartClicked()
        {
            onStartClicked?.Invoke(curCount);
        }

        /// <summary>
        /// 设置是否激活
        /// </summary>
        /// <param name="active"></param>
        public void SetNodeActive(bool active)
        {
            root.SetActive(active);
        }

        /// <summary>
        /// 点击了英雄
        /// </summary>
        /// <param name="cItem"></param>
        private void OnClickRoleItem(CommonRoleItem cItem)
        {
            if (!cItem.IsSelected() && cItem.m_IsOn.activeSelf)
            {
                cItem.ShowSelected();
                curCount++;
                RefreshParts(curCount);
                return;
            }

            if (cItem.m_IsOn.activeSelf)
            {
                EventManager.Call(LogicEvent.ShowTips, "已装备的司机不能取消！");
                return;
            }
                
                

            if(cItem.IsSelected())
            {
                cItem.HideSelected();
                curCount--;
            } 
            else
            {
                if (curCount >= maxCount)
                    return;

                cItem.ShowSelected();
                curCount++;
            }

            RefreshParts(curCount);     
        }

        /// <summary>
        /// 刷新了司机槽位
        /// </summary>
        /// <param name="progress"></param>
        void RefreshParts(int progress)
        {

            for(int i = 0; i < GetPartCount(); i ++)
            {
                var partMaxCount = (i+1) * GetMaxProgressPerPart();
                var partMinCount = i * GetMaxProgressPerPart();
                if (progress >= partMaxCount)
                {
                    imgProgress[i].fillAmount = 1f;
                    imgIcons[i].material = null;
                }
                else
                {
                    var value = Mathf.Max(0, progress - partMinCount);
                    imgProgress[i].fillAmount = (float)value / GetMaxProgressPerPart();

                    if(imgProgress[i].fillAmount > 0)
                    {
                        imgIcons[i].material = null;
                    }
                    else
                    {
                        imgIcons[i].material = matGrey;
                    }
                }
            }

        }

        /// <summary>
        /// 每个部位的最大进度
        /// </summary>
        /// <returns></returns>
        private int GetMaxProgressPerPart()
        {
            return 3;
        }

        private int GetPartCount()
        {
            return imgProgress.Length;
        }
    }
}