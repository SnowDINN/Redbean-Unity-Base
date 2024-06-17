using Redbean.Popup.Content;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Redbean
{
	public class AppLifeCycle : MonoBase
	{
		public static bool IsReady { get; private set; }

		private async void Awake()
		{
			await AppBootstrap.BootstrapInitialize();
			
			Application.logMessageReceived += OnLogMessageReceived;
			IsReady = true;
		}

		public override void OnDestroy()
		{
			Application.logMessageReceived -= OnLogMessageReceived;
			IsReady = false;

			AppBootstrap.BootstrapDispose();
			
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