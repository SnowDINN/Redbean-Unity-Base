using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Redbean.Base;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Redbean.Dependencies
{
	public class SingletonContainer : IApplicationBootstrap
	{
		private static readonly Dictionary<Type, ISingleton> singletons = new();
		private GameObject parent;

		public int ExecutionOrder => 0;

		public Task Setup()
		{
#region Native

			var nativeSingletons = AppDomain.CurrentDomain.GetAssemblies()
			                                .SelectMany(x => x.GetTypes())
			                                .Where(x => x.FullName != null
			                                            && typeof(ISingleton).IsAssignableFrom(x)
			                                            && !typeof(MonoBehaviour).IsAssignableFrom(x)
			                                            && !x.FullName.Equals(typeof(RxBase).FullName)
			                                            && !x.IsInterface
			                                            && !x.IsAbstract)
			                                .Select(x => Activator.CreateInstance(Type.GetType(x.FullName)) as ISingleton);

			foreach (var singleton in nativeSingletons
				         .Where(singleton => singletons.TryAdd(singleton.GetType(), singleton)))
				Log.System($"Create instance {singleton.GetType().FullName}");

#endregion

#region Mono

			var monoSingletons = AppDomain.CurrentDomain.GetAssemblies()
			                              .SelectMany(x => x.GetTypes())
			                              .Where(x => x.FullName != null
			                                          && typeof(ISingleton).IsAssignableFrom(x)
			                                          && typeof(MonoBehaviour).IsAssignableFrom(x)
			                                          && !x.FullName.Equals(typeof(RxBase).FullName)
			                                          && !x.IsInterface
			                                          && !x.IsAbstract);
			
			if (monoSingletons.Any())
			{
				parent = new GameObject("[Singleton Group]");
				Object.DontDestroyOnLoad(parent);
			}
			
			foreach (var singleton in monoSingletons
				         .Where(singleton => singletons.TryAdd(singleton, parent.AddComponent(singleton) as ISingleton)))
				Log.System($"Create instance {singleton.FullName}");

#endregion

			return Task.CompletedTask;
		}

		public void Dispose()
		{
			foreach (var singleton in singletons.Values)
				singleton.Dispose();
			
			Log.System("Rx or Event has been terminated.");
		}

		/// <summary>
		/// 싱글톤 전부 제거
		/// </summary>
		public static void Clear() => singletons.Clear();
		
		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		public static ISingleton Get(Type type) => singletons[type];
		
		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		public static T Get<T>() where T : ISingleton => (T)singletons[typeof(T)];
	}
}