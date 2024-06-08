using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiSingleton : ISingleton
	{
		private static readonly Dictionary<Type, IApi> apis = new();

		public ApiSingleton()
		{
			var apis = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(x => x.GetTypes())
				.Where(x => x.FullName != null
				            && typeof(IApi).IsAssignableFrom(x)
				            && !x.IsInterface
				            && !x.IsAbstract)
				.Select(x => Activator.CreateInstance(Type.GetType(x.FullName)) as IApi);

			foreach (var _ in apis.Where(api => api != null && ApiSingleton.apis.TryAdd(api.GetType(), api))) ;
		}

		public void Dispose()
		{
			apis.Clear();
		}

		public async Task GetApi(Type type, params string[] parameters) => await apis[type].Request(parameters);
		public async Task GetApi<T>(params string[] parameters) => await apis[typeof(T)].Request(parameters);
	}
}