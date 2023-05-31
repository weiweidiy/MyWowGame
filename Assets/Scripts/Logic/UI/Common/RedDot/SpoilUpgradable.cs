using Networks;
using System.Collections.Generic;

namespace Logic.Common.RedDot
{
    public class SpoilUpgradable : RedDotLogic<SpoilData>
    {
        protected override IDataNotifier<SpoilData> GetDataNotifier()
        {
            return new SpoilDataNotifier();
        }

        //protected override RedDotInfo GetParentNodeRedDotInfo(List<RedDotInfo> lstCacheInfo)
        //{
        //    RedDotInfo info = new RedDotInfo();


        //    return info;
        //}

        protected override string GetUID(SpoilData data)
        {
            return data.SpoilId.ToString();
        }

        protected override RedDotStatus GetStatus(SpoilData data)
        {
            throw new System.NotImplementedException();
        }
    }
}