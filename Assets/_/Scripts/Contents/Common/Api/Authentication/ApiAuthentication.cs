using System;
using System.Threading.Tasks;
using Firebase.Auth;
using Redbean.Auth;
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
		public static async Task<object> EditorRequestApi<T>(params object[] args) where T : IApiContainer
		{
			const string Key = "EDITOR_ACCESS_EMAIL";
			
			using var api = new ApiContainer();
			await api.Setup();
			
			var email = PlayerPrefs.GetString(Key);
			if (string.IsNullOrEmpty(email))
			{
				var authenticationProvider = new GoogleAuthenticationProvider();
				if (!GoogleAuthenticationProvider.IsInitialize)
					await authenticationProvider.Initialize();

				var authenticationResult = await authenticationProvider.Login();
				var user = await FirebaseAuth.DefaultInstance.SignInWithCredentialAsync(authenticationResult.Credential);
				
				await RequestEditorAccessTokenAsync(user.Email);
				
				PlayerPrefs.SetString(Key, user.Email);
			}
			else
				await RequestEditorAccessTokenAsync(email);

			return await api.EditorRequestApi<T>(args);
		}
		
		private static async Task RequestEditorAccessTokenAsync(string email)
		{
			var request = await ApiPostRequest.PostAppAccessTokenRequest(new StringRequest
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