using System.Collections.Generic;
using Newtonsoft.Json;
using R3;
using Redbean.Base;
using UnityEngine;

namespace Redbean.Rx
{
	public class RxPlayerPrefsBinder : RxBase
	{
		private readonly Subject<(string key, object value)> onPlayerPrefsChanged = new();
		public Observable<(string key, object value)> OnPlayerPrefsChanged => onPlayerPrefsChanged.Share();

		private readonly Dictionary<string, string> PlayerPrefsGroup = new();

		public RxPlayerPrefsBinder()
		{
			var deserializer = JsonConvert.DeserializeObject<Dictionary<string, string>>(PlayerPrefs.GetString(Key.GetDataGroup));
			if (deserializer != null)
				PlayerPrefsGroup = deserializer;
		}

		public bool IsContains(string key) => PlayerPrefsGroup.ContainsKey(key);

		/// <summary>
		/// 로컬 데이터 저장 및 퍼블리싱
		/// </summary>
		public T Save<T>(string key, T value)
		{
			PlayerPrefsGroup[key] = JsonConvert.SerializeObject(value);
			PlayerPrefs.SetString(Key.GetDataGroup, JsonConvert.SerializeObject(PlayerPrefsGroup));
			
			onPlayerPrefsChanged.OnNext((key, PlayerPrefsGroup[key]));
			return value;
		}

		/// <summary>
		/// 로컬 데이터 호출
		/// </summary>
		public T Load<T>(string key)
		{
			return PlayerPrefsGroup.TryGetValue(key, out var value) 
				? JsonConvert.DeserializeObject<T>(value) 
				: default;
		}
	}   
}