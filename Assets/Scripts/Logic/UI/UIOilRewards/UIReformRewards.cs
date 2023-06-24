using Framework.Extension;
using Framework.UI;
using Logic.UI.Cells;
using UnityEngine.UI;
using UnityEngine;
using Networks;
using TMPro;

namespace Logic.UI.UIPlaceRewards
{
    public class UIReformRewards : UIPage
    {
        [SerializeField] Button btnConfirm;

        [SerializeField] TextMeshProUGUI txtTechnologyPoint;

        [SerializeField] Transform content;

        [SerializeField] CommonEngineItem cylinderTemp;


        private void Awake()
        {
            btnConfirm.onClick.AddListener(OnConfirm);
        }


        public override void OnShow()
        {
            base.OnShow();

            var data = m_OpenData_ as S2C_ReformCopyReward;

            txtTechnologyPoint.text = data.TechnologyPoint.ToString();

            foreach (var cylinder in data.LstCylinders)
            {
                var item = Instantiate(cylinderTemp, content);
                item.Init(cylinder);
                item.Show();
            }
        }


        private void OnConfirm()
        {
            Close();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            btnConfirm.onClick.RemoveAllListeners();
        }
    }
}