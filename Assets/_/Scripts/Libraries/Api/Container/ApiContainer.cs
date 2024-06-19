using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Redbean.Api;

#if UNITY_EDITOR
using System.Web;
using Firebase.Auth;
using Redbean.Auth;
using UnityEngine;
#endif

namespace Redbean.Singleton
{
	public class ApiContainer : IAppBootstrap
	{
		private static readonly Dictionary<Type, IApi> apiGroup = new();
		public static readonly HttpClient Http = new()
		{
			BaseAddress = new Uri("https://localhost:44395"),
			DefaultRequestHeaders =
			{
				{ "accept", "application/json" },
			},
			Timeout = TimeSpan.FromSeconds(10),
		};

		private const string AuthorizationKey = "Authorization";

		public AppBootstrapType ExecutionType => AppBootstrapType.Runtime;
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
#if UNITY_EDITOR
			RemoveAccessToken();
#endif
			
			apiGroup.Clear();
			
			Log.System("Api has been terminated.");
		}

		public static void RemoveAccessToken()
		{
			if (Http.DefaultRequestHeaders.Contains(AuthorizationKey))
				Http.DefaultRequestHeaders.Remove(AuthorizationKey);
		}

		public static void SetAccessToken(string token)
		{
			RemoveAccessToken();
			Http.DefaultRequestHeaders.Add(AuthorizationKey, $"Bearer {token}");
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
			await api.Setup();
			
			var token = PlayerPrefs.GetString(Key);
			if (string.IsNullOrEmpty(token))
			{
				var authenticationProvider = new GoogleAuthenticationProvider();
				if (!GoogleAuthenticationProvider.IsInitialize)
					await authenticationProvider.Initialize();

				var authenticationResult = await authenticationProvider.Login();
				var user = await FirebaseAuth.DefaultInstance.SignInWithCredentialAsync(authenticationResult.Credential);
				var uid = user.UserId.Encrypt();
				
				await RequestAccessTokenAsync(uid);
				
				PlayerPrefs.SetString(Key, uid);
			}
			else
				await RequestAccessTokenAsync(token);

			return await api.EditorRequestApi<T>(args);
		}
		
		private static async Task RequestAccessTokenAsync(string token)
		{
			var request = await ApiGetRequest.GetTokenRequest(HttpUtility.UrlEncode(token));
			var response = request.ToConvert<AccessTokenResponse>();

			SetAccessToken(response.AccessToken);
		}
#endif
	}
}