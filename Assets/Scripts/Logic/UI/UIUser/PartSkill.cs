using BreakInfinity;
using Configs;
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
    public class PartSkill : MonoBehaviour
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

            m_EventGroup.Register(LogicEvent.SkillOn, OnSkillOn);
            m_EventGroup.Register(LogicEvent.SkillOff, OnSkillOff);
            m_EventGroup.Register(LogicEvent.SkillAllEffectUpdate,
                (i, o) =>
                {
                    m_HaveEffect.text = "攻击力+" + ((BigDouble)(SkillManager.Ins.AllHaveEffect * 100)).ToUIStringFloat() +
                                        "%";
                });
        }

        private void OnDestroy()
        {
            m_EventGroup.Release();
        }

        private void Start()
        {
            var _OnList = SkillManager.Ins.SkillOnList;
            for (int i = 0; i < 6; i++)
            {
                if (_OnList[i] == 0)
                {
                    m_OnItems[i].Reset();
                }
                else
                {
                    m_OnItems[i].InitBySkill(SkillCfg.GetData(_OnList[i]));
                }
            }

            //初始化技能列表
            for (int i = 3001; i <= GameDefine.SkillMaxID; i++)
            {
                var _SkillData = SkillCfg.GetData(i);
                if (_SkillData == null) break;

                var _Obj = m_ItemPrefabObj.Clone(Vector3.zero, m_ItemListRoot, Quaternion.identity);
                var _Item = _Obj.GetComponent<CommonItem>();
                _Item.InitBySkill(_SkillData);
                _Item.m_ClickCB += OnClickSKillItem;
                _Obj.Show();

                //await UniTask.NextFrame();   
            }

            m_HaveEffect.text = "攻击力+" + ((BigDouble)(SkillManager.Ins.AllHaveEffect * 100)).ToUIStringFloat() + "%";
        }

        public void OnClickAuto()
        {
            if (SkillManager.Ins.HasOneCanUpgrade())
            {
                SkillManager.Ins.DoIntensify(0, true);
            }
            else
            {
                EventManager.Call(LogicEvent.ShowTips, "没有可升级的技能");
            }
        }

        public void OnClickGet()
        {
            UIBottomMenu.Ins.ClickBtn(BottomBtnType.Shop);
        }

        private async void OnClickSKillItem(CommonItem pItem)
        {
            await UIManager.Ins.OpenUI<UISkillInfo>(pItem.m_SkillData.ID);
        }

        private void OnSkillOn(int arg1, object arg2)
        {
            var Index = ((S2C_SkillOn)arg2).Index;
            var _OnList = SkillManager.Ins.SkillOnList;
            m_OnItems[Index].InitBySkill(SkillCfg.GetData(_OnList[Index]));
        }

        private void OnSkillOff(int arg1, object arg2)
        {
            var Index = ((S2C_SkillOff)arg2).Index;
            m_OnItems[Index].Reset();
        }
    }
}