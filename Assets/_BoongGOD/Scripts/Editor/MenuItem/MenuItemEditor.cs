using Redbean.Extension;
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
			Log.Print("모든 로컬 데이터가 삭제되었습니다.", Color.yellow);
		}
	}
}