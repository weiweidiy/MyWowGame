using DG.Tweening;
using Framework.EventKit;
using Logic.Common;
using Logic.Manager;
using Logic.UI.Cells;
using Logic.UI.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Logic.UI.UIMining
{
    public class UISpecialProp : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public MiningType m_TreasureType;
        private Image m_Image;
        private Vector3 m_OriginPosition;
        public CommonMiningGround m_TargetMiningGround;

        private Camera UICamera;

        private void Awake()
        {
            m_Image = this.GetComponent<Image>();
            m_OriginPosition = this.transform.position;
            UICommonHelper.LoadMiningType(m_Image, m_TreasureType);
            UICamera = GameObject.FindWithTag("UICamera").GetComponent<Camera>();
        }


        public void OnBeginDrag(PointerEventData eventData)
        {
            switch (m_TreasureType)
            {
                case MiningType.Bomb:
                    if (MiningManager.Ins.m_MiningData.m_BombCount <= 0)
                    {
                        eventData.pointerDrag = null;
                        EventManager.Call(LogicEvent.ShowTips, "没有足够的炸弹");
                        return;
                    }

                    break;
                case MiningType.Scope:
                    if (MiningManager.Ins.m_MiningData.m_ScopeCount <= 0)
                    {
                        eventData.pointerDrag = null;
                        EventManager.Call(LogicEvent.ShowTips, "没有足够的透视镜");
                        return;
                    }

                    break;
            }

            m_TargetMiningGround = null;
            m_Image.raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = UICamera.ScreenToWorldPoint(Input.mousePosition);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            m_Image.raycastTarget = true;

            if (m_TargetMiningGround == null)
            {
                transform.localPosition = m_OriginPosition;
            }
            else
            {
                transform.position = m_TargetMiningGround.transform.position;
                switch (m_TreasureType)
                {
                    case MiningType.Bomb:
                        UpdateBombEvent();
                        break;
                    case MiningType.Scope:
                        UpdateScopeEvent();
                        break;
                }
            }
        }

        private void UpdateBombEvent()
        {
            /*
             * TODO:
             * 爆炸特效
             */
            MiningManager.Ins.SendMsgC2SUpdateMiningData(MiningType.Bomb, MiningUpdateType.Reduce);
            DOTween.Sequence()
                .Append(transform.DOPunchScale(new Vector3(1.2f, 1.2f, 1f), 1f, 3))
                .AppendCallback(() =>
                {
                    (int, bool) data = (m_TargetMiningGround.m_ID, true);
                    EventManager.Call(LogicEvent.HideCrossedGrid, data);
                    transform.localPosition = m_OriginPosition;
                });
        }

        private void UpdateScopeEvent()
        {
            MiningManager.Ins.SendMsgC2SUpdateMiningData(MiningType.Scope, MiningUpdateType.Reduce);
            (int, bool) data = (m_TargetMiningGround.m_ID, true);
            EventManager.Call(LogicEvent.HideNineGrid, data);
            transform.localPosition = m_OriginPosition;
        }
    }
}