using System.Threading.Tasks;
using Firebase.Auth;
using Google;

namespace Redbean.Auth
{
	public enum GoogleAuthErrorCode
	{
		Success = 0,
		GoogleLoginCancelled = 10000000
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
			
			var request = await GoogleSignIn.DefaultInstance.SignIn().AwaitCompleted();
			if (request.code > 0)
			{
				// 로그인 취소
				if (request.code == 2)
				{
					result = new AuthenticationResult
					{
						Code = (int)GoogleAuthErrorCode.GoogleLoginCancelled,
						Message = "Google sign-in has been cancelled."
					};
				}
				else
				{
					result = new AuthenticationResult
					{
						Code = (int)GoogleAuthErrorCode.GoogleLoginCancelled + request.code,
						Message = "An unknown error occurred while signing in to Google."
					};
				}
			}
			else
			{
				result = new AuthenticationResult
				{
					Code = (int)GoogleAuthErrorCode.Success,
					Credential = GoogleAuthProvider.GetCredential(request.user.IdToken, "")
				};
			}

			return result;
		}

		public async Task<AuthenticationResult> AutoLogin()
		{
			var result = new AuthenticationResult();
			
			var request = await GoogleSignIn.DefaultInstance.SignInSilently().AwaitCompleted();
			if (request.code > 0)
			{
				// 로그인 취소
				if (request.code == 2)
				{
					result = new AuthenticationResult
					{
						Code = (int)GoogleAuthErrorCode.GoogleLoginCancelled,
						Message = "Google sign-in has been cancelled."
					};
				}
				else
				{
					result = new AuthenticationResult
					{
						Code = (int)GoogleAuthErrorCode.GoogleLoginCancelled + request.code,
						Message = "An unknown error occurred while signing in to Google."
					};
				}
			}
			else
			{
				result = new AuthenticationResult
				{
					Code = (int)GoogleAuthErrorCode.Success,
					Credential = GoogleAuthProvider.GetCredential(request.user.IdToken, "")
				};
			}

			return result;
		}
	}
}