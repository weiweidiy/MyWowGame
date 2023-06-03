using Configs;
using Framework.EventKit;
using Framework.Extension;
using Framework.UI;
using Logic.Common;
using Logic.Manager;
using Logic.UI.Common;
using Networks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.UIUser
{
    public class PartRole : MonoBehaviour
    {
        [Header("武器")] public GameObject m_WeaponEmpty;
        public GameObject m_WeaponNode;
        public Image m_WQuality;
        public Image m_WIcon;
        public TextMeshProUGUI m_WLevel;

        [Header("防具")] public GameObject m_ArmorEmpty;
        public GameObject m_ArmorNode;
        public Image m_AQuality;
        public Image m_AIcon;
        public TextMeshProUGUI m_ALevel;

        [Header("引擎")] public GameObject m_EngineEmpty;
        public GameObject m_EngineNode;
        public Image m_EQuality;
        public Image m_EIcon;
        public TextMeshProUGUI m_ELevel;

        [Header("属性")] public TextMeshProUGUI m_ATKValue;
        public TextMeshProUGUI m_HPValue;
        public TextMeshProUGUI m_HPRecoverValue;
        public TextMeshProUGUI m_SpeedValue;
        public TextMeshProUGUI m_CriticalValue;
        public TextMeshProUGUI m_CriticalDamageValue;
        public TextMeshProUGUI m_DoubleHitValue;
        public TextMeshProUGUI m_TripletHitValue;
        public TextMeshProUGUI m_EvasionRateValue;
        public TextMeshProUGUI m_CompanionDamageValue;
        public TextMeshProUGUI m_CompanionASPDValue;
        public TextMeshProUGUI m_SkillDamageValue;
        public TextMeshProUGUI m_SkillCooldownValue;
        public TextMeshProUGUI m_GoldObtainValue;
        public TextMeshProUGUI m_BossDamageAmountValue;
        public TextMeshProUGUI m_BattleDurationValue;
        public TextMeshProUGUI m_HPRecoverEverySecondValue;
        public TextMeshProUGUI m_MultipleShotValue;
        public TextMeshProUGUI m_ResearchHammerLimitValue;
        public TextMeshProUGUI m_ResearchMineObtainAmountValue;
        public TextMeshProUGUI m_ResearchHammerRecoverSpeedValue;
        public TextMeshProUGUI m_ResearchSpeedValue;


        private EventGroup m_EventGroup = new EventGroup();

        private void Awake()
        {
            m_EventGroup.Register(LogicEvent.EquipOn, (i, o) =>
            {
                var _Msg = (S2C_EquipOn)o;
                var type = (ItemType)_Msg.Type;
                if (type == ItemType.Weapon)
                {
                    UpdateWeaponNode();
                    UpdateAttrInfo();
                }
                else if (type == ItemType.Armor)
                {
                    UpdateArmorNode();
                    UpdateAttrInfo();
                }
            });

            m_EventGroup.Register(LogicEvent.EquipWeaponUpgraded, (i, o) => UpdateWeaponNode());
            m_EventGroup.Register(LogicEvent.EquipArmorUpgraded, (i, o) => UpdateArmorNode());
            m_EventGroup.Register(LogicEvent.EquipAllATKEffectUpdate, (i, o) => UpdateAttrInfo());
            m_EventGroup.Register(LogicEvent.EquipAllHPEffectUpdate, (i, o) => UpdateAttrInfo());
            m_EventGroup.Register(LogicEvent.EquipListChanged, (i, o) =>
            {
                if (ItemType.Weapon == (ItemType)o)
                    UpdateWeaponNode();
                else if (ItemType.Armor == (ItemType)o)
                    UpdateArmorNode();
            });

            // 引擎
            m_EventGroup.Register(LogicEvent.EngineOn, (i, o) => { UpdateEngineNode(); });
            m_EventGroup.Register(LogicEvent.EngineOff, (i, o) => { UpdateEngineNode(); });
            m_EventGroup.Register(LogicEvent.EngineAllEffectUpdate, (i, o) => UpdateAttrInfo());
            m_EventGroup.Register(LogicEvent.EngineIntensify, (i, o) => UpdateEngineNode());
            //研究
            m_EventGroup.Register(LogicEvent.ResearchCompleteEffectUpdate, (i, o) => { UpdateAttrInfo(); });
            m_EventGroup.Register(LogicEvent.QuenchingEffectUpdate, (i, o) => { UpdateAttrInfo(); });
            //战利品
            m_EventGroup.Register(LogicEvent.OnSpoilDraw, (i, o) => { UpdateAttrInfo(); });
            m_EventGroup.Register(LogicEvent.OnSpoilUpgrade, (i, o) => { UpdateAttrInfo(); });
        }

        private void OnDestroy()
        {
            m_EventGroup.Release();
        }

        private void OnEnable()
        {
            UpdateAttrInfo();
        }

        private void UpdateAttrInfo()
        {
            m_ATKValue.text = Formula.GetGJJAtk().ToUIString();
            m_HPValue.text = Formula.GetGJJHP().ToUIString();
            m_HPRecoverValue.text = Formula.GetGJJHPRecover().ToUIString();
            m_SpeedValue.text = Formula.GetGJJAtkSpeed().ToString("F2") + "";
            m_CriticalValue.text = (Formula.GetGJJCritical() * 100) + "%";
            m_CriticalDamageValue.text = (Formula.GetGJJCriticalDamage() * 100).ToUIStringFloat() + "%";
            m_DoubleHitValue.text = Formula.GetGJJDoubleHit() + "%";
            m_TripletHitValue.text = Formula.GetGJJTripletHit() + "%";
            m_EvasionRateValue.text = (Formula.GetGJJEvasionRate() * 100) + "%";
            m_CompanionDamageValue.text = (Formula.GetCompanionDamage() * 100) + "%";
            m_CompanionASPDValue.text = Formula.GetCompanionASPD() + "";
            m_SkillDamageValue.text = (Formula.GetSkillDamage() * 100) + "%";
            m_SkillCooldownValue.text = (Formula.GetSkillCooldown() * 100) + "%";
            m_GoldObtainValue.text = (Formula.GetGoldObtain() * 100) + "%";
            m_BossDamageAmountValue.text = (Formula.GetBossDamageAmount() * 100) + "%";
            m_BattleDurationValue.text = Formula.GetBattleDuration() + "";
            m_HPRecoverEverySecondValue.text = (Formula.GetGJJHPRecoverEverySecond() * 100) + "%";
            m_MultipleShotValue.text = (Formula.GetGJJMultipleShot() * 100) + "%";
            m_ResearchHammerLimitValue.text = Formula.GetResearchHammerLimit() + "";
            m_ResearchMineObtainAmountValue.text = (Formula.GetResearchMineObtainAmount() * 100) + "%";
            m_ResearchHammerRecoverSpeedValue.text = (Formula.GetResearchHammerRecoverSpeed() * 100) + "%";
            m_ResearchSpeedValue.text = (Formula.GetResearchSpeed() * 100) + "%";
        }

        private void Start()
        {
            UpdateWeaponNode();
            UpdateArmorNode();
            UpdateEngineNode();
        }

        private void UpdateWeaponNode()
        {
            if (EquipManager.Ins.CurWeaponOnID == 0)
            {
                m_WeaponEmpty.Show();
                m_WeaponNode.Hide();
            }
            else
            {
                m_WeaponEmpty.Hide();
                var curWeaponOnID = EquipManager.Ins.CurWeaponOnID;
                var itemData = ItemCfg.GetData(curWeaponOnID);
                var weaponData = EquipCfg.GetData(curWeaponOnID);
                var weaponGameData = EquipManager.Ins.GetEquipData(curWeaponOnID, ItemType.Weapon);
                UICommonHelper.LoadIcon(m_WIcon, itemData.Res);
                UICommonHelper.LoadQuality(m_WQuality, weaponData.Quality);
                if (EquipManager.Ins.IsMaxLevel(curWeaponOnID, ItemType.Weapon))
                {
                    //服务器处理，客户端可以不处理
                    m_WLevel.text = "LV" + GameDefine.CommonItemMaxLevel;
                }
                else
                {
                    m_WLevel.text = "Lv." + weaponGameData.Level;
                }

                m_WeaponNode.Show();
            }
        }

        private void UpdateArmorNode()
        {
            if (EquipManager.Ins.CurArmorOnID == 0)
            {
                m_ArmorEmpty.Show();
                m_ArmorNode.Hide();
            }
            else
            {
                m_ArmorEmpty.Hide();
                var curArmorOnID = EquipManager.Ins.CurArmorOnID;
                var itemData = ItemCfg.GetData(curArmorOnID);
                var armorData = EquipCfg.GetData(curArmorOnID);
                var armorGameData = EquipManager.Ins.GetEquipData(curArmorOnID, ItemType.Armor);
                UICommonHelper.LoadIcon(m_AIcon, itemData.Res);
                UICommonHelper.LoadQuality(m_AQuality, armorData.Quality);
                if (EquipManager.Ins.IsMaxLevel(curArmorOnID, ItemType.Armor))
                {
                    //服务器处理，客户端可以不处理
                    m_ALevel.text = "LV" + GameDefine.CommonItemMaxLevel;
                }
                else
                {
                    m_ALevel.text = "Lv." + armorGameData.Level;
                }

                m_ArmorNode.Show();
            }
        }

        private void UpdateEngineNode()
        {
            if (EngineManager.Ins.curEngineOnId == 0)
            {
                m_EngineEmpty.Show();
                m_EngineNode.Hide();
            }
            else
            {
                m_EngineEmpty.Hide();
                var curEngineOnID = EngineManager.Ins.curEngineOnId;
                var engineGameData = EngineManager.Ins.GetGameEngineData(curEngineOnID);
                var engineData = EngineCfg.GetData(engineGameData.TypeId);
                var resPath = ResCfg.GetData(engineData.ResID).Res;
                UICommonHelper.LoadIcon(m_EIcon, resPath);
                UICommonHelper.LoadQuality(m_EQuality, engineData.Quality);
                m_ELevel.text = "Lv." + engineGameData.Level;
                m_EngineNode.Show();
            }
        }

        public async void OnClickWeapon()
        {
            await UIManager.Ins.OpenUI<UIWeapon>();
        }

        public async void OnClickArmor()
        {
            await UIManager.Ins.OpenUI<UIArmor>();
        }

        public async void OnClickEngine()
        {
            await UIManager.Ins.OpenUI<UIEngine>();
        }

        public async void OnBtnRoleChangeClick()
        {
            await UIManager.Ins.OpenUI<UIRole.UIRole>();
        }
    }
}