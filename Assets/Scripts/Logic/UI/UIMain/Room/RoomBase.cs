using BreakInfinity;
using Framework.EventKit;
using Framework.Extension;
using Logic.Common;
using Logic.Data;
using Logic.Manager;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;


namespace Logic.UI.UIMain.Room
{
    /// <summary>
    /// GJJ 属性升级房间 基类
    /// </summary>
    public class RoomBase : MonoBehaviour
    {
        private EventGroup m_EventGroup = new();

        [LabelText("房间Id")] public int m_RoomId;

        [LabelText("房间类型")] public AttributeType m_RoomType; //房间类型

        [LabelText("房间等级")] public TextMeshProUGUI m_RoomLevel; //房间等级

        [LabelText("房间效果")] public TextMeshProUGUI m_RoomEffect; //房间效果

        [LabelText("房间升级消耗")] public TextMeshProUGUI m_RoomCost; //房间升级消耗

        [LabelText("消耗节点")] public GameObject m_CostNode;
        [LabelText("满级节点")] public GameObject m_MaxNode;


        [LabelText("伙伴进入点")] public GameObject[] m_RoomPartnerSpawnPoint;
        [LabelText("房间边缘点")] public GameObject m_RoomEdgePoint; //房间边缘点

        public ButtonEx m_Btn;
        public TextMeshProUGUI m_BtnText;
        public Sprite m_NormalImage;
        public Sprite m_CantImage;
        public Sprite m_MaxImage;

        private void Awake()
        {
            m_EventGroup.Register(LogicEvent.CoinChanged, OnCoinChanged);
            m_Btn.onClick.AddListener(OnClickUpgrade);
            m_Btn.onLongClick.AddListener(OnClickUpgrade);

            if (m_RoomType == AttributeType.ATK)
            {
                m_EventGroup.Register(LogicEvent.SkillAllEffectUpdate, (i, o) => ShowAddEffect());
                m_EventGroup.Register(LogicEvent.PartnerAllEffectUpdate, (i, o) => ShowAddEffect());
                m_EventGroup.Register(LogicEvent.EquipAllATKEffectUpdate, (i, o) => ShowAddEffect());
                m_EventGroup.Register(LogicEvent.EquipAllHPEffectUpdate, (i, o) => ShowAddEffect());
            }

            if (m_RoomType == AttributeType.HP)
            {
                m_EventGroup.Register(LogicEvent.EquipAllHPEffectUpdate, (i, o) => ShowAddEffect());
            }

            m_EventGroup.Register(LogicEvent.ResearchCompleteEffectUpdate, (i, o) => ShowAddEffect());
            m_EventGroup.Register(LogicEvent.QuenchingEffectUpdate, (i, o) => ShowAddEffect());
            m_EventGroup.Register(LogicEvent.OnSpoilDraw, (i, o) => { ShowAddEffect(); });
            m_EventGroup.Register(LogicEvent.OnSpoilUpgrade, (i, o) => { ShowAddEffect(); });
            m_EventGroup.Register(LogicEvent.RoleBreakTreeEffectUpdate, (i, o) => { ShowAddEffect(); });
        }

        private void OnDestroy()
        {
            m_EventGroup.Release();
        }

        private void Start()
        {
            Refresh();
        }

        protected long GetCurLevel()
        {
            switch (m_RoomType)
            {
                case AttributeType.ATK:
                    return GameDataManager.Ins.GJJAtkLevel;
                case AttributeType.HP:
                    return GameDataManager.Ins.GJJHPLevel;
                case AttributeType.HPRecover:
                    return GameDataManager.Ins.GJJHPRecoverLevel;
                case AttributeType.Critical:
                    return GameDataManager.Ins.GJJCriticalLevel;
                case AttributeType.CriticalDamage:
                    return GameDataManager.Ins.GJJCriticalDamageLevel;
                case AttributeType.Speed:
                    return GameDataManager.Ins.GJJAtkSpeedLevel;
                case AttributeType.DoubleHit:
                    return GameDataManager.Ins.GJJDoubleHitLevel;
                case AttributeType.TripletHit:
                    return GameDataManager.Ins.GJJTripletHitLevel;
            }

            return -1;
        }

        protected BigDouble GetRoomUpgradeCost()
        {
            return Formula.GetGJJRoomUpgradeCost(m_RoomType, GetCurLevel());
        }

        protected bool IsMaxLevel()
        {
            switch (m_RoomType)
            {
                case AttributeType.ATK:
                    return false;
                case AttributeType.HP:
                    return false;
                case AttributeType.HPRecover:
                    return false;
                case AttributeType.Critical:
                    return false;
                case AttributeType.CriticalDamage:
                    return false;
                case AttributeType.Speed:
                    return false;
                case AttributeType.DoubleHit:
                    return false;
                case AttributeType.TripletHit:
                    return false;
            }

            return false;
        }

        protected bool IsCanUpgrade()
        {
            return GameDataManager.Ins.Coin >= GetRoomUpgradeCost();
        }

