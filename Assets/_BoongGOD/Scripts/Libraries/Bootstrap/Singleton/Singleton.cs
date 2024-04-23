using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Console = Redbean.Extension.Console;
using Object = UnityEngine.Object;

namespace Redbean.Static
{
	public class Singleton : IBootstrap
	{
		private static readonly Dictionary<string, ISingleton> Singletons = new();

		public Singleton()
		{
			var go = new GameObject("[Singleton Group]");
			Object.DontDestroyOnLoad(go);

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

			foreach (var singleton in nativeSingletons
				         .Where(singleton => Singletons.TryAdd(singleton.GetType().Name, singleton)))
				Console.Log("Native Singleton", $" Create instance {singleton.GetType().FullName}", Color.cyan);
			
			foreach (var singleton in monoSingletons
				         .Where(singleton => Singletons.TryAdd(singleton.Name, (ISingleton)go.AddComponent(singleton))))
				Console.Log("Mono Singleton", $"Create instance {singleton.FullName}", Color.cyan);
		}

		~Singleton()
		{
			Singletons.Clear();
		}

		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		public static T Get<T>() => (T)Singletons[typeof(T).Name];
	}	
}