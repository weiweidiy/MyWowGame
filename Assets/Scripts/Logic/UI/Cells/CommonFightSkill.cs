
using Configs;
using Framework.EventKit;
using Framework.Extension;
using Logic.Common;
using Logic.Data;
using Logic.Fight.Common;
using Logic.Manager;
using Logic.UI.Common;
using Logic.UI.UIMain;
using Logic.UI.UIUser;
using Networks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.Cells
{
    /// <summary>
    /// 战斗技能格子
    /// </summary>
    public class CommonFightSkill : MonoBehaviour
    {
        [LabelText("技能索引")]
        public int m_Index;
        public GameObject m_Empty;
        
        public GameObject m_DisableBK;
        public GameObject m_DoingBK;
        public Image m_DisableMask;
        public Image m_DoingMask;
        
        public Image m_SkillIcon;
        public Material m_GrayMaterial;

        private readonly EventGroup m_EventGroup = new();
        private SkillData m_SkillData;
        private GameSkillData m_GameSkillData;
        private ItemData m_ItemData;
        
        private void OnEnable()
        {
            m_EventGroup.Register(LogicEvent.SkillOn, OnSkillOn).
                Register(LogicEvent.SkillOff, OnSkillOff).
                Register(LogicEvent.SkillReset, (i, o) => { ResetSkill(); }).
                Register(LogicEvent.SkillAutoPlay, (i, o) =>
                {
                    if(m_SkillData != null && (int)o == m_SkillData.ID)
                        Play();
                });
            
 
            Refresh();
        }
        
        private void Refresh()
        {
            ResetSkill();
            if (SkillManager.Ins.SkillOnList[m_Index] == 0)
            {
                m_SkillData = null;
                m_Empty.Show();
                m_SkillIcon.Hide();
            }
            else
            {
                m_SkillData = SkillCfg.GetData(SkillManager.Ins.SkillOnList[m_Index]);
                m_GameSkillData = SkillManager.Ins.GetSkillData(m_SkillData.ID);
                m_ItemData = ItemCfg.GetData(m_SkillData.ID);
                UICommonHelper.LoadIcon(m_SkillIcon, m_ItemData.Res);
                m_Empty.Hide();
                m_SkillIcon.Show();
            }
        }

        private void OnDisable()
        {
            m_EventGroup.Release();
        }

        private bool m_IsDoing = false;
        private bool m_IsDisable = false;
        private float m_CurTime = 0;
        //UI 播放技能释放流程
        public void Play()
        {
            m_CurTime = 0;
            m_IsDoing = true;
        }

        public void ResetSkill()
        {
            m_IsDoing = false;
            m_IsDisable = false;
            m_CurTime = 0;
                    
            m_DoingBK.Hide();
            m_DoingMask.Hide();
            m_DisableBK.Hide();
            m_DisableMask.Hide();

            m_SkillIcon.material = null;
        }

        private void Update()
        {
            if (m_IsDoing)
            {
                m_CurTime += Time.deltaTime;
                m_DoingMask.fillAmount = (m_SkillData.DurationTime - m_CurTime) / m_SkillData.DurationTime;
                if (m_CurTime >= m_SkillData.DurationTime)
                {
                    m_IsDoing = false;
                    m_IsDisable = true;
                    m_CurTime = 0;
                    return;
                }

                InDoing();
            }
            else if(m_IsDisable)
            {
                if (m_SkillIcon.material != null)
                    m_SkillIcon.material = m_GrayMaterial;
                m_CurTime += Time.deltaTime;
                m_DisableMask.fillAmount = (m_SkillData.CD - m_CurTime) / m_SkillData.CD;
                if (m_CurTime >= m_SkillData.CD)
                {
                    ResetSkill();
                    return;
                }

                InDisable();
            }
        }

        private void InDoing()
        {
            m_DoingBK.Show();
            m_DoingMask.Show();
            m_DisableBK.Hide();
            m_DisableMask.Hide();
        }
        
        private void InDisable()
        {
            m_DoingBK.Hide();
            m_DoingMask.Hide();
            m_DisableBK.Show();
            m_DisableMask.Show();
        }

        public void OnClickSkill()
        {
            if (m_SkillData == null)
            {
                UIUser.UIUser.m_OpenType = UserPageType.Skill;
                UIBottomMenu.Ins.ClickBtn(BottomBtnType.User);
                return;
            }

            if (GameDataManager.Ins.AutoSkill)
                return;

            if(m_IsDisable || m_IsDoing)
                return;
            
            Play();
            FightSkillManager.Ins.CastSkill(m_SkillData.ID);
        }

        #region 上下阵处理

        private void OnSkillOn(int i, object o)
        {
            var _Data = o as S2C_SkillOn;
            if(_Data.m_Index != m_Index) return;

            Refresh();
            m_IsDisable = true;
        }
        
        private void OnSkillOff(int i, object o)
        {
            var _Data = o as S2C_SkillOff;
            if(_Data.m_Index != m_Index) return;

            Refresh();
        }

        #endregion
    }
}
