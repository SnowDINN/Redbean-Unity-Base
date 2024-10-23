using Redbean.MVP;
using Redbean.Utility;

namespace Redbean
{
	public static partial class Extension
	{
		/// <summary>
		/// 로컬 데이터 저장
		/// </summary>
		public static float SetPlayerPrefs(this float value, string key) =>
			Database.Save(key, value);
		
		/// <summary>
		/// 로컬 데이터 저장
		/// </summary>
		public static int SetPlayerPrefs(this int value, string key) =>
			Database.Save(key, value);
		
		/// <summary>
		/// 로컬 데이터 저장
		/// </summary>
		public static string SetPlayerPrefs(this string value, string key) =>
			Database.Save(key, value);

		/// <summary>
		/// 로컬 데이터 저장
		/// </summary>
		public static T SetPlayerPrefs<T>(this T value, string key) where T : IModel => 
			Database.Save(key, value);
		
		/// <summary>
		/// 로컬 데이터 호출
		/// </summary>
		public static T GetPlayerPrefs<T>(this IExtension extension, string key) =>
			Database.Load<T>(key);
	}
}