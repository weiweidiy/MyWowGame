using Logic.Manager;
using Networks;
using System;
using System.Collections.Generic;

namespace Logic.Common.RedDot
{
    public class SpoilDataNotifier : IDataNotifier<SpoilData>
    {
        public event Action<SpoilData, object> onDataChanged;

        public List<SpoilData> GetDataList()
        {
            return SpoilManager.Ins.m_SpoilsData;
        }

        public void Init()
        {
            
        }
    }







}