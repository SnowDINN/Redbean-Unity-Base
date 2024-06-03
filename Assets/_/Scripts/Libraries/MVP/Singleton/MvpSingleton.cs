using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Redbean.Cryptography;
using Redbean.MVP;
using UnityEngine;

namespace Redbean.Singleton
{
	public class MvpSingleton : ISingleton
	{
		private readonly Dictionary<string, string> playerPrefsGroup = new();
		private readonly Dictionary<Type, IModel> models = new();
		private readonly AES128 aes = new();

		public MvpSingleton()
		{
			var nativeSingletons = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(x => x.GetTypes())
				.Where(x => typeof(IModel).IsAssignableFrom(x)
				            && !typeof(IRxModel).IsAssignableFrom(x)
				            && !x.IsInterface
				            && !x.IsAbstract)
				.Select(x => Activator.CreateInstance(Type.GetType(x.FullName)) as IModel);

			foreach (var _ in nativeSingletons
				         .Where(model => model != null && models.TryAdd(model.GetType(), model)))

#region PlayerPrefs

			if (PlayerPrefs.HasKey(Key.GetDataGroup))
			{
				var dataDecrypt = aes.Decrypt(PlayerPrefs.GetString(Key.GetDataGroup));
				var dataGroups = JsonConvert.DeserializeObject<Dictionary<string, string>>(dataDecrypt);
				foreach (var dataGroup in dataGroups)
				{
					var key = Assembly.Load("Assembly-CSharp").GetTypes().FirstOrDefault(_ => _.FullName == dataGroup.Key);
					var value = JsonConvert.DeserializeObject(dataGroup.Value, key);

					if (value is IModel model)
						models[key] = model;
				}

				playerPrefsGroup = dataGroups;
			}

#endregion
		}

		public void Dispose()
		{
			models.Clear();
			playerPrefsGroup.Clear();
		}

		/// <summary>
		/// 모델 전부 제거
		/// </summary>
		public void Clear() => models.Clear();

		/// <summary>
		/// 모델 호출
		/// </summary>
		public T GetModel<T>() where T : IModel => (T)models[typeof(T)];

		/// <summary>
		/// 모델 호출
		/// </summary>
		public IModel GetModel(Type type) => models[type];

		/// <summary>
		/// 모델 재정의
		/// </summary>
		public T Override<T>(T model, bool isPlayerPrefs = false) where T : IModel
		{
			var targetFields = models[model.GetType()].GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(_ => _.CanWrite).ToArray();
			var copyFields = model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(_ => _.CanWrite).ToArray();
			
			for (var i = 0; i < targetFields.Length; i++)
				targetFields[i].SetValue(models[model.GetType()], copyFields[i].GetValue(model));

			if (isPlayerPrefs)
				model.SetPlayerPrefs(typeof(T).FullName);
			
			return model;
		}
		
		/// <summary>
		/// 로컬 데이터 저장 및 퍼블리싱
		/// </summary>
		public T Save<T>(string key, T value)
		{
			playerPrefsGroup[key] = JsonConvert.SerializeObject(value);
			
			var encryptValue = aes.Encrypt(JsonConvert.SerializeObject(playerPrefsGroup));
			PlayerPrefs.SetString(Key.GetDataGroup, encryptValue);
			
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