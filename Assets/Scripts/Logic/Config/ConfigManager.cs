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
        public AttributeCfg m_AttributeCfg;
        public StoryCfg m_StoryCfg;
        public LockCfg m_LockCfg;
        public LevelCfg m_LevelCfg;
        public DigResearchCfg m_DigResearchCfg;
        public CopyOilCfg m_CopyOilCfg;
        public CopyTrophyCfg m_CopyTrophyCfg;
        public DJCfg m_DJCfg;
        public DJComboCfg m_DJComboCfg;
        public DJDesCfg m_DJDesCfg;
        public SceneCfg m_SceneCfg;
        public SpoilUnlockCfg m_SpoilUnlockCfg;
        public SpoilCfg m_SpoilCfg;
        public SpoilLvlUpCfg m_SpoilLvlUpCfg;
        public HerosCfg m_HerosCfg;
        public HerosLvUpCfg m_HerosLvUpCfg;
        public HerosBreakUpCfg m_HerosBreakUpCfg;
        public RoomCfg m_RoomCfg;
        public SpoilBreakUpCfg m_SpoilBreakUpCfg;
        public HerosBreakCfg m_HerosBreakCfg;
        public ShopCfg m_ShopCfg;
        public ShopDiamondCfg m_ShopDiamondCfg;
        public CopyReformCfg m_CopyReformCfg;
        public ReformHeroSlotCfg m_ReformHeroSlotCfg;
        public CylinderCfg m_CylinderCfg;
        public ReformFightAttributeCfg m_ReformFightAttributeCfg;
        public ReformCylinderGroupCfg m_ReformCylinderGroupCfg;
        public ReformRewardCfg m_ReformRewardCfg;
        public AttributeRandomCfg m_AttributeRandomCfg;
        public DigSparkCfg m_DigSparkCfg;
        public SparkCfg m_SparkCfg;
        public SparkGroupCfg m_SparkGroupCfg;
        public MonsterCfg m_MonsterCfg;
        public FormationGroupCfg m_FormationGroupCfg;
        public FormationCfg m_FormationCfg;

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
                //m_ResCfg = null;  //这个表后面没用 这里直接释放掉 读取操作需要通过 ResCfgEx 获取 
                //0517：暂时不释放，需要查询单个数据
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

                _Load = YooAssets.LoadAssetAsync<TextAsset>("DigResearchCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_DigResearchCfg = JsonMapper.ToObject<DigResearchCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("CopyOilCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_CopyOilCfg = JsonMapper.ToObject<CopyOilCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("CopyTrophyCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_CopyTrophyCfg = JsonMapper.ToObject<CopyTrophyCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("DJCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_DJCfg = JsonMapper.ToObject<DJCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("DJComboCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_DJComboCfg = JsonMapper.ToObject<DJComboCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("DJDesCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_DJDesCfg = JsonMapper.ToObject<DJDesCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("SceneCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_SceneCfg = JsonMapper.ToObject<SceneCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("SpoilUnlockCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_SpoilUnlockCfg = JsonMapper.ToObject<SpoilUnlockCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("SpoilCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_SpoilCfg = JsonMapper.ToObject<SpoilCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("SpoilLvlUpCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_SpoilLvlUpCfg = JsonMapper.ToObject<SpoilLvlUpCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("HerosCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_HerosCfg = JsonMapper.ToObject<HerosCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("HerosLvUpCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_HerosLvUpCfg = JsonMapper.ToObject<HerosLvUpCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("HerosBreakUpCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_HerosBreakUpCfg = JsonMapper.ToObject<HerosBreakUpCfg>(_Data.text);
                _Load.Release();


                _Load = YooAssets.LoadAssetAsync<TextAsset>("RoomCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_RoomCfg = JsonMapper.ToObject<RoomCfg>(_Data.text);
                _Load.Release();


                _Load = YooAssets.LoadAssetAsync<TextAsset>("SpoilBreakUpCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_SpoilBreakUpCfg = JsonMapper.ToObject<SpoilBreakUpCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("HerosBreakCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_HerosBreakCfg = JsonMapper.ToObject<HerosBreakCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("ShopCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_ShopCfg = JsonMapper.ToObject<ShopCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("ShopDiamondCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_ShopDiamondCfg = JsonMapper.ToObject<ShopDiamondCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("CopyReformCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_CopyReformCfg = JsonMapper.ToObject<CopyReformCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("ReformHeroSlotCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_ReformHeroSlotCfg = JsonMapper.ToObject<ReformHeroSlotCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("CylinderCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_CylinderCfg = JsonMapper.ToObject<CylinderCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("ReformFightAttributeCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_ReformFightAttributeCfg = JsonMapper.ToObject<ReformFightAttributeCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("ReformCylinderGroupCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_ReformCylinderGroupCfg = JsonMapper.ToObject<ReformCylinderGroupCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("ReformRewardCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_ReformRewardCfg = JsonMapper.ToObject<ReformRewardCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("AttributeRandomCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_AttributeRandomCfg = JsonMapper.ToObject<AttributeRandomCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("DigSparkCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_DigSparkCfg = JsonMapper.ToObject<DigSparkCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("SparkCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_SparkCfg = JsonMapper.ToObject<SparkCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("SparkGroupCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_SparkGroupCfg = JsonMapper.ToObject<SparkGroupCfg>(_Data.text);
                _Load.Release();


                _Load = YooAssets.LoadAssetAsync<TextAsset>("MonsterCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_MonsterCfg = JsonMapper.ToObject<MonsterCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("FormationGroupCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_FormationGroupCfg = JsonMapper.ToObject<FormationGroupCfg>(_Data.text);
                _Load.Release();

                _Load = YooAssets.LoadAssetAsync<TextAsset>("FormationCfg");
                await _Load.ToUniTask();
                _Data = _Load.AssetObject as TextAsset;
                m_FormationCfg = JsonMapper.ToObject<FormationCfg>(_Data.text);
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