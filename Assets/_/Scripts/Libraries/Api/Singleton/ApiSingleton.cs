﻿using System;
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

		public async Task<Response> RequestApi(Type type, params object[] args) => 
			await apis[type].Request(args);

		public async Task<Response> RequestApi<T>(params object[] args) where T : IApi =>
			await apis[typeof(T)].Request(args);
	}
}