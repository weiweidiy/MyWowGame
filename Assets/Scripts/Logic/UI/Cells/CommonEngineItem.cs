using System;
using System.Collections.Generic;
using Configs;
using Framework.EventKit;
using Framework.Extension;
using Logic.Common;
using Logic.Manager;
using Logic.UI.Common;
using Networks;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.Cells
{
    public class CommonEngineItem : MonoBehaviour
    {
        public GameEnginePartData gameEngineData { get; private set; }
        public bool IsOn { get; private set; }
        public bool IsRemoveSelected { get; private set; }

        public Image quality;
        public Image icon;
        public GameObject selected;
        public GameObject removeSelected;
        public GameObject isOn;
        public Action<CommonEngineItem> clickEngine;
        private EventGroup eventGroup = new();


        private void Awake()
        {
            eventGroup.Register(LogicEvent.EngineOn, OnEngineOn);
            eventGroup.Register(LogicEvent.EngineOff, OnEngineOff);
            eventGroup.Register(LogicEvent.EngineResolve, OnEngineResolve);
        }

        private void OnDestroy()
        {
            clickEngine = null;
            eventGroup.Release();
        }

        //初始化
        public void Init(GameEnginePartData pData)
        {
            //数据
            gameEngineData = pData;
            //品质
            var pQuality = 0;
            //Icon资源
            var pResID = 0;
            switch ((ItemType)gameEngineData.Type)
            {
                //火花塞
                case ItemType.Spark:
                    var sparkData = SparkCfg.GetData(gameEngineData.CfgID);
                    pQuality = sparkData.Quality;
                    pResID = sparkData.ResID;
                    break;
                //气缸
                case ItemType.Cylinder:
                    var cylinderData = CylinderCfg.GetData(gameEngineData.CfgID);
                    pQuality = cylinderData.Quilty;
                    pResID = cylinderData.ResID;
                    break;
            }

            //加载品质
            UICommonHelper.LoadQuality(quality, pQuality);
            //加载Icon
            // UICommonHelper.LoadIcon(icon, ResCfg.GetData(pResID).Res);
            //刷新装配按钮状态
            IsOn = EngineManager.Ins.IsEngineOn(gameEngineData.InsID);
            isOn.gameObject.SetActive(IsOn);
        }

        //装配
        private void OnEngineOn(int eventId, object data)
        {
            var (pInsID, pSlotID) = (ValueTuple<int, int>)data;
            if (pInsID != gameEngineData.InsID) return;

            IsOn = true;
            isOn.Show();
        }

        //解除
        private void OnEngineOff(int eventId, object data)
        {
            var (pInsID, pSlotID) = (ValueTuple<int, int>)data;
            if (pInsID != gameEngineData.InsID) return;

            IsOn = false;
            isOn.Hide();
        }

        //分解
        private void OnEngineResolve(int eventId, object data)
        {
            var pInsIDList = (List<int>)data;
            foreach (var pInsID in pInsIDList)
            {
                if (pInsID == gameEngineData.InsID)
                {
                    this.gameObject.Destroy();
                }
            }
        }

        //点击
        public void OnClickEngine()
        {
            clickEngine?.Invoke(this);
        }

        //选中
        public void ShowSelected()
        {
            selected.Show();
        }

        //未选择
        public void HideSelected()
        {
            selected.Hide();
        }

        //批量删除选择
        public void RemoveSelected()
        {
            IsRemoveSelected = !IsRemoveSelected;
            removeSelected.SetActive(IsRemoveSelected);
        }
    }
}