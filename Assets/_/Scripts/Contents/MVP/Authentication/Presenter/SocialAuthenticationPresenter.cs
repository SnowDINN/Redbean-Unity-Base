using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using Redbean.Core;
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
			if (this.IsContains(typeof(UserModel).FullName))
				this.GetPlayerPrefs<UserModel>(typeof(UserModel).FullName).Publish();
			
			if (string.IsNullOrEmpty(model.Social.Id))
				model.Social.Platform = "";
			
			if (string.IsNullOrEmpty(model.Social.Platform))
				model.Social.Platform = $"{view.Type}";
			
			if (string.IsNullOrEmpty(model.Information.Nickname))
				model.Social.Platform = "Pengu";

			var equalTo = FirebaseCore.Firestore.Collection("users").WhereEqualTo("id", model.Id);
			var user = await equalTo.GetSnapshotAsync();
			if (user.Any())
			{
				user.Documents
				    .Select(_ => _.ConvertTo<UserModel>())
				    .FirstOrDefault(_ => _.Id == model.Id)
				    .Publish();
				
				Log.Print("User information exists in the Firestore.");
			}
			else
				Log.Print("User information not exists in the Firestore. It stores local data on the server.", Color.red);
				
			await model.UserValidation().CreateUserAsync().AttachExternalCancellation(token);
			
			Log.Print($"User id : {model.Id}");	
		}
	}
}