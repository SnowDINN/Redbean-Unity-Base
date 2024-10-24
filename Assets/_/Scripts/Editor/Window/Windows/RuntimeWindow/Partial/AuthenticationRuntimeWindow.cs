using System.Collections.Generic;
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
		
		[TabGroup(TabGroup, AuthenticationTab), TitleGroup(LoginGroup), PropertyOrder(LoginOrder), Button, DisableInEditorMode]
		private async void UserLogin(string ID)
		{
			await this.EditorGetApi<PostAccessTokenAndUserProtocol>().Parameter(ID).RequestAsync();
		}

		[TabGroup(TabGroup, AuthenticationTab), TitleGroup(UserInformationGroup), PropertyOrder(UserInformationOrder)]
		[Button("DELETE", ButtonSizes.Large), ShowIf(nameof(isExistUser), Value = true), PropertySpace, DisableInEditorMode]
		private async void UserDeleteAccount()
		{
			await this.EditorGetApi<PostUserWithdrawalProtocol>()
				.Parameter(this.GetModel<UserModel>().Database.Social.Platform)
				.RequestAsync();
			
			EditorApplication.isPlaying = false;
			
			Log.Notice("User account has been deleted.");
		}

		[TabGroup(TabGroup, AuthenticationTab), TitleGroup(UserInformationGroup), PropertyOrder(UserInformationOrder)]
		[DictionaryDrawerSettings(IsReadOnly = true), ShowInInspector, DisableInEditorMode]
		private Dictionary<string, object> user
		{
			get => ApplicationLifeCycle.IsAppReady
				? new Dictionary<string, object> { { "User", this.GetModel<UserModel>() } }
				: new Dictionary<string, object>();
			
			set { }
		}
	}
}