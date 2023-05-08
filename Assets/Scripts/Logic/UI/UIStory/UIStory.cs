using Configs;
using DG.Tweening;
using Framework.Extension;
using Framework.UI;
using Logic.Manager;
using TMPro;
using UnityEngine;

namespace Logic.UI.UIStory
{
    public class UIStory : UIPage
    {
        public TextMeshProUGUI m_Context;

        public GameObject m_ContextBK;

        private StoryData m_CurrentData;

        public override void OnShow()
        {
            m_ContextBK.SetLocalScale(new Vector3(0.1f, 0.1f, 1));
            var pID = (int)m_OpenData_;
            ShowContext(pID);
        }

        public void OnClick()
        {
            if (m_CurrentData.NextID <= 0)
            {
                Hide();
            }
            else
            {
                m_ContextBK.SetLocalScale(new Vector3(0.1f, 0.1f, 1));
                ShowContext(m_CurrentData.NextID);
            }
        }

        private void ShowContext(int pID)
        {
            m_ContextBK.transform.DOScale(new Vector3(0.8f, 0.8f, 1), 0.2f).SetEase(Ease.OutBack);
            m_CurrentData = LockStoryManager.Ins.GetStoryData(pID);
            m_Context.text = "";
            m_Context.DOText(m_CurrentData.Dialogue, 1.0f);
        }
    }
}