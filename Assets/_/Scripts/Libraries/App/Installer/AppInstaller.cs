using Redbean.Api;
using Redbean.Base;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Redbean
{
	[CreateAssetMenu(fileName = "Application", menuName = "Redbean/Application")]
	public class AppInstaller : ScriptableObject
	{
		[Header("Get application information during runtime")]
		public string Version;
	}

	public class AppSettings : SettingsBase<AppInstaller>
	{
		public const string ApiUri = "https://localhost:44395";

		public static string Version =>
			string.IsNullOrEmpty(Installer.Version) ? Application.version : Installer.Version;
		
		public static int PlatformType
		{
			get
			{
#if UNITY_ANDROID
				return (int)MobileType.Android;
#elif UNITY_IOS
				return (int)MobileType.iOS;
#else
				return (int)MobileType.None;
#endif
			}
		}
	}
}