        protected void ShowAddEffect()
        {
            switch (m_RoomType)
            {
                case AttributeType.ATK:
                    m_RoomEffect.text = Formula.GetGJJAtk().ToUIString();
                    break;
                case AttributeType.HP:
                    m_RoomEffect.text = Formula.GetGJJHP().ToUIString();
                    break;
                case AttributeType.HPRecover:
                    m_RoomEffect.text = Formula.GetGJJHPRecover().ToUIString();
                    break;
                case AttributeType.Critical:
                    m_RoomEffect.text = (Formula.GetGJJCritical() * 100).ToString("F2") + "%";
                    break;
                case AttributeType.CriticalDamage:
                    m_RoomEffect.text = (Formula.GetGJJCriticalDamage() * 100).ToUIStringFloat() + "%";
                    break;
                case AttributeType.Speed:
                    m_RoomEffect.text = Formula.GetGJJAtkSpeed().ToString("F2") + "";
                    break;
                case AttributeType.DoubleHit:
                    m_RoomEffect.text = Formula.GetGJJDoubleHit() + "%";
                    break;
                case AttributeType.TripletHit:
                    m_RoomEffect.text = Formula.GetGJJTripletHit() + "%";
                    break;
            }
        }

        protected virtual void Refresh()
        {
            ShowAddEffect();
            if (IsMaxLevel())
            {
                m_CostNode.Hide();
                m_MaxNode.Show();
                m_Btn.interactable = false;
                m_Btn.image.sprite = m_MaxImage;
                m_RoomLevel.text = "MAX";
                return;
            }

            m_RoomCost.text = GetRoomUpgradeCost().ToUIString();
            m_RoomLevel.text = "Lv: " + GetCurLevel();
            m_CostNode.Show();
            m_MaxNode.Hide();

            if (IsCanUpgrade())
            {
                m_Btn.interactable = true;
                m_Btn.image.sprite = m_NormalImage;
                m_BtnText.color = Color.white;
                m_RoomCost.color = Color.white;
            }
            else
            {
                m_Btn.interactable = true;
                m_Btn.image.sprite = m_CantImage;
                m_BtnText.color = Color.red;
                m_RoomCost.color = Color.red;
            }
        }

        protected virtual void OnCoinChanged(int arg1, object arg2)
        {
            if (IsMaxLevel())
                return;

            if (IsCanUpgrade())
            {
                m_Btn.interactable = true;
                m_Btn.image.sprite = m_NormalImage;
                m_BtnText.color = Color.white;
                m_RoomCost.color = Color.white;
            }
            else
            {
                m_Btn.interactable = true;
                m_Btn.image.sprite = m_CantImage;
                m_BtnText.color = Color.red;
                m_RoomCost.color = Color.red;
            }
        }

        protected virtual void OnClickUpgrade()
        {
            if (IsMaxLevel())
                return;
            if (!IsCanUpgrade())
            {
                // EventManager.Call(LogicEvent.ShowTips, "金币不足");
                return;
            }

            //扣钱
            var _Cost = GetRoomUpgradeCost();
            GameDataManager.Ins.Coin -= _Cost;

            //升级
            DoUpgrade();
        }

        protected virtual void DoUpgrade()
        {
            switch (m_RoomType)
            {
                case AttributeType.ATK:
                    GameDataManager.Ins.GJJAtkLevel++;
                    TaskManager.Ins.DoTaskUpdate(TaskType.TT_3001, 1);
                    break;
                case AttributeType.HP:
                    GameDataManager.Ins.GJJHPLevel++;
                    TaskManager.Ins.DoTaskUpdate(TaskType.TT_3002, 1);
                    break;
                case AttributeType.HPRecover:
                    GameDataManager.Ins.GJJHPRecoverLevel++;
                    TaskManager.Ins.DoTaskUpdate(TaskType.TT_3003, 1);
                    break;
                case AttributeType.Critical:
                    GameDataManager.Ins.GJJCriticalLevel++;
                    TaskManager.Ins.DoTaskUpdate(TaskType.TT_3004, 1);
                    break;
                case AttributeType.CriticalDamage:
                    GameDataManager.Ins.GJJCriticalDamageLevel++;
                    TaskManager.Ins.DoTaskUpdate(TaskType.TT_3005, 1);
                    break;
                case AttributeType.Speed:
                    GameDataManager.Ins.GJJAtkSpeedLevel++;
                    TaskManager.Ins.DoTaskUpdate(TaskType.TT_3006, 1);
                    break;
                case AttributeType.DoubleHit:
                    GameDataManager.Ins.GJJDoubleHitLevel++;
                    TaskManager.Ins.DoTaskUpdate(TaskType.TT_3007, 1);
                    break;
                case AttributeType.TripletHit:
                    GameDataManager.Ins.GJJTripletHitLevel++;
                    TaskManager.Ins.DoTaskUpdate(TaskType.TT_3008, 1);
                    break;
            }

            Refresh();

            EventManager.Call(LogicEvent.RoomUpgraded, m_RoomType);
            EventManager.Call(LogicEvent.CoinChanged);
        }
    }
}