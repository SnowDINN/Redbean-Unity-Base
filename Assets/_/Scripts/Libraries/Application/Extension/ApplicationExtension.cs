﻿using System.Threading.Tasks;
using Redbean.Api;
using Redbean.MVP;

namespace Redbean
{
	public static partial class Extension
	{
		/// <summary>
		/// 모델 호출
		/// </summary>
		public static T GetModel<T>(this IApplicationBootstrap bootstrap) where T : IModel => GetModel<T>();
		
		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		public static T GetSingleton<T>(this IApplicationBootstrap bootstrap) where T : ISingleton => GetSingleton<T>();
		
		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		public static async Task RequestApi<T>(this IApplicationBootstrap bootstrap, params object[] parameters) where T : IApi => await GetSingleton<ApiSingleton>().RequestApi<T>(parameters);
	}
}