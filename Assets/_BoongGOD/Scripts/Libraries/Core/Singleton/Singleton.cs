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
		private static readonly Dictionary<string, ISingleton> singletons = new();
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
				         .Where(singleton => singletons.TryAdd(singleton.GetType().Name, singleton)))
				Console.Log("Native Singleton", $" Create instance {singleton.GetType().FullName}", Color.cyan);
			
			foreach (var singleton in monoSingletons
				         .Where(singleton => singletons.TryAdd(singleton.Name, (ISingleton)componentParent.AddComponent(singleton))))
				Console.Log("Mono Singleton", $"Create instance {singleton.FullName}", Color.cyan);
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
		public static T Get<T>() where T : ISingleton => (T)singletons[typeof(T).Name];
	}	
}