using System.Threading;
using Cysharp.Threading.Tasks;
using R3;

namespace Redbean.MVP.Content
{
	public class GuestAuthenticationPresenter : Presenter
	{
		[Model]
		private UserModel model;
		
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
			
			if (string.IsNullOrEmpty(model.Information.Nickname))
				model.Information.Nickname = "Pengu";
			
			await model.UserIdValidate();
			
			Log.Print("User logged in as a guest.");
		}
	}
}