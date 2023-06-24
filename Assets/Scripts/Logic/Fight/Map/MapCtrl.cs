using Configs;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Framework.Extension;
using Logic.Common;
using Logic.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using YooAsset;

namespace Logic.Fight.GJJ
{
    public class MapCtrl : MonoWithEvent
    {
        enum LayerType
        {
            Sky = 1,
            Far,
            Middle1,
            Middle2,
            Front

        }

        enum SceneType
        {
            Normal = 1,
            Boss
        }


        [SerializeField] ScrollUVController[] scrollUVController;

        ScrollUVController m_SkyController;
        ScrollUVController m_FarController;
        ScrollUVController m_Middle1Controller;
        ScrollUVController m_Middle2Controller;
        ScrollUVController m_FrontController;

        [Tooltip("GJJ向前移动的时间")]
        public float speedUp;
        [Tooltip("GJJ向后移动的时间")]
        public float speedDown;

        /// <summary>
        /// 资源handle，销毁时要release
        /// </summary>
        Dictionary<string, AssetOperationHandle> m_assetHandle = new Dictionary<string, AssetOperationHandle>();

        private void Awake()
        {
            m_SkyController = scrollUVController[0];
            m_FarController = scrollUVController[1];
            m_Middle1Controller = scrollUVController[2];
            m_Middle2Controller = scrollUVController[3];
            m_FrontController = scrollUVController[4];

            m_EventGroup.Register(LogicEvent.Fight_MapChanged, (i, o) =>
            {
                LevelType levelType = (LevelType)o;
                //Debug.LogError("场景切换了 " + levelType.ToString() + " node:" + GameDataManager.Ins.CurLevelNode);
                switch (levelType)
                {
                    case LevelType.NormalLevel:
                        OnChangeToNormalLevel();
                        break;
                    case LevelType.DiamondCopy:
                        OnChangeToDiamondCopy();
                        break;
                    case LevelType.CoinCopy:
                        OnChangeToCoinCopy();
                        break;
                    case LevelType.OilCopy:
                        OnChangeToOilCopy();
                        break;
                    case LevelType.TrophyCopy:
                        OnChangeToTrophyCopy();
                        break;
                }
                
            });

            m_EventGroup.Register(LogicEvent.Fight_MapMove, ((_, _) => ReStart()))
                    .Register(LogicEvent.Fight_MapStop, ((_, _) => Stop()))
                    .Register(LogicEvent.Fight_MapMoveBack,((_, _) => MoveBack()));
        }


        private void Start()
        {
            //默认是普通副本
            OnChangeToNormalLevel();
        }

        #region 切换地图响应方法
        /// <summary>
        /// 切换到普通副本
        /// </summary>
        async void OnChangeToNormalLevel()
        {
            var levelCfg = LevelCfg.GetLevelData(GameDataManager.Ins.CurLevelID);
            //对应场景组id
            var sceneGroup = levelCfg.MonsterScenes;
            //获取到场景组列表
            var sceneDataList = SceneCfg.GetSceneDataList((data) => data.ScenesGroup.Equals(sceneGroup));
            //循环节点和boss节点数据不同
            List<SceneData> layersData = GameDataManager.Ins.CurLevelNode == 5 ? 
                                                GetSceneDataBySceneType(sceneDataList, SceneType.Boss) :
                                                GetSceneDataBySceneType(sceneDataList, SceneType.Normal);

            await LoadMapLayers(layersData);
        }

        /// <summary>
        ///  切换到钻石副本
        /// </summary>
        async void OnChangeToDiamondCopy()
        {
            var data = CopyDiamondCfg.GetData(0);
            var sceneGroup = data.BossScenes;
            var sceneDataList = SceneCfg.GetSceneDataList((data) => data.ScenesGroup.Equals(sceneGroup));
            List<SceneData> layersData = GetSceneDataBySceneType(sceneDataList, SceneType.Boss);
            await LoadMapLayers(layersData);
        }

        async void OnChangeToCoinCopy()
        {
            var data = CopyCoinCfg.GetData(0);
            var sceneGroup = data.BossScenes;
            await LoadMapLayers(sceneGroup);
        }

        async void OnChangeToOilCopy()
        {
            var data = CopyOilCfg.GetData(0);
            var sceneGroup = data.BossScenes;
            await LoadMapLayers(sceneGroup);
        }

        async void OnChangeToTrophyCopy()
        {
            var data = CopyTrophyCfg.GetData(0);
            var sceneGroup = data.BossScenes;
            await LoadMapLayers(sceneGroup);
        }
        #endregion

