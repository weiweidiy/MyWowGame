using System;
using Main.GamePatches.FsmManagers;
using Main.GamePatches.PatchUpdater.FsmNode;
using UnityEngine;
using YooAsset;

namespace Main.GamePatches.PatchUpdater
{
	public static class PatchUpdater
	{
		public static bool IsRun;

		/// <summary>
		/// 下载器
		/// </summary>
		public static ResourceDownloaderOperation Downloader { set; get; }

		/// <summary>
		/// 包裹的版本信息
		/// </summary>
		public static string PackageVersion { set; get; }

		/// <summary>
		/// 开启初始化流程
		/// </summary>
		public static void Run()
		{
			if (IsRun == false)
			{
				IsRun = true;

				Debug.Log("开始补丁更新...");

				// 注意：按照先后顺序添加流程节点
				FsmManager.AddNode(new FsmPatchInit());
				FsmManager.AddNode(new FsmUpdateStaticVersion());
				FsmManager.AddNode(new FsmUpdateManifest());
				FsmManager.AddNode(new FsmCreateDownloader());
				FsmManager.AddNode(new FsmDownloadWebFiles());
				FsmManager.AddNode(new FsmPatchDone());
				FsmManager.AddNode(new FsmClearCache());
				FsmManager.AddNode(new FsmLoadDll());
				FsmManager.AddNode(new FsmAllDone());
				FsmManager.Run(nameof(FsmPatchInit));
			}
			else
			{
				Debug.LogWarning("补丁更新已经正在进行中!");
			}
		}

		/// <summary>
		/// 处理请求操作
		/// </summary>
		public static void HandleOperation(EPatchOperation operation)
		{
			if (operation == EPatchOperation.BeginDownloadWebFiles)
			{
				FsmManager.Transition(nameof(FsmDownloadWebFiles));
			}
			else if(operation == EPatchOperation.TryUpdateStaticVersion)
			{
				FsmManager.Transition(nameof(FsmUpdateStaticVersion));
			}
			else if (operation == EPatchOperation.TryUpdatePatchManifest)
			{
				FsmManager.Transition(nameof(FsmUpdateManifest));
			}
			else if (operation == EPatchOperation.TryDownloadWebFiles)
			{
				FsmManager.Transition(nameof(FsmCreateDownloader));
			}
			else
			{
				throw new NotImplementedException($"{operation}");
			}
		}
	}
}