using System.Linq;
using Redbean.Firebase;
using Redbean.MVP.Content;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Redbean.Editor
{
	internal partial class RuntimeWindow
	{
		private const string Login = "Login";
		private const string UserInformation = "User Information";
		
		[TabGroup(Authentication), Title(Login), PropertyOrder(0), DisableInEditorMode, Button]
		public async void UserIdLogin(string userId)
		{
			var equalTo = FirebaseCore.Firestore.Collection("users").WhereEqualTo(DataKey.USER_ID_KEY, userId);
			var userQuery = await equalTo.GetSnapshotAsync();
			if (userQuery.Any())
			{
				var user = userQuery.Documents
				                    .Select(_ => _.ConvertTo<UserModel>())
				                    .FirstOrDefault(_ => _.Id == userId)
				                    .Publish();

				await user.UserValidation().CreateUserAsync();
				
				Log.Print("User information exists in the Firestore.");
				Log.Print($"User id : {user.Id}");	
			}
			else
				Log.Print("User information not exists in the Firestore. It stores local data on the server.", Color.red);
		}
		
		[TabGroup(Authentication), PropertyOrder(1), DisableInEditorMode, Button]
		public async void UserSocialIdLogin(string userSocialId)
		{
			var equalTo = FirebaseCore.Firestore.Collection("users").WhereEqualTo($"{DataKey.USER_SOCIAL_KEY}.{DataKey.USER_ID_KEY}", userSocialId);
			var userQuery = await equalTo.GetSnapshotAsync();
			if (userQuery.Any())
			{
				var user = userQuery.Documents
				                    .Select(_ => _.ConvertTo<UserModel>())
				                    .FirstOrDefault(_ => _.Social.Id == userSocialId)
				                    .Publish();

				await user.UserValidation().CreateUserAsync();
				
				Log.Print("User information exists in the Firestore.");
				Log.Print($"User id : {user.Id}");	
			}
			else
				Log.Print("User information not exists in the Firestore. It stores local data on the server.", Color.red);
		}

		[TabGroup(Authentication), Title(UserInformation), PropertyOrder(100), DisableInEditorMode, ShowInInspector]
		public UserModel User => this.IsContains<UserModel>() 
			? this.GetModel<UserModel>()
			: new UserModel();
	}
}