using Main.GamePatches.FsmManagers;
using UnityEngine;

namespace Main.GamePatches.PatchUpdater.FsmNode
{
	/// <summary>
	/// 在这补丁完成
	/// </summary>
	internal class FsmPatchDone : IFsmNode
	{
		public string Name { private set; get; } = nameof(FsmPatchDone);

		void IFsmNode.OnEnter()
		{
			PatchEventDispatcher.SendPatchStepsChangeMsg(EPatchStates.PatchDone);
			Debug.Log("补丁流程更新完毕！");

			FsmManager.Transition(nameof(FsmClearCache));
		}
		void IFsmNode.OnUpdate()
		{
		}
		void IFsmNode.OnExit()
		{
		}
	}
}