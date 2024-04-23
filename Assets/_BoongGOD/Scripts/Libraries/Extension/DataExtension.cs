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
		public static void LocalSave(this float value, string key) =>
			Singleton.Get<RxDataBinder>().Save(key, value);
		
		/// <summary>
		/// 로컬 데이터 저장
		/// </summary>
		public static void LocalSave(this int value, string key) =>
			Singleton.Get<RxDataBinder>().Save(key, Convert.ToInt32(value));
		
		/// <summary>
		/// 로컬 데이터 저장
		/// </summary>
		public static void LocalSave(this string value, string key) =>
			Singleton.Get<RxDataBinder>().Save(key, value);
	}
}