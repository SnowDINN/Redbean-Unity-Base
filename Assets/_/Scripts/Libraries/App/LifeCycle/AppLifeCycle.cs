using System.Collections.Generic;
using Redbean.Popup.Content;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Redbean
{
	public class AppLifeCycle : MonoBase
	{
		private List<IAppBootstrap> instances = new();

		public static bool IsReady { get; private set; }
		
		public void Bootstrap(List<IAppBootstrap> instances)
		{
			Application.logMessageReceived += OnLogMessageReceived;
			
			this.instances = instances;
			this.instances.Reverse();

			IsReady = true;
		}

		public override void OnDestroy()
		{
			Application.logMessageReceived -= OnLogMessageReceived;
			
			foreach (var instance in instances)
				instance.Dispose();

			IsReady = false;
			
#if UNITY_EDITOR
			if (EditorApplication.isPlaying)
				EditorApplication.isPlaying = false;
#endif
		}
		
		private void OnLogMessageReceived(string condition, string stacktrace, LogType type)
		{
			if (type != LogType.Exception)
				return;

			this.Popup().Open<PopupException>().ExceptionMessage.Value = condition;
		}
	}
}