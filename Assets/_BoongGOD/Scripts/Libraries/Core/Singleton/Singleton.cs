using System;
using System.Collections.Generic;
using System.Linq;
using Redbean.Extension;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Redbean.Static
{
	public class Singleton : IBootstrap
	{
		private static readonly Dictionary<Type, ISingleton> singletons = new();
		private readonly GameObject componentParent;

		public Singleton()
		{
			var nativeSingletons = AppDomain.CurrentDomain.GetAssemblies()
			                                .SelectMany(x => x.GetTypes())
			                                .Where(x => !typeof(MonoBehaviour).IsAssignableFrom(x)
			                                            && typeof(ISingleton).IsAssignableFrom(x)
			                                            && !x.IsInterface
			                                            && !x.IsAbstract)
			                                .Select(x => (ISingleton)Activator
				                                        .CreateInstance(Type.GetType(x.FullName)));

			var monoSingletons = AppDomain.CurrentDomain.GetAssemblies()
			                              .SelectMany(x => x.GetTypes())
			                              .Where(x => typeof(MonoBehaviour).IsAssignableFrom(x)
			                                          && typeof(ISingleton).IsAssignableFrom(x)
			                                          && !x.IsInterface
			                                          && !x.IsAbstract);

			if (monoSingletons.Any())
			{
				componentParent = new GameObject("[Singleton Group]");
				Object.DontDestroyOnLoad(componentParent);
			}

			foreach (var singleton in nativeSingletons
				         .Where(singleton => singletons.TryAdd(singleton.GetType(), singleton)))
				Log.Print("Native Singleton", $" Create instance {singleton.GetType().FullName}", Color.cyan);
			
			foreach (var singleton in monoSingletons
				         .Where(singleton => singletons.TryAdd(singleton, (ISingleton)componentParent.AddComponent(singleton))))
				Log.Print("Mono Singleton", $"Create instance {singleton.FullName}", Color.cyan);
		}
		
		public void Dispose()
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