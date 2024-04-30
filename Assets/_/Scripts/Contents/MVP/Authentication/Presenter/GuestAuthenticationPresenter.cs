﻿using System;
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
		private AccountModel model;
		
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
			model.authenticationType = AuthenticationType.Guest;
			model.userId = $"{Guid.NewGuid()}";
			
			var isDone = await model.Publish().CreateAsync().AttachExternalCancellation(token);
			if (isDone)
				Log.Print("System", $"User ID is {model.userId}.");
		}
	}
}