using Framework.Extension;
using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.UIUser
{
    public enum UserPageType
    {
        None = -1,
        Role = 0, //统领页签
        Skill, // 技能页签
        Partner, // 伙伴页签
        Trophy, //战利品页签
        Quenching, // 淬炼页签
    }

    /// <summary>
    /// 主角界面
    /// </summary>
    public class UIUser : UIPage
    {
        public GameObject RoleNode;
        public GameObject SkillNode;
        public GameObject PartnerNode;
        public GameObject TrophyNode;
        public GameObject QuenchingNode;

        public Toggle[] Toggles;

        public static UserPageType m_OpenType = UserPageType.None;

        public override void OnShow()
        {
            if (m_OpenType == UserPageType.None) return;

            Toggles[(int)m_OpenType].isOn = true;
            m_OpenType = UserPageType.None;
        }

        public void OnToggleRole(bool pOn)
        {
            if (pOn)
            {
                RoleNode.Show();
            }
            else
            {
                RoleNode.Hide();
            }
        }

        public void OnToggleSkill(bool pOn)
        {
            if (pOn)
            {
                SkillNode.Show();
            }
            else
            {
                SkillNode.Hide();
            }
        }

        public void OnTogglePartner(bool pOn)
        {
            if (pOn)
            {
                PartnerNode.Show();
            }
            else
            {
                PartnerNode.Hide();
            }
        }

        public void OnToggleTrophy(bool pOn)
        {
            if (pOn)
            {
                TrophyNode.Show();
            }
            else
            {
                TrophyNode.Hide();
            }
        }

        public void OnToggleQuenching(bool pOn)
        {
            if (pOn)
            {
                QuenchingNode.Show();
            }
            else
            {
                QuenchingNode.Hide();
            }
        }
    }
}