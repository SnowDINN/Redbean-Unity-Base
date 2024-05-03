using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Firebase.Firestore;
using R3;
using Redbean.Core;
using Redbean.Debug;
using Redbean.Firebase;
using Redbean.Rx;

namespace Redbean.MVP.Content
{
	public class SocialAuthenticationPresenter : Presenter
	{
		[Model(SubscribeType.Subscribe)]
		private UserModel model;
		
		[View]
		private SocialAuthenticationView view;

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
			model.AuthenticationType = view.Type;
			
			if (this.IsContains(LocalKey.USER_INFO_KEY))
			{
				this.GetPlayerPrefs<UserModel>(LocalKey.USER_INFO_KEY).Publish();
				Log.Print("System", $"User ID is {model.UserId}.");	
			}
			else
			{
				model.UserId = $"{Guid.NewGuid()}";
				
				FirebaseCore.UserDB = FirebaseCore.Firestore.Collection("users").Document(model.UserId);
			
				var isDone = await model.Publish().CreateAsync().AttachExternalCancellation(token);
				if (isDone)
					Log.Print("System", $"User ID is {model.UserId}.");
			}

			var user = FirebaseCore.Firestore.Collection("users").WhereIn(new FieldPath(model.UserId), null);
			if (user == null)
				await this.GetPlayerPrefs<UserModel>(LocalKey.USER_INFO_KEY).Publish().CreateAsync().AttachExternalCancellation(token);
		}
	}
}