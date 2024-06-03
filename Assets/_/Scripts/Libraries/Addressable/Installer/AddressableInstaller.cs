using System;
using Redbean.Base;
using UnityEngine;

namespace Redbean.Bundle
{
	[CreateAssetMenu(fileName = "Addressable", menuName = "Redbean/Addressable")]
	public class AddressableInstaller : ScriptableObject
	{
		[Header("Get addressable information during runtime")]
		public string[] Labels;
	}

	public class AddressableSettings : SettingsBase<AddressableInstaller>
	{
		public static string GetPopupPath(Type type) => $"Popup/{type.Name}.prefab";
		
		public static string[] Labels
		{
			get => Installer.Labels;
			set
			{
				Installer.Labels = value;
				
#if UNITY_EDITOR
				Save();
#endif
			}
		}
	}
}