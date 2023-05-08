using Logic.Config;

namespace Configs
{
    public partial class LevelCfg
    {
        public static LevelData GetLevelData(long pID)
        {
            LevelData _data = null;
            _data = ConfigManager.Ins.m_LevelCfg.AllData.TryGetValue(pID.ToString(), out _data) ? _data : null;
            if(_data == null)
                _data = ConfigManager.Ins.m_LevelCfg.AllData.TryGetValue("0", out _data) ? _data : null;
            return _data;
        }
    }
}