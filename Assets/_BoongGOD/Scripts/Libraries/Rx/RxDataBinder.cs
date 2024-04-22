using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using R3;
using Redbean.Base;
using Redbean.Define;
using UnityEngine;

namespace Redbean.Rx
{
	public class RxDataBinder : ISingleton
	{
		private readonly Subject<(string key, object value)> onDataChanged = new();
		public Observable<(string key, object value)> OnDataChanged => 
			onDataChanged.Share();

		private readonly Dictionary<string, object> dataGroup = new();

		public RxDataBinder()
		{
			var deserializer =
				JsonConvert.DeserializeObject<Dictionary<string, object>>(PlayerPrefs.GetString(Key.GetDataGroup));
			if (deserializer != null)
				dataGroup = deserializer;
		}

		~RxDataBinder() =>
			dataGroup.Clear();

		public void Add<T>(string key, T value)
		{
			dataGroup[key] = value;
			PlayerPrefs.SetString(Key.GetDataGroup, JsonConvert.SerializeObject(dataGroup));
			
			onDataChanged.OnNext((key, dataGroup[key]));
		}
		
		public int GetInt(string key) =>
			dataGroup.TryGetValue(key, out var value) ? Convert.ToInt32(value) : default;
		
		public float GetFloat(string key) =>
			dataGroup.TryGetValue(key, out var value) ? Convert.ToSingle(value) : default;
		
		public string GetString(string key) =>
			dataGroup.TryGetValue(key, out var value) ? Convert.ToString(value) : default;

		public T Get<T>(string key) =>
			dataGroup.TryGetValue(key, out var value) ? (T)value : default;
	}   
}