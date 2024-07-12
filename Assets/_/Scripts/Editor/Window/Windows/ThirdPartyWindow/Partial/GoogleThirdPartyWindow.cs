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
			get => googleAuth.androidClientId;
			set
			{
				googleAuth.androidClientId = value;
				googleAuth.Save();
			}
		}

		[TabGroup(TabGroup, GoogleTab), TitleGroup(ClientGroup), LabelText("iOS Key"), InlineButton(nameof(GetIosClientKey), "GET"), ShowInInspector]
		private string IosClientKey
		{
			get => googleAuth.iosClientId;
			set
			{
				googleAuth.iosClientId = value;
				googleAuth.Save();
			}
		}

		[TabGroup(TabGroup, GoogleTab), TitleGroup(ClientGroup), LabelText("Web Key"), InlineButton(nameof(GetWebClientKey), "GET"), ShowInInspector]
		private string WebClientKey
		{
			get => googleAuth.webClientId;
			set
			{
				googleAuth.webClientId = value;
				googleAuth.Save();
			}
		}

		[TabGroup(TabGroup, GoogleTab), TitleGroup(OnlyEditorGroup), LabelText("Web Secret"), ShowInInspector]
		private string WebSecretKey
		{
			get => googleAuth.webClientSecretId;
			set
			{
				googleAuth.webClientSecretId = value;
				googleAuth.Save();
			}
		}

		private void GetAosClientKey() => AndroidClientKey = googleAuth.GetClientId(ClientType.AndroidClientId);

		private void GetIosClientKey() => IosClientKey = googleAuth.GetClientId(ClientType.IosClientId);

		private void GetWebClientKey() => WebClientKey = googleAuth.GetClientId(ClientType.WebClientId);
	}
}