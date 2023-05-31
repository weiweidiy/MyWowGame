using System;
using Cysharp.Threading.Tasks;
using Framework.EventKit;
using Framework.Extension;
using Sirenix.OdinInspector;
using UIKit;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.UI
{
    //所有UI的基类
    //UI静态设置
    //所有对UI的显示和隐藏都通过UIManager进行

    public class UIPage : MonoBehaviour
    {
        [NonSerialized] public UIEntity m_Entity;

        //UI层级
        [EnumToggleButtons, LabelText("UI层级")] public UILayer m_Layer = UILayer.Normal;

        //是否一直保留
        [LabelText("CloseAll 时是否保留"), Tooltip("一些通用UI打开以后就不会再销毁")]
        public bool m_AlwaysKeep = false;

        [LabelText("是否需要重置UI"), Tooltip("重置UI的锚点和坐标等参数, 部分特殊UI可能不需要")]
        public bool m_NeedReset = true;
        
        //UI类型
        //public UIType m_Type = UIType.None;

        //是否需要Mask层
        [TabGroup("背景层"), LabelText("是否添加背景")] public bool m_NeedMask = false;

        [TabGroup("背景层"), ShowIf("m_NeedMask"), LabelText("背景颜色")]
        private Color m_MaskColor = new Color(0f, 0f, 0f, 230f / 255f);

        private GameObject m_MaskObj;
        protected readonly EventGroup m_EventGroup = new();

        [TabGroup("背景层"), ShowIf("m_NeedMask"), LabelText("点击背景关闭")]
        public bool m_MaskClick = false;

        /// <summary>
        /// 打开UI时 传递的数据 要在OnShow回调中处理
        /// </summary>
        [HideInInspector] public object m_OpenData_ = null;

        //关闭自己
        public void Close()
        {
            UIManager.Ins.CloseUI(this);
        }

        //显示 - 已经是显示状态无效果
        public void Show()
        {
            UIManager.Ins.Show(m_Entity);
        }

        //隐藏
        public void Hide()
        {
            UIManager.Ins.Hide(m_Entity);
        }

        /// <summary>
        /// 某些情况下替代OnEnable, 主要是执行顺序的问题
        /// 这个函数会在一些赋值完成后才会调用(UI传递的值)
        /// </summary>
        public virtual void OnShow()
        {
            
        }

        public virtual void OnDestroy()
        {
            m_EventGroup.Release();
        }

        //获取传递的数据
        protected T GetOpenData<T>()
        {
            return (T)m_OpenData_;
        }
        
        public void ShowMask()
        {
            if (!m_NeedMask)
                return;

            GameObject _GameObject;
            string _MaskName = "Mask_" + (_GameObject = gameObject).GetInstanceID();
            m_MaskObj = gameObject.FindOrCreateGameObject(_MaskName).SetLayerRecursive(_GameObject.layer);
            m_MaskObj.transform.SetAsFirstSibling();

            var _Image = m_MaskObj.GetComponentOrAdd<Image>();
            _Image.color = m_MaskColor;
            _Image.raycastTarget = true;

            var _RectTrans = m_MaskObj.GetComponent<RectTransform>();
            _RectTrans.anchorMin = Vector2.zero;
            _RectTrans.anchorMax = Vector2.one;
            _RectTrans.localScale = Vector3.one;
            _RectTrans.sizeDelta = Vector2.zero;
            _RectTrans.localPosition = Vector3.zero;

            //m_MaskObj.GetComponentOrAdd<GraphicRaycaster>();
            if (m_MaskClick)
            {
                var _Btn = m_MaskObj.GetComponentOrAdd<Button>();
                _Btn.transition = Selectable.Transition.None;
                _Btn.targetGraphic = _Image;
                _Btn.onClick.RemoveAllListeners();
                _Btn.onClick.AddListener(Hide);
            }
        }
    }
}