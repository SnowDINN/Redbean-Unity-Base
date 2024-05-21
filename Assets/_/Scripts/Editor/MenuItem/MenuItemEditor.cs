using UnityEditor;
using UnityEngine;

namespace Redbean.Editor
{
	public class MenuItemEditor
	{
		[MenuItem("Quick Menu/Local Data/Delete All")]
		public static void PlayPrefsDeleteAll()
		{
			PlayerPrefs.DeleteAll();
			Log.Notice("All local data has been deleted.");
		}
	}
}