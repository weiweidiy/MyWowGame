using DG.Tweening;
using Framework.EventKit;
using Framework.Extension;
using Framework.Helper;
using Logic.Common;
using Logic.Manager;
using Logic.UI.UIMining;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Logic.UI.Cells
{
    public class CommonMiningGround : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler,
        IPointerEnterHandler, IPointerExitHandler
    {
        [LabelText("地质背景")] public Sprite[] m_MiningGround;
        public GameObject m_GeenMask, m_BlueMask, m_RedMask;
        public GameObject m_GroundMask;
        public Image m_Image;

        public int m_ID;
        private bool m_IsCanClick = true;
        private bool m_IsDoorCanClick;

        public int m_TreasureType;

        public ParticleSystem m_ParticleSystem;

        private void Awake()
        {
            m_Image = m_GroundMask.GetComponent<Image>();
        }

        public void Init(int id)
        {
            m_ID = id;
            var max = m_MiningGround.Length;
            var pId = RandomHelper.Range(0, max);
            m_Image.sprite = m_MiningGround[pId];
            m_GroundMask.Show();
            m_IsCanClick = true;
            m_IsDoorCanClick = false;
            transform.DOPunchScale(new Vector3(1.1f, 1.1f, 1f), 0.02f * (m_ID + 1));
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!m_IsCanClick)
            {
                m_RedMask.Show();
            }
            else
            {
                m_GeenMask.Show();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!m_IsCanClick)
            {
                m_RedMask.Hide();
            }
            else
            {
                m_GeenMask.Hide();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (m_IsDoorCanClick && MiningManager.Ins.m_PropDoTweenCount == 0)
            {
                MiningManager.Ins.SendMsgC2SUpdateMiningData(MiningType.Door, MiningUpdateType.Increase);
                return;
            }

            if (!m_IsCanClick)
            {
                return;
            }

            if (MiningManager.Ins.m_MiningData.m_HammerCount <= 0)
            {
                EventManager.Call(LogicEvent.ShowTips, "没有足够的矿锤");
            }
            else
            {
                MiningManager.Ins.SendMsgC2SUpdateMiningData(MiningType.Hammer, MiningUpdateType.Reduce);
                HideMiningGround();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
            {
                return;
            }

            var prop = eventData.pointerDrag.GetComponent<UISpecialProp>();
            if (prop != null)
            {
                prop.m_TargetMiningGround = this;

                switch (prop.m_TreasureType)
                {
                    case MiningType.Bomb:
                        EventManager.Call(LogicEvent.ShowCrossedGrid, m_ID);
                        break;
                    case MiningType.Scope:
                        EventManager.Call(LogicEvent.ShowNineGrid, m_ID);
                        break;
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
            {
                return;
            }

            var prop = eventData.pointerDrag.GetComponent<UISpecialProp>();
            if (prop != null)
            {
                prop.m_TargetMiningGround = null;
                (int, bool) data = (m_ID, false);
                switch (prop.m_TreasureType)
                {
                    case MiningType.Bomb:
                        EventManager.Call(LogicEvent.HideCrossedGrid, data);
                        break;
                    case MiningType.Scope:
                        EventManager.Call(LogicEvent.HideNineGrid, data);
                        break;
                }
            }
        }

        public void HideMiningGround()
        {
            m_GroundMask.Hide();
            m_ParticleSystem.Play();
            m_IsCanClick = false;
            if ((MiningType)m_TreasureType == MiningType.Door)
            {
                m_IsDoorCanClick = true;
            }

            MiningManager.Ins.m_CommonMiningProp[m_ID].OnPropMatch();
        }
    }
}