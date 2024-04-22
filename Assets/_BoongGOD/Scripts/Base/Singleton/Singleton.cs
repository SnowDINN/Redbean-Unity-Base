using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Redbean.Base
{
	public class Singleton : MonoBehaviour
	{
		private List<ISingleton> singletons = new();
	
		[RuntimeInitializeOnLoadMethod]
		public static void Initialize()
		{
			var go = new GameObject("Mono Singleton");
			go.AddComponent<Singleton>();
			DontDestroyOnLoad(go);
		}
	
		private void Awake()
		{
			singletons = AppDomain.CurrentDomain.GetAssemblies()
			                      .SelectMany(x => x.GetTypes())
			                      .Where(x => typeof(ISingleton).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
			                      .Select(x => (ISingleton)Activator.CreateInstance(Type.GetType(x.FullName)))
			                      .ToList();
		}
	}	
}