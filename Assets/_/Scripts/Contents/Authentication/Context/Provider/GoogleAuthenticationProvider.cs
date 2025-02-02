﻿using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Firebase.Auth;
using Google;
using Google.Apis.Auth.OAuth2;
using Redbean.Api;
using UnityEngine;

#if UNITY_EDITOR
#else
using System;
#endif

namespace Redbean.Auth
{
	public enum GoogleAuthErrorCode
	{
		Success = 0,
		Error = 10000000,
		Exception = 10999999
	}
	
	public class GoogleAuthenticationProvider : IAuthentication
	{
		public AuthenticationType Type => AuthenticationType.Google;
		public bool IsInitialize { get; set; }
		
		public Task<bool> Initialize(CancellationToken cancellationToken = default)
		{
			if (IsInitialize)
				Task.FromResult(IsInitialize);
			
			var configuration = new GoogleSignInConfiguration
			{
				WebClientId = GoogleAuthClient.GetWebClientId(),
				RequestEmail = true,
				RequestProfile = true,
				RequestAuthCode = true,
				RequestIdToken = true,
				HidePopups = true
			};
			GoogleSignIn.Configuration = configuration;

			IsInitialize = true;
			return Task.FromResult(IsInitialize);
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

			return new AuthenticationResult
			{
				Code = (int)GoogleAuthErrorCode.Success,
				Credential = GoogleAuthProvider.GetCredential(credential.Token.IdToken, "")
			};
		}
#endif
	}
}