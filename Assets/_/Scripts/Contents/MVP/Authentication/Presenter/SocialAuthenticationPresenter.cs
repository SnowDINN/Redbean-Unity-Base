using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using Redbean.ServiceBridge;

namespace Redbean.MVP.Content
{
	public class SocialAuthenticationPresenter : Presenter
	{
		[Model]
		private UserModel m_user;
		
		[View]
		private SocialAuthenticationView view;

		[Singleton]
		private AuthenticationSingleton authentication;
		
		public override void Setup()
		{
			view.Button.AsButtonObservable().Subscribe(_ =>
			{
				UniTask.Void(LoginAsync, view.DestroyCancellation.Token);
			}).AddTo(this);
			
			UniTask.Void(AutoLoginAsync, view.DestroyCancellation.Token);
		}
		
		private async UniTaskVoid LoginAsync(CancellationToken token)
		{
			var auth = authentication.GetPlatform(view.Type);
			var isInitialize = await auth.Initialize();
			if (!isInitialize)
				return;

			var credential = await auth.Login();
			var user = await Extension.Auth.SignInWithCredentialAsync(credential.Credential);
			
			m_user.Social.Id = user.UserId;
			m_user.Social.Platform = user.ProviderData.First().ProviderId;
			m_user.Information.Nickname = user.ProviderData.First().DisplayName;

			var isExist = await m_user.TryGetUserSnapshot(user.UserId);
			if (isExist)
				m_user.SetReferenceUser();
			else
			{
				var isCreated = await m_user.UserCreateAsync();
				if (isCreated)
					Log.Print($"Created a new user's data. [ {m_user.Information.Nickname} | {m_user.Social.Id} ]");
			}
		}

		private async UniTaskVoid AutoLoginAsync(CancellationToken token)
		{
			if (!m_user.Social.Platform.Contains($"{view.Type}".ToLower()))
				return;
			
			await UniTask.WaitUntil(() => ApplicationLifeCycle.IsReady, cancellationToken: token);
				
			if (m_user.Social.Platform.Contains($"{view.Type}".ToLower()))
			{
				var auth = authentication.GetPlatform(view.Type);
				var isInitialize = await auth.Initialize();
				if (!isInitialize)
					return;

				var credential = await auth.AutoLogin();
				var user = await Extension.Auth.SignInWithCredentialAsync(credential.Credential);
					
				var isSuccess = await m_user.TryGetUserSnapshot(user.UserId);
				if (isSuccess)
					m_user.SetReferenceUser();
			}
				
		}
	}
}