using Framework.UI;
using Logic.Common;
using Logic.UI.UICopy;
using Logic.UI.UIPlaceRewards;
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

                await UIManager.Ins.OpenUI<UIOilRewards>(o);
            });

            m_EventGroup.Register(LogicEvent.ShowReformCopyRewards, async (i, o) =>
            {
                await UIManager.Ins.OpenUI<UIReformRewards>(o);
            });
        }
    }
}