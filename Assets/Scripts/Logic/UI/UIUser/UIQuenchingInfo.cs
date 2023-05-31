using System.Collections.Generic;
using Framework.UI;
using Logic.Common;
using Logic.UI.Cells;
using TMPro;
using UnityEngine;

namespace Logic.UI.UIUser
{
    public class UIQuenchingInfo : UIPage
    {
        [Header("读取GameDefine配置表")] public TextMeshProUGUI m_ProbabilityF;
        public TextMeshProUGUI m_ProbabilityE;
        public TextMeshProUGUI m_ProbabilityD;
        public TextMeshProUGUI m_ProbabilityC;
        public TextMeshProUGUI m_ProbabilityB;
        public TextMeshProUGUI m_ProbabilityA;
        public TextMeshProUGUI m_ProbabilityS;
        public TextMeshProUGUI m_ProbabilitySS;

        [Header("读取DJDes配置表")] public List<CommonQuenchingInfo> m_ATKList = new List<CommonQuenchingInfo>();
        public List<CommonQuenchingInfo> m_HPList = new List<CommonQuenchingInfo>();
        public List<CommonQuenchingInfo> m_TripletHitList = new List<CommonQuenchingInfo>();
        public List<CommonQuenchingInfo> m_SpeedList = new List<CommonQuenchingInfo>();
        public List<CommonQuenchingInfo> m_CriticalList = new List<CommonQuenchingInfo>();
        public List<CommonQuenchingInfo> m_CriticalDamageList = new List<CommonQuenchingInfo>();
        public List<CommonQuenchingInfo> m_SkillDamageList = new List<CommonQuenchingInfo>();
        public List<CommonQuenchingInfo> m_CompanionDamageList = new List<CommonQuenchingInfo>();
        public List<CommonQuenchingInfo> m_GoldObtainList = new List<CommonQuenchingInfo>();

        private void Start()
        {
            UpdateProbabilityInfo();
            UpdateQuenchingInfo();
        }

        public void OnBtnCloseClick()
        {
            this.Hide();
        }

        /// <summary>
        /// 更新淬炼详情各级别获得概率
        /// </summary>
        private void UpdateProbabilityInfo()
        {
            TextMeshProUGUI[] probabilityTexts =
            {
                m_ProbabilityF, m_ProbabilityE, m_ProbabilityD, m_ProbabilityC, m_ProbabilityB, m_ProbabilityA,
                m_ProbabilityS, m_ProbabilitySS
            };

            int[] probabilityGameDefine =
            {
                GameDefine.DJProbabilityF, GameDefine.DJProbabilityE, GameDefine.DJProbabilityD,
                GameDefine.DJProbabilityC, GameDefine.DJProbabilityB, GameDefine.DJProbabilityA,
                GameDefine.DJProbabilityS, GameDefine.DJProbabilitySS
            };

            var length = probabilityTexts.Length;
            for (var i = 0; i < length; i++)
            {
                probabilityTexts[i].text = (probabilityGameDefine[i] / 1000f).ToString("F4") + "%";
            }
        }

        /// <summary>
        /// 更新淬炼详情各个属性的信息
        /// </summary>
        private void UpdateQuenchingInfo()
        {
            var allLists = new List<List<CommonQuenchingInfo>>()
            {
                m_ATKList,
                m_HPList,
                m_TripletHitList,
                m_SpeedList,
                m_CriticalList,
                m_CriticalDamageList,
                m_SkillDamageList,
                m_CompanionDamageList,
                m_GoldObtainList
            };
            var offsets = new int[] { 1, 9, 17, 25, 33, 41, 49, 57, 65 };
            var allListsCount = allLists.Count;
            for (var i = 0; i < allListsCount; i++)
            {
                var currentList = allLists[i];
                var currentOffset = offsets[i];
                var currentListCount = currentList.Count;
                for (var j = 0; j < currentListCount; j++)
                {
                    currentList[j].Init(j + currentOffset);
                }
            }
        }
    }
}