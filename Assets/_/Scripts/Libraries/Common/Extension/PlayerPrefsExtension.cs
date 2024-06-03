using System;
using Redbean.Container;
using Redbean.MVP;

namespace Redbean
{
	public static partial class Extension
	{
		/// <summary>
		/// 로컬 데이터 저장
		/// </summary>
		public static float SetPlayerPrefs(this float value, string key) =>
			GetSingleton<MvpSingleton>().Save(key, value);
		
		/// <summary>
		/// 로컬 데이터 저장
		/// </summary>
		public static int SetPlayerPrefs(this int value, string key) =>
			GetSingleton<MvpSingleton>().Save(key, Convert.ToInt32(value));
		
		/// <summary>
		/// 로컬 데이터 저장
		/// </summary>
		public static string SetPlayerPrefs(this string value, string key) =>
			GetSingleton<MvpSingleton>().Save(key, value);

		/// <summary>
		/// 로컬 데이터 저장
		/// </summary>
		public static T SetPlayerPrefs<T>(this T value, string key) where T : IModel => 
			GetSingleton<MvpSingleton>().Save(key, value);
		
		/// <summary>
		/// 로컬 데이터 호출
		/// </summary>
		public static T GetPlayerPrefs<T>(this IMVP mvp, string key) =>
			GetSingleton<MvpSingleton>().Load<T>(key);
	}
}