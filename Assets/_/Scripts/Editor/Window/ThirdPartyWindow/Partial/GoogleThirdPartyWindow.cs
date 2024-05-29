﻿using Google;
using Sirenix.OdinInspector;

namespace Redbean.Editor
{
	internal partial class ThirdPartyWindow
	{
		private const string ClientTitle = "Client key settings";
		private const string OnlyEditorTitle = "Use only in the editor";

		[TabGroup(GoogleTab), LabelText("Android Key"), Title(ClientTitle), InlineButton(nameof(GetAosClientKey), "GET"), ShowInInspector]
		private string AndroidClientKey
		{
			get => googleSdk.androidClientId;
			set
			{
				googleSdk.androidClientId = value;
				googleSdk.Save();
			}
		}

		[TabGroup(GoogleTab), LabelText("iOS Key"), InlineButton(nameof(GetIosClientKey), "GET"), ShowInInspector]
		private string IosClientKey
		{
			get => googleSdk.iosClientId;
			set
			{
				googleSdk.iosClientId = value;
				googleSdk.Save();
			}
		}

		[TabGroup(GoogleTab), LabelText("Web Key"), InlineButton(nameof(GetWebClientKey), "GET"), ShowInInspector]
		private string WebClientKey
		{
			get => googleSdk.webClientId;
			set
			{
				googleSdk.webClientId = value;
				googleSdk.Save();
			}
		}

		[TabGroup(GoogleTab), LabelText("Web Secret"), Title(OnlyEditorTitle), ShowInInspector]
		private string WebSecretKey
		{
			get => googleSdk.webSecretId;
			set
			{
				googleSdk.webSecretId = value;
				googleSdk.Save();
			}
		}
		
		[TabGroup(GoogleTab), ShowInInspector]
		private int Port
		{
			get => googleSdk.webRedirectPort;
			set
			{
				googleSdk.webRedirectPort = value;
				googleSdk.Save();
			}
		}

		private void GetAosClientKey() => AndroidClientKey = googleSdk.GetClientId(ClientType.AndroidClientId);

		private void GetIosClientKey() => IosClientKey = googleSdk.GetClientId(ClientType.IosClientId);

		private void GetWebClientKey() => WebClientKey = googleSdk.GetClientId(ClientType.WebClientId);
	}
}