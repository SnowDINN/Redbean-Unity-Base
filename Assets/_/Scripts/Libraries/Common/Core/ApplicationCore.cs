using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Redbean.Core
{
	public class ApplicationCore
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
		public static async void AssembliesSetup()
		{
			var instances = AppDomain.CurrentDomain.GetAssemblies()
			                     .SelectMany(x => x.GetTypes())
			                     .Where(x => typeof(IApplicationCore).IsAssignableFrom(x)
			                                 && !x.IsInterface
			                                 && !x.IsAbstract)
			                     .Select(x => Activator.CreateInstance(Type.GetType(x.FullName)) as IApplicationCore)
			                     .OrderBy(_ => _.ExecutionOrder)
			                     .ToList();

			foreach (var instance in instances)
				await instance.Setup();
			
			var go = new GameObject("[Application Core]");
			var core = go.AddComponent<MonoApplicationCore>();
			core.AddInstances(instances);
			
			Object.DontDestroyOnLoad(go);
		}
	}

	public class MonoApplicationCore : MonoBehaviour
	{
		private List<IApplicationCore> instances = new();
		
		private void OnDestroy()
		{
			foreach (var instance in instances)
				instance.TearDown();
			
			Log.System("App has been terminated.");
		}

		public void AddInstances(List<IApplicationCore> instances)
		{
			this.instances = instances;
			this.instances.Reverse();
		}
	}
}