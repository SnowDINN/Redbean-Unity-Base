using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using Redbean.Core;
using Redbean.Debug;
using Redbean.Firebase;
using Redbean.Rx;

namespace Redbean.MVP.Content
{
	public class SocialAuthenticationPresenter : Presenter
	{
		[Model]
		private AccountModel model;
		
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
			model.authenticationType = view.Type;
			model.userId = $"{Guid.NewGuid()}";
			
			FirebaseCore.UserDB = FirebaseCore.Firestore.Collection("users").Document(model.userId);
			
			var isDone = await model.Publish().CreateAsync().AttachExternalCancellation(token);
			if (isDone)
				Log.Print("System", $"User ID is {model.userId}.");
		}
	}
}