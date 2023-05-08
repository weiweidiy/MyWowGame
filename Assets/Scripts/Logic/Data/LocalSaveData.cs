using System;
using LitJson;
using UnityEngine;

namespace Logic.Data
{
    /// <summary>
    /// 客户端本地存档数据
    /// </summary>
    [Serializable]
    public class LocalSaveData
    {
        #region 静态操作接口
        
        public static string GetSaveKey()
        {
            return "LocalSaveData_KEY";
        }
        
        public static void Save(LocalSaveData pIns)
        {
            var _S = JsonMapper.ToJson(pIns);
            PlayerPrefs.SetString(GetSaveKey(), _S);
            PlayerPrefs.Save();
        }
        
        public static LocalSaveData Load()
        {
            if(PlayerPrefs.HasKey(GetSaveKey()))
            {
                var _S = PlayerPrefs.GetString(GetSaveKey());
                return JsonMapper.ToObject<LocalSaveData>(_S);
            }
            else
            {
                var _Data = new LocalSaveData();
                return _Data;
            }
        }

        public static void Clear()
        {
            PlayerPrefs.DeleteKey(GetSaveKey());
        }

        #endregion
    }
}