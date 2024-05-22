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
				model.Social.Id = "";
			
			if (string.IsNullOrEmpty(model.Social.Platform))
				model.Social.Platform = $"{view.Type}";
			
			if (string.IsNullOrEmpty(model.Information.Nickname))
				model.Information.Nickname = "Pengu";

			var equalTo = FirebaseCore.Firestore.Collection("users").WhereEqualTo($"{DataKey.USER_SOCIAL_KEY}.{DataKey.USER_ID_KEY}", model.Social.Id);
			var user = await equalTo.GetSnapshotAsync();
			if (user.Any())
			{
				user.Documents
				    .Select(_ => _.ConvertTo<UserModel>())
				    .FirstOrDefault(_ => _.Social.Id == model.Social.Id)
				    .Publish();
				
				Log.Print("User data has been verified.");
			}
			else
				Log.Print("User information not exists in the Server. It stores local data on the server.", Color.red);

			await model.UserIdValidate().AttachExternalCancellation(token);
			await model.UserCreateAsync().AttachExternalCancellation(token);
			
			Log.Print($"User id : {model.Social.Id}");	
		}
	}
}