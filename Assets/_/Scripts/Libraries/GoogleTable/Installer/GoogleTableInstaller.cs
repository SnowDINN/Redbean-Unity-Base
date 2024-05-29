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
		private static ApplicationInstaller installer => Resources.Load<ApplicationInstaller>("Settings/Application");
		public static string RequestPath(string name) => $"Table/{installer.Version}/{name}.tsv";
	}
}