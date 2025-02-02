﻿using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Google;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Redbean.Api
{
	public class ApiToken
	{
		private static TokenResponse currentToken = new();
		public static bool IsAccessTokenExpired => currentToken.AccessTokenExpire < DateTime.UtcNow;
		public static bool IsRefreshTokenExpired => currentToken.RefreshTokenExpire < DateTime.UtcNow;
		public static bool IsRefreshTokenExist => !string.IsNullOrEmpty(currentToken.RefreshToken);
		public static string RefreshToken => currentToken.RefreshToken;
		
		public static void RemoveAccessToken()
		{
			if (ApiContainer.Http.DefaultRequestHeaders.Contains("Authorization"))
				ApiContainer.Http.DefaultRequestHeaders.Remove("Authorization");
		}

		public static void SetAccessToken(TokenResponse response)
		{
			currentToken = response;
			
			RemoveAccessToken();
			ApiContainer.Http.DefaultRequestHeaders.Add("Authorization", $"Bearer {response.AccessToken}");
		}
		
#if UNITY_EDITOR
		public static async Task<T> EditorRequestApi<T>(CancellationToken cancellationToken = default) where T : ApiProtocol
		{
			const string Key = "EDITOR_ACCESS_EMAIL";
			
			var email = PlayerPrefs.GetString(Key);
			if (string.IsNullOrEmpty(email))
			{
				var settings = Resources.Load<GoogleAuthScriptable>(nameof(GoogleAuthScriptable));
				var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets
				                                                              {
					                                                              ClientId = settings.desktopClientId,
					                                                              ClientSecret = settings.desktopClientSecret
				                                                              },
				                                                              new[] { "email", "openid" },
				                                                              "user",
				                                                              cancellationToken);
				var accessToken = await credential.GetAccessTokenForRequestAsync(cancellationToken: cancellationToken);
				
				using var http = new HttpClient
				{
					DefaultRequestHeaders =
					{
						{ "Authorization", "Bearer " + accessToken }
					}
				};

				var request = await http.GetAsync("https://openidconnect.googleapis.com/v1/userinfo", cancellationToken);
				var userInfo = JObject.Parse(await request.Content.ReadAsStringAsync());
				await RequestEditorAccessTokenAsync($"{userInfo.GetValue("email")}");
				
				PlayerPrefs.SetString(Key, $"{userInfo.GetValue("email")}");
			}
			else
				await RequestEditorAccessTokenAsync(email);

			return ApiContainer.GetProtocol<T>();
		}
		
		private static async Task RequestEditorAccessTokenAsync(string email)
		{
			RemoveAccessToken();
			
			var request = await ApiPostRequest.EditAppAccessTokenRequest(new StringRequest
			{
				Value = email.Encryption()
			});
			ApiContainer.Http.DefaultRequestHeaders.Add("Authorization", $"Bearer {request.Response.Value}");
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