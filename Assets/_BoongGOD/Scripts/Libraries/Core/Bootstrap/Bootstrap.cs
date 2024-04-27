using System;
using System.Linq;
using UnityEngine;

namespace Redbean
{
	public class Bootstrap
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
		public static async void AssembliesSetup()
		{
			var instances = AppDomain.CurrentDomain.GetAssemblies()
			                         .SelectMany(x => x.GetTypes())
			                         .Where(x => typeof(IBootstrap).IsAssignableFrom(x)
			                                     && !x.IsInterface
			                                     && !x.IsAbstract)
			                         .Select(x => (IBootstrap)Activator.CreateInstance(Type.GetType(x.FullName)))
			                         .OrderBy(_ => _.ExecutionOrder);

			foreach (var instance in instances)
				await instance.Setup();
		}
	}   
}