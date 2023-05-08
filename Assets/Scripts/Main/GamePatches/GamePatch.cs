using System;
using System.Collections;
using System.IO;
using Main.GamePatches.EventManagers;
using Main.GamePatches.FsmManagers;
using UnityEngine;
using YooAsset;

namespace Main.GamePatches
{
    /// <summary>
    /// 热更入口
    /// </summary>
    public class GamePatch : MonoBehaviour
    {
        public static EPlayMode GamePlayMode;

        private void OnDestroy()
        {
            PatchUpdater.PatchUpdater.IsRun = false;
        }

        void Update()
        {
            EventManager.Update();
            FsmManager.Update();
        }

        IEnumerator Start()
        {
            GamePlayMode = GameMain.Ins.PlayMode;
            Debug.Log($"资源系统运行模式：{GamePlayMode}");
            
            // 初始化资源系统
            YooAssets.Initialize();
            YooAssets.SetOperationSystemMaxTimeSlice(30);
            
            var defaultPackage = YooAssets.TryGetPackage(GameMain.Ins.DefaultPackageName);
            if (defaultPackage == null)
            {
                // 创建默认的资源包
                defaultPackage = YooAssets.CreatePackage(GameMain.Ins.DefaultPackageName);
                // 设置该资源包为默认的资源包
                YooAssets.SetDefaultPackage(defaultPackage);
            }
            
            InitializationOperation initializationOperation = null;
            // 编辑器下的模拟模式
            if (GamePlayMode == EPlayMode.EditorSimulateMode)
            {
                var createParameters = new EditorSimulateModeParameters
                {
                    SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(GameMain.Ins.DefaultPackageName)
                };
                initializationOperation = defaultPackage.InitializeAsync(createParameters);
            }

            // 单机运行模式
            if (GamePlayMode == EPlayMode.OfflinePlayMode)
            {
                var createParameters = new OfflinePlayModeParameters
                {
                    DecryptionServices = new GameDecryptionServices()
                };
                initializationOperation = defaultPackage.InitializeAsync(createParameters);
            }

            // 联机运行模式
            if (GamePlayMode == EPlayMode.HostPlayMode)
            {
                var createParameters = new HostPlayModeParameters
                {
                    DecryptionServices = new GameDecryptionServices(),
                    QueryServices = new GameQueryServices(),
                    DefaultHostServer = GetHostServerURL(GameMain.Ins.HostURL, GameMain.Ins.GameVersion),
                    FallbackHostServer = GetHostServerURL(GameMain.Ins.FallbackHostURL, GameMain.Ins.GameVersion)
                };
                initializationOperation = defaultPackage.InitializeAsync(createParameters);
            }

            yield return initializationOperation;
            
            //Check Init State
            if (defaultPackage.InitializeStatus == EOperationStatus.Succeed)
            {
                // 运行补丁流程
                PatchUpdater.PatchUpdater.Run();
            }
            else
            {
                //初始化失败了
                Debug.LogError($"Init Error : {initializationOperation.Error}");
            }
        }
        
        private string GetHostServerURL(string pHostURL, string pVersion)
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.Android)
                return $"{pHostURL}/CDN/Android/{pVersion}";
            else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.iOS)
                return $"{pHostURL}/CDN/IPhone/{pVersion}";
            else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.WebGL)
                return $"{pHostURL}/CDN/WebGL/{pVersion}";
            else
                return $"{pHostURL}/CDN/PC/{pVersion}";
#else
		if (Application.platform == RuntimePlatform.Android)
			return $"{pHostURL}/CDN/Android/{pVersion}";
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
			return $"{pHostURL}/CDN/IPhone/{pVersion}";
		else if (Application.platform == RuntimePlatform.WebGLPlayer)
			return $"{pHostURL}/CDN/WebGL/{pVersion}";
		else
			return $"{pHostURL}/CDN/PC/{pVersion}";
#endif
        }
        
        /// <summary>
        /// 资源文件解密服务类
        /// </summary>
        private class GameDecryptionServices : IDecryptionServices
        {
            public ulong LoadFromFileOffset(DecryptFileInfo fileInfo)
            {
                return 32;
            }

            public byte[] LoadFromMemory(DecryptFileInfo fileInfo)
            {
                throw new NotImplementedException();
            }

            public Stream LoadFromStream(DecryptFileInfo fileInfo)
            {
                BundleStream bundleStream = new BundleStream(fileInfo.FilePath, FileMode.Open);
                return bundleStream;
            }

            public uint GetManagedReadBufferSize()
            {
                return 1024;
            }
        }
        
        private class GameQueryServices : IQueryServices
        {
            public bool QueryStreamingAssets(string fileName)
            {
                string buildinFolderName = YooAssets.GetStreamingAssetBuildinFolderName();
                return StreamingAssetsHelper.FileExists($"{buildinFolderName}/{fileName}");
            }
        }
    }
}