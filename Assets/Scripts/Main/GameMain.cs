using System;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;

namespace Main
{
    /*
     * 游戏入口 非热更 无法更新
     */
    public class GameMain : MonoBehaviour
    {
        public static GameMain Ins;
        private void Awake()
        {
            if (Ins != null)
            {
                Destroy(gameObject);
                return;
            }
            
            Application.targetFrameRate = 60;
            Application.runInBackground = true;
            
            UniTaskScheduler.UnobservedTaskException += OnUniTaskException;
            
            Ins = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            Ins = null;
            UniTaskScheduler.UnobservedTaskException -= OnUniTaskException;
        }

        private void Start()
        {
            //进入Login场景 开启热更新流程
            SceneManager.LoadScene("Scenes/Login");
        }
        
        /*
         * 在Hierarchy里面配置的内容
         */

        /// <summary>
        /// 出包版本号
        /// </summary>
        [LabelText("包体静态版本")]
        public string GameVersion = "v0.1";
        
        /// <summary>
        /// 热更新模式
        /// </summary>
        [LabelText("热更模式")]
        public EPlayMode PlayMode = EPlayMode.EditorSimulateMode; 
        
        /// <summary>
        /// 热更新地址
        /// </summary>
        [LabelText("热更URL")]
        public string HostURL = @"http://127.0.0.1";
        
        /// <summary>
        /// 热更新地址(备用)
        /// </summary>
        [LabelText("备用热更URL")]
        public string FallbackHostURL = @"http://127.0.0.1";

        /// <summary>
        /// YA 默认的热更资源包名
        /// </summary>
        [LabelText("默认资源包名")]
        public string DefaultPackageName = "MainPackage";

        /// <summary>
        /// 卸载无用资源
        /// </summary>
        public void UnloadUnusedAssets()
        {
            YooAssets.GetPackage(DefaultPackageName).UnloadUnusedAssets();
        }
        
        private void OnUniTaskException(Exception obj)
        {
            Debug.LogError("UniTaskException : " + obj);
        }
    }
}