using System;
using Cysharp.Threading.Tasks;
using Framework.Extension;
using Framework.UI;
using Logic.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.UIGuidance
{
    public class UIGuidance : UIPage
    {
        //手指相关
        public Animation fingerAnim;
        private int curAnimType;
        private int tempAnimType;
        public Transform finger;
        private Vector3 fingerOrilPos;
        private Vector3 fingerOriScale;
        private Vector3 fingerOffset;

        //引导相关
        public GameObject bk;
        private GuidanceType guidanceType;

        private GameObject go;
        private ScrollRect scrollRect;
        private float viewportTopY;
        private float viewportBottomY;
        public Canvas fingerCanvas;

        /// <summary>
        /// 初始化
        /// </summary>
        private void Awake()
        {
            fingerOrilPos = finger.localPosition;
            fingerOriScale = finger.localScale;
            fingerOffset = new Vector3(0, 0.5f, 0);

            m_EventGroup.Register(LogicEvent.GuidanceStart, OnGuidanceStart);
            m_EventGroup.Register(LogicEvent.GuidanceEnd, OnGuidanceEnd);
        }

        private void Update()
        {
            //更新手指位置
            UpdateFinger();
        }

        /// <summary>
        /// 更新手指位置
        /// </summary>
        private void UpdateFinger()
        {
            if (go != null)
            {
                //设置手指位置
                finger.position = go.transform.position - fingerOffset;
                tempAnimType = 1;
                //更新滑动面板内手指位置
                if (scrollRect != null && scrollRect.enabled)
                {
                    if (finger.localPosition.y > viewportTopY)
                    {
                        finger.localPosition = fingerOrilPos;
                        tempAnimType = 3;
                    }

                    if (finger.localPosition.y < viewportBottomY)
                    {
                        finger.localPosition = fingerOrilPos;
                        tempAnimType = 2;
                    }
                }

                //设置手指动画
                if (curAnimType == tempAnimType) return;
                curAnimType = tempAnimType;
                //动画播放类型
                string[] animationNames = { "", "G_Point", "G_Up", "G_Down" };
                //播放动画
                fingerAnim.Play(animationNames[tempAnimType]);
            }
        }

        /// <summary>
        /// 接收引导开启事件
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="data"></param>
        private void OnGuidanceStart(int eventId, object data)
        {
            var (type, path) = (ValueTuple<GuidanceType, string>)data;
            StartGuide(type, path);
        }

        /// <summary>
        /// 接收引导关闭事件
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="data"></param>
        private void OnGuidanceEnd(int eventId, object data)
        {
            ResetGuide();
        }

        /// <summary>
        /// 开启引导
        /// </summary>
        /// <param name="pType"></param>
        /// <param name="pPath"></param>
        private void StartGuide(GuidanceType pType, string pPath)
        {
            SetGuide(pType, pPath);
        }

        /// <summary>
        /// 设置引导
        /// </summary>
        /// <param name="pType"></param>
        /// <param name="pPath"></param>
        private void SetGuide(GuidanceType pType, string pPath)
        {
            guidanceType = pType;
            go = GameObject.Find(pPath);
            scrollRect = go.GetComponentInParent<ScrollRect>();
            if (go != null)
            {
                finger.gameObject.Show();
                //强引导
                if (guidanceType == GuidanceType.Forced)
                {
                    bk.Show();
                    //添加Canvas和GraphicRaycaster组件
                    var canvas = go.AddComponent<Canvas>();
                    canvas.overrideSorting = true;
                    canvas.sortingOrder = 3001;
                    go.AddComponent<GraphicRaycaster>();

                    //禁用ScrollRect组件
                    if (scrollRect != null)
                    {
                        scrollRect.enabled = false;
                    }
                }
                else
                {
                    //软引导
                    bk.Hide();
                    if (scrollRect != null)
                    {
                        var viewportRect = scrollRect.viewport.rect;
                        viewportTopY = viewportRect.yMax;
                        viewportBottomY = viewportRect.yMin;
                    }

                    fingerCanvas.sortingOrder = go.GetComponentInParent<Canvas>().sortingOrder + 1;
                }
            }
            else
            {
                Debug.LogError($"{pPath} not found");
            }
        }

        /// <summary>
        /// 重置手指动画
        /// </summary>
        private async void ResetGuide()
        {
            fingerAnim.Stop();
            curAnimType = 0;
            finger.localPosition = fingerOrilPos;
            finger.localScale = fingerOriScale;

            //强引导
            if (guidanceType == GuidanceType.Forced)
            {
                //删除Canvas和GraphicRaycaster组件
                go.RemoveComponentIfExists<GraphicRaycaster>();
                go.RemoveComponentIfExists<Canvas>();
                go.SetActive(false);
                await UniTask.NextFrame();
                go.SetActive(true);

                //启用ScrollRect组件
                if (scrollRect != null)
                {
                    scrollRect.enabled = true;
                }

                scrollRect = null;
            }

            fingerCanvas.sortingOrder = 3002;

            go = null;

            finger.gameObject.Hide();
            bk.Hide();
        }
    }
}