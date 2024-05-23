using System.Collections.Generic;
using Newtonsoft.Json;
using R3;
using Redbean.Base;
using Redbean.Cryptography;
using UnityEngine;

namespace Redbean.Rx
{
	public class RxPlayerPrefsBinder : RxBase
	{
		private readonly Subject<(string key, object value)> onPlayerPrefsChanged = new();
		public Observable<(string key, object value)> OnPlayerPrefsChanged => onPlayerPrefsChanged.Share();

		private readonly Dictionary<string, string> playerPrefsGroup = new();
		private readonly AES128 aes = new();

		public RxPlayerPrefsBinder()
		{
			if (!PlayerPrefs.HasKey(Key.GetDataGroup))
				return;

			var decryptValue = aes.Decrypt(PlayerPrefs.GetString(Key.GetDataGroup));
				
			var deserializer = JsonConvert.DeserializeObject<Dictionary<string, string>>(decryptValue);
			if (deserializer != null)
				playerPrefsGroup = deserializer;
		}

		public bool IsContains(string key) => playerPrefsGroup.ContainsKey(key);

		/// <summary>
		/// 로컬 데이터 저장 및 퍼블리싱
		/// </summary>
		public T Save<T>(string key, T value)
		{
			playerPrefsGroup[key] = JsonConvert.SerializeObject(value);
			
			var encryptValue = aes.Encrypt(JsonConvert.SerializeObject(playerPrefsGroup));
			PlayerPrefs.SetString(Key.GetDataGroup, encryptValue);
			
			onPlayerPrefsChanged.OnNext((key, playerPrefsGroup[key]));
			return value;
		}

		/// <summary>
		/// 로컬 데이터 호출
		/// </summary>
		public T Load<T>(string key)
		{
			return playerPrefsGroup.TryGetValue(key, out var value) 
				? JsonConvert.DeserializeObject<T>(value) 
				: default;
		}
	}   
}