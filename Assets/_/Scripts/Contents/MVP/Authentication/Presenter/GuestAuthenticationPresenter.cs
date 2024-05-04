using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using Redbean.Core;
using Redbean.Debug;
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
			model.AuthenticationType = AuthenticationType.Guest;
			
			if (this.IsContains(LocalKey.USER_INFO_KEY))
				this.GetPlayerPrefs<UserModel>(LocalKey.USER_INFO_KEY).Publish();
			else
				model.UserValidation();
			
			Log.Print("System", $"User id : {model.UserId}.");	
		}
	}
}