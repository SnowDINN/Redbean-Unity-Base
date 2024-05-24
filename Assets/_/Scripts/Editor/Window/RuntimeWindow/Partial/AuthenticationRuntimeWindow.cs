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
		private const string LoginTitle = "Login";
		private const string UserInformationTitle = "User Information";

		private bool isExistUser => !string.IsNullOrEmpty(user.Social.Id);
		
		[TabGroup(AuthenticationTab), Title(UserInformationTitle), PropertyOrder(100), DisableInEditorMode, ShowInInspector]
		private UserModel user => ApplicationSetup.IsReady
			? this.GetModel<UserModel>()
			: new UserModel();
		
		[TabGroup(AuthenticationTab), Title(LoginTitle), PropertyOrder(0), DisableInEditorMode, Button]
		private async void UserLogin(string ID)
		{
			var isSuccess = await user.TryGetUserSnapshot(ID);
			if (isSuccess)
				user.SetReferenceUser();
		}

		[TabGroup(AuthenticationTab), Button("DELETE", ButtonSizes.Large), PropertyOrder(101), ShowIf("isExistUser", Value = true), PropertySpace, DisableInEditorMode]
		private async void UserDeleteAccount()
		{
			if (!ApplicationSetup.IsReady)
				return;

			user.SetReferenceUser();
			
			await FirebaseSetup.UserDB.DeleteAsync();
			await FirebaseSetup.Auth.CurrentUser.DeleteAsync();
			
			PlayerPrefs.DeleteAll();
			
			EditorApplication.isPlaying = false;
			
			Log.Notice("User account has been deleted.");
		}
	}
}