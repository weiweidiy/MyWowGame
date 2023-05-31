
using Logic.UI.Common;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.UISpoil
{
    public class SpoilUnitView : MonoBehaviour
    {
        [SerializeField] GameObject m_Selection;
        [SerializeField] GameObject m_BgUnHold;
        //[SerializeField] GameObject m_BgUnHoldMask;
        [SerializeField] GameObject m_BgHold;
        [SerializeField] GameObject m_IconEquiped;
        [SerializeField] Image m_IconSpoil;
        [SerializeField] Material m_Grey;

        Material m_OriginMat;

        public event Action<SpoilUnitVO> onSelected;

        SpoilUnitVO m_VO;

        public bool Selected { get; private set; }

        private void Awake()
        {
            m_OriginMat = m_IconSpoil.material;
        }

        public void OnClicked()
        {
            if (!Selected)
            {
                Select(true);
            }

        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="vo"></param>
        public void Refresh(SpoilUnitVO vo)
        {
            m_VO = vo;

            m_Selection.SetActive(Selected);
            m_BgUnHold.SetActive(!vo.hold);
            //m_BgUnHoldMask.SetActive(!vo.hold);
            m_IconSpoil.material = vo.hold ? m_OriginMat : m_Grey;
            m_BgHold.SetActive(vo.hold);
            m_IconEquiped.SetActive(vo.equiped);

            UICommonHelper.LoadIcon(m_IconSpoil, vo.iconPath);
        }

        /// <summary>
        /// 选中
        /// </summary>
        /// <param name="select"></param>
        public void Select(bool select)
        {
            m_Selection.SetActive(select);
            Selected = select;

            if (select)
                onSelected?.Invoke(m_VO);
        }


        public int GetId()
        {
            return m_VO.spoilId;
        }

        public bool GetHold()
        {
            return m_VO.hold;
        }

        public bool GetEquiped()
        {
            return m_VO.equiped;
        }
    }
   
}


