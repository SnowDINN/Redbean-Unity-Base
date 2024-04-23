using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Console = Redbean.Extension.Console;
using Object = UnityEngine.Object;

namespace Redbean.Static
{
	public class Singleton
	{
		public static readonly Dictionary<string, ISingleton> Singletons = new();

		public Singleton()
		{
			var go = new GameObject("[Singleton Group]");
			Object.DontDestroyOnLoad(go);
			
			var nativeSingletons = AppDomain.CurrentDomain.GetAssemblies()
			                          .SelectMany(x => x.GetTypes())
			                          .Where(x => typeof(ISingleton).IsAssignableFrom(x)
			                                      && !typeof(MonoBehaviour).IsAssignableFrom(x)
			                                      && !x.IsInterface
			                                      && !x.IsAbstract)
			                          .Select(x => (ISingleton)Activator.CreateInstance(Type.GetType(x.FullName)))
			                          .ToList();
			
			var monoSingletons = AppDomain.CurrentDomain.GetAssemblies()
			                          .SelectMany(x => x.GetTypes())
			                          .Where(x => typeof(ISingleton).IsAssignableFrom(x) 
			                                      && typeof(MonoBehaviour).IsAssignableFrom(x)
			                                      && !x.IsInterface
			                                      && !x.IsAbstract)
			                          .ToList();

			foreach (var singleton in nativeSingletons
				         .Where(singleton => Singletons.TryAdd(singleton.GetType().Name, singleton)))
				Console.Log("Native Singleton", $" Create Instance {singleton.GetType().FullName}", Color.cyan);
			
			foreach (var singleton in monoSingletons
				         .Where(singleton => Singletons.TryAdd(singleton.Name, (ISingleton)go.AddComponent(singleton))))
				Console.Log("Mono Singleton", $"Create Instance {singleton.FullName}", Color.cyan);
		}

		~Singleton()
		{
			Singletons.Clear();
		}

		public static T Get<T>() => (T)Singletons[typeof(T).Name];
	}	
}