using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using R3;
using Redbean.Api;
using Redbean.Auth;
using Redbean.Singleton;

namespace Redbean.MVP.Content
{
	public class SocialAuthenticationPresenter : Presenter
	{
		[Model]
		private UserModel m_user;
		
		[View]
		private SocialAuthenticationView view;

		[Singleton]
		private AuthenticationSingletonContainer authentication;
		
		public override void Setup()
		{
			view.Button.AsButtonObservable().Subscribe(_ =>
			{
				UniTask.Void(LoginAsync, view.destroyCancellationToken);
			}).AddTo(this);
			
			UniTask.Void(AutoLoginAsync, view.destroyCancellationToken);
		}
		
		private async UniTaskVoid LoginAsync(CancellationToken token)
		{
			var auth = authentication.GetPlatform(view.Type);
			var isInitialize = await auth.Initialize();
			if (!isInitialize)
				return;
			
			await SetUserData(await auth.Login());
		}

		private async UniTaskVoid AutoLoginAsync(CancellationToken token)
		{
			if (!m_user.Response.Social.Platform.Contains($"{view.Type}".ToLower()))
				return;
			
			await UniTask.WaitUntil(() => AppLifeCycle.IsReady, cancellationToken: token);
				
			if (m_user.Response.Social.Platform.Contains($"{view.Type}".ToLower()))
			{
				var auth = authentication.GetPlatform(view.Type);
				var isInitialize = await auth.Initialize();
				if (!isInitialize)
					return;
				
				await SetUserData(await auth.AutoLogin());
			}
		}

		private async UniTask SetUserData(AuthenticationResult result)
		{
			var user = await FirebaseAuth.DefaultInstance.SignInWithCredentialAsync(result.Credential);
			
			await this.RequestApi<GetAccessTokenAndUserProtocol>(user.UserId);
			
			m_user.Response.Social.Id = user.UserId;
			m_user.Response.Social.Platform = user.ProviderData.First().ProviderId;
			m_user.Response.Information.Nickname = user.ProviderData.First().DisplayName;
		}
	}
}