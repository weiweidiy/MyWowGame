using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Logic.Common.RedDot
{
    public abstract class RedDotParentNode : RedDotMono
    {
        /// <summary>
        /// 是否强制隐藏数字
        /// </summary>
        [SerializeField] bool forceHideNumber;

        /// <summary>
        /// 所以关注事件的状态
        /// </summary>
        protected Dictionary<string, RedDotInfo> dicInfo = new Dictionary<string, RedDotInfo>();

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        protected override void DoRegist(string key, Action<string, RedDotInfo, string> action)
        {
            dicInfo.Add(key, new RedDotInfo("", RedDotStatus.Null, 0));
            GetRedDotManager().Regist(key, OnStatusUpdate, "");
        }

        /// <summary>
        /// 反注册事件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="onStatusUpdate"></param>
        protected override void DoUnRegist(string key, Action<string, RedDotInfo, string> onStatusUpdate)
        {
            GetRedDotManager().UnRegist(key, OnStatusUpdate);
            dicInfo.Clear();
        }


        /// <summary>
        /// 获取所有子类红点总和
        /// </summary>
        /// <returns></returns>
        protected int GetSumNumber()
        {
            var result = from n in dicInfo
                         select n.Value.number;

            return result.Sum();
        }


        /// <summary>
        /// 整合红点数据
        /// </summary>
        /// <param name="dicInfo"></param>
        /// <returns></returns>
        protected virtual RedDotInfo MergeInfo(Dictionary<string, RedDotInfo> dicInfo)
        {
            RedDotInfo result = new RedDotInfo();

            int number = 0;
            foreach (var key in dicInfo.Keys)
            {
                var info = dicInfo[key];

                //只要有一个不为空，就跟着他显示
                if (info.status != RedDotStatus.Null)
                    result.status = info.status;

                number += info.number;
            }

            result.number = number;

            return result;
        }

        /// <summary>
        /// 状态更新显示规则： 整合所有红点数值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="info"></param>
        /// <param name="uid"></param>
        protected override void OnStatusUpdate(string type, RedDotInfo info, string uid)
        {
            //更新缓存数据
            dicInfo[type] = info;

            //整合最终数据
            var showInfo = MergeInfo(dicInfo);

            base.OnStatusUpdate(type, showInfo, uid);

            if (forceHideNumber)
                txt.text = "";
        }

        /// <summary>
        /// 文本显示规则
        /// </summary>
        /// <param name="showInfo"></param>
        /// <returns></returns>
        protected override string GetTextContent(RedDotInfo showInfo)
        {
            return showInfo.status == RedDotStatus.Number ? GetSumNumber().ToString() : "";
        }


        /// <summary>
        /// 获取红点样式
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override int GetSpriteIndex(RedDotInfo info)
        {
            switch (info.status)
            {
                case RedDotStatus.New:
                    return 1;
                case RedDotStatus.Full:
                    return 2;
                default:
                    return 0;
            }
        }

    }
}

