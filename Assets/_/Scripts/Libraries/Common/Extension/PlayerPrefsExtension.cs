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
		public static float PlayerPrefsSave(this float value, string key) =>
			Singleton.GetOrAdd<RxPlayerPrefsBinder>().Save(key, value);
		
		/// <summary>
		/// 로컬 데이터 저장
		/// </summary>
		public static int PlayerPrefsSave(this int value, string key) =>
			Singleton.GetOrAdd<RxPlayerPrefsBinder>().Save(key, Convert.ToInt32(value));
		
		/// <summary>
		/// 로컬 데이터 저장
		/// </summary>
		public static string PlayerPrefsSave(this string value, string key) =>
			Singleton.GetOrAdd<RxPlayerPrefsBinder>().Save(key, value);
		
		/// <summary>
		/// 로컬 데이터 저장
		/// </summary>
		public static T PlayerPrefsSave<T>(this T value, string key) where T : IModel=>
			Singleton.GetOrAdd<RxPlayerPrefsBinder>().Save(key, value);
	}
}