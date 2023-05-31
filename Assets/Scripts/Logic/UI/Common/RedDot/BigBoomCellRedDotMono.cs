using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.Common.RedDot
{
    //[DefaultExecutionOrder(10000)]
    public class BigBoomCellRedDotMono : RedDotCellNode
    {
        [SerializeField] List<RedDotKey> lstInteresting;

        protected override List<string> GetInterestingKeys()
        {
            var result = new List<string>();

            foreach(var i in lstInteresting)
            {
                result.Add(i.ToString());
            }

            return result;
        }

        protected override RedDotManager GetRedDotManager()
        {
            return BigBoomRedDotManager.Ins;
        }

        public void AddInteresting(RedDotKey key)
        {
            AddInteresting(key.ToString());
        }

    }
}