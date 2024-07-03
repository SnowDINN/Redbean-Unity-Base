using Redbean.Popup.Content;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Redbean
{
	public class AppLifeCycle : MonoBase
	{
		public static bool IsAppChecked { get; private set; }
		public static bool IsAppReady { get; private set; }
		public static string ExceptionMessage { get; private set; }

		private async void Awake()
		{
			await AppBootstrap.BootstrapInitialize();
			
			Application.logMessageReceived += OnLogMessageReceived;
			IsAppReady = true;
		}

		public override void OnDestroy()
		{
			Application.logMessageReceived -= OnLogMessageReceived;
			IsAppReady = false;

			AppBootstrap.BootstrapDispose();
			
#if UNITY_EDITOR
			if (EditorApplication.isPlaying)
				EditorApplication.isPlaying = false;
#endif
		}

		public static void AppCheckSuccess()
		{
			IsAppChecked = true;
		}
		
		public static void AppCheckFail()
		{
			IsAppChecked = false;
		}
		
		private void OnLogMessageReceived(string condition, string stacktrace, LogType type)
		{
			if (type != LogType.Exception)
				return;

			ExceptionMessage = condition;
			
			this.Popup().AssetOpen<PopupException>();
		}
	}
}