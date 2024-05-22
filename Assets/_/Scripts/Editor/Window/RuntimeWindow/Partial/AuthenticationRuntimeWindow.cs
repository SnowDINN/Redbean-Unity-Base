using System.Linq;
using Redbean.Firebase;
using Redbean.MVP.Content;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Redbean.Editor
{
	internal partial class RuntimeWindow
	{
		private const string Login = nameof(Login);
		private const string UserInformation = "User Information";
		
		[TabGroup(Authentication), Title(Login), PropertyOrder(0), DisableInEditorMode, Button]
		public async void UserIdLogin(string userId)
		{
			var equalTo = FirebaseCore.Firestore.Collection("users").WhereEqualTo($"{DataKey.USER_SOCIAL_KEY}.{DataKey.USER_ID_KEY}", userId);
			var userQuery = await equalTo.GetSnapshotAsync();
			if (userQuery.Any())
			{
				var user = userQuery.Documents
				                    .Select(_ => _.ConvertTo<UserModel>())
				                    .FirstOrDefault(_ => _.Social.Id == userId)
				                    .Publish();

				await user.UserIdValidate();
				await user.UserCreateAsync();
				
				Log.Print("User data has been verified.");
			}
			else
				Log.Print("User information not exists in the Server.", Color.red);
		}

		[TabGroup(Authentication), Title(UserInformation), PropertyOrder(100), DisableInEditorMode, ShowInInspector]
		public UserModel User => this.IsContainsModel<UserModel>() 
			? this.GetModel<UserModel>()
			: new UserModel();
	}
}