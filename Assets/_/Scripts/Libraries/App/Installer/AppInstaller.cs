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

	public class ApplicationSettings : SettingsBase<AppInstaller>
	{
		public const string ApiUri = "https://localhost:44395";

		public static string Version =>
			string.IsNullOrEmpty(Installer.Version) ? Application.version : Installer.Version;
		
		public static MobileType MobileType
		{
			get
			{
#if UNITY_ANDROID
				return MobileType.Android;
#elif UNITY_IOS
				return MobileType.iOS;
#else
				return MobileType.None;
#endif
			}
		}
	}
}