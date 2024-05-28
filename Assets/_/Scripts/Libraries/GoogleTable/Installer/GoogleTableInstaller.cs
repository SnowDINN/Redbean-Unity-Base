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
	
	public class GoogleTableDefine
	{
		public static string RequestPath(string name) => $"Table/{PlayerSettings.bundleVersion}/{name}.tsv";
	}
}