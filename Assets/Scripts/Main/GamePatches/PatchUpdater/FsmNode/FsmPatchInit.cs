using System.Collections;
using Main.GamePatches.FsmManagers;
using UnityEngine;

namespace Main.GamePatches.PatchUpdater.FsmNode
{
	internal class FsmPatchInit : IFsmNode
	{
		public string Name { private set; get; } = nameof(FsmPatchInit);

		void IFsmNode.OnEnter()
		{
			// 加载更新面板
			var go = Resources.Load<GameObject>("PatchWindow");
			var root = GameObject.Find("UIRoot/Top");
			Object.Instantiate(go,root.transform);

			GameMain.Ins.StartCoroutine(Begin());
		}
		void IFsmNode.OnUpdate()
		{
		}
		void IFsmNode.OnExit()
		{
		}

		private IEnumerator Begin()
		{
			yield return new WaitForSecondsRealtime(0.1f);

			FsmManager.Transition(nameof(FsmUpdateStaticVersion));
		}
	}
}