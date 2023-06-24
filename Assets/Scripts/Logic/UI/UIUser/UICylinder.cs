using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BreakInfinity;
using Configs;
using Framework.EventKit;
using Framework.Extension;
using Framework.UI;
using Logic.Common;
using Logic.Data;
using Logic.Manager;
using Logic.UI.Cells;
using Logic.UI.Common;
using Networks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Logic.UI.UIUser
{
    public class UICylinder : UIPage
    {
        //上阵位ID
        private int soltID;

        //科技点
        public TextMeshProUGUI tecPointCount;

        //引擎
        public Transform engineRoot;
        public CommonEngineItem commonEngineItem;
        private CommonEngineItem curSelectItem;
        private List<CommonEngineItem> engineList = new List<CommonEngineItem>();

        //属性
        public Image quality;
        public Image icon;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI qualityText;
        public TextMeshProUGUI attr1Name;
        public TextMeshProUGUI attr1Value;
        public TextMeshProUGUI attr2Name;
        public TextMeshProUGUI attr2Value;

        //装配 解除 分解
        public TextMeshProUGUI tecPointGet;
        public GameObject btnEquipOn, btnEquipOff;
        public GameObject equipCanRemove, equipCantRemove;

        //批量分解
        public GameObject selectRemoveNode;
        public GameObject canSelectRemove, cantSelectRemove;
        public Transform engineRemoveRoot;
        private List<CommonEngineItem> curRemoveSelectItemList = new List<CommonEngineItem>();
        private CommonEngineItem curRemoveSelectItem;
        private int removeSelectCount;
        private int removeSelectedGet;
        public TextMeshProUGUI engineSelectCount;
        public TextMeshProUGUI engineSelectRemoveGet;

        //状态
        public GameObject empty;
        public GameObject btnGroup;

        private void Awake()
        {
            m_EventGroup.Register(LogicEvent.TecPointChanged, (i, o) => OnTecPointChanged());
            m_EventGroup.Register(LogicEvent.EnginePartsUpdate, OnEnginePartsUpdate);
            m_EventGroup.Register(LogicEvent.EngineOn, (i, o) => OnEngineOnOrOff());
            m_EventGroup.Register(LogicEvent.EngineOff, (i, o) => OnEngineOnOrOff());
        }

        public override void OnShow()
        {
            base.OnShow();
            soltID = (int)m_OpenData_;
            RefreshSelect();
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            InstantiateEngine(EngineManager.Ins.cylinderMap.Values.ToList());
            RefreshEngine();
            OnTecPointChanged();
        }

        //初始化
        private void InstantiateEngine(List<GameEnginePartData> pList)
        {
            foreach (var gameSparkData in pList)
            {
                var item = Instantiate(commonEngineItem, engineRoot);
                item.Init(gameSparkData);
                item.clickEngine += OnClickEngine;
                item.Show();
                engineList.Add(item);
            }
        }

        //更新引擎详细信息
        private void UpdateEngine()
        {
            var gameEngineData = curSelectItem.gameEngineData;
            var cylinderData = CylinderCfg.GetData(gameEngineData.CfgID);
            var pQuality = cylinderData.Quilty;
            var pResID = cylinderData.ResID;
            var pName = cylinderData.Name;
            var pTecPoint = cylinderData.Decompose;
            UICommonHelper.LoadQuality(quality, pQuality);
            // UICommonHelper.LoadIcon(icon, ResCfg.GetData(pResID).Res);
            nameText.text = pName;
            qualityText.text = UICommonHelper.GetQualityShowText(pQuality);
            var attributeData1 = AttributeCfg.GetData(gameEngineData.Attr1ID);
            var attributeData2 = AttributeCfg.GetData(gameEngineData.Attr2ID);
            attr1Name.text = attributeData1.Name;
            attr2Name.text = attributeData2.Name;
            attr1Value.text = attributeData1.Value.ToString(CultureInfo.InvariantCulture);
            attr2Value.text = attributeData2.Value.ToString(CultureInfo.InvariantCulture);
            tecPointGet.text = pTecPoint.ToString();
        }

        //刷新选中状态
        private void RefreshSelect()
        {
            foreach (var item in engineList)
            {
                if (EngineManager.Ins.IsIDOnSlot(item.gameEngineData.InsID, soltID))
                {
                    OnClickEngine(item);
                    return;
                }
            }

            if (engineList.Count <= 0) return;

            if (engineList[0] != null)
            {
                OnClickEngine(engineList[0]);
            }
        }

        // 清空选择
        private void ClearSelect()
        {
            if (curSelectItem != null)
            {
                curSelectItem.HideSelected();
            }

            curSelectItem = null;
            btnGroup.Hide();
            empty.Show();
            attr1Name.text = string.Empty;
            attr2Name.text = string.Empty;
            attr1Value.text = string.Empty;
            attr2Value.text = string.Empty;
            nameText.text = string.Empty;
            qualityText.text = string.Empty;
        }

        //刷新引擎状态
        private void RefreshEngine()
        {
            //是否选中
            if (curSelectItem == null)
            {
                empty.Show();
                btnGroup.Hide();
            }
            else
            {
                empty.Hide();
                btnGroup.Show();
                //是否上阵
                if (EngineManager.Ins.IsEngineOn(curSelectItem.gameEngineData.InsID))
                {
                    btnEquipOn.Hide();
                    btnEquipOff.Show();
                    equipCanRemove.Hide();
                    equipCantRemove.Show();
                }
                else
                {
                    btnEquipOn.Show();
                    btnEquipOff.Hide();
                    equipCanRemove.Show();
                    equipCantRemove.Hide();
                }
            }
        }

        /// <summary>
        /// 生成批量分解引擎
        /// </summary>
        private void InstantiateRemoveEngine()
        {
            removeSelectCount = 0;
            removeSelectedGet = 0;
            UpdateRemoveEngine();
            foreach (var gameEngineData in EngineManager.Ins.cylinderMap.Values.ToList())
            {
                var item = Instantiate(commonEngineItem, engineRemoveRoot);
                item.Init(gameEngineData);
                item.clickEngine += OnClickRemoveEngine;
                curRemoveSelectItemList.Add(item);
                item.Show();
            }
        }

        /// <summary>
        /// 销毁批量分解引擎
        /// </summary>
        private void DestroyRemoveEngine()
        {
            foreach (var removeItem in curRemoveSelectItemList)
            {
                removeItem.gameObject.Destroy();
            }

            curRemoveSelectItemList.Clear();
        }

        /// <summary>
        /// 更新批量分解面板
        /// </summary>
        private void UpdateRemoveEngine()
        {
            engineSelectCount.text = $"{removeSelectCount} 已选择";
            engineSelectRemoveGet.text = removeSelectedGet.ToString();
            if (removeSelectCount > 0)
            {
                canSelectRemove.Show();
                cantSelectRemove.Hide();
            }
            else
            {
                canSelectRemove.Hide();
                cantSelectRemove.Show();
            }
        }

        //批量选择分解引擎
        private void OnClickRemoveEngine(CommonEngineItem pItem)
        {
            if (pItem.IsOn)
            {
                EventManager.Call(LogicEvent.ShowTips, "已经装配不可选择");
                return;
            }

            curRemoveSelectItem = pItem;
            curRemoveSelectItem.RemoveSelected();

            var gameEngineData = curRemoveSelectItem.gameEngineData;
            var cylinderData = CylinderCfg.GetData(gameEngineData.CfgID);
            var pTecPoint = cylinderData.Decompose;
            if (curRemoveSelectItem.IsRemoveSelected)
            {
                removeSelectCount++;
                removeSelectedGet += pTecPoint;
            }
            else
            {
                removeSelectCount--;
                removeSelectedGet -= pTecPoint;
            }

            UpdateRemoveEngine();
        }

        #region 事件

        //更新引擎强化消耗科技点
        private void OnTecPointChanged()
        {
            BigDouble bigTecPoint = GameDataManager.Ins.TecPoint;
            tecPointCount.text = bigTecPoint.ToUIString();
        }

        //更新获取引擎
        private void OnEnginePartsUpdate(int eventId, object data)
        {
            var pList = (List<GameEnginePartData>)data;
            var cylinderList = pList.Where(gameEnginePartData => (ItemType)gameEnginePartData.Type == ItemType.Cylinder)
                .ToList();
            InstantiateEngine(cylinderList);
        }

        //更新引擎装配解除
        private void OnEngineOnOrOff()
        {
            if (curSelectItem == null) return;

            OnClickEngine(curSelectItem);
        }

        //选择引擎
        private void OnClickEngine(CommonEngineItem pItem)
        {
            if (curSelectItem != null)
            {
                curSelectItem.HideSelected();
            }

            curSelectItem = pItem;
            curSelectItem.ShowSelected();
            UpdateEngine();
            RefreshEngine();
        }

        #endregion

        #region 按钮

        //点击装配按钮
        public void OnBtnEquipOnClick()
        {
            if (curSelectItem == null) return;

            EngineManager.Ins.DoEngineOn(curSelectItem.gameEngineData.InsID, soltID);
        }

        //点击解除按钮
        public void OnBtnEquipOffClick()
        {
            if (curSelectItem == null) return;

            EngineManager.Ins.DoEngineOff(curSelectItem.gameEngineData.InsID);
        }

        //点击分解按钮
        public void OnBtnEquipRemoveClick()
        {
            if (curSelectItem == null) return;

            var resolveList = new List<int> { curSelectItem.gameEngineData.InsID };
            EngineManager.Ins.DoEngineResolve(resolveList);
            ClearSelect();
        }

        //点击批量分解按钮
        public void OnBtnSelectRemoveClick()
        {
            if (EngineManager.Ins.cylinderMap.Count >= 1)
            {
                InstantiateRemoveEngine();
                selectRemoveNode.Show();
            }
            else
            {
                EventManager.Call(LogicEvent.ShowTips, "当前未满足批量分解条件");
            }
        }

        //点击关闭批量分解节点按钮
        public void OnBtnCloseRemoveNodeClick()
        {
            DestroyRemoveEngine();
            selectRemoveNode.Hide();
        }

        //点击批量分解
        public void OnBtnClickBatchRemove()
        {
            if (removeSelectCount == 0)
            {
                EventManager.Call(LogicEvent.ShowTips, "当前未满足批量分解条件");
                return;
            }

            var removeList = new List<int>();
            foreach (var removeItem in curRemoveSelectItemList.Where(removeItem =>
                         removeItem.IsRemoveSelected).ToList())
            {
                curRemoveSelectItemList.Remove(removeItem);
                removeList.Add(removeItem.gameEngineData.InsID);
                removeSelectCount--;
            }

            removeSelectedGet = 0;
            UpdateRemoveEngine();
            EngineManager.Ins.DoEngineResolve(removeList);
            ClearSelect();
        }

        //点击关闭按钮
        public void OnBtnClose()
        {
            this.Hide();
        }

        #endregion
    }
}