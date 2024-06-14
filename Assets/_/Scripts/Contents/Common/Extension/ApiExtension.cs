using System;
using System.Threading.Tasks;
using Redbean.Api;
using Redbean.MVP;
using Redbean.Rx;

namespace Redbean
{
	public static partial class Extension
	{
		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		public static T GetSingleton<T>(this IApi api) where T : ISingleton => GetSingleton<T>();
		
		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		public static object GetSingleton(this IApi api, Type type) => GetSingleton(type);
		
		/// <summary>
		/// 모델 호출
		/// </summary>
		public static T GetModel<T>(this IApi api) where T : IModel => GetModel<T>();
		
		/// <summary>
		/// 모델 호출
		/// </summary>
		public static object GetModel(this IApi api, Type type) => GetModel(type);
		
		/// <summary>
		/// API 호출
		/// </summary>
		public static async Task<Response> RequestApi<T>(this IApi api, params object[] args) where T : IApi => 
			await GetSingleton<ApiSingleton>().RequestApi<T>(args);

		/// <summary>
		/// API 데이터 배포
		/// </summary>
		public static void Publish<T>(this T api, Response response) where T : IApi =>
			GetSingleton<RxApiBinder>().Publish<T>(response);
	}
}