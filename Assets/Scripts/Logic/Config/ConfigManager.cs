using System;
using Configs;
using Cysharp.Threading.Tasks;
using Framework.Extension;
using LitJson;
using Logic.Common;
using Main;
using UnityEngine;
using YooAsset;

namespace Logic.Config
{
    /// <summary>
    /// 游戏配置管理
    /// </summary>
    public class ConfigManager : Singleton<ConfigManager>
    {
        #region 配置表对象实例

        public GameDefineCfg m_GameDefineCfg;
        public ResCfg m_ResCfg;
        public LevelResCfg m_LevelResCfg;
        public SkillCfg m_SkillCfg;
        public SkillLvlUpCfg m_SkillLvlUpCfg;
        public PartnerCfg m_PartnerCfg;
        public PartnerLvlUpCfg m_PartnerLvlUpCfg;
        public EquipCfg m_EquipCfg;
        public EquipLvlUpCfg m_EquipLvlUpCfg;
        public SummonCfg m_SummonCfg;
        public GroupCfg m_GroupCfg;
        public ItemCfg m_ItemCfg;
        public TaskCfg m_TaskCfg;
        public TaskTypeCfg m_TaskTypeCfg;
        public DigMapCfg m_DigMapCfg;
        public CopyCoinCfg m_CopyCoinCfg;
        public CopyDiamondCfg m_CopyDiamondCfg;
        public HandUpCfg m_HandUpCfg;
        public EngineCfg m_EngineCfg;
        public DigEngineCfg m_DigEngineCfg;
        public AttributeCfg m_AttributeCfg;
        public StoryCfg m_StoryCfg;
        public LockCfg m_LockCfg;
        public LevelCfg m_LevelCfg;
        public EngineLvlUpCfg m_EngineLvlUpCfg;
        public DigResearchCfg m_DigResearchCfg;

        #endregion

        /// <summary>
        /// 加载全部配置表
        /// </summary>
        public async UniTask LoadAllConfigs()
        {
            try
            {
                var _Load = YooAssets.LoadAssetAsync<TextAsset>("GameDefineCfg");
                await _Load.ToUniTask();
                var _Data = _Load.AssetObject as TextAsset;
                m_GameDefineCfg = JsonMapper.ToObject<GameDefineCfg>(_Data.text);
                GameDefine.Init();
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("ResCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_ResCfg = JsonMapper.ToObject<ResCfg>(_Data.text);
                ResCfgEx.Init(m_ResCfg);
                m_ResCfg = null; //这个表后面没用 这里直接释放掉 读取操作需要通过 ResCfgEx 获取
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("LevelResCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_LevelResCfg = JsonMapper.ToObject<LevelResCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("SkillCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_SkillCfg = JsonMapper.ToObject<SkillCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("SkillLvlUpCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_SkillLvlUpCfg = JsonMapper.ToObject<SkillLvlUpCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("PartnerCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_PartnerCfg = JsonMapper.ToObject<PartnerCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("PartnerLvlUpCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_PartnerLvlUpCfg = JsonMapper.ToObject<PartnerLvlUpCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("EquipLvlUpCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_EquipLvlUpCfg = JsonMapper.ToObject<EquipLvlUpCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("EquipCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_EquipCfg = JsonMapper.ToObject<EquipCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("SummonCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_SummonCfg = JsonMapper.ToObject<SummonCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("GroupCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_GroupCfg = JsonMapper.ToObject<GroupCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("ItemCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_ItemCfg = JsonMapper.ToObject<ItemCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("TaskCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_TaskCfg = JsonMapper.ToObject<TaskCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("TaskTypeCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_TaskTypeCfg = JsonMapper.ToObject<TaskTypeCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("CopyCoinCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_CopyCoinCfg = JsonMapper.ToObject<CopyCoinCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("CopyDiamondCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_CopyDiamondCfg = JsonMapper.ToObject<CopyDiamondCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("DigMapCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_DigMapCfg = JsonMapper.ToObject<DigMapCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("HandUpCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_HandUpCfg = JsonMapper.ToObject<HandUpCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("EngineCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_EngineCfg = JsonMapper.ToObject<EngineCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("DigEngineCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_DigEngineCfg = JsonMapper.ToObject<DigEngineCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("AttributeCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_AttributeCfg = JsonMapper.ToObject<AttributeCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("StoryCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_StoryCfg = JsonMapper.ToObject<StoryCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("LockCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_LockCfg = JsonMapper.ToObject<LockCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("LevelCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_LevelCfg = JsonMapper.ToObject<LevelCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("EngineLvlUpCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_EngineLvlUpCfg = JsonMapper.ToObject<EngineLvlUpCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("DigResearchCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_DigResearchCfg = JsonMapper.ToObject<DigResearchCfg>(_Data.text);
                _Load.Release();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            //卸载
            GameMain.Ins.UnloadUnusedAssets();
        }
    }
}