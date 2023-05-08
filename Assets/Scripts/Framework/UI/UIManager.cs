using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Framework.Extension;
using Framework.Helper;
using UIKit;
using UnityEngine;
using YooAsset;

namespace Framework.UI
{
    public class UIManager : PersistentMonoSingleton<UIManager>
    {
        //UI根目录
        public GameObject m_UIRoot { get; set; }

        //不同层级
        public GameObject m_Ignore { get; set; }       //忽略层
        public GameObject m_BackGround { get; set; }    //背景层 - 最低
        public GameObject m_Normal { get; set; }        //默认
        public GameObject m_Pop { get; set; }           //弹出层
        public GameObject m_Guide { get; set; }         //引导层
        public GameObject m_Top { get; set; }           //最高层 - 最高

        //private string m_UIPathPreFix = "Assets/GameRes/UI/Prefabs/";

        //UI数据保存
        private readonly Dictionary<string, UIEntity> m_UIMap = new (64);

        protected override void Awake()
        {
            base.Awake();
            
            m_UIMap.Clear();

            m_UIRoot = GameObjectHelper.FindOrCreateGameObject("UIRoot").DontDestroy().SetPosition(new Vector3(0, 0, 0));
            m_Ignore = m_UIRoot.FindChildGameObject("Ignore");
            m_BackGround = m_UIRoot.FindChildGameObject("BackGround");
            m_Normal = m_UIRoot.FindChildGameObject("Normal");
            m_Pop = m_UIRoot.FindChildGameObject("Pop");
            m_Guide = m_UIRoot.FindChildGameObject("Guide");
            m_Top = m_UIRoot.FindChildGameObject("Top");
        }

        #region 打开UI接口
        //通过类型打开UI
        public async UniTask<UIPage> OpenUI<T>(object pOpenData = null) where T : UIPage
        {
            string _UIPath = GetUIPathByType<T>();
            return await OpenUI(_UIPath, pOpenData);
        }
        
        //通过类型打开UI - 加载完成后 回调
        public async void OpenUI<T>(Action<UIPage>pCallBack, object pOpenData = null) where T : UIPage
        {
            var _Page = await OpenUI<T>(pOpenData);
            if(_Page != null && pCallBack != null)
                pCallBack.Invoke(_Page);
        }
    
        //直接指定路径
        public async UniTask<UIPage> OpenUI(string pUIPath, object pOpenData = null)
        {
            var _Entity = Check(pUIPath);
            //Check 是否已经打开
            if (_Entity != null)
            {
                if (_Entity.m_State == UIState.Loading)
                {
                    Debug.LogError("正在加载不要重复打开 : " + pUIPath);
                    return null;
                }

                _Entity.m_UIPage.m_OpenData_ = pOpenData;

                //如果当前是显示的, 先隐藏一次 然后重新显示,主要用来让UI可以刷新一些数据
                if (_Entity.m_State == UIState.Show)
                    Hide(_Entity);
                Show(_Entity);
                return _Entity.m_UIPage;
            }

            _Entity = new UIEntity
            {
                m_State = UIState.Loading,
                m_UIPath = pUIPath
            };
            m_UIMap.Add(pUIPath, _Entity);

            //加载UI预设
            var _Handle = YooAssets.LoadAssetAsync<GameObject>(pUIPath);
            await _Handle.ToUniTask();

            if (!_Handle.IsDone)
            {
                m_UIMap.Remove(pUIPath);
                Debug.LogError("UI加载出错 资源不存在 : " + pUIPath);
                return null;
            }

            //创建go
            var _GoHandle = _Handle.InstantiateAsync();
            await _GoHandle;

            var _UIObj = _GoHandle.Result;
            var _UIPage = _UIObj.GetComponent<UIPage>();
            if (_UIPage == null)
            {
                m_UIMap.Remove(pUIPath);
                Destroy(_UIObj);
                Debug.LogError("UI加载出错 资源没有UIPage : " + pUIPath);
                return null;
            }

            _Entity.m_GameObject = _UIObj;
            _Entity.m_UIPage = _UIPage;
            _Entity.m_ResHandle = _Handle;
            _UIPage.m_Entity = _Entity;
            _UIPage.m_OpenData_ = pOpenData;

            switch (_UIPage.m_Layer)
            {
                case UILayer.Ignore:
                    _UIObj.SetParent(m_Ignore);
                    break;
                case UILayer.BackGround:
                    _UIObj.SetParent(m_BackGround);
                    break;
                case UILayer.Pop:
                    _UIObj.SetParent(m_Pop);
                    break;
                case UILayer.Guide:
                    _UIObj.SetParent(m_Guide);
                    break;
                case UILayer.Top:
                    _UIObj.SetParent(m_Top);
                    break;
                case UILayer.Normal:
                default:
                    _UIObj.SetParent(m_Normal);
                    break;
            }
            
            Show(_Entity);
            //UI重置
            if(_UIPage.m_NeedReset)
                _UIObj.transform.ReSetUI();
            _Entity.m_UIPage.ShowMask();
            return _UIPage;
        }
        
        //指定路径打开UI - 加载完成后 回调
        public async void OpenUI(string pUIPath, Action<UIPage>pCallBack, object pOpenData = null)
        {
            var _Page = await OpenUI(pUIPath, pOpenData);
            if(_Page != null)
                pCallBack.Invoke(_Page);
        }
        #endregion

