using System.Collections.Generic;
using Newtonsoft.Json;
using R3;
using Redbean.Base;
using Redbean.Define;
using Redbean.Extension;
using UnityEngine;

namespace Redbean.Rx
{
	public class RxPlayerPrefsBinder : RxBase
	{
		private readonly Subject<(string key, object value)> onPlayerPrefsChanged = new();
		public Observable<(string key, object value)> OnPlayerPrefsChanged => onPlayerPrefsChanged.Share();

		public readonly Dictionary<string, object> PlayerPrefsGroup = new();

		public RxPlayerPrefsBinder()
		{
			var deserializer =
				JsonConvert.DeserializeObject<Dictionary<string, object>>(PlayerPrefs.GetString(Key.GetDataGroup));
			if (deserializer != null)
				PlayerPrefsGroup = deserializer;

			OnPlayerPrefsChanged.Subscribe(_ =>
			{
				Log.Print("Data", $"Published PlayerPrefs : {_.key} | {_.value}", Color.yellow);
			}).AddTo(disposables);
		}

		public override void Dispose()
		{
			base.Dispose();
			
			PlayerPrefsGroup.Clear();
		}

		public void Save<T>(string key, T value)
		{
			PlayerPrefsGroup[key] = value;
			PlayerPrefs.SetString(Key.GetDataGroup, JsonConvert.SerializeObject(PlayerPrefsGroup));
			
			onPlayerPrefsChanged.OnNext((key, PlayerPrefsGroup[key]));
		}
	}   
}