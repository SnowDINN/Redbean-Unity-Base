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
		private AuthenticationContainer authentication;
		
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
			
			await auth.Initialize(token);
			await SetUserData(await auth.Login(token));
		}

		private async UniTaskVoid AutoLoginAsync(CancellationToken token)
		{
			if (!m_user.Social.Platform.Contains($"{view.Type}".ToLower()))
				return;
			
			if (m_user.Social.Platform.Contains($"{view.Type}".ToLower()))
			{
				var auth = authentication.GetPlatform(view.Type);

				await auth.Initialize(token);
				await SetUserData(await auth.AutoLogin(token));
			}
		}

		private async UniTask SetUserData(AuthenticationResult result)
		{
			var user = await FirebaseAuth.DefaultInstance.SignInWithCredentialAsync(result.Credential);
			
			await this.GetProtocol<GetAccessTokenAndUserProtocol>().Parameter(user.UserId).RequestAsync(view.destroyCancellationToken);
			
			m_user.Information.Id = user.UserId;
			m_user.Information.Nickname = user.ProviderData.First().DisplayName;
			m_user.Social.Platform = user.ProviderData.First().ProviderId;
		}
	}
}