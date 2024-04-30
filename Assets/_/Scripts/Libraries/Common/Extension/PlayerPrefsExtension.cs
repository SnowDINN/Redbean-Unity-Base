using System;
using Redbean.Core;
using Redbean.MVP;
using Redbean.Rx;

namespace Redbean
{
	public static partial class Extension
	{
		/// <summary>
		/// 로컬 데이터 저장
		/// </summary>
		public static float SetPlayerPrefs(this float value, string key) =>
			Singleton.GetOrAdd<RxPlayerPrefsBinder>().Save(key, value);
		
		/// <summary>
		/// 로컬 데이터 저장
		/// </summary>
		public static int SetPlayerPrefs(this int value, string key) =>
			Singleton.GetOrAdd<RxPlayerPrefsBinder>().Save(key, Convert.ToInt32(value));
		
		/// <summary>
		/// 로컬 데이터 저장
		/// </summary>
		public static string SetPlayerPrefs(this string value, string key) =>
			Singleton.GetOrAdd<RxPlayerPrefsBinder>().Save(key, value);
		
		/// <summary>
		/// 로컬 데이터 저장
		/// </summary>
		public static T SetPlayerPrefs<T>(this T value, string key) where T : IModel=>
			Singleton.GetOrAdd<RxPlayerPrefsBinder>().Save(key, value);
		
		/// <summary>
		/// 로컬 데이터 호출
		/// </summary>
		public static T GetPlayerPrefs<T>(this IMVP mvp, string key) =>
			GetSingleton<RxPlayerPrefsBinder>().Load<T>(key);
	}
}