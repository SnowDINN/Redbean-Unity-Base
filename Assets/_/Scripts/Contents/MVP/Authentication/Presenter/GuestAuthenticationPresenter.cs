using System;
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

		[Model]
		private AccountModel accountModel;

		[Singleton]
		private RxPlayerPrefsBinder rxPlayerPrefsBinder;
		
		public override void Setup()
		{
			view.Button.AsButtonObservable().Subscribe(_ =>
			{
				accountModel.userId = $"{Guid.NewGuid()}".PlayerPrefsSave(LocalKey.USER_ACCOUNT_KEY);
				accountModel.Publish();
			}).AddTo(this);
			
			accountModel.Rx.As<AccountRxModel>().UserId
			            .Where(_ => !string.IsNullOrEmpty(_))
			            .Subscribe(_ =>
			            {
				            // FirebaseCore.UserDB = FirebaseCore.Firestore.Collection("users").Document(_);
				
				            Log.Print("System", $"Your UID is {_}.");
			            }).AddTo(this);
		}
	}
}