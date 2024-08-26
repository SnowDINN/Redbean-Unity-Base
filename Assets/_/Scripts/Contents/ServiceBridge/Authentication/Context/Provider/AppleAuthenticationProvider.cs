using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AppleAuth;
using AppleAuth.Enums;
using AppleAuth.Interfaces;
using AppleAuth.Native;
using Firebase.Auth;
using R3;
using UnityEngine;

namespace Redbean.Auth
{
	public enum AppleAuthErrorCode
	{
		Success = 0,
		Error = 11000000,
		Exception = 11999999
	}
	
	public class AppleAuthenticationProvider : IAuthenticationContainer
	{
		public AuthenticationType Type => AuthenticationType.Apple;
		public bool IsInitialize { get; set; }

		public const string APPLE_USER_ID = nameof(APPLE_USER_ID);
		public const string APPLE_USER_ID_TOKEN = nameof(APPLE_USER_ID_TOKEN);

		private string userId => PlayerPrefs.GetString(APPLE_USER_ID);
		private string userIdToken => PlayerPrefs.GetString(APPLE_USER_ID_TOKEN);
		
		private IAppleAuthManager appleAuthManager;
		private IDisposable disposable;
		
		public Task<bool> Initialize(CancellationToken cancellationToken = default)
		{
			if (IsInitialize)
				Task.FromResult(IsInitialize);

			appleAuthManager = new AppleAuthManager(new PayloadDeserializer());

			disposable?.Dispose();
			disposable = Observable.EveryUpdate()
				.Where(_ => appleAuthManager != null)
				.Subscribe(_ => appleAuthManager.Update());
			
			IsInitialize = true;
			return Task.FromResult(IsInitialize);
		}

		public Task<AuthenticationResult> Login(CancellationToken cancellationToken = default)
		{
			var completionSource = new TaskCompletionSource<AuthenticationResult>();
			var result = new AuthenticationResult();
			
			var appleAuthLoginArgs = new AppleAuthLoginArgs(LoginOptions.IncludeFullName);
			appleAuthManager.LoginWithAppleId(appleAuthLoginArgs,
			                                  credential =>
			                                  {
				                                  PlayerPrefs.SetString(APPLE_USER_ID, (credential as IAppleIDCredential).User);
				                                  PlayerPrefs.SetString(APPLE_USER_ID_TOKEN, Encoding.UTF8.GetString((credential as IAppleIDCredential).IdentityToken));

				                                  result = new AuthenticationResult
				                                  {
					                                  Code = (int)AppleAuthErrorCode.Success,
					                                  Credential = OAuthProvider.GetCredential("apple.com", userIdToken, GenerateRandomString(32))
				                                  };
				                                  completionSource.SetResult(result);
			                                  },
			                                  error =>
			                                  {
				                                  result = new AuthenticationResult
				                                  {
					                                  Code = (int)AppleAuthErrorCode.Error + error.Code,
					                                  Message = error.LocalizedFailureReason
				                                  };
				                                  completionSource.SetResult(result);
			                                  });
			
            return completionSource.Task;
		}

		public Task<AuthenticationResult> AutoLogin(CancellationToken cancellationToken = default)
		{
			var completionSource = new TaskCompletionSource<AuthenticationResult>();
			var result = new AuthenticationResult();

			appleAuthManager.GetCredentialState(userId, 
			                                    _ =>
			                                    {
				                                    result = new AuthenticationResult
				                                    {
					                                    Code = (int)AppleAuthErrorCode.Success,
					                                    Credential = OAuthProvider.GetCredential("apple.com", userIdToken, GenerateRandomString(32))
				                                    };
				                                    completionSource.SetResult(result);
			                                    }, 
			                                    error =>
			                                    {
				                                    result = new AuthenticationResult
				                                    {
					                                    Code = (int)AppleAuthErrorCode.Error + error.Code,
					                                    Message = error.LocalizedFailureReason
				                                    };
				                                    completionSource.SetResult(result);
			                                    });
			
			return completionSource.Task;
		}
		
		private string GenerateRandomString(int length)
		{
			const string charset = "0123456789ABCDEFGHIJKLMNOPQRSTUVXYZabcdefghijklmnopqrstuvwxyz-._";
			var cryptographicallySecureRandomNumberGenerator = new RNGCryptoServiceProvider();
			var result = string.Empty;
			var remainingLength = length;

			var randomNumberHolder = new byte[1];
			while (remainingLength > 0)
			{
				var randomNumbers = new List<int>(16);
				for (var randomNumberCount = 0; randomNumberCount < 16; randomNumberCount++) {
					cryptographicallySecureRandomNumberGenerator.GetBytes(randomNumberHolder);
					randomNumbers.Add(randomNumberHolder[0]);
				}

				foreach (var randomNumber in randomNumbers.TakeWhile(_ => remainingLength != 0).Where(randomNumber => randomNumber < charset.Length))
				{
					result += charset[randomNumber];
					remainingLength--;
				}
			}

			return result;
		}

		public void Dispose()
		{
			disposable?.Dispose();
		}
	}
}