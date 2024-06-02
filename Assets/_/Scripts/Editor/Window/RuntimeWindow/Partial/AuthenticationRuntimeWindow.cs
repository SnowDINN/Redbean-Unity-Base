﻿using Redbean.Container;
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
		
		[TabGroup("Tabs", AuthenticationTab), Title(UserInformationTitle), PropertyOrder(100), DisableInEditorMode, ShowInInspector]
		private UserModel user => ApplicationLifeCycle.IsReady
			? this.GetModel<UserModel>()
			: new UserModel();
		
		[TabGroup("Tabs", AuthenticationTab), Title(LoginTitle), PropertyOrder(0), DisableInEditorMode, Button]
		private async void UserLogin(string ID)
		{
			var isSuccess = await user.TryGetUserSnapshot(ID);
			if (isSuccess)
				user.SetReferenceUser();
		}

		[TabGroup("Tabs", AuthenticationTab), Button("DELETE", ButtonSizes.Large), PropertyOrder(101), ShowIf(nameof(isExistUser), Value = true), PropertySpace, DisableInEditorMode]
		private async void UserDeleteAccount()
		{
			if (!ApplicationLifeCycle.IsReady)
				return;

			user.SetReferenceUser();
			
			await FirebaseContainer.UserDB.DeleteAsync();
			await FirebaseContainer.Auth.CurrentUser.DeleteAsync();
			
			PlayerPrefs.DeleteAll();
			
			EditorApplication.isPlaying = false;
			
			Log.Notice("User account has been deleted.");
		}
	}
}