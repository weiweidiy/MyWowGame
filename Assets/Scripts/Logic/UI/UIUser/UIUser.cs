using Framework.Extension;
using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.UIUser
{
    public enum UserPageType
    {
        None = -1,
        Role = 0,
        Skill,
        Partner,
        Gift,
        Remould,
    }
    
    /// <summary>
    /// 主角界面
    /// </summary>
    public class UIUser : UIPage
    {
        public GameObject RoleNode;
        public GameObject SkillNode;
        public GameObject PartnerNode;
        public GameObject GiftNode;
        public GameObject RemouldNode;
        
        public Toggle[] Toggles;
        
        public static UserPageType m_OpenType = UserPageType.None;
        public override void OnShow()
        {
            if (m_OpenType == UserPageType.None)
                return;
            
            Toggles[(int)m_OpenType].isOn = true;
            m_OpenType = UserPageType.None;
        }

        public void OnToggleRole(bool pOn)
        {
            if(pOn) 
                RoleNode.Show(); 
            else 
                RoleNode.Hide();   
        }
        
        public void OnToggleSkill(bool pOn) 
        {
            if(pOn) 
                SkillNode.Show(); 
            else 
                SkillNode.Hide();   
        }
        
        public void OnTogglePartner(bool pOn)
        {
            if(pOn) 
                PartnerNode.Show(); 
            else 
                PartnerNode.Hide();   
        }
        
        public void OnToggleGift(bool pOn)
        {
            if(pOn) 
                GiftNode.Show(); 
            else 
                GiftNode.Hide();   
        }
        
        public void OnToggleRemould(bool pOn)
        {
            if(pOn) 
                RemouldNode.Show(); 
            else 
                RemouldNode.Hide();   
        }
    }
}
