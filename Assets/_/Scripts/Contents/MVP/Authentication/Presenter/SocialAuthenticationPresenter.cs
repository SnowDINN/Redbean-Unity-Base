using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using Redbean.Core;
using Redbean.Firebase;
using Redbean.ServiceBridge;
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
		private Authentication authentication;
		
		public override void Setup()
		{
			view.Button.AsButtonObservable().Subscribe(_ =>
			{
				UniTask.Void(LoginAsync, view.DestroyCancellation.Token);
			}).AddTo(this);
			
			UniTask.Void(AutoLoginAsync, view.DestroyCancellation.Token);
		}
		
		private async UniTaskVoid LoginAsync(CancellationToken token)
		{
			var auth = authentication.GetPlatform(view.Type);
			var isInitialize = await auth.Initialize();
			if (!isInitialize)
				return;

			var credential = await auth.Login();
			var user = await FirebaseCore.Auth.SignInWithCredentialAsync(credential.Credential);
			
			if (string.IsNullOrEmpty(model.Social.Id))
				model.Social.Id = user.UserId;
			
			if (string.IsNullOrEmpty(model.Social.Platform))
				model.Social.Platform = user.ProviderData.First().ProviderId;
			
			if (string.IsNullOrEmpty(model.Information.Nickname))
				model.Information.Nickname = user.ProviderData.First().DisplayName;

			var isFind = await FindUserSnapshot(user.UserId);
			if (isFind)
			{
				await model.UserIdValidate().AttachExternalCancellation(token);
				await model.UserCreateAsync().AttachExternalCancellation(token);
			
				Log.Print($"User id : {model.Social.Id}");	
			}
		}

		private async UniTaskVoid AutoLoginAsync(CancellationToken token)
		{
			if (this.IsContains(typeof(UserModel).FullName))
			{
				await UniTask.WaitUntil(() => ApplicationCore.IsReady, cancellationToken: token);
				
				var saveData = this.GetPlayerPrefs<UserModel>(typeof(UserModel).FullName);
				if (saveData.Social.Platform.Contains($"{view.Type}".ToLower()))
				{
					var auth = authentication.GetPlatform(view.Type);
					var isInitialize = await auth.Initialize();
					if (!isInitialize)
						return;

					var credential = await auth.AutoLogin();
					var user = await FirebaseCore.Auth.SignInWithCredentialAsync(credential.Credential);
					
					var isFind = await FindUserSnapshot(user.UserId);
					if (isFind)
					{
						await model.UserIdValidate().AttachExternalCancellation(token);
					
						Log.Print($"User id : {model.Social.Id}");	
					}
				}
			}
		}

		private async UniTask<bool> FindUserSnapshot(string id)
		{
			var equalTo = FirebaseCore.Firestore.Collection("users").WhereEqualTo($"{DataKey.USER_SOCIAL_KEY}.{DataKey.USER_ID_KEY}", id);
			var querySnapshot = await equalTo.GetSnapshotAsync();
			if (querySnapshot.Any())
			{
				querySnapshot.Documents
				             .Select(_ => _.ConvertTo<UserModel>())
				             .FirstOrDefault(_ =>  _.Social.Id == id)
				             .Publish();
				
				Log.Print("User data has been verified.");
				return true;
			}
			
			Log.Print("User information not exists in the Server.", Color.red);
			return false;
		}
	}
}