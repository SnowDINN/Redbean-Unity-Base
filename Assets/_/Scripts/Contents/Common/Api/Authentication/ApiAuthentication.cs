using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Redbean.Api
{
	public class ApiAuthentication
	{
		private static TokenResponse currentToken = new();
		public static bool IsAccessTokenExpired => currentToken.AccessTokenExpire < DateTime.UtcNow;
		public static bool IsRefreshTokenExpired => currentToken.RefreshTokenExpire < DateTime.UtcNow;
		public static bool IsRefreshTokenExist => !string.IsNullOrEmpty(currentToken.RefreshToken);
		public static string RefreshToken => currentToken.RefreshToken;
		
		public static void RemoveAccessToken()
		{
			if (ApiSingleton.Http.DefaultRequestHeaders.Contains("Authorization"))
				ApiSingleton.Http.DefaultRequestHeaders.Remove("Authorization");
		}

		public static void SetAccessToken(TokenResponse response)
		{
			currentToken = response;
			
			RemoveAccessToken();
			ApiSingleton.Http.DefaultRequestHeaders.Add("Authorization", $"Bearer {response.AccessToken}");
		}
		
#if UNITY_EDITOR
		public static async Task<T> EditorRequestApi<T>(CancellationToken cancellationToken = default) where T : ApiProtocol
		{
			const string Key = "EDITOR_ACCESS_EMAIL";
			
			var email = PlayerPrefs.GetString(Key);
			if (string.IsNullOrEmpty(email))
			{
				var token = await GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets
				                                                              {
					                                                              ClientId = "517818090277-dh7nin47elvha6uhn64ihiboij7pv57p.apps.googleusercontent.com",
					                                                              ClientSecret = "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"
				                                                              },
				                                                              new[] { "email", "openid" },
				                                                              "user",
				                                                              CancellationToken.None);

				using var http = new HttpClient
				{
					DefaultRequestHeaders =
					{
						{ "Authorization", "Bearer " + token.Token.AccessToken }
					}
				};

				var request = await http.GetAsync("https://openidconnect.googleapis.com/v1/userinfo", cancellationToken);
				var userInfo = JObject.Parse(await request.Content.ReadAsStringAsync());
				await RequestEditorAccessTokenAsync($"{userInfo.GetValue("email")}");
				
				PlayerPrefs.SetString(Key, $"{userInfo.GetValue("email")}");
			}
			else
				await RequestEditorAccessTokenAsync(email);

			using var api = new ApiSingleton();
			return api.EditorGetApi<T>();
		}
		
		private static async Task RequestEditorAccessTokenAsync(string email)
		{
			RemoveAccessToken();
			
			var request = await ApiPostRequest.EditAppAccessTokenRequest(new StringRequest
			{
				Value = email.Encryption()
			});
			ApiSingleton.Http.DefaultRequestHeaders.Add("Authorization", $"Bearer {request.Response.Value}");
		}
#endif
		
		public void Dispose()
		{
#if UNITY_EDITOR
			RemoveAccessToken();
#endif
		}
	}
}