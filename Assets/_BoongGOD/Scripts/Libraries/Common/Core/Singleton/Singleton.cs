using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Redbean.Debug;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Redbean.Core
{
	public class Singleton : IApplicationStarted
	{
		private static readonly Dictionary<Type, ISingleton> singletons = new();
		private GameObject parent;

		public int ExecutionOrder => 0;

		public UniTask Setup()
		{
#region Native

			var nativeSingletons = AppDomain.CurrentDomain.GetAssemblies()
			                                .SelectMany(x => x.GetTypes())
			                                .Where(x => !typeof(MonoBehaviour).IsAssignableFrom(x)
			                                            && typeof(ISingleton).IsAssignableFrom(x)
			                                            && !x.IsInterface
			                                            && !x.IsAbstract)
			                                .Select(x => (ISingleton)Activator.CreateInstance(Type.GetType(x.FullName)));

			foreach (var singleton in nativeSingletons
				         .Where(singleton => singletons.TryAdd(singleton.GetType(), singleton)))
				Log.Print("Native Singleton", $" Create instance {singleton.GetType().FullName}", Color.cyan);

#endregion

#region Mono

			var monoSingletons = AppDomain.CurrentDomain.GetAssemblies()
			                              .SelectMany(x => x.GetTypes())
			                              .Where(x => typeof(MonoBehaviour).IsAssignableFrom(x)
			                                          && typeof(ISingleton).IsAssignableFrom(x)
			                                          && !x.IsInterface
			                                          && !x.IsAbstract);
			
			if (monoSingletons.Any())
			{
				parent = new GameObject("[Singleton Group]");
				Object.DontDestroyOnLoad(parent);
			}
			
			foreach (var singleton in monoSingletons
				         .Where(singleton => singletons.TryAdd(singleton, (ISingleton)parent.AddComponent(singleton))))
				Log.Print("Mono Singleton", $"Create instance {singleton.FullName}", Color.cyan);

#endregion

			return UniTask.CompletedTask;
		}
		
		/// <summary>
		/// 싱글톤 전부 제거
		/// </summary>
		public static void Clear()
		{
			foreach (var singleton in singletons)
				singleton.Value.Dispose();
			
			singletons.Clear();
		}

		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		public static T GetOrAdd<T>() where T : ISingleton
		{
			if (singletons.TryGetValue(typeof(T), out var value))
				return (T)value;
			
			singletons[typeof(T)] = (ISingleton)Activator.CreateInstance(typeof(T));
			return (T)singletons[typeof(T)];
		}
		
		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		public static ISingleton GetOrAdd(Type type)
		{
			if (singletons.TryGetValue(type, out var value))
				return value;

			singletons[type] = (ISingleton)Activator.CreateInstance(type);
			return singletons[type];
		}
	}	
}