using System;
using Redbean.Rx;
using Redbean.Static;

namespace Redbean.Extension
{
	public static partial class Extension
	{
		public static void LocalSave(this float value, string key) =>
			Singleton.Get<RxDataBinder>().Add(key, value);
		
		public static void LocalSave(this int value, string key) =>
			Singleton.Get<RxDataBinder>().Add(key, Convert.ToInt32(value));
		
		public static void LocalSave(this string value, string key) =>
			Singleton.Get<RxDataBinder>().Add(key, value);
	}
}