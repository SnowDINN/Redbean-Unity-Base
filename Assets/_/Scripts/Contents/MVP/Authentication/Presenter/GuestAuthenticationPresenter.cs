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
				InteractionAsync();
			}).AddTo(this);
		}

		private void InteractionAsync()
		{
			if (this.IsContains(typeof(UserModel).FullName))
				this.GetPlayerPrefs<UserModel>(typeof(UserModel).FullName).Publish();
			
			if (string.IsNullOrEmpty(model.Information.Nickname))
				model.Social.Platform = "Pengu";
			
			model.UserValidation();
			
			Log.Print($"User id : {model.Id}");
		}
	}
}