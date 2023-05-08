using System.Collections;
using System.Collections.Generic;
using Framework.Extension;
using Framework.UI;
using Logic.UI.Cells;
using Networks;
using UnityEngine;

namespace Logic.UI.UIUser
{
    public class UIUpgradedInfo : UIPage
    {
        public GameObject m_ItemObj;
        public Transform m_ItemRoot;

        private List<GameObject> m_ItemObjList = new List<GameObject>(64);

        private bool m_IsOnShow;

        public override void OnShow()
        {
            StartCoroutine(OnShowUpgradedInfoCoroutine());
        }

        private IEnumerator OnShowUpgradedInfoCoroutine()
        {
            var delay = new WaitForSeconds(0.02f);
            m_IsOnShow = true;

            if (m_OpenData_ is List<GameSkillUpgradeData>)
            {
                var _List = (List<GameSkillUpgradeData>)m_OpenData_;
                foreach (var id in _List)
                {
                    var _Obj = Instantiate(m_ItemObj, m_ItemRoot.transform);
                    _Obj.GetComponent<CommonShowUpgradedItem>().InitBySkill(id);
                    _Obj.Show();
                    m_ItemObjList.Add(_Obj);
                    yield return delay;
                }

                m_IsOnShow = false;
            }

            if (m_OpenData_ is List<GamePartnerUpgradeData>)
            {
                var _List = (List<GamePartnerUpgradeData>)m_OpenData_;
                foreach (var id in _List)
                {
                    var _Obj = Instantiate(m_ItemObj, m_ItemRoot.transform);
                    _Obj.GetComponent<CommonShowUpgradedItem>().InitByPartner(id);
                    _Obj.Show();
                    m_ItemObjList.Add(_Obj);
                    yield return delay;
                }

                m_IsOnShow = false;
            }

            if (m_OpenData_ is List<GameEquipUpgradeData>)
            {
                var _List = (List<GameEquipUpgradeData>)m_OpenData_;
                foreach (var id in _List)
                {
                    var _Obj = Instantiate(m_ItemObj, m_ItemRoot.transform);
                    _Obj.GetComponent<CommonShowUpgradedItem>().InitByEquip(id);
                    _Obj.Show();
                    m_ItemObjList.Add(_Obj);
                    yield return delay;
                }

                m_IsOnShow = false;
            }
        }

        public void OnClickCloseBtn()
        {
            if (m_IsOnShow) return;

            foreach (var Obj in m_ItemObjList)
            {
                Obj.Destroy();
            }

            Hide();
        }
    }
}