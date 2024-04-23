using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Console = Redbean.Extension.Console;

namespace Redbean.Base
{
	public class Singleton
	{
		private static readonly Dictionary<string, ISingleton> Singletons = new();

		public Singleton()
		{
			var singletons = AppDomain.CurrentDomain.GetAssemblies()
			                          .SelectMany(x => x.GetTypes())
			                          .Where(x => typeof(ISingleton).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
			                          .Select(x => (ISingleton)Activator.CreateInstance(Type.GetType(x.FullName)))
			                          .ToList();

			foreach (var singleton in singletons)
			{
				Singletons.TryAdd(singleton.GetType().Name, singleton);
				Console.Log($"[Singleton] Create Instance {singleton.GetType().Name}", Color.cyan);
			}
		}

		public static T Get<T>() => (T)Singletons[typeof(T).Name];
	}	
}