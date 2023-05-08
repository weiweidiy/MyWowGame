using Cysharp.Threading.Tasks;
using Main.GamePatches.FsmManagers;
using UnityEngine;
using YooAsset;

namespace Main.GamePatches.PatchUpdater.FsmNode
{
    /// <summary>
    /// 启动游戏逻辑
    /// </summary>
    public class FsmAllDone : IFsmNode
    {
        public string Name { get; private set; } = nameof(FsmAllDone);
        public void OnEnter()
        {
            PatchEventDispatcher.SendAllDone();
            StartGame();
        }

        public void OnUpdate()
        {
            
        }

        public void OnExit()
        {
            
        }

        private async void StartGame()
        {
            var _Handle = YooAssets.LoadAssetAsync<GameObject>("GameCore");
            await _Handle.ToUniTask();
            _Handle.InstantiateAsync();
        }
    }
}