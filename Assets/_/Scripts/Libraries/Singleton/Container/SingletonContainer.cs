using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Redbean.Rx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Redbean.Container
{
	public class SingletonContainer : IAppBootstrap
	{
		private static readonly Dictionary<Type, ISingleton> singletonGroup = new();
		private GameObject parent;

		public BootstrapType ExecutionType => BootstrapType.Runtime;
		public int ExecutionOrder => 10;

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
				.Select(x => Activator.CreateInstance(Type.GetType(x.FullName)) as ISingleton)
				.ToArray();

			foreach (var singleton in nativeSingletons
				         .Where(singleton => singletonGroup.TryAdd(singleton.GetType(), singleton)))
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
				            && !x.IsAbstract)
				.ToArray();
			
			if (monoSingletons.Any())
			{
				parent = new GameObject("[Singleton Group]");
				Object.DontDestroyOnLoad(parent);
			}
			
			foreach (var singleton in monoSingletons
				         .Where(singleton => singletonGroup.TryAdd(singleton, parent.AddComponent(singleton) as ISingleton)))
				Log.System($"Create instance {singleton.FullName}");

#endregion

			return Task.CompletedTask;
		}

		public void Dispose()
		{
			foreach (var singleton in singletonGroup.Values)
				singleton.Dispose();
			
			Log.System("Rx or Event has been terminated.");
		}

		/// <summary>
		/// 싱글톤 전부 제거
		/// </summary>
		public static void Clear() => singletonGroup.Clear();
		
		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		public static ISingleton GetSingleton(Type type) => singletonGroup[type];
		
		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		public static T GetSingleton<T>() where T : ISingleton => (T)singletonGroup[typeof(T)];
	}
}