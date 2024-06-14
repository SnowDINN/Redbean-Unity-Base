using System;
using System.Threading.Tasks;
using Firebase.Auth;
using Google;

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
		public AuthenticationType Type => AuthenticationType.Google;
		
		public Task<bool> Initialize()
		{
			var completionSource = new TaskCompletionSource<bool>();
			
			if (GoogleSignIn.Configuration != null)
				completionSource.TrySetResult(false);
			
			var configuration = new GoogleSignInConfiguration
			{
#if UNITY_EDITOR || UNITY_STANDALONE
				ClientSecret = GoogleExtension.GetWebSecretId(),
#endif
				WebClientId = GoogleExtension.GetWebClientId(),
				RequestEmail = true,
				RequestIdToken = true
			};
			GoogleSignIn.Configuration = configuration;
			GoogleSignIn.Configuration.RequestIdToken = true;
			GoogleSignIn.Configuration.UseGameSignIn = false;

			completionSource.TrySetResult(true);
			return completionSource.Task;
		}

		public async Task<AuthenticationResult> Login()
		{
			var result = new AuthenticationResult();

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

			return result;
		}

		public async Task<AuthenticationResult> AutoLogin()
		{
			var result = new AuthenticationResult();
			
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

			return result;
		}
	}
}