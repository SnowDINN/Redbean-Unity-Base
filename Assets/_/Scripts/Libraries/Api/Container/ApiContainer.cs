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

namespace Redbean
{
#region Activator

	public interface IApi : IExtension
	{
		Task<Response> Request(params object[] args);
	}

#endregion
	
	public class ApiContainer : IAppBootstrap
	{
		public AppBootstrapType ExecutionType => AppBootstrapType.Runtime;
		public int ExecutionOrder => 20;
		
		private static readonly Dictionary<Type, IApi> apiGroup = new();
		public static readonly HttpClient Http = new(new HttpClientHandler
		{
			UseProxy = false,
		})
		{
			BaseAddress = new Uri("https://localhost:44395"),
			DefaultRequestHeaders =
			{
				{ "accept", "application/json" },
			},
			Timeout = TimeSpan.FromSeconds(10),
		};
		
		private static TokenResponse currentToken = new();
		public static bool IsAccessTokenExpired => currentToken.AccessTokenExpire < DateTime.UtcNow;
		public static bool IsRefreshTokenExpired => currentToken.RefreshTokenExpire < DateTime.UtcNow;
		public static bool IsRefreshTokenExist => !string.IsNullOrEmpty(currentToken.RefreshToken);
		public static string RefreshToken => currentToken.RefreshToken;
		
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
			if (Http.DefaultRequestHeaders.Contains("Authorization"))
				Http.DefaultRequestHeaders.Remove("Authorization");
		}

		public static void SetAccessToken(TokenResponse response)
		{
			currentToken = response;
			
			RemoveAccessToken();
			Http.DefaultRequestHeaders.Add("Authorization", $"Bearer {response.AccessToken}");
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
			
			var uid = PlayerPrefs.GetString(Key);
			if (string.IsNullOrEmpty(uid))
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
				await RequestAccessTokenAsync(uid);

			return await api.EditorRequestApi<T>(args);
		}
		
		private static async Task RequestAccessTokenAsync(string uid)
		{
			var request = await ApiGetRequest.GetEditorAccessTokenRequest(HttpUtility.UrlEncode(uid.Encryption()),
			                                                              HttpUtility.UrlEncode(AppSettings.Version.Encryption()));
			var response = request.ToConvert<TokenResponse>();

			SetAccessToken(response);
		}
#endif
	}
}