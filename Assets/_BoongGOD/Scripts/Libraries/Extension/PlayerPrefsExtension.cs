using System;
using Redbean.Rx;
using Redbean.Static;

namespace Redbean.Extension
{
	public static partial class Extension
	{
		/// <summary>
		/// 로컬 데이터 저장
		/// </summary>
		public static void PlayerPrefsSave(this float value, string key) =>
			Singleton.Get<RxPlayerPrefsBinder>().Save(key, value);
		
		/// <summary>
		/// 로컬 데이터 저장
		/// </summary>
		public static void PlayerPrefsSave(this int value, string key) =>
			Singleton.Get<RxPlayerPrefsBinder>().Save(key, Convert.ToInt32(value));
		
		/// <summary>
		/// 로컬 데이터 저장
		/// </summary>
		public static void PlayerPrefsSave(this string value, string key) =>
			Singleton.Get<RxPlayerPrefsBinder>().Save(key, value);
	}
}