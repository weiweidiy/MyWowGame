using System.Collections.Generic;
using UnityEngine;

namespace Logic.Common.RedDot
{
    public class BigBoomParentRedDotMono : RedDotParentNode
    {
        [SerializeField] List<RedDotKey> lstInteresting;

        protected override List<string> GetInterestingKeys()
        {
            var result = new List<string>();

            foreach (var i in lstInteresting)
            {

                result.Add(i.ToString());
            }

            return result;
        }

        protected override RedDotManager GetRedDotManager()
        {
            return BigBoomRedDotManager.Ins;
        }
    }
}