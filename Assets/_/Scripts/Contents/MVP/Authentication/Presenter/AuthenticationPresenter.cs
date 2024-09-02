using System.Threading;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using R3;
using Redbean.Api;
using Redbean.Auth;
using Redbean.Rx;
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

		[Singleton]
		private AuthenticationContainer container;
		private IAuthenticationContainer platform => container.GetPlatform(view.Type);
		

		private const string LAST_LOGIN_HISTORY = nameof(LAST_LOGIN_HISTORY);
		
		public override void Setup()
		{
			view.Button
				.AsButtonObservable()
				.Subscribe(_ =>
				{
					UniTask.Void(LoginAsync, view.destroyCancellationToken);
				}).AddTo(this);

			RxApiBinder.OnApiResponse
				.Where(_ => _.type == typeof(PostAccessTokenAndUserProtocol))
				.Where(_ => _.response.ErrorCode == 0)
				.Subscribe(_ =>
				{
					$"{view.Type}".SetPlayerPrefs(LAST_LOGIN_HISTORY);
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
			if (LocalDatabase.Load<string>(LAST_LOGIN_HISTORY) != $"{view.Type}")
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
					? user.Information.Id 
					: (await FirebaseAuth.DefaultInstance.SignInWithCredentialAsync(result.Credential)).UserId
			};
			
			await this.GetProtocol<PostAccessTokenAndUserProtocol>()
				.Parameter(parameter)
				.RequestAsync(view.destroyCancellationToken);
		}
	}
}