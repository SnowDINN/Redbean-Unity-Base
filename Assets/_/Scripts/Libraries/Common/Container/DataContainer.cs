using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Redbean.Cryptography;
using Redbean.MVP;
using UnityEngine;

namespace Redbean.Dependencies
{
	public class DataContainer : IApplicationBootstrap
	{
		private static readonly Dictionary<Type, IModel> models = new();
		private static readonly AES128 aes = new();
		
		private static Dictionary<string, string> playerPrefsGroup = new();
		
		public int ExecutionOrder => 1;
		
		public UniTask Setup()
		{
			var nativeSingletons = AppDomain.CurrentDomain.GetAssemblies()
			                                .SelectMany(x => x.GetTypes())
			                                .Where(x => typeof(IModel).IsAssignableFrom(x)
			                                            && !typeof(IRxModel).IsAssignableFrom(x)
			                                            && !x.IsInterface
			                                            && !x.IsAbstract)
			                                .Select(x => Activator.CreateInstance(Type.GetType(x.FullName)) as IModel);

			foreach (var singleton in nativeSingletons
				         .Where(model => models.TryAdd(model.GetType(), model)))
				Log.System($"Create instance {singleton.GetType().FullName}");

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
			
			return UniTask.CompletedTask;
		}

		public void Dispose()
		{
			models.Clear();
			playerPrefsGroup.Clear();
		}

		/// <summary>
		/// 모델 전부 제거
		/// </summary>
		public static void Clear() => models.Clear();

		/// <summary>
		/// 모델 호출
		/// </summary>
		public static T Get<T>() where T : IModel => (T)models[typeof(T)];

		/// <summary>
		/// 모델 호출
		/// </summary>
		public static IModel Get(Type type) => models[type];

		/// <summary>
		/// 모델 재정의
		/// </summary>
		public static T Override<T>(T model, bool isPlayerPrefs = false) where T : IModel
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
		public static T Save<T>(string key, T value)
		{
			playerPrefsGroup[key] = JsonConvert.SerializeObject(value);
			
			var encryptValue = aes.Encrypt(JsonConvert.SerializeObject(playerPrefsGroup));
			PlayerPrefs.SetString(Key.GetDataGroup, encryptValue);
			
			return value;
		}
		
		/// <summary>
		/// 로컬 데이터 호출
		/// </summary>
		public static T Load<T>(string key)
		{
			return playerPrefsGroup.TryGetValue(key, out var value) 
				? JsonConvert.DeserializeObject<T>(value) 
				: default;
		}
	}
}