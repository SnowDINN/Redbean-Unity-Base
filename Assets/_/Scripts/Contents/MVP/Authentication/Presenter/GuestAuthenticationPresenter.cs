using System.Threading;
using Cysharp.Threading.Tasks;
using R3;

namespace Redbean.MVP.Content
{
	public class GuestAuthenticationPresenter : Presenter
	{
		[Model]
		private UserModel m_user;
		
		[View]
		private ButtonView view;
		
		public override void Setup()
		{
			view.Button.AsButtonObservable().Subscribe(_ =>
			{
				UniTask.Void(LoginAsync, view.DestroyCancellation.Token);
			}).AddTo(this);
		}

		private async UniTaskVoid LoginAsync(CancellationToken token)
		{
			if (this.IsContains(typeof(UserModel).FullName))
				this.GetPlayerPrefs<UserModel>(typeof(UserModel).FullName).Publish();
			
			if (string.IsNullOrEmpty(m_user.Information.Nickname))
				m_user.Information.Nickname = "Guest";
			
			m_user.SetReferenceUser();
			
			Log.Print("User logged in as a guest.");
		}
	}
}