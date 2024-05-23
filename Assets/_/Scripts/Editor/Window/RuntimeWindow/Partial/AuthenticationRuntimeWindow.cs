using System.Linq;
using Redbean.Firebase;
using Redbean.MVP.Content;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Redbean.Editor
{
	internal partial class RuntimeWindow
	{
		private const string Login = nameof(Login);
		private const string UserInformation = "User Information";

		private bool isExistUser => !string.IsNullOrEmpty(User.Social.Id);
		
		[TabGroup(Authentication), Title(Login), PropertyOrder(0), DisableInEditorMode, Button]
		private async void UserIdLogin(string id)
		{
			var equalTo = FirebaseSetup.Firestore.Collection("users").WhereEqualTo($"{DataKey.USER_SOCIAL_KEY}.{DataKey.USER_ID_KEY}", id);
			var userQuery = await equalTo.GetSnapshotAsync();
			if (userQuery.Any())
			{
				var user = userQuery.Documents
				                    .Select(_ => _.ConvertTo<UserModel>())
				                    .FirstOrDefault(_ => _.Social.Id == id)
				                    .Publish();

				await user.UserIdValidate();
				
				Log.Print("User data has been verified.");
			}
			else
				Log.Print("User information not exists in the Server.", Color.red);
		}

		[TabGroup(Authentication), Title(UserInformation), PropertyOrder(100), DisableInEditorMode, ShowInInspector]
		private UserModel User => this.IsContainsModel<UserModel>()
			? this.GetModel<UserModel>()
			: new UserModel();

		[TabGroup(Authentication), Button(ButtonSizes.Large, ButtonStyle.Box), PropertyOrder(101), ShowIf("isExistUser", Value = true), PropertySpace, DisableInEditorMode]
		private async void UserDeleteAccount()
		{
			if (!this.IsContainsModel<UserModel>())
				return;

			await User.UserIdValidate();
			
			await FirebaseSetup.UserDB.DeleteAsync();
			await FirebaseSetup.Auth.CurrentUser.DeleteAsync();
			
			PlayerPrefs.DeleteAll();
			
			EditorApplication.isPlaying = false;
			
			Log.Notice("User account has been deleted.");
		}
	}
}