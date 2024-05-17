using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using Redbean.Core;
using Redbean.Debug;
using Redbean.Firebase;
using Redbean.Rx;
using UnityEngine;

namespace Redbean.MVP.Content
{
	public class SocialAuthenticationPresenter : Presenter
	{
		[Model]
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
				this.GetPlayerPrefs<UserModel>(LocalKey.USER_INFO_KEY).Publish();
			else
			{
				if (!string.IsNullOrEmpty(view.InputField.text))
					model.UserId = view.InputField.text;
			}

			var equalTo = FirebaseCore.Firestore.Collection("users").WhereEqualTo("id", model.UserId);
			var user = await equalTo.GetSnapshotAsync();
			if (user.Any())
			{
				user.Documents
				    .Select(_ => _.ConvertTo<UserModel>())
				    .FirstOrDefault(_ => _.UserId == model.UserId)
				    .Publish();
				
				Log.Print("System", "User information exists in the Firestore.");
			}
			else
				Log.Print("System", "User information not exists in the Firestore. It stores local data on the server.", Color.red);
				
			await model.UserValidation().CreateUserAsync().AttachExternalCancellation(token);
			
			Log.Print("System", $"User id : {model.UserId}");	
		}
	}
}