        private UIEntity Check(string pUIPath)
        {
            return m_UIMap.TryGetValue(pUIPath, out var _Entity) ? _Entity : null;
        }

        //设置到当前节点下最高层
        private void TopUI(UIEntity pEntity)
        {
            pEntity.m_GameObject.Top();
        }

        #region 关闭UI
        //关闭一个UI
        public void CloseUI(UIPage pPage)
        {
            CloseUI(pPage.m_Entity);
        }

        public void CloseUI(UIEntity pEntity)
        {
            pEntity.m_State = UIState.UnLoad;
            m_UIMap.Remove(pEntity.m_UIPath);
            Destroy(pEntity.m_GameObject);
            pEntity.m_ResHandle.Release();
        }

        public void CloseUI(List<UIEntity> pEntityList)
        {
            foreach (var _Entity in pEntityList)
            {
                CloseUI(_Entity);
            }
        }

        public void CloseUI<T>() where T : UIPage
        {
            string _UIPath = GetUIPathByType<T>();
            if (m_UIMap.TryGetValue(_UIPath, out var _Entity))
            {
                CloseUI(_Entity);
            }
            else
            {
                Debug.LogError("关闭UI没有找到对应的UI实例 : " + _UIPath);
            }
        }
        
        public void CloseUI(string pUIPath)
        {
            if (m_UIMap.TryGetValue(pUIPath, out var _Entity))
            {
                CloseUI(_Entity);
            }
            else
            {
                Debug.LogError("关闭UI没有找到对应的UI实例 : " + pUIPath);
            }
        }

        //关闭所有UI
        public void CloseAll(bool pIncludeKeep = true)
        {
            var _Entities = m_UIMap.Values;
            var _NeedDels = new List<UIEntity>();
            foreach (var _Entity in _Entities)
            {
                if(pIncludeKeep)
                    _NeedDels.Add(_Entity);
                else
                {
                    if(!_Entity.m_UIPage.m_AlwaysKeep)
                        _NeedDels.Add(_Entity);
                }
            }

            CloseUI(_NeedDels);
        }

        public void CloseAllExcept(UIPage pExceptUI, bool pIncludeKeep = true)
        {
            var _Entities = m_UIMap.Values;
            var _NeedDels = new List<UIEntity>();

            foreach (var _Entity in _Entities)
            {
                if (_Entity != pExceptUI.m_Entity)
                {
                    if (pIncludeKeep)
                        _NeedDels.Add(_Entity);
                    else
                    {
                        if (!_Entity.m_UIPage.m_AlwaysKeep)
                            _NeedDels.Add(_Entity);
                    }
                }
            }

            CloseUI(_NeedDels);
        }
        #endregion
        
        //当前UI数量
        public int GetAllUICount()
        {
            return m_UIMap.Count;
        }

        //获取当前处于显示状态的UI数量
        public int GetAllShowUICount()
        {
            var _ICount = 0;
            foreach (var _Entity in m_UIMap.Values)
            {
                if (_Entity.m_State == UIState.Show)
                    _ICount++;
            }
            return _ICount;
        }

        //隐藏所有UI
        public void HideAllUI()
        {
            foreach (var _Entity in m_UIMap.Values)
            {
                if (_Entity.m_State == UIState.Show)
                {
                    Hide(_Entity);
                }
            }
        }

        //获得指定UI
        public UIPage GetUI(string pUIPath)
        {
            return Check(pUIPath)?.m_UIPage;
        }

        public UIPage GetUI(UIEntity pEntity)
        {
            return pEntity.m_UIPage;
        }

        public T GetUI<T>() where T : UIPage
        {
            var _T = GetUI(GetUIPathByType<T>());
            if (_T == null)
                return null;
            return _T as T;
        }

        //显示指定UI - 已经是显示状态 将不会进行任何操作
        //pNeedLoad - 如果UI不存在是否自动加载
        public async void Show<T>(object pOpenData = null, bool pNeedLoad = false) where T : UIPage
        {
            string _UIPath = GetUIPathByType<T>();
            var _Entity = Check(_UIPath);
            if (_Entity != null)
            {
                _Entity.m_UIPage.m_OpenData_ = pOpenData;
                //如果当前是显示的, 先隐藏一次 然后重新显示,主要用来让UI可以刷新一些数据
                if (_Entity.m_State == UIState.Show)
                    Hide(_Entity);
                Show(_Entity);
            }
            else
            {
                if (pNeedLoad)
                    await OpenUI<T>(pOpenData);
                else
                    Debug.LogError("Show UI不存在 : " + _UIPath);
            }
        }

        public void Show(UIEntity pEntity)
        {
            if (pEntity.m_State != UIState.Show)
            {
                pEntity.m_State = UIState.Show;
                pEntity.m_GameObject.Show();
                pEntity.m_UIPage.OnShow();
                TopUI(pEntity);
            }
        }

        //隐藏UI
        public void Hide<T>() where T : UIPage
        {
            string _UIPath = GetUIPathByType<T>();
            var _Entity = Check(_UIPath);
            if (_Entity != null)
            {
                Hide(_Entity);
            }
        }

        public void Hide(UIEntity pEntity)
        {
            if (pEntity.m_State == UIState.Show)
            {
                pEntity.m_State = UIState.Hide;
                if(pEntity.m_GameObject != null) //Editor 停止运行
                    pEntity.m_GameObject.Hide();
            }
        }

        //获取资源路径
        public string GetUIPathByType<T>() where T : UIPage
        {
            return typeof(T).Name;
        }
    }
}
