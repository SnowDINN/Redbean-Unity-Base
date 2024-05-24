using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Redbean
{
	public class ApplicationBootstrap
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
		public static async void AssembliesBootstrap()
		{
			Application.runInBackground = true;
			Application.targetFrameRate = 60;
			
			var instances = AppDomain.CurrentDomain.GetAssemblies()
			                     .SelectMany(x => x.GetTypes())
			                     .Where(x => typeof(IApplicationBootstrap).IsAssignableFrom(x)
			                                 && !x.IsInterface
			                                 && !x.IsAbstract)
			                     .Select(x => Activator.CreateInstance(Type.GetType(x.FullName)) as IApplicationBootstrap)
			                     .OrderBy(_ => _.ExecutionOrder)
			                     .ToList();

			foreach (var instance in instances)
				await instance.Setup();
			
			var go = new GameObject("[Application Life Cycle]");
			go.AddComponent<ApplicationLifeCycle>().Bootstrap(instances);
			
			Object.DontDestroyOnLoad(go);
		}
	}
}