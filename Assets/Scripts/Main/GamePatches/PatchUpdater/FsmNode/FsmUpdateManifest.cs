using System.Collections;
using Main.GamePatches.FsmManagers;
using UnityEngine;
using YooAsset;

namespace Main.GamePatches.PatchUpdater.FsmNode
{
	/// <summary>
	/// 更新资源清单
	/// </summary>
	public class FsmUpdateManifest : IFsmNode
	{
		public string Name { private set; get; } = nameof(FsmUpdateManifest);

		void IFsmNode.OnEnter()
		{
			PatchEventDispatcher.SendPatchStepsChangeMsg(EPatchStates.UpdateManifest);
			GameMain.Ins.StartCoroutine(UpdateManifest());
		}
		void IFsmNode.OnUpdate()
		{
		}
		void IFsmNode.OnExit()
		{
		}

		private IEnumerator UpdateManifest()
		{
			yield return new WaitForSecondsRealtime(0.1f);

			// 更新补丁清单
			var package = YooAssets.GetPackage(GameMain.Ins.DefaultPackageName);
			var operation = package.UpdatePackageManifestAsync(PatchUpdater.PackageVersion);
			yield return operation;

			if(operation.Status == EOperationStatus.Succeed)
			{
				operation.SavePackageVersion();
				FsmManager.Transition(nameof(FsmCreateDownloader));
			}
			else
			{
				Debug.LogWarning(operation.Error);
				PatchEventDispatcher.SendPatchManifestUpdateFailedMsg();
			}
		}
	}
}