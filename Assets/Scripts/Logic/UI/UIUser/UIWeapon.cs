using System.Collections.Generic;
using BreakInfinity;
using Configs;
using Cysharp.Threading.Tasks;
using Framework.EventKit;
using Framework.Extension;
using Framework.UI;
using Logic.Common;
using Logic.Manager;
using Logic.UI.Cells;
using Logic.UI.Common;
using Logic.UI.UIMain;
using Networks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.UIUser
{
    public class UIWeapon : UIPage
    {
        public Image m_Quality;
        public TextMeshProUGUI m_QualityText;
        public Image m_Icon;
        public Image m_CantProcess;
        public Image m_CanProcess;
        public TextMeshProUGUI m_TextProcess;
        public TextMeshProUGUI m_Level;
        public TextMeshProUGUI m_EquipName;
        public TextMeshProUGUI m_HaveEffect;
        public TextMeshProUGUI m_EquipEffect;
        
        public GameObject m_ItemPrefabObj;
        public Transform m_ItemListRoot;
        public ScrollViewNavigation m_ScrollViewNavigation;
        public TextMeshProUGUI m_ALLHaveEffect;
        
        public GameObject m_NotHaveNode;
        public GameObject m_BtnUpgrade;
        public GameObject m_BtnOn;
        public GameObject m_BtnOff;

        public GameObject m_UpArrow;
        public TextMeshProUGUI m_UpEquipEffect;
        public GameObject m_DownArrow;
        public TextMeshProUGUI m_DownEquipEffect;

        private CommonItem m_CurSelectItem;
        private Dictionary<int, CommonItem> m_ItemDic = new Dictionary<int, CommonItem>(64);
        private void Awake()
        {
            m_EventGroup.Register(LogicEvent.EquipOn, (i, o) =>
            {
                var _Msg = (S2C_EquipOn)o;
                if(_Msg.m_Type == ItemType.Weapon)
                    UpdateEquip();
            });
            m_EventGroup.Register(LogicEvent.EquipWeaponUpgraded, (i, o) => { UpdateEquip(); });
            m_EventGroup.Register(LogicEvent.EquipAllATKEffectUpdate, (i, o) =>
            {
                m_ALLHaveEffect.text = "攻击力+"+((BigDouble)(EquipManager.Ins.AllHaveATKEffect * 100)).ToUIStringFloat() + "%";
            });
            m_EventGroup.Register(LogicEvent.EquipListChanged, (i, o) =>
            {
                if(ItemType.Weapon == (ItemType)o)
                    UpdateEquip();
            });
        }

        public override void OnDestroy()
        {
            m_EventGroup.Release();
        }

        private void OnEnable()
        {
            if(!IsFirst)
                AutoSelectOne();
        }

        private bool IsFirst = true;
        private async void Start()
        {
            //初始化技能列表
            for (int i = 1001; i <= GameDefine.WeaponMaxID; i++)
            {
                var _WeaponData = EquipCfg.GetData(i);
                if (_WeaponData == null) break;
                
                var _Obj = m_ItemPrefabObj.Clone(Vector3.zero, m_ItemListRoot, Quaternion.identity);
                _Obj.Show();
                var _Item = _Obj.GetComponent<CommonItem>();
                _Item.InitByEquip(_WeaponData);
                _Item.m_ClickCB += OnClickEquipItem;
                
                if(i == EquipManager.Ins.CurWeaponOnID)
                    _Item.OnClickItem();
                
                m_ItemDic.Add(i, _Item);
            }

            IsFirst = false;
            m_ALLHaveEffect.text = "攻击力+"+((BigDouble)(EquipManager.Ins.AllHaveATKEffect * 100)).ToUIStringFloat() + "%";

            await UniTask.NextFrame();
            AutoSelectOne();
        }
        
        private void AutoSelectOne()
        {
            if (EquipManager.Ins.CurWeaponOnID == 0)
            {
                m_ItemDic[1001].OnClickItem();
                m_ScrollViewNavigation.Navigate(m_ItemDic[1001].GetComponent<RectTransform>());
            }
            else
            {
                m_ItemDic[EquipManager.Ins.CurWeaponOnID].OnClickItem();
                m_ScrollViewNavigation.Navigate(m_ItemDic[EquipManager.Ins.CurWeaponOnID].GetComponent<RectTransform>());
            }
        }
        
        private void OnClickEquipItem(CommonItem pItem)
        {
            if(m_CurSelectItem != null)
                m_CurSelectItem.HideSelected();
            m_CurSelectItem = pItem;
            m_CurSelectItem.ShowSelected();
            UpdateEquip();
        }

        private bool IsHave = false;
        private void UpdateEquip()
        {
            var _EquipData = m_CurSelectItem.m_EquipData;
            IsHave = EquipManager.Ins.IsHave(_EquipData.ID, ItemType.Weapon);
            GameEquipData _GameEquipData = null;
            if(IsHave)
                _GameEquipData = EquipManager.Ins.GetEquipData(_EquipData.ID, ItemType.Weapon);
            else
                _GameEquipData = new GameEquipData { m_EquipID = _EquipData.ID, m_Level = 1, m_Count = 0}; //没有按1级算

            var itemData = ItemCfg.GetData(_GameEquipData.m_EquipID);
            UICommonHelper.LoadIcon(m_Icon, itemData.Res);
            UICommonHelper.LoadQuality(m_Quality, _EquipData.Quality);
            m_QualityText.text = UICommonHelper.GetQualityShowText(_EquipData.Quality);
            
            m_EquipName.text = _EquipData.EquipName;
            m_Level.text = "LV" + _GameEquipData.m_Level;
            
            var _CurCount = EquipManager.Ins.CurCount(_EquipData.ID, ItemType.Weapon);
            var _NeedCount = EquipManager.Ins.NeedCount(_EquipData.ID, ItemType.Weapon);
            if (_CurCount >= _NeedCount)
            {
                m_CantProcess.Hide();
                m_CanProcess.Show();
            }
            else
            {
                m_CantProcess.Show();
                m_CanProcess.Hide();
            }
            
            float _Process = (float)_CurCount / _NeedCount;
            m_CantProcess.fillAmount = _Process;
            m_CanProcess.fillAmount = _Process;

            m_TextProcess.text = _CurCount + "/" + _NeedCount;
            
            m_HaveEffect.text = "+" + ((BigDouble)(EquipManager.Ins.GetHaveEffect(_EquipData.ID, ItemType.Weapon) * 100f)).ToUIStringFloat() + "%";
            m_EquipEffect.text = "+" + ((BigDouble)(EquipManager.Ins.GetEquipEffect(_EquipData.ID, ItemType.Weapon) * 100f)).ToUIStringFloat() + "%";
            
            //Up - Down
            if (EquipManager.Ins.CurWeaponOnID == 0 || EquipManager.Ins.CurWeaponOnID == _EquipData.ID)
            {
                m_UpArrow.Hide();
                m_DownArrow.Hide();
            }
            else
            {
                var _CurEffect = EquipManager.Ins.GetEquipEffect(_EquipData.ID, ItemType.Weapon);
                var _EquipedEffect = EquipManager.Ins.GetEquipEffect(EquipManager.Ins.CurWeaponOnID, ItemType.Weapon);
                if (_CurEffect > _EquipedEffect)
                {
                    m_UpArrow.Show();
                    m_DownArrow.Hide();
                    m_UpEquipEffect.text = ((BigDouble)((_CurEffect - _EquipedEffect) * 100f)).ToUIStringFloat() + "%";
                }
                else
                {
                    m_UpArrow.Hide();
                    m_DownArrow.Show();
                    m_DownEquipEffect.text = ((BigDouble)((_EquipedEffect - _CurEffect) * 100f)).ToUIStringFloat() + "%";
                }
            }
            
            //Btns
            if (IsHave)
            {
                m_NotHaveNode.Hide();
                m_BtnUpgrade.Show();
                if(EquipManager.Ins.CurWeaponOnID == _EquipData.ID)
                {
                    m_BtnOn.Hide();
                    m_BtnOff.Show();
                }
                else
                {
                    m_BtnOn.Show();
                    m_BtnOff.Hide();
                }
            }
            else
            {
                m_NotHaveNode.Show();
                m_BtnUpgrade.Hide();
                m_BtnOn.Hide();
                m_BtnOff.Hide();
            }
        }

        #region 按钮事件
        
        public void OnClickOn()
        {
            if (m_CurSelectItem == null) return;
            var _EquipData = m_CurSelectItem.m_EquipData;
            if (EquipManager.Ins.CurWeaponOnID == _EquipData.ID) return;
            EquipManager.Ins.DoOn(_EquipData.ID, ItemType.Weapon);
        }
        
        public void OnClickUpgrade()
        {
            if (m_CurSelectItem == null) return;
            if (!IsHave) return;
            
            var _EquipData = m_CurSelectItem.m_EquipData;
            
            var _CurCount = EquipManager.Ins.CurCount(_EquipData.ID, ItemType.Weapon);
            var _NeedCount = EquipManager.Ins.NeedCount(_EquipData.ID, ItemType.Weapon);
            if (_CurCount < _NeedCount)
            {
                EventManager.Call(LogicEvent.ShowTips, "数量不足");
                return;
            }
            
            EquipManager.Ins.DoIntensify(_EquipData.ID, ItemType.Weapon, false);
        }

        public void OnClickGet()
        {
            Hide();
            UIBottomMenu.Ins.ClickBtn(BottomBtnType.Shop);
        }

        public void OnClickAuto()
        {
            if(EquipManager.Ins.HaveCanUpgradeEquip(ItemType.Weapon))
                EquipManager.Ins.DoIntensify(0, ItemType.Weapon, true);
            else
                EventManager.Call(LogicEvent.ShowTips, "没有可升级的武器");
        }
        #endregion
    }
}