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
		private static ApplicationInstaller installer;

		private static ApplicationInstaller Installer
		{
			get
			{
				if (!installer)
					installer = Resources.Load<ApplicationInstaller>("Settings/Application");

				return installer;
			}
		}

		public static string Version =>
			string.IsNullOrEmpty(Installer.Version) ? Application.version : Installer.Version;
	}
}