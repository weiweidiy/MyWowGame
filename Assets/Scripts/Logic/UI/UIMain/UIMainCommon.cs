using Framework.UI;
using Logic.Common;
using Logic.UI.UICopy;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Logic.UI.UIMain
{
    public class UIMainCommon : UIPage
    {
        private void Awake()
        {
            m_EventGroup.Register(LogicEvent.ShowOilCopyRewards, async (i, o) =>
            {

                await UIManager.Ins.OpenUI<UIPlaceRewards.UIOilRewards>(o);
            });
        }
    }
}