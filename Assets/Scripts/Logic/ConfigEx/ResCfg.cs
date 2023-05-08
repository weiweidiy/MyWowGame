using System.Collections.Generic;
using UnityEngine;

namespace Configs
{
    public class ResCfgEx
    {
        public static Dictionary<long, List<string>> m_ResGroups = new Dictionary<long, List<string>>(64);
        
        public static void Init(ResCfg pCfg)
        {
            foreach (var _Data in pCfg.AllData.Values)
            {
                if (m_ResGroups.TryGetValue(_Data.ResGroupId, out var _ResList))
                {
                    _ResList.Add(_Data.Res);
                }
                else
                {
                    _ResList = new List<string>(12) { _Data.Res };
                    m_ResGroups.Add(_Data.ResGroupId, _ResList);
                }
            }
        }
        
        public static List<string> GetResGroup(long pResGroupId)
        {
            if(m_ResGroups.TryGetValue(pResGroupId, out var _ResList))
                return _ResList;
            
            Debug.LogError("ResCfgEx.GetResGroup: Can't find ResGroup: " + pResGroupId);
            return null;
        }
    }
}