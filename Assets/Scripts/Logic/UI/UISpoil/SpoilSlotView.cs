using Configs;
using Logic.UI.Common;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.UISpoil
{
    public class SpoilSlotView : MonoBehaviour
    {
        public event Action<SpoilSlotVO> onClicked;

        [SerializeField] int m_SlotId;

        [SerializeField] Image m_BgLocked;

        [SerializeField] Image m_BgUnlocked;

        [SerializeField] Image m_BgEquiped;

        [SerializeField] Image m_FlagLocked;

        [SerializeField] Image m_FlagEquiped;

        [SerializeField] Image m_Icon;

        [SerializeField] TextMeshProUGUI m_Level;

        [SerializeField] Button m_Button;

        [SerializeField] GameObject m_GoBreak;

        [SerializeField] TextMeshProUGUI m_TxtBreakCount;

        SpoilSlotVO m_VO;


        public void Refresh(SpoilSlotVO vo)
        {
            m_VO = vo;
            m_BgLocked.gameObject.SetActive(vo.state == SpoilSlotVO.State.Locked);
            m_BgUnlocked.gameObject.SetActive(vo.state == SpoilSlotVO.State.Unlocked);
            m_BgEquiped.gameObject.SetActive(vo.state == SpoilSlotVO.State.Equiped);
            m_FlagLocked.gameObject.SetActive(vo.state == SpoilSlotVO.State.Locked);
            m_FlagEquiped.gameObject.SetActive(vo.state == SpoilSlotVO.State.Equiped);
            m_Icon.gameObject.SetActive(vo.state == SpoilSlotVO.State.Equiped);
            m_GoBreak.SetActive(vo.breakCount > 0);
            m_TxtBreakCount.text = vo.breakCount.ToString();
            //m_Level.gameObject.SetActive(vo.state == SpoilSlotVO.State.Equiped);

            switch (vo.state)
            {
                case SpoilSlotVO.State.Locked:
                    m_Level.text = "未获得";
                    break;
                case SpoilSlotVO.State.Unlocked:
                    m_Level.text = "未装配";
                    break;
                case SpoilSlotVO.State.Equiped:
                    m_Level.text = "Lv. " + vo.spoilLevel.ToString();
                    break;
            }
            

            if(vo.iconPath != null)
                UICommonHelper.LoadIcon(m_Icon, vo.iconPath);
        }

        /// <summary>
        /// prefab上的button回调
        /// </summary>
        public void OnButtonClick()
        {
            onClicked?.Invoke(m_VO);

        }

        public int GetId()
        {
            return m_SlotId;
        }


    }
}