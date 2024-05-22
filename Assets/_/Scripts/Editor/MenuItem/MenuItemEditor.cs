using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Redbean.Editor
{
	public class MenuItemEditor
	{
		[MenuItem("Redbean Menu/Windows/Config Window", false, 0)]
		public static void OpenConfigWindow()
		{
			OdinEditorWindow.GetWindow<ConfigWindow>().Show();
		}
		
		[MenuItem("Redbean Menu/Windows/ThirdParty Window", false, 1)]
		public static void OpenThirdPartyWindow()
		{
			OdinEditorWindow.GetWindow<ThirdPartyWindow>().Show();
		}
		
		[MenuItem("Redbean Menu/Windows/Runtime Window", false, 2)]
		public static void OpenRuntimeWindow()
		{
			OdinEditorWindow.GetWindow<RuntimeWindow>().Show();
		}
		
		[MenuItem("Redbean Menu/Play Prefs/Delete All", false, 100)]
		public static void PlayPrefsDeleteAll()
		{
			PlayerPrefs.DeleteAll();
			Log.Notice("All local data has been deleted.");
		}
	}
}