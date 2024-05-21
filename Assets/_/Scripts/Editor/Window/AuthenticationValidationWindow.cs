using System.Linq;
using Firebase.Firestore;
using Redbean.Dependencies;
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
			var equalTo = FirebaseCore.Firestore.Collection("users").WhereEqualTo("id", userId);
			var user = await equalTo.GetSnapshotAsync();
			if (user.Any())
			{
				user.Documents
				    .Select(_ => _.ConvertTo<UserModel>())
				    .FirstOrDefault(_ => _.Id == userId)
				    .Publish();
				
				Log.Print("User information exists in the Firestore.");
				Log.Print($"User id : {this.GetModel<UserModel>().Id}");	
			}
			else
				Log.Print("User information not exists in the Firestore. It stores local data on the server.", Color.red);
		}
		
		[TabGroup(Authentication), PropertyOrder(1), DisableInEditorMode, Button]
		public async void UserNicknameLogin(string userNickname)
		{
			var equalTo = FirebaseCore.Firestore.Collection("users").WhereEqualTo("user_details.nickname", userNickname);
			var user = await equalTo.GetSnapshotAsync();
			if (user.Any())
			{
				user.Documents
				    .Select(_ => _.ConvertTo<UserModel>())
				    .FirstOrDefault(_ => _.Details.Nickname == userNickname)
				    .Publish();
				
				Log.Print("User information exists in the Firestore.");
				Log.Print($"User id : {this.GetModel<UserModel>().Id}");	
			}
			else
				Log.Print("User information not exists in the Firestore. It stores local data on the server.", Color.red);
		}

		[TabGroup(Authentication), Title(UserInformation), PropertyOrder(100), DisableInEditorMode, ShowInInspector]
		public UserModel User => DependenciesModel.IsContains<UserModel>() 
			? this.GetModel<UserModel>()
			: new UserModel();
	}
}