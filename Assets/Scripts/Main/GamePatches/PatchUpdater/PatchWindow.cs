using System;
using System.Collections.Generic;
using Main.GamePatches.EventManagers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main.GamePatches.PatchUpdater
{
	public class PatchWindow : MonoBehaviour
	{
		/// <summary>
		/// 对话框封装类
		/// </summary>
		private class MessageBox
		{
			private GameObject _cloneObject;
			private TextMeshProUGUI _content;
			private Button _btnOK;
			private Action _clickOK;

			public bool ActiveSelf => _cloneObject.activeSelf;

			public void Create(GameObject cloneObject)
			{
				_cloneObject = cloneObject;
				_content = cloneObject.transform.Find("Content").GetComponent<TextMeshProUGUI>();
				_btnOK = cloneObject.transform.Find("BtnOK").GetComponent<Button>();
				_btnOK.onClick.AddListener(OnClickYes);
			}
			public void Show(string content, Action clickOK)
			{
				_content.text = content;
				_clickOK = clickOK;
				_cloneObject.SetActive(true);
				_cloneObject.transform.SetAsLastSibling();
			}
			public void Hide()
			{
				_content.text = string.Empty;
				_clickOK = null;
				_cloneObject.SetActive(false);
			}
			private void OnClickYes()
			{
				_clickOK?.Invoke();
				Hide();
			}
		}


		private readonly EventGroup _eventGroup = new EventGroup();
		private readonly List<MessageBox> _msgBoxList = new List<MessageBox>();
		
		public GameObject _messageBoxObj;
		public Slider _slider;
		public TextMeshProUGUI _tips;
		
		void Awake()
		{
			_tips.text = "Updating ...";
			_messageBoxObj.SetActive(false);

			_eventGroup.AddListener<PatchEventMessageDefine.PatchStatesChange>(OnHandleEvent);
			_eventGroup.AddListener<PatchEventMessageDefine.FoundUpdateFiles>(OnHandleEvent);
			_eventGroup.AddListener<PatchEventMessageDefine.DownloadProgressUpdate>(OnHandleEvent);
			_eventGroup.AddListener<PatchEventMessageDefine.StaticVersionUpdateFailed>(OnHandleEvent);
			_eventGroup.AddListener<PatchEventMessageDefine.PatchManifestUpdateFailed>(OnHandleEvent);
			_eventGroup.AddListener<PatchEventMessageDefine.WebFileDownloadFailed>(OnHandleEvent);
			_eventGroup.AddListener<PatchEventMessageDefine.AllDone>(OnHandleEvent);
		}
		void OnDestroy()
		{
			_eventGroup.RemoveAllListener();
		}

		/// <summary>
		/// 接收事件
		/// </summary>
		private void OnHandleEvent(IEventMessage msg)
		{
			if (msg is PatchEventMessageDefine.PatchStatesChange)
			{
				var message = msg as PatchEventMessageDefine.PatchStatesChange;
				if (message.CurrentStates == EPatchStates.UpdateStaticVersion)
					_tips.text = "获取最新的资源版本...";
				else if (message.CurrentStates == EPatchStates.UpdateManifest)
					_tips.text = "更新资源清单...";
				else if (message.CurrentStates == EPatchStates.CreateDownloader)
					_tips.text = "创建补丁下载器...";
				else if (message.CurrentStates == EPatchStates.DownloadWebFiles)
					_tips.text = "下载补丁文件...";
				else if (message.CurrentStates == EPatchStates.PatchDone)
					_tips.text = "更新完成...";
				else
					throw new NotImplementedException(message.CurrentStates.ToString());
			}
			else if (msg is PatchEventMessageDefine.FoundUpdateFiles)
			{
				var message = msg as PatchEventMessageDefine.FoundUpdateFiles;
				Action callback = () =>
				{
					PatchUpdater.HandleOperation(EPatchOperation.BeginDownloadWebFiles);
				};
				float sizeMB = message.TotalSizeBytes / 1048576f;
				sizeMB = Mathf.Clamp(sizeMB, 0.1f, float.MaxValue);
				string totalSizeMB = sizeMB.ToString("f1");
				ShowMessageBox($"发现新的更新内容, 总数:{message.TotalCount} 总计:{totalSizeMB}MB", callback);
			}
			else if (msg is PatchEventMessageDefine.DownloadProgressUpdate)
			{
				var message = msg as PatchEventMessageDefine.DownloadProgressUpdate;
				_slider.value = (float)message.CurrentDownloadCount / message.TotalDownloadCount;
				string currentSizeMB = (message.CurrentDownloadSizeBytes / 1048576f).ToString("f1");
				string totalSizeMB = (message.TotalDownloadSizeBytes / 1048576f).ToString("f1");
				_tips.text = $"{message.CurrentDownloadCount}/{message.TotalDownloadCount} {currentSizeMB}MB/{totalSizeMB}MB";
			}
			else if (msg is PatchEventMessageDefine.StaticVersionUpdateFailed)
			{
				Action callback = () =>
				{
					PatchUpdater.HandleOperation(EPatchOperation.TryUpdateStaticVersion);
				};
				ShowMessageBox($"获取最新的资源版本失败 请检查网络", callback);
			}
			else if (msg is PatchEventMessageDefine.PatchManifestUpdateFailed)
			{
				Action callback = () =>
				{
					PatchUpdater.HandleOperation(EPatchOperation.TryUpdatePatchManifest);
				};
				ShowMessageBox($"更新资源清单 请检查网络", callback);
			}
			else if (msg is PatchEventMessageDefine.WebFileDownloadFailed)
			{
				var message = msg as PatchEventMessageDefine.WebFileDownloadFailed;
				Action callback = () =>
				{
					PatchUpdater.HandleOperation(EPatchOperation.TryDownloadWebFiles);
				};
				ShowMessageBox($"下载文件失败 : {message.FileName}", callback);
			}
			else if (msg is PatchEventMessageDefine.AllDone)
			{
				Destroy(gameObject, 0.25f);
			}
			else
			{
				throw new System.NotImplementedException($"{msg.GetType()}");
			}
		}

		/// <summary>
		/// 显示对话框
		/// </summary>
		private void ShowMessageBox(string content, Action ok)
		{
			// 尝试获取一个可用的对话框
			MessageBox msgBox = null;
			for (int i = 0; i < _msgBoxList.Count; i++)
			{
				var item = _msgBoxList[i];
				if (item.ActiveSelf == false)
				{
					msgBox = item;
					break;
				}
			}

			// 如果没有可用的对话框，则创建一个新的对话框
			if (msgBox == null)
			{
				msgBox = new MessageBox();
				var cloneObject = GameObject.Instantiate(_messageBoxObj, _messageBoxObj.transform.parent);
				msgBox.Create(cloneObject);
				_msgBoxList.Add(msgBox);
			}

			// 显示对话框
			msgBox.Show(content, ok);
		}
	}
}