        #region 加载地图方法
        async Task LoadMapLayers(int sceneGroup)
        {
            var sceneDataList = SceneCfg.GetSceneDataList((data) => data.ScenesGroup.Equals(sceneGroup));
            List<SceneData> layersData = GetSceneDataBySceneType(sceneDataList, SceneType.Boss);
            await LoadMapLayers(layersData);
        }

        /// <summary>
        /// 加载场景图层
        /// </summary>
        /// <param name="layersData"></param>
        /// <returns></returns>
        async Task LoadMapLayers(List<SceneData> layersData)
        {
            //设置天空层级图片
            SceneData data = null;
            Texture2D tex = null;

            data = GetDataByLayerType(layersData, LayerType.Sky);
            tex = await LoadTexture(data);
            m_SkyController.SetTexture(tex);
            m_SkyController.ResetUVX();

            data = GetDataByLayerType(layersData, LayerType.Far);
            tex = await LoadTexture(data);
            m_FarController.SetTexture(tex);
            m_FarController.ResetUVX();

            data = GetDataByLayerType(layersData, LayerType.Middle1);
            tex = await LoadTexture(data);
            m_Middle1Controller.SetTexture(tex);
            m_Middle1Controller.ResetUVX();

            data = GetDataByLayerType(layersData, LayerType.Middle2);
            tex = await LoadTexture(data);
            m_Middle2Controller.SetTexture(tex);
            m_Middle2Controller.ResetUVX();

            data = GetDataByLayerType(layersData, LayerType.Front);
            tex = await LoadTexture(data);
            m_FrontController.SetTexture(tex);
            m_FrontController.ResetUVX();
        }

        /// <summary>
        /// 加载Texture
        /// </summary>
        /// <param name="resPath"></param>
        /// <returns></returns>
        private async UniTask<Texture2D> LoadTexture(string resPath)
        {
            AssetOperationHandle _assetHandle = null;
            if (m_assetHandle.ContainsKey(resPath))
            {
                _assetHandle = m_assetHandle[resPath];
            }
            else
            {
                _assetHandle = YooAssets.LoadAssetAsync<Texture2D>(resPath);
                await _assetHandle.ToUniTask();
            }

            var tex = _assetHandle.AssetObject as Texture2D;
            return tex;
        }

        private async UniTask<Texture2D> LoadTexture(SceneData data)
        {
            var resId = data.ScenesRes;
            var resPath = ResCfg.GetData(resId).Res;
            return await LoadTexture(resPath);
        }
        #endregion

        #region 地图移动和回退响应方法

        List<Tween> tweens = new List<Tween>();

        /// <summary>
        /// 启动
        /// </summary>
        void ReStart()
        {
            foreach(var ctr in scrollUVController)
            {
                ctr.SetSpeed(0);
                var tween = DOTween.To(() => ctr.GetSpeed(), pValue => ctr.SetSpeed(pValue), ctr.targetSpeed, speedUp).SetUpdate(UpdateType.Manual);
                tweens.Add(tween);
            }
           
        }

      

        /// <summary>
        /// 回退
        /// </summary>
        private void MoveBack()
        {
            
            foreach (var ctr in scrollUVController)
            {
                var tween = DOTween.To(() => ctr.GetSpeed(), pValue => ctr.SetSpeed(pValue), 0, speedDown).SetUpdate(UpdateType.Manual);
                tweens.Add(tween);
            }

        }

        /// <summary>
        /// 停止
        /// </summary>
        void Stop()
        {
            foreach(var tween in tweens)
            {
                tween?.Pause();
            }
            tweens.Clear();

            foreach (var ctr in scrollUVController)
            {
                ctr.SetSpeed(0);
            }
        }
        #endregion

        /// <summary>
        /// 获取场景类型
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="sceneType"></param>
        /// <returns></returns>
        List<SceneData> GetSceneDataBySceneType(List<SceneData> lst, SceneType sceneType)
        {
            return lst.Where(p => p.ScenesType.Equals((int)sceneType)).ToList();
        }

        /// <summary>
        /// 获取层数据
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        SceneData GetDataByLayerType(List<SceneData> lst , LayerType layer)
        {
            return lst.Where(p => p.LayersType.Equals((int)layer)).SingleOrDefault();
        }


        protected override void OnDestroy()
        {
            base.OnDestroy();

            foreach (var key in m_assetHandle.Keys)
            {
                m_assetHandle[key].Release();
            }
        }
    }
}