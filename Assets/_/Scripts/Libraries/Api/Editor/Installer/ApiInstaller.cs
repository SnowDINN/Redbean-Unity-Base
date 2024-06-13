using Redbean.Base;
using UnityEngine;

namespace Redbean.Api
{
	[CreateAssetMenu(fileName = "Api", menuName = "Redbean/Api")]
	public class ApiInstaller : ScriptableObject
	{
		[Header("Get generation path")]
		public string ProtocolPath;
	}
	
	public class ApiSettings : SettingsBase<ApiInstaller>
	{
		public const string Uri = "https://localhost:44395";
		
		public static string ProtocolPath
		{
			get => Installer.ProtocolPath;
			set
			{
				Installer.ProtocolPath = value;
				Save();
			}
		}
	}
}