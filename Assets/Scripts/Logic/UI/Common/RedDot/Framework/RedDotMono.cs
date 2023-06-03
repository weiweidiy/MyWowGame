using System;
using System.Collections.Generic;
using Framework.Extension;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.Common.RedDot
{
    public abstract class RedDotMono : MonoBehaviour
    {
        [SerializeField] Image img;
        [SerializeField] protected TextMeshProUGUI txt;

        /// <summary>
        /// 红点类型对应的资源
        /// </summary>
        [SerializeField] Sprite[] redDots; // 0 normal, 1,new 2,full

        /// <summary>
        /// 是否初始化过
        /// </summary>
        bool init = false;

        /// <summary>
        /// 感兴趣的事
        /// </summary>
        protected List<string> interesting = new List<string>();

        private void Awake()
        {
            img.Hide();
            interesting = GetInterestingKeys();
        }

        private void OnEnable()
        {
            if (init == true)
                Regist();
        }


        private void Start()
        {
            //Debug.LogError("红点注册");
            Regist();

            init = true;
        }

        /// <summary>
        /// 子类提供
        /// </summary>
        /// <returns></returns>
        protected abstract RedDotManager GetRedDotManager();

        /// <summary>
        /// 获取感兴趣的红点配置
        /// </summary>
        /// <returns></returns>
        protected abstract List<string> GetInterestingKeys();

        /// <summary>
        /// 获取红点样式索引
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected abstract int GetSpriteIndex(RedDotInfo info);


        /// <summary>
        /// 红点显示规则
        /// </summary>
        /// <param name="showInfo"></param>
        /// <returns></returns>
        protected bool GetRedDotEnable(RedDotInfo showInfo)
        {
            return showInfo.status != RedDotStatus.Null;
        }

        /// <summary>
        /// 获取文本内容
        /// </summary>
        /// <param name="showInfo"></param>
        /// <returns></returns>
        protected abstract string GetTextContent(RedDotInfo showInfo);

        /// <summary>
        /// 注册
        /// </summary>
        protected void Regist()
        {
            foreach (var key in interesting)
            {
                DoRegist(key, OnStatusUpdate);
            }
        }

        protected abstract void DoRegist(string key, Action<string, RedDotInfo, string> action);

        /// <summary>
        /// 反注册
        /// </summary>
        protected void UnRegist()
        {
            foreach (var key in interesting)
            {
                DoUnRegist(key, OnStatusUpdate);
            }
        }

        protected abstract void DoUnRegist(string key, Action<string, RedDotInfo, string> onStatusUpdate);


        /// <summary>
        /// 动态添加感兴趣的事件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isParentNode"></param>
        public void AddInteresting(string key)
        {
            if (interesting == null)
                interesting = new List<string>();

            interesting.Add(key);
        }

        /// <summary>
        /// 状态更新
        /// </summary>
        /// <param name="type"></param>
        /// <param name="info"></param>
        /// <param name="uid"></param>
        protected virtual void OnStatusUpdate(string type, RedDotInfo info, string uid)
        {
            img.gameObject.SetActive(GetRedDotEnable(info));
            txt.text = GetTextContent(info);
            int iconIndex = GetSpriteIndex(info);
            img.sprite = redDots[iconIndex];
        }

        private void OnDisable()
        {
            UnRegist();
        }
    }
}