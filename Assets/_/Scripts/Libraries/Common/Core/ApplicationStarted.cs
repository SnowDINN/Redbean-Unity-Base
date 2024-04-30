using System;
using System.Linq;
using UnityEngine;

namespace Redbean.Core
{
	public class ApplicationStarted
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
		public static async void AssembliesSetup()
		{
			var instances = AppDomain.CurrentDomain.GetAssemblies()
			                         .SelectMany(x => x.GetTypes())
			                         .Where(x => typeof(IApplicationStarted).IsAssignableFrom(x)
			                                     && !x.IsInterface
			                                     && !x.IsAbstract)
			                         .Select(x => Activator.CreateInstance(Type.GetType(x.FullName)) as IApplicationStarted)
			                         .OrderBy(_ => _.ExecutionOrder);

			foreach (var instance in instances)
				await instance.Setup();
		}
	}   
}