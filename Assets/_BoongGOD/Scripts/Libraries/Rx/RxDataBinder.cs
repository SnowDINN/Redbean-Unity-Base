using System.Collections.Generic;
using Newtonsoft.Json;
using R3;
using Redbean.Base;
using Redbean.Define;
using Redbean.Extension;
using Redbean.Static;
using UnityEngine;

namespace Redbean.Rx
{
	public class RxDataBinder : RxBase, ISingleton
	{
		private readonly Subject<(string key, object value)> onDataChanged = new();
		public Observable<(string key, object value)> OnDataChanged => onDataChanged.Share();

		public readonly Dictionary<string, object> DataGroup = new();

		public RxDataBinder()
		{
			var deserializer =
				JsonConvert.DeserializeObject<Dictionary<string, object>>(PlayerPrefs.GetString(Key.GetDataGroup));
			if (deserializer != null)
				DataGroup = deserializer;

			onDataChanged.Subscribe(_ =>
			{
				Console.Log("Data", $"Update data : {_.key} | {_.value}", Color.yellow);
			}).AddTo(disposables);
		}

		~RxDataBinder()
		{
			DataGroup.Clear();
			Dispose();
		}

		public void Save<T>(string key, T value)
		{
			DataGroup[key] = value;
			PlayerPrefs.SetString(Key.GetDataGroup, JsonConvert.SerializeObject(DataGroup));
			
			onDataChanged.OnNext((key, DataGroup[key]));
		}
	}   
}