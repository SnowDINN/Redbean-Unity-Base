﻿using System.Threading;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using R3;
using Redbean.Api;
using Redbean.Auth;
using Redbean.Singleton;
using Redbean.Utility;

namespace Redbean.MVP.Content
{
	public class AuthenticationPresenter : Presenter
	{
		[Model]
		private UserModel user;
		
		[View]
		private AuthenticationView view;
		
		private IAuthentication platform => AuthenticationContainer.GetPlatform(view.Type);
		
		public override void Setup()
		{
			view.Button
				.AsButtonObservable()
				.Subscribe(_ =>
				{
					UniTask.Void(LoginAsync, view.destroyCancellationToken);
				}).AddTo(this);

			UniTask.Void(AutoLoginAsync, view.destroyCancellationToken);
		}
		
		private async UniTaskVoid LoginAsync(CancellationToken token)
		{
			await platform.Initialize(token);
			await SetUserData(await platform.Login(token));
		}

		private async UniTaskVoid AutoLoginAsync(CancellationToken token)
		{
			if (LocalDatabase.Load<string>(PlayerPrefsKey.LAST_LOGIN_HISTORY) != $"{view.Type}")
				return;

			await platform.Initialize(token);
			await SetUserData(await platform.AutoLogin(token));
		}

		private async UniTask SetUserData(AuthenticationResult result)
		{
			var parameter = new AuthenticationRequest
			{
				type = view.Type,
				id = view.Type == AuthenticationType.Guest 
					? user.Database.Information.Id
					: (await FirebaseAuth.DefaultInstance.SignInWithCredentialAsync(result.Credential)).UserId
			};
			
			var response = await this.GetProtocol<PostAccessTokenAndUserProtocol>()
				.Parameter(parameter)
				.RequestAsync(view.destroyCancellationToken);

			if (response.IsSuccess)
			{
				if (view.Type == AuthenticationType.Guest)
					user.Database.Information.Id.SetPlayerPrefs(PlayerPrefsKey.GUEST_USER_ID);
				
				$"{view.Type}".SetPlayerPrefs(PlayerPrefsKey.LAST_LOGIN_HISTORY);
			}
		}
	}
}