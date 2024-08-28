﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Redbean.Api;
using Redbean.MVP.Content;
using Redbean.Utility;

namespace Redbean.Auth
{
	public class GuestAuthenticationProvider : IAuthenticationContainer
	{
		public AuthenticationType Type => AuthenticationType.Guest;
		
		public bool IsInitialize { get; set; }

		private const string GUEST_USER = nameof(GUEST_USER);
		
		public Task<bool> Initialize(CancellationToken cancellationToken = default)
		{
			return Task.FromResult(true);
		}

		public Task<AuthenticationResult> Login(CancellationToken cancellationToken = default)
		{
			var completionSource = new TaskCompletionSource<AuthenticationResult>();
			var result = new AuthenticationResult();

			var user = new UserModel
			{
				Information =
				{
					Id = $"{Guid.NewGuid()}".Replace("-", "")
				},
				Social =
				{
					Platform = "Guest"
				}
			};
			user.Override().SetPlayerPrefs(GUEST_USER);
			
			result = new AuthenticationResult
			{
				Code = (int)AppleAuthErrorCode.Success,
				Credential = null
			};
			completionSource.SetResult(result);

			return completionSource.Task;
		}

		public Task<AuthenticationResult> AutoLogin(CancellationToken cancellationToken = default)
		{
			var completionSource = new TaskCompletionSource<AuthenticationResult>();
			var result = new AuthenticationResult();

			var user = LocalDatabase.Load<UserModel>(GUEST_USER);
			user?.Override();

			result = new AuthenticationResult
			{
				Code = (int)AppleAuthErrorCode.Success,
				Credential = null
			};
			completionSource.SetResult(result);
			
			return completionSource.Task;
		}
	}
}