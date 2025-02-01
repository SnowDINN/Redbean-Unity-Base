using Google;
using Sirenix.OdinInspector;

namespace Redbean.Editor
{
	internal partial class ThirdPartyWindow
	{
		private const string GoogleAuthTab = "Google Auth";
		private const string GoogleAuthGroup = "Tabs/Google Auth/Key settings";
		private const string GoogleAuthDesktopGroup = "Tabs/Google Auth/Editor Key settings";

		[TabGroup(TabGroup, GoogleAuthTab), TitleGroup(GoogleAuthGroup), LabelText("Android Key"), InlineButton(nameof(GetAosClientKey), "GET"), ShowInInspector]
		private string AndroidClientKey
		{
			get => googleAuth.androidClientId;
			set
			{
				googleAuth.androidClientId = value;
				googleAuth.Save();
			}
		}

		[TabGroup(TabGroup, GoogleAuthTab), TitleGroup(GoogleAuthGroup), LabelText("iOS Key"), InlineButton(nameof(GetIosClientKey), "GET"), ShowInInspector]
		private string IosClientKey
		{
			get => googleAuth.iosClientId;
			set
			{
				googleAuth.iosClientId = value;
				googleAuth.Save();
			}
		}

		[TabGroup(TabGroup, GoogleAuthTab), TitleGroup(GoogleAuthGroup), LabelText("Web Key"), InlineButton(nameof(GetWebClientKey), "GET"), ShowInInspector]
		private string WebClientKey
		{
			get => googleAuth.webClientId;
			set
			{
				googleAuth.webClientId = value;
				googleAuth.Save();
			}
		}
		
		[TabGroup(TabGroup, GoogleAuthTab), TitleGroup(GoogleAuthDesktopGroup), LabelText("Desktop Client Id"), InlineButton(nameof(GetWebClientKey), "GET"), ShowInInspector]
		private string DesktopClientId
		{
			get => googleAuth.desktopClientId;
			set
			{
				googleAuth.desktopClientId = value;
				googleAuth.Save();
			}
		}
		
		[TabGroup(TabGroup, GoogleAuthTab), TitleGroup(GoogleAuthDesktopGroup), LabelText("Desktop Client Secret"), InlineButton(nameof(GetWebClientKey), "GET"), ShowInInspector]
		private string DesktopClientSecret
		{
			get => googleAuth.desktopClientSecret;
			set
			{
				googleAuth.desktopClientSecret = value;
				googleAuth.Save();
			}
		}

		private void GetAosClientKey() => AndroidClientKey = googleAuth.GetClientId(ClientType.AndroidClientId);
		private void GetIosClientKey() => IosClientKey = googleAuth.GetClientId(ClientType.IosClientId);
		private void GetWebClientKey() => WebClientKey = googleAuth.GetClientId(ClientType.WebClientId);
	}
}