using Redbean.Base;
using UnityEngine;

namespace Redbean
{
	[CreateAssetMenu(fileName = "Application", menuName = "Redbean/Application")]
	public class ApplicationInstaller : ScriptableObject
	{
		[Header("Get application information during runtime")]
		public string Version;
	}

	public class ApplicationSettings : SettingsBase<ApplicationInstaller>
	{
		public static string Version =>
			string.IsNullOrEmpty(Installer.Version) ? Application.version : Installer.Version;
	}
}