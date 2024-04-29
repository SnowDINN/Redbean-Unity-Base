using R3;
using Redbean.Core;
using Redbean.Debug;
using Redbean.Rx;

namespace Redbean.MVP.Content
{
	public class GuestAuthenticationPresenter : Presenter
	{
		[View]
		private ButtonView view;

		[Singleton]
		private RxPlayerPrefsBinder rxPlayerPrefsBinder;
		
		public override void Setup()
		{
			view.Button.AsButtonObservable().Subscribe(_ =>
			{
				Account.CreateAccount();
				
				Log.Print(this.User().uid);
			}).AddTo(this);

			
			rxPlayerPrefsBinder.OnPlayerPrefsChanged
			                   .Where(_ => _.key == "USER_ACCOUNT")
			                   .Subscribe(_ =>
			                   {
				                   Log.Print($"{((AccountModel)_.value).uid}");
			                   }).AddTo(this);

			var account = this.GetLocalModel<AccountModel>("USER_ACCOUNT");
			Log.Print(account.uid);
		}
	}
}