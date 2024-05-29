using UnityEngine;

namespace Redbean
{
	[CreateAssetMenu(fileName = "Application", menuName = "Redbean/Application")]
	public class ApplicationInstaller : ScriptableObject
	{
		public string Version;
	}

	public class ApplicationSettings
	{
		private static ApplicationInstaller installer => Resources.Load<ApplicationInstaller>("Settings/Application");

		public static string Version =>
			string.IsNullOrEmpty(installer.Version) ? Application.version : installer.Version;
	}
}