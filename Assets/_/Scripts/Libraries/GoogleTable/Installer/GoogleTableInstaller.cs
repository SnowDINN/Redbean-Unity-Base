﻿using Redbean.Base;
using UnityEngine;

namespace Redbean.Table
{
	[CreateAssetMenu(fileName = "GoogleTable", menuName = "Redbean/GoogleTable")]
	public class GoogleTableInstaller : ScriptableObject
	{
		[Header("Get table information during runtime")]
		public string Path;
		public string ItemPath;
	}
	
	public class GoogleTableSettings : SettingsBase<GoogleTableInstaller>
	{
		public static string Path
		{
			get => Installer.Path;
			set
			{
				Installer.Path = value;

#if UNITY_EDITOR
				Save();
#endif
			}
		}

		public static string ItemPath
		{
			get => Installer.ItemPath;
			set
			{
				Installer.ItemPath = value;
				
#if UNITY_EDITOR
				Save();
#endif
			}
		}
	}
}