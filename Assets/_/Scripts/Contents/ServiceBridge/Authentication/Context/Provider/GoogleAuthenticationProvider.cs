using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Firebase.Auth;
using Google;
using Google.Apis.Auth.OAuth2;


#if UNITY_EDITOR
using System;
#else
#endif

namespace Redbean.Auth
{
	public enum GoogleAuthErrorCode
	{
		Success = 0,
		Error = 10000000,
		Exception = 19999999
	}
	
	public class GoogleAuthenticationProvider : IAuthentication
	{
		public static bool IsInitialize => GoogleSignIn.Configuration is not null;
		
		public AuthenticationType Type => AuthenticationType.Google;
		
		public Task<bool> Initialize(CancellationToken cancellationToken = default)
		{
			var completionSource = new TaskCompletionSource<bool>();
			
			if (GoogleSignIn.Configuration != null)
				completionSource.TrySetResult(false);
			
			var configuration = new GoogleSignInConfiguration
			{
				WebClientId = GoogleAuthenticationClient.GetWebClientId(),
				RequestEmail = true,
				RequestProfile = true,
				RequestAuthCode = true,
				RequestIdToken = true,
				HidePopups = true
			};
			GoogleSignIn.Configuration = configuration;

			completionSource.TrySetResult(true);
			return completionSource.Task;
		}

		public async Task<AuthenticationResult> Login(CancellationToken cancellationToken = default)
		{
			var result = new AuthenticationResult();
			
#if UNITY_EDITOR
			result = await UnityEditorLogin(cancellationToken);
#else
			try
			{
				var user = await GoogleSignIn.DefaultInstance.SignInAsync();
				result = new AuthenticationResult
				{
					Code = (int)GoogleAuthErrorCode.Success,
					Credential = GoogleAuthProvider.GetCredential(user.IdToken, "")
				};
			}
			catch (GoogleSignIn.SignInException e)
			{
				result = new AuthenticationResult
				{
					Code = (int)GoogleAuthErrorCode.Error + (int)e.Status,
					Message = e.Message
				};
			}
			catch (Exception e)
			{
				result = new AuthenticationResult
				{
					Code = (int)GoogleAuthErrorCode.Exception,
					Message = e.Message
				};
			}
#endif
			return result;
		}

		public async Task<AuthenticationResult> AutoLogin(CancellationToken cancellationToken = default)
		{
			var result = new AuthenticationResult();
			
#if UNITY_EDITOR
			result = await UnityEditorLogin(cancellationToken);
#else
			try
			{
				var user = await GoogleSignIn.DefaultInstance.SignInSilentlyAsync();
				result = new AuthenticationResult
				{
					Code = (int)GoogleAuthErrorCode.Success,
					Credential = GoogleAuthProvider.GetCredential(user.IdToken, "")
				};
			}
			catch (GoogleSignIn.SignInException e)
			{
				result = new AuthenticationResult
				{
					Code = (int)GoogleAuthErrorCode.Error + (int)e.Status,
					Message = e.Message
				};
			}
			catch (Exception e)
			{
				result = new AuthenticationResult
				{
					Code = (int)GoogleAuthErrorCode.Exception,
					Message = e.Message
				};
			}
#endif
			return result;
		}

#if UNITY_EDITOR
		private async Task<AuthenticationResult> UnityEditorLogin(CancellationToken cancellationToken = default)
		{
			var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets
			                                                                   {
				                                                                   ClientId = "517818090277-dh7nin47elvha6uhn64ihiboij7pv57p.apps.googleusercontent.com",
				                                                                   ClientSecret = "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"
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

			return new AuthenticationResult
			{
				Code = (int)GoogleAuthErrorCode.Success,
				Credential = GoogleAuthProvider.GetCredential(credential.Token.IdToken, "")
			};
		}
#endif
	}
}