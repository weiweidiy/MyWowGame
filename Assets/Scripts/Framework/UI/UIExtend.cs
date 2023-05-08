using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.UI;

namespace UIKit
{
    //UI相关的操作

    public static class UIExtend
    {
        //关闭自己
        public static void CloseUI(this UIPage pPage)
        {
            UIManager.Ins.CloseUI(pPage);
        }
    }
}
