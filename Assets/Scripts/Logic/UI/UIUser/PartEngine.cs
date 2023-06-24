using System.Collections.Generic;
using System.Linq;
using Framework.EventKit;
using Framework.Extension;
using Framework.UI;
using Logic.Common;
using Logic.Data;
using Logic.Manager;
using Logic.UI.Cells;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.UIUser
{
    public class PartEngine : MonoBehaviour
    {
        //战力相关
        public TextMeshProUGUI fightPower;

        //强化相关
        public ButtonEx btnUpgrade;
        public TextMeshProUGUI upgradeCost;
        public GameObject upgradeCan, upgradeCant;
        public GameObject btnMax;

        //等级相关
        public Image canProcess, cantProcess;
        public TextMeshProUGUI textProcess;
        public TextMeshProUGUI textLevel;

        //属性相关
        public TextMeshProUGUI atkDes;
        public TextMeshProUGUI atkNextDes;
        public TextMeshProUGUI hpDes;
        public TextMeshProUGUI hpNextDes;

        //火花塞气缸相关
        public Transform engineNode;
        private List<CommonEngine> commonEngines;

        //其他
        private EventGroup eventGroup = new EventGroup();

        private void Awake()
        {
            //注册
            btnUpgrade.onClick.AddListener(OnBtnUpgradeClick);
            btnUpgrade.onLongClick.AddListener(OnBtnUpgradeClick);
            eventGroup.Register(LogicEvent.TecPointChanged, (i, o) => OnTecPointChanged());
            eventGroup.Register(LogicEvent.EngineUpgrade, (i, o) =>
            {
                OnLevelExpChanged();
                OnAttrsChanged();
                OnFightPowerChanged();
            });
        }

        private void OnDestroy()
        {
            //注销
            eventGroup.Release();
        }

        private void OnEnable()
        {
            OnTecPointChanged();
            OnLevelExpChanged();
            OnAttrsChanged();
            OnFightPowerChanged();
        }

        private void Start()
        {
            Init();
        }

        /// <summary>
        /// 初始化火花塞气缸
        /// </summary>
        private void Init()
        {
            commonEngines = engineNode.GetComponentsInChildren<CommonEngine>().Take(6).ToList();
            var index = 1;
            foreach (var commonEngine in commonEngines)
            {
                commonEngine.Init(index);
                if (index <= 3)
                {
                    //火花塞
                    commonEngine.clickEngine += OnSparkClick;
                }
                else
                {
                    //气缸
                    commonEngine.clickEngine += OnCylinderClick;
                }

                index++;
            }
        }

        #region 事件

        /// <summary>
        /// 更新引擎强化消耗
        /// </summary>
        private void OnTecPointChanged()
        {
            upgradeCost.text = $"{GameDataManager.Ins.TecPoint}/{EngineManager.Ins.engineData.Costtech}";
            RefreshBtnUpgrade();
        }

        /// <summary>
        /// 更新引擎等级经验
        /// </summary>
        private void OnLevelExpChanged()
        {
            var level = EngineManager.Ins.gameEngineData.Level;
            var exp = EngineManager.Ins.gameEngineData.Exp;
            var totalExp = EngineManager.Ins.engineData.CostGear;
            textLevel.text = $"Lv{level}";
            textProcess.text = $"{exp}/{totalExp}";
            canProcess.fillAmount = (float)exp / totalExp;

            //引擎到达最大等级
            if (!EngineManager.Ins.IsEngineMaxLevel()) return;
            textLevel.text = "Max";
            RefreshMaxLevelState();
        }

        /// <summary>
        /// 更新引擎基础属性
        /// </summary>
        private void OnAttrsChanged()
        {
            var engineData = EngineManager.Ins.engineData;
            var gameEngineData = EngineManager.Ins.gameEngineData;
            var nextEngineData = EngineManager.Ins.GetEngineData(gameEngineData.Level + 1);
            atkDes.text = $"攻击力\n{engineData.BaseAdditionATK}%";
            hpDes.text = $"体力\n{engineData.BaseAdditionHP}%";
            if (nextEngineData == null)
            {
                atkNextDes.text = $"+0%";
                hpNextDes.text = $"+0%";
            }
            else
            {
                atkNextDes.text = $"+{nextEngineData.BaseAdditionATK - engineData.BaseAdditionATK}%";
                hpNextDes.text = $"+{nextEngineData.BaseAdditionHP - engineData.BaseAdditionHP}%";
            }
        }

        /// <summary>
        /// 更新战力
        /// </summary>
        private void OnFightPowerChanged()
        {
            fightPower.text = Formula.GetGJJFightPower().ToUIString();
        }

        /// <summary>
        /// 弹出火花塞面板
        /// </summary>
        /// <param name="pID"></param>
        private async void OnSparkClick(int pID)
        {
            await UIManager.Ins.OpenUI<UISpark>(pID);
        }

        /// <summary>
        /// 弹出气缸面板
        /// </summary>
        /// <param name="pID"></param>
        private async void OnCylinderClick(int pID)
        {
            await UIManager.Ins.OpenUI<UICylinder>(pID);
        }

        #endregion

        #region 刷新

        /// <summary>
        /// 刷新引擎强化按钮状态
        /// </summary>
        private void RefreshBtnUpgrade()
        {
            if (EngineManager.Ins.IsCanEngineUpgrade())
            {
                upgradeCan.Show();
                upgradeCant.Hide();
            }
            else
            {
                upgradeCan.Hide();
                upgradeCant.Show();
            }
        }

        /// <summary>
        /// 刷新引擎进度条状态
        /// </summary>
        private void RefreshMaxLevelState()
        {
            canProcess.Hide();
            cantProcess.Show();
            btnUpgrade.gameObject.Hide();
            btnMax.Show();
        }

        #endregion

        #region 按钮

        /// <summary>
        /// 点击引擎强化按钮
        /// </summary>
        private void OnBtnUpgradeClick()
        {
            if (EngineManager.Ins.IsCanEngineUpgrade())
            {
                EngineManager.Ins.DoEngineUpgrade();
            }
            else
            {
                EventManager.Call(LogicEvent.ShowTips, "未满足强化所需条件");
            }
        }

        /// <summary>
        /// 点击Max按钮
        /// </summary>
        public void OnBtnMaxClick()
        {
            EventManager.Call(LogicEvent.ShowTips, "已经达到最大强化等级");
        }

        /// <summary>
        /// 点击引擎属性面板
        /// </summary>
        public async void OnBtnAttrsInfoClick()
        {
            await UIManager.Ins.OpenUI<UIEngineAttrs>();
        }

        #endregion
    }
}