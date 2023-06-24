using Configs;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Framework.EventKit;
using Framework.UI;
using Logic.Common;
using Logic.Config;
using Logic.Data;
using Logic.Fight;
using Logic.UI.UICopy;
using Logic.UI.UIFight;
using Logic.UI.UIMain;
using Networks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Logic.Manager
{
    public class ReformCopyManager : CopyLogicManager
    {
        /// <summary>
        /// 相机操作配置
        /// </summary>
        public struct CameraOperation
        {
            public Vector3 posTarget;
            public float size;
        }

        /// <summary>
        /// 选择道具的值对象
        /// </summary>
        public struct SelectionItemVO
        {
            public ItemType itemType;
            public int id;
            public string title;
            public string name;
            public string iconPath;
            public string desc;
            public GameEnginePartData partData;

        }

        /// <summary>
        /// 改造副本状态
        /// </summary>
        public enum ReformState
        {
            DriverSelection,
            CardsSelection,
        }

        /// <summary>
        /// 随机道具类型
        /// </summary>
        public enum ItemType
        {
            Attr,
            Cylinder,
        }


        private EventGroup m_EventGroup = new();

        /// <summary>
        /// 默认的相机配置
        /// </summary>
        CameraOperation defaultCameraOp;

        /// <summary>
        /// 选择的改造列表
        /// </summary>
        List<SelectionItemVO> selectionList = new List<SelectionItemVO>();

        List<GameEnginePartData> gameEnginePartData = new List<GameEnginePartData>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="copyManager"></param>
        public ReformCopyManager(CopyManager copyManager) : base(copyManager) { }

        /// <summary>
        /// 进入副本
        /// </summary>
        /// <returns></returns>
        public async override UniTask OnEnter(S2C_EnterCopy pMsg)
        {
            copyManager.m_ReformCopyCount = 1;
            //缓存默认的摄像机配置
            var cam = Camera.main;
            defaultCameraOp.posTarget = cam.transform.position;
            defaultCameraOp.size = cam.orthographicSize;

            //注册切换副本成功的消息
            m_EventGroup.Register(LogicEvent.Fight_SwitchComplete, (i, o) =>
            {
                OnSwitchComplete((LevelType)o);
            });

            //发送切换转场通知
            var _Para = new FightSwitchTo { m_SwitchToType = SwitchToType.ToReformCopy };
            EventManager.Call(LogicEvent.Fight_Switch, _Para);

            
            //await UIManager.Ins.OpenUI<UICopyFighting>();

        }

        /// <summary>
        /// 退出副本
        /// </summary>
        public async override void OnExit(S2C_ExitCopy pMsg)
        {
            copyManager.m_ReformCopyData.Level = pMsg.Level;
            copyManager.m_ReformCopyData.KeyCount = pMsg.KeyCount;

            //计算奖励，发给服务器保存
            //var rewards = GetRewards();
            //EventManager.Call(LogicEvent.ShowReformCopyRewards, rewards);

            //ExitCopyFight();

            m_EventGroup.Release();

            selectionList.Clear();

            gameEnginePartData.Clear();

            CopyManager.Ins.StopCopyTimer();

            await UpdateUI(false);
            MoveCamera(defaultCameraOp, true);
            var uiFight = UIManager.Ins.GetUI<UIFight>();
            uiFight.ShowSkillNode();
            uiFight.ShowNormalLevelNode();
        }

        /// <summary>
        /// 获取奖励
        /// </summary>
        /// <returns></returns>
        private object GetRewards()
        {
            var selectionList = GetSelectionList();
            foreach(var vo in selectionList)
            {
                if (vo.itemType.Equals(ItemType.Attr))
                    continue;


            }
            return null;
        }



        /// <summary>
        /// 切换动画播放完成
        /// </summary>
        /// <param name="o"></param>
        private async void OnSwitchComplete(LevelType o)
        {
            if (o != LevelType.ReformCopy)
                return;

            //进入idle状态，什么都不干
            FightManager.Ins.ToIdle();

            //隐藏掉技能栏
            var uiFight = UIManager.Ins.GetUI<UIFight>();
            uiFight.HideSkillNode();
            uiFight.HideNormalLevelNode();

            //设置摄像机到特写镜头
            var op = GetCameraOperation();
            MoveCamera(op, true);

            await UpdateUI(true);
        }

        /// <summary>
        /// 开始战斗
        /// </summary>
        public void StartFight()
        {
            //缓动的恢复摄像机视角
            MoveCamera(defaultCameraOp, false);

            //立即暂停进行卡片选择
            PauseFightToSelectCard();
        }

        /// <summary>
        /// 暂停战斗选择卡片
        /// </summary>
        public async void PauseFightToSelectCard()
        {
            FightManager.Ins.ToIdle();

            //隐藏技能栏
            var uiFight = UIManager.Ins.GetUI<UIFight>();
            uiFight.HideSkillNode();
            //uiFight.HideNormalLevelNode();

            //获取3个随机选择项目
            var items = GetRandomSelectionItems();

            //显示技能选择界面
            await UIManager.Ins.OpenUI<UIReformCopyFighting>(GetUIArgs(ReformState.CardsSelection,items));
        }

        /// <summary>
        /// 刷新选择道具列表
        /// </summary>
        public void RefreshItems()
        {
            //获取3个随机选择项目
            var items = GetRandomSelectionItems();

            EventManager.Call(LogicEvent.RefreshReformItems, items);
        }

        /// <summary>
        /// 恢复战斗进行
        /// </summary>
        public void ResumeFight(SelectionItemVO vo)
        {
            //添加到改造列表
            selectionList.Add(vo);

            if(vo.itemType == ItemType.Cylinder)
                gameEnginePartData.Add(vo.partData);

            //显示技能栏
            var uiFight = UIManager.Ins.GetUI<UIFight>();
            uiFight.ShowSkillNode(1005);
            //uiFight.ShowNormalLevelNode();

            FightManager.Ins.ToStandBy();
        }


        /// <summary>
        /// 主动推出战斗
        /// </summary>
        public void EscapFight()
        {
            //通知状态机切换状态
            var _Para = new FightSwitchTo { m_SwitchToType = SwitchToType.ToNormalLevel };
            EventManager.Call(LogicEvent.Fight_Switch, _Para);
            if (_Para.m_CanSwitchToNextNode == false)
                return;

            RequestExitCopy(false);
        }

        /// <summary>
        /// 更新副本ui
        /// </summary>
        /// <param name="onEnter"></param>
        /// <returns></returns>
        public async UniTask UpdateUI(bool onEnter)
        {
            if(onEnter)
            {
                //隐藏一些ui
                UIManager.Ins.Hide<UIMainLeft>();
                UIManager.Ins.Hide<UIMainRight>();
                UIManager.Ins.Hide<UICopy>();

                //打开ui
                await UIManager.Ins.OpenUI<UIReformCopyFighting>(GetUIArgs(ReformState.DriverSelection,null));
            }
            else
            {
                UIManager.Ins.Show<UICopy>();
                UIManager.Ins.Show<UIMainLeft>();
                UIManager.Ins.Show<UIMainRight>();
                UIManager.Ins.CloseUI<UIReformCopyFighting>();
            }
        }

        #region Get方法
        /// <summary>
        /// 获取摄像机操作配置
        /// </summary>
        /// <returns></returns>
        CameraOperation GetCameraOperation()
        {
            CameraOperation vo;
            vo.posTarget = new Vector3(-3, 1, -10);
            vo.size = 9;

            return vo;
        }

        /// <summary>
        /// 移动摄像机
        /// </summary>
        /// <param name="op"></param>
        /// <param name="immedatly"></param>
        void MoveCamera(CameraOperation op, bool immedatly = true)
        {
            if (immedatly)
            {
                Camera.main.transform.position = op.posTarget;
                Camera.main.orthographicSize = op.size;
            }
            else
            {
                DOTween.To(() => Camera.main.transform.position, (value) => Camera.main.transform.position = value, op.posTarget, 1f);
                DOTween.To(() => Camera.main.orthographicSize, (value) => Camera.main.orthographicSize = value, op.size, 1f);
            }
        }

        /// <summary>
        /// 获取随机3个选项
        /// </summary>
        List<SelectionItemVO> GetRandomSelectionItems()
        {
            var result = new List<SelectionItemVO>();

            for(int i = 0; i < 3; i ++)
            {
                //随机类型：属性 or 气缸
                ItemType itemType = GetRandomSelectionItemType();

                //获取指定类型随机选项
                SelectionItemVO item = GetRandomItem(itemType);
                result.Add(item);
            }
  
            return result;
        }

        /// <summary>
        /// 获取ui参数
        /// </summary>
        /// <param name="state"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        Tuple<ReformState, List<SelectionItemVO>> GetUIArgs(ReformState state, List<SelectionItemVO> items)
        {
            return new Tuple<ReformState, List<SelectionItemVO>>(state, items);
        }

        /// <summary>
        /// 获取随机选择项
        /// </summary>
        /// <param name="itemType"></param>
        /// <returns></returns>
        private SelectionItemVO GetRandomItem(ItemType itemType)
        {
            switch(itemType)
            {
                case ItemType.Attr:
                    return GetRandomAttrItem();
                case ItemType.Cylinder:
                    return GetRandomCylinderItem();
                default:
                    throw new Exception("没有实现对应道具类型 " + itemType);
            }
        }

        /// <summary>
        /// 获取随机气缸
        /// </summary>
        /// <returns></returns>
        SelectionItemVO GetRandomCylinderItem()
        {
            var curLevel = copyManager.CurSelectedLevel;

            //获取副本气缸等级组
            var cfg = CopyReformCfg.GetData(curLevel);
            if (cfg == null)
                cfg = CopyReformCfg.GetData(0);

            var cylinderGroup = cfg.CylinderLevelGroup;

            var groupCfg = ReformCylinderGroupCfg.GetData(cylinderGroup);

            var rate0 = groupCfg.NormalWight;
            var rate1 = groupCfg.AdvancedWight;
            var rate2 = groupCfg.RareWight;
            var rate3 = groupCfg.EpiclWight;
            var rate4 = groupCfg.LegendaryWight;
            var rate5 = groupCfg.MythicWight;
            var rate6 = groupCfg.TransWight;

            var total = rate0 + rate1 + rate2 + rate3 + rate4 + rate5 + rate6;

            var randomRateValue = UnityEngine.Random.Range(0, total + 1);
            int quality = 0;
            if(randomRateValue < rate0)
            {
                quality = 0;
            }
            else if( randomRateValue < rate0 + rate1)
            {
                quality = 1;
            }
            else if (randomRateValue < rate0 + rate1 + rate2)
            {
                quality = 2;
            }
            else if (randomRateValue < rate0 + rate1 + rate2 + rate3)
            {
                quality = 3;
            }
            else if (randomRateValue < rate0 + rate1 + rate2 + rate3 + rate4)
            {
                quality = 4;
            }
            else if (randomRateValue < rate0 + rate1 + rate2 + rate3 + rate4 + rate5)
            {
                quality = 5;
            }
            else if (randomRateValue < rate0 + rate1 + rate2 + rate3 + rate4 + rate5 + rate6)
            {
                quality = 6;
            }

            var cylinderList = new List<CylinderData>();
            CylinderCfg.GetDataList((data) => {
                return data.Quilty.Equals(quality);   
            }, cylinderList);

            var length = cylinderList.Count;
            var randomIndex = UnityEngine.Random.Range(0, length);
            var randomCylinder = cylinderList[randomIndex];
            int id = randomCylinder.ID;

            var vo = new SelectionItemVO();
            vo.itemType = ItemType.Cylinder;
            vo.id = id;
            vo.title = "道具";          
            var cylinderCfg = CylinderCfg.GetData(id);
            vo.name = cylinderCfg.Name;

            var partData = EngineManager.Ins.CreateGameEnginePartData(id);
            vo.partData = partData;

            var attrCfg1 = AttributeCfg.GetData(vo.partData.Attr1ID);
            var attrCfg2 = AttributeCfg.GetData(vo.partData.Attr2ID);
            var str1 = string.Format(attrCfg1.Des, attrCfg1.Value);
            var str2 = string.Format(attrCfg2.Des, attrCfg2.Value);

            vo.desc = str1 + "\n" + str2;

            var res = cylinderCfg.ResID;


            return vo;

        }

        /// <summary>
        /// 获取随机属性
        /// </summary>
        /// <returns></returns>
        SelectionItemVO GetRandomAttrItem()
        {
            var valueList = ConfigManager.Ins.m_ReformFightAttributeCfg.AllData.Values.ToList();
            var length = valueList.Count;
            var randomIndex = UnityEngine.Random.Range(0, length);
            var data = valueList[randomIndex];
            var id = data.AttributeID;

            var cfg = AttributeCfg.GetData(id);

            var vo = new SelectionItemVO();
            vo.itemType = ItemType.Attr;
            vo.id = id;
            vo.title = "属性";
            vo.name = cfg.Name;
            vo.desc = string.Format(cfg.Des, cfg.Value);

            var resId = cfg.Res;
            
            return vo;
        }


        /// <summary>
        /// 随机一个选择类型
        /// </summary>
        /// <returns></returns>
        private ItemType GetRandomSelectionItemType()
        {
            var cylinderRate = GameDefine.CopyReformCylinderRate;
            var attrRate = GameDefine.CopyReformAttrRate;

            var randomValue = UnityEngine.Random.Range(0, cylinderRate + attrRate + 1);
            
            if(randomValue < cylinderRate)
            {
                return ItemType.Cylinder;
            }

            return ItemType.Attr;

        }

        /// <summary>
        /// 获取改造列表
        /// </summary>
        /// <returns></returns>
        public List<SelectionItemVO> GetSelectionList()
        {
            return selectionList;
        }


        public List<GameEnginePartData> GetEnginePartData()
        {
            return gameEnginePartData;
        }


        #endregion


        public override void RequestEnterCopy()
        {
            //CurSelectedLevel = pCurSelectLevel;
            NetworkManager.Ins.SendMsg(new C2S_EnterCopy { LevelType = (int)LevelType.ReformCopy });
        }

        /// <summary>
        /// 退出副本战斗
        /// </summary>
        public override void RequestExitCopy(bool isWin)
        {
            NetworkManager.Ins.SendMsg(new C2S_ExitCopy
            {
                LevelType = (int)LevelType.ReformCopy,
                LstEnginePartData = GetEnginePartData(),
                IsWin = isWin

            });
        }

    }



}