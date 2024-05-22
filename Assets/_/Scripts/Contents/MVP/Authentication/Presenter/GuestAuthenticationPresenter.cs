using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using Redbean.Core;
using Redbean.Rx;

namespace Redbean.MVP.Content
{
	public class GuestAuthenticationPresenter : Presenter
	{
		[Model]
		private UserModel model;
		
		[View]
		private ButtonView view;

		[Singleton]
		private RxPlayerPrefsBinder rxPlayerPrefsBinder;
		
		public override void Setup()
		{
			view.Button.AsButtonObservable().Subscribe(_ =>
			{
				UniTask.Void(InteractionAsync, view.DestroyCancellation.Token);
			}).AddTo(this);
		}

		private async UniTaskVoid InteractionAsync(CancellationToken token)
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