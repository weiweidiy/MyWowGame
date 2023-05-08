using System;
using DG.Tweening;
using Framework.EventKit;
using Framework.Extension;
using Logic.Common;
using Logic.Manager;
using Logic.UI.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.Cells
{
    public class CommonLockItem : MonoBehaviour
    {
        public LockType m_LockType;
        public TextMeshProUGUI m_UnlockDes;
        public Button m_LockClick;
        private Image m_LockBg;

        private EventGroup m_EventGroup = new();

        private void Awake()
        {
            if (m_LockType == LockType.None) return;

            m_LockBg = GetComponent<Image>();
            m_LockClick.onClick.AddListener(OnLockClick);
            m_EventGroup.Register(LogicEvent.UpdateLockState, OnUpdateLockState);
            m_EventGroup.Register(LogicEvent.UpdateUnlockAll, (i, o) => { OnUpdateUnlockAll(); });
        }

        private void OnDestroy()
        {
            if (m_LockType == LockType.None) return;

            m_EventGroup.Release();
        }

        private void Start()
        {
            if (m_LockType == LockType.None) return;

            //解锁所有开放功能
            if (LockStoryManager.Ins.m_IsUnlockAll)
            {
                OnUpdateUnlockAll();
                return;
            }

            UpdateUnlockDescription();
            UpdateLockState();
        }

        #region 表现逻辑

        private void UpdateUnlockDescription()
        {
            var lockData = LockStoryManager.Ins.GetLockData((int)m_LockType);
            var unlockLvl = (long)lockData.UnlockLvl;
            var unlockDes = UICommonHelper.GetLevelNameByID(unlockLvl);
            m_UnlockDes.text = string.Format(lockData.UnlockDes, unlockDes);
        }

        private void UpdateLockState()
        {
            var lockStoryMap = LockStoryManager.Ins.LockStoryMap;
            if (lockStoryMap.Count == 0) return;
            foreach (var gameLockStoryData in LockStoryManager.Ins.LockStoryMap)
            {
                if (gameLockStoryData.Key == (int)m_LockType && gameLockStoryData.Value == (int)LockState.Unlock)
                {
                    this.gameObject.Hide();
                }
            }
        }

        private void OnLockClick()
        {
            EventManager.Call(LogicEvent.ShowTips, m_UnlockDes.text);
        }

        #endregion


        #region 事件处理

        private void OnUpdateLockState(int eventId, object data)
        {
            var (lockType, lockState) = (ValueTuple<LockType, LockState>)data;

            if (lockType != m_LockType || lockState != LockState.Unlock) return;

            // TODO:解锁动画
            m_LockBg.DOFade(0, 2f).OnComplete(() => { this.gameObject.Hide(); });
        }

        /// <summary>
        /// 解锁所有开放功能
        /// </summary>
        private void OnUpdateUnlockAll()
        {
            this.gameObject.Hide();
        }

        #endregion
    }
}