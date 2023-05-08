using Main.GamePatches.FsmManagers;
using UnityEngine;

namespace Main.GamePatches.PatchUpdater.FsmNode
{
	/// <summary>
	/// 清理没用的缓存
	/// </summary>
	internal class FsmClearCache : IFsmNode
	{
		public string Name { private set; get; } = nameof(FsmClearCache);

		void IFsmNode.OnEnter()
		{
			Debug.Log("清理未使用的缓存文件！");
			var package = YooAsset.YooAssets.GetPackage(GameMain.Ins.DefaultPackageName);
			var operation = package.ClearUnusedCacheFilesAsync();
			operation.Completed += Operation_Completed;
		}

		private void Operation_Completed(YooAsset.AsyncOperationBase obj)
		{
			FsmManager.Transition(nameof(FsmLoadDll));
		}

		void IFsmNode.OnUpdate()
		{
		}
		void IFsmNode.OnExit()
		{
		}
	}
}