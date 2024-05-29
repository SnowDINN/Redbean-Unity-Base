using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Redbean
{
	[CreateAssetMenu(fileName = "GoogleTable", menuName = "Redbean/GoogleTable")]
	public class GoogleTableInstaller : ScriptableObject
	{
		public string Path;
		public string ItemPath;
		
#if UNITY_EDITOR
		public void Save()
		{
			EditorUtility.SetDirty(this);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
#endif
	}
	
	public class GoogleTableSettings
	{
		private static GoogleTableInstaller installer;

		private static GoogleTableInstaller Installer
		{
			get
			{
				if (!installer)
					installer = Resources.Load<GoogleTableInstaller>("Settings/GoogleTable");

				return installer;
			}
		}

		public static string Path
		{
			get => Installer.Path;
			set => Installer.Path = value;
		}

		public static string ItemPath
		{
			get => Installer.ItemPath;
			set => Installer.ItemPath = value;
		}
		
		public static string RequestPath(string name) => $"Table/{ApplicationSettings.Version}/{name}.tsv";
		
#if UNITY_EDITOR
		public static void Save() => Installer.Save();
#endif
	}
}