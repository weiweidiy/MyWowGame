using Framework.UI;
using Logic.Manager;
using Logic.UI.Common;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.UISpoil
{
    public struct SpoilBreakVO
    {
        public int spoilId;
        public string name;
        public int breakLevel;
        public string iconPath;
        public string maxLevelContent;
        public string holdEffectAtkContent;
        public string holdEffectHpContent;
        public int cost;
        public string skillDesc;
    }

    public class UISpoilBreak : UIPage
    {
        [SerializeField] TextMeshProUGUI txtSpoilName;
        [SerializeField] Image iconSpoil;
        [SerializeField] GameObject goBreak;
        [SerializeField] TextMeshProUGUI txtBreakCount;
        [SerializeField] Button btnBreakthrough;
        [SerializeField] TextMeshProUGUI txtMaxLevelContent;
        [SerializeField] TextMeshProUGUI txtHoldEffectAtk;
        [SerializeField] TextMeshProUGUI txtHoldEffectAtkContent;
        [SerializeField] TextMeshProUGUI txtHoldEffectHp;
        [SerializeField] TextMeshProUGUI txtHoldEffectHpContent;
        [SerializeField] TextMeshProUGUI txtSkill;
        [SerializeField] TextMeshProUGUI txtSkillContent;
        [SerializeField] TextMeshProUGUI txtCost;

        [SerializeField] Button btnClose;


        int spoilId;

        private void Awake()
        {
            btnBreakthrough.onClick.AddListener(OnBreakthroughClicked);
            btnClose.onClick.AddListener(OnCloseClicked);
        }



        public override void OnShow()
        {
            spoilId = (int)m_OpenData_;

            var vo = GetSpoilBreakVO(spoilId);

            Refresh(vo);
        }

        #region 刷新界面
        public void Refresh(SpoilBreakVO vo)
        {
            RefreshSpoil(vo);
            RefreshDetailDesc(vo);
            RefreshCost(vo);
        }


        public void RefreshSpoil(SpoilBreakVO vo)
        {
            goBreak.SetActive(vo.breakLevel > 0);

            UICommonHelper.LoadIcon(iconSpoil, vo.iconPath);
            txtSpoilName.text = vo.name;
            txtBreakCount.text = vo.breakLevel.ToString();
        }

        public void RefreshDetailDesc(SpoilBreakVO vo)
        {
            txtMaxLevelContent.text = vo.maxLevelContent;
            txtHoldEffectAtk.gameObject.SetActive(vo.holdEffectAtkContent != "");
            txtHoldEffectAtkContent.text = vo.holdEffectAtkContent;
            txtHoldEffectHp.gameObject.SetActive(vo.holdEffectHpContent != "");
            txtHoldEffectHpContent.text = vo.holdEffectHpContent;
            txtSkillContent.text = vo.skillDesc;
            
        }

        public void RefreshCost(SpoilBreakVO vo)
        {
            txtCost.text = vo.cost.ToString();
        }

        #endregion

        #region view交互
        private void OnBreakthroughClicked()
        {
            SpoilManager.Ins.RequestSpoilUpgrade(spoilId);
            Hide();
        }

        private void OnCloseClicked()
        {
            Hide();
        }

        #endregion


        #region Get方法
        public SpoilBreakVO GetSpoilBreakVO(int spoilId)
        {
            var vo = new SpoilBreakVO();
            var cfg = Configs.SpoilCfg.GetData(spoilId);

            vo.spoilId = spoilId;
            vo.name = cfg.SpoilName;
            vo.breakLevel = SpoilManager.Ins.GetSpoilBreakthroughLevel(spoilId);
            vo.iconPath = SpoilManager.Ins.GetSpoilResPath(spoilId);

            var spoil = SpoilManager.Ins.GetSpoil(spoilId);
            Debug.Assert(spoil != null, " 没有找到 spoil " + spoilId);
            var level = spoil.Level;
            var nextLevel = SpoilManager.Ins.GetNextBreakthroughLevel(spoilId);
            vo.maxLevelContent = "最高等级：Lv" + level + "->" + nextLevel + "\n";

            float value = 0;

            var allAtkEffect = SpoilManager.Ins.GetAllAtkEffect();
            var nextAtkEffect = SpoilManager.Ins.GetBreakAtkEffect(spoilId, vo.breakLevel + 1);
            value = allAtkEffect + nextAtkEffect;
            vo.holdEffectAtkContent = nextAtkEffect == 0 ? "" : allAtkEffect + "% ->" + value + "%";

            var allHpEffect = SpoilManager.Ins.GetAllHpEffect();
            var nextHpEffect = SpoilManager.Ins.GetBreakHpEffect(spoilId, vo.breakLevel + 1);
            value = allHpEffect + nextHpEffect;
            vo.holdEffectHpContent = nextHpEffect == 0 ? "" : allHpEffect + "% ->" + value + "%";


            vo.skillDesc = SpoilManager.Ins.GetSkillDesc(spoilId, vo.breakLevel + 1);



            vo.cost = SpoilManager.Ins.GetBreakCost(spoilId);

            return vo;
        }
        #endregion
    }

    //var skillEffect = SpoilManager.Ins.GetSkillDesc(spoilId);
    //var nextSkillEffect = SpoilManager.Ins.GetSkillBreakEffect(spoilId, vo.breakLevel + 1);
    //value = skillEffect + nextSkillEffect;
    //vo.skillDesc = nextSkillEffect == 0 ? skillEffect + "%" : value + "%";
}