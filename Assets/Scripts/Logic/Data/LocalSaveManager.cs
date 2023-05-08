using Framework.Extension;

namespace Logic.Data
{
    /// <summary>
    /// `本地存档管理器`
    /// </summary>
    public class LocalSaveManager : Singleton<LocalSaveManager>
    {
        private LocalSaveData m_LocalData;
        public void Load()
        {
            m_LocalData = LocalSaveData.Load();
        }
        
        public void Save()
        {
            LocalSaveData.Save(m_LocalData);
        }
        
        //数据接口
        public LocalSaveData LocalData => m_LocalData;
    }
}