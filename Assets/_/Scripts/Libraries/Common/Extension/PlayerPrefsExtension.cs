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
		public static void PlayerPrefsSave(this float value, string key) =>
			Singleton.GetOrAdd<RxPlayerPrefsBinder>().Save(key, value);
		
		/// <summary>
		/// 로컬 데이터 저장
		/// </summary>
		public static void PlayerPrefsSave(this int value, string key) =>
			Singleton.GetOrAdd<RxPlayerPrefsBinder>().Save(key, Convert.ToInt32(value));
		
		/// <summary>
		/// 로컬 데이터 저장
		/// </summary>
		public static void PlayerPrefsSave(this string value, string key) =>
			Singleton.GetOrAdd<RxPlayerPrefsBinder>().Save(key, value);
		
		/// <summary>
		/// 로컬 데이터 저장
		/// </summary>
		public static void PlayerPrefsSave<T>(this T value, string key) where T : IModel=>
			Singleton.GetOrAdd<RxPlayerPrefsBinder>().Save(key, value);
	}
}