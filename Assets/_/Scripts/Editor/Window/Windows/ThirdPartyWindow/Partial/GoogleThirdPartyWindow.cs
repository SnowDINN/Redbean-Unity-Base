using Google;
using Sirenix.OdinInspector;

namespace Redbean.Editor
{
	internal partial class ThirdPartyWindow
	{
		private const string GoogleTab = "Google";
		
		private const string ClientGroup = "Tabs/Google/Client key settings";
		private const string OnlyEditorGroup = "Tabs/Google/Use only in the editor";

		[TabGroup(TabGroup, GoogleTab), TitleGroup(ClientGroup), LabelText("Android Key"), InlineButton(nameof(GetAosClientKey), "GET"), ShowInInspector]
		private string AndroidClientKey
		{
			get => googleSdk.androidClientId;
			set
			{
				googleSdk.androidClientId = value;
				googleSdk.Save();
			}
		}

		[TabGroup(TabGroup, GoogleTab), TitleGroup(ClientGroup), LabelText("iOS Key"), InlineButton(nameof(GetIosClientKey), "GET"), ShowInInspector]
		private string IosClientKey
		{
			get => googleSdk.iosClientId;
			set
			{
				googleSdk.iosClientId = value;
				googleSdk.Save();
			}
		}

		[TabGroup(TabGroup, GoogleTab), TitleGroup(ClientGroup), LabelText("Web Key"), InlineButton(nameof(GetWebClientKey), "GET"), ShowInInspector]
		private string WebClientKey
		{
			get => googleSdk.webClientId;
			set
			{
				googleSdk.webClientId = value;
				googleSdk.Save();
			}
		}

		[TabGroup(TabGroup, GoogleTab), TitleGroup(OnlyEditorGroup), LabelText("Web Secret"), ShowInInspector]
		private string WebSecretKey
		{
			get => googleSdk.webClientSecretId;
			set
			{
				googleSdk.webClientSecretId = value;
				googleSdk.Save();
			}
		}

		private void GetAosClientKey() => AndroidClientKey = googleSdk.GetClientId(ClientType.AndroidClientId);

		private void GetIosClientKey() => IosClientKey = googleSdk.GetClientId(ClientType.IosClientId);

		private void GetWebClientKey() => WebClientKey = googleSdk.GetClientId(ClientType.WebClientId);
	}
}