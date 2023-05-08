
using Sirenix.OdinInspector;

namespace UIKit
{
    //UI层级
    public enum UILayer
    {
        [LabelText("忽略")]
        Ignore,     //忽略
        [LabelText("底层")]
        BackGround, //背景层 - 最低
        [LabelText("正常")]
        Normal,     //默认
        [LabelText("弹出")]
        Pop,        //弹出层
        [LabelText("引导")]
        Guide,      //引导层
        [LabelText("最高")]
        Top,        //最高层 - 最高
    }

    //UI类型
    // public enum UIType
    // {
    //     None, 
    // }

    //UI状态
    public enum UIState
    {
        UnLoad, //未加载
        Loading, //加载中
        Show, //显示
        Hide, //隐藏
    }

    //MsgBox 显示按钮类型
    public enum MsgBoxBtnType
    {
        OnlyClose,
        OneBtn,
        TwoBtn,
    }
}
