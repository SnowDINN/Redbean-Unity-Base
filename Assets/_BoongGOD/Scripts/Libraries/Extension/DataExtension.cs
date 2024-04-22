using System;
using Redbean.Base;
using Redbean.Rx;

namespace Redbean.Extension
{
	public static class DataExtension
	{
		public static void Save(this float value, string key) =>
			Singleton.Get<RxDataBinder>().Add(key, value);
		
		public static void Save(this int value, string key) =>
			Singleton.Get<RxDataBinder>().Add(key, Convert.ToInt32(value));
		
		public static void Save(this string value, string key) =>
			Singleton.Get<RxDataBinder>().Add(key, value);
	}
}