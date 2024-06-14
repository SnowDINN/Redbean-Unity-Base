using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Redbean.Base
{
	public class SettingsBase<T> where T : Object
	{
		private static readonly string resourceLocation = $"Settings/{typeof(T).Name.Replace("Installer", "")}";
		
		private static T installer;
		protected static T Installer
		{
			get
			{
				if (!installer)
					installer = Resources.Load<T>(resourceLocation);

				return installer;
			}
		}
		
#if UNITY_EDITOR
		public static void Save()
		{
			EditorUtility.SetDirty(Installer);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
#endif
	}
}