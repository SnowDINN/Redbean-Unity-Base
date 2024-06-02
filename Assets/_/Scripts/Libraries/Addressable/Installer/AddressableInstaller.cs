using Redbean.Base;
using UnityEngine;

namespace Redbean
{
	[CreateAssetMenu(fileName = "Addressable", menuName = "Redbean/Addressable")]
	public class AddressableInstaller : ScriptableObject
	{
		[Header("Get addressable information during runtime")]
		public string[] Labels;
	}

	public class AddressableSettings : SettingsBase<AddressableInstaller>
	{
		public static string[] Labels
		{
			get => Installer.Labels;
			set
			{
				Installer.Labels = value;
				Save();
			}
		}
	}
}