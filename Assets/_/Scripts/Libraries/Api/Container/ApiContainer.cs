using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Redbean.Api;

#if UNITY_EDITOR
using Firebase.Auth;
using Redbean.Auth;
using UnityEngine;
#endif

namespace Redbean.Singleton
{
	public class ApiContainer : IAppBootstrap
	{
		private static readonly Dictionary<Type, IApi> apiGroup = new();

		public BootstrapType ExecutionType => BootstrapType.Runtime;
		public int ExecutionOrder => 20;
		
		public Task Setup()
		{
			var apis = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(x => x.GetTypes())
				.Where(_ => _.FullName != null
				            && typeof(IApi).IsAssignableFrom(_)
				            && !_.IsInterface
				            && !_.IsAbstract)
				.Select(_ => Activator.CreateInstance(Type.GetType(_.FullName)) as IApi)
				.ToArray();

			foreach (var api in apis)
				apiGroup.TryAdd(api.GetType(), api);
			
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			apiGroup.Clear();

#if UNITY_EDITOR
			if (ApiBase.Http.DefaultRequestHeaders.Contains("Authorization"))
				ApiBase.Http.DefaultRequestHeaders.Remove("Authorization");
#endif
			
			Log.System("Api has been terminated.");
		}

		public static async Task<Response> RequestApi(Type type, params object[] args) => 
			await apiGroup[type].Request(args);

		public static async Task<Response> RequestApi<T>(params object[] args) where T : IApi =>
			await apiGroup[typeof(T)].Request(args);
		
#if UNITY_EDITOR
		public static async Task<Response> EditorRequestApi<T>(params object[] args) where T : IApi
		{
			const string Key = "EDITOR_ACCESS_UID";
			
			using var api = new ApiContainer();
			
			var token = PlayerPrefs.GetString(Key);
			if (string.IsNullOrEmpty(token))
			{
				var authenticationProvider = new GoogleAuthenticationProvider();
				if (!GoogleAuthenticationProvider.IsInitialize)
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