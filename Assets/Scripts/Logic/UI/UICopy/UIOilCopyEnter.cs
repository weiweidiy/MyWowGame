using BreakInfinity;
using Framework.EventKit;
using Framework.UI;
using Logic.Common;
using Logic.Manager;
using TMPro;
using UnityEngine;

namespace Logic.UI.UICopy
{
    public class UIOilCopyEnter : UIPage
    {
        public TextMeshProUGUI m_KeyCount;

        public TextMeshProUGUI m_BestDamageRecord;

        public TextMeshProUGUI m_BestLevelRecord;

        [Tooltip("原油副本表中的副本ID")]
        public int m_OilCopyLevel = 1;

        public override void OnShow()
        {
            m_KeyCount.text = $"{CopyManager.Ins.m_OilCopyData.KeyCount}/2";

            BigDouble bestDamage = 0;
            if (CopyManager.Ins.m_OilCopyData.BestDamageRecord != "")
            {
                bestDamage = BigDouble.Parse(CopyManager.Ins.m_OilCopyData.BestDamageRecord);
            }

            m_BestDamageRecord.text =$"最高记录：<color=#2CEC3C>{bestDamage.ToUIString()} </color>";
            m_BestLevelRecord.text = $"{CopyManager.Ins.m_OilCopyData.BestLevelRecord}";
        }

        public void OnClickEnter()
        {
            int _KeyCount = CopyManager.Ins.m_OilCopyData.KeyCount;

            if (_KeyCount <= 0)
            {
                EventManager.Call(LogicEvent.ShowTips, "钥匙不足");
                return;
            }

            CopyManager.Ins.SendEnterCopy(LevelType.OilCopy, m_OilCopyLevel);
            Hide();
        }
    }
}