﻿using System.Collections.Generic;
using System.Linq;
using Firebase.Auth;
using Redbean.Api;
using Redbean.MVP.Content;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Redbean.Editor
{
	internal partial class RuntimeWindow
	{
		private const string AuthenticationTab = "Authentication";
		
		private const string LoginGroup = "Tabs/Authentication/Login";
		private const string UserInformationGroup = "Tabs/Authentication/User Information";

		private const int LoginOrder = 100;
		private const int UserInformationOrder = 200;
		
		private bool isExistUser => user.Any();
		
		[TabGroup(TabGroup, AuthenticationTab), TitleGroup(LoginGroup), PropertyOrder(LoginOrder), DisableInEditorMode, Button]
		private async void UserLogin(string ID)
		{
			await this.EditorRequestApi<GetAccessTokenAndUserProtocol>(ID);
		}

		[TabGroup(TabGroup, AuthenticationTab), TitleGroup(UserInformationGroup), Button("DELETE", ButtonSizes.Large), PropertyOrder(UserInformationOrder), ShowIf(nameof(isExistUser), Value = true), PropertySpace, DisableInEditorMode]
		private async void UserDeleteAccount()
		{
			if (!AppLifeCycle.IsReady)
				return;
			
			// 유저 계정 제거 API 필요
			await FirebaseAuth.DefaultInstance.CurrentUser.DeleteAsync();
			
			PlayerPrefs.DeleteAll();
			
			EditorApplication.isPlaying = false;
			
			Log.Notice("User account has been deleted.");
		}

		[TabGroup(TabGroup, AuthenticationTab), TitleGroup(UserInformationGroup), PropertyOrder(UserInformationOrder), DisableInEditorMode, ShowInInspector]
		private Dictionary<string, object> user => AppLifeCycle.IsReady
			? new Dictionary<string, object> { { "User", this.GetModel<UserModel>() } }
			: new Dictionary<string, object>();
	}
}