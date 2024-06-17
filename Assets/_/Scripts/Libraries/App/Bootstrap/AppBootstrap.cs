using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Redbean
{
	public class AppBootstrap
	{
		private static readonly Dictionary<BootstrapType, IAppBootstrap[]> Bootstraps = new();
		
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
		public static async void RuntimeBootstrap()
		{
			Application.runInBackground = true;
			Application.targetFrameRate = 60;
			
			var bootstraps = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(_ => _.GetTypes())
				.Where(_ => typeof(IAppBootstrap).IsAssignableFrom(_)
				            && !_.IsInterface
				            && !_.IsAbstract)
				.Select(_ => Activator.CreateInstance(Type.GetType(_.FullName)) as IAppBootstrap)
				.OrderBy(_ => _.ExecutionOrder)
				.ToList();

			var flags = Enum.GetNames(typeof(BootstrapType));
			foreach (var flag in flags)
			{
				var type = Enum.Parse<BootstrapType>(flag);
				Bootstraps[type] = bootstraps.Where(_ => _.ExecutionType == type).ToArray();
			}

			await BootstrapSetup(BootstrapType.Runtime);
			
			var go = new GameObject("[Application Life Cycle]");
			go.AddComponent<AppLifeCycle>();
			
			Object.DontDestroyOnLoad(go);
		}

		public static async Task BootstrapSetup(BootstrapType type)
		{
			foreach (var bootstrap in Bootstraps[type])
				await bootstrap.Setup();
		}

		public static void BootstrapDispose()
		{
			var bootstrapGroup = new List<IAppBootstrap>();
			foreach (var bootstraps in Bootstraps.Values)
				bootstrapGroup.AddRange(bootstraps);

			var orderByDescending = bootstrapGroup.OrderByDescending(_ => _.DisposeOrder).ToArray();
			foreach (var bootstrap in orderByDescending)
				bootstrap.Dispose();
		}
	}
}