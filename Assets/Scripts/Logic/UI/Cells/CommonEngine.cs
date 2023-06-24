using System;
using Configs;
using Framework.EventKit;
using Framework.Extension;
using Logic.Common;
using Logic.Manager;
using Logic.UI.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.Cells
{
    public class CommonEngine : MonoBehaviour
    {
        public int soltID { get; private set; }
        public int insID { get; private set; }

        public Image quality;
        public Image icon;
        public GameObject empty;

        public Action<int> clickEngine;
        private EventGroup eventGroup = new EventGroup();

        private void Awake()
        {
            //注册
            eventGroup.Register(LogicEvent.EngineOn, OnEngineOn);
            eventGroup.Register(LogicEvent.EngineOff, OnEngineOff);
        }

        private void OnDestroy()
        {
            //注销
            clickEngine = null;
            eventGroup.Release();
        }

        /// <summary>
        /// 初始化引擎上阵位ID
        /// </summary>
        /// <param name="pID"></param>
        public void Init(int pID)
        {
            soltID = pID;
            insID = EngineManager.Ins.gameEngineData.OnList[soltID - 1];
            UpdateEngine(insID);
        }

        //更新引擎上阵的火花塞和气缸
        private void UpdateEngine(int pInsID)
        {
            //获取实例数据
            var engineData = EngineManager.Ins.GetGameEngineData(pInsID);
            //当前引擎上阵位是否有装备
            if (engineData != null)
            {
                //当前上阵位有装备
                var pQuality = 0;
                var pResID = 0;
                switch ((ItemType)engineData.Type)
                {
                    //火花塞
                    case ItemType.Spark:
                        var sparkData = SparkCfg.GetData(engineData.CfgID);
                        pQuality = sparkData.Quality;
                        pResID = sparkData.ResID;
                        break;
                    //气缸
                    case ItemType.Cylinder:
                        var cylinderData = CylinderCfg.GetData(engineData.CfgID);
                        pQuality = cylinderData.Quilty;
                        pResID = cylinderData.ResID;
                        break;
                }

                //加载品质
                UICommonHelper.LoadQuality(quality, pQuality);
                //加载Icon
                // UICommonHelper.LoadIcon(icon, ResCfg.GetData(pResID).Res);

                quality.Show();
                icon.Show();
                empty.Hide();
            }
            else
            {
                //当前上阵位无装备
                quality.Hide();
                icon.Hide();
                empty.Show();
            }
        }

        /// <summary>
        /// 引擎上阵变化
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="data"></param>
        private void OnEngineOn(int eventId, object data)
        {
            var (pInsID, pSlotID) = (ValueTuple<int, int>)data;
            if (pSlotID == soltID)
            {
                UpdateEngine(pInsID);
            }
        }

        /// <summary>
        /// 引擎下阵变化
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="data"></param>
        private void OnEngineOff(int eventId, object data)
        {
            var (pInsID, pSlotID) = (ValueTuple<int, int>)data;
            if (pSlotID == soltID)
            {
                insID = 0;
                UpdateEngine(insID);
            }
        }

        /// <summary>
        /// 点击引擎按钮
        /// </summary>
        public void OnBtnEngineClick()
        {
            clickEngine?.Invoke(soltID);
        }
    }
}