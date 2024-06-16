using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Auth;
using Google;
using Redbean.Auth;
using UnityEngine;

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

			if (ApiBase.Http.DefaultRequestHeaders.Contains("Authorization"))
				ApiBase.Http.DefaultRequestHeaders.Remove("Authorization");
		}

		public async Task<Response> RequestApi(Type type, params object[] args) => 
			await apis[type].Request(args);

		public async Task<Response> RequestApi<T>(params object[] args) where T : IApi =>
			await apis[typeof(T)].Request(args);
		
#if UNITY_EDITOR
		public static async Task<Response> EditorRequestApi<T>(params object[] args) where T : IApi
		{
			const string Key = "EDITOR_ACCESS_UID";
			
			using var api = new ApiSingleton();
			
			var token = PlayerPrefs.GetString(Key);
			if (string.IsNullOrEmpty(token))
			{
				var authenticationProvider = new GoogleAuthenticationProvider();
				await authenticationProvider.Initialize();

				var authenticationResult = await authenticationProvider.Login();
				var user = await FirebaseAuth.DefaultInstance.SignInWithCredentialAsync(authenticationResult.Credential);
				await RequestAccessTokenAsync(user.UserId);
				
				PlayerPrefs.SetString(Key, user.UserId);
			}
			else
				await RequestAccessTokenAsync(token);

			return await api.RequestApi<T>(args);
		}
		
		private static async Task RequestAccessTokenAsync(string token)
		{
			var request = await ApiGetRequest.GetTokenRequest(token);
			var response = request.ToConvert<AccessTokenResponse>();

			ApiBase.Http.DefaultRequestHeaders.Add("Authorization",  $"Bearer {response.AccessToken}");
		}
#endif
	}
}