using System;
using Configs;
using Logic.Manager;
using Logic.UI.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.Cells
{
    public class CommonQuenchingAttr : MonoBehaviour
    {
        public Image m_AttrQuality;
        public Sprite[] m_AttrIconGroup;
        public Image m_AttrIcon;
        public TextMeshProUGUI m_AttrText;
        public GameObject m_UnLock, m_Lock;
        public GameObject m_UnLockImage, m_LockImage;

        public int m_QuenchingId { get; private set; }
        public bool m_IsUnLock { get; private set; }
        public AttributeData m_AttributeData { get; private set; }
        private int m_IsUnLockType;

        private Button m_BtnClick;
        [HideInInspector] public Image m_ImageClick;
        public Action<CommonQuenchingAttr> m_ClickItem;

        private void Awake()
        {
            m_IsUnLock = true;
            m_BtnClick = transform.GetComponent<Button>();
            m_ImageClick = transform.GetComponent<Image>();
            m_BtnClick.onClick.AddListener(OnBtnItemClick);
        }

        private void OnDestroy()
        {
            m_ClickItem = null;
        }

        private void OnBtnItemClick()
        {
            m_ClickItem?.Invoke(this);
        }

        // 初始化
        public void Init(int id, int unLockType = 1)
        {
            m_QuenchingId = id;
            m_IsUnLockType = unLockType;
            UpdateLockState(unLockType);
        }

        // 点击淬炼属性进行锁定和解锁
        public void DoQuenchingLock()
        {
            m_IsUnLock = !m_IsUnLock;
            m_UnLock.SetActive(m_IsUnLock);
            m_Lock.SetActive(!m_IsUnLock);
            m_UnLockImage.SetActive(m_IsUnLock);
            m_LockImage.SetActive(!m_IsUnLock);
            m_IsUnLockType = m_IsUnLock ? 1 : 0;
            QuenchingManager.Ins.DoQuenchingLock(m_QuenchingId, m_IsUnLockType);
        }

        // 更新淬炼属性锁的状态
        private void UpdateLockState(int unLockType)
        {
            m_IsUnLock = unLockType == 1;
            m_UnLock.SetActive(m_IsUnLock);
            m_Lock.SetActive(!m_IsUnLock);
            m_UnLockImage.SetActive(m_IsUnLock);
            m_LockImage.SetActive(!m_IsUnLock);
        }

        // 更新淬炼属性信息
        public void UpdateQuenchingAttr(int attributeId, int melodyId)
        {
            m_AttributeData = QuenchingManager.Ins.GetAttributeData(attributeId);
            UICommonHelper.LoadQuenchingQuality(m_AttrQuality, m_AttributeData.Quality);
            m_AttrIcon.sprite = m_AttrIconGroup[melodyId - 1];
            m_AttrText.text = string.Format(m_AttributeData.Des, m_AttributeData.Value);
        }
    }
}