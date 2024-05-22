using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Redbean.Core
{
	public class ApplicationCore
	{
		public static bool IsReady;
		
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
		public static async void AssembliesSetup()
		{
			Application.runInBackground = true;
			Application.targetFrameRate = 60;
			
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
			go.AddComponent<MonoApplicationCore>().AddInstances(instances);
			
			Object.DontDestroyOnLoad(go);
			
			IsReady = true;
		}
	}

	public class MonoApplicationCore : MonoBehaviour
	{
		private List<IApplicationCore> instances = new();
		
		private void OnDestroy()
		{
			foreach (var instance in instances)
				instance.Dispose();
			
			Log.System("App has been terminated.");
		}

		public void AddInstances(List<IApplicationCore> instances)
		{
			this.instances = instances;
			this.instances.Reverse();
		}
	}
}