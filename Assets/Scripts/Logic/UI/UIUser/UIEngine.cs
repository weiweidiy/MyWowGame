using System.Collections.Generic;
using System.Linq;
using Framework.EventKit;
using Framework.Extension;
using Framework.UI;
using Logic.Common;
using Logic.Data;
using Logic.Manager;
using Logic.UI.Cells;
using Logic.UI.Common;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.UIUser
{
    public class UIEngine : UIPage
    {
        [Header("引擎生成")] public GameObject m_CommonEngineItem;
        public Transform m_EngineRoot;
        [Header("引擎选择")] private CommonEngineItem m_CurSelectItem;
        public GameObject m_EngineNode;
        public ScrollViewNavigation m_ScrollViewNavigation;
        private RectTransform m_Navigate;
        public GameObject m_SelectEmptyText;
        public GameObject m_BtnEngineGroup;
        [Header("引擎拥有加成")] public TextMeshProUGUI m_ATKEffect;
        public TextMeshProUGUI m_HPEffect;
        public GameObject m_SpecialAttrItem;
        public TextMeshProUGUI m_SpecialAttrName;
        public TextMeshProUGUI m_SpecialAttrEffect;
        [Header("引擎强化消耗材料")] public TextMeshProUGUI m_EngineIronCost;
        [Header("引擎分解获取材料")] public TextMeshProUGUI m_EngineIronGet;
        [Header("引擎改造次数")] public TextMeshProUGUI m_AttrsReformCount;
        [Header("引擎数量")] public TextMeshProUGUI m_EngineCount;
        [Header("引擎强化分解材料数量")] public TextMeshProUGUI m_EngineIronCount;
        [Header("引擎名称")] public TextMeshProUGUI m_EngineName;
        [Header("引擎图片")] public Image m_Icon;
        [Header("引擎品质")] public Image m_Quality;
        public TextMeshProUGUI m_QualityText;
        [Header("按钮")] public GameObject m_EngineEquipOn;
        public GameObject m_EngineEquipOff;
        public GameObject m_BtnEquipCanRemove, m_BtnEquipCantRemove;
        public GameObject m_BtnEquipCanIntensify, m_BtnEquipCantIntensify;
        [Header("引擎批量分解")] public GameObject m_BtnCanSelectRemove;
        public GameObject m_BtnCantSelectRemove;
        public GameObject m_EngineSelectRemoveNode;
        public Transform m_EngineRemoveRoot;
        private List<CommonEngineItem> m_CurRemoveSelectItemList = new List<CommonEngineItem>();
        private CommonEngineItem m_CurRemoveSelectItem;
        private int m_RemoveSelectCount;
        private int m_RemoveSelectedGet;
        public TextMeshProUGUI m_EngineSelectCount;
        public TextMeshProUGUI m_EngineSelectRemoveGet;


        private void Awake()
        {
            m_EventGroup.Register(LogicEvent.EngineGet, OnEngineGet);
            m_EventGroup.Register(LogicEvent.EngineRemove, (i, o) => { OnEngineRemove(); });
            m_EventGroup.Register(LogicEvent.EngineIronChanged, (i, o) => { OnEngineIronUpdate(); });
            m_EventGroup.Register(LogicEvent.EngineIntensify, (i, o) => OnEngineIntensify());
        }

        public override void OnShow()
        {
            base.OnShow();
            if (m_Navigate == null) return;
            m_ScrollViewNavigation.Navigate(m_Navigate);
        }

        private void Start()
        {
            InitEngine();
            UpdateEngineIron();
            UpdateEngineCount();
        }

        //初始化引擎列表
        private void InitEngine()
        {
            foreach (var gameEngineData in EngineManager.Ins.EngineMap)
            {
                if (gameEngineData.Value.IsGet == 1)
                {
                    var engine = m_CommonEngineItem.Clone(Vector3.zero, m_EngineRoot, Quaternion.identity);
                    engine.Show();
                    var engineItem = engine.GetComponent<CommonEngineItem>();
                    engineItem.Init(gameEngineData.Value);
                    engineItem.m_ClickEngine += OnClickEngineItem;
                    if (engineItem.m_GameEngineData.Id == EngineManager.Ins.curEngineOnId)
                    {
                        engineItem.OnClickEngine();
                        m_Navigate = engineItem.GetComponent<RectTransform>();
                        m_ScrollViewNavigation.Navigate(m_Navigate);
                    }
                }
            }
        }

        //更新引擎列表
        private void UpdateEngine()
        {
            var gameEngineData = m_CurSelectItem.m_GameEngineData;
            var engineData = m_CurSelectItem.m_EngineData;
            var attributeData = m_CurSelectItem.m_AttributeData;
            var resData = m_CurSelectItem.m_ResData;
            var engineLevel = gameEngineData.Level;
            var engineReform = gameEngineData.Reform;
            m_ATKEffect.text = $"{engineData.HasAdditionATK + engineLevel * engineData.AttGrow}%";
            m_HPEffect.text = $"{engineData.HasAdditionHP + engineLevel * engineData.HPGrow}%";
            m_AttrsReformCount.text = $"改造：{gameEngineData.Reform}/{engineData.ReformTime}次";
            m_EngineName.text = engineData.Name;
            UICommonHelper.LoadIcon(m_Icon, resData.Res);
            UICommonHelper.LoadQuality(m_Quality, engineData.Quality);
            m_QualityText.text = UICommonHelper.GetQualityShowText(engineData.Quality);
            m_SpecialAttrName.text = attributeData.Name;
            m_SpecialAttrEffect.text = $"{attributeData.Value}%";
            m_EngineIronCost.text = $"{Formula.GetEngineIntensifyCost(engineLevel, engineReform)}";
            m_EngineIronGet.text = $"+{Formula.GetEngineDecomposeGet(engineLevel, engineReform)}";
        }

        //分解引擎
        private void RemoveEngine(int engineId)
        {
            EngineManager.Ins.DoEngineRemove(engineId);
            EngineManager.Ins.OnEngineRemove(engineId);
        }

        //更新引擎数量
        private void UpdateEngineCount()
        {
            m_EngineCount.text = $"{EngineManager.Ins.curEngineCount}/{GameDefine.MaxEngineCount}";
            RefreshBtnSelectRemove();
        }

        //更新引擎分解强化材料数量
        private void UpdateEngineIron()
        {
            m_EngineIronCount.text = GameDataManager.Ins.Iron.ToString();
            if (m_CurSelectItem == null) return;
            RefreshBtnIntensify(Formula.GetEngineIntensifyCost(m_CurSelectItem.m_GameEngineData.Level,
                m_CurSelectItem.m_GameEngineData.Reform));
        }

        /// <summary>
        /// 生成批量分解页面引擎
        /// </summary>
        private void InitEngineSelectRemoveNode()
        {
            m_RemoveSelectCount = 0;
            m_RemoveSelectedGet = 0;
            UpdateEngineSelectRemoveText();
            foreach (var gameEngineData in EngineManager.Ins.EngineMap)
            {
                if (gameEngineData.Value.IsGet == 1)
                {
                    var engine = m_CommonEngineItem.Clone(Vector3.zero, m_EngineRemoveRoot, Quaternion.identity);
                    engine.Show();
                    var engineItem = engine.GetComponent<CommonEngineItem>();
                    engineItem.Init(gameEngineData.Value);
                    engineItem.m_ClickEngine += OnClickRemoveEngineItem;
                    m_CurRemoveSelectItemList.Add(engineItem);
                }
            }
        }

        /// <summary>
        /// 删除批量分解页面引擎
        /// </summary>
        private void DestroyEngineSelectRemoveNode()
        {
            foreach (var commonEngineItem in m_CurRemoveSelectItemList)
            {
                commonEngineItem.gameObject.Destroy();
            }

            m_CurRemoveSelectItemList.Clear();
        }

        /// <summary>
        /// 更新批量分解页面信息
        /// </summary>
        private void UpdateEngineSelectRemoveText()
        {
            m_EngineSelectCount.text = $"{m_RemoveSelectCount} 已选择";
            m_EngineSelectRemoveGet.text = m_RemoveSelectedGet.ToString();
        }

        #region 事件处理

        private void OnEngineGet(int eventId, object data)
        {
            var gameEngineData = EngineManager.Ins.EngineMap[(int)data];
            var engine = m_CommonEngineItem.Clone(Vector3.zero, m_EngineRoot, quaternion.identity);
            engine.Show();
            var engineItem = engine.GetComponent<CommonEngineItem>();
            engineItem.Init(gameEngineData);
            engineItem.m_ClickEngine += OnClickEngineItem;
            UpdateEngineCount();
        }

        private void OnEngineRemove()
        {
            UpdateEngineCount();
        }

        private void OnEngineIronUpdate()
        {
            UpdateEngineIron();
        }

        private void OnEngineIntensify()
        {
            var gameEngineData = m_CurSelectItem.m_GameEngineData;
            var engineData = m_CurSelectItem.m_EngineData;
            var engineId = gameEngineData.Id;
            var engineLevel = gameEngineData.Level;
            var engineReform = gameEngineData.Reform;
            var engineIronCost = Formula.GetEngineIntensifyCost(engineLevel, engineReform);
            var engineIronGet = Formula.GetEngineDecomposeGet(engineLevel, engineReform);
            m_EngineIronCost.text = $"{engineIronCost}";
            m_EngineIronGet.text = $"+{engineIronGet}";
            m_ATKEffect.text = $"{engineData.HasAdditionATK + engineLevel * engineData.AttGrow}%";
            m_HPEffect.text = $"{engineData.HasAdditionHP + engineLevel * engineData.HPGrow}%";
            RefreshBtnIntensify(engineIronCost);
        }

        private void OnClickEngineItem(CommonEngineItem pItem)
        {
            if (m_CurSelectItem != null)
            {
                //选择其他CommonEngineItem时隐藏上一次CommonEngineItem的选择
                m_CurSelectItem.HideSelected();
            }

            m_CurSelectItem = pItem;
            m_Navigate = m_CurSelectItem.GetComponent<RectTransform>();
            m_CurSelectItem.ShowSelected();
            m_BtnEngineGroup.Show();
            UpdateEngine();

            m_EngineNode.Show();
            m_SpecialAttrItem.Show();

            if (IsSelectItemOn())
            {
                RefreshBtnEquipOn();
            }
            else
            {
                RefreshBtnEquipOff();
            }


            RefreshBtnIntensify(Formula.GetEngineIntensifyCost(m_CurSelectItem.m_GameEngineData.Level,
                m_CurSelectItem.m_GameEngineData.Reform));
        }


        //批量选择引擎分解
        private void OnClickRemoveEngineItem(CommonEngineItem pItem)
        {
            m_CurRemoveSelectItem = pItem;
            m_CurRemoveSelectItem.UpdateRemoveSelected();

            var gameEngineData = m_CurRemoveSelectItem.m_GameEngineData;
            var engineLevel = gameEngineData.Level;
            var engineReform = gameEngineData.Reform;
            if (m_CurRemoveSelectItem.m_IsRemoveSelected)
            {
                m_RemoveSelectCount++;
                m_RemoveSelectedGet += Formula.GetEngineDecomposeGet(engineLevel, engineReform);
            }
            else
            {
                m_RemoveSelectCount--;
                m_RemoveSelectedGet -= Formula.GetEngineDecomposeGet(engineLevel, engineReform);
            }

            UpdateEngineSelectRemoveText();
        }

        #endregion

        #region 通用逻辑

        private bool IsSelectItemOn()
        {
            return EngineManager.Ins.IsOn(m_CurSelectItem.m_GameEngineData.Id);
        }

        // 清空所有选择
        private void ClearSelectItem()
        {
            if (m_CurSelectItem != null)
            {
                m_CurSelectItem.HideSelected();
            }

            m_CurSelectItem = null;
            m_Navigate = null;
            m_SelectEmptyText.Show();
            m_BtnEngineGroup.Hide();
            m_EngineNode.Hide();
            m_SpecialAttrItem.Hide();
            m_ATKEffect.text = string.Empty;
            m_HPEffect.text = string.Empty;
            m_AttrsReformCount.text = string.Empty;
            m_EngineName.text = string.Empty;
            m_QualityText.text = string.Empty;
        }

        private void RefreshBtnEquipOn()
        {
            m_EngineEquipOn.Hide();
            m_EngineEquipOff.Show();
            m_BtnEquipCanRemove.Hide();
            m_BtnEquipCantRemove.Show();
        }

        private void RefreshBtnEquipOff()
        {
            m_EngineEquipOn.Show();
            m_EngineEquipOff.Hide();
            m_BtnEquipCanRemove.Show();
            m_BtnEquipCantRemove.Hide();
        }

        private void RefreshBtnIntensify(int cost)
        {
            if (EngineManager.Ins.IsCanIntensify(cost))
            {
                m_BtnEquipCanIntensify.Show();
                m_BtnEquipCantIntensify.Hide();
            }
            else
            {
                m_BtnEquipCanIntensify.Hide();
                m_BtnEquipCantIntensify.Show();
            }
        }

        private void RefreshBtnSelectRemove()
        {
            if (EngineManager.Ins.curEngineCount >= 1)
            {
                m_BtnCanSelectRemove.Show();
                m_BtnCantSelectRemove.Hide();
            }
            else
            {
                m_BtnCanSelectRemove.Hide();
                m_BtnCantSelectRemove.Show();
            }
        }

        #endregion

        #region 按钮处理

        public void OnBtnClickEquipOn()
        {
            RefreshBtnEquipOn();
            var engineId = m_CurSelectItem.m_GameEngineData.Id;
            EngineManager.Ins.DoEngineOn(engineId);
        }

        public void OnBtnClickEquipOff()
        {
            RefreshBtnEquipOff();
            EngineManager.Ins.DoEngineOff(m_CurSelectItem.m_GameEngineData.Id);
            if (!IsSelectItemOn())
            {
                ClearSelectItem();
            }
        }

        public void OnBtnClickEquipRemove()
        {
            if (m_CurSelectItem == null) return;
            var engineId = m_CurSelectItem.m_GameEngineData.Id;
            if (!IsSelectItemOn())
            {
                RemoveEngine(engineId);
                ClearSelectItem();
            }
            else
            {
                // 如何需要提示时打开按钮m_BtnEquipCantRemove的Image的RaycastTarget
                EventManager.Call(LogicEvent.ShowTips, "无法分解已经装备的引擎");
            }
        }

        public void OnBtnClickEquipIntensify()
        {
            if (m_CurSelectItem == null) return;
            var engineData = m_CurSelectItem.m_GameEngineData;
            var engineId = engineData.Id;
            var engineLevel = engineData.Level;
            var engineReform = engineData.Reform;
            var engineIronCost = Formula.GetEngineIntensifyCost(engineLevel, engineReform);
            if (EngineManager.Ins.IsCanIntensify(engineIronCost))
            {
                EngineManager.Ins.DoEngineIntensify(engineId);
            }
            else
            {
                // 如何需要提示时打开按钮m_BtnEquipCantIntensify的Image的RaycastTarget
                EventManager.Call(LogicEvent.ShowTips, "引擎强化材料不足");
            }
        }

        public void OnBtnClickSelectRemove()
        {
            if (EngineManager.Ins.curEngineCount >= 1)
            {
                InitEngineSelectRemoveNode();
                m_EngineSelectRemoveNode.Show();
            }
            else
            {
                // 如何需要提示时打开按钮m_BtnCantSelectRemove的Image的RaycastTarget
                EventManager.Call(LogicEvent.ShowTips, "当前没有引擎无法进行批量分解");
            }
        }

        public void OnBtnClickCloseRemoveNode()
        {
            DestroyEngineSelectRemoveNode();
            m_EngineSelectRemoveNode.Hide();
        }

        public void OnBtnClickBatchRemove()
        {
            if (m_RemoveSelectCount == 0)
            {
                EventManager.Call(LogicEvent.ShowTips, "当前没有批量选择引擎");
                return;
            }

            /*
             * 使用foreach循环迭代m_CurRemoveSelectItemList集合时，同时又在循环体内修改了这个集合，导致了InvalidOperationException异常
             * 使用了LINQ的ToList()方法来创建了m_CurRemoveSelectItemList集合的副本，并在副本上进行了迭代。由于在循环体内修改的是原始集合的副本，所以不会抛出InvalidOperationException异常
             */
            foreach (var commonEngineItem in m_CurRemoveSelectItemList.Where(commonEngineItem =>
                         commonEngineItem.m_IsRemoveSelected).ToList())
            {
                if (commonEngineItem.m_IsOn.activeSelf)
                {
                    EngineManager.Ins.DoEngineOff(commonEngineItem.m_GameEngineData.Id);
                }

                RemoveEngine(commonEngineItem.m_GameEngineData.Id);
                m_CurRemoveSelectItemList.Remove(commonEngineItem);
                m_RemoveSelectCount--;
            }

            m_RemoveSelectedGet = 0;
            UpdateEngineSelectRemoveText();
            ClearSelectItem();
        }

        #endregion
    }
}