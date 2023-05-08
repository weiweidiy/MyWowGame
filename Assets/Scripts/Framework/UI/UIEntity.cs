using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.UI;
using UnityEngine;
using YooAsset;

namespace UIKit
{
    //UI实体 - 方便Manager管理UI
    public class UIEntity
    {
        //状态
        public UIState m_State = UIState.UnLoad;

        //UI资源路径
        public string m_UIPath = "";

        //UI实例
        public UIPage m_UIPage;

        //UI GO
        public GameObject m_GameObject;
        
        //UI ABRes
        public AssetOperationHandle m_ResHandle;
    }

    //--------------------------------------------------------------

    //打开UIMsgBox传递数据
    public class MsgBox_Data
    {
        public MsgBoxBtnType BtnType;
        public string Title;
        public string Content;
        public Action m_LeftCB;
        public Action m_MiddleCB;
        public Action m_RightCB;
    }
}
