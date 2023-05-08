
using System;
using BreakInfinity;
using Configs;
using Cysharp.Threading.Tasks;
using Framework.EventKit;
using Framework.Extension;
using Framework.UI;
using Logic.Common;
using Logic.Manager;
using Logic.UI.Cells;
using Logic.UI.UIMain;
using Networks;
using TMPro;
using UnityEngine;

namespace Logic.UI.UIUser
{
    public class PartPartner : MonoBehaviour
    {
        public CommonOnItem[] m_OnItems;
        public GameObject m_ItemPrefabObj;
        public Transform m_ItemListRoot;
        public TextMeshProUGUI m_HaveEffect;

        private EventGroup m_EventGroup = new EventGroup();
        private void Awake()
        {
            //监听解锁事件
            //TODO m_EventGroup
            
            m_EventGroup.Register(LogicEvent.PartnerOn, OnPartnerOn);
            m_EventGroup.Register(LogicEvent.PartnerOff, OnPartnerOff);
            m_EventGroup.Register(LogicEvent.PartnerAllEffectUpdate, (i, o) => { m_HaveEffect.text = "攻击力+" + ((BigDouble)(PartnerManager.Ins.AllHaveEffect * 100)).ToUIStringFloat() + "%";  });
        }

        private void OnDestroy()
        {
            m_EventGroup.Release();
        }

        private void Start()
        {
            var _OnList = PartnerManager.Ins.PartnerOnList;
            for (int i = 0; i < 5; i++)
            {
                if(_OnList[i] == 0)
                    m_OnItems[i].Reset();
                else
                    m_OnItems[i].InitByPartner(PartnerCfg.GetData(_OnList[i]));
            }
            
            //初始化技能列表
            for (int i = 4001; i <= GameDefine.PartnerMaxID; i++)
            {
                var _PartnerData = PartnerCfg.GetData(i);
                if (_PartnerData == null) break;
                
                var _Obj = m_ItemPrefabObj.Clone(Vector3.zero, m_ItemListRoot, Quaternion.identity);
                var _Item = _Obj.GetComponent<CommonItem>();
                _Item.InitByPartner(_PartnerData);
                _Item.m_ClickCB += OnClickPartnerItem;
                _Obj.Show();
                
                //await UniTask.NextFrame();   
            }
            
            m_HaveEffect.text = "攻击力+" + ((BigDouble)(PartnerManager.Ins.AllHaveEffect * 100)).ToUIStringFloat() + "%";
        }

        public void OnClickAuto()
        {
            if(PartnerManager.Ins.HasOneCanUpgrade())
                PartnerManager.Ins.DoIntensify(0, true);
            else
                EventManager.Call(LogicEvent.ShowTips, "没有可升级的技能");
        }
        
        public void OnClickGet()
        {
            UIBottomMenu.Ins.ClickBtn(BottomBtnType.Shop);
        }

        private async void OnClickPartnerItem(CommonItem pItem)
        {
            await UIManager.Ins.OpenUI<UIPartnerInfo>(pItem.m_PartnerData.ID);
        }
        
        private void OnPartnerOn(int arg1, object arg2)
        {
            var Index = ((S2C_PartnerOn)arg2).m_Index;
            var _OnList = PartnerManager.Ins.PartnerOnList;
            m_OnItems[Index].InitByPartner(PartnerCfg.GetData(_OnList[Index]));
        }

        private void OnPartnerOff(int arg1, object arg2)
        {
            var Index = ((S2C_PartnerOff)arg2).m_Index;
            m_OnItems[Index].Reset();
        }
    }
}
