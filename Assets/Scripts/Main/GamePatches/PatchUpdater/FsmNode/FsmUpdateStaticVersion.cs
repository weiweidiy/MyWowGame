using System.Collections;
using Main.GamePatches.FsmManagers;
using UnityEngine;
using YooAsset;

namespace Main.GamePatches.PatchUpdater.FsmNode
{
	/// <summary>
	/// 更新资源版本号
	/// </summary>
	internal class FsmUpdateStaticVersion : IFsmNode
	{
		public string Name { private set; get; } = nameof(FsmUpdateStaticVersion);

		void IFsmNode.OnEnter()
		{
			PatchEventDispatcher.SendPatchStepsChangeMsg(EPatchStates.UpdateStaticVersion);
			GameMain.Ins.StartCoroutine(GetStaticVersion());
		}
		void IFsmNode.OnUpdate()
		{
		}
		void IFsmNode.OnExit()
		{
		}

		private IEnumerator GetStaticVersion()
		{
			yield return new WaitForSecondsRealtime(0.1f);

			// 更新资源版本号
			var package = YooAssets.GetPackage(GameMain.Ins.DefaultPackageName);
			var operation = package.UpdatePackageVersionAsync();
			yield return operation;

			if (operation.Status == EOperationStatus.Succeed)
			{
				PatchUpdater.PackageVersion = operation.PackageVersion;
				FsmManager.Transition(nameof(FsmUpdateManifest));
			} 
			else
			{
				Debug.LogWarning(operation.Error);
				PatchEventDispatcher.SendStaticVersionUpdateFailedMsg();
			}
		}
	